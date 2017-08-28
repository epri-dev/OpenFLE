//*********************************************************************************************************************
// RecordHeader.cs
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
//  04/05/2012 - Mehulbhai Thakkar, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;
using System.Text;

namespace openPQDIF.Physical
{
    /// <summary>
    /// The header of a PQDIF <see cref="Record"/>. The header is part of
    /// the physical structure of a PQDIF file, and contains information
    /// on how to parse the <see cref="RecordBody"/> as well as how to find
    /// the next record.
    /// </summary>
    public class RecordHeader
    {
        #region [ Members ]

        // Fields
        private Guid m_recordSignature;
        private Guid m_recordTypeTag;
        private int m_headerSize;
        private int m_bodySize;
        private int m_nextRecordPosition;
        private uint m_checksum;
        private byte[] m_reserved;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the record's globally unique identifier.
        /// </summary>
        public Guid RecordSignature
        {
            get
            {
                return m_recordSignature;
            }
            set
            {
                m_recordSignature = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the record which determines
        /// the logical structure of the record.
        /// </summary>
        public RecordType TypeOfRecord
        {
            get
            {
                return Record.GetRecordType(m_recordTypeTag);
            }
        }

        /// <summary>
        /// Gets or sets the tag which identifies the type of the record.
        /// </summary>
        public Guid RecordTypeTag
        {
            get
            {
                return m_recordTypeTag;
            }
            set
            {
                m_recordTypeTag = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the header, in bytes.
        /// </summary>
        public int HeaderSize
        {
            get
            {
                return m_headerSize;
            }
            set
            {
                m_headerSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the body, in bytes.
        /// </summary>
        public int BodySize
        {
            get
            {
                return m_bodySize;
            }
            set
            {
                m_bodySize = value;
            }
        }

        /// <summary>
        /// Gets or sets the position of the next record in the PQDIF file.
        /// This value is a byte offset relative to the beginning of the file.
        /// </summary>
        public int NextRecordPosition
        {
            get
            {
                return m_nextRecordPosition;
            }
            set
            {
                m_nextRecordPosition = value;
            }
        }

        /// <summary>
        /// Optional checksum (such as a 32-bit CRC)
        /// of the record body to verify decompression.
        /// </summary>
        public uint Checksum
        {
            get
            {
                return m_checksum;
            }
            set
            {
                m_checksum = value;
            }
        }

        /// <summary>
        /// Reserved to fill structure to 64 bytes. Should be filled with 0.
        /// </summary>
        public byte[] Reserved
        {
            get
            {
                return m_reserved;
            }
            set
            {
                m_reserved = value;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns a string representation of the record header.
        /// </summary>
        /// <returns>A string representation of the record header.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("Record type: {0}", TypeOfRecord);
            builder.AppendLine();
            builder.AppendFormat("Header size: {0}", m_headerSize);
            builder.AppendLine();
            builder.AppendFormat("Body size: {0}", m_bodySize);

            return builder.ToString();
        }

        #endregion
    }
}
