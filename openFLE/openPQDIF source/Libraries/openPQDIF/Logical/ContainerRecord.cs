//*********************************************************************************************************************
// ContainerRecord.cs
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
using System.Text;
using openPQDIF.Physical;

namespace openPQDIF.Logical
{
    /// <summary>
    /// Represents the container record in a PQDIF file. There can be only
    /// one container record in a PQDIF file, and it is the first physical
    /// <see cref="Record"/>.
    /// </summary>
    public class ContainerRecord
    {
        #region [ Members ]

        // Fields
        private Record m_physicalRecord;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="ContainerRecord"/> class.
        /// </summary>
        /// <param name="physicalRecord">The physical structure of the container record.</param>
        private ContainerRecord(Record physicalRecord)
        {
            m_physicalRecord = physicalRecord;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the major version number of the PQDIF file writer.
        /// </summary>
        public uint WriterMajorVersion
        {
            get
            {
                return m_physicalRecord.Body.Collection
                    .GetVectorByTag(VersionInfoTag)
                    .GetUInt4(0);
            }
        }

        /// <summary>
        /// Gets the minor version number of the PQDIF file writer.
        /// </summary>
        public uint WriterMinorVersion
        {
            get
            {
                return m_physicalRecord.Body.Collection
                    .GetVectorByTag(VersionInfoTag)
                    .GetUInt4(1);
            }
        }

        /// <summary>
        /// Gets the compatible major version that the file can be read as.
        /// </summary>
        public uint CompatibleMajorVersion
        {
            get
            {
                return m_physicalRecord.Body.Collection
                    .GetVectorByTag(VersionInfoTag)
                    .GetUInt4(2);
            }
        }

        /// <summary>
        /// Gets the compatible minor version that the file can be read as.
        /// </summary>
        public uint CompatibleMinorVersion
        {
            get
            {
                return m_physicalRecord.Body.Collection
                    .GetVectorByTag(VersionInfoTag)
                    .GetUInt4(3);
            }
        }

        /// <summary>
        /// Gets the name of the file at the time the file was written.
        /// </summary>
        public string FileName
        {
            get
            {
                VectorElement fileNameVector = m_physicalRecord.Body.Collection.GetVectorByTag(FileNameTag);
                return Encoding.ASCII.GetString(fileNameVector.GetValues()).Trim((char)0);
            }
        }

        /// <summary>
        /// Gets the date and time of file creation.
        /// </summary>
        public DateTime Creation
        {
            get
            {
                return m_physicalRecord.Body.Collection
                    .GetScalarByTag(CreationTag)
                    .GetTimestamp();
            }
        }

        /// <summary>
        /// Gets the style of compression used to compress the PQDIF file.
        /// </summary>
        public CompressionStyle CompressionStyle
        {
            get
            {
                CollectionElement collection = m_physicalRecord.Body.Collection;
                ScalarElement compressionStyleElement = collection.GetScalarByTag(CompressionStyleTag);
                uint compressionStyleID = (uint)CompressionStyle.None;

                if ((object)compressionStyleElement != null)
                    compressionStyleID = compressionStyleElement.GetUInt4();

                return (CompressionStyle)compressionStyleID;
            }
        }

        /// <summary>
        /// Gets the compression algorithm used to compress the PQDIF file.
        /// </summary>
        public CompressionAlgorithm CompressionAlgorithm
        {
            get
            {
                CollectionElement collection = m_physicalRecord.Body.Collection;
                ScalarElement compressionAlgorithmElement = collection.GetScalarByTag(CompressionAlgorithmTag);
                uint compressionAlgorithmID = (uint)CompressionAlgorithm.None;

                if ((object)compressionAlgorithmElement != null)
                    compressionAlgorithmID = compressionAlgorithmElement.GetUInt4();

                return (CompressionAlgorithm)compressionAlgorithmID;
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        
        /// <summary>
        /// Tag that identifies the version info.
        /// </summary>
        public static readonly Guid VersionInfoTag = new Guid("89738607-f1c3-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the file name.
        /// </summary>
        public static readonly Guid FileNameTag = new Guid("89738608-f1c3-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the date and time of creation.
        /// </summary>
        public static readonly Guid CreationTag = new Guid("89738609-f1c3-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the compression style of the PQDIF file.
        /// </summary>
        public static readonly Guid CompressionStyleTag = new Guid("8973861b-f1c3-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the compression algorithm used when writing the PQDIF file.
        /// </summary>
        public static readonly Guid CompressionAlgorithmTag = new Guid("8973861c-f1c3-11cf-9d89-0080c72e70a3");

        // Static Methods

        /// <summary>
        /// Creates a new container record from the given physical record
        /// if the physical record is of type container. Returns null if
        /// it is not.
        /// </summary>
        /// <param name="physicalRecord">The physical record used to create the container record.</param>
        /// <returns>The new container record, or null if the physical record does not define a container record.</returns>
        public static ContainerRecord CreateContainerRecord(Record physicalRecord)
        {
            bool isValidPhysicalRecord = physicalRecord.Header.TypeOfRecord == RecordType.Container;
            return (isValidPhysicalRecord) ? new ContainerRecord(physicalRecord) : null;
        }

        #endregion
    }
}
