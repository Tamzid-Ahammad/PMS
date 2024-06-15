namespace Core.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string RecipientName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Guid UserId { get; set; }


        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
