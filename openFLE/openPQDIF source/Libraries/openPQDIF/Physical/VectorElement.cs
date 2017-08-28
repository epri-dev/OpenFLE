//*********************************************************************************************************************
// VectorElement.cs
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
//  05/02/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;
using System.Text;
using TVA;

namespace openPQDIF.Physical
{
    /// <summary>
    /// Represents an <see cref="Element"/> which is a collection of values
    /// in a PQDIF file. Vector elements are part of the physical structure
    /// of a PQDIF file. They exist within the body of a <see cref="Record"/>
    /// (contained by a <see cref="CollectionElement"/>).
    /// </summary>
    public class VectorElement : Element
    {
        #region [ Members ]

        // Fields
        private int m_size;
        private byte[] m_values;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the number of values in the vector.
        /// </summary>
        public int Size
        {
            get
            {
                return m_size;
            }
            set
            {
                if (m_size != value)
                {
                    m_size = value;
                    Reallocate();
                }
            }
        }

        /// <summary>
        /// Gets the type of the element.
        /// Returns <see cref="ElementType.Vector"/>.
        /// </summary>
        public override ElementType TypeOfElement
        {
            get
            {
                return ElementType.Vector;
            }
        }

        /// <summary>
        /// Gets or sets the physical type of the value or values contained
        /// by the element.
        /// </summary>
        /// <remarks>
        /// This determines the data type and size of the
        /// value or values. The value of this property is only relevant when
        /// <see cref="TypeOfElement"/> is either <see cref="ElementType.Scalar"/>
        /// or <see cref="ElementType.Vector"/>.
        /// </remarks>
        public override PhysicalType TypeOfValue
        {
            get
            {
                return base.TypeOfValue;
            }
            set
            {
                if (base.TypeOfValue != value)
                {
                    base.TypeOfValue = value;
                    Reallocate();
                }
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets the value at the given index as the physical type defined
        /// by <see cref="TypeOfValue"/> and returns it as a generic
        /// <see cref="object"/>.
        /// </summary>
        /// <param name="index">The index of the value to be retrieved.</param>
        /// <returns>The value that was retrieved from the vector.</returns>
        public object Get(int index)
        {
            switch (TypeOfValue)
            {
                case PhysicalType.Boolean1:
                    return GetUInt1(index) != 0;

                case PhysicalType.Boolean2:
                    return GetInt2(index) != 0;

                case PhysicalType.Boolean4:
                    return GetInt4(index) != 0;

                case PhysicalType.Char1:
                    return Encoding.ASCII.GetString(m_values)[index];

                case PhysicalType.Char2:
                    return Encoding.Unicode.GetString(m_values)[index];

                case PhysicalType.Integer1:
                    return (sbyte)GetUInt1(index);

                case PhysicalType.Integer2:
                    return GetInt2(index);

                case PhysicalType.Integer4:
                    return GetInt4(index);

                case PhysicalType.UnsignedInteger1:
                    return GetUInt1(index);

                case PhysicalType.UnsignedInteger2:
                    return (ushort)GetInt2(index);

                case PhysicalType.UnsignedInteger4:
                    return GetUInt4(index);

                case PhysicalType.Real4:
                    return GetReal4(index);

                case PhysicalType.Real8:
                    return GetReal8(index);

                case PhysicalType.Complex8:
                    return new ComplexNumber((double)GetReal4(index * 2), (double)GetReal4(index * 2 + 1));

                case PhysicalType.Complex16:
                    return new ComplexNumber(GetReal8(index * 2), GetReal8(index * 2 + 1));

                case PhysicalType.Timestamp:
                    return GetTimestamp(index);

                case PhysicalType.Guid:
                    return new Guid(m_values.BlockCopy(index * 16, 16));

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets a value in this vector as an 8-bit unsigned integer.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <returns>The value as an 8-bit unsigned integer.</returns>
        public byte GetUInt1(int index)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to retrieve values from uninitialized vector; set the size and physical type of the vector first");

            return m_values[index];
        }

        /// <summary>
        /// Sets a value in this vector as an 8-bit unsigned integer.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <param name="value">The new value of an 8-bit unsigned integer.</param>
        public void SetUInt1(int index, byte value)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to insert values into uninitialized vector; set the size and physical type of the vector first");

            m_values[index] = value;
        }

        /// <summary>
        /// Gets a value in this vector as a 16-bit signed integer.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <returns>The value as a 16-bit signed integer.</returns>
        public short GetInt2(int index)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to retrieve values from uninitialized vector; set the size and physical type of the vector first");

            int byteIndex = index * 2;
            return EndianOrder.LittleEndian.ToInt16(m_values, byteIndex);
        }

        /// <summary>
        /// Sets a value in this vector as a 16-bit signed integer.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <param name="value">The new value of a 16-bit signed integer.</param>
        public void SetInt2(int index, short value)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to insert values into uninitialized vector; set the size and physical type of the vector first");

            int byteIndex = index * 2;
            EndianOrder.LittleEndian.CopyBytes(value, m_values, byteIndex);
        }

        /// <summary>
        /// Gets a value in this vector as a 32-bit signed integer.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <returns>The value as a 32-bit signed integer.</returns>
        public int GetInt4(int index)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to retrieve values from uninitialized vector; set the size and physical type of the vector first");

            int byteIndex = index * 4;
            return EndianOrder.LittleEndian.ToInt32(m_values, byteIndex);
        }

