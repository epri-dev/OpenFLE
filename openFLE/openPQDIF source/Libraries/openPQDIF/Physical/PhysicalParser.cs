//*********************************************************************************************************************
// PhysicalParser.cs
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
// •    DotNetZip 1.9.1.8 
//
//      Licensed under Microsoft Public License (MS-PL); you may not use DotNetZip except in compliance 
//      with MS-PL. You may obtain a copy of MS-PL at http://dotnetzip.codeplex.com/license.
//
//      DotNetZip is provided by the copyright holders and contributors "as is" and any express 
//      or implied warranties, including, but not limited to, the implied warranties of merchantability 
//      and fitness for a particular purpose are disclaimed.
//
//      This software uses Ionic.Zlib.dll from DotNetZip. The managed ZLIB code included in 
//      Ionic.Zlib.dll is modified code based on jzlib.
//
//      The following notice applies to jzlib:
//      -----------------------------------------------------------------------
//    
//      Copyright (c) 2000,2001,2002,2003 ymnk, JCraft,Inc. All rights reserved.
//    
//      Redistribution and use in source and binary forms, with or without
//      modification, are permitted provided that the following conditions are met:
//    
//      1. Redistributions of source code must retain the above copyright notice,
//      this list of conditions and the following disclaimer.
//    
//      2. Redistributions in binary form must reproduce the above copyright
//      notice, this list of conditions and the following disclaimer in
//      the documentation and/or other materials provided with the distribution.
//
//      3. The names of the authors may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
//      THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED WARRANTIES,
//      INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//      FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL JCRAFT,
//      INC. OR ANY CONTRIBUTORS TO THIS SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT,
//      INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
//      LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
//      OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//      LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//      NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
//      EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//      -----------------------------------------------------------------------
//        
//      jzlib is based on zlib-1.1.3.
//    
//      The following notice applies to zlib:
//
//      -----------------------------------------------------------------------
//
//      Copyright (C) 1995-2004 Jean-loup Gailly and Mark Adler
//
//      The ZLIB software is provided 'as-is', without any express or implied
//      warranty.  In no event will the authors be held liable for any damages
//      arising from the use of this software.
//
//      Permission is granted to anyone to use this software for any purpose,
//      including commercial applications, and to alter it and redistribute it
//      freely, subject to the following restrictions:
//
//      1. The origin of this software must not be misrepresented; you must not
//         claim that you wrote the original software. If you use this software
//         in a product, an acknowledgment in the product documentation would be
//         appreciated but is not required.
//      2. Altered source versions must be plainly marked as such, and must not be
//         misrepresented as being the original software.
//      3. This notice may not be removed or altered from any source distribution.
//
//      Jean-loup Gailly jloup@gzip.org
//      Mark Adler madler@alumni.caltech.edu
//
//     -----------------------------------------------------------------------
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
using System.IO;
using Ionic.Zlib;

namespace openPQDIF.Physical
{
    #region [ Enumerations ]

    /// <summary>
    /// Enumeration which defines the types of compression used in PQDIF files.
    /// </summary>
    public enum CompressionStyle : uint
    {
        /// <summary>
        /// No compression.
        /// </summary>
        None = 0,

        /// <summary>
        /// Compress the entire file after the container record.
        /// This compression style is deprecated and is currently
        /// not supported by this PQDIF library.
        /// </summary>
        TotalFile = 1,

        /// <summary>
        /// Compress the body of each record.
        /// </summary>
        RecordLevel = 2
    }

    /// <summary>
    /// Enuemration which defines the algorithms used to compress PQDIF files.
    /// </summary>
    public enum CompressionAlgorithm : uint
    {
        /// <summary>
        /// No compression.
        /// </summary>
        None = 0,

        /// <summary>
        /// Zlib compression.
        /// http://www.zlib.net/
        /// </summary>
        Zlib = 1,

        /// <summary>
        /// PKZIP compression.
        /// This compression algorithm is deprecated and
        /// is currently not supported by this PQDIF library.
        /// </summary>
        PKZIP = 64
    }

    #endregion

