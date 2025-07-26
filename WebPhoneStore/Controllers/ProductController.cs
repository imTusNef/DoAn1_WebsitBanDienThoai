using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.DAO;

namespace WebPhoneStore.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Category(string seotitle, int cateid)
        {
            var category = new ProductCategory_DAO().ViewDetail(cateid);
            if(category == null)
            {
                return HttpNotFound();
            }

            ViewBag.Category = category;
            var model = new Product_DAO().ListByCategoryID(cateid);
            return View(model);
        }

        public ActionResult Detail(string SeoTitle,int id)
        {
            var product = new Product_DAO().ViewDetail(id);

            var relatedProducts = new Product_DAO().ListRelatedProduct(id);

            ViewBag.Related = relatedProducts;
            return View(product);
        }
    }
}