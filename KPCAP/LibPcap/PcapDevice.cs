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
using System.Runtime.InteropServices;

namespace KPCAP.LibPcap
{
    /// <summary>
    /// Base class for all pcap devices
    /// </summary>
    public abstract partial class PcapDevice : ICaptureDevice
    {
        /// <summary>
        /// Low level interface object that contains device specific information
        /// </summary>
        protected PcapInterface m_pcapIf;

        /// <summary>
        /// Handle to an open dump file, not equal to IntPtr.Zero if a dump file is open
        /// </summary>
        protected IntPtr       m_pcapDumpHandle    = IntPtr.Zero;

        /// <summary>
        /// Handle to a pcap adapter, not equal to IntPtr.Zero if an adapter is open
        /// </summary>
        protected IntPtr       m_pcapAdapterHandle = IntPtr.Zero;

        /// <summary>
        /// Number of packets that this adapter should capture
        /// </summary>
        protected int          m_pcapPacketCount   = Pcap.InfinitePacketCount;

        private int            m_mask  = 0; //for filter expression

        /// <summary>
        /// Device name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Description
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Fires whenever a new packet is processed, either when the packet arrives
        /// from the network device or when the packet is read from the on-disk file.<br/>
        /// For network captured packets this event is invoked only when working in "PcapMode.Capture" mode.
        /// </summary>
        public event PacketArrivalEventHandler OnPacketArrival;

        /// <summary>
        /// Fired when the capture process of this pcap device is stopped
        /// </summary>
        public event CaptureStoppedEventHandler OnCaptureStopped;

        /// <value>
        /// Low level pcap device values
        /// </value>
        public PcapInterface Interface
        {
            get { return m_pcapIf; }
        }

        /// <summary>
        /// Return a value indicating if this adapter is opened
        /// </summary>
        public virtual bool Opened
        {
            get { return (PcapHandle != IntPtr.Zero); }
        }

        /// <summary>
        /// Gets a value indicating wether pcap dump file is already associated with this device
        /// </summary>
        public virtual bool DumpOpened
        {
            get { return m_pcapDumpHandle!=IntPtr.Zero; }
        }

        /// <summary>
        /// The underlying pcap device handle
        /// </summary>
        internal virtual IntPtr PcapHandle
        {
            get { return m_pcapAdapterHandle; }
            set { m_pcapAdapterHandle = value; }
        }

        /// <summary>
        /// Retrieve the last error string for a given pcap_t* device
        /// </summary>
        /// <param name="deviceHandle">
        /// A <see cref="IntPtr"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        internal static string GetLastError(IntPtr deviceHandle)
        {
            IntPtr err_ptr = LibPcapSafeNativeMethods.pcap_geterr(deviceHandle);
            return Marshal.PtrToStringAnsi(err_ptr);
        }

        /// <summary>
        /// The last pcap error associated with this pcap device
        /// </summary>
        public virtual string LastError
        {
            get { return GetLastError(PcapHandle);  }
        }

        /// <summary>
        /// Link type in terms of Kavprot.Packets.LinkLayers
        /// </summary>
        public virtual Kavprot.Packets.LinkLayers LinkType
        {
            get
            {
                ThrowIfNotOpen("Cannot get datalink, the pcap device is not opened");
                return (Kavprot.Packets.LinkLayers)LibPcapSafeNativeMethods.pcap_datalink(PcapHandle);
            }
        }

        /// <summary>
        /// Open the device with class specific options
        /// </summary>
        public abstract void Open();

        /// <summary>
        /// Open the device. To start capturing call the 'StartCapture' function
        /// </summary>
        /// <param name="mode">
        /// A <see cref="DeviceMode"/>
        /// </param>
        public virtual void Open(DeviceMode mode)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Open the device. To start capturing call the 'StartCapture' function
        /// </summary>
        /// <param name="mode">
        /// A <see cref="DeviceMode"/>
        /// </param>
        /// <param name="read_timeout">
        /// A <see cref="System.Int32"/>
        /// </param>
        public virtual void Open(DeviceMode mode, int read_timeout)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Closes this adapter
        /// </summary>
        public virtual void Close()
        {
            if(PcapHandle==IntPtr.Zero)
                return;

            if (Started)
            {
                StopCapture();
            }
            LibPcapSafeNativeMethods.pcap_close(PcapHandle);
            PcapHandle = IntPtr.Zero;

            //Remove event handlers
            if ( OnPacketArrival != null)
            {
                foreach(PacketArrivalEventHandler pa in OnPacketArrival.GetInvocationList())
                {
                    OnPacketArrival -= pa;
                }
            }
        }

