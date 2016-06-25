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

using Kavprot.Packets.Utils;

﻿namespace Kavprot.Packets
{
    /// <summary>
    /// Represents an application layer packet as described at http://en.wikipedia.org/wiki/Application_Layer
    /// </summary>
    public abstract class ApplicationPacket : Packet
    {
        /// <summary>
        /// ApplicationPacket constructor
        /// </summary>
        public ApplicationPacket()
        {}
    }
}
