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
        const int PAGE_SIZE = 10;
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


        public Google.Apis.Drive.v3.Data.File GetRootFolder()
        {
            return service.Files.Get("root").Execute();
        }

        public List<GoogleDriveFile> GetChildrenFolders(string folderId)
        {
            List<GoogleDriveFile> folderList = new List<GoogleDriveFile>();

            FilesResource.ListRequest request = service.Files.List();
           
            request.Q = string.Format("mimeType='application/vnd.google-apps.folder' and '{0}' in parents", folderId);
            request.Fields = "files(id, name)";

            Google.Apis.Drive.v3.Data.FileList result = request.Execute();
            foreach (var file in result.Files)
            {
                GoogleDriveFile googleDriveFile = new GoogleDriveFile
                {
                    Id = file.Id,
                    Name = file.Name,
                    Size = file.Size,
                    Version = file.Version,
                    CreatedTime = file.CreatedTime,
                    Parents = file.Parents
                };
                folderList.Add(googleDriveFile);
            }
            return folderList;
        }

        public List<GoogleDriveFile> GetChildrenFilesAndFolders(string folderId)
        {
            List<GoogleDriveFile> folderList = new List<GoogleDriveFile>();

            FilesResource.ListRequest request = service.Files.List();

            request.Q = string.Format("'{0}' in parents", folderId);
            request.Fields = "files(id, name, thumbnailLink, mimeType)";

            Google.Apis.Drive.v3.Data.FileList result = request.Execute();
            foreach (var file in result.Files)
            {
                GoogleDriveFile googleDriveFile = new GoogleDriveFile
                {
                    Id = file.Id,
                    Name = file.Name,
                    Size = file.Size,
                    Version = file.Version,
                    CreatedTime = file.CreatedTime,
                    Parents = file.Parents,
                    MimeType = file.MimeType,
                    Thumbnail = file.ThumbnailLink
                };
                folderList.Add(googleDriveFile);
            }
            return folderList;
        }


        public IList<Google.Apis.Drive.v3.Data.File> ListFile()
        {
            try
            {
             
               FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.PageSize = PAGE_SIZE;
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

        //get all files from Google Drive.    
        public List<GoogleDriveFile> GetDriveFiles()
        {
   
            // Define parameters of request.    
            Google.Apis.Drive.v3.FilesResource.ListRequest FileListRequest = service.Files.List();
            // for getting folders only.    
            //FileListRequest.Q = "mimeType='application/vnd.google-apps.folder'";    
            FileListRequest.Fields = "nextPageToken, files(*)";

            // List files.    
            IList<Google.Apis.Drive.v3.Data.File> files = FileListRequest.Execute().Files;
            List<GoogleDriveFile> FileList = new List<GoogleDriveFile>();


            // For getting only folders    
            // files = files.Where(x => x.MimeType == "application/vnd.google-apps.folder").ToList();    


            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    GoogleDriveFile File = new GoogleDriveFile
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Size = file.Size,
                        Version = file.Version,
                        CreatedTime = file.CreatedTime,
                        Parents = file.Parents,
                        MimeType = file.MimeType,
                        Thumbnail = file.ThumbnailLink
                    };
                    FileList.Add(File);
                }
            }
            return FileList;
        }


    }
}
