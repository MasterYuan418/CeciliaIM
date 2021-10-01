using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CeciliaIM.Server.Backend.TypeDefines;

namespace CeciliaIM.Server.Backend.Utils
{
    class ConfigUtil
    {
        public const string CONFIG_PATH = @".\Configs";
        public const string PROFILE_PATH = @".\Profiles";
        public const string TEMP_PATH = @".\Temp";
        public const string DATA_PATH = @".\Data";
        public const string DATA_LOGS_PATH = @".\Data\Logs";
        private const string Init_ServerEECFG_Data =
            "servername myserver^abc\n" +
            "ip auto\n" +
            "port 30683\n" +
            "sslpfx .\\default.pfx\n" +
            "pfxpassword 123abc\n" +
            "adminname Admin\n";
        private static string Init_ServerSecertCFG_Data =
            "aes_publickey 70928823819479621044526729358297\n" +
            "aes_iv 0ZW5O7jkcw3kNwTx";
        private static string[] Config_Server;
        private static string[] Config_Secret;
        public static void InitFile()
        {
            if (!Directory.Exists(@".\Configs")) Directory.CreateDirectory(@".\Configs");
            if (!Directory.Exists(@".\Profiles")) Directory.CreateDirectory(@".\Profiles");
            if (!Directory.Exists(@".\Temp")) Directory.CreateDirectory(@".\Temp");
            if (!Directory.Exists(@".\Data")) Directory.CreateDirectory(@".\Data");
            if (!Directory.Exists(DATA_LOGS_PATH)) Directory.CreateDirectory(DATA_LOGS_PATH);
            if (!File.Exists(@".\Configs\server.eecfg")) File.WriteAllText(@".\Configs\server.eecfg",Init_ServerEECFG_Data);
            if (!File.Exists(@".\Configs\secret.eecfg")) File.WriteAllText(@".\Configs\secret.eecfg", Init_ServerSecertCFG_Data);
            LogUtil.WriteVerbose("Config.initfile module init successful.");
        }
        public static void InitReadConfig()
        {
            string[] _ = File.ReadAllLines(CONFIG_PATH + "\\server.eecfg");
            string[] __ = File.ReadAllLines(CONFIG_PATH + "\\secret.eecfg");
            Config_Server = new string[_.Length];
            Config_Server = _;
            _ = null;
            Config_Secret = new string[__.Length];
            Config_Secret = __;
            __ = null;
            LogUtil.WriteInformation("Read config done.");
        }
        public static ServerConfig ReadServerConfig()
        {
            ServerConfig sc = new ServerConfig();
            for (int i = 0;i < Config_Server.Length;i++)
            {
                if (Config_Server[i].Split(' ')[0] == "servername")
                {
                    sc.ServerName = Config_Server[i].Split(' ')[1].Replace('^',' ');
                }
                else if (Config_Server[i].Split(' ')[0] == "ip")
                {
                    sc.IPAddress = Config_Server[i].Split(' ')[1];
                    if (sc.IPAddress == "auto") sc.IsAutoGetIPAddr = true;
                }
                else if (Config_Server[i].Split(' ')[0] == "port")
                {
                    sc.Port = Convert.ToInt32(Config_Server[i].Split(' ')[1]);
                }
                else if (Config_Server[i].Split(' ')[0] == "sslpfx")
                {
                    if (!File.Exists(Config_Server[i].Split(' ')[1]))
                    {
                        LogUtil.WriteWarning("Cert not found.");
                    }
                    sc.SSLPFXCertPath = Config_Server[i].Split(' ')[1];
                }
                else if (Config_Server[i].Split(' ')[0] == "pfxpassword")
                {
                    sc.SSLPFXPassword = Config_Server[i].Split(' ')[1];
                }
                else if (Config_Server[i].Split(' ')[0] == "adminname")
                {
                    sc.AdminName = Config_Server[i].Split(' ')[1];
                }
            }
            LogUtil.WriteInformation("Read server-config OK.");
            LogUtil.WriteInformation("Server config: s.sn=" + sc.ServerName + ",s.ipa=" + sc.IPAddress + ",s.iagipa=" + sc.IsAutoGetIPAddr
                + ",s.p=" + sc.Port + ",s.spfxpth=" + sc.SSLPFXCertPath + ",s.pfxpwd_md5=" + EncryptionHelper.MD5(sc.SSLPFXPassword)
                + ",s.admin=" + sc.AdminName);
            return sc;
        }
        public static SecretConfig ReadServerSecretConfig()
        {
            SecretConfig sc = new SecretConfig();
            for (int i = 0; i < Config_Secret.Length; i++)
            {
                if (Config_Secret[i].Split(' ')[0] == "aes_publickey")
                {
                    sc.AESPublickey = Config_Secret[i].Split(' ')[1];
                    if (sc.AESPublickey.Length != 32)
                    {
                        LogUtil.WriteError("AES PublicKey length error.Must equals 32");
                        throw new Exception();
                    }
                }
                if (Config_Secret[i].Split(' ')[0] == "aes_iv")
                {
                    sc.AESIv = Config_Secret[i].Split(' ')[1];
                }
            }
            LogUtil.WriteInformation("Secret config read done.");
            return sc;
        }
    }
}
