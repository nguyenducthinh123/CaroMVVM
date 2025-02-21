using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace System
{
    public static class NetworkHelper
    {
        /// <summary>
        /// Kiểm tra xem có bất kỳ giao diện mạng nào đang hoạt động không.
        /// </summary>
        public static bool HasNetworkInterface()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Any(ni => ni.OperationalStatus == OperationalStatus.Up &&
                           ni.NetworkInterfaceType != NetworkInterfaceType.Loopback);
        }

        /// <summary>
        /// Kiểm tra xem có kết nối Internet thực sự không bằng cách gửi Ping đến Google DNS.
        /// </summary>
        public static async Task<bool> HasInternetAccessAsync()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = await ping.SendPingAsync("8.8.8.8", 3000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool HasInternetAccess => Task.Run(() => HasInternetAccessAsync()).Result;
    }
}

