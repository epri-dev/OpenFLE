//*********************************************************************************************************************
// LogicalParser.cs
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
//*********************************************************************************************************************
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  05/03/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;
using openPQDIF.Physical;

namespace openPQDIF.Logical
{
    /// <summary>
    /// Represents a parser which parses the logical structure of a PQDIF file.
    /// </summary>
    public class LogicalParser : IDisposable
    {
        #region [ Members ]

        // Fields
        private PhysicalParser m_physicalParser;
        private ContainerRecord m_containerRecord;
        private DataSourceRecord m_currentDataSourceRecord;
        private MonitorSettingsRecord m_currentMonitorSettingsRecord;
        private ObservationRecord m_nextObservationRecord;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="LogicalParser"/> class.
        /// </summary>
        /// <param name="fileName">Name of the PQDIF file to be parsed.</param>
        public LogicalParser(string fileName)
        {
            m_physicalParser = new PhysicalParser(fileName);
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the container record from the PQDIF file. This is
        /// parsed as soon as the parser is <see cref="Open"/>ed.
        /// </summary>
        public ContainerRecord ContainerRecord
        {
            get
            {
                return m_containerRecord;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Opens the parser and parses the <see cref="ContainerRecord"/>.
        /// </summary>
        public void Open()
        {
            m_physicalParser.Open();
            m_containerRecord = ContainerRecord.CreateContainerRecord(m_physicalParser.NextRecord());
            m_physicalParser.CompressionAlgorithm = m_containerRecord.CompressionAlgorithm;
            m_physicalParser.CompressionStyle = m_containerRecord.CompressionStyle;
        }

        /// <summary>
        /// Determines whether there are any more
        /// <see cref="ObservationRecord"/>s to be
        /// read from the PQDIF file.
        /// </summary>
        /// <returns>true if there is another observation record to be read from PQDIF file; false otherwise</returns>
        public bool HasNextObservationRecord()
        {
            Record physicalRecord;
            RecordType recordType;

            // Read records from the file until we encounter an observation record or end of file
            while ((object)m_nextObservationRecord == null && m_physicalParser.HasNextRecord())
            {
                physicalRecord = m_physicalParser.NextRecord();
                recordType = physicalRecord.Header.TypeOfRecord;

                switch (recordType)
                {
                    case RecordType.DataSource:
                        // Keep track of the latest data source record in order to associate it with observation records
                        m_currentDataSourceRecord = DataSourceRecord.CreateDataSourceRecord(physicalRecord);
                        break;

                    case RecordType.MonitorSettings:
                        // Keep track of the latest monitor settings record in order to associate it with observation records
                        m_currentMonitorSettingsRecord = MonitorSettingsRecord.CreateMonitorSettingsRecord(physicalRecord);
                        break;

                    case RecordType.Observation:
                        // Found an observation record!
                        m_nextObservationRecord = ObservationRecord.CreateObservationRecord(physicalRecord, m_currentDataSourceRecord, m_currentMonitorSettingsRecord);
                        break;

                    case RecordType.Container:
                        // The container record is parsed when the file is opened; it should never be encountered here
                        throw new InvalidOperationException("Found more than one container record in PQDIF file.");

                    default:
                        // Ignore unrecognized record types as the rest of the file might be valid.
                        //throw new ArgumentOutOfRangeException(string.Format("Unknown record type: {0}", physicalRecord.Header.RecordTypeTag));
                        break;
                }
            }

            return (object)m_nextObservationRecord != null;
        }

        /// <summary>
        /// Gets the next observation record from the PQDIF file.
        /// </summary>
        /// <returns>The next observation record.</returns>
        public ObservationRecord NextObservationRecord()
        {
            ObservationRecord nextObservationRecord;

            // Call this first to read ahead to the next
            // observation record if we haven't already
            HasNextObservationRecord();

            // We need to set m_nextObservationRecord to null so that
            // subsequent calls to HasNextObservationRecord() will
            // continue to parse new records
            nextObservationRecord = m_nextObservationRecord;
            m_nextObservationRecord = null;

            return nextObservationRecord;
        }

        /// <summary>
        /// Resets the parser to the beginning of the PQDIF file.
        /// </summary>
        public void Reset()
        {
            m_currentDataSourceRecord = null;
            m_currentMonitorSettingsRecord = null;
            m_nextObservationRecord = null;

            m_physicalParser.Reset();
            m_physicalParser.NextRecord(); // skip container record
        }

        /// <summary>
        /// Closes the PQDIF file.
        /// </summary>
        public void Close()
        {
            m_physicalParser.Close();
        }

        /// <summary>
        /// Releases resources held by the parser.
        /// </summary>
        public void Dispose()
        {
            m_physicalParser.Dispose();
        }

        #endregion
    }
}