        /// <summary>
        /// Retrieves pcap statistics
        /// </summary>
        /// <returns>
        /// A <see cref="PcapStatistics"/>
        /// </returns>
        public abstract ICaptureStatistics Statistics { get; }

        /// <summary>
        /// Notify the OnPacketArrival delegates about a newly captured packet
        /// </summary>
        /// <param name="p">
        /// A <see cref="Kavprot.Packets.RawPacket"/>
        /// </param>
        protected void SendPacketArrivalEvent(Kavprot.Packets.RawPacket p)
        {
            var handler = OnPacketArrival;
            if(handler != null )
            {
                //Invoke the packet arrival event
                handler(this, new CaptureEventArgs(p, this));
            }
        }

        /// <summary>
        /// Notify the delegates that are subscribed to the capture stopped event
        /// </summary>
        /// <param name="status">
        /// A <see cref="CaptureStoppedEventStatus"/>
        /// </param>
        protected void SendCaptureStoppedEvent(CaptureStoppedEventStatus status)
        {
            var handler = OnCaptureStopped;
            if(handler != null)
            {
                handler(this, status);
            }
        }

        /// <summary>
        /// Gets the next packet captured on this device
        /// </summary>
        /// <returns>The next packet captured on this device</returns>
        public virtual Kavprot.Packets.RawPacket GetNextPacket()
        {
            Kavprot.Packets.RawPacket p;
            int res = GetNextPacket( out p );
            if(res==-1)
                throw new PcapException("Error receiving packet.");
            return p;
        }

        /// <summary>
        /// Gets the next packet captured on this device
        /// </summary>
        /// <param name="p">
        /// A <see cref="Kavprot.Packets.RawPacket"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Int32"/> that contains the result code
        /// </returns>
        public virtual int GetNextPacket(out Kavprot.Packets.RawPacket p)
        {
            //Pointer to a packet info struct
            IntPtr header = IntPtr.Zero;

            //Pointer to a packet struct
            IntPtr data = IntPtr.Zero;
            int res = 0;

            // using an invalid PcapHandle can result in an unmanaged segfault
            // so check for that here
            ThrowIfNotOpen("Device must be opened via Open() prior to use");

            // If a user is calling GetNextPacket() when the background capture loop
            // is also calling into libpcap then bad things can happen
            //
            // The bad behavior I (Chris M.) saw was that the background capture would keep running
            // but no more packets were captured. Took two days to debug and regular users
            // may hit the issue more often so check and report the issue here
            if(Started)
            {
                throw new InvalidOperationDuringBackgroundCaptureException("GetNextPacket() invalid during background capture");
            }

            //Get a packet from winpcap
            res = LibPcapSafeNativeMethods.pcap_next_ex( PcapHandle, ref header, ref data);
            p = null;

            if(res>0)
            {
                //Marshal the packet
                if ( (header != IntPtr.Zero) && (data != IntPtr.Zero) )
                {
                    p = MarshalRawPacket(header, data);
                }
            }
            return res;
        }

        /// <summary>
        /// Pcap_loop callback method.
        /// </summary>
        protected virtual void PacketHandler(IntPtr param, IntPtr /* pcap_pkthdr* */ header, IntPtr data)
        {
            var p = MarshalRawPacket(header, data);
            SendPacketArrivalEvent(p);
        }

        /// <summary>
        /// Convert an unmanaged packet into a managed Kavprot.Packets.RawPacket
        /// </summary>
        /// <param name="header">
        /// A <see cref="IntPtr"/>
        /// </param>
        /// <param name="data">
        /// A <see cref="IntPtr"/>
        /// </param>
        /// <returns>
        /// A <see cref="Kavprot.Packets.RawPacket"/>
        /// </returns>
        protected virtual Kavprot.Packets.RawPacket MarshalRawPacket(IntPtr /* pcap_pkthdr* */ header, IntPtr data)
        {
            Kavprot.Packets.RawPacket p;

            // marshal the header
            var pcapHeader = new PcapHeader(header);

            var pkt_data = new byte[pcapHeader.CaptureLength];
            Marshal.Copy(data, pkt_data, 0, (int)pcapHeader.CaptureLength);

            p = new Kavprot.Packets.RawPacket(LinkType,
                              new Kavprot.Packets.PosixTimeval(pcapHeader.Seconds,
                                                       pcapHeader.MicroSeconds),
                              pkt_data);

            return p;
        }

