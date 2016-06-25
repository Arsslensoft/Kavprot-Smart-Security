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
 * Copyright 2008-2010 Chris Morgan <chmorgan@gmail.com>
 */
using System;

namespace KPCAP
{
    /// <summary>
    /// Thrown when a method not supported on an offline device is called
    /// </summary>
    public class NotSupportedOnOfflineDeviceException : PcapException
    {
        /// <summary>
        /// string constructor
        /// </summary>
        /// <param name="msg">
        /// A <see cref="System.String"/>
        /// </param>
        public NotSupportedOnOfflineDeviceException(string msg) : base(msg)
        {
        }
    }
}
