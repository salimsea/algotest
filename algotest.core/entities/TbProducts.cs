using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace algotest.core.entities
{
    [Table("products", Schema = "public")]
    public class TbProducts
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("price")]
        public int Price { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}