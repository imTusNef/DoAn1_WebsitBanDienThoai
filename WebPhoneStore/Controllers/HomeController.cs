using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.Common;
using WebPhoneStore.DAO;
using WebPhoneStore.EF;
using static System.Collections.Specialized.BitVector32;

namespace WebPhoneStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var productdao = new Product_DAO();
            ViewBag.ListIphoneProduct = productdao.ListIphoneProduct(4);
            ViewBag.ListSamsungProduct = productdao.ListSamsungProduct(4);
            ViewBag.ListOppoProduct = productdao.ListOppoProduct(4);
            ViewBag.ListXiaomiProduct = productdao.ListXiaomiProduct(4);
            return View();
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var model = new ProductCategory_DAO().ListAll();
            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[PhoneStore_Func.CART_SESSION];
            var list = new List<CartDetail>();
            if (cart != null)
            {
                list = (List<CartDetail>)cart;
            }
            return PartialView(list);
        }

        [ChildActionOnly]
        public PartialViewResult _User()
        {
            var session = (UserLogin)Session[PhoneStore_Func.USER_SESSION];
            if (session != null)
            {
                var dao = new User_DAO();
                var user = dao.getItem(session.Email);
                return PartialView(user);
            }
            return PartialView();
        }

    }
}