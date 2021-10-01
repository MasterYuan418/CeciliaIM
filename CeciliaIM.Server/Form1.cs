using CeciliaIM.Server.Backend.TypeDefines;
using CeciliaIM.Server.Backend.Utils;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CeciliaIM.Server
{
    public partial class Form1 : Sunny.UI.UIForm
    {
        ServerConfig sc;
        SecretConfig secret_c;
        public Form1()
        {
            InitializeComponent();
            ConfigUtil.InitFile();
            ConfigUtil.InitReadConfig();
            sc = ConfigUtil.ReadServerConfig();
            secret_c = ConfigUtil.ReadServerSecretConfig();
            this.Style = Sunny.UI.UIStyle.Office2010Blue;
            uiLabel3.Text = sc.Port.ToString();
            uiLabel5.Text = sc.AdminName;
            if (sc.IsAutoGetIPAddr)
            {
                LogUtil.WriteVerbose("Auto IPAddress seted.");
                uiCheckBox1.Checked = true;
                sc.IPAddress = NetworkUtil.GetLocalIP();
                LogUtil.WriteVerbose("Local IP: " + sc.IPAddress);
                IPAddrBox.Text = sc.IPAddress;
                IPAddrBox.Enabled = false;
            }
            if (!System.IO.Directory.Exists(ConfigUtil.PROFILE_PATH + "\\" + IPAddrBox.Text))
            {
                System.IO.Directory.CreateDirectory(ConfigUtil.PROFILE_PATH + "\\" + IPAddrBox.Text);
                System.IO.Directory.CreateDirectory(ConfigUtil.PROFILE_PATH + "\\" + IPAddrBox.Text + "\\Users");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            uiLight1.State = Sunny.UI.UILightState.Off;

        }

        private void uiButton2_Click(object sender, EventArgs e) //发送
        {
        }
        protected override void OnClosed(EventArgs e)
        {
            LogUtil.WriteInformation("Bye! OnClosed()");
            LogUtil.Close();
            base.OnClosed(e);
        }

        private void uiCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.uiCheckBox1.Checked == false)
            {
                IPAddrBox.Text = "";
                IPAddrBox.Enabled = true;
            }
        }

        private void uiButton1_Click(object sender, EventArgs e) //Confirm user
        {
            string uname = uiTextBox2.Text;
            string upwd = uiTextBox3.Text;
            if (uname == string.Empty || upwd == string.Empty)
            {
                UIMessageBox.ShowError("用户名或密码为空");
                return;
            }
            UserProfile p = new UserProfile();
            UIWaitForm u = new UIWaitForm();
            u.Text = "正在生成加密符...";
            u.Show();
            p.name = uname;
            p.password = upwd;
            string salt = EncryptionHelper.SHA256(Guid.NewGuid().ToString() + ":" + upwd + ":" + uname).ToUpper();
            p.authString = EncryptionHelper.AES(secret_c.AESPublickey, secret_c.AESIv, "name=" + uname + ",upwd=" + upwd
                + ",salt=" + salt);
            System.IO.File.WriteAllText(ConfigUtil.PROFILE_PATH + "\\" + IPAddrBox.Text + "\\Users\\" + uname + ".prof",
                "name=" + uname + ",upwd=" + upwd
                + ",salt=" + salt + ",auth=" + p.authString);
            u.Close();
            UIMessageBox.ShowInfo("请牢记以下字符串：\n " + salt + "\n已复制到你的剪贴板，该字符串仅会出现一次。");
            LogUtil.WriteInformation("New user generated.Name=" + uname);
            Clipboard.SetDataObject(salt, true);
        }

        private void uiTextBox2_TextChanged(object sender, EventArgs e)
        {
            string t = uiTextBox2.Text;
            if (System.IO.File.Exists(ConfigUtil.PROFILE_PATH + "\\" + IPAddrBox.Text + "\\Users\\" + t + ".prof"))
            {
                //TODO:(自动填充用户)
            }
            else
            {

            }
        }
    }
}
