using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RTC.Common
{
    internal class IpFun
    {
        public static bool SetNetworkAdapter(string ipAddress, bool rdoAutoSetIP, string subnetMask = null, string gateway = null)
        {
            IPAddress ethernetIPAddress = GetEthernetIPAddress();
            ManagementBaseObject inPar = null;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    continue;
                if (((string[])mo["IPAddress"])[0] == ethernetIPAddress.ToString())
                {
                    if (rdoAutoSetIP)
                    {
                        mo.InvokeMethod("EnableStatic", null);
                        //重置DNS为空
                        mo.InvokeMethod("SetDNSServerSearchOrder", null);
                        //开启DHCP
                        mo.InvokeMethod("EnableDHCP", null);
                        return true;
                    }
                    else
                    {
                        inPar = mo.GetMethodParameters("EnableStatic");
                        //设置ip地址和子网掩码
                        inPar["IPAddress"] = new string[] { ipAddress };
                        inPar["SubnetMask"] = new string[] { subnetMask };

                        //设置网关地址
                        if (gateway != null)
                        {
                            inPar = mo.GetMethodParameters("SetGateways");
                            inPar["DefaultIPGateway"] = new string[] { gateway };
                            mo.InvokeMethod("SetGateways", inPar, null);
                        }
                    }


                    ManagementBaseObject put = mo.InvokeMethod("EnableStatic", inPar, null);
                    string str = put["returnvalue"].ToString();
                    return (str == "0" || str == "1") ? true : false;

                }
            }
            return false;
        }

        //查找以太网ip
        private static IPAddress GetEthernetIPAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet && adapter.Name == "以太网")
                {
                    foreach (var item in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (item.Address.AddressFamily == AddressFamily.InterNetwork)
                            return item.Address;            //item.IPv4Mask获取掩码
                    }
                }
                //adapter.GetIPProperties().GatewayAddresses获取网关
            }
            throw new Exception("Ethernet not connected");
        }
    }
}
