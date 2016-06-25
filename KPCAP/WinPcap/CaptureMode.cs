/*
This file is part of KPCAP.

KPCAP is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

KPCAP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with KPCAP.  If not, see <http://www.gnu.org/licenses/>.
*/
/* 
 * Copyright 2010 Chris Morgan <chmorgan@gmail.com>
 */

using System;

namespace KPCAP.WinPcap
{
        /// <summary>
        /// The working mode of a Pcap device
        /// </summary>
        public enum CaptureMode : int
        {
            /// <summary>
            /// Set a Pcap device to capture packets, Capture mode
            /// </summary>
            Packets = 0,

            /// <summary>
            /// Set a Pcap device to report statistics.
            /// <br/>
            /// Statistics mode is only supported in WinPcap
            /// </summary>
            Statistics = 1
        };
}
