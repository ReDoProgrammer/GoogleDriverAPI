using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessGoogleDriverAPI
{
    public class GoogleDriveFile
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public long? Size { get; set; }
        public long? Version { get; set; }
        public DateTime? CreatedTime { get; set; }
        public IList<String> Parents { get; set; }
        public String MimeType { get; set; }
        public String Thumbnail { get; set; }
    }
}