    /// <summary>
    /// Represents a parser which parses the physical structure of a PQDIF file.
    /// </summary>
    public class PhysicalParser : IDisposable
    {
        #region [ Members ]

        // Fields
        private string m_fileName;
        private BinaryReader m_fileReader;
        private CompressionStyle m_compressionStyle;
        private CompressionAlgorithm m_compressionAlgorithm;

        private bool m_hasNextRecord;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="PhysicalParser"/> class.
        /// </summary>
        /// <param name="fileName">Name of the PQDIF file to be parsed.</param>
        public PhysicalParser(string fileName)
        {
            FileName = fileName;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the file name of the PQDIF file to be parsed.
        /// </summary>
        public string FileName
        {
            get
            {
                return m_fileName;
            }
            set
            {
                if ((object)value == null)
                    throw new ArgumentNullException("value");

                m_fileName = value;
            }
        }

        /// <summary>
        /// Gets or sets the compression style used by the PQDIF file.
        /// </summary>
        public CompressionStyle CompressionStyle
        {
            get
            {
                return m_compressionStyle;
            }
            set
            {
                if (value == CompressionStyle.TotalFile)
                    throw new ArgumentException("Total file compression has been deprecated and is not supported", "value");

                m_compressionStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets the compression algorithm used by the PQDIF file.
        /// </summary>
        public CompressionAlgorithm CompressionAlgorithm
        {
            get
            {
                return m_compressionAlgorithm;
            }
            set
            {
                if (value == CompressionAlgorithm.PKZIP)
                    throw new ArgumentException("PKZIP compression has been deprecated and is not supported", "value");

                m_compressionAlgorithm = value;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Opens the PQDIF file.
        /// </summary>
        public void Open()
        {
            m_fileReader = new BinaryReader(File.OpenRead(m_fileName));
            m_hasNextRecord = true;
        }

        /// <summary>
        /// Returns true if this parser has not reached the end of the PQDIF file.
        /// </summary>
        /// <returns><c>false</c> if the end of the file has been reached; <c>true</c> otherwise</returns>
        public bool HasNextRecord()
        {
            return m_hasNextRecord;
        }

        /// <summary>
        /// Reads the next record from the PQDIF file.
        /// </summary>
        /// <returns>The next record to be parsed from the PQDIF file.</returns>
        public Record NextRecord()
        {
            RecordHeader header;
            RecordBody body;

            header = ReadRecordHeader();
            body = ReadRecordBody(header.BodySize);

            if ((object)body != null)
                body.Collection.TagOfElement = header.RecordTypeTag;

            m_hasNextRecord = header.NextRecordPosition != 0;
            m_fileReader.BaseStream.Seek(header.NextRecordPosition, SeekOrigin.Begin);

            return new Record(header, body);
        }

        /// <summary>
        /// Sets the parser back to the beginning of the file.
        /// </summary>
        public void Reset()
        {
            m_fileReader.BaseStream.Seek(0, SeekOrigin.Begin);
            m_hasNextRecord = true;
        }

        /// <summary>
        /// Closes the PQDIF file.
        /// </summary>
        public void Close()
        {
            m_fileReader.Close();
            m_hasNextRecord = false;
        }

        /// <summary>
        /// Releases all resources held by this parser.
        /// </summary>
        public void Dispose()
        {
            if ((object)m_fileReader != null)
            {
                m_fileReader.Dispose();
                m_fileReader = null;
            }

            m_hasNextRecord = false;
        }

        // Reads the header of a record from the PQDIF file.
        private RecordHeader ReadRecordHeader()
        {
            return new RecordHeader()
            {
                RecordSignature = new Guid(m_fileReader.ReadBytes(16)),
                RecordTypeTag = new Guid(m_fileReader.ReadBytes(16)),
                HeaderSize = m_fileReader.ReadInt32(),
                BodySize = m_fileReader.ReadInt32(),
                NextRecordPosition = m_fileReader.ReadInt32(),
                Checksum = m_fileReader.ReadUInt32(),
                Reserved = m_fileReader.ReadBytes(16)
            };
        }

        // Reads the body of a record from the PQDIF file.
        private RecordBody ReadRecordBody(int byteSize)
        {
            byte[] buffer;
            MemoryStream bufferStream;

            if (byteSize == 0)
                return null;

            buffer = m_fileReader.ReadBytes(byteSize);
            Decompress(ref buffer);
            bufferStream = new MemoryStream(buffer);

            return new RecordBody()
            {
                Collection = ReadCollection(new BinaryReader(bufferStream))
            };
        }

        // Reads an element from the PQDIF file.
        private Element ReadElement(BinaryReader recordBodyReader)
        {
            Element element = null;

            Guid tagOfElement = new Guid(recordBodyReader.ReadBytes(16));
            ElementType typeOfElement = (ElementType)recordBodyReader.ReadByte();
            PhysicalType typeOfValue = (PhysicalType)recordBodyReader.ReadByte();
            bool isEmbedded = recordBodyReader.ReadByte() != 0;
            byte reserved = recordBodyReader.ReadByte();

            long link;
            long returnLink;

            returnLink = recordBodyReader.BaseStream.Position + 8L;

            if (!isEmbedded || typeOfElement != ElementType.Scalar)
            {
                link = recordBodyReader.ReadInt32();
                recordBodyReader.BaseStream.Seek(link, SeekOrigin.Begin);
            }

            switch(typeOfElement)
            {
                case ElementType.Collection:
                    element = ReadCollection(recordBodyReader);
                    break;

                case ElementType.Scalar:
                    element = ReadScalar(recordBodyReader, typeOfValue);
                    break;

                case ElementType.Vector:
                    element = ReadVector(recordBodyReader, typeOfValue);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(string.Format("Unknown element type: {0}", typeOfElement));
            }

            element.TagOfElement = tagOfElement;
            recordBodyReader.BaseStream.Seek(returnLink, SeekOrigin.Begin);

            return element;
        }

        // Reads a collection element from the PQDIF file.
        private CollectionElement ReadCollection(BinaryReader recordBodyReader)
        {
            CollectionElement collection = new CollectionElement()
            {
                Size = recordBodyReader.ReadInt32()
            };

            for (int i = 0; i < collection.Size; i++)
                collection.AddElement(ReadElement(recordBodyReader));

            return collection;
        }

        // Reads a vector element from the PQDIF file.
        private VectorElement ReadVector(BinaryReader recordBodyReader, PhysicalType typeOfValue)
        {
            VectorElement element = new VectorElement()
            {
                Size = recordBodyReader.ReadInt32(),
                TypeOfValue = typeOfValue
            };

            byte[] values = recordBodyReader.ReadBytes(element.Size * typeOfValue.GetByteSize());

            element.SetValues(values, 0);

            return element;
        }

        // Reads a scalar element from the PQDIF file.
        private ScalarElement ReadScalar(BinaryReader recordBodyReader, PhysicalType typeOfValue)
        {
            ScalarElement element = new ScalarElement()
            {
                TypeOfValue = typeOfValue
            };

            byte[] value = recordBodyReader.ReadBytes(typeOfValue.GetByteSize());

            element.SetValue(value, 0);

            return element;
        }

        // Decompresses the given buffer based on the compression style and algorithm currently used by the parser.
        // The result is placed back in the buffer that was sent to this method.
        private void Decompress(ref byte[] buffer)
        {
            if (CompressionAlgorithm == CompressionAlgorithm.None || CompressionStyle == CompressionStyle.None)
                return;

            byte[] readBuffer = new byte[65536];
            MemoryStream readStream = new MemoryStream();
            int readAmount;

            using (MemoryStream bufferStream = new MemoryStream(buffer))
            {
                using (ZlibStream inflater = new ZlibStream(bufferStream, CompressionMode.Decompress))
                {
                    do
                    {
                        readAmount = inflater.Read(readBuffer, 0, readBuffer.Length);
                        readStream.Write(readBuffer, 0, readAmount);
                    } while (readAmount != 0);
                }
            }

            buffer = readStream.ToArray();
        }

        #endregion
    }
}
