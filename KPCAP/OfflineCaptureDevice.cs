﻿/*
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
 * Copyright 2011 Chris Morgan <chmorgan@gmail.com>
 */

using System;

namespace KPCAP
{
    /// <summary>
    /// Capture device that reads from a pcap format file
    /// </summary>
    public class OfflineCaptureDevice : LibPcap.OfflinePcapDevice
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="pcapFile">
        /// A <see cref="System.String"/>
        /// </param>
        public OfflineCaptureDevice(string pcapFile) : base(pcapFile)
        {
        }
    }
}
