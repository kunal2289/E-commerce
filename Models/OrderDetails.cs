namespace E_commerce.Models
{
    public class OrderDetails
    {
        public string orderId { get; set; }
        public string orderDetailsId { get; set; }
        public string productId { get; set; }
        public int quantity { get; set; }
        public decimal price{ get; set; }
    }
}
