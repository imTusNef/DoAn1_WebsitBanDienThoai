using PagedList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.DAO;
using WebPhoneStore.EF;

namespace WebPhoneStore.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index(string searchString,int page = 1, int pagesize = 5)
        {
            var dao = new User_DAO();
            var model = dao.ListAllPaging(searchString,page, pagesize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var user = new User_DAO().ViewDetail(id);
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new User_DAO();
                var result = dao.Update(user);
                if (result)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật user thành công");
                }

            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new User_DAO();
                long id = dao.Insert(user);
                if (id > 0) 
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user thành công");
                }

            }
            return View(user);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new User_DAO().Delete(id);
            return RedirectToAction("Index");
        }

    }
}