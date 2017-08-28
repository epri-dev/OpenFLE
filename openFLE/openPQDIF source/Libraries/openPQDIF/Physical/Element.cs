//*********************************************************************************************************************
// ElementType.cs
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
//  04/30/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;

namespace openPQDIF.Physical
{
    #region [ Enumerations ]

    /// <summary>
    /// Enumeration that defines the types of
    /// elements found in the body of a record.
    /// </summary>
    public enum ElementType : byte
    {
        /// <summary>
        /// Collection element.
        /// Represents a collection of elements.
        /// </summary>
        Collection = 1,

        /// <summary>
        /// Scalar element.
        /// Represents a single value.
        /// </summary>
        Scalar = 2,

        /// <summary>
        /// Vector element.
        /// Represents a collection of values.
        /// </summary>
        Vector = 3
    }

    #endregion

    /// <summary>
    /// Base class for elements. Elements are part of the physical structure
    /// of a PQDIF file. They exist within the body of a <see cref="Record"/>.
    /// </summary>
    public abstract class Element
    {
        #region [ Members ]

        // Fields
        private Guid m_tagOfElement;
        private PhysicalType m_typeOfValue;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the tag which identifies the element.
        /// </summary>
        public virtual Guid TagOfElement
        {
            get
            {
                return m_tagOfElement;
            }
            set
            {
                m_tagOfElement = value;
            }
        }

        /// <summary>
        /// Gets the type of the element. The element can be a
        /// <see cref="ScalarElement"/>, a <see cref="VectorElement"/>,
        /// or a <see cref="CollectionElement"/>.
        /// </summary>
        public abstract ElementType TypeOfElement { get; }

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
        public virtual PhysicalType TypeOfValue
        {
            get
            {
                return m_typeOfValue;
            }
            set
            {
                m_typeOfValue = value;
            }
        }

        #endregion
    }
}