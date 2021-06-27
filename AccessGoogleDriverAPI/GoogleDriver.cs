using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.Web;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace AccessGoogleDriverAPI
{

    public class GoogleDriver
    {
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Google Driver";
        public UserCredential credential;
       static DriveService service;
        public GoogleDriver()
        {
            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }
        }

        public IList<Google.Apis.Drive.v3.Data.File> ListFile()
        {
            try
            {
             
               FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.PageSize = 10;
                listRequest.Fields = "nextPageToken, files(id, name, thumbnailLink)";
                return listRequest.Execute().Files;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }      
        }



        public static void CreateFolderOnDrive(string Folder_Name)
        {
            Google.Apis.Drive.v3.Data.File FileMetaData = new
            Google.Apis.Drive.v3.Data.File();
            FileMetaData.Name = Folder_Name;
            FileMetaData.MimeType = "application/vnd.google-apps.folder";

            FilesResource.CreateRequest request;

            request = service.Files.Create(FileMetaData);
            request.Fields = "id";
            var file = request.Execute();
        }


    }
}
