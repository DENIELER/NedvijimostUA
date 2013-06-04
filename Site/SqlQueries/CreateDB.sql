CREATE DATABASE [1gb_RentHouses];
GO

use [1gb_RentHouses];
GO

CREATE TABLE [RentSearchResults] (
	[id] int NOT NULL identity,
	[filename] text NOT NULL,
	[createDate] datetime NOT NULL,
	[parsedAdvertCount] int NULL
)
GO

CREATE TABLE [SubPurchases] (
	[id] uniqueidentifier NOT NULL,
	[phone] varchar(100) NOT NULL,
	[name] varchar(100) NULL,
	[surname] varchar(100) NULL,
	[not_checked] bit NULL default 0
)
GO

INSERT INTO [RentSearchResults] ([filename], [createDate], [parsedAdvertCount]) 
VALUES ('/xmlRentHouseResults/result_06_05_2012.xml', GETDATE(), 6044);
GO

