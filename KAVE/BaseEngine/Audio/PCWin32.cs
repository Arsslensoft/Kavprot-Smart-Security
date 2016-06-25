
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace KAVE.VolumeControl
{
    /// <summary>
    /// Class that contains all the Win32 API calls
    /// we need for controlling the system sound
    /// </summary>
    public static class PCWin32
    {
        [DllImport("winmm.dll", CharSet=CharSet.Ansi)] 
        public static extern int mixerClose (int hmx);

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerGetControlDetailsA(int hmxobj, ref VolumeStructs.MixerDetails pmxcd, int fdwDetails);

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerGetDevCapsA(int uMxId, VolumeStructs.MixerCaps pmxcaps, int cbmxcaps);

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerGetID(int hmxobj, int pumxID, int fdwId);

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerGetLineControlsA(int hmxobj, ref VolumeStructs.LineControls pmxlc, int fdwControls);

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerGetLineInfoA(int hmxobj, ref VolumeStructs.MixerLine pmxl, int fdwInfo);

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerGetNumDevs();

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerMessage(int hmx, int uMsg, int dwParam1, int dwParam2);

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerOpen(out int phmx, int uMxId, int dwCallback, int dwInstance, int fdwOpen);

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerSetControlDetails(int hmxobj, ref VolumeStructs.MixerDetails pmxcd, int fdwDetails);
    }
}
