﻿using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;

namespace HotelAppLibrary.Data
{
    public interface IDatabaseData
    {
        void BookGuest(string firstName,
                       string lastName,
                       DateTime startDate,
                       DateTime endDate,
                       int roomTypeId);
        void CheckInGuest(int bookingId);
        List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate,
                                                  DateTime endDate);
        List<BookingFullModel> SearchBookings(string lastName);
        RoomTypeModel GetRoomTypeById(int roomTypeId);
    }
}