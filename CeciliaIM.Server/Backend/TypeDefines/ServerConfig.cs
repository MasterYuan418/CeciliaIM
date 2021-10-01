using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeciliaIM.Server.Backend.TypeDefines
{
    public class ServerConfig
    {
        public int Port;
        public string AdminName;
        public string ServerName;
        public string IPAddress;
        public bool IsAutoGetIPAddr = false;
        public string SSLPFXCertPath;
        public string SSLPFXPassword;
    }
}
