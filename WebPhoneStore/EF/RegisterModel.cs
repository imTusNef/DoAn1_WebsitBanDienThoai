using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPhoneStore.EF
{
	public class RegisterModel
	{
        [Key]
        public int ID { get; set; } 
        [Display(Name="Tên Đăng Nhập")]
        [Required(ErrorMessage ="Vui lòng nhập tên đăng nhập")]
        public string UserName { get; set; }
        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage ="Vui lòng nhập mật khẩu")]
        [StringLength(20,MinimumLength =6,ErrorMessage ="Độ dài cần đủ 6 ký tự")]
        public string Password { get; set; }
        [Display(Name = "Xác Nhận Mật Khẩu")]
        [Required(ErrorMessage ="Vui lòng nhập lại mật khẩu")]
        [Compare("Password",ErrorMessage ="Mật khẩu không trùng nhau")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Họ Tên")]
        public string Name { get; set; }
        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }

        [Display(Name="Email")]
        [Required(ErrorMessage ="Vui lòng nhập email")]
        public string Email { get; set; }
        [Display(Name = "Số Điện Thoại")]
        public string Phone { get; set; }
    }
}