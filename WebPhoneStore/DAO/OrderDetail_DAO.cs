using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoneStore.EF;

namespace WebPhoneStore.DAO
{
	public class OrderDetail_DAO
	{
        PhoneStoreDbContext db = null;
        public OrderDetail_DAO()
        {
            db = new PhoneStoreDbContext();
        }
        public bool Inser(OrderDetail detail)
        {
            try
            {
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}