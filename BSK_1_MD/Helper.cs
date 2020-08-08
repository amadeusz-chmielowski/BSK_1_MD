using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BSK_1_MD
{
    class Helper
    {
        public static System.Net.IPAddress[] GetAllLocalIPv4(NetworkInterfaceType _type)
        {
            List<System.Net.IPAddress> ipAddrList = new List<System.Net.IPAddress>();
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddrList.Add(ip.Address);
                        }
                    }
                }
            }
            return ipAddrList.ToArray();
        }
    }
}
