namespace WebPhoneStore.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductComment")]
    public partial class ProductComment
    {
        [Key]
        public int CommentID { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [StringLength(250)]
        public string Detail { get; set; }

        public bool? Status { get; set; }

        public int? ProductID { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UpdatedDate { get; set; }

        public virtual Product Product { get; set; }
    }
}
