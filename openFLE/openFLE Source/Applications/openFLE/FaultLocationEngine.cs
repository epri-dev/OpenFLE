//*********************************************************************************************************************
// FaultLocationEngine.cs
//
// Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC. All rights reserved.
//
// openFLE ("this software") is licensed under BSD 3-Clause license.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
// following conditions are met:
//
// •    Redistributions of source code must retain the above copyright  notice, this list of conditions and 
//      the following disclaimer.
//
// •    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//
// •    Neither the name of the Electric Power Research Institute, Inc. (“EPRI”) nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
// DISCLAIMED. IN NO EVENT SHALL EPRI BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
// OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
//
//
// This software incorporates work covered by the following copyright and permission notice: 
//
// •    TVA Code Library 4.0.4.3 - Tennessee Valley Authority, tvainfo@tva.gov
//      No copyright is claimed pursuant to 17 USC § 105. All Other Rights Reserved.
//
//      Licensed under TVA Custom License based on NASA Open Source Agreement (TVA Custom NOSA); 
//      you may not use TVA Code Library except in compliance with the TVA Custom NOSA. You may  
//      obtain a copy of the TVA Custom NOSA at http://tvacodelibrary.codeplex.com/license.
//
//      TVA Code Library is provided by the copyright holders and contributors "as is" and any express 
//      or implied warranties, including, but not limited to, the implied warranties of merchantability 
//      and fitness for a particular purpose are disclaimed.
//
//*********************************************************************************************************************
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  05/16/2012 - J. Ritchie Carroll, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using FaultAlgorithms;
using TVA;
using TVA.Configuration;
using TVA.IO;
using TVA.Units;

namespace openFLE
{
    /// <summary>
    /// Represents an engine that processes power quality data
    /// to determine the locations of faults along power lines.
    /// </summary>
    public class FaultLocationEngine : IDisposable
    {
        #region [ Members ]

        // Events

        /// <summary>
        /// Triggered when a message concerning the status
        /// of the fault location engine is encountered.
        /// </summary>
        public event EventHandler<EventArgs<string>> StatusMessage;

        /// <summary>
        /// Triggered when an exception is handled by the fault location engine.
        /// </summary>
        public event EventHandler<EventArgs<Exception>> ProcessException;

        // Fields

        private Dictionary<string, Tuple<FaultDetectionAlgorithm, string>> m_faultDetectionAlgorithms;
        private Dictionary<string, Tuple<FaultLocationAlgorithm, string>> m_faultLocationAlgorithms;
        private string m_defaultFaultDetectionAlgorithm;
        private string m_defaultFaultLocationAlgorithm;

        private Dictionary<string, DateTime> m_fileCreationTimes = new Dictionary<string, DateTime>();
        private System.Timers.Timer m_fileMonitor;
        private double m_processDelay;
        private string m_dropFolder;
        private string m_resultsFolder;
        private string[] m_fileExtensionFilter = { "*.pqd", "*.d00", "*.dat" };

        private string m_lengthUnits;

