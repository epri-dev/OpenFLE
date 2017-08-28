//*********************************************************************************************************************
// MonitorSettingsRecord.cs
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
    /// Represents a monitor settings record in a PQDIF file.
    /// </summary>
    public class MonitorSettingsRecord
    {
        #region [ Members ]

        // Fields
        private Record m_physicalRecord;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="MonitorSettingsRecord"/> class.
        /// </summary>
        /// <param name="physicalRecord">The physical structure of the monitor settings record.</param>
        private MonitorSettingsRecord(Record physicalRecord)
        {
            m_physicalRecord = physicalRecord;
        }

        #endregion

        #region [ Properties ]

        public double NominalFrequency
        {
            get
            {
                ScalarElement nominalFrequencyElement = m_physicalRecord.Body.Collection.GetScalarByTag(NominalFrequencyTag);

                if ((object)nominalFrequencyElement == null)
                    return 60.0D;

                return nominalFrequencyElement.GetReal8();
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>
        /// Tag that identifies the time that these settings become effective.
        /// </summary>
        public static readonly Guid EffectiveTag = new Guid("62f28183-f9c4-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the install time.
        /// </summary>
        public static readonly Guid TimeInstalledTag = new Guid("3d786f85-f76e-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the flag which determines whether to apply calibration to the series.
        /// </summary>
        public static readonly Guid UseCalibrationTag = new Guid("62f28180-f9c4-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the flag which determines whether to apply transducer adjustments to the series.
        /// </summary>
        public static readonly Guid UseTransducerTag = new Guid("62f28181-f9c4-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the collection of channel settings.
        /// </summary>
        public static readonly Guid ChannelSettingsArrayTag = new Guid("62f28182-f9c4-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies one channel setting in the collection.
        /// </summary>
        public static readonly Guid OneChannelSettingTag = new Guid("3d786f9a-f76e-11cf-9d89-0080c72e70a3");

        public static readonly Guid NominalFrequencyTag = new Guid("0fa118c3-cb4a-11d2-b30b-fe25cb9a1760");

        // Static Methods

        /// <summary>
        /// Creates a new monitor settings record from the given physical record
        /// if the physical record is of type monitor settings. Returns null if
        /// it is not.
        /// </summary>
        /// <param name="physicalRecord">The physical record used to create the monitor settings record.</param>
        /// <returns>The new monitor settings record, or null if the physical record does not define a monitor settings record.</returns>
        public static MonitorSettingsRecord CreateMonitorSettingsRecord(Record physicalRecord)
        {
            bool isValidMonitorSettingsRecord = physicalRecord.Header.TypeOfRecord == RecordType.MonitorSettings;
            return isValidMonitorSettingsRecord ?  new MonitorSettingsRecord(physicalRecord) : null;
        }

        #endregion
        
    }
}
