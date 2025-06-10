using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelAppLibrary.Data
{
    public class SqliteData : IDatabaseData
    {
        private readonly ISqliteDataAccess _db;
        private const string connectionStringName = "SqliteDb";

        public SqliteData(ISqliteDataAccess db)
        {
            _db = db;
        }

        public void BookGuest(string firstName,
            string lastName,
            DateTime startDate,
            DateTime endDate,
            int roomTypeId)
        {
            string sql = String.Empty;

            int guestId = GetGuestId(firstName, lastName);

            if (guestId is 0)
            {
                sql = @"INSERT INTO Guests
			        (FirstName,LastName)
		            VALUES(@firstName, @lastName)";

                _db.SaveData(sql, new { firstName, lastName }, connectionStringName);
            }

            guestId = GetGuestId(firstName, lastName);

            sql = @"SELECT r.Id, r.RoomNumber, r.RoomTypeId
                    FROM RoomTypes t
                        INNER JOIN Rooms r ON r.RoomTypeId=t.Id
                    WHERE r.RoomTypeId = @roomTypeId
                    AND NOT EXISTS(
                        SELECT 1
                        FROM Bookings b
                        WHERE b.RoomId = r.Id
                            AND @startDate < b.EndDate
                            AND @endDate > b.StartDate
                    )";

            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>(
                sql,
                new { startDate, endDate, roomTypeId },
                connectionStringName);

            sql = @"SELECT r.Id, r.Title, r.Description, r.Price FROM RoomTypes r WHERE r.Id = @roomTypeId";

            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>(
                    sql,
                    new { roomTypeId },
                    connectionStringName)
                .First();

            TimeSpan daysStaying = endDate.Date - startDate.Date;

            sql =
                @"INSERT INTO Bookings(RoomId, GuestId, StartDate, EndDate, TotalCost)
                VALUES(@roomId, @guestId, @startDate, @endDate, @totalCost);";

            _db.SaveData<dynamic>(
                sql,
                new
                {
                    roomId = availableRooms.First().Id,
                    guestId,
                    startDate = startDate.ToString("yyyy-MM-dd"),
                    endDate = endDate.ToString("yyyy-MM-dd"),
                    totalCost = daysStaying.Days * roomType.Price
                },
                connectionStringName);
        }

        private int GetGuestId(string firstName, string lastName)
        {
            const string sql = @"SELECT g.Id
	                             FROM Guests g
	                             WHERE g.FirstName = @firstName 
	                             AND g.LastName = @lastName";

            return _db.LoadData<int, dynamic>(
                sql,
                new { firstName, lastName },
                connectionStringName).FirstOrDefault();
        }

        public void CheckInGuest(int bookingId)
        {
            string sql = @"UPDATE Bookings SET CheckedIn = 1 WHERE Id = @bookingId";

            _db.SaveData<dynamic>(
                sql,
                new { bookingId },
                connectionStringName);
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            string sql =
                @"SELECT t.Id, t.Title, t.[Description], t.Price
                FROM RoomTypes t
                    INNER JOIN Rooms r ON r.RoomTypeId=t.Id
                WHERE r.Id NOT IN(
                SELECT b.RoomId
                FROM Bookings b
                WHERE @startDate < b.EndDate
                    AND @endDate > b.StartDate)
                GROUP BY t.Id, t.Title, t.[Description], t.Price;";

            var output = _db.LoadData<RoomTypeModel, dynamic>(
                sql,
                new { startDate, endDate },
                connectionStringName);

            output.ForEach(x => x.Price /= 100);

            return output;
        }

        public List<BookingFullModel> SearchBookings(string lastName)
        {
            List<BookingFullModel> output = new List<BookingFullModel>();

            DateTime todayDate = DateTime.Today;

            string sql =
                @"SELECT b.Id, b.RoomId, b.GuestId, b.StartDate, 
                b.EndDate, b.CheckedIn, (b.TotalCost/100) as TotalCost 
	            FROM Bookings b
	            INNER JOIN Guests g ON g.Id = b.GuestId
	            WHERE b.StartDate <= @todayDate
	            AND g.LastName = @lastName
	            AND b.CheckedIn = 0";

            var basicBookings = _db.LoadData<BookingBasicModel, dynamic>(
                sql,
                new
                {
                    lastName,
                    todayDate
                },
                connectionStringName);

            foreach (BookingBasicModel bb in basicBookings)
            {
                BookingFullModel fullModel = new BookingFullModel();
                fullModel.Booking = bb;

                fullModel.Guest = _db.LoadData<GuestModel, dynamic>(
                        "SELECT Id, FirstName, LastName FROM Guests WHERE Id = @Id",
                        new
                        {
                            Id = bb.GuestId
                        },
                        connectionStringName)
                    .First();

                fullModel.Room = _db.LoadData<RoomModel, dynamic>(
                        "SELECT Id, RoomNumber, RoomTypeId FROM Rooms WHERE Id = @Id",
                        new
                        {
                            Id = bb.RoomId
                        },
                        connectionStringName)
                    .First();

                fullModel.RoomType = _db.LoadData<RoomTypeModel, dynamic>(
                        "SELECT Id, Title, Description, (Price/100) AS Price FROM RoomTypes WHERE Id = @Id",
                        new
                        {
                            Id = fullModel.Room.RoomTypeId
                        },
                        connectionStringName)
                    .First();

                output.Add(fullModel);
            }

            return output;
        }

        public RoomTypeModel GetRoomTypeById(int roomTypeId)
        {
            string sql =
                @"  SELECT r.Id, r.Title, r.[Description], (r.Price/100) AS Price
                FROM RoomTypes r
                WHERE r.Id = @id;";

            return _db.LoadData<RoomTypeModel, dynamic>(
                sql,
                new { id = roomTypeId },
                connectionStringName).FirstOrDefault();
        }
    }
}