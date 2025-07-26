using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhoneStore.Areas.Admin.Data;
using WebPhoneStore.EF;

namespace WebPhoneStore.Areas.Admin.Controllers
{
    public class AdminHomeController : BaseController
    {
        private PhoneStoreDbContext db = new PhoneStoreDbContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            var now = DateTime.Now;

            // Lấy toàn bộ đơn hàng (cả đã duyệt và chưa duyệt)
            var allOrders = db.Orders.ToList();
            var approvedOrders = allOrders.Where(o => o.Status == true).ToList();

            var orders = db.Orders.Where(o => o.Status == true).ToList();

            // Doanh thu trong tháng hiện tại
            var monthlyRevenue = orders.Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Month == now.Month && o.OrderDate.Value.Year == now.Year).Sum(o => (decimal?)o.TotalAmount) ?? 0;

            // Doanh thu trong năm hiện tại
            var annualRevenue = orders.Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == now.Year).Sum(o => (decimal?)o.TotalAmount) ?? 0;

            // Tổng doanh thu tất cả thời gian
            var totalRevenue = orders.Sum(o => (decimal?)o.TotalAmount) ?? 0;


            // Đơn hàng
            int totalOrders = allOrders.Count();
            int totalApprovedOrders = approvedOrders.Count;

            int pendingApprovalOrders = allOrders.Count(o => o.Status == false);
            double pendingApprovalPercent = totalOrders > 0 ? (double)pendingApprovalOrders / totalOrders * 100 : 0;
            int pendingDeliveryOrders = approvedOrders.Count(o => !o.Delivered.HasValue || o.Delivered == false);

            //int deliveredOrders = orders.Count(o => o.Delivered.HasValue && o.Delivered.Value);
            //int pendingOrders = orders.Count(o => !o.Delivered.HasValue || o.Delivered == false);
            //double pendingPercent = totalOrders > 0 ? (double)pendingOrders / totalOrders * 100 : 0;
            //double pendingOrderPercent = totalOrders > 0 ? (double)pendingOrders / totalOrders * 100 : 0;

            double taskProgress = approvedOrders.Count > 0 ? (double)approvedOrders
                .Count(o => o.Delivered.HasValue && o.Delivered.Value) / approvedOrders.Count * 100 : 0;

            var categoryStats = db.ProductCategories
                .Select(c => new { c.Name, Count = c.Products.Count })
                .ToDictionary(x => x.Name, x => x.Count);


            var model = new DashboardViewModel
            {
                MonthlyRevenue = monthlyRevenue,
                AnnualRevenue = annualRevenue,
                TotalAmount = totalRevenue,
                TaskProgress = taskProgress,
                PendingRequests = pendingApprovalOrders,
                PendingOrderPercent = pendingApprovalPercent,
                ProductCategoryStats = categoryStats,
                TotalApprovedOrders = totalApprovedOrders
            };

            var revenueByCategory = (from od in db.OrderDetails
                                     join o in db.Orders on od.OrderID equals o.OrderID
                                     where o.Status == true
                                     join p in db.Products on od.ProductID equals p.ProductID
                                     join c in db.ProductCategories on p.CateID equals c.CateID
                                     group new { od, p } by c.Name into g
                                     select new
                                     {
                                         CategoryName = g.Key,
                                         TotalRevenue = g.Sum(x => x.od.Price * x.od.Quantity)
                                     }).ToList();
            var monthlyRevenueData = db.Orders
                .Where(o => o.Status == true && o.OrderDate.HasValue && o.OrderDate.Value.Year == now.Year)
                .GroupBy(o => o.OrderDate.Value.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Revenue = g.Sum(o => o.TotalAmount)
                }).ToList();
                

            // Tạo mảng 12 tháng (nếu tháng nào không có thì để = 0)
            var revenuePerMonth = new decimal[12];
            foreach (var item in monthlyRevenueData)
            {
                revenuePerMonth[item.Month - 1] = item.Revenue;
            }

            // Truyền qua ViewBag
            ViewBag.MonthLabels = Enumerable.Range(1, 12).Select(m => "Tháng " + m).ToList(); // ["Tháng 1", ..., "Tháng 12"]
            ViewBag.MonthRevenue = revenuePerMonth.ToList();

            // Gửi dữ liệu qua ViewBag
            ViewBag.CategoriesRevenue = revenueByCategory.Select(x => x.CategoryName).ToList();
            ViewBag.RevenueValues = revenueByCategory.Select(x => x.TotalRevenue).ToList();

            return View(model);
        }

        public ActionResult RevenueByCategory()
        {
            var db = new PhoneStoreDbContext();

            var data = from od in db.OrderDetails
                       join p in db.Products on od.ProductID equals p.ProductID
                       join c in db.ProductCategories on p.CateID equals c.CateID
                       group new { od, p } by c.Name into g
                       select new
                       {
                           CategoryName = g.Key,
                           TotalRevenue = g.Sum(x => x.od.Price * x.od.Quantity)
                       };

            ViewBag.Categories = data.Select(x => x.CategoryName).ToList();
            ViewBag.Revenues = data.Select(x => x.TotalRevenue).ToList();

            return View();
        }

    }
}