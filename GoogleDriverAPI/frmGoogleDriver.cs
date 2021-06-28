using AccessGoogleDriverAPI;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleDriverAPI
{
    public partial class frmGoogleDriver : Form
    {
        public UserCredential credential;
        private GoogleDriver gd;
        public frmGoogleDriver()
        {
            InitializeComponent();
        }

        private void GoogleDriver_Load(object sender, EventArgs e)
        {
            gd = new GoogleDriver();
            var lst = gd.ListFile();
            var files = gd.GetDriveFiles();
            var root = gd.GetRootFolder();
            var rootNode = new TreeNode()
            {
                Text = root.Name,
                Tag = root.Id
            };
            AddTreeNode(rootNode);
            tvFolders.Nodes.Add(rootNode);


        }

        private void ptbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
       
        private void tvFolders_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedNode = tvFolders.SelectedNode;
                txtFilePath.Text = selectedNode.FullPath;
                var files = gd.GetChildrenFilesAndFolders(selectedNode.Tag.ToString());
                BindingListView(files);

            }
            catch (Exception ex)
            {

                var t = ex.Message;
            }
        }



        private void BindingListView(List<GoogleDriveFile> files)
        {
            lsvFile.Items.Clear();


            foreach (var file in files)
            {
                try
                {
                    if (file.Thumbnail != null)
                    {
                        var request = WebRequest.Create(file.Thumbnail);

                        using (var response = request.GetResponse())
                        using (var stream = response.GetResponseStream())
                        {
                            imglFiles.Images.Add(Bitmap.FromStream(stream));
                        }

                       
                    }
                }
                catch(Exception ex)
                {
                    var log = ex.Message;
                    
                }
            }
            lsvFile.View = View.LargeIcon;
            imglFiles.ImageSize = new Size(128, 128);
            this.lsvFile.LargeImageList = imglFiles;





           
        }


        private void AddTreeNode(TreeNode parent)
        {            
            var children = gd.GetChildrenFolders(parent.Tag.ToString());
            foreach (var child in children)
            {
                var node = new TreeNode()
                {
                    Text = child.Name,
                    Tag = child.Id
                };
                parent.Nodes.Add(node);
                AddTreeNode(node);
            }

            

        }

        private Image LoadImage(string url)
        {
            url = url.Replace('"', ' ').Trim();
            System.Net.WebRequest request =
                System.Net.WebRequest.Create(url);

            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream =
                response.GetResponseStream();

            Bitmap bmp = new Bitmap(responseStream);

            responseStream.Dispose();

            return bmp;
        }

    }
}
