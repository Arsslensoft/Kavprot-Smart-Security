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
 * Copyright 2010 Chris Morgan <chmorgan@gmail.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Linq;
using Kavprot.Packets.Utils;
using MiscUtil.Conversion;

namespace Kavprot.Packets
{
    namespace Ieee80211
    {
        /// <summary>
        /// .Net analog of a ieee80211_radiotap_header from airpcap.h
        /// </summary>
        public class RadioPacket : InternetLinkLayerPacket
        {
#if DEBUG
            private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#else
        // NOTE: No need to warn about lack of use, the compiler won't
        //       put any calls to 'log' here but we need 'log' to exist to compile
#pragma warning disable 0169, 0649
        private static readonly ILogInactive log;
#pragma warning restore 0169, 0649
#endif

            /// <summary>
            /// Version 0. Only increases for drastic changes, introduction of compatible
            /// new fields does not count.
            /// </summary>
            public byte Version { get; set; }
            
            private byte VersionBytes
            {
                get
                {
                    return header.Bytes[header.Offset + RadioFields.VersionPosition];
                }

                set
                {
                    header.Bytes[header.Offset + RadioFields.VersionPosition] = value;
                }
            }

            /// <summary>
            /// Length of the whole header in bytes, including it_version, it_pad, it_len
            /// and data fields
            /// </summary>
            public UInt16 Length {get; set;}
            
            private UInt16 LengthBytes
            {
                get
                {
                    return EndianBitConverter.Little.ToUInt16(header.Bytes,
                                                              header.Offset + RadioFields.LengthPosition);
                }

                set
                {
                    EndianBitConverter.Little.CopyBytes(value,
                                                        header.Bytes,
                                                        header.Offset + RadioFields.LengthPosition);
                }
            }

            /// <summary>
            /// Returns an array of UInt32 bitmap entries. Each bit in the bitmap indicates
            /// which fields are present. Set bit 31 (0x8000000)
            /// to extend the bitmap by another 32 bits. Additional extensions are made
            /// by setting bit 31.
            /// </summary>
            private UInt32[] Present {get; set;}
                
            private UInt32[] ReadPresentFields()
            {
                // make an array of the bitmask fields
                // the highest bit indicates whether other bitmask fields follow
                // the current field
                var bitmaskFields = new List<UInt32>();
                UInt32 bitmask = EndianBitConverter.Little.ToUInt32(header.Bytes,
                                                                    header.Offset + RadioFields.PresentPosition);
                bitmaskFields.Add(bitmask);
                int bitmaskOffsetInBytes = 4;
                while ((bitmask & (1 << 31)) == 1)
                {
                    // retrieve the next field
                    bitmask = EndianBitConverter.Little.ToUInt32(header.Bytes,
                                                                 header.Offset + RadioFields.PresentPosition + bitmaskOffsetInBytes);
                    bitmaskFields.Add(bitmask);
                    bitmaskOffsetInBytes += 4;
                }

                return bitmaskFields.ToArray();
            }
   
            /// <summary>
            /// Initializes a new instance of the <see cref="Kavprot.Packets.Ieee80211.RadioPacket"/> class.
            /// </summary>
            public RadioPacket()
            {
                Present = new UInt32[1];
                RadioTapFields = new SortedDictionary<RadioTapType, RadioTapField>();
                Length = (ushort)RadioFields.DefaultHeaderLength;
            }
            
            internal RadioPacket (ByteArraySegment bas)
            {
                log.Debug ("");

                // slice off the header portion
                header = new ByteArraySegment (bas);
                header.Length = RadioFields.DefaultHeaderLength;
                Version = VersionBytes;
                Length = LengthBytes;
                
                // update the header size based on the headers packet length
                header.Length = Length;
                Present = ReadPresentFields();
                RadioTapFields = ReadRadioTapFields();
    
                //Before we attempt to parse the payload we need to work out if 
                //the FCS was valid and if it will be present at the end of the frame
                FlagsRadioTapField flagsField = this[RadioTapType.Flags] as FlagsRadioTapField;
		        payloadPacketOrData = ParseEncapsulatedBytes(header.EncapsulatedBytes(), flagsField);
            }

