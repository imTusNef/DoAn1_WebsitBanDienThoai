using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoneStore.EF;

namespace WebPhoneStore.DAO
{ 
    public class ProductCategory_DAO
	{
        PhoneStoreDbContext db = null;
        public ProductCategory_DAO()
        {
            db = new PhoneStoreDbContext();
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.Sort ?? int.MaxValue).ToList();
        }

        public ProductCategory ViewDetail(int id)
        {
            return db.ProductCategories.Find(id);
        }


    }
}