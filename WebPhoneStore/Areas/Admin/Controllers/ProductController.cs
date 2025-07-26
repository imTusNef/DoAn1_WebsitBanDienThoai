using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.EF;
using WebPhoneStore.DAO;
using System.Drawing.Printing;
using System.Web.UI;
using PagedList;
using System.Net;

namespace WebPhoneStore.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index(string searchString, int page = 1, int pagesize = 5)
        {
            var dao = new Product_DAO();
            var model = dao.ListAllPaging(searchString, page, pagesize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        public void SetViewBag(int? selectedId = null)
        {
            var dao = new ProductCategory_DAO();
            ViewBag.CateID = new SelectList(dao.ListAll(), "CateID", "Name", selectedId);
        }

        //Hàm sửa sản phẩm
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = new Product_DAO().ViewDetail(id);
            if (product == null)
                return HttpNotFound();

            SetViewBag(product.CateID);
            return View(product);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new Product_DAO();
                var result = dao.Update(product);
                if (result)
                {
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật sản phẩm thành công");
                }

            }
            return View(product);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new Product_DAO();
                long id = dao.Insert(product);
                if (id > 0)
                {
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm sản phẩm thành công");
                }

            }
            return View(product);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new Product_DAO().Delete(id);
            return RedirectToAction("Index");
        }

    }
}