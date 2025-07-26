using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.DAO;
using WebPhoneStore.EF;
using System.Data.Entity;

namespace WebPhoneStore.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private PhoneStoreDbContext db = new PhoneStoreDbContext();
        // GET: Admin/Order
        public ActionResult Index()
        {
            var orders = db.Orders
                .Where(o => o.Status == true)
                .Include(o => o.OrderDetails.Select(od => od.Product))
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            return View(orders);
        }

        public ActionResult OrderDetail()
        {
            var orders = new Order_DAO().GetPendingOrders(); // Lấy danh sách đơn chờ duyệt
            return View(orders);
        }

        public ActionResult Approve(int id)
        {
            new Order_DAO().UpdateStatus(id, true); // 1 = Đã duyệt
            return RedirectToAction("OrderDetail");
        }

        public ActionResult Reject(int id)
        {
            new Order_DAO().Delete(id); //
            return RedirectToAction("OrderDetail");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new Order_DAO().Delete(id);
            return RedirectToAction("Index");
        }
    }
}