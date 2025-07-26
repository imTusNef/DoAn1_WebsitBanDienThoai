using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPhoneStore.Models
{
	public class LoginModel
	{
        [Key]
        [Display(Name ="Email")]
        [Required(ErrorMessage ="Vui lòng nhập Email")]
        public string Email { get; set; }
        [Display(Name ="Mật Khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }

    }
}