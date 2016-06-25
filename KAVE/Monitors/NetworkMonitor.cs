using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using Kavprot.Packets;
using KPCAP;
using KAVE.BaseEngine;

namespace KAVE.Monitors
{

    /// <summary>
    /// Network Protection Agent (NIDS, Virtual Firewall)
    /// </summary>
    public static class NetworkMonitor
    {
       static CaptureDeviceList devices;
        public static bool Initialized = false;
        public static bool Runing = false;
        public static void Initialize(string filter)
        {
            try
            {
                if (!Initialized)
                {
                    devices = CaptureDeviceList.Instance;
                    if (devices.Count < 1)
                    {
                        Initialized = false;
                    }
                    else
                    {
                        int i = 0;
                        foreach (var dev in devices)
                        {

                            var dev1 = CaptureDeviceList.New()[i];
                            // Register our handler function to the 'packet arrival' event
                            dev.OnPacketArrival +=
                                new PacketArrivalEventHandler(device_OnPacketArrival);
                            dev1.OnPacketArrival +=
                                new PacketArrivalEventHandler(device1_OnPacketArrival);
                            // Open the device for capturing
                            int readTimeoutMilliseconds = 1000;
                            dev.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
                            dev1.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
                            dev.Filter = filter;

                            i++;
                        }
                        Initialized = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Initialized =false;
                AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
    
        private static void device1_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            try
            {
                Kavprot.Packets.Packet packet = Kavprot.Packets.Packet.ParsePacket(e.Packet);
                if (packet is Kavprot.Packets.EthernetPacket)
                {
                    var ip = Kavprot.Packets.IpPacket.GetEncapsulated(packet);

                    if (ip.Protocol == Kavprot.Packets.IPProtocolType.TCP)
                    {
                        TcpPacket tcp = TcpPacket.GetEncapsulated(packet);
                        if (tcp != null)
                        {
                            if (!tcp.IsValidChecksum(TransportPacket.TransportChecksumOption.None))
                                Alert.Attack("Intrusion Detected : Invalid TCP Checksum", "an intrusion was detected using TCP from " + ip.SourceAddress.ToString() + " @port " + tcp.SourcePort.ToString(), ToolTipIcon.Warning, true);


                        }

                    }
                    else if (ip.Protocol == Kavprot.Packets.IPProtocolType.UDP)
                    {
                        UdpPacket udp = UdpPacket.GetEncapsulated(packet);
                        if (udp != null)
                        {
                            if (!udp.IsValidChecksum(TransportPacket.TransportChecksumOption.None))
                                Alert.Attack("Intrusion Detected : Invalid UDP Checksum", "an intrusion was detected using UDP from " + ip.SourceAddress.ToString() + " @port " + udp.SourcePort.ToString(), ToolTipIcon.Warning, true);

                        }

                    }
                }

            }
            catch
            {

            }
            finally
            {

            }
        }
        private static void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            try
            {
                Kavprot.Packets.Packet packet = Kavprot.Packets.Packet.ParsePacket(e.Packet);
                if (packet is Kavprot.Packets.EthernetPacket)
                {
                    var ip = Kavprot.Packets.IpPacket.GetEncapsulated(packet);

                    if (ip.Protocol == Kavprot.Packets.IPProtocolType.TCP)
                    {
                        TcpPacket tcp = TcpPacket.GetEncapsulated(packet);
                        if (tcp != null)
                        {
                            Alert.Attack("Intrusion Detected", "an intrusion was detected using TCP from " + ip.SourceAddress.ToString() + " @port " + tcp.SourcePort.ToString(), ToolTipIcon.Warning, true);
                        }

                    }
                    else if (ip.Protocol == Kavprot.Packets.IPProtocolType.UDP)
                    {
                        UdpPacket udp = UdpPacket.GetEncapsulated(packet);
                        if (udp != null)
                        {
                            Alert.Attack("Intrusion Detected", "an intrusion was detected using UDP from " + ip.SourceAddress.ToString() + " @port " + udp.SourcePort.ToString(), ToolTipIcon.Warning, true);

                        }

                    }
                    else if (ip.Protocol == Kavprot.Packets.IPProtocolType.IGMP)
                    {
                        IGMPv2Packet igmp = IGMPv2Packet.GetEncapsulated(packet);
                        if (igmp != null)
                        {
                            Alert.Attack("Intrusion Detected : Unwanted IGMP Packet", "an intrusion was detected using IGMP from " + ip.SourceAddress.ToString(), ToolTipIcon.Warning, true);

                        }
                    }
                    else if (ip.Protocol == Kavprot.Packets.IPProtocolType.ICMPV6)
                    {
                        ICMPv6Packet icmp6 = ICMPv6Packet.GetEncapsulated(packet);
                        if (icmp6 != null)
                        {
                            Alert.Attack("Intrusion Detected : Unwanted ICMPv6 Packet", "an intrusion was detected using ICMPv6 from " + ip.SourceAddress.ToString(), ToolTipIcon.Warning, true);

                        }
                    }
                    else if (ip.Protocol == Kavprot.Packets.IPProtocolType.ICMP)
                    {
                        ICMPv4Packet icmp4 = ICMPv4Packet.GetEncapsulated(packet);
                        if (icmp4 != null)
                        {
                            Alert.Attack("Intrusion Detected : Unwanted ICMPv4 Packet", "an intrusion was detected using ICMPv4 from " + ip.SourceAddress.ToString(), ToolTipIcon.Warning, true);

                        }
                    }
                }

            }
            catch
            {

            }
            finally
            {

            }
        }
    
        public static void Stop()
        {
            try
            {
                foreach (var dev in devices)
                {
                    dev.StopCapture();
                }
                Runing = false;
            }
            catch
            {

            }
            finally
            {

            }
        }
        public static void Start()
        {
            try
            {
                foreach (var dev in devices)
                {
                    dev.StartCapture();
                }
                Runing = true;
            }

            catch
            {

            }
            finally
            {

            }
        }
    }
}
