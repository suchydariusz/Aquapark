/****** Object:  Database [AquaparkDB]    Script Date: 25.01.2019 01:36:31 ******/
CREATE DATABASE [AquaparkDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AquaparkDB', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\AquaparkDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AquaparkDB_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\AquaparkDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [AquaparkDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AquaparkDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AquaparkDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AquaparkDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AquaparkDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AquaparkDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AquaparkDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [AquaparkDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AquaparkDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AquaparkDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AquaparkDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AquaparkDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AquaparkDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AquaparkDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AquaparkDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AquaparkDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AquaparkDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AquaparkDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AquaparkDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AquaparkDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AquaparkDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AquaparkDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AquaparkDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AquaparkDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AquaparkDB] SET RECOVERY FULL 
GO
ALTER DATABASE [AquaparkDB] SET  MULTI_USER 
GO
ALTER DATABASE [AquaparkDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AquaparkDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AquaparkDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AquaparkDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AquaparkDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'AquaparkDB', N'ON'
GO
ALTER DATABASE [AquaparkDB] SET QUERY_STORE = OFF
GO
USE [AquaparkDB]
GO
/****** Object:  Table [dbo].[tbl_Attraction]    Script Date: 25.01.2019 01:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Attraction](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Attraction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_AttractionHistory]    Script Date: 25.01.2019 01:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_AttractionHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeginDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[IDAttractionList] [int] NOT NULL,
	[AttractionName] [varchar](50) NOT NULL,
	[AttractionPrice] [float] NOT NULL,
 CONSTRAINT [PK_tbl_AttractionHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Client]    Script Date: 25.01.2019 01:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Client](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Surname] [nvarchar](50) NOT NULL,
	[PESEL] [varchar](11) NOT NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Gate]    Script Date: 25.01.2019 01:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Gate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [bit] NOT NULL,
	[IDAttraction] [int] NOT NULL,
 CONSTRAINT [PK_Gate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_GateHistory]    Script Date: 25.01.2019 01:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_GateHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[IDGate] [int] NOT NULL,
	[IDVisit] [int] NOT NULL,
 CONSTRAINT [PK_GateHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Pass]    Script Date: 25.01.2019 01:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Pass](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WhenEnds] [date] NOT NULL,
	[IDClient] [int] NOT NULL,
 CONSTRAINT [PK_Pass] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_PriceHistory]    Script Date: 25.01.2019 01:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_PriceHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeginDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[IDPriceList] [int] NOT NULL,
	[TicketName] [varchar](50) NOT NULL,
	[TicketPrice] [float] NOT NULL,
 CONSTRAINT [PK_PriceHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_PriceList]    Script Date: 25.01.2019 01:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_PriceList](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Entry] [varchar](50) NOT NULL,
	[Price] [float] NOT NULL,
 CONSTRAINT [PK_PriceList] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_PriceListAttraction]    Script Date: 25.01.2019 01:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_PriceListAttraction](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PriceAttraction] [float] NOT NULL,
	[IDAttraction] [int] NOT NULL,
 CONSTRAINT [PK_PriceListAttraction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_RFIDWatch]    Script Date: 25.01.2019 01:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_RFIDWatch](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_RFIDWatch] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Visit]    Script Date: 25.01.2019 01:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Visit](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StartTime] [datetime] NULL,
	[StopTime] [datetime] NULL,
	[IDWatch] [int] NOT NULL,
	[IDPriceEntry] [int] NULL,
	[IDPass] [int] NULL,
 CONSTRAINT [PK_Visit] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_AttractionHistory]  WITH CHECK ADD  CONSTRAINT [FK_AttractionHistory_PriceListAttraction] FOREIGN KEY([IDAttractionList])
REFERENCES [dbo].[tbl_PriceListAttraction] ([ID])
GO
ALTER TABLE [dbo].[tbl_AttractionHistory] CHECK CONSTRAINT [FK_AttractionHistory_PriceListAttraction]
GO
ALTER TABLE [dbo].[tbl_Gate]  WITH CHECK ADD  CONSTRAINT [FK_Gate_Attraction] FOREIGN KEY([IDAttraction])
REFERENCES [dbo].[tbl_Attraction] ([ID])
GO
ALTER TABLE [dbo].[tbl_Gate] CHECK CONSTRAINT [FK_Gate_Attraction]
GO
ALTER TABLE [dbo].[tbl_GateHistory]  WITH CHECK ADD  CONSTRAINT [FK_GateHistory_Gate] FOREIGN KEY([IDGate])
REFERENCES [dbo].[tbl_Gate] ([ID])
GO
ALTER TABLE [dbo].[tbl_GateHistory] CHECK CONSTRAINT [FK_GateHistory_Gate]
GO
ALTER TABLE [dbo].[tbl_GateHistory]  WITH CHECK ADD  CONSTRAINT [FK_GateHistory_Visit] FOREIGN KEY([IDVisit])
REFERENCES [dbo].[tbl_Visit] ([ID])
GO
ALTER TABLE [dbo].[tbl_GateHistory] CHECK CONSTRAINT [FK_GateHistory_Visit]
GO
ALTER TABLE [dbo].[tbl_Pass]  WITH CHECK ADD  CONSTRAINT [FK_Pass_Client] FOREIGN KEY([IDClient])
REFERENCES [dbo].[tbl_Client] ([ID])
GO
ALTER TABLE [dbo].[tbl_Pass] CHECK CONSTRAINT [FK_Pass_Client]
GO
ALTER TABLE [dbo].[tbl_PriceHistory]  WITH CHECK ADD  CONSTRAINT [FK_PriceHistory_PriceList] FOREIGN KEY([IDPriceList])
REFERENCES [dbo].[tbl_PriceList] ([ID])
GO
ALTER TABLE [dbo].[tbl_PriceHistory] CHECK CONSTRAINT [FK_PriceHistory_PriceList]
GO
ALTER TABLE [dbo].[tbl_PriceListAttraction]  WITH CHECK ADD  CONSTRAINT [FK_PriceListAttraction_Attraction] FOREIGN KEY([IDAttraction])
REFERENCES [dbo].[tbl_Attraction] ([ID])
GO
ALTER TABLE [dbo].[tbl_PriceListAttraction] CHECK CONSTRAINT [FK_PriceListAttraction_Attraction]
GO
ALTER TABLE [dbo].[tbl_Visit]  WITH CHECK ADD  CONSTRAINT [FK_Visit_Pass] FOREIGN KEY([IDPass])
REFERENCES [dbo].[tbl_Pass] ([ID])
GO
ALTER TABLE [dbo].[tbl_Visit] CHECK CONSTRAINT [FK_Visit_Pass]
GO
ALTER TABLE [dbo].[tbl_Visit]  WITH CHECK ADD  CONSTRAINT [FK_Visit_PriceList] FOREIGN KEY([IDPriceEntry])
REFERENCES [dbo].[tbl_PriceList] ([ID])
GO
ALTER TABLE [dbo].[tbl_Visit] CHECK CONSTRAINT [FK_Visit_PriceList]
GO
ALTER TABLE [dbo].[tbl_Visit]  WITH CHECK ADD  CONSTRAINT [FK_Visit_RFIDWatch] FOREIGN KEY([IDWatch])
REFERENCES [dbo].[tbl_RFIDWatch] ([ID])
GO
ALTER TABLE [dbo].[tbl_Visit] CHECK CONSTRAINT [FK_Visit_RFIDWatch]
GO
USE [master]
GO
ALTER DATABASE [AquaparkDB] SET  READ_WRITE 
GO

USE [AquaparkDB]
GO

BEGIN
    INSERT INTO tbl_Attraction VALUES ('Sauna');
	INSERT INTO tbl_Attraction VALUES ('Zjeżdżalnia cebula');
	INSERT INTO tbl_Attraction VALUES ('Zjeżdżalnia szybka');
	INSERT INTO tbl_Attraction VALUES ('Solanka');
	INSERT INTO tbl_Attraction VALUES ('Sztuczna fala');
	INSERT INTO tbl_Attraction VALUES ('Jaskinia wodna');
	INSERT INTO tbl_Attraction VALUES ('Zjeżdżalnia pontonowa');

	DECLARE @i int = 0
	WHILE @i < 7
	BEGIN
		SET @i = @i + 1
		INSERT INTO tbl_Gate VALUES ( 0, @i )
		INSERT INTO tbl_Gate VALUES ( 1, @i )
	END
	
	INSERT INTO tbl_PriceListAttraction VALUES (8, 1);
	INSERT INTO tbl_PriceListAttraction VALUES (5, 2);
	INSERT INTO tbl_PriceListAttraction VALUES (4, 3);
	INSERT INTO tbl_PriceListAttraction VALUES (6, 4);
	INSERT INTO tbl_PriceListAttraction VALUES (2, 5);
	INSERT INTO tbl_PriceListAttraction VALUES (8, 6);
	INSERT INTO tbl_PriceListAttraction VALUES (4, 7);
	
	INSERT INTO tbl_PriceList VALUES ('Bilet jednogodzinny', 17);
	INSERT INTO tbl_PriceList VALUES ('Bilet dwugodzinny', 26);
	INSERT INTO tbl_PriceList VALUES ('Bilet trzygodzinny', 35);
	INSERT INTO tbl_PriceList VALUES ('Bilet całodniowy', 50);
	INSERT INTO tbl_PriceList VALUES ('Bilet jednogodzinny weekend', 22);
	INSERT INTO tbl_PriceList VALUES ('Bilet dwugodzinny weekend', 28);
	INSERT INTO tbl_PriceList VALUES ('Bilet trzygodzinny weekend', 38);
	INSERT INTO tbl_PriceList VALUES ('Bilet całodniowy weekend', 70);
	INSERT INTO tbl_PriceList VALUES ('Przekroczenie czasu 1h', 15);
	INSERT INTO tbl_PriceList VALUES ('Przekroczenie czasu 2h', 22);
	INSERT INTO tbl_PriceList VALUES ('Przekroczenie czasu 3h i więcej', 40);
	
	DECLARE @j int = 0
	WHILE @j < 2100 
    BEGIN
		SET @j = @j + 1
		INSERT INTO tbl_RFIDWatch VALUES ( 1 )
	END
END