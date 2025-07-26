using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using WebPhoneStore.EF;
using System.Data.Entity;

namespace WebPhoneStore.DAO
{
	public class Order_DAO
	{
		PhoneStoreDbContext db = null;
		public Order_DAO()
		{
			db = new PhoneStoreDbContext();
		}

        public List<Order> GetOrdersByUser(int userId)
        {
            return db.Orders.Where(o => o.UserID == userId).OrderByDescending(o => o.OrderDate).ToList();
        }

        public int Insert(Order order)
		{
			db.Orders.Add(order);
			db.SaveChanges();
			return order.OrderID; 
		}

        public void Delete(int id)
        {
            //using (var db = new PhoneStoreDbContext())
            //{
            //    var order = db.Orders.Find(id);
            //    if (order != null)
            //    {
            //        // Xóa chi tiết đơn hàng trước
            //        var orderDetails = db.OrderDetails.Where(d => d.OrderID == id).ToList();
            //        db.OrderDetails.RemoveRange(orderDetails);

            //        // Sau đó xóa đơn hàng
            //        db.Orders.Remove(order);

            //        db.SaveChanges();
            //    }
            //}
            var order = db.Orders.Find(id);
            if (order != null)
            {
                // Xóa tất cả OrderDetail liên quan
                var details = db.OrderDetails.Where(od => od.OrderID == id).ToList();
                db.OrderDetails.RemoveRange(details);

                // Xóa đơn hàng
                db.Orders.Remove(order);

                db.SaveChanges();
            }
        }
        public void UpdateStatus(int orderId, bool status)
        {
            using (var db = new PhoneStoreDbContext())
            {
                var order = db.Orders.Find(orderId);
                if (order != null)
                {
                    order.Status = status;
                    db.SaveChanges();
                }
            }
        }

        public List<Order> GetPendingOrders()
        {
                return db.Orders.Where(o => o.Status == false).Include(o => o.OrderDetails.Select(od => od.Product))
                                .OrderByDescending(o => o.OrderDate).ToList(); // <-- dòng thêm
        }

    }
}