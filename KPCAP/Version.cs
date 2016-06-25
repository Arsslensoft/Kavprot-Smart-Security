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
 * Copyright 2008-2009 Phillip Lemon <lucidcomms@gmail.com>
 */

namespace KPCAP
{
    /// <summary>
    /// Helper class/method to retrieve the version of the KPCAP assembly
    /// </summary>
    public sealed class Version
    {
        Version() { }

        /// <summary>
        /// Returns the current version string of the KPCAP library
        /// </summary>
        /// <returns>the current version string of the KPCAP library</returns>
        public static string VersionString
        {
            get
            {
                System.Reflection.Assembly asm
                    = System.Reflection.Assembly.GetAssembly(typeof(KPCAP.Version));
                return asm.GetName().Version.ToString();
            }
        }
    }
}
