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
 * Copyright 2005 Tamir Gal <tamir@tamirgal.com>
 * Copyright 2009-2010 Chris Morgan <chmorgan@gmail.com>
 */

using System;
using KPCAP.LibPcap;

namespace KPCAP.WinPcap
{
    /// <summary>
    /// Event that contains statistics mode data
    /// NOTE: WinPcap only
    /// </summary>
    public class StatisticsModeEventArgs : CaptureEventArgs
    {
        /// <summary>
        /// Constructor for a statistics mode event
        /// </summary>
        /// <param name="packet">
        /// A <see cref="Kavprot.Packets.RawPacket"/>
        /// </param>
        /// <param name="device">
        /// A <see cref="PcapDevice"/>
        /// </param>
        public StatisticsModeEventArgs(Kavprot.Packets.RawPacket packet, PcapDevice device)
            : base(packet, device)
        {
        }

        /// <summary>
        /// Statistics data for this event
        /// </summary>
        public StatisticsModePacket Statistics
        {
            get
            {
                return new StatisticsModePacket(base.Packet);
            }
        }
    }
}
