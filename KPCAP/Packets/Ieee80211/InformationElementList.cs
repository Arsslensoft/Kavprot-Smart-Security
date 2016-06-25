/*
This file is part of Kavprot.Packets

Kavprot.Packets is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Kavprot.Packets is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with Kavprot.Packets.  If not, see <http://www.gnu.org/licenses/>.
*/
/*
 * Copyright 2012 Alan Rushforth <alan.rushforth@gmail.com>
 */

using System;
using System.Linq;
using System.Collections.Generic;
using Kavprot.Packets.Utils;

namespace Kavprot.Packets
{
    namespace Ieee80211
    {
        /// <summary>
        /// A <see cref="System.Collections.Generic.List"/> of 
        /// <see cref="Kavprot.Packets.Ieee80211.InformationElement">InformationElements</see>.
        /// </summary>
        /// <remarks>
        /// The order and set of Information Elements allowed in a particular 802.11 frame type is dictated
        /// by the 802.11 standards.
        /// </remarks>
        public class InformationElementList : List<InformationElement>
        {          
            /// <summary>
            /// Initializes an empty <see cref="Kavprot.Packets.Ieee80211.InformationElementList"/>.
            /// </summary>
            public InformationElementList ()
            {
             
            }
            
            /// <summary>
            /// Initializes a new instance of the <see cref="Kavprot.Packets.Ieee80211.InformationElementList"/> class.
            /// </summary>
            /// <param name='list'>
            /// The elements to be included in the list.
            /// </param>
            public InformationElementList (InformationElementList list)
               :base(list)
            {
             
            }
            
            /// <summary>
            /// Initializes a new instance of the <see cref="Kavprot.Packets.Ieee80211.InformationElementList"/> class.
            /// </summary>
            /// <param name='bas'>
            /// A <see cref="Kavprot.Packets.Utils.ByteArraySegment"/> containing one or more information elements.
            /// bas.Offset should point to the first byte of the first Information Element. 
            /// </param>
            public InformationElementList (ByteArraySegment bas)
            {
                int index = 0;
                while ((index + InformationElement.ElementLengthPosition) < bas.Length)
                {
                    var ieStartPosition = bas.Offset + index;
                    Byte valueLength = bas.Bytes [ieStartPosition + InformationElement.ElementLengthPosition];
                    var ieLength = InformationElement.ElementIdLength + InformationElement.ElementLengthLength + valueLength;
                    var availableLength = Math.Min(ieLength, bas.Length - index);
                    this.Add (new InformationElement (new ByteArraySegment (bas.Bytes, ieStartPosition, availableLength)));

                    index += ieLength;
                }
            }
            
            /// <summary>
            /// Gets the total length in bytes of the list if its elements were serialised into a byte array
            /// </summary>
            /// <value>
            /// The length
            /// </value>
            public int Length
            {
                get
                {
                    int length = 0;
                    foreach (InformationElement ie in this)
                    {
                        length += ie.ElementLength;
                    }
                    return length;
                }
            }
            
            /// <summary>
            /// Gets a Byte[] containing the serialised 
            /// <see cref="Kavprot.Packets.Ieee80211.InformationElement">InformationElements</see>
            /// </summary>
            /// <value>
            /// The serialised <see cref="Kavprot.Packets.Ieee80211.InformationElement">InformationElements</see>
            /// </value>
            public Byte[] Bytes
            {
                get
                {
                    var bytes = new Byte[Length];
                    int index = 0;
                    foreach (var ie in this)
                    {
                        var ieBytes = ie.Bytes;
                        Array.Copy (ieBytes, 0, bytes, index, ieBytes.Length);

                        index += ieBytes.Length;
                    }

                    return bytes;
                }
            }
            
            /// <summary>
            /// Finds all <see cref="InformationElement">InformatonElements</see> in the lists
            /// with the provided id.
            /// </summary>
            /// <returns>
            /// The <see cref="InformationElement">InformationElements</see> found, or an empty array if none are found
            /// </returns>
            /// <param name='id'>
            /// The Id to search for
            /// </param>
            public InformationElement[] FindById (InformationElement.ElementId id)
            {
                return (from ie in this where ie.Id == id select ie).ToArray ();
            }
            
            /// <summary>
            /// Finds the first <see cref="InformationElement"/> in the list
            /// with the provided id.
            /// </summary>
            /// <returns>
            /// The first element with the provided Id or null if the list contains no relevant elements
            /// </returns>
            /// <param name='id'>
            /// The Id to search for
            /// </param>
            public InformationElement FindFirstById (InformationElement.ElementId id)
            {
                return (from ie in this where ie.Id == id select ie).FirstOrDefault ();
            }
            
            /// <summary>
            /// Serialises the <see cref="InformationElement">InformationElements</see>
            /// in the list into the provided buffer.
            /// </summary>
            /// <param name='destination'>
            /// The <see cref="Kavprot.Packets.Utils.ByteArraySegment"/> to copy the elements into.
            /// </param>
            /// <param name='offset'>
            /// The offset into destination at which to start copy the <see cref="InformationElement">InformationElements</see>
            /// </param>
            /// <remarks>Ensure that the destination is large enough to contain serialised elements
            /// before calling this method</remarks>
            public void CopyTo (ByteArraySegment destination, int offset)
            {
                int index = 0;
                foreach (var ie in this)
                {
                    var ieBytes = ie.Bytes;
                    Array.Copy (ieBytes, 0, destination.Bytes, offset + index, ieBytes.Length);

                    index += ieBytes.Length;
                }

            }
            
        }
    }
}

