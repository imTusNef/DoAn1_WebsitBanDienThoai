using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.Common;
using WebPhoneStore.DAO;
using WebPhoneStore.EF;

namespace WebPhoneStore.Controllers
{
    public class CartController : Controller
    {
        //private const string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[PhoneStore_Func.CART_SESSION];
            var list = new List<CartDetail>();
            if(cart != null)
            {
                list = (List<CartDetail>)cart;
            }
            return View(list);
        }

        public ActionResult AddItem(int id, int quantity)
        {
            var db = new PhoneStoreDbContext();
            var product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            // Tạo item
            var item = new CartDetail
            {
                ProductID = product.ProductID,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1,
                Product = product,
                Total = product.Price
            };

            // Thêm vào giỏ hàng
            var cart = Session[PhoneStore_Func.CART_SESSION] as List<CartDetail> ?? new List<CartDetail>();
            var existing = cart.FirstOrDefault(x => x.ProductID == id);
            if (existing != null)
            {
                existing.Quantity += 1;
                existing.Total = existing.Quantity * existing.Price;
            }
            else
            {
                cart.Add(item);
            }

            Session[PhoneStore_Func.CART_SESSION] = cart;

            return RedirectToAction("Index"); // Hiển thị giỏ hàng
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var sessionCart = (List<CartDetail>)Session[PhoneStore_Func.CART_SESSION];
            if (sessionCart != null)
            {
                var item = sessionCart.FirstOrDefault(x => x.ProductID == id);
                if (item != null)
                {
                    sessionCart.Remove(item);
                    Session[PhoneStore_Func.CART_SESSION] = sessionCart;
                }
            }

            return Json(new { status = true });
        }

        [HttpGet]
        public ActionResult Payment()
        {

            var cart = Session[PhoneStore_Func.CART_SESSION];

            // Kiểm tra nếu chưa đăng nhập thì chuyển về trang Login
            if (Session[PhoneStore_Func.USER_SESSION] == null)
            {
                return RedirectToAction("Login", "ClientUser");
            }

            var list = new List<CartDetail>();
            if (cart != null)
            {
                list = (List<CartDetail>)cart;
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult Payment(string shipname, string address, string phone, string email)
        {
            // Kiểm tra nếu chưa đăng nhập thì chuyển về trang Login
            if (Session[PhoneStore_Func.USER_SESSION] == null)
            {
                return RedirectToAction("Login", "ClientUser");
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                ShipAddress = address,
                Phone = phone,
                Name = shipname,
                Email = email,
                Status = false // Chờ duyệt
            };

            try
            {
                var cart = (List<CartDetail>)Session[PhoneStore_Func.CART_SESSION];
                if (cart == null || cart.Count == 0)
                {
                    return RedirectToAction("Index"); // Nếu không có gì trong giỏ thì quay lại
                }

                // Tính tổng tiền
                decimal totalAmount = cart.Sum(item => (decimal)((item.Product.Price ?? 0) * item.Quantity));
                order.TotalAmount = totalAmount;

                // Thêm order
                var id = new Order_DAO().Insert(order);

                // Thêm chi tiết order
                var detailDAO = new OrderDetail_DAO();
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductID = item.Product.ProductID,
                        OrderID = id,
                        Price = item.Product.Price,
                        Quantity = item.Quantity
                    };
                    detailDAO.Inser(orderDetail);
                }

                Session[PhoneStore_Func.CART_SESSION] = null;
            }
            catch
            {
            }

            return Redirect("/hoan-thanh");
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult MyOrders()
        {
            var session = (CartSession)Session[PhoneStore_Func.CART_SESSION];
            if (session == null)
                return RedirectToAction("Login", "ClientUser");

            var dao = new Order_DAO();
            var orders = dao.GetOrdersByUser(session.CartID);
            return View(orders);
        }

    }
}