        private Logger m_currentLogger;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Finalizer for <see cref="FaultLocationEngine"/> class.
        /// </summary>
        ~FaultLocationEngine()
        {
            Dispose();
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Starts the fault location engine.
        /// </summary>
        public void Start()
        {
            // Make sure default service settings exist
            ConfigurationFile configFile = ConfigurationFile.Current;

            // System settings
            // TODO: Add description to system settings
            CategorizedSettingsElementCollection systemSettings = configFile.Settings["systemSettings"];
            systemSettings.Add("ProcessDelay", "15", "");
            systemSettings.Add("DropFolder", "Drop", "");
            systemSettings.Add("ResultsFolder", "Results", "");
            systemSettings.Add("LengthUnits", "Miles", "");

            // Retrieve file paths as defined in the config file
            m_processDelay = systemSettings["ProcessDelay"].ValueAs(m_processDelay);
            m_dropFolder = FilePath.AddPathSuffix(FilePath.GetAbsolutePath(systemSettings["DropFolder"].Value));
            m_resultsFolder = FilePath.AddPathSuffix(FilePath.GetAbsolutePath(systemSettings["ResultsFolder"].Value));
            m_lengthUnits = systemSettings["LengthUnits"].Value;

            // Make sure file path directories exist
            try
            {
                if (!Directory.Exists(m_dropFolder))
                    Directory.CreateDirectory(m_dropFolder);

                if (!Directory.Exists(m_resultsFolder))
                    Directory.CreateDirectory(m_resultsFolder);
            }
            catch (Exception ex)
            {
                OnProcessException(new InvalidOperationException(string.Format("Failed to create directory due to exception: {0}", ex.Message), ex));
            }

            // Setup new simple file monitor - we do this since the .NET 4.0 FileWatcher has a bad memory leak :-(
            if ((object)m_fileMonitor == null)
            {
                m_fileMonitor = new System.Timers.Timer();
                m_fileMonitor.Interval = 1000;
                m_fileMonitor.AutoReset = false;
                m_fileMonitor.Elapsed += FileMonitor_Elapsed;
            }

            // Load all defined fault related algorithms
            LoadDefaultFaultAlgorithms();

            if (m_faultDetectionAlgorithms.Count > 0 && m_faultLocationAlgorithms.Count > 0)
            {
                // Start the file processing monitor
                m_fileMonitor.Start();
            }
            else
            {
                OnProcessException(new InvalidOperationException("Cannot proceed without at least one fault detection and fault location algorithm defined."));
            }
        }

        /// <summary>
        /// Stops the fault location engine.
        /// </summary>
        public void Stop()
        {
            if ((object)m_fileMonitor != null)
                m_fileMonitor.Stop();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            // Stop file monitor timer
            if ((object)m_fileMonitor != null)
            {
                m_fileMonitor.Elapsed -= FileMonitor_Elapsed;
                m_fileMonitor.Dispose();
            }
            m_fileMonitor = null;
        }

        private void FileMonitor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Queue files from the unprocessed file directory that match the desired file patterns
            foreach (string fileName in FilePath.GetFileList(m_dropFolder))
            {
                if (FilePath.IsFilePatternMatch(m_fileExtensionFilter, FilePath.GetFileName(fileName), true))
                {
                    if (CanProcessFile(fileName))
                    {
                        ProcessFile(fileName);
                        m_fileCreationTimes.Remove(fileName);
                    }
                }
            }

            if ((object)m_fileMonitor != null)
                m_fileMonitor.Start();
        }

        private bool CanProcessFile(string fileName)
        {
            string rootFileName;
            string extension;

            string eventFileName;
            string cfgFileName;
            TimeSpan timeSinceCreation;

            if ((object)fileName == null || !File.Exists(fileName))
                return false;

            rootFileName = FilePath.GetFileNameWithoutExtension(fileName);
            extension = FilePath.GetExtension(fileName).ToLowerInvariant().Trim();
            eventFileName = Path.Combine(m_dropFolder, rootFileName + "_eventFileDefinition.xml");
            cfgFileName = Path.Combine(m_dropFolder, rootFileName + ".cfg");
            timeSinceCreation = DateTime.Now - GetFileCreationTime(fileName);

            return File.Exists(eventFileName)
                && (extension == ".pqd" || File.Exists(cfgFileName))
                && (extension != ".d00" || timeSinceCreation.TotalSeconds >= m_processDelay);
        }

