using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using System.Windows;

namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for CheckInForm.xaml
    /// </summary>
    public partial class CheckInForm : Window
    {
        private readonly IDatabaseData _db;
        private BookingFullModel _data = null;

        public CheckInForm(IDatabaseData db)
        {
            InitializeComponent();
            _db = db;
        }

        public void PopulateData(BookingFullModel data)
        {
            _data = data;
            this.firstName.Text = _data.Guest.FirstName;
            this.lastName.Text = _data.Guest.LastName;
            this.roomTitle.Text = _data.RoomType.Title;
            this.roomNumber.Text = _data.Room.RoomNumber.ToString();
            this.totalCost.Text = _data.Booking.TotalCost.ToString("C");
        }

        private void checkIn_Click(object sender, RoutedEventArgs e)
        {
            _db.CheckInGuest(_data.Booking.Id);
            this.Close();
        }
    }
}
