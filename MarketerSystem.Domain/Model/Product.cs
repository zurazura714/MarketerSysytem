using System.ComponentModel.DataAnnotations;

namespace MarketerSystem.Domain.Model;

public class Product
{
    [Key]
    public int ID { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }
}
