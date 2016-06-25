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
using Kavprot.Packets.Utils;
using MiscUtil.Conversion;
using System.Net.NetworkInformation;

namespace Kavprot.Packets
{
    namespace Ieee80211
    {
        /// <summary>
        /// Format of an 802.11 management beacon frame.
        /// 
        /// Beacon frames are used to annouce the existance of a wireless network. If an
        /// access point has been configured to not broadcast its SSID then it may not transmit
        /// beacon frames.
        /// </summary>
        public class BeaconFrame : ManagementFrame
        {

            private class BeaconFields
            {
                public readonly static int TimestampLength = 8;
                public readonly static int BeaconIntervalLength = 2;
                public readonly static int CapabilityInformationLength = 2;

                public readonly static int TimestampPosition;
                public readonly static int BeaconIntervalPosition;
                public readonly static int CapabilityInformationPosition;
                public readonly static int InformationElement1Position;

                static BeaconFields ()
                {
                    TimestampPosition = MacFields.SequenceControlPosition + MacFields.SequenceControlLength;
                    BeaconIntervalPosition = TimestampPosition + TimestampLength;
                    CapabilityInformationPosition = BeaconIntervalPosition + BeaconIntervalLength;
                    InformationElement1Position = CapabilityInformationPosition + CapabilityInformationLength;
                }
            }
            

            /// <summary>
            /// The number of microseconds the networks master timekeeper has been active.
            /// 
            /// Used for synchronisation between stations in an IBSS. When it reaches the maximum value the timestamp will wrap (not very likely).
            /// </summary>
            public UInt64 Timestamp {get; set;}
            
            private UInt64 TimestampBytes
            {
                get
                {
					if(header.Length >= (BeaconFields.TimestampPosition + BeaconFields.TimestampLength))
					{
						return EndianBitConverter.Little.ToUInt64(header.Bytes, header.Offset + BeaconFields.TimestampPosition);
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
                                                     header.Offset + BeaconFields.TimestampPosition);
                }
            }

            /// <summary>
            /// The number of "time units" between beacon frames.
            /// 
            /// A time unit is 1,024 microseconds. This interval is usually set to 100 which equates to approximately 100 milliseconds or 0.1 seconds.
            /// </summary>
            public UInt16 BeaconInterval {get; set;}
            
            private UInt16 BeaconIntervalBytes
            {
                get
                {
					if(header.Length >= (BeaconFields.BeaconIntervalPosition + BeaconFields.BeaconIntervalLength))
					{
						return EndianBitConverter.Little.ToUInt16(header.Bytes, header.Offset + BeaconFields.BeaconIntervalPosition);
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
                                                     header.Offset + BeaconFields.BeaconIntervalPosition);
                }
            }

            /// <summary>
            /// Frame control bytes are the first two bytes of the frame
            /// </summary>
            private UInt16 CapabilityInformationBytes
            {
                get
                {
					if(header.Length >= (BeaconFields.CapabilityInformationPosition + BeaconFields.CapabilityInformationLength))
					{
						return EndianBitConverter.Little.ToUInt16(header.Bytes,
						                                          header.Offset + BeaconFields.CapabilityInformationPosition);
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
                                                     header.Offset + BeaconFields.CapabilityInformationPosition);
                }
            }

            /// <summary>
            /// Defines the capabilities of the network.
            /// </summary>
            public CapabilityInformationField CapabilityInformation
            {
                get;
                set;
            }

            /// <summary>
            /// Gets the size of the frame.
            /// </summary>
            /// <value>
            /// The size of the frame.
            /// </value>
            public override int FrameSize
            {
                get
                {
                    return (MacFields.FrameControlLength +
                        MacFields.DurationIDLength +
                        (MacFields.AddressLength * 3) +
                        MacFields.SequenceControlLength +
                        BeaconFields.TimestampLength +
                        BeaconFields.BeaconIntervalLength +
                        BeaconFields.CapabilityInformationLength +
                        InformationElements.Length);
                }
            }

