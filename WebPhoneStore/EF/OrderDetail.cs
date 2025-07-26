namespace WebPhoneStore.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        public int OrderDetailID { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public int? OrderID { get; set; }

        public int? ProductID { get; set; }

        [StringLength(250)]
        public string ProductName { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
