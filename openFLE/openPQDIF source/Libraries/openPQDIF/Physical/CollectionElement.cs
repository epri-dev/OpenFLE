//*********************************************************************************************************************
// CollectionElement.cs
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
//  05/02/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace openPQDIF.Physical
{
    /// <summary>
    /// Represents an <see cref="Element"/> which is a collection of other
    /// elements. Collection elements are part of the physical structure of
    /// a PQDIF file. They exist within the body of a <see cref="Record"/>.
    /// </summary>
    public class CollectionElement : Element
    {
        #region [ Members ]

        // Fields
        private int m_size;
        private IList<Element> m_elements;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="CollectionElement"/> class.
        /// </summary>
        public CollectionElement()
        {
            m_elements = new List<Element>();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the number of elements in the collection.
        /// </summary>
        public int Size
        {
            get
            {
                return m_size;
            }
            set
            {
                m_size = value;
            }
        }

        /// <summary>
        /// Gets the type of the element.
        /// Returns <see cref="ElementType.Collection"/>.
        /// </summary>
        public override ElementType TypeOfElement
        {
            get
            {
                return ElementType.Collection;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Adds the given element to the collection.
        /// </summary>
        /// <param name="element">The element to be added.</param>
        public void AddElement(Element element)
        {
            m_elements.Add(element);
        }

        /// <summary>
        /// Gets the elements whose tag matches the one given as a parameter.
        /// </summary>
        /// <param name="tag">The tag of the elements to be retrieved.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of <see cref="Element"/>s
        /// identified by the given <paramref name="tag"/>.
        /// </returns>
        public IEnumerable<Element> GetElementsByTag(Guid tag)
        {
            return m_elements.Where(element => element.TagOfElement == tag);
        }

        /// <summary>
        /// Gets the element whose tag matches the one given as a
        /// parameter, type cast to <see cref="CollectionElement"/>.
        /// </summary>
        /// <param name="tag">The tag to search by.</param>
        /// <returns>The element whose tag matches the one given, or null if no matching collection element exists.</returns>
        public CollectionElement GetCollectionByTag(Guid tag)
        {
            return m_elements.SingleOrDefault(element => element.TagOfElement == tag) as CollectionElement;
        }

        /// <summary>
        /// Gets the element whose tag matches the one given as a
        /// parameter, type cast to <see cref="ScalarElement"/>.
        /// </summary>
        /// <param name="tag">The tag to search by.</param>
        /// <returns>The element whose tag matches the one given, or null if no matching scalar element exists.</returns>
        public ScalarElement GetScalarByTag(Guid tag)
        {
            return m_elements.SingleOrDefault(element => element.TagOfElement == tag) as ScalarElement;
        }

        /// <summary>
        /// Gets the element whose tag matches the one given as a
        /// parameter, type cast to <see cref="VectorElement"/>.
        /// </summary>
        /// <param name="tag">The tag to search by.</param>
        /// <returns>The element whose tag matches the one given, or null if no matching vector element exists.</returns>
        public VectorElement GetVectorByTag(Guid tag)
        {
            return m_elements.SingleOrDefault(element => element.TagOfElement == tag) as VectorElement;
        }

        /// <summary>
        /// Returns a string that represents the collection.
        /// </summary>
        /// <returns>A string that represents the collection.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            string[] lines;

            builder.AppendFormat("Collection -- Size: {0}", m_size);

            foreach (Element element in m_elements)
            {
                lines = element.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    builder.AppendLine();
                    builder.AppendFormat("    {0}", line);
                }
            }

            return builder.ToString();
        }

        #endregion
    }
}