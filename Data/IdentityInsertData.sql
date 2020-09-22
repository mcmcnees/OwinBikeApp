USE [ASP_ID]
GO

INSERT INTO [dbo].[AspNetUserClaims] ([UserId],[ClaimType],[ClaimValue]) VALUES ('1fb162ae-8043-4631-afa8-581b2529cab0','Role','Read') 

INSERT INTO [dbo].[AspNetUserClaims] ([UserId],[ClaimType],[ClaimValue]) VALUES ('da11bf73-9553-4843-b320-3a55b2cd7187','role','Read') ;
INSERT INTO [dbo].[AspNetUserClaims] ([UserId],[ClaimType],[ClaimValue]) VALUES ('da11bf73-9553-4843-b320-3a55b2cd7187','role','Write') ;
INSERT INTO [dbo].[AspNetUserClaims] ([UserId],[ClaimType],[ClaimValue]) VALUES ('da11bf73-9553-4843-b320-3a55b2cd7187','role','UserAdmin');


-- update [AspNetUsers] set FirstName = 'John', LastName = 'Blue' where [id] = '1fb162ae-8043-4631-afa8-581b2529cab0';