            /// <summary cref="Packet.ToString(StringOutputType)" />
            public override string ToString(StringOutputType outputFormat)
            {
                var buffer = new StringBuilder();
                string color = "";
                string colorEscape = "";

                if (outputFormat == StringOutputType.Colored || outputFormat == StringOutputType.VerboseColored)
                {
                    color = Color;
                    colorEscape = AnsiEscapeSequences.Reset;
                }

                if (outputFormat == StringOutputType.Normal || outputFormat == StringOutputType.Colored)
                {
                    // build the output string
                    buffer.AppendFormat("{0}[Ieee80211RadioPacket: Version={2}, Length={3}, Present[0]=0x{4:x}]{1}",
                        color,
                        colorEscape,
                        Version,
                        Length,
                        Present[0]);
                }

                if (outputFormat == StringOutputType.Verbose || outputFormat == StringOutputType.VerboseColored)
                {
                    // collect the properties and their value
                    Dictionary<string, string> properties = new Dictionary<string, string>();
                    properties.Add("version", Version.ToString());
                    properties.Add("length", Length.ToString());
                    properties.Add("present", " (0x" + Present[0].ToString("x") + ")");

                    var radioTapFields = this.RadioTapFields;

                    foreach (var r in radioTapFields)
                    {
                        properties.Add(r.Value.FieldType.ToString(),
                                       r.Value.ToString());
                    }

                    // calculate the padding needed to right-justify the property names
                    int padLength = Utils.RandomUtils.LongestStringLength(new List<string>(properties.Keys));

                    // build the output string
                    buffer.AppendLine("Ieee80211RadioPacket");
                    foreach (var property in properties)
                    {
                        buffer.AppendLine("TAP: " + property.Key.PadLeft(padLength) + " = " + property.Value);
                    }
                    buffer.AppendLine("TAP:");
                }

                // append the base output
                buffer.Append(base.ToString(outputFormat));

                return buffer.ToString();
            }
   
            /// <summary>
            /// Add the specified field to the packet.
            /// </summary>
            /// <param name='field'>
            /// Field to be added
            /// </param>
            public void Add(RadioTapField field)
            {
                RadioTapFields[field.FieldType] = field;
                Length += field.Length;
                var presenceBit = (int)field.FieldType;
                var presenceField = (presenceBit / 32);
                if(Present.Length <= presenceField)
                {
                    var newPresentFields = new UInt32[presenceField];
                    Array.Copy(Present, newPresentFields, Present.Length);
                    //set bit 31 to true for every present field except the last one
                    for(int i = 0; i < newPresentFields.Length - 1; i++)
                    {
                        newPresentFields[i] |= 0x80000000;
                    }
                    Present = newPresentFields;
                }
                Present[presenceField] |= (UInt32)(1 << presenceBit);
            }
            
            /// <summary>
            /// Removes a field of the specified type if one is present in the packet.
            /// </summary>
            /// <param name='fieldType'>
            /// Field type.
            /// </param>
            public void Remove(RadioTapType fieldType)
            {
                RadioTapField field;
                if(RadioTapFields.TryGetValue(fieldType, out field))
                {
                    RadioTapFields.Remove(fieldType);
                    Length -= field.Length;
                    var presenceBit = (int)field.FieldType;
                    var presenceField = (presenceBit / 32);
                    Present[presenceField] &= (UInt32)~(1 << presenceBit);
                }
            }
            
            /// <summary>
            /// Checks for the presence of a field of the specified type in the packet.
            /// </summary>
            /// <param name='fieldType'>
            /// The field type to check for.
            /// </param>
            /// <returns><c>true</c> if the packet contains a field of the specified type; otherwise, <c>false</c>.</returns>
            public bool Contains(RadioTapType fieldType)
            {
                return RadioTapFields.ContainsKey(fieldType);
            }
            
            /// <summary>
            /// Gets the <see cref="Kavprot.Packets.Ieee80211.RadioTapField"/> with the specified type, or null if the
            /// field is not in the packet.
            /// </summary>
            /// <param name='type'>
            /// Radio Tap field type
            /// </param>
            public RadioTapField this[RadioTapType type]
            {
                get
                {
                    RadioTapField field;
                    RadioTapFields.TryGetValue(type, out field);
                    return field;
                }
            }
            
