using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using WebPhoneStore.EF;
using PagedList;
using System.Web.Mvc;

namespace WebPhoneStore.DAO
{
	public class Product_DAO
	{
		PhoneStoreDbContext db = null;

		public Product_DAO()
        {
            db = new PhoneStoreDbContext();
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.ToList();
        }
        public Product getItem(string seotitle)     
        {
            return db.Products.FirstOrDefault(x => x.SeoTitle == seotitle);
        }

        //Hàm hiển thị sản phẩm
        public Product ViewDetail(int id)
        {
            return db.Products.Find(id);
        }

        //Hiển thị sản phẩm
        public IPagedList<Product> ListAllPaging(string searchString, int page, int pagesize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderBy(u => u.ProductID).ToPagedList(page, pagesize);
        }

        //Thêm sản phẩm
        public long Insert(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
            return product.ProductID;
        }

        //Cập nhật sản phẩm
        public bool Update(Product product)
        {
            try
            {
                var pd = db.Products.Find(product.ProductID);
                pd.Name = product.Name;
                pd.SeoTitle = product.SeoTitle;
                pd.Status = product.Status;
                pd.Image = product.Image;
                pd.Price = product.Price;
                pd.Quantity = product.Quantity;
                pd.Description = product.Description;
                pd.Detail = product.Detail;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Xóa
        public bool Delete(int id)
        {
            try
            {
                var pd = db.Products.Find(id);
                db.Products.Remove(pd);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

       //Giao diện cho client
        public List<Product> ListIphoneProduct(int productid)
        {
            return db.Products.Where(x => x.SeoTitle.Contains("iphone")).OrderBy(x => x.ProductID).Take(productid).ToList();
        }
        public List<Product> ListSamsungProduct(int productid)
        {
            return db.Products.Where(x => x.SeoTitle.Contains("samsung")).OrderBy(x => x.ProductID).Take(productid).ToList();
        }
        public List<Product> ListOppoProduct(int productid)
        {
            return db.Products.Where(x => x.SeoTitle.Contains("oppo")).OrderBy(x => x.ProductID).Take(productid).ToList();
        }
        public List<Product> ListXiaomiProduct(int productid)
        {
            return db.Products.Where(x => x.SeoTitle.Contains("xiaomi")).OrderBy(x => x.ProductID).Take(productid).ToList();
        }

        public List<Product> ListRelatedProduct(int productid)
        {
            var currentProduct = db.Products.Find(productid);

            if (currentProduct == null)
                return new List<Product>();

            return db.Products
                     .Where(x => x.ProductID != productid && x.CateID == currentProduct.CateID)
                     .OrderByDescending(x => x.CreatedDate)
                     .Take(4) // lấy 4 sản phẩm liên quan gần đây
                     .ToList();
        }

        public List<Product> ListByCategoryID(int cateid)
        {
            return db.Products.Where(x => x.CateID == cateid).ToList();
        }
    }
}