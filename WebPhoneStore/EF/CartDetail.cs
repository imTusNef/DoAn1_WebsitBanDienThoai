namespace WebPhoneStore.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CartDetail")]
    [Serializable]
    public partial class CartDetail
    {
        public int CartDetailID { get; set; }

        [StringLength(250)]
        public string ProductName { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public decimal? Total { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AddedDate { get; set; }

        public int? CartID { get; set; }

        public int? ProductID { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual Product Product { get; set; }
    }
}
