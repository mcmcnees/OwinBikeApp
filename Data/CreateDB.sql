CREATE DATABASE BikeMgr;
GO

USE BikeMgr
GO

CREATE TABLE dbo.BikeType (
	ID int identity,
	TypeName nvarchar(200),
	CONSTRAINT PK_BikeType PRIMARY KEY NONCLUSTERED (ID)
);

CREATE TABLE dbo.Bike (
	ID int identity,
	Name nvarchar(200),
	Brand nvarchar(200),
	Wheels int,
	FrameMaterial nvarchar(200),
	BikeTypeID int,
	Price money,
	ImageLocation nvarchar(500),
	CONSTRAINT PK_Bike PRIMARY KEY NONCLUSTERED (ID),
    CONSTRAINT FK_Bike_TypeID FOREIGN KEY (BikeTypeID)
	REFERENCES dbo.BikeType(ID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

CREATE TABLE dbo.AuthorizedApp (
	AuthorizedAppId int identity,
	Name nvarchar(100),
	AppToken nvarchar(32),
	AppSecret nvarchar(32),
	TokenExpiration datetime
);

CREATE TABLE BikeModComp (
	ID int identity,
	BikeID int
);

CREATE TABLE BikeMod (
	ID int identity,
	BikeModCompID int,
	PartTypeID int,
	Name nvarchar(200),
	Brand nvarchar(200),
	Material nvarchar(200),	
	Price money,
	ImageLocation nvarchar(500)
);

CREATE TABLE PartType (
	ID int identity,
	PartType nvarchar(200)	
);

INSERT INTO	BikeType (TypeName) VALUES ('Hybrid');
INSERT INTO	BikeType (TypeName) VALUES ('Road');
INSERT INTO	BikeType (TypeName) VALUES ('Mountain');
INSERT INTO	BikeType (TypeName) VALUES ('Tandem');
INSERT INTO	BikeType (TypeName) VALUES ('Unicycle');

INSERT INTO [dbo].[Bike] ([Name],[Brand],[Wheels],[FrameMaterial],[BikeTypeID],[Price],[ImageLocation]) VALUES ('Schwinn','Kaga',2,'Carbon Fiber',1,2000,'');
INSERT INTO [dbo].[Bike] ([Name],[Brand],[Wheels],[FrameMaterial],[BikeTypeID],[Price],[ImageLocation]) VALUES ('Firmstrong','Sea',2,'Aluminum',1,1850,'');
INSERT INTO [dbo].[Bike] ([Name],[Brand],[Wheels],[FrameMaterial],[BikeTypeID],[Price],[ImageLocation]) VALUES ('Dynacraft','Sky',2,'Steel',1,1700,'');
INSERT INTO [dbo].[Bike] ([Name],[Brand],[Wheels],[FrameMaterial],[BikeTypeID],[Price],[ImageLocation]) VALUES ('Razor','Streamer',2,'Steel',1,1650,'');
INSERT INTO [dbo].[Bike] ([Name],[Brand],[Wheels],[FrameMaterial],[BikeTypeID],[Price],[ImageLocation]) VALUES ('Generic','Joker',2,'Steel',1,1600,'');

INSERT INTO [dbo].[AuthorizedApp] ([Name],[AppToken],[AppSecret],[TokenExpiration]) VALUES ('BikeMgrWeb','ca5df7dafac772917074ecd4b965d29d','4204031c36f522cd22c7b5a52a223603','2060/06/20');