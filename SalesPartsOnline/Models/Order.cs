namespace SalesPartsOnline.Models
{
        public class Order
        {
            public Guid OrderId { get; set; }

            public Guid UserId { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal TotalAmount { get; set; }
            public ICollection<OrderItem> OrderItems { get; set; }
            public User User { get; set; }
    }

        public class OrderItem
        {
            public Guid OrderItemId { get; set; }
            public Guid OrderId { get; set; }
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }

        
    }
    
}
