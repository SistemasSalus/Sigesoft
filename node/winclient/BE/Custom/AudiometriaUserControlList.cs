using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class AudiometriaUserControlList
    {
        public string VA_OD_125 { get; set; }
        public string VA_OD_250 { get; set; }
        public string VA_OD_500 { get; set; }
        public string VA_OD_1000 { get; set; }
        public string VA_OD_2000 { get; set; }
        public string VA_OD_3000 { get; set; }
        public string VA_OD_4000 { get; set; }
        public string VA_OD_6000 { get; set; }
        public string VA_OD_8000 { get; set; }

        public string VO_OD_125 { get; set; }
        public string VO_OD_250 { get; set; }
        public string VO_OD_500 { get; set; }
        public string VO_OD_1000 { get; set; }
        public string VO_OD_2000 { get; set; }
        public string VO_OD_3000 { get; set; }
        public string VO_OD_4000 { get; set; }
        public string VO_OD_6000 { get; set; }
        public string VO_OD_8000 { get; set; }

        public string VA_OI_125 { get; set; }
        public string VA_OI_250 { get; set; }
        public string VA_OI_500 { get; set; }
        public string VA_OI_1000 { get; set; }
        public string VA_OI_2000 { get; set; }
        public string VA_OI_3000 { get; set; }
        public string VA_OI_4000 { get; set; }
        public string VA_OI_6000 { get; set; }
        public string VA_OI_8000 { get; set; }

        public string VO_OI_125 { get; set; }
        public string VO_OI_250 { get; set; }
        public string VO_OI_500 { get; set; }
        public string VO_OI_1000 { get; set; }
        public string VO_OI_2000 { get; set; }
        public string VO_OI_3000 { get; set; }
        public string VO_OI_4000 { get; set; }
        public string VO_OI_6000 { get; set; }
        public string VO_OI_8000 { get; set; }

        public string VA_OD_chk { get; set; }
        public string VA_OI_chk { get; set; }

        // AGVR nuevos campos
        //public string txt_VA_OD_125 { get; set; }
        //public string txt_VA_OD_250 { get; set; }
        //public string txt_VO_OD_125 { get; set; }
        //public string txt_VO_OD_250 { get; set; }
        //public string txt_VA_OI_125 { get; set; }
        //public string txt_VA_OI_250 { get; set; }
        //public string txt_VO_OI_125 { get; set; }
        //public string txt_VO_OI_250 { get; set; }

        public byte[] b_AudiogramaOD { get; set; }
        public byte[] b_AudiogramaOI { get; set; }
    }
}
