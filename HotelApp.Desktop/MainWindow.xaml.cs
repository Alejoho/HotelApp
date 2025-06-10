using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDatabaseData _db;

        public MainWindow(IDatabaseData db)
        {
            InitializeComponent();
            _db = db;
            DisplayDate();
        }

        private void DisplayDate()
        {
            var now = DateTime.Now.ToShortDateString();
            string displayText = $"Today's date: {now}";
            this.todayDate.Text = displayText;
        }

        private void searchGuest_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(guestLastName.Text) is true)
            {
                MessageBox.Show(this, "Please fill the Last Name.");
                this.guestLastName.Focus();
                return;
            }

            List<BookingFullModel> results = _db.SearchBookings(guestLastName.Text);

            this.results.ItemsSource = null;

            this.results.ItemsSource = results;
        }

        private void checkIn_Click(object sender, RoutedEventArgs e)
        {
            var checkInForm = App.ServiceProvider.GetRequiredService<CheckInForm>();

            var data = (BookingFullModel)((Button)e.Source).DataContext;

            checkInForm.PopulateData(data);

            checkInForm.ShowDialog();

            List<BookingFullModel> results = _db.SearchBookings(guestLastName.Text);

            this.results.ItemsSource = null;

            this.results.ItemsSource = results;
        }
    }
}