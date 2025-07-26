using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPhoneStore.Areas.Admin.Data
{
	public class DashboardViewModel
	{
        public decimal MonthlyRevenue { get; set; }
        public decimal AnnualRevenue { get; set; }
        public double TaskProgress { get; set; } // Đơn đã xử lý / Tổng đơn
        public int PendingRequests { get; set; }
        public decimal TotalAmount { get; set; }
        public double PendingOrderPercent { get; set; }
        public int TotalApprovedOrders { get; set; }

        public Dictionary<string, int> ProductCategoryStats { get; set; }
        public List<CategoryRevenue> CategoryRevenues { get; set; }
    }
}