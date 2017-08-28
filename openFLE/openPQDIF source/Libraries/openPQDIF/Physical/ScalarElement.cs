//*********************************************************************************************************************
// ScalarElement.cs
//
// Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC. All rights reserved.
//
// openFLE ("this software") is licensed under BSD 3-Clause license.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
// following conditions are met:
//
// �    Redistributions of source code must retain the above copyright  notice, this list of conditions and 
//      the following disclaimer.
//
// �    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//
// �    Neither the name of the Electric Power Research Institute, Inc. (�EPRI�) nor the names of its contributors 
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
// �    TVA Code Library 4.0.4.3 - Tennessee Valley Authority, tvainfo@tva.gov
//      No copyright is claimed pursuant to 17 USC � 105. All Other Rights Reserved.
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
    /// Represents an <see cref="Element"/> which is a single value in a
    /// PQDIF file. Scalar elements are part of the physical structure of
    /// a PQDIF file. They exist within the body of a <see cref="Record"/>
    /// (contained by a <see cref="CollectionElement"/>).
    /// </summary>
    public class ScalarElement : Element
    {
        #region [ Members ]

        // Fields
        private byte[] m_value;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="ScalarElement"/> class.
        /// </summary>
        public ScalarElement()
        {
            m_value = new byte[16];
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the type of the element.
        /// Returns <see cref="ElementType.Scalar"/>.
        /// </summary>
        public override ElementType TypeOfElement
        {
            get
            {
                return ElementType.Scalar;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets the value of the scalar as the physical type defined
        /// by <see cref="Element.TypeOfValue"/> and returns it as a generic
        /// <see cref="object"/>.
        /// </summary>
        /// <returns>The value of the scalar.</returns>
        public object Get()
        {
            switch (TypeOfValue)
            {
                case PhysicalType.Boolean1:
                    return m_value[0] != 0;

                case PhysicalType.Boolean2:
                    return GetInt2() != 0;

                case PhysicalType.Boolean4:
                    return GetInt4() != 0;

                case PhysicalType.Char1:
                    return Encoding.ASCII.GetString(m_value, 0, 1);

                case PhysicalType.Char2:
                    return Encoding.Unicode.GetString(m_value, 0, 2);

                case PhysicalType.Integer1:
                    return (sbyte)m_value[0];

                case PhysicalType.Integer2:
                    return GetInt2();

                case PhysicalType.Integer4:
                    return GetInt4();

                case PhysicalType.UnsignedInteger1:
                    return m_value[0];

                case PhysicalType.UnsignedInteger2:
                    return GetUInt2();

                case PhysicalType.UnsignedInteger4:
                    return GetUInt4();

                case PhysicalType.Real4:
                    return GetReal4();

                case PhysicalType.Real8:
                    return GetReal8();

                case PhysicalType.Complex8:
                    return GetComplex8();

                case PhysicalType.Complex16:
                    return GetComplex16();

                case PhysicalType.Timestamp:
                    return GetTimestamp();

                case PhysicalType.Guid:
                    return GetGuid();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets the value of this scalar as a 16-bit unsigned integer.
        /// </summary>
        /// <returns>The value as a 16-bit unsigned integer.</returns>
        public ushort GetUInt2()
        {
            return EndianOrder.LittleEndian.ToUInt16(m_value, 0);
        }

        /// <summary>
        /// Sets the value of this scalar as a 16-bit unsigned integer.
        /// </summary>
        /// <param name="value">The new value as a 16-bit unsigned integer.</param>
        public void SetUInt2(ushort value)
        {
            EndianOrder.LittleEndian.CopyBytes(value, m_value, 0);
        }

        /// <summary>
        /// Gets the value of this scalar as a 16-bit signed integer.
        /// </summary>
        /// <returns>The value as a 16-bit signed integer.</returns>
        public short GetInt2()
        {
            return EndianOrder.LittleEndian.ToInt16(m_value, 0);
        }

        /// <summary>
        /// Sets the value of this scalar as a 16-bit signed integer.
        /// </summary>
        /// <param name="value">The new value as a 16-bit signed integer.</param>
        public void SetInt2(short value)
        {
            EndianOrder.LittleEndian.CopyBytes(value, m_value, 0);
        }

        /// <summary>
        /// Gets the value of this scalar as a 32-bit unsigned integer.
        /// </summary>
        /// <returns>The value as a 32-bit unsigned integer.</returns>
        public uint GetUInt4()
        {
            return EndianOrder.LittleEndian.ToUInt32(m_value, 0);
        }

        /// <summary>
        /// Sets the value of this scalar as a 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">The new value as a 32-bit unsigned integer.</param>
        public void SetUInt4(uint value)
        {
            EndianOrder.LittleEndian.CopyBytes(value, m_value, 0);
        }

        /// <summary>
        /// Gets the value of this scalar as a 32-bit signed integer.
        /// </summary>
        /// <returns>The value as a 32-bit signed integer.</returns>
        public int GetInt4()
        {
            return EndianOrder.LittleEndian.ToInt32(m_value, 0);
        }

        /// <summary>
        /// Sets the value of this scalar as a 32-bit signed integer.
        /// </summary>
        /// <param name="value">The new value as a 32-bit signed integer.</param>
        public void SetInt4(int value)
        {
            EndianOrder.LittleEndian.CopyBytes(value, m_value, 0);
        }

        /// <summary>
        /// Gets the value of this scalar as a 4-byte boolean.
        /// </summary>
        /// <returns>The value as a 4-byte boolean.</returns>
        public bool GetBool4()
        {
            return EndianOrder.LittleEndian.ToInt32(m_value, 0) != 0;
        }

        /// <summary>
        /// Sets the value of this scalar as a 4-byte boolean.
        /// </summary>
        /// <param name="value">The new value as a 4-byte boolean.</param>
        public void SetBool4(bool value)
        {
            EndianOrder.LittleEndian.CopyBytes(value ? 1 : 0, m_value, 0);
        }

        /// <summary>
        /// Gets the value of this scalar as a 32-bit floating point number.
        /// </summary>
        /// <returns>The value as a 32-bit floating point number.</returns>
        public float GetReal4()
        {
            return EndianOrder.LittleEndian.ToSingle(m_value, 0);
        }

        /// <summary>
        /// Sets the value of this scalar as a 32-bit floating point number.
        /// </summary>
        /// <param name="value">The new value as a 32-bit floating point number.</param>
        public void SetReal4(float value)
        {
            EndianOrder.LittleEndian.CopyBytes(value, m_value, 0);
        }

        /// <summary>
        /// Gets the value of this scalar as a 64-bit floating point number.
        /// </summary>
        /// <returns>The value as a 64-bit floating point number.</returns>
        public double GetReal8()
        {
            return EndianOrder.LittleEndian.ToDouble(m_value, 0);
        }

        /// <summary>
        /// Sets the value of this scalar as a 64-bit floating point number.
        /// </summary>
        /// <param name="value">The new value as a 64-bit floating point number.</param>
        public void SetReal8(double value)
        {
            EndianOrder.LittleEndian.CopyBytes(value, m_value, 0);
        }

        /// <summary>
        /// Gets the value of this scalar as an 8-byte complex number.
        /// </summary>
        /// <returns>The value as an 8-byte complex number.</returns>
        public ComplexNumber GetComplex8()
        {
            double real = EndianOrder.LittleEndian.ToSingle(m_value, 0);
            double imaginary = EndianOrder.LittleEndian.ToSingle(m_value, 4);
            return new ComplexNumber(real, imaginary);
        }

        /// <summary>
        /// Sets the value of this scalar as an 8-byte complex number.
        /// </summary>
        /// <param name="value">The new value as an 8-byte complex number.</param>
        public void SetComplex8(ComplexNumber value)
        {
            EndianOrder.LittleEndian.CopyBytes((float)value.Real, m_value, 0);
            EndianOrder.LittleEndian.CopyBytes((float)value.Imaginary, m_value, 4);
        }

        /// <summary>
        /// Gets the value of this scalar as a 16-byte complex number.
        /// </summary>
        /// <returns>The value as a 16-byte complex number.</returns>
        public ComplexNumber GetComplex16()
        {
            double real = EndianOrder.LittleEndian.ToDouble(m_value, 0);
            double imaginary = EndianOrder.LittleEndian.ToDouble(m_value, 8);
            return new ComplexNumber(real, imaginary);
        }

        /// <summary>
        /// Sets the value of this scalar as a 16-byte complex number.
        /// </summary>
        /// <param name="value">The new value as a 16-byte complex number.</param>
        public void SetComplex16(ComplexNumber value)
        {
            EndianOrder.LittleEndian.CopyBytes(value.Real, m_value, 0);
            EndianOrder.LittleEndian.CopyBytes(value.Imaginary, m_value, 0);
        }

        /// <summary>
        /// Gets the value of this scalar as a globally unique identifier.
        /// </summary>
        /// <returns>The value as a globally unique identifier.</returns>
        public Guid GetGuid()
        {
            return new Guid(m_value);
        }

        /// <summary>
        /// Sets the value of this scalar as a globally unique identifier.
        /// </summary>
        /// <param name="value">The new value as a globally unique identifier.</param>
        public void SetGuid(Guid value)
        {
            m_value = value.ToByteArray();
        }

        /// <summary>
        /// Gets the value of this scalar as <see cref="DateTime"/>.
        /// </summary>
        /// <returns>The value of this scalar as a <see cref="DateTime"/>.</returns>
        public DateTime GetTimestamp()
        {
            DateTime epoch = new DateTime(1900, 1, 1);
            uint days = EndianOrder.LittleEndian.ToUInt32(m_value, 0);
            double seconds = EndianOrder.LittleEndian.ToDouble(m_value, 4);

            return DateTime.SpecifyKind(epoch.AddDays(days).AddSeconds(seconds), DateTimeKind.Utc);
        }

        /// <summary>
        /// Sets the value of this scalar as a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="value">The new value of this scalar as a <see cref="DateTime"/>.</param>
        public void SetTimestamp(DateTime value)
        {
            DateTime epoch = new DateTime(1900, 1, 1);
            TimeSpan sinceEpoch = value - epoch;
            TimeSpan daySpan = TimeSpan.FromDays(Math.Floor(sinceEpoch.TotalDays));
            TimeSpan secondSpan = sinceEpoch - daySpan;

            EndianOrder.LittleEndian.CopyBytes((uint)daySpan.TotalDays, m_value, 0);
            EndianOrder.LittleEndian.CopyBytes(secondSpan.TotalSeconds, m_value, 0);
        }

        /// <summary>
        /// Gets the raw bytes of the value that this scalar represents.
        /// </summary>
        /// <returns>The value in bytes.</returns>
        public byte[] GetValue()
        {
            return m_value.BlockCopy(0, TypeOfValue.GetByteSize());
        }

        /// <summary>
        /// Sets the raw bytes of the value that this scalar represents.
        /// </summary>
        /// <param name="value">The array containing the bytes.</param>
        /// <param name="offset">The offset into the array at which the value starts.</param>
        public void SetValue(byte[] value, int offset)
        {
            Buffer.BlockCopy(value, offset, m_value, 0, TypeOfValue.GetByteSize());
        }

        /// <summary>
        /// Returns a string representation of the scalar.
        /// </summary>
        /// <returns>A string representation of the scalar.</returns>
        public override string ToString()
        {
            return string.Format("Scalar -- Type: {0}", TypeOfValue);
        }

        #endregion
    }
}