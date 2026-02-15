namespace Store.Domain.Entities.Orders
{
    // table
    public class OrderItem : BaseEntity<int>
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductInOrderItem product, decimal price, int quantity)
        {
            this.product = product;
            Price = price;
            Quantity = quantity;
        }

        public ProductInOrderItem product { get; set; } 
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}