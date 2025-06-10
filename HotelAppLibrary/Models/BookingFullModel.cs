namespace HotelAppLibrary.Models
{
    public class BookingFullModel
    {
        public BookingBasicModel Booking { get; set; }
        public GuestModel Guest { get; set; }
        public RoomModel Room { get; set; }
        public RoomTypeModel RoomType { get; set; }
    }
}
