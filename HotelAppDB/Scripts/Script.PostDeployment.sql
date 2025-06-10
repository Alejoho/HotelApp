-- This file contains SQL statements that will be executed after the build script.
IF NOT EXISTS(SELECT 1
FROM [dbo].[RoomTypes])
BEGIN

    INSERT INTO [dbo].[RoomTypes]
        ([Title],[Description],[Price])
    VALUES('King Size Bed', 'A room with a king-size bed and a window.', 100),
        ('Two Queen Size Beds', 'A room with two queen-size beds and a window.', 115),
        ('Executive Suite', 'Two rooms, each with a king-size bed and a window', 205);

END
GO

IF NOT EXISTS(SELECT 1
FROM [dbo].[Rooms])
BEGIN

    DECLARE @roomId1 int;
    DECLARE @roomId2 int;
    DECLARE @roomId3 int;

    SELECT @roomId1 =Id
    FROM [dbo].[RoomTypes]
    WHERE Title = 'King Size Bed';

    SELECT @roomId2 =Id
    FROM [dbo].[RoomTypes]
    WHERE Title = 'Two Queen Size Beds';

    SELECT @roomId3 =Id
    FROM [dbo].[RoomTypes]
    WHERE Title = 'Executive Suite';

    INSERT INTO [dbo].[Rooms]
        (RoomNumber,RoomTypeId)
    VALUES('101', @roomId1),
        ('102', @roomId1),
        ('103', @roomId1),
        ('201', @roomId2),
        ('202', @roomId2),
        ('301', @roomId3);

END