using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelAppLibrary.Data
{
    public class SqlData : IDatabaseData
    {
        private readonly ISqlDataAccess _db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(
            DateTime startDate,
            DateTime endDate)
        {
            return _db.LoadData<RoomTypeModel, dynamic>(
                "dbo.spRoomTypes_GetAvailableTypes",
                new { startDate, endDate },
                connectionStringName,
                true);
        }

        public void BookGuest(string firstName,
                              string lastName,
                              DateTime startDate,
                              DateTime endDate,
                              int roomTypeId)
        {
            GuestModel guest = _db.LoadData<GuestModel, dynamic>(
                "dbo.spGuests_Insert",
                new { firstName, lastName },
                connectionStringName,
                true).First();

            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>(
                "dbo.spRooms_GetAvailableRooms",
                new { startDate, endDate, roomTypeId },
                connectionStringName,
                true);

            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>(
                "SELECT r.Id, r.Title, r.Description, r.Price FROM dbo.RoomTypes r WHERE r.Id = @roomTypeId",
                new { roomTypeId },
                connectionStringName,
                false).First();

            TimeSpan daysStaying = endDate.Date - startDate.Date;

            _db.SaveData<dynamic>(
                "dbo.spBookings_Insert",
                new
                {
                    roomId = availableRooms.First().Id,
                    guestId = guest.Id,
                    startDate,
                    endDate,
                    totalCost = daysStaying.Days * roomType.Price
                },
                connectionStringName,
                true);
        }

        public List<BookingFullModel> SearchBookings(string lastName)
        {
            List<BookingFullModel> output = new List<BookingFullModel>();

            DateTime todayDate = DateTime.Today;

            var basicBookings = _db.LoadData<BookingBasicModel, dynamic>(
                "dbo.spBookings_Search",
                new
                {
                    lastName,
                    todayDate
                },
                connectionStringName,
                true);

            foreach (var bb in basicBookings)
            {
                BookingFullModel fullModel = new BookingFullModel();
                fullModel.Booking = bb;

                fullModel.Guest = _db.LoadData<GuestModel, dynamic>(
                    "SELECT Id, FirstName, LastName FROM dbo.Guests WHERE Id = @Id",
                    new
                    {
                        Id = bb.GuestId
                    },
                    connectionStringName,
                    false).First();

                fullModel.Room = _db.LoadData<RoomModel, dynamic>(
                    "SELECT Id, RoomNumber, RoomTypeId FROM dbo.Rooms WHERE Id = @Id",
                    new
                    {
                        Id = bb.RoomId
                    },
                    connectionStringName,
                    false).First();

                fullModel.RoomType = _db.LoadData<RoomTypeModel, dynamic>(
                    "SELECT Id, Title, Description, Price FROM dbo.RoomTypes WHERE Id = @Id",
                    new
                    {
                        Id = fullModel.Room.RoomTypeId
                    },
                    connectionStringName,
                    false).First();

                output.Add(fullModel);
            }

            return output;
        }

        public RoomTypeModel GetRoomTypeById(int id)
        {
            return _db.LoadData<RoomTypeModel, dynamic>(
                "dbo.spRoomTypes_GetById",
                new { id },
                connectionStringName,
                true).FirstOrDefault();
        }

        public void CheckInGuest(int bookingId)
        {
            string sql = @"UPDATE dbo.Bookings SET CheckedIn = 1 WHERE Id = @bookingId";

            _db.SaveData<dynamic>(
                sql,
                new { bookingId },
                connectionStringName,
                false);
        }
    }
}