        #region Dump methods
        /// <summary>
        /// Opens a file for packet writings
        /// </summary>
        /// <param name="fileName"></param>
        public void DumpOpen(string fileName)
        {
            ThrowIfNotOpen("Dump requires an open device");

            if(DumpOpened)
            {
                throw new PcapException("A dump file is already opened");
            }
            m_pcapDumpHandle = LibPcapSafeNativeMethods.pcap_dump_open(PcapHandle, fileName);
            if(!DumpOpened)
                throw new PcapException("Error openning dump file.");
        }

        /// <summary>
        /// Closes the opened dump file
        /// </summary>
        public void DumpClose()
        {
            if(DumpOpened)
            {
                LibPcapSafeNativeMethods.pcap_dump_close(m_pcapDumpHandle);
                m_pcapDumpHandle = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Flushes all write buffers of the opened dump file
        /// </summary>
        public void DumpFlush()
        {
            if (DumpOpened)
            {
                int result = LibPcapSafeNativeMethods.pcap_dump_flush(m_pcapDumpHandle);
                if (result < 0)
                    throw new PcapException("Error writing buffer to dumpfile. " + LastError);
            }
        }

        /// <summary>
        /// Writes a packet to the pcap dump file associated with this device.
        /// </summary>
        public void Dump(byte[] p, PcapHeader h)
        {
            ThrowIfNotOpen("Cannot dump packet, device is not opened");
            if(!DumpOpened)
                throw new DeviceNotReadyException("Cannot dump packet, dump file is not opened");

            //Marshal packet
            IntPtr pktPtr;
            pktPtr = Marshal.AllocHGlobal(p.Length);
            Marshal.Copy(p, 0, pktPtr, p.Length);

            //Marshal header
            IntPtr hdrPtr = h.MarshalToIntPtr();

            LibPcapSafeNativeMethods.pcap_dump(m_pcapDumpHandle, hdrPtr, pktPtr);

            Marshal.FreeHGlobal(pktPtr);
            Marshal.FreeHGlobal(hdrPtr);
        }

        /// <summary>
        /// Writes a packet to the pcap dump file associated with this device.
        /// </summary>
        /// <param name="p">The packet to write</param>
        public void Dump(byte[] p)
        {
            Dump(p, new PcapHeader(0, 0, (uint)p.Length, (uint)p.Length));
        }

        /// <summary>
        /// Writes a packet to the pcap dump file associated with this device.
        /// </summary>
        /// <param name="p">The packet to write</param>
        public void Dump(Kavprot.Packets.RawPacket p)
        {
            var data = p.Data;
            var timeval = p.Timeval;
            var header = new PcapHeader(timeval.Seconds, timeval.MicroSeconds,
                                        (uint)data.Length, (uint)data.Length);
            Dump(data, header);
        }

        #endregion

        #region Filtering
        /// <summary>
        /// Assign a filter to this device given a filterExpression
        /// </summary>
        /// <param name="filterExpression">The filter expression to compile</param>
        protected void SetFilter(string filterExpression)
        {
            // save the filter string
            _filterString = filterExpression;

            int res;
            IntPtr bpfProgram;
            string errorString;

            // pcap_setfilter() requires a valid pcap_t which isn't present if
            // the device hasn't been opened
            ThrowIfNotOpen("device is not open");

            // attempt to compile the program
            if(!CompileFilter(PcapHandle, filterExpression, (uint)m_mask, out bpfProgram, out errorString))
            {
                string err = string.Format("Can't compile filter ({0}) : {1} ", filterExpression, errorString);
                throw new PcapException(err);
            }

            //associate the filter with this device
            res = LibPcapSafeNativeMethods.pcap_setfilter( PcapHandle, bpfProgram );

            // Free the program whether or not we were successful in setting the filter
            // we don't want to leak unmanaged memory if we throw an exception.
            FreeBpfProgram(bpfProgram);

            //watch for errors
            if(res < 0)
            {
                errorString = string.Format("Can't set filter ({0}) : {1}", filterExpression, LastError);
                throw new PcapException(errorString);
            }
        }

        private string _filterString;

        /// <summary>
        /// Kernel level filtering expression associated with this device.
        /// For more info on filter expression syntax, see:
        /// http://www.winpcap.org/docs/docs31/html/group__language.html
        /// </summary>
        public virtual string Filter
        {
            get
            {
                return _filterString;
            }

            set
            {
                SetFilter(value);
            }
        }

        // If CompileFilter() returns true bpfProgram must be freed by passing it to FreeBpfProgram()
        /// or unmanaged memory will be leaked
        private static bool CompileFilter(IntPtr pcapHandle,
                                          string filterExpression,
                                          uint mask,
                                          out IntPtr bpfProgram,
                                          out string errorString)
        {
            int result;
            string err = String.Empty;

            bpfProgram = IntPtr.Zero;
            errorString = null;

            //Alocate an unmanaged buffer
            bpfProgram = Marshal.AllocHGlobal( Marshal.SizeOf(typeof(PcapUnmanagedStructures.bpf_program)));

            //compile the expressions
            result = LibPcapSafeNativeMethods.pcap_compile(pcapHandle,
                                                    bpfProgram,
                                                    filterExpression,
                                                    1,
                                                    mask);

            if(result < 0)
            {
                err = GetLastError(pcapHandle);

                // free up the program memory
                Marshal.FreeHGlobal(bpfProgram);            
                bpfProgram = IntPtr.Zero; // make sure not to pass out a valid pointer

                // set the error string
                errorString = err;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Free memory allocated in CompileFilter()
        /// </summary>
        /// <param name="bpfProgram">
        /// A <see cref="IntPtr"/>
        /// </param>
        private static void FreeBpfProgram(IntPtr bpfProgram)
        {
            // free any pcap internally allocated memory from pcap_compile()
            LibPcapSafeNativeMethods.pcap_freecode(bpfProgram);

            // free allocated buffers
            Marshal.FreeHGlobal(bpfProgram);            
        }

        /// <summary>
        /// Returns true if the filter expression was able to be compiled into a
        /// program without errors
        /// </summary>
        public static bool CheckFilter(string filterExpression,
                                       out string errorString)
        {
            IntPtr bpfProgram;
            IntPtr fakePcap = LibPcapSafeNativeMethods.pcap_open_dead((int)Kavprot.Packets.LinkLayers.Ethernet, Pcap.MAX_PACKET_SIZE);

            uint mask = 0;
            if(!CompileFilter(fakePcap, filterExpression, mask, out bpfProgram, out errorString))
            {
                LibPcapSafeNativeMethods.pcap_close(fakePcap);
                return false;
            }

            FreeBpfProgram(bpfProgram);

            LibPcapSafeNativeMethods.pcap_close(fakePcap);
            return true;
        }
        #endregion

        /// <summary>
        /// Sends a raw packet throgh this device
        /// </summary>
        /// <param name="p">The packet to send</param>
        public virtual void SendPacket(Kavprot.Packets.Packet p)
        {
            SendPacket(p.Bytes);
        }

        /// <summary>
        /// Sends a raw packet throgh this device
        /// </summary>
        /// <param name="p">The packet to send</param>
        /// <param name="size">The number of bytes to send</param>
        public virtual void SendPacket(Kavprot.Packets.Packet p, int size)
        {
            SendPacket(p.Bytes, size);
        }

        /// <summary>
        /// Sends a raw packet throgh this device
        /// </summary>
        /// <param name="p">The packet bytes to send</param>
        public virtual void SendPacket(byte[] p)
        {
            SendPacket(p, p.Length);
        }

        /// <summary>
        /// Sends a raw packet throgh this device
        /// </summary>
        /// <param name="p">The packet bytes to send</param>
        /// <param name="size">The number of bytes to send</param>
        public virtual void SendPacket(byte[] p, int size)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Helper method for checking that the adapter is open, throws an
        /// exception with a string of ExceptionString if the device isn't open
        /// </summary>
        /// <param name="ExceptionString">
        /// A <see cref="System.String"/>
        /// </param>
        protected void ThrowIfNotOpen(string ExceptionString)
        {
            if(!Opened)
            {
                throw new DeviceNotReadyException(ExceptionString);
            }
        }

        /// <summary>
        /// Override the default ToString() implementation
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public override string ToString ()
        {
            return "interface: " + m_pcapIf.ToString() + "\n";
        }
    }
}
