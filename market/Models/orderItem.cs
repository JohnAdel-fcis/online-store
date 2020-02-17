namespace market.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("orderItem")]
    public partial class orderItem
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int orderItemID { get; set; }

        public int? orderID { get; set; }

        public int? ItemId { get; set; }

        public int? quantity { get; set; }

        public virtual Item Item { get; set; }

        public virtual order order { get; set; }
    }
}
