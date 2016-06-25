using System;
using System.Runtime.InteropServices;

namespace KAVE.VolumeControl
{
    /// <summary>
    /// Class file for holding all the custom sructures we need
    /// for controlling the system sound
    /// </summary>
    public static class VolumeStructs
    {
        /// <summary>
        /// struct for holding data for the mixer caps
        /// </summary>
        public struct MixerCaps
        {
            public int wMid;
            public int wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MAXPNAMELEN)] public string szPname;
            public int fdwSupport;
            public int cDestinations;
        }

        /// <summary>
        /// struct to hold data for the mixer control
        /// </summary>
        public struct Mixer
        {
            public int cbStruct;
            public int dwControlID;
            public int dwControlType;
            public int fdwControl;
            public int cMultipleItems;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_SHORT_NAME_CHARS)] public string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_LONG_NAME_CHARS)] public string szName;
            public int lMinimum;
            public int lMaximum;
            [MarshalAs(UnmanagedType.U4, SizeConst = 10)]  public int reserved;
        }

        /// <summary>
        /// struct for holding data about the details of the mixer control
        /// </summary>
        public struct MixerDetails
        {
            public int cbStruct;
            public int dwControlID;
            public int cChannels;
            public int item;
            public int cbDetails;
            public IntPtr paDetails;
        }

        /// <summary>
        /// struct to hold data for an unsigned mixer control details
        /// </summary>
        public struct UnsignedMixerDetails
        {
            public int dwValue;
        }

        /// <summary>
        /// struct to hold data for the mixer line
        /// </summary>
        public struct MixerLine
        {
            public int cbStruct;
            public int dwDestination;
            public int dwSource;
            public int dwLineID;
            public int fdwLine;
            public int dwUser;
            public int dwComponentType;
            public int cChannels;
            public int cConnections;
            public int cControls;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_SHORT_NAME_CHARS)] public string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_LONG_NAME_CHARS)] public string szName;
            public int dwType;
            public int dwDeviceID;
            public int wMid;
            public int wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MAXPNAMELEN)] public string szPname;
        }

        /// <summary>
        /// struct for holding data for the mixer line controls
        /// </summary>
        public struct LineControls
        {
            public int cbStruct;
            public int dwLineID;
            public int dwControl;
            public int cControls;
            public int cbmxctrl;
            public IntPtr pamxctrl;
        }
    }
}
