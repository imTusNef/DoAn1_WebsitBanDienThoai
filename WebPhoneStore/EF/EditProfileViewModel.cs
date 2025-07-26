using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPhoneStore.EF
{
	public class EditProfileViewModel
	{

        [Required]
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        // Mật khẩu hiện tại
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        // Mật khẩu mới
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        // Xác nhận mật khẩu
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}