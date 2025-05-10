namespace EventEaseManagementSystem.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; }
        public string? ProfilePicture {  get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}
