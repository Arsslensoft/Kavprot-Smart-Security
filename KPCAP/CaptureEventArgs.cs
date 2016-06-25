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
 * Copyright 2008-2009 Chris Morgan <chmorgan@gmail.com>
 * Copyright 2008-2009 Phillip Lemon <lucidcomms@gmail.com>
 */
using System;

namespace KPCAP
{
    /// <summary>
    /// Capture event arguments
    /// </summary>
    public class CaptureEventArgs : EventArgs
    {
        private Kavprot.Packets.RawPacket packet;

        /// <summary>
        /// Packet that was captured
        /// </summary>
        public Kavprot.Packets.RawPacket Packet
        {
            get { return packet; }
        }

        private ICaptureDevice device;

        /// <summary>
        /// Device this EventArgs was generated for
        /// </summary>
        public ICaptureDevice Device
        {
            get { return device; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="packet">
        /// A <see cref="Kavprot.Packets.RawPacket"/>
        /// </param>
        /// <param name="device">
        /// A <see cref="ICaptureDevice"/>
        /// </param>
        public CaptureEventArgs(Kavprot.Packets.RawPacket packet, ICaptureDevice device)
        {
            this.packet = packet;
            this.device = device;
        }
    }
}
