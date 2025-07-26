using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.Areas.Admin.Data;
using WebPhoneStore.Common;
using WebPhoneStore.DAO;
using WebPhoneStore.EF;

namespace WebPhoneStore.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new User_DAO();
                var result = dao.Login(model.Email, model.Password);
                if(result == 1)
                {
                    var user = dao.getItem(model.Email);
                    var session = new UserLogin();
                    session.Email = user.Email;
                    session.UserName = user.UserName;
                    session.FullName = user.FullName;
                    Session.Add(PhoneStore_Func.USER_SESSION, session);
                    return RedirectToAction("Index", "AdminHome");
                }
                else if(result == 0)
                {
                    ModelState.AddModelError("","Tài khoản chưa được kích hoạt. Vui lòng Liên hệ admin!");
                }
                else if(result == -1)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng. Vui lòng kiểm tra lại");
                }
                else if(result == -2)
                {
                    ModelState.AddModelError("", "Email không tồn tại.");
                }
            }
            return View("Index");
        }
    }



}