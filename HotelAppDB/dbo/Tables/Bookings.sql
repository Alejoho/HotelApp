CREATE TABLE [dbo].[Bookings]
(
  [Id] INT NOT NULL PRIMARY KEY IDENTITY,
  [RoomId] INT NOT NULL,
  [GuestId] INT NOT NULL,
  [StartDate] DATE NOT NULL,
  [EndDate] DATE NOT NULL,
  [CheckedIn] BIT NOT NULL DEFAULT 0,
  [TotalCost] MONEY NOT NULL
)
