namespace E_commerce.Models
{
    public class Cart
    {
        public string cartId { get; set; }
        public string userId { get; set; }
        public string productId { get; set; }
        public int quantity { get; set; }
    }
}
