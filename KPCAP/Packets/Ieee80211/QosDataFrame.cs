﻿/*
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.Conversion;
using Kavprot.Packets.Utils;

namespace Kavprot.Packets
{
    namespace Ieee80211
    {
        /// <summary>
        /// Qos data frames are like regualr data frames except they contain a quality of service 
        /// field as deinfed in the 802.11e standard.
        /// </summary>
        public class QosDataFrame : DataFrame
        {
            private class QosDataField
            {
                public readonly static int QosControlLength = 2;

                public readonly static int QosControlPosition;

                static QosDataField()
                {
                    QosControlPosition = MacFields.SequenceControlPosition + MacFields.SequenceControlLength;
                }
            }
   
            /// <summary>
            /// Gets or sets the qos control field.
            /// </summary>
            /// <value>
            /// The qos control field.
            /// </value>
            public UInt16 QosControl { get; set; }

            private UInt16 QosControlBytes
            {
                get
                {
					if(header.Length >= (QosDataField.QosControlPosition + QosDataField.QosControlLength))
					{
						return EndianBitConverter.Little.ToUInt16(header.Bytes,
						                                          header.Offset + QosDataField.QosControlPosition);
					}
					else
					{
						return 0;
					}
                }

                set
                {
                    EndianBitConverter.Little.CopyBytes(value,
                                                     header.Bytes,
                                                     header.Offset + QosDataField.QosControlPosition);
                }
            }

            /// <summary>
            /// Length of the frame header.
            /// 
            /// This does not include the FCS, it represents only the header bytes that would
            /// would preceed any payload.
            /// </summary>
            public override int FrameSize
            {
                get
                {
                    //if we are in WDS mode then there are 4 addresses (normally it is just 3)
                    int numOfAddressFields = (FrameControl.ToDS && FrameControl.FromDS) ? 4 : 3;

                    return (MacFields.FrameControlLength +
                        MacFields.DurationIDLength +
                        (MacFields.AddressLength * numOfAddressFields) +
                        MacFields.SequenceControlLength +
                        QosDataField.QosControlLength);
                }
            }


            /// <summary>
            /// Initializes a new instance of the <see cref="Kavprot.Packets.Ieee80211.QosDataFrame"/> class.
            /// </summary>
            /// <param name='bas'>
            /// A <see cref="ByteArraySegment"/>
            /// </param>
            public QosDataFrame (ByteArraySegment bas)
            {
                header = new ByteArraySegment (bas);

                FrameControl = new FrameControlField (FrameControlBytes);
                Duration = new DurationField (DurationBytes);
                SequenceControl = new SequenceControlField (SequenceControlBytes);
                QosControl = QosControlBytes;
                ReadAddresses ();
                
                header.Length = FrameSize;
                var availablePayloadLength = GetAvailablePayloadLength();
                if(availablePayloadLength > 0)
				{
					payloadPacketOrData.TheByteArraySegment = header.EncapsulatedBytes (availablePayloadLength);
				}
            }
            
            /// <summary>
            /// Initializes a new instance of the <see cref="Kavprot.Packets.Ieee80211.QosDataFrame"/> class.
            /// </summary>
            public QosDataFrame ()
            {
                this.FrameControl = new FrameControlField ();
                this.Duration = new DurationField ();
                this.SequenceControl = new SequenceControlField ();
                AssignDefaultAddresses ();
                
                FrameControl.SubType = FrameControlField.FrameSubTypes.QosData;
            }
            
            /// <summary>
            /// Writes the current packet properties to the backing ByteArraySegment.
            /// </summary>
            public override void UpdateCalculatedValues ()
            {
                if ((header == null) || (header.Length > (header.BytesLength - header.Offset)) || (header.Length < FrameSize))
                {
                    header = new ByteArraySegment (new Byte[FrameSize]);
                }
                
                this.FrameControlBytes = this.FrameControl.Field;
                this.DurationBytes = this.Duration.Field;
                this.SequenceControlBytes = this.SequenceControl.Field;
                this.QosControlBytes = this.QosControl;
                WriteAddressBytes ();
            }
        } 
    }
}
