using Domain.Enums;

namespace Domain;

public class Products : BaseEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Currency { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
}
