using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPhoneStore.Areas.Admin.Data
{
	public class CategoryRevenue
	{
        public string CategoryName { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}