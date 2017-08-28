//*********************************************************************************************************************
// ViewModel.cs
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
//  07/18/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using FaultAlgorithms;
using TVA;
using TVA.IO;

namespace openFLEManager
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region [ Members ]

        // Constants
        private const string ConfigFile = "openFLE.exe.config";

        // Nested Types
        public class FaultAlgorithm
        {
            public string Assembly;
            public string TypeName;
            public string MethodName;

            public string FullMethodName
            {
                get
                {
                    return string.Format("{0}.{1}", TypeName, MethodName);
                }
            }
        }

        // Events

        public event PropertyChangedEventHandler PropertyChanged;

        // Fields

        private XDocument m_dataModel;
        private List<FaultAlgorithm> m_faultDetectionAlgorithms;
        private List<FaultAlgorithm> m_faultLocationAlgorithms;

        private string m_processDelay;
        private string m_dropFolder;
        private string m_resultsFolder;
        private string m_detectionAssembly;
        private string m_detectionAlgorithm;
        private string m_detectionParameters;
        private string m_locationAssembly;
        private string m_locationAlgorithm;
        private string m_locationParameters;
        private string m_lengthUnits;

        private ObservableCollection<string> m_faultDetectionAlgorithmNames;
        private ObservableCollection<string> m_faultLocationAlgorithmNames;
        private int m_selectedFaultDetectionAlgorithm;
        private int m_selectedFaultLocationAlgorithm;

        private bool m_ignoreNotify;

        #endregion

        #region [ Constructors ]

        public ViewModel()
        {
            PropertyChanged += (sender, args) => HandlePropertyChanged(args.PropertyName);
        }

        #endregion

        #region [ Properties ]

        public string ProcessDelay
        {
            get
            {
                return m_processDelay;
            }
            set
            {
                double processDelay;

                m_processDelay = value;
                OnPropertyChanged("ProcessDelay");

                if (!double.TryParse(value, out processDelay))
                    throw new InvalidOperationException("Process delay must be a number.");
            }
        }

        public string DropFolder
        {
            get
            {
                return m_dropFolder;
            }
            set
            {
                m_dropFolder = value;
                OnPropertyChanged("DropFolder");
            }
        }

        public string ResultsFolder
        {
            get
            {
                return m_resultsFolder;
            }
            set
            {
                m_resultsFolder = value;
                OnPropertyChanged("ResultsFolder");
            }
        }

        public string DetectionAssembly
        {
            get
            {
                return m_detectionAssembly;
            }
            set
            {
                m_detectionAssembly = value;
                OnPropertyChanged("DetectionAssembly");
            }
        }

        public string DetectionAlgorithm
        {
            get
            {
                return m_detectionAlgorithm;
            }
            set
            {
                m_detectionAlgorithm = value;
                OnPropertyChanged("DetectionAlgorithm");
            }
        }

        public string DetectionParameters
        {
            get
            {
                return m_detectionParameters;
            }
            set
            {
                m_detectionParameters = value;
                OnPropertyChanged("DetectionParameters");
            }
        }

        public string LocationAssembly
        {
            get
            {
                return m_locationAssembly;
            }
            set
            {
                m_locationAssembly = value;
                OnPropertyChanged("LocationAssembly");
            }
        }

        public string LocationAlgorithm
        {
            get
            {
                return m_locationAlgorithm;
            }
            set
            {
                m_locationAlgorithm = value;
                OnPropertyChanged("LocationAlgorithm");
            }
        }

        public string LocationParameters
        {
            get
            {
                return m_locationParameters;
            }
            set
            {
                m_locationParameters = value;
                OnPropertyChanged("LocationParameters");
            }
        }

        public string LengthUnits
        {
            get
            {
                return m_lengthUnits;
            }
            set
            {
                m_lengthUnits = value;
                OnPropertyChanged("LengthUnits");
            }
        }

        public ObservableCollection<string> FaultDetectionAlgorithmNames
        {
            get
            {
                return m_faultDetectionAlgorithmNames;
            }
            set
            {
                m_faultDetectionAlgorithmNames = value;
                OnPropertyChanged("FaultDetectionAlgorithmNames");
            }
        }

        public ObservableCollection<string> FaultLocationAlgorithmNames
        {
            get
            {
                return m_faultLocationAlgorithmNames;
            }
            set
            {
                m_faultLocationAlgorithmNames = value;
                OnPropertyChanged("FaultLocationAlgorithmNames");
            }
        }

        public int SelectedFaultDetectionAlgorithm
        {
            get
            {
                return m_selectedFaultDetectionAlgorithm;
            }
            set
            {
                m_selectedFaultDetectionAlgorithm = value;
                OnPropertyChanged("SelectedFaultDetectionAlgorithm");
            }
        }

        public int SelectedFaultLocationAlgorithm
        {
            get
            {
                return m_selectedFaultLocationAlgorithm;
            }
            set
            {
                m_selectedFaultLocationAlgorithm = value;
                OnPropertyChanged("SelectedFaultLocationAlgorithm");
            }
        }

        #endregion

        #region [ Methods ]

        public void Load()
        {
            // Set up data model
            m_dataModel = XDocument.Load(ConfigFile);
            m_faultDetectionAlgorithms = GetFaultDetectionAlgorithms();
            m_faultLocationAlgorithms = GetFaultLocationAlgorithms();

            // Set up view model
            ProcessDelay = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "ProcessDelay").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            DropFolder = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "DropFolder").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            ResultsFolder = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "ResultsFolder").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            DetectionAssembly = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "DetectionAssembly").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            DetectionAlgorithm = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "DetectionAlgorithm").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            DetectionParameters = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "DetectionParameters").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            LocationAssembly = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "LocationAssembly").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            LocationAlgorithm = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "LocationAlgorithm").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            LocationParameters = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "LocationParameters").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            LengthUnits = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "LengthUnits").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            FaultDetectionAlgorithmNames = new ObservableCollection<string>(m_faultDetectionAlgorithms.Select(algorithm => algorithm.MethodName));
            FaultLocationAlgorithmNames = new ObservableCollection<string>(m_faultLocationAlgorithms.Select(algorithm => algorithm.MethodName));
            SelectedFaultDetectionAlgorithm = -1;
            SelectedFaultLocationAlgorithm = -1;
        }

        public void Save()
        {
            bool saved = false;
            bool restartService = false;
            ServiceController controller = null;

            XElement systemSettings;
            XElement processDelaySetting;
            XElement dropFolderSetting;
            XElement resultsFolderSetting;
            XElement detectionAssemblySetting;
            XElement detectionAlgorithmSetting;
            XElement detectionParametersSetting;
            XElement locationAssemblySetting;
            XElement locationAlgorithmSetting;
            XElement locationParametersSetting;
            XElement lengthUnitsSetting;

            try
            {
                controller = new ServiceController("openFLE");

                if (controller.CanStop && controller.Status != ServiceControllerStatus.Stopped && controller.Status != ServiceControllerStatus.StopPending)
                {
                    controller.Stop();
                    controller.WaitForStatus(ServiceControllerStatus.Stopped);
                    restartService = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to stop service due to exception: {0} Service will not be restarted.", ex.Message.EnsureEnd('.')));
            }

            try
            {
                systemSettings =
                    (m_dataModel.Root ?? m_dataModel.GetOrAddElement("configuration"))
                    .GetOrAddElement("categorizedSettings")
                    .GetOrAddElement("systemSettings");

                processDelaySetting = systemSettings.GetOrAddSetting("ProcessDelay");
                dropFolderSetting = systemSettings.GetOrAddSetting("DropFolder");
                resultsFolderSetting = systemSettings.GetOrAddSetting("ResultsFolder");
                detectionAssemblySetting = systemSettings.GetOrAddSetting("DetectionAssembly");
                detectionAlgorithmSetting = systemSettings.GetOrAddSetting("DetectionAlgorithm");
                detectionParametersSetting = systemSettings.GetOrAddSetting("DetectionParameters");
                locationAssemblySetting = systemSettings.GetOrAddSetting("LocationAssembly");
                locationAlgorithmSetting = systemSettings.GetOrAddSetting("LocationAlgorithm");
                locationParametersSetting = systemSettings.GetOrAddSetting("LocationParameters");
                lengthUnitsSetting = systemSettings.GetOrAddSetting("LengthUnits");

                processDelaySetting.AddOrUpdateAttribute("value", ProcessDelay);
                dropFolderSetting.AddOrUpdateAttribute("value", DropFolder);
                resultsFolderSetting.AddOrUpdateAttribute("value", ResultsFolder);
                detectionAssemblySetting.AddOrUpdateAttribute("value", DetectionAssembly);
                detectionAlgorithmSetting.AddOrUpdateAttribute("value", DetectionAlgorithm);
                detectionParametersSetting.AddOrUpdateAttribute("value", DetectionParameters);
                locationAssemblySetting.AddOrUpdateAttribute("value", LocationAssembly);
                locationAlgorithmSetting.AddOrUpdateAttribute("value", LocationAlgorithm);
                locationParametersSetting.AddOrUpdateAttribute("value", LocationParameters);
                lengthUnitsSetting.AddOrUpdateAttribute("value", LengthUnits);

                m_dataModel.Save(ConfigFile);
                saved = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Configuration changes could not be saved due to exception: {0}", ex.Message));
            }

            try
            {
                if (restartService && controller.Status == ServiceControllerStatus.Stopped)
                {
                    controller.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to restart service due to exception: {0}", ex.Message));
            }

            if (saved)
            {
                MessageBox.Show("Configuration changes saved successfully.");
            }
        }

        public void LaunchConsoleMonitor()
        {
            const string processName = "openFLEConsole";
            Process consoleProcess;

            if (Process.GetProcessesByName(processName).Length <= 0)
            {
                consoleProcess = Process.Start(string.Format("{0}.exe", processName));

                if ((object)consoleProcess != null)
                    consoleProcess.Close();
            }
        }

        private List<FaultAlgorithm> GetFaultDetectionAlgorithms()
        {
            return GetFaultAlgorithms(typeof(FaultDetectionAlgorithmAttribute));
        }

        private List<FaultAlgorithm> GetFaultLocationAlgorithms()
        {
            return GetFaultAlgorithms(typeof(FaultLocationAlgorithmAttribute));
        }

        private List<FaultAlgorithm> GetFaultAlgorithms(Type attributeType)
        {
            string directoryPath = FilePath.GetAbsolutePath(".");
            List<FaultAlgorithm> algorithms = new List<FaultAlgorithm>();

            string[] assemblyFiles;
            Type[] types;
            MethodInfo[] methods;

            // Get the list of files which are assemblies in the application directory.
            assemblyFiles = FilePath.GetFileList(Path.Combine(directoryPath, "*.dll"))
                .Concat(FilePath.GetFileList(Path.Combine(directoryPath, "*.exe")))
                .ToArray();

            foreach (string assemblyFile in assemblyFiles)
            {
                try
                {
                    // Get the list of types in the current assembly.
                    types = Assembly.LoadFrom(assemblyFile).GetTypes();
                }
                catch
                {
                    // Cannot load assembly; set types to empty array.
                    types = new Type[0];
                }

                foreach (Type type in types)
                {
                    // Get the list of static methods annotated with the given attribute type.
                    methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                        .Where(method => method.GetCustomAttributes(false).Any(attribute => attribute.GetType() == attributeType))
                        .ToArray();

                    // Add the methods to the fault algorithms collection.
                    algorithms.AddRange(methods.Select(method => new FaultAlgorithm()
                    {
                        Assembly = FilePath.GetFileName(assemblyFile),
                        TypeName = type.FullName,
                        MethodName = method.Name
                    }));
                }
            }

            return algorithms;
        }

        private void HandlePropertyChanged(string propertyName)
        {
            if (!m_ignoreNotify)
            {
                m_ignoreNotify = true;
                HandlePropertyChangedOnce(propertyName);
                m_ignoreNotify = false;
            }
        }

        private void HandlePropertyChangedOnce(string propertyName)
        {
            FaultAlgorithm algorithm;

            switch (propertyName)
            {
                case "SelectedFaultDetectionAlgorithm":
                    if (m_selectedFaultDetectionAlgorithm >= 0)
                    {
                        algorithm = m_faultDetectionAlgorithms[m_selectedFaultDetectionAlgorithm];
                        DetectionAssembly = algorithm.Assembly;
                        DetectionAlgorithm = algorithm.FullMethodName;

                        Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => SelectedFaultDetectionAlgorithm = -1));
                    }
                    break;

                case "SelectedFaultLocationAlgorithm":
                    if (m_selectedFaultLocationAlgorithm >= 0)
                    {
                        algorithm = m_faultLocationAlgorithms[m_selectedFaultLocationAlgorithm];
                        LocationAssembly = algorithm.Assembly;
                        LocationAlgorithm = algorithm.FullMethodName;

                        Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => SelectedFaultLocationAlgorithm = -1));
                    }
                    break;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if ((object)PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    internal static class XmlExtensions
    {
        public static XElement GetOrAddElement(this XContainer container, string elementName)
        {
            XElement element = container.Element(elementName);

            if ((object)element == null)
            {
                element = new XElement(elementName);
                container.Add(element);
            }

            return element;
        }

        public static XElement GetOrAddSetting(this XContainer container, string settingName)
        {
            XElement setting = container.Elements().FirstOrDefault(element => settingName == (string)element.Attribute("name"));

            if ((object)setting == null)
            {
                setting = new XElement("add",
                    new XAttribute("name", settingName),
                    new XAttribute("value", string.Empty),
                    new XAttribute("description", string.Empty),
                    new XAttribute("encrypted", "False")
                );

                container.Add(setting);
            }

            return setting;
        }

        public static void AddOrUpdateAttribute(this XElement element, string name, string value)
        {
            XAttribute attribute = element.Attribute(name);

            if ((object)attribute != null)
            {
                attribute.Value = value;
            }
            else
            {
                attribute = new XAttribute(name, value);
                element.Add(attribute);
            }
        }
    }
}
