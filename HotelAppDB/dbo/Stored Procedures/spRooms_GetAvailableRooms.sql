CREATE PROCEDURE [dbo].[spRooms_GetAvailableRooms]
	@startDate DATE,
	@endDate DATE,
    @roomTypeId int
AS
BEGIN

    SET NOCOUNT ON;

    SELECT r.Id, r.RoomNumber, r.RoomTypeId
    FROM dbo.RoomTypes t
        INNER JOIN dbo.Rooms r ON r.RoomTypeId=t.Id
    WHERE r.RoomTypeId = @roomTypeId
    AND NOT EXISTS(
        SELECT 1
        FROM dbo.Bookings b
        WHERE b.RoomId = r.Id
            AND @startDate < b.EndDate
            AND @endDate > b.StartDate
    );

END