            /// <summary>
            /// Array of radio tap fields
            /// </summary>
            private SortedDictionary<RadioTapType, RadioTapField> RadioTapFields { get; set; }
            
            private byte[] UnhandledFieldBytes {get; set;}
            
            private SortedDictionary<RadioTapType, RadioTapField> ReadRadioTapFields()
            {
                var bitmasks = Present;

                var retval = new SortedDictionary<RadioTapType, RadioTapField>();

                int bitIndex = 0;

                // create a binary reader that points to the memory immediately after the bitmasks
                var offset = header.Offset +
                             RadioFields.PresentPosition +
                             (bitmasks.Length) * Marshal.SizeOf (typeof(UInt32));
                var br = new BinaryReader (new MemoryStream (header.Bytes,
                                                           offset,
                                                           (int)(Length - offset)));

                // now go through each of the bitmask fields looking at the least significant
                // bit first to retrieve each field
                foreach (var bmask in bitmasks)
                {
                    int[] bmaskArray = new int[1];
                    bmaskArray [0] = (int)bmask;
                    var ba = new BitArray (bmaskArray);
                    
                    bool unhandledFieldFound = false;

                    // look at all of the bits, note we don't want to consider the
                    // highest bit since that indicates another bitfield that follows
                    for (int x = 0; x < 31; x++)
                    {
                        if (ba [x] == true)
                        {
                            var field = RadioTapField.Parse (bitIndex, br);
                            if (field != null)
                            {
                                retval[field.FieldType] = field;
                            }
                            else
                            {
                                //We have found a field that we dont handle. As we dont know how big
                                //it is we can't handle any fields after it. We will just copy
                                //the rest of the data around as a lump
                                unhandledFieldFound = true;
                                break;
                            }
                        }
                        bitIndex++;
                    }
                    
                    if(unhandledFieldFound)
                    {
                        break;
                    }
                }
                
                //this will read the rest of the bytes. We pass in max value because we dont know how
                //much there is but this will ensure we get up to the end of the buffer
                UnhandledFieldBytes = br.ReadBytes(UInt16.MaxValue);

                return retval;
            }
   
            /// <summary>
            /// Called to ensure that field values are updated before
            /// the packet bytes are retrieved
            /// </summary>
            public override void UpdateCalculatedValues()
            {
                if ((header == null) || (header.Length < Length))
                {
                    //the backing buffer isnt big enough to accommodate the info elements so we need to resize it
                    header = new ByteArraySegment (new Byte[Length]);
                }
                
                VersionBytes = Version;
                LengthBytes = Length;
                var index = RadioFields.PresentPosition;
                foreach(var presentField in Present)
                {
                    EndianBitConverter.Little.CopyBytes(presentField,
                                                        header.Bytes,
                                                        header.Offset + index);
                    index += RadioFields.PresentLength;
                }
                
                foreach(var field in RadioTapFields)
                {
                    //then copy the field data to the appropriate index
                    field.Value.CopyTo(header.Bytes, header.Offset + index);
                    index += field.Value.Length;
                }
                
                if((UnhandledFieldBytes != null) && (UnhandledFieldBytes.Length > 0))
                {
                    Array.Copy(UnhandledFieldBytes, 0, header.Bytes, header.Offset + index, UnhandledFieldBytes.Length);
                }
            }

            internal static PacketOrByteArraySegment ParseEncapsulatedBytes (ByteArraySegment payload, FlagsRadioTapField flagsField)
            {
                var payloadPacketOrData = new PacketOrByteArraySegment ();
                MacFrame frame = null;
                
                if (flagsField != null)
                {
                    bool fcsPresent = ((flagsField.Flags & RadioTapFlags.FcsIncludedInFrame) == RadioTapFlags.FcsIncludedInFrame);
                    
                    if (fcsPresent)
                    {
                        frame = MacFrame.ParsePacketWithFcs (payload);
                    }
                    else
                    {
                        frame = MacFrame.ParsePacket (payload);
                    }
                }
                else
                {
                    frame = MacFrame.ParsePacket (payload);
                }
                
                if (frame == null)
                {
                    payloadPacketOrData.TheByteArraySegment = payload;
                }
                else
                {
                    payloadPacketOrData.ThePacket = frame;
                }
                
                return payloadPacketOrData;
            }
        } 
    }
}
