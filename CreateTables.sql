CREATE DATABASE dbTVReminderSystem

USE dbTVReminderSystem;

drop TABLE tblUser;
drop TABLE tblTVShows;
drop TABLE tblSubscription;

CREATE TABLE tblUser
([UserID] INT IDENTITY,
[FirstName] NVARCHAR(20) NOT NULL,
[Surname] NVARCHAR(20) NOT NULL,
[Email] NVARCHAR(50) NOT NULL UNIQUE,
[Mobile] NVARCHAR(15) NULL,																						-- made this no unique because sql errors with multiple null values
[Password] NVARCHAR(30) NOT NULL,
CONSTRAINT PK_tblUser_UserID PRIMARY KEY (UserID));

CREATE TABLE tblTVShows
([ShowID] INT IDENTITY,
[TVRageID] INT NOT NULL UNIQUE,
[ShowName] NVARCHAR(200) NOT NULL,
[Status] NVARCHAR(50) NOT NULL,
[LatestEpNumber] NVARCHAR(70),
[LatestEpTitle] NVARCHAR(200),
[LatestEpAirDate] NVARCHAR(100),
[NextEpNumber] NVARCHAR(70),
[NextEpTitle] NVARCHAR(200),
[NextEpAirDate] NVARCHAR(100),
[NextEpAirTime] NVARCHAR(100),
[RemindersSent] NVARCHAR(3) CHECK([RemindersSent] IN ('YES', 'NO')),
CONSTRAINT PK_tblTVShows_ShowID PRIMARY KEY (ShowID));

CREATE TABLE tblSubscription 
([UserID] INT NOT NULL,
[ShowID] INT NOT NULL,
CONSTRAINT PK_tblSubscription_ShowIDUserID PRIMARY KEY (ShowID, UserID));

INSERT INTO tblUser VALUES ('George', 'Smith', 'test@mail.com', '+447814690125', 'password')
INSERT INTO tblUser VALUES ('James', 'Small', 'example@mail.com', '',  'password')
INSERT INTO tblUser VALUES ('Bob', 'White', 'mail@example.com', '+447599016092', 'password')
INSERT INTO tblUser VALUES ('Sarah', 'Walker', 'test@example.com', '+447589016092', 'password')
INSERT INTO tblUser VALUES ('Jenna', 'Hugh', 'email@example.com', '+447591016092', 'password')

SET DATEFORMAT dmy;
delete from tblTVShows;
INSERT INTO tblTVShows VALUES (130715, 'Arrow', 'New Series', '01x19', 'Unfinished Business', '03/04/2013', '01x20', 'Home Invasion', '24/04/2013', '24/04/2013 01:00:00', 'YES');
INSERT INTO tblTVShows VALUES (16190, 'Simpsons', 'NReturning Series', '24x17', 'What Animated Women Want', '14/04/2013', '01x20', 'Pulpit Friction', '28/04/2013', '29/04/2013 01:00:00', 'NO');
INSERT INTO tblTVShows VALUES (116356, 'Mad Men', 'Returning Series', '06x03', 'The Collaborators', '14/04/2013', '01x20', 'To Have and to Hold', '21/04/2013', '22/04/2013 03:00:00', 'NO');
INSERT INTO tblTVShows VALUES (121686, 'Parks and Recreation', 'New Series', '05x18', 'Animal Control', '11/04/2013', '01x20', 'Article Two', '19/04/2013', '19/04/2013 02:00:00', 'NO');
INSERT INTO tblTVShows VALUES (125189, 'Nikita', 'Returning Series', '03x17', 'Masks', '12/04/2013', '01x20', 'Broken Home', '19/04/2013', '20/04/2013 01:00:00', 'NO');

INSERT INTO tblSubscription VALUES (1, 1);
INSERT INTO tblSubscription VALUES (1, 2);
INSERT INTO tblSubscription VALUES (1, 3);
INSERT INTO tblSubscription VALUES (1, 4);
INSERT INTO tblSubscription VALUES (2, 5);
INSERT INTO tblSubscription VALUES (2, 4);
INSERT INTO tblSubscription VALUES (2, 3);
INSERT INTO tblSubscription VALUES (3, 2);
INSERT INTO tblSubscription VALUES (3, 1);
INSERT INTO tblSubscription VALUES (3, 5);
INSERT INTO tblSubscription VALUES (4, 5);
INSERT INTO tblSubscription VALUES (4, 2);
INSERT INTO tblSubscription VALUES (4, 1);
INSERT INTO tblSubscription VALUES (5, 5);