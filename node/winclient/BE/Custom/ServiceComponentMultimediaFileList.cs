using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ServiceComponentMultimediaFileList
    {
        public string v_ServiceComponentMultimediaId { get; set; }
        public string v_ServiceComponentId { get; set; }
        public string v_MultimediaFileId { get; set; }
        public string v_Comment { get; set; }
        public string v_PersonFullName{ get; set; }
        public string v_DocNumber { get; set; }
        public DateTime? d_ServiceDate { get; set; }
        public string v_ComponentName { get; set; }

        public string v_PersonId { get; set; }
        public string v_FileName { get; set; }
        public byte[] b_File { get; set; }
    }
}
