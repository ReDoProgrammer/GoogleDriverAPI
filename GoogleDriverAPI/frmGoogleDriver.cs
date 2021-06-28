using AccessGoogleDriverAPI;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleDriverAPI
{
    public partial class frmGoogleDriver : Form
    {
        public UserCredential credential;
        public frmGoogleDriver()
        {
            InitializeComponent();
        }

        private void GoogleDriver_Load(object sender, EventArgs e)
        {
           var gd = new GoogleDriver();
            var lst = gd.ListFile();
            var files = gd.GetDriveFiles();
            var root = gd.GetRootFolder();
            var children = gd.GetChildrenFiles(root.Id);
        }

        private void ptbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
