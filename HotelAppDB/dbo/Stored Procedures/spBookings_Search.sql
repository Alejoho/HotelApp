CREATE PROCEDURE [dbo].[spBookings_Search]
	@todayDate DATE,
	@lastName VARCHAR(50)
AS
BEGIN

	SELECT b.Id, b.RoomId, b.GuestId, b.StartDate, b.EndDate, b.CheckedIn, b.TotalCost 
	FROM dbo.Bookings b
	INNER JOIN dbo.Guests g ON g.Id = b.GuestId
	WHERE b.StartDate <= @todayDate
	AND g.LastName = @lastName
	AND b.CheckedIn = 0;

END
