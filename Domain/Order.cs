using Domain.Enums;

namespace Domain;

public class Order: BaseEntity
{
    public string Description { get; set; }
    public List<OrderItem> Items { get; set; } = [];
    public decimal TotalPrice { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string UserId { get; set; }

    public int StatusId
    {
        get => (int)OrderStatus; 
        set => OrderStatus = (OrderStatus)value;
    }
}