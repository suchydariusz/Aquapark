USE [AquaparkDB]
GO

DECLARE @i int = 0
WHILE @i < 2100 
BEGIN
    SET @i = @i + 1
    INSERT INTO tbl_RFIDWatch VALUES ( 1 )
END