        private void ProcessFile(string fileName)
        {
            string rootFileName = null;
            string extension = null;
            m_currentLogger = null;

            try
            {
                rootFileName = FilePath.GetFileNameWithoutExtension(fileName);
                extension = FilePath.GetExtension(fileName).ToLowerInvariant().Trim();
                m_currentLogger = Logger.Open(string.Format("{0}{1}_openFLElog.txt", m_dropFolder, rootFileName));

                OnStatusMessage(string.Format("Processing {0}...", fileName));

                StringBuilder parameters = new StringBuilder();

                // TODO: Load other needed specific associated parameters based on file / line information once all meta-data is known and defined
                parameters.AppendFormat("fileName={0}; ", fileName);
                parameters.Append("dataSourceType=");

                // Process files based on file extension
                switch (extension)
                {
                    case ".pqd":
                        parameters.Append("PQDIF");
                        break;
                    case ".d00":
                    case ".dat":
                        parameters.Append("Comtrade");
                        break;
                    default:
                        OnProcessException(new InvalidOperationException(string.Format("Unknown file extension encountered: \"{0}\" - cannot parse file.", extension)));
                        return;
                }

                // Load the associated event file definition
                string eventFileDefinitionFileName = Path.Combine(m_dropFolder, rootFileName + "_EventFileDefinition.xml");

                // Make sure event file definitions exists...
                if (!File.Exists(eventFileDefinitionFileName))
                    throw new FileNotFoundException(string.Format("Event file definition \"{0}\" was not found.", eventFileDefinitionFileName));

                XDocument eventFileDefinition = XDocument.Load(eventFileDefinitionFileName);
                XElement line = eventFileDefinition.Root.Element("EventFile").Element("Line");
                XElement impedance = line.Element("LineImpedance");
                double r1 = double.Parse(impedance.Element("R1").Value);
                double x1 = double.Parse(impedance.Element("X1").Value);
                double r0 = double.Parse(impedance.Element("R0").Value);
                double x0 = double.Parse(impedance.Element("X0").Value);

                // Load the fault data set based on provided parameters
                MeasurementDataSet voltageMeasurementDataSet = new MeasurementDataSet();
                MeasurementDataSet currentMeasurementDataSet = new MeasurementDataSet();
                CycleDataSet cycleDataSet;

                FaultLocationDataSet faultDataSet;

                // Load data sets based on specified parameters
                LoadDataSets(voltageMeasurementDataSet, currentMeasurementDataSet, parameters.ToString(), eventFileDefinition);
                cycleDataSet = new CycleDataSet(voltageMeasurementDataSet, currentMeasurementDataSet, GetSampleRate(voltageMeasurementDataSet.AN.Times));
                faultDataSet = new FaultLocationDataSet(voltageMeasurementDataSet, currentMeasurementDataSet, cycleDataSet)
                {
                    PositiveImpedance = new ComplexNumber(r1, x1),
                    ZeroImpedance = new ComplexNumber(r0, x0),
                    LineDistance = double.Parse(line.Element("LineLength").Value)
                };

                // Export data to CSV for validation
                MeasurementDataSet.ExportToCSV(FilePath.GetAbsolutePath(string.Format("{0}{1}_measurementData.csv", m_dropFolder, rootFileName)), voltageMeasurementDataSet, currentMeasurementDataSet);
                CycleDataSet.ExportToCSV(FilePath.GetAbsolutePath(string.Format("{0}{1}_cycleData.csv", m_dropFolder, rootFileName)), cycleDataSet);

                // TODO: Load specific algorithms per file once event file definition is parsed
                string faultDetectionAlgorithmName = m_defaultFaultDetectionAlgorithm;
                string faultLocationAlgorithmName = m_defaultFaultLocationAlgorithm;

                FaultDetectionAlgorithm faultDetectionAlgorithm;
                FaultLocationAlgorithm faultLocationAlgorithm;

                string faultDetectionAlgorithmParameters;
                string faultlocationAlgorithmParameters;

                // Lookup fault detection algorithm and its associated parameters
                Tuple<FaultDetectionAlgorithm, string> faultDetectionAlgorithmItems;

                if (m_faultDetectionAlgorithms.TryGetValue(faultDetectionAlgorithmName, out faultDetectionAlgorithmItems))
                {
                    faultDetectionAlgorithm = faultDetectionAlgorithmItems.Item1;
                    faultDetectionAlgorithmParameters = faultDetectionAlgorithmItems.Item2;
                }
                else
                {
                    // Cannot proceed without a valid fault detection algorithm
                    throw new InvalidOperationException(string.Format("Fault detection algorithm \"{0}\" does not exist - fault calculations will not continue.", faultDetectionAlgorithmName));
                }

                // Lookup fault location algorithm and its associated parameters
                Tuple<FaultLocationAlgorithm, string> faultLocationAlgorithmItems;

                if (m_faultLocationAlgorithms.TryGetValue(faultLocationAlgorithmName, out faultLocationAlgorithmItems))
                {
                    faultLocationAlgorithm = faultLocationAlgorithmItems.Item1;
                    faultlocationAlgorithmParameters = faultLocationAlgorithmItems.Item2;
                }
                else
                {
                    // Cannot proceed without a valid fault location algorithm
                    throw new InvalidOperationException(string.Format("Fault location algorithm \"{0}\" does not exist - fault calculations will not continue.", faultLocationAlgorithmName));
                }

                // Attempt to execute fault detection algorithms for each phase
                ExecuteFaultDetectionAlgorithm(faultDataSet, faultDetectionAlgorithmName, faultDetectionAlgorithm, faultDetectionAlgorithmParameters);

                // Attempt to execute fault location algorithm
                ExecuteFaultLocationAlgorithm(faultDataSet, faultLocationAlgorithmName, faultLocationAlgorithm, faultlocationAlgorithmParameters);

                // Write results to results file
                string resultsFileName = string.Format("{0}{1}_openFLEresults.xml", m_dropFolder, rootFileName);
                CreateResultsFile(resultsFileName, eventFileDefinition, faultDataSet);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Unable to process file \"{0}\" due to exception: {1}", fileName, ex.Message);
                OnProcessException(new InvalidOperationException(errorMessage, ex));
            }

