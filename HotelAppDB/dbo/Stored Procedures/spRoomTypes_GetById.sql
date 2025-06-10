CREATE PROCEDURE [dbo].[spRoomTypes_GetById]
  @id int
AS
BEGIN

  SET NOCOUNT ON;

  SELECT r.Id, r.Title, r.[Description], r.Price
  FROM dbo.RoomTypes r
  WHERE r.Id = @id;

END