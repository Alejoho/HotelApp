CREATE PROCEDURE [dbo].[spGuests_Insert]
	@firstName varchar(50),
	@lastName varchar(50)
AS
BEGIN

	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1
	FROM dbo.Guests g
	WHERE g.FirstName=@firstName AND g.LastName=@lastName)
	BEGIN

		INSERT INTO dbo.Guests
			(FirstName,LastName)
		VALUES(@firstName, @lastName);

	END

	SELECT TOP(1)
		g.Id, g.FirstName, g.LastName
	FROM dbo.Guests g
	WHERE g.FirstName=@firstName AND g.LastName=@lastName

END