            /// <summary>
            /// The information elements included in the frame
            /// 
            /// Most (but not all) beacons frames will contain an Information element that contains the SSID.
            /// </summary>
            public InformationElementList InformationElements { get; private set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="bas">
            /// A <see cref="ByteArraySegment"/>
            /// </param>
            public BeaconFrame (ByteArraySegment bas)
            {
                header = new ByteArraySegment (bas);

                FrameControl = new FrameControlField (FrameControlBytes);
                Duration = new DurationField (DurationBytes);
                DestinationAddress = GetAddress (0);
                SourceAddress = GetAddress (1);
                BssId = GetAddress (2);
                SequenceControl = new SequenceControlField (SequenceControlBytes);
                Timestamp = TimestampBytes;
                BeaconInterval = BeaconIntervalBytes;
                CapabilityInformation = new CapabilityInformationField (CapabilityInformationBytes);

				if(bas.Length > BeaconFields.InformationElement1Position)
				{
					//create a segment that just refers to the info element section
					ByteArraySegment infoElementsSegment = new ByteArraySegment (bas.Bytes,
					                                                             (bas.Offset + BeaconFields.InformationElement1Position),
					                                                             (bas.Length - BeaconFields.InformationElement1Position ));
					
					InformationElements = new InformationElementList (infoElementsSegment);
				}
				else
				{
					InformationElements = new InformationElementList ();
				}
                
                //cant set length until after we have handled the information elements
                //as they vary in length
                header.Length = FrameSize;
            }
   
            /// <summary>
            /// Initializes a new instance of the <see cref="Kavprot.Packets.Ieee80211.BeaconFrame"/> class.
            /// </summary>
            /// <param name='SourceAddress'>
            /// Source address.
            /// </param>
            /// <param name='BssId'>
            /// Bss identifier (MAC Address of the Access Point).
            /// </param>
            /// <param name='InformationElements'>
            /// Information elements.
            /// </param>
            public BeaconFrame (PhysicalAddress SourceAddress,
                                PhysicalAddress BssId, 
                                InformationElementList InformationElements)
            {
                this.FrameControl = new FrameControlField ();
                this.Duration = new DurationField ();
                this.SequenceControl = new SequenceControlField ();
                this.CapabilityInformation = new CapabilityInformationField ();
                this.InformationElements = new InformationElementList (InformationElements);
                this.FrameControl.SubType = FrameControlField.FrameSubTypes.ManagementBeacon;
                this.SourceAddress = SourceAddress;
                this.DestinationAddress = PhysicalAddress.Parse ("FF-FF-FF-FF-FF-FF");
                this.BssId = BssId;
                this.BeaconInterval = 100;
            }
            
            /// <summary>
            /// Writes the current packet properties to the backing ByteArraySegment.
            /// </summary>
            public override void UpdateCalculatedValues ()
            {
				
                if ((header == null) || (header.Length > (header.BytesLength - header.Offset)) || (header.Length < FrameSize))
                {
                    //the backing buffer isnt big enough to accommodate the info elements so we need to resize it
                    header = new ByteArraySegment (new Byte[FrameSize]);
                }
                
                this.FrameControlBytes = this.FrameControl.Field;
                this.DurationBytes = this.Duration.Field;
                SetAddress (0, DestinationAddress);
                SetAddress (1, SourceAddress);
                SetAddress (2, BssId);
                this.SequenceControlBytes = this.SequenceControl.Field;
                this.TimestampBytes = Timestamp;
                this.BeaconIntervalBytes = BeaconInterval;
                this.CapabilityInformationBytes = this.CapabilityInformation.Field;
                
                //we now know the backing buffer is big enough to contain the info elements so we can safely copy them in
                this.InformationElements.CopyTo (header, header.Offset + BeaconFields.InformationElement1Position);
                
                header.Length = FrameSize;
            }

        } 
    }
}
