CREATE PROCEDURE [dbo].[spRoomTypes_GetAvailableTypes]
    @startDate DATE,
    @endDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT t.Id, t.Title, t.[Description], t.Price
    FROM dbo.RoomTypes t
        INNER JOIN dbo.Rooms r ON r.RoomTypeId=t.Id
    WHERE NOT EXISTS(
    SELECT 1
    FROM dbo.Bookings b
    WHERE b.RoomId = r.Id
        AND @startDate < b.EndDate
        AND @endDate > b.StartDate
)
    GROUP BY t.Id, t.Title, t.[Description], t.Price;

END