        /// <summary>
        /// Sets a value in this vector as a 32-bit signed integer.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <param name="value">The new value of a 32-bit signed integer.</param>
        public void SetInt4(int index, int value)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to insert values into uninitialized vector; set the size and physical type of the vector first");

            int byteIndex = index * 4;
            EndianOrder.LittleEndian.CopyBytes(value, m_values, byteIndex);
        }

        /// <summary>
        /// Gets a value in this vector as a 32-bit unsigned integer.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <returns>The value as a 32-bit unsigned integer.</returns>
        public uint GetUInt4(int index)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to retrieve values from uninitialized vector; set the size and physical type of the vector first");

            int byteIndex = index * 4;
            return EndianOrder.LittleEndian.ToUInt32(m_values, byteIndex);
        }

        /// <summary>
        /// Sets a value in this vector as a 32-bit unsigned integer.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <param name="value">The new value of a 32-bit unsigned integer.</param>
        public void SetUInt4(int index, uint value)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to insert values into uninitialized vector; set the size and physical type of the vector first");

            int byteIndex = index * 4;
            EndianOrder.LittleEndian.CopyBytes(value, m_values, byteIndex);
        }

        /// <summary>
        /// Gets a value in this vector as a 32-bit floating point number.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <returns>The value as a 32-bit floating point number.</returns>
        public float GetReal4(int index)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to retrieve values from uninitialized vector; set the size and physical type of the vector first");

            int byteIndex = index * 4;
            return EndianOrder.LittleEndian.ToSingle(m_values, byteIndex);
        }

        /// <summary>
        /// Sets a value in this vector as a 32-bit floating point number.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <param name="value">The new value of a 32-bit floating point number.</param>
        public void SetReal4(int index, float value)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to insert values into uninitialized vector; set the size and physical type of the vector first");

            int byteIndex = index * 4;
            EndianOrder.LittleEndian.CopyBytes(value, m_values, byteIndex);
        }

        /// <summary>
        /// Gets a value in this vector as a 64-bit floating point number.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <returns>The value as a 64-bit floating point number.</returns>
        public double GetReal8(int index)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to retrieve values from uninitialized vector; set the size and physical type of the vector first");

            int byteIndex = index * 8;
            return EndianOrder.LittleEndian.ToDouble(m_values, byteIndex);
        }

        /// <summary>
        /// Sets a value in this vector as a 64-bit floating point number.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <param name="value">The new value of a 64-bit floating point number.</param>
        public void SetReal8(int index, double value)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to insert values into uninitialized vector; set the size and physical type of the vector first");

            int byteIndex = index * 8;
            EndianOrder.LittleEndian.CopyBytes(value, m_values, byteIndex);
        }

        /// <summary>
        /// Gets a value in this vector as a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <returns>The value as a <see cref="DateTime"/>.</returns>
        public DateTime GetTimestamp(int index)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to retrieve values from uninitialized vector; set the size and physical type of the vector first");

            int byteIndex = index * 12;

            DateTime epoch = new DateTime(1900, 1, 1);
            uint days = EndianOrder.LittleEndian.ToUInt32(m_values, byteIndex);
            double seconds = EndianOrder.LittleEndian.ToDouble(m_values, byteIndex + 4);

            return DateTime.SpecifyKind(epoch.AddDays(days).AddSeconds(seconds), DateTimeKind.Utc);
        }

        /// <summary>
        /// Sets a value in this vector as a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <param name="value">The new value of a <see cref="DateTime"/>.</param>
        public void SetTimestamp(int index, DateTime value)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to insert values into uninitialized vector; set the size and physical type of the vector first");

            int byteIndex = index * 12;

            DateTime epoch = new DateTime(1900, 1, 1);
            TimeSpan sinceEpoch = value - epoch;
            TimeSpan daySpan = TimeSpan.FromDays(Math.Floor(sinceEpoch.TotalDays));
            TimeSpan secondSpan = sinceEpoch - daySpan;

            EndianOrder.LittleEndian.CopyBytes((uint)daySpan.TotalDays, m_values, byteIndex);
            EndianOrder.LittleEndian.CopyBytes(secondSpan.TotalSeconds, m_values, byteIndex + 4);
        }

        /// <summary>
        /// Gets the raw bytes of the values contained by this vector.
        /// </summary>
        /// <returns>The raw bytes of the values contained by this vector.</returns>
        public byte[] GetValues()
        {
            return m_values;
        }

        /// <summary>
        /// Sets the raw bytes of the values contained by this vector.
        /// </summary>
        /// <param name="values">The array that contains the raw bytes.</param>
        /// <param name="offset">The offset into the array at which the values start.</param>
        public void SetValues(byte[] values, int offset)
        {
            if ((object)m_values == null)
                throw new InvalidOperationException("Unable to insert values into uninitialized vector; set the size and physical type of the vector first");

            Buffer.BlockCopy(values, offset, m_values, 0, m_size * TypeOfValue.GetByteSize());
        }

        /// <summary>
        /// Returns a string representation of this vector.
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public override string ToString()
        {
            return string.Format("Vector -- Type: {0}, Size: {1}", TypeOfValue, m_size);
        }

        // Reallocates the byte array containing the vector data based on
        // the size of the vector and the physical type of the values.
        private void Reallocate()
        {
            if (TypeOfValue != 0 && m_size > 0)
                m_values = new byte[m_size * TypeOfValue.GetByteSize()];
        }

        #endregion
    }
}