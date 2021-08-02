using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcSigesoft.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Usuario es requerido")]
        [Display(Name = "Usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password es requerido")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public int NodoId { get; set; }
    }

    public class LoginExternoViewModel
    {
        [Required(ErrorMessage = "Usuario es requerido")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Password es requerido")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public int NodoId { get; set; }
    }

    public class LoginDto
    {
        public string v_UserName { get; set; }
        public string v_Password { get; set; }
        public int i_NodoId { get; set; }
    }
}