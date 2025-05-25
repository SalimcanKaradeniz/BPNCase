namespace Domain;

public class OrderItem: BaseEntity
{
    public Products Product { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public string Currency { get; set; }
    public string UserId { get; set; }
    public long OrderId { get; set; }
    public Order Order { get; set; }
    
}
