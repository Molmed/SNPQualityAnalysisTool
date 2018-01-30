--RETURNS THE WELL NAME FOR A PAIR OF X AND Y COORDINATES.

CREATE FUNCTION dbo.fTranslateCoord (@x TINYINT, @y TINYINT)
RETURNS VARCHAR(3)

AS
BEGIN
	DECLARE @valid AS BIT

	--
	-- Return empty string if any of the coordinates are null or not supported.
	--
	IF (@x IS NULL) OR (@y IS NULL) OR (@x < 1) OR (@x > 24) OR (@y < 1) OR (@y > 16) RETURN ''
	
	RETURN CHAR(64 + @y) + CAST(@x AS VARCHAR(2))
END

