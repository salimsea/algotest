using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace algotest.core.entities
{
    [Table("order_items", Schema = "public")]
    public class TbOrderItems
    {
        [Key,Required]
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("product_id")]
        public int ProductId { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
    }
}