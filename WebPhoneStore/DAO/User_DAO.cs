using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using WebPhoneStore.EF;
using PagedList;

namespace WebPhoneStore.DAO
{
	public class User_DAO
	{
        PhoneStoreDbContext db = null;


        public User_DAO()
        {
            db = new PhoneStoreDbContext();
        }

        public User getItem(string email)
        {
            return db.Users.FirstOrDefault(x => x.Email == email);
        }

        public List<User> getList()
        {
            return db.Users.ToList();
        }
            
        //Hàm hiển thị user
        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }

        //Trả về danh sách user
        public IPagedList<User> ListAllPaging(string searchString, int page, int pagesize)
        {
            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.FullName.Contains(searchString));
            }
            return model.OrderBy(u => u.UserID).ToPagedList(page, pagesize);
        }
        //Thêm user
        public long Insert(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user.UserID;
        }
        //Cập nhật user
        public bool Update(User entity)
        {
            try
            {
                var us = db.Users.Find(entity.UserID);
                us.Password = entity.Password;
                us.FullName = entity.FullName;
                us.Email = entity.Email;
                us.Phone = entity.Phone;
                us.Address = entity.Address;
                us.Status = entity.Status;
                us.UpdatedBy = entity.UpdatedBy;
                us.UpdatedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Đăng nhập
        public int Login(string email, string pass)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return -2; //Email không tồn tại
            }
            else
            {
                if(user.Status == false)
                {
                    return 0; // Vô hiệu hóa
                }
                else
                {
                    if(user.Password == pass)
                    {
                        return 1; //Đăng nhập thành công
                    }
                    else
                    {
                        return -1; //Sai mật khẩu
                    }
                }
            }
        }
        //Xóa
        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }  
        }

        public bool CheckUserName(string username)
        {
            return db.Users.Count(x => x.UserName == username) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }

        // Lấy user theo username
        public User GetByUserName(string username)
        {
            return db.Users.FirstOrDefault(x => x.UserName == username);
        }

        public bool UpdateProfile(User updatedUser, string oldPassword = null, string newPassword = null)
        {
            using (var db = new PhoneStoreDbContext())
            {
                var user = db.Users.Find(updatedUser.UserID);
                if (user == null) return false;

                // Cập nhật thông tin chung
                user.FullName = updatedUser.FullName;
                user.Email = updatedUser.Email;
                user.Phone = updatedUser.Phone;
                user.Address = updatedUser.Address;

                // Kiểm tra và đổi mật khẩu nếu được yêu cầu
                if (!string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(newPassword))
                {
                    if (user.Password != oldPassword)
                        return false; // mật khẩu cũ không đúng

                    user.Password = newPassword;
                }

                db.SaveChanges();
                return true;
            }
        }

    }
}