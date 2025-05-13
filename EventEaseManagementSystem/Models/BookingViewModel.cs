namespace EventEaseManagementSystem.Models
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public DateOnly BookingDate { get; set; }

        public int EventId { get; set; }
        public string? EventName { get; set; }
        public DateOnly? EventDate { get; set; }
        public string? Description { get; set; }

        public int VenueId { get; set; }
        public string? VenueName { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }
        public string? ImageUrl { get; set; }

        // For form binding
        public Booking Booking { get; set; } = new Booking();
        public Event Event { get; set; } = new Event();
        public Venue Venue { get; set; } = new Venue();
    }

}
