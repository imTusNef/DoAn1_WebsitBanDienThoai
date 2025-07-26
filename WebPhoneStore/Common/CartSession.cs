using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhoneStore.EF;
namespace WebPhoneStore.Common
{
    [Serializable]
    public class CartSession
	{
        public int CartID { get; set; }
        public int UserID { get; set; }
        public List<CartDetail> Items { get; set; }
    }
}