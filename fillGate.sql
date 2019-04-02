USE [AquaparkDB]
GO

DECLARE @i int = 0
WHILE @i < 7
BEGIN
    SET @i = @i + 1
    INSERT INTO tbl_Gate VALUES ( 0, @i )
	INSERT INTO tbl_Gate VALUES ( 1, @i )
END

