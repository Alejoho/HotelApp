CREATE PROCEDURE [dbo].[spBookings_Insert]
	@roomId INT,
	@guestId INT,
	@startDate DATE,
	@endDate DATE,
	@totalCost MONEY

AS
BEGIN

SET NOCOUNT ON;

INSERT INTO dbo.Bookings(RoomId, GuestId, StartDate, EndDate, TotalCost)
VALUES(@roomId, @guestId, @startDate, @endDate, @totalCost);

END
