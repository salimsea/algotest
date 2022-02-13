using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace algotest.core.entities
{
    [Table("orders", Schema = "public")]
    public class TbOrders
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}