using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigesoft.Common
{
    public class Option
    {
        public int i_OptionId { get; set; }
        public int? i_ModuleId { get; set; }
        public int? i_OptionTypeId { get; set; }
        public string v_OptionDescription { get; set; }
        public string v_Form { get; set; }
        public int? i_OptionParentId { get; set; }
        public string v_Controller { get; set; }
        public string v_Action { get; set; }
        public string v_ImageClassName { get; set; }
                   
        public int? i_Status { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }

        public int i_AccessId { get; set; }
        public int i_RolId { get; set; }

        public bool b_IsSelected{ get; set; }
        public string v_Module { get; set; }
        
    }
}