            try
            {
                if ((object)m_currentLogger != null)
                {
                    m_currentLogger.Close();
                    m_currentLogger = null;
                }
            }
            catch (Exception ex)
            {
                OnProcessException(ex);
            }

            try
            {
                if ((object)rootFileName != null)
                {
                    int destinationFileIncrement = 2;

                    List<string> sourceFiles = FilePath.GetFileList(string.Format("{0}{1}.*", m_dropFolder, rootFileName))
                        .Concat(FilePath.GetFileList(string.Format("{0}{1}_*", m_dropFolder, rootFileName)))
                        .ToList();

                    List<string> destinationFiles = sourceFiles.Select(FilePath.GetFileName)
                        .Select(destinationFileName => Path.Combine(m_resultsFolder, destinationFileName))
                        .ToList();

                    while (destinationFiles.Any(File.Exists))
                    {
                        destinationFiles = sourceFiles.Select(FilePath.GetFileName)
                            .Select(sourceFileName => sourceFileName.Insert(rootFileName.Length, string.Format(" ({0})", destinationFileIncrement)))
                            .Select(destinationFileName => Path.Combine(m_resultsFolder, destinationFileName))
                            .ToList();

                        destinationFileIncrement++;
                    }

                    for (int i = 0; i < sourceFiles.Count && i < destinationFiles.Count; i++)
                    {
                        // Move the file to the processed files directory
                        File.Move(sourceFiles[i], destinationFiles[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                OnProcessException(ex);
            }

            OnStatusMessage("");
        }

        // Execute fault detection algorithm and log results
        private void ExecuteFaultDetectionAlgorithm(FaultLocationDataSet faultDataSet, string algorithmName, FaultDetectionAlgorithm algorithm, string parameters)
        {
            try
            {
                if ((object)faultDataSet == null)
                    throw new ArgumentNullException("faultDataSet");

                // Execute fault detection algorithm
                bool faultFound = algorithm(faultDataSet, parameters);

                if (faultFound)
                    OnStatusMessage("Fault detected!");
                else
                    OnStatusMessage("No fault detected...");
            }
            catch (Exception ex)
            {
                OnProcessException(new InvalidOperationException(string.Format("Failed while executing fault detection algorithm \"{0} [{1}]\" due to exception: {2}", algorithmName, parameters, ex.Message), ex));
            }
        }

        // Execute fault location algorithm and log results
        private void ExecuteFaultLocationAlgorithm(FaultLocationDataSet faultDataSet, string algorithmName, FaultLocationAlgorithm algorithm, string parameters)
        {
            try
            {
                if ((object)faultDataSet == null)
                    throw new ArgumentNullException("faultDataSet");

                // TODO: Definition of algorithm results could be over simplified - is it possible to have three separate line distances for each phase?

                // Execute fault location algorithm
                double lineDistance = algorithm(faultDataSet, parameters);
                OnStatusMessage("Calculated distance of fault down the line: {0} {1}", lineDistance, m_lengthUnits);
            }
            catch (Exception ex)
            {
                OnProcessException(new InvalidOperationException(string.Format("Failed while executing fault location algorithm \"{0} [{1}]\" due to exception: {2}", algorithmName, parameters, ex.Message), ex));
            }
        }

        // Loads fault related algorithms.
        private void LoadDefaultFaultAlgorithms()
        {
            ConfigurationFile config = ConfigurationFile.Current;
            CategorizedSettingsElementCollection settings = config.Settings["systemSettings"];

            string detectionAssemblyName = "openFLE.exe";
            string detectionAlgorithmName = "openFLE.FaultAlgorithms.SimpleFaultDetectionAlgorithm";
            string detectionParameters = "";

            string locationAssemblyName = "openFLE.exe";
            string locationAlgorithmName = "openFLE.FaultAlgorithms.SimpleFaultLocationAlgorithm";
            string locationParameters = "";

            FaultDetectionAlgorithm detectionAlgorithm;
            FaultLocationAlgorithm locationAlgorithm;

            // TODO: Add descriptions to config file settings
            settings.Add("DetectionAssembly", detectionAssemblyName, "");
            settings.Add("DetectionAlgorithm", detectionAlgorithmName, "");
            settings.Add("DetectionParameters", detectionParameters, "");
            settings.Add("LocationAssembly", locationAssemblyName, "");
            settings.Add("LocationAlgorithm", locationAlgorithmName, "");
            settings.Add("LocationParameters", locationParameters, "");
            detectionAssemblyName = FilePath.GetAbsolutePath(settings["DetectionAssembly"].Value);
            detectionAlgorithmName = settings["DetectionAlgorithm"].Value;
            detectionParameters = settings["DetectionParameters"].Value;
            locationAssemblyName = FilePath.GetAbsolutePath(settings["LocationAssembly"].Value);
            locationAlgorithmName = settings["LocationAlgorithm"].Value;
            locationParameters = settings["LocationParameters"].Value;

            m_faultDetectionAlgorithms = new Dictionary<string, Tuple<FaultDetectionAlgorithm, string>>(StringComparer.InvariantCultureIgnoreCase);
            m_faultLocationAlgorithms = new Dictionary<string, Tuple<FaultLocationAlgorithm, string>>(StringComparer.InvariantCultureIgnoreCase);

            detectionAlgorithm = LoadAlgorithm<FaultDetectionAlgorithm>(detectionAssemblyName, detectionAlgorithmName);
            locationAlgorithm = LoadAlgorithm<FaultLocationAlgorithm>(locationAssemblyName, locationAlgorithmName);

            if ((object)detectionAlgorithm != null && (object)locationAlgorithm != null)
            {
                m_faultDetectionAlgorithms.Add(detectionAlgorithmName, new Tuple<FaultDetectionAlgorithm, string>(detectionAlgorithm, detectionParameters));
                m_faultLocationAlgorithms.Add(locationAlgorithmName, new Tuple<FaultLocationAlgorithm, string>(locationAlgorithm, locationParameters));
                m_defaultFaultDetectionAlgorithm = detectionAlgorithmName;
                m_defaultFaultLocationAlgorithm = locationAlgorithmName;
            }
        }

        private T LoadAlgorithm<T>(string assemblyName, string algorithmName) where T : class
        {
            int index;
            string typeName;
            string methodName;

            Assembly assembly;
            Type type;
            MethodInfo method;

            try
            {
                index = algorithmName.LastIndexOf('.');
                typeName = algorithmName.Substring(0, index);
                methodName = algorithmName.Substring(index + 1);

                assembly = Assembly.LoadFrom(assemblyName);
                type = assembly.GetType(typeName);
                method = type.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.InvokeMethod);

                return Delegate.CreateDelegate(typeof(T), method) as T;
            }
            catch (Exception ex)
            {
                OnProcessException(new InvalidOperationException(string.Format("Failed while loading {0} due to exception: {1}", typeof(T).Name, ex.Message), ex));
            }

            return null;
        }

        private void CreateResultsFile(string fileName, XDocument eventFileDefinition, FaultLocationDataSet faultDataSet)
        {
            const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

            long[] times = faultDataSet.Voltages.AN.Times;
            int firstFaultCycleIndex = faultDataSet.FaultedCycles.First();
            CycleData firstFaultCycle = faultDataSet.Cycles[firstFaultCycleIndex];
            DateTime firstFaultCycleTime = new DateTime(times[firstFaultCycle.StartIndex]);

            CycleData faultCalculationCycle = faultDataSet.Cycles[faultDataSet.FaultCalculationCycle];

            XElement root =
                new XElement("openFLE",
                    new XElement("Results",
                        new XElement("MeterName", eventFileDefinition.Root.Descendants("MeterName").First().Value),
                        new XElement("LineName", eventFileDefinition.Root.Descendants("LineName").First().Value),
                        new XElement("MeterStationName", eventFileDefinition.Root.Descendants("MeterStationName").First().Value),
                        new XElement("LineLength", faultDataSet.LineDistance),
                        new XElement("FaultType", faultDataSet.FaultType.ToString()),
                        new XElement("FaultDistance", faultDataSet.FaultDistance),
                        new XElement("CyclesOfData", faultDataSet.Cycles.Count),
                        new XElement("FaultCycles", faultDataSet.FaultCycleCount),
                        new XElement("FaultCalculationCycle", faultDataSet.FaultCalculationCycle),
                        new XElement("FirstFaultCycleTime", firstFaultCycleTime.ToString(DateTimeFormat)),
                        new XElement("IAfault", faultCalculationCycle.AN.I.RMS),
                        new XElement("IBfault", faultCalculationCycle.BN.I.RMS),
                        new XElement("ICfault", faultCalculationCycle.CN.I.RMS),
                        new XElement("VAfault", faultCalculationCycle.AN.V.RMS),
                        new XElement("VBfault", faultCalculationCycle.BN.V.RMS),
                        new XElement("VCfault", faultCalculationCycle.CN.V.RMS),
                        new XElement("IAmax", faultDataSet.Cycles.Max(cycle => cycle.AN.I.RMS)),
                        new XElement("IBmax", faultDataSet.Cycles.Max(cycle => cycle.BN.I.RMS)),
                        new XElement("ICmax", faultDataSet.Cycles.Max(cycle => cycle.CN.I.RMS)),
                        new XElement("VAmin", faultDataSet.Cycles.Min(cycle => cycle.AN.V.RMS)),
                        new XElement("VBmin", faultDataSet.Cycles.Min(cycle => cycle.BN.V.RMS)),
                        new XElement("VCmin", faultDataSet.Cycles.Min(cycle => cycle.CN.V.RMS))
                    )
                );

            XDocument results = new XDocument(root);

            results.Save(fileName);
        }

        // Displays status message to the console - proxy method for service implementation
        private void OnStatusMessage(string format, params object[] args)
        {
            string message = string.Format(format, args);

            if ((object)m_currentLogger != null)
                m_currentLogger.WriteLine(message);

            if ((object)StatusMessage != null)
                StatusMessage(this, new EventArgs<string>(message));
        }

        // Displays exception message to the console - proxy method for service implmentation
        private void OnProcessException(Exception ex)
        {
            if ((object)m_currentLogger != null)
                m_currentLogger.WriteLine(string.Format("ERROR: {0}", ex.Message));

            if ((object)ProcessException != null)
                ProcessException(this, new EventArgs<Exception>(ex));
        }

        // Gets the creation time of the file with the given file name.
        private DateTime GetFileCreationTime(string fileName)
        {
            DateTime creationTime;

            if (!m_fileCreationTimes.TryGetValue(fileName, out creationTime))
            {
                creationTime = DateTime.Now;
                m_fileCreationTimes.Add(fileName, creationTime);
            }

            return creationTime;
        }

        #endregion

        #region [ Static ]

        // Static Methods

        // Load voltage and current data set based on connection string parameters
        private static void LoadDataSets(MeasurementDataSet voltageDataSet, MeasurementDataSet currentDataSet, string parameters, XDocument eventFileDefinition)
        {
            if (string.IsNullOrEmpty(parameters))
                throw new ArgumentNullException("parameters");

            Dictionary<string, string> settings = parameters.ParseKeyValuePairs();
            string dataSourceType;

            if (!settings.TryGetValue("dataSourceType", out dataSourceType) || string.IsNullOrWhiteSpace(dataSourceType))
                throw new ArgumentException("Parameters must define a \"dataSourceType\" setting.");

            switch (dataSourceType.ToLowerInvariant().Trim())
            {
                case "pqdif":
                    PQDIFLoader.PopulateDataSets(settings, voltageDataSet, currentDataSet);
                    break;
                case "comtrade":
                    ComtradeLoader.PopulateDataSets(settings, voltageDataSet, currentDataSet, eventFileDefinition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("parameters", string.Format("Cannot parse \"{0}\" data source type - format is undefined.", dataSourceType));
            }

            // TODO: Determine if this is a valid assumption
            if ((object)voltageDataSet.AN.Values == null || voltageDataSet.AN.Values.Length == 0 || (object)voltageDataSet.BN.Values == null || voltageDataSet.BN.Values.Length == 0 || (object)voltageDataSet.CN.Values == null || voltageDataSet.CN.Values.Length == 0)
                throw new InvalidOperationException("Cannot calculate fault location without line-to-neutral values.");
        }

        private static int GetSampleRate(long[] times)
        {
            int[] knownSampleRates = { 96, 128, 256 };

            long startTime;
            long cycleTime;
            int samples;
            int min;

            // If there are no times in the array, default to 128
            if ((object)times == null || times.Length <= 0)
                return 128;

            // Assume 60 Hz and get a full cycle
            startTime = times[0];
            cycleTime = Ticks.PerSecond / 60L;
            samples = times.TakeWhile(time => time - startTime < cycleTime).Count();

            // Return the known sample rate closest to the 60 Hz sample rate
            min = knownSampleRates.Min(sampleRate => Math.Abs(sampleRate - samples));
            return knownSampleRates.Single(sampleRate => Math.Abs(sampleRate - samples) == min);
        }

        #endregion
    }
}
