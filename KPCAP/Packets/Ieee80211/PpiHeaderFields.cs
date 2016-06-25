﻿#region Header

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
 * Copyright 2010 Chris Morgan <chmorgan@gmail.com>
 */

#endregion Header
using System;

namespace Kavprot.Packets
{
    namespace Ieee80211
    {
        /// <summary>
        /// As defined by Airpcap.h
        ///
        /// NOTE: PresentPosition may not be the only position present
        /// as this the field can be extended if the high bit is set
        /// </summary>
        public class PpiHeaderFields
        {
        #region Fields
            
            /// <summary>Position of the first iField Header</summary>
            public static readonly int FirstFieldPosition;

            /// <summary>Length of the Data Link Type</summary>
            public static readonly int DataLinkTypeLength = 4;
   
            /// <summary>The data link type position.</summary>
            public static readonly int DataLinkTypePosition;
            
            /// <summary>Length of the Flags field</summary>
            public static readonly int FlagsLength = 1;

            /// <summary>Position of the Flags field</summary>
            public static readonly int FlagsPosition;

            /// <summary>Length of the length field</summary>
            public static readonly int LengthLength = 2;

            /// <summary>Position of the length field</summary>
            public static readonly int LengthPosition;

            /// <summary>Length of the version field</summary>
            public static readonly int VersionLength = 1;

            /// <summary>Position of the version field</summary>
            public static readonly int VersionPosition = 0;
            
            /// <summary>The total length of the ppi packet header</summary>
            public static readonly int PpiPacketHeaderLength;
            
            /// <summary>The length of the PPI field header</summary>
            public static readonly int FieldHeaderLength = 4;
            
        #endregion Fields

        #region Constructors

            static PpiHeaderFields ()
            {
                FlagsPosition = VersionPosition + VersionLength;
                LengthPosition = FlagsPosition + FlagsLength;
                DataLinkTypePosition = LengthPosition + LengthLength;
                FirstFieldPosition = DataLinkTypePosition + DataLinkTypeLength;
                PpiPacketHeaderLength = FirstFieldPosition;
            }

        #endregion Constructors
        }
    }
}