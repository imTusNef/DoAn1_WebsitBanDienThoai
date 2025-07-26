using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.Areas.Admin.Data;
using WebPhoneStore.Common;
using WebPhoneStore.DAO;
using WebPhoneStore.EF;

namespace WebPhoneStore.Controllers
{
    public class ClientUserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        
        [HttpPost]        
        public ActionResult Register(RegisterModel model)
         {
            if (ModelState.IsValid)
            {
                var dao = new User_DAO();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("","Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");   
                }
                else
                {
                    var user = new User();
                    user.UserName = model.UserName;
                    user.Password = model.Password;
                    user.FullName = model.Name;
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.Status = true;
                    var result = dao.Insert(user);
                    if(result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công");
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new User_DAO();
                var result = dao.Login(model.Email, model.Password);
                if (result == 1)
                {
                    var user = dao.getItem(model.Email);
                    var session = new UserLogin();
                    session.UserID = user.UserID;
                    session.Email = user.Email;
                    session.UserName = user.UserName;
                    session.FullName = user.FullName;
                    Session.Add(PhoneStore_Func.USER_SESSION, session);
                    return Redirect("/trang-chu");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản chưa được kích hoạt. Vui lòng Liên hệ admin!");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng. Vui lòng kiểm tra lại");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Email không tồn tại.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Profile()
        {
            // Có thể cần kiểm tra đăng nhập
            var session = (UserLogin)Session[PhoneStore_Func.USER_SESSION];
            if (session == null)
                return RedirectToAction("Login");

            // Lấy thông tin user theo ID hoặc username
            var userDAO = new User_DAO();
            var user = userDAO.GetByUserName(session.UserName);
            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "ClientUser");
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            var session = (UserLogin)Session[PhoneStore_Func.USER_SESSION];
            if (session == null)
                return RedirectToAction("Login");

            int userID = session.UserID;

            using (var db = new PhoneStoreDbContext())
            {
                var user = db.Users.Find(userID);
                if (user == null)
                    return HttpNotFound();

                var model = new EditProfileViewModel
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Address = user.Address
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(EditProfileViewModel model)
        {
            var session = (UserLogin)Session[PhoneStore_Func.USER_SESSION];
            if (!ModelState.IsValid)
                return View(model);

            int userID = session.UserID;
            if (session == null)
                return RedirectToAction("Login");

            using (var db = new PhoneStoreDbContext())
            {
                var user = db.Users.Find(userID);
                if (user == null)
                    return HttpNotFound();

                // Cập nhật thông tin chung
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.Phone = model.Phone;
                user.Address = model.Address;

                // Đổi mật khẩu nếu có
                if (!string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.NewPassword))
                {
                    if (user.Password != model.OldPassword)
                    {
                        ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng.");
                        return View(model);
                    }

                    user.Password = model.NewPassword;
                }

                db.SaveChanges();
            }

            TempData["Success"] = "Cập nhật hồ sơ thành công.";
            return RedirectToAction("Profile");
        }

    }
}