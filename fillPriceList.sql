USE [AquaparkDB]
GO

BEGIN
    INSERT INTO tbl_PriceList VALUES ('Bilet jednogodzinny', 17);
	INSERT INTO tbl_PriceList VALUES ('Bilet dwugodzinny', 26);
	INSERT INTO tbl_PriceList VALUES ('Bilet trzygodzinny', 35);
	INSERT INTO tbl_PriceList VALUES ('Bilet ca³odniowy', 50);
	INSERT INTO tbl_PriceList VALUES ('Bilet jednogodzinny weekend', 22);
	INSERT INTO tbl_PriceList VALUES ('Bilet dwugodzinny weekend', 28);
	INSERT INTO tbl_PriceList VALUES ('Bilet trzygodzinny weekend', 38);
	INSERT INTO tbl_PriceList VALUES ('Bilet ca³odniowy weekend', 70);
	INSERT INTO tbl_PriceList VALUES ('Przekroczenie czasu 1h', 15);
	INSERT INTO tbl_PriceList VALUES ('Przekroczenie czasu 2h', 22);
	INSERT INTO tbl_PriceList VALUES ('Przekroczenie czasu 3h i wiêcej', 40);
END