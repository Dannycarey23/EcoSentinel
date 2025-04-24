# DATABASE SQL QUERIES
## DOCKER SETUP 
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=ecoSentinel25" -p 1433:1433 --name ecoSentinel1 --hostname ecoSentinel -d mcr.microsoft.com/mssql/server:2022-latest

## AZURE DATA STUDIO SETUP
Connect to DOCKER container using details set above

## SETUP ADMIN USER 
On MASTER database perform the query below
CREATE LOGIN AdminUser WITH PASSWORD='ecoSentinel25';

## SETUP ADS DATABASE
Setup a new Database in ADS called ecoSentinel
Change over to that Database and perform the queries below

## SETUP AdminUser IN NEW DATABASE
CREATE user AdminUser for login AdminUser;
GRANT control on DATABASE::ecoSentinel to AdminUser;

Logout of the connection in ADS
Login as AdminUser
ensure you are connected to the ecoSentinel database
Create the database tables using the SQL below

## CREATE TABLES 
--Create users table
CREATE TABLE users (
    userID INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(15) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
	role VARCHAR (7) NOT NULL,
	email VARCHAR (255) NOT NULL,
	fname VARCHAR (25) NOT NULL,
	lname VARCHAR (25) NOT NULL,
	CONSTRAINT chk_pass CHECK (LEN(password)>= 12),
	CONSTRAINT chk_role CHECK (role IN ('Admin', 'EnvSci', 'OpsMan'))
);

--Create sensors table
CREATE TABLE sensors (
	sensorID INT IDENTITY (1,1),
	sensorType VARCHAR(25) NOT NULL,
	sensorStatus VARCHAR(3) NOT NULL,
	latitude DECIMAL(9,6) NOT NULL,
	longitude DECIMAL(9,6) NOT NULL,
	siteName VARCHAR (50) NOT NULL,
	siteType VARCHAR (25) NOT NULL,
	PRIMARY KEY (sensorID),
	CONSTRAINT chk_status CHECK (sensorStatus IN ('ON', 'OFF')),
	CONSTRAINT chk_siteType CHECK (siteType IN ('Urban', 'Country', 'Residential'))
);

--Create waterdata table
CREATE TABLE waterdata (
	dataID int IDENTITY (1,1),
	sensorID INT NOT NULL,
	sensorType VARCHAR(25) NOT NULL,
	date DATE NOT NULL,
	time TIME NOT NULL,
	nitrateMgl1 FLOAT,
	nitrateLessThanMgL1 FLOAT,
	phosphateMgl1 FLOAT,
	ecCfu100ml FLOAT,
	PRIMARY KEY (dataID),
    FOREIGN KEY (sensorID) REFERENCES sensors(sensorID)
);

--Create airdata table
CREATE TABLE airdata (
    dataID int IDENTITY (1,1),
	sensorID INT NOT NULL,
	sensorType VARCHAR(25) NOT NULL,
	zone VARCHAR (50),
	agglomeration VARCHAR(50),
	localAuthority VARCHAR (50),
	date DATE NOT NULL,
	time TIME NOT NULL,
	nitrogenDioxide FLOAT,
	sulfurDioxide FLOAT,
	pmTwoPointFive FLOAT,
	pmTen FLOAT,
	PRIMARY KEY (dataID),
    FOREIGN KEY (sensorID) REFERENCES sensors(sensorID)
);

--Create weatherdata table
CREATE TABLE weatherdata (
	dataID int IDENTITY (1,1),
    sensorID INT NOT NULL,
	sensorType VARCHAR(25) NOT NULL,
	elevation FLOAT,
	utcOffsetSeconds int,
	timezone VARCHAR(25),
	timezoneAbr VARCHAR(5),
	date DATE NOT NULL,
	time TIME NOT NULL,
	temp2m FLOAT,
	relativeHumidity2m FLOAT,
	windSpeed FLOAT,
	windDirection INT,
	PRIMARY KEY (dataID),
    FOREIGN KEY (sensorID) REFERENCES sensors(sensorID)
);

--Create issues table
CREATE TABLE issues (
	issueID INT IDENTITY (1,1) PRIMARY KEY,
    sensorID INT NOT NULL,
	sensorType VARCHAR(25) NOT NULL,
	date DATE NOT NULL,
	time TIME NOT NULL,
	reportedBy VARCHAR(15),
	issueDescription VARCHAR(255),
    FOREIGN KEY (sensorID) REFERENCES sensors(sensorID),
	FOREIGN KEY (reportedBy) REFERENCES users (username)
);

--Create maintenance table
CREATE TABLE maintenance (
	maintenanceID INT IDENTITY (1,1) PRIMARY KEY,
    sensorID INT NOT NULL,
	sensorType VARCHAR(25) NOT NULL,
	date DATE NOT NULL,
	time TIME NOT NULL,
	performedBy VARCHAR(15) NOT NULL,
	scheduledBy VARCHAR(15) NOT NULL,
    FOREIGN KEY (sensorID) REFERENCES sensors(sensorID),
	FOREIGN KEY (performedBy) REFERENCES users (username),
	FOREIGN KEY (scheduledBy) REFERENCES users (username)
);

# Insert Statements

## Users TABLE
insert into users (username,[password],role,email,fname,lname)
VALUES 
('Admin','EcoSentinel25','Admin','40783357@live.napier.ac.uk','Admin','User'),
('DavidMc','EcoSentinel2025','Admin','40783357@live.napier.ac.uk','David','McIntyre'),
('DannyC','EcoSentinel4Life','Admin','40783356@live.napier.ac.uk','Danny','Carey'),
('EdwardE','octoberThirteenth','EnvSci','fma@email.com','Edward','Elric'),
('WinryR','iLuvAutomail3000','OpsMan','rockbellWorks@email.com','Winry','Rockbell'),
('LinkK','heroOfTimeN64','EnvSci','hyaaaah@email.com','Link','Kokiri'),
('ZeldaH','peachSucksN64','OpsMan','tough_princess@email.com','Zelda','Hyrule');

## sensors table

INSERT INTO sensors (sensorType, sensorStatus, latitude, longitude, siteName, siteType) VALUES ('air', 'ON', 55.94476, -3.183991, 'Edinburgh Nicolson Street', 'Urban' );
INSERT INTO sensors (sensorType, sensorStatus, latitude, longitude, siteName, siteType) VALUES ('water', 'ON', 55.8611111, -3.25388888, 'Glencorse B', 'Country');
INSERT INTO sensors (sensorType, sensorStatus, latitude, longitude, siteName, siteType) VALUES ('weather', 'ON', 55.008785, -3.5856323, 'GibbonHill Farm', 'Country');

## airdata table
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '01:00:00', 26.3925, 1.59654, 5.094, 8.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '02:00:00', 22.5675, 1.33045, 5.094, 7.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '03:00:00', 14.535, 1.4635, 5.755, 8.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '04:00:00', 17.9775, 1.33045, 5.943, 9.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '05:00:00', 12.24, 1.33045, 6.132, 10.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '06:00:00', 23.90625, 1.4635, 6.415, 9.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '07:00:00', 22.95, 1.13088, 5.755, 9.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '08:00:00', 51.44625, 1.19741, 5.472, 9.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '09:00:00', 67.89375, 1.72959, 6.604, 11.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '10:00:00', 51.6375, 1.53002, 5.755, 9.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '11:00:00', 36.3375, 1.33045, 4.717, 8.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '12:00:00', 41.11875, 1.79611, 5.943, 9.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '13:00:00', 42.64875, 1.66306, 6.509, 10.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '14:00:00', 22.5675, 1.66306, 5.66, 9.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '15:00:00', 39.97125, 1.86263, 6.792, 10.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '16:00:00', 28.305, 1.86263, 7.547, 11.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '17:00:00', 26.775, 1.13088, 6.698, 11.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '18:00:00', 33.08625, 1.59654, 7.17, 10.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '19:00:00', 33.66, 1.99568, 7.076, 10.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '20:00:00', 50.29875, 2.26177, 8.774, 12.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '21:00:00', 38.05875, 2.19524, 7.925, 10.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '22:00:00', 44.94375, 2.26177, 8.396, 11.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-01', '23:00:00', 28.6875, 1.99568, 7.83, 10);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '00:00:00', 30.2175, 1.79611, 6.981, 9.4),;
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '01:00:00', 19.69875, 1.59654, 6.698, 9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '02:00:00', 18.7425, 1.41915, 6.792, 9.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '03:00:00', 12.81375, 1.4635, 6.604, 8.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '04:00:00', 16.83, 1.26393, 6.226, 7.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '05:00:00', 8.415, 1.39697, 5.094, 6.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '06:00:00', 8.22375, 1.39697, 5.566, 7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '07:00:00', 9.18, 1.53002, 4.811, 5.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '08:00:00', 17.9775, 1.72959, 3.962, 5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '09:00:00', 36.72, 2.19524, 3.962, 5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '10:00:00', 15.3, 1.66306, 2.453, 3.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '11:00:00', 20.08125, 1.59654, 2.264, 2.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '12:00:00', 19.89, 1.53002, 2.075, 2.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '13:00:00', 20.2725, 1.4635, 1.792, 2.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '14:00:00', 17.40375, 1.39697, 1.132, 2.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '15:00:00', 11.66625, 1.66306, 0.849, 1.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '16:00:00', 14.1525, 1.39697, 0.755, 1.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '17:00:00', 24.0975, 1.39697, 1.321, 1.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '18:00:00', 25.05375, 1.59654, 1.604, 2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '19:00:00', 26.20125, 1.39697, 4.057, 4.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '20:00:00', 16.4475, 1.4635, 2.453, 2.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '21:00:00', 22.75875, 1.59654, 2.075, 2.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '22:00:00', 17.2125, 1.53002, 2.359, 2.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-02', '23:00:00', 17.78625, 1.39697, 1.792, 2.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '00:00:00', 15.6825, 1.59654, 1.132, 1.4),;
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '01:00:00', 10.3275, 1.41915, 1.604, 2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '02:00:00', 4.78125, 1.50784, 1.604, 2.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '03:00:00', 3.825, 1.4635, 1.132, 1.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '04:00:00', 2.6775, 1.4635, 1.038, 1.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '05:00:00', 3.825, 1.53002, 1.226, 1.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '06:00:00', 7.65, 1.4635, 1.509, 2.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '07:00:00', 5.7375, 1.53002, 1.509, 2.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '08:00:00', 13.3875, 1.33045, 2.17, 3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '09:00:00', 13.3875, 1.66306, 2.547, 4.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '10:00:00', 11.28375, 1.66306, 2.264, 4.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '11:00:00', 10.3275, 1.59654, 1.415, 2.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '12:00:00', 11.0925, 1.53002, 1.038, 2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '13:00:00', 11.66625, 1.53002, 2.547, 5.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '14:00:00', 11.66625, 1.53002, 2.642, 5.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '15:00:00', 8.60625, 1.53002, 2.359, 5.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '16:00:00', 8.98875, 1.33045, 2.83, 6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '17:00:00', 9.75375, 1.59654, 3.679, 6.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '18:00:00', 9.75375, 1.59654, 3.868, 6.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '19:00:00', 9.945, 1.66306, 3.868, 6.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '20:00:00', 13.3875, 1.59654, 4.528, 7.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '21:00:00', 11.66625, 1.72959, 5.189, 8.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '22:00:00', 8.415, 1.72959, 3.868, 6.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-03', '23:00:00', 4.9725, 2.12872, 1.604, 2.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '00:00:00', 2.86875, 1.66306, 1.038, 1.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '01:00:00', 2.86875, 1.59654, 0.66, 1.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '02:00:00', 2.10375, 1.59654, 0.472, 1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '03:00:00', 3.06, 1.66306, 0.283, 0.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '04:00:00', 1.53, 1.59654, 0.189, 0.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '05:00:00', 1.53, 1.72959, 0.377, 0.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '06:00:00', 1.9125, 1.59654, 0.189, 0.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '07:00:00', 4.01625, 1.66306, 0.283, 0.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '08:00:00', 6.12, 1.86263, 0.189, 0.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '09:00:00', 16.065, 1.59654, 0.283, 0.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '10:00:00', 13.77, 1.53002, 0.377, 1.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '11:00:00', 12.81375, 1.53002, 0.566, 1.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '12:00:00', 16.63875, 1.59654, 0.849, 2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '13:00:00', 28.6875, 1.66306, 3.302, 5.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '14:00:00', 23.14125, 1.4635, 1.887, 4.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '15:00:00', 15.87375, 1.59654, 2.453, 5.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '16:00:00', 15.87375, 1.86263, 3.774, 8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '17:00:00', 18.55125, 1.99568, 3.679, 7.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '18:00:00', 14.34375, 1.79611, 2.453, 4.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '19:00:00', 26.58375, 1.59654, 1.415, 2.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '20:00:00', 20.46375, 1.53002, 1.132, 2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '21:00:00', 16.63875, 1.26393, 1.321, 1.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '22:00:00', 18.55125, 1.19741, 1.792, 3.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-04', '23:00:00', 11.66625, 1.59654, 1.604, 3.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '00:00:00', 12.6225, 1.59654, 1.792, 3.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '01:00:00', 17.9775, 1.24175, 1.887, 4.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '02:00:00', 6.885, 1.06436, 2.17, 4.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '03:00:00', 4.59, 0.93132, 2.925, 5.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '04:00:00', 3.06, 1.19741, 3.396, 6.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '05:00:00', 5.16375, 1.59654, 2.925, 5.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '06:00:00', 6.5025, 1.53002, 2.547, 5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '07:00:00', 9.75375, 1.66306, 1.887, 3.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '08:00:00', 20.2725, 1.4635, 2.17, 4.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '09:00:00', 26.01, 0.53218, 2.075, 4.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '10:00:00', 26.58375, 0.39914, 3.113, 8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '11:00:00', 20.08125, 0.66523, 3.774, 8.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '12:00:00', NULL, 0.86479, 3.962, 9.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '13:00:00', 17.9775, 1.19741, 4.245, 9.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '14:00:00', 30.6, 0.99784, 4.528, 10.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '15:00:00', 23.52375, 1.13088, 3.774, 8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '16:00:00', 22.75875, 1.06436, 3.585, 7.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '17:00:00', 26.775, 0.99784, 4.528, 9.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '18:00:00', 28.11375, 0.99784, 4.34, 8.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '19:00:00', 41.50125, 0.79827, 5.094, 9.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '20:00:00', 26.58375, 0.79827, 5.094, 9.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '21:00:00', 24.28875, 1.19741, 5.755, 11.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '22:00:00', 23.52375, 1.19741, 5.283, 11.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-05', '23:00:00', 27.73125, 0.79827, 4.811, 10.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '00:00:00', 22.37625, 0.79827, 4.528, 9.6); 
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '01:00:00', 16.63875, 0.88697, 4.245, 8.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '01:00:00', 16.63875, 0.88697, 4.245, 8.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '02:00:00', 10.3275, 0.79827, 4.245, 8.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '03:00:00', 13.3875, 0.93132, 4.434, 9.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '04:00:00', 14.1525, 0.86479, 4.151, 8.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '05:00:00', 16.83, 0.86479, 3.868, 7.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '06:00:00', 23.715, 0.73175, 3.868, 8.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '07:00:00', 48.00375, 0.79827, 3.868, 7.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '08:00:00', 69.04125, 1.66306, 4.434, 8.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '09:00:00', 55.08, 1.53002, 4.717, 9.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '10:00:00', 53.9325, 1.26393, 4.528, 8.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '11:00:00', 40.1625, 1.39697, 6.132, 12.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '12:00:00', 43.9875, 1.66306, 7.736, 16.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '13:00:00', 51.82875, 2.46133, 10.377, 19.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '14:00:00', 59.09625, 2.86047, 14.717, 24.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '15:00:00', 41.11875, 1.79611, 9.906, 17.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '16:00:00', 40.545, 1.33045, 7.264, 13.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '17:00:00', 49.91625, 1.53002, 6.887, 12.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '18:00:00', 51.6375, 1.19741, 6.509, 11.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '19:00:00', 51.44625, 1.26393, 6.509, 11.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '20:00:00', 48.195, 1.06436, 7.83, 11.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '21:00:00', 38.05875, 0.86479, 6.792, 9.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '22:00:00', 34.8075, 0.79827, 8.585, 11.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-06', '23:00:00', 40.1625, 0.86479, 10, 13.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '00:00:00', 48.38625, 1.06436, 7.83, 10.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '01:00:00', 41.11875, 0.88697, 6.792, 9.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '02:00:00', 19.125, 0.88697, 5.566, 8.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '03:00:00', 12.04875, 1.06436, 5.66, 7.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '04:00:00', 8.7975, 0.93132, 4.811, 5.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '05:00:00', 10.90125, 1.13088, 4.434, 5.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '06:00:00', 21.22875, 0.66523, 3.774, 4.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '07:00:00', 30.9825, 1.13088, 3.679, 5.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '08:00:00', 29.07, 0.99784, 5.377, 6.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '09:00:00', 42.4575, 0.86479, 7.17, 8.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '10:00:00', 37.29375, 1.06436, 5.094, 8.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '11:00:00', 19.31625, 1.13088, 4.528, 8.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '12:00:00', 13.3875, 0.99784, 5.377, 9.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '13:00:00', 11.28375, 1.19741, 5.472, 11.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '14:00:00', 9.75375, 1.59654, 5.943, 12.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '15:00:00', 12.6225, 1.4635, 5.755, 12.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '16:00:00', 14.535, 1.26393, 5.189, 15);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '17:00:00', 13.57875, 1.33045, 6.226, 17.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '18:00:00', 17.595, 1.26393, 6.321, 15.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '19:00:00', 15.6825, 1.66306, 6.415, 14.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '20:00:00', 17.595, 1.79611, 6.321, 13.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '21:00:00', 11.8575, 1.59654, 6.226, 14.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '22:00:00', 12.43125, 1.53002, 6.038, 14.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-07', '23:00:00', 9.37125, 1.59654, 5.094, 12);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '00:00:00', 9.37125, 1.13088, 5.283, 12.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '01:00:00', 9.945, 0.97566, 5, 11.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '02:00:00', 10.71, 1.68524, 4.811, 11.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '03:00:00', 8.415, 1.53002, 4.811, 11.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '04:00:00', 7.45875, 1.06436, 4.057, 8.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '05:00:00', 4.78125, 1.19741, 3.019, 6.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '06:00:00', 8.415, 1.33045, 3.302, 6.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '07:00:00', 11.8575, 1.4635, 2.359, 5.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '08:00:00', 16.065, 1.39697, 2.453, 5.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '09:00:00', 12.81375, 1.19741, 1.887, 4.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '10:00:00', 14.1525, 1.39697, 1.981, 5.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '11:00:00', 19.69875, 1.53002, 1.981, 4.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '12:00:00', 34.23375, 1.66306, 2.925, 5.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '13:00:00', 16.4475, NULL, 3.585, 7.7); 
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '14:00:00', 19.125, 2.04002, 6.132, 11.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '15:00:00', 18.55125, 1.66306, 6.038, 12.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '16:00:00', 13.77, 1.33045, 7.736, 13.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '17:00:00', 21.0375, 1.4635, 9.623, 14.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '18:00:00', 22.185, 1.59654, 11.509, 18.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '19:00:00', 28.49625, 1.4635, 10, 14.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '20:00:00', 28.49625, 1.59654, 12.453, 20.5);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '21:00:00', 18.93375, 1.59654, 8.962, 12.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '22:00:00', 12.04875, 1.59654, 9.151, 12.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-08', '23:00:00', 13.77, 1.39697, 11.793, 15.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '00:00:00', 12.24, 1.19741, 15.377, 24.3); 
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '01:00:00', 8.7975, 0.88697, 17.264, 25.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '02:00:00', 12.43125, 0.79827, 17.264, 26);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '03:00:00', 10.71, 0.93132, 15, 24.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '04:00:00', 6.31125, 1.13088, 14.245, 22.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '05:00:00', 4.78125, 1.26393, 11.793, 17.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '06:00:00', 15.3, 0.99784, 10.283, 16.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '07:00:00', 16.25625, 1.06436, 10.755, 21.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '08:00:00', 19.125, 0.5987, 11.226, 23.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '09:00:00', 18.55125, 0.93132, 11.793, 18.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '10:00:00', 32.895, 1.26393, 13.396, 22.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '11:00:00', 11.475, 1.26393, 11.226, 15.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '12:00:00', 10.90125, 1.13088, 12.264, 16);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '13:00:00', 9.945, 0.99784, 16.321, 21.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '14:00:00', 13.19625, 1.06436, 18.585, 24.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '15:00:00', 13.57875, 1.19741, 22.359, 28.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '16:00:00', 9.37125, 1.13088, 25.094, 32.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '17:00:00', 11.8575, 0.79827, 23.774, 30.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '18:00:00', 11.475, 1.19741, 21.321, 27.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '19:00:00', 10.71, 1.13088, 21.321, 27.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '20:00:00', 12.24, 1.4635, 16.132, 19.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '21:00:00', 8.98875, 1.33045, 16.51, 21.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '22:00:00', 9.945, 1.13088, 11.887, 16);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-09', '23:00:00', 8.7975, 0.93132, 9.151, 13.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '00:00:00', 15.10875, 0.73175, 11.887, 19); 
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '01:00:00', 4.9725, 0.88697, 11.321, 18);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '02:00:00', 4.2075, 1.24175, 10.283, 16.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '03:00:00', 3.825, 0.79827, 8.774, 14.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '04:00:00', 3.4425, 0.86479, 6.981, 13.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '05:00:00', 3.25125, 0.99784, 5.377, 11.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '06:00:00', 3.825, 0.73175, 6.415, 13.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '07:00:00', 5.92875, 1.33045, 6.509, 9.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '08:00:00', 11.28375, 0.93132, 4.811, 7.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '09:00:00', 14.72625, 1.13088, 4.717, 8.3);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '10:00:00', 11.475, 1.06436, 4.245, 7.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '11:00:00', 11.0925, 0.93132, 5.377, 10.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '12:00:00', 8.22375, 0.93132, 6.038, 12.2);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '13:00:00', 8.415, 1.13088, 5, 9.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '14:00:00', 8.22375, 1.26393, 6.887, 12.8);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '15:00:00', 8.0325, 1.06436, 6.415, 11.6);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '16:00:00', 9.37125, 1.33045, 6.981, 13.4);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '17:00:00', 11.28375, 1.13088, 6.321, 11.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '18:00:00', 11.475, 0.93132, 5.377, 9.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '19:00:00', 8.22375, 1.13088, 5.943, 10.9);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '20:00:00', 10.51875, 0.79827, 5.094, 9.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '21:00:00', 10.3275, 1.26393, 5, 9.1);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '22:00:00', 10.13625, 1.13088, 6.132, 11.7);
INSERT INTO airdata (sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen) VALUES (1, 'air', 'Central Scotland', 'Edinburgh Urban Area', 'Edinburgh', '2025-02-10', '23:00:00', 7.07625, 1.13088, 6.509, 11);


## waterdata table
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '01:00:00', 26.33, 1.33, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '02:00:00', 23.4, 1.52, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '03:00:00', 28.9, 1.32, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '04:00:00', 22.54, 1.41, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '05:00:00', 29.36, 1.61, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '06:00:00', 25.19, 1.42, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '07:00:00', 25.39, 1.26, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '08:00:00', 23.18, 1.63, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '09:00:00', 27.25, 1.26, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '10:00:00', 29.07, 1.4, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '11:00:00', 25.7, 1.52, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '12:00:00', 27.38, 1.26, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '13:00:00', 23.65, 1.33, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '14:00:00', 21.96, 1.42, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '15:00:00', 28.31, 1.28, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '16:00:00', 26.87, 1.21, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '17:00:00', 26.75, 1.64, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '18:00:00', 25.65, 1.42, 0.01, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '19:00:00', 25.2, 1.65, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '20:00:00', 27.00, 1.46, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '21:00:00', 26.54, 1.35, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '22:00:00', 24.15, 1.36, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-01', '23:00:00', 23.72, 1.52, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '00:00:00', 20.89, 1.33, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '01:00:00', 21.82, 1.22, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '02:00:00', 25.15, 1.35, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '03:00:00', 25.83, 1.46, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '04:00:00', 27.57, 1.21, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '05:00:00', 25.22, 1.65, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '06:00:00', 28.57, 1.26, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '07:00:00', 26.45, 1.44, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '08:00:00', 22.04, 1.46, 0.01, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '09:00:00', 25.1, 1.23, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '10:00:00', 25.32, 1.67, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '11:00:00', 28.03, 1.57, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '12:00:00', 26.1, 1.36, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '13:00:00', 25.23, 1.67, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '14:00:00', 27.15, 1.28, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '15:00:00', 23.61, 1.57, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '16:00:00', 22.72, 1.66, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '17:00:00', 26.63, 1.64, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '18:00:00', 23.3, 1.44, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '19:00:00', 24.97, 1.3, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '20:00:00', 20.32, 1.67, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '21:00:00', 21.87, 1.53, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '22:00:00', 24.75, 1.48, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-02', '23:00:00', 22.53, 1.43, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '00:00:00', 29.15, 1.48, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '01:00:00', 20.54, 1.33, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '02:00:00', 22.22, 1.42, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '03:00:00', 23.14, 1.41, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '04:00:00', 22.21, 1.40, 0.01, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '05:00:00', 22.56, 1.20, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '06:00:00', 24.91, 1.33, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '07:00:00', 24.38, 1.24, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '08:00:00', 28.70, 1.55, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '09:00:00', 23.04, 1.41, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '10:00:00', 23.65, 1.39, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '11:00:00', 26.68, 1.58, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '12:00:00', 20.28, 1.65, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '13:00:00', 26.20, 1.46, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '14:00:00', 29.23, 1.65, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '15:00:00', 22.47, 1.32, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '16:00:00', 22.51, 1.20, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '17:00:00', 21.46, 1.25, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '18:00:00', 23.36, 1.52, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '19:00:00', 25.16, 1.23, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '20:00:00', 27.69, 1.47, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '21:00:00', 27.78, 1.33, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '22:00:00', 27.10, 1.44, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-03', '23:00:00', 22.82, 1.66, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '00:00:00', 26.38, 1.64, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '01:00:00', 27.12, 1.23, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '02:00:00', 28.21, 1.34, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '03:00:00', 27.50, 1.38, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '04:00:00', 28.90, 1.34, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '05:00:00', 24.44, 1.50, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '06:00:00', 25.27, 1.39, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '07:00:00', 26.09, 1.61, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '08:00:00', 22.64, 1.29, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '09:00:00', 21.95, 1.59, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '10:00:00', 28.91, 1.63, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '11:00:00', 23.95, 1.42, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '12:00:00', 27.64, 1.62, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '13:00:00', 27.08, 1.40, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '14:00:00', 25.35, 1.40, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '15:00:00', 27.85, 1.46, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '16:00:00', 27.00, 1.26, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '17:00:00', 28.64, 1.41, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '18:00:00', 22.28, 1.46, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '19:00:00', 27.09, 1.40, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '20:00:00', 27.23, 1.65, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '21:00:00', 21.46, 1.33, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '22:00:00', 26.14, 1.36, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-04', '23:00:00', 23.72, 1.21, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '00:00:00', 28.71, 1.61, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '01:00:00', 21.78, 1.38, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '02:00:00', 24.94, 1.56, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '03:00:00', 21.59, 1.26, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '04:00:00', 22.87, 1.53, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '05:00:00', 20.65, 1.63, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '06:00:00', 23.36, 1.55, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '07:00:00', 21.12, 1.54, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '08:00:00', 28.57, 1.44, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '09:00:00', 25.25, 1.26, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '10:00:00', 22.38, 1.34, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '11:00:00', 24.96, 1.66, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '12:00:00', 22.33, 1.58, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '13:00:00', 27.57, 1.48, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '14:00:00', 27.90, 1.29, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '15:00:00', 21.38, 1.22, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '16:00:00', 24.78, 1.27, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '17:00:00', 21.02, 1.30, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '18:00:00', 29.48, 1.47, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '19:00:00', 27.96, 1.59, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '20:00:00', 25.48, 1.48, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '21:00:00', 27.84, 1.21, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '22:00:00', 21.97, 1.56, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-05', '23:00:00', 21.17, 1.28, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '00:00:00', 27.05, 1.49, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '01:00:00', 27.05, 1.49, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '02:00:00', 24.97, 1.48, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '03:00:00', 21.11, 1.57, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '04:00:00', 28.66, 1.41, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '05:00:00', 21.09, 1.62, 0.01, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '06:00:00', 24.57, 1.36, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '07:00:00', 26.24, 1.57, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '08:00:00', 26.60, 1.31, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '09:00:00', 22.16, 1.46, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '10:00:00', 20.31, 1.32, 0.01, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '11:00:00', 26.93, 1.45, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '12:00:00', 23.78, 1.52, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '13:00:00', 26.07, 1.24, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '14:00:00', 27.17, 1.43, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '15:00:00', 27.61, 1.48, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '16:00:00', 28.21, 1.32, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '17:00:00', 28.38, 1.67, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '18:00:00', 23.84, 1.65, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '19:00:00', 25.56, 1.51, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '20:00:00', 22.72, 1.42, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '21:00:00', 29.42, 1.55, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '22:00:00', 26.47, 1.60, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-06', '23:00:00', 27.66, 1.58, 0.01, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '00:00:00', 29.28, 1.35, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '01:00:00', 29.50, 1.27, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '02:00:00', 28.42, 1.48, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '03:00:00', 28.28, 1.25, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '04:00:00', 20.18, 1.43, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '05:00:00', 23.57, 1.54, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '06:00:00', 26.41, 1.61, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '07:00:00', 20.96, 1.44, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '08:00:00', 21.34, 1.27, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '09:00:00', 25.57, 1.22, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '10:00:00', 28.83, 1.43, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '11:00:00', 20.32, 1.56, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '12:00:00', 26.20, 1.44, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '13:00:00', 24.24, 1.31, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '14:00:00', 21.19, 1.63, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '15:00:00', 23.16, 1.67, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '16:00:00', 24.07, 1.24, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '17:00:00', 22.77, 1.58, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '18:00:00', 28.92, 1.42, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '19:00:00', 28.13, 1.38, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '20:00:00', 25.27, 1.21, 0.01, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '21:00:00', 27.96, 1.20, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '22:00:00', 22.80, 1.26, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-07', '23:00:00', 26.88, 1.22, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '00:00:00', 24.79, 1.63, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '01:00:00', 20.56, 1.42, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '02:00:00', 27.26, 1.48, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '03:00:00', 24.26, 1.42, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '04:00:00', 26.80, 1.28, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '05:00:00', 23.00, 1.67, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '06:00:00', 20.78, 1.22, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '07:00:00', 23.03, 1.25, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '08:00:00', 20.86, 1.38, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '09:00:00', 25.40, 1.39, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '10:00:00', 22.64, 1.34, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '11:00:00', 22.97, 1.37, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '12:00:00', 29.19, 1.61, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '13:00:00', 29.52, 1.55, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '14:00:00', 25.78, 1.54, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '15:00:00', 25.28, 1.51, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '16:00:00', 26.81, 1.24, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '17:00:00', 23.60, 1.42, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '18:00:00', 27.20, 1.59, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '19:00:00', 21.41, 1.40, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '20:00:00', 22.39, 1.51, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '21:00:00', 22.52, 1.54, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '22:00:00', 22.43, 1.53, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-08', '23:00:00', 28.66, 1.56, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '00:00:00', 27.49, 1.37, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '01:00:00', 24.15, 1.53, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '02:00:00', 24.89, 1.48, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '03:00:00', 29.09, 1.27, 0.01, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '04:00:00', 23.01, 1.24, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '05:00:00', 27.52, 1.55, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '06:00:00', 24.75, 1.63, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '07:00:00', 29.33, 1.44, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '08:00:00', 27.41, 1.63, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '09:00:00', 21.35, 1.36, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '10:00:00', 23.78, 1.35, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '11:00:00', 28.31, 1.67, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '12:00:00', 23.31, 1.40, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '13:00:00', 21.45, 1.55, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '14:00:00', 20.72, 1.47, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '15:00:00', 27.57, 1.60, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '16:00:00', 26.73, 1.61, 0.01, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '17:00:00', 21.88, 1.31, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '18:00:00', 26.83, 1.30, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '19:00:00', 25.60, 1.21, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '20:00:00', 20.90, 1.27, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '21:00:00', 21.74, 1.57, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '22:00:00', 24.21, 1.57, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-09', '23:00:00', 25.39, 1.66, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '00:00:00', 28.34, 1.24, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '01:00:00', 23.98, 1.22, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '02:00:00', 24.60, 1.37, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '03:00:00', 20.88, 1.52, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '04:00:00', 22.54, 1.29, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '05:00:00', 23.16, 1.65, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '06:00:00', 25.23, 1.49, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '07:00:00', 21.88, 1.56, 0.07, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '08:00:00', 20.76, 1.61, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '09:00:00', 21.60, 1.50, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '10:00:00', 26.06, 1.41, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '11:00:00', 25.93, 1.27, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '12:00:00', 25.36, 1.50, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '13:00:00', 21.42, 1.55, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '14:00:00', 29.36, 1.52, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '15:00:00', 26.54, 1.21, 0.01, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '16:00:00', 27.17, 1.57, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '17:00:00', 20.27, 1.64, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '18:00:00', 25.27, 1.40, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '19:00:00', 27.89, 1.43, 0.01, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '20:00:00', 24.27, 1.52, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '21:00:00', 24.45, 1.43, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '22:00:00', 28.05, 1.30, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-10', '23:00:00', 28.54, 1.35, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-11', '00:00:00', 21.23, 1.27, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-11', '01:00:00', 28.35, 1.61, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-11', '02:00:00', 22.06, 1.51, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-11', '03:00:00', 23.93, 1.27, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-11', '04:00:00', 23.89, 1.35, 0.03, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-11', '05:00:00', 27.64, 1.46, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-11', '06:00:00', 23.97, 1.39, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-11', '07:00:00', 25.51, 1.54, 0.06, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-11', '08:00:00', 28.85, 1.47, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-11', '09:00:00', 23.58, 1.58, 0.05, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-11', '10:00:00', 24.96, 1.35, 0.04, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-11', '11:00:00', 24.06, 1.35, 0.02, 0);
INSERT INTO waterdata (sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml) VALUES (2, 'water','2025-02-11', '12:00:00', 27.09, 1.61, 0.06, 0);

## weatherdata table

INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '00:00:00', 0.6, 98, 1.18, 78);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '01:00:00', 2.4, 96, 0.93, 106);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '02:00:00', 2.5, 97, 1.08, 103);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '03:00:00', 2.4, 97, 1.53, 142);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '04:00:00', 1.9, 96, 2.15, 179);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '05:00:00', 3.1, 93, 3.35, 179);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '06:00:00', 3.5, 93, 3.65, 178);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '07:00:00', 3.5, 94, 3.46, 169);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '08:00:00', 3.3, 93, 3.38, 161);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '09:00:00', 3.0, 95, 3.36, 152);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '10:00:00', 4.0, 93, 3.61, 158);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '11:00:00', 5.5, 85, 4.29, 165);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '12:00:00', 5.9, 82, 4.63, 174);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '13:00:00', 6.5, 80, 5.50, 182);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '14:00:00', 6.5, 80, 5.40, 182);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '15:00:00', 6.2, 80, 4.96, 183);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '16:00:00', 5.7, 82, 5.10, 179);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '17:00:00', 5.5, 82, 4.94, 172);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '18:00:00', 5.7, 79, 4.88, 169);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '19:00:00', 5.6, 78, 4.83, 170);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '20:00:00', 5.5, 79, 4.78, 174);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '21:00:00', 5.5, 80, 5.36, 171);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '22:00:00', 5.5, 79, 5.66, 171);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-01', '23:00:00', 5.4, 80, 5.27, 171);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '00:00:00', 5.2, 83, 5.63, 174);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '01:00:00', 5.0, 80, 4.91, 168);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '02:00:00', 4.9, 82, 5.52, 171);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '03:00:00', 4.9, 86, 4.73, 170);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '04:00:00', 5.0, 88, 5.04, 173);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '05:00:00', 5.4, 85, 5.2, 172);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '06:00:00', 5.2, 86, 4.82, 167);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '07:00:00', 4.7, 95, 3.94, 165);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '08:00:00', 4.6, 96, 3.35, 160);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '09:00:00', 4.5, 96, 3.15, 162);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '10:00:00', 4.6, 96, 3.81, 175);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '11:00:00', 4.7, 96, 4.25, 177);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '12:00:00', 5.2, 92, 4.51, 177);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '13:00:00', 5.8, 91, 5.25, 194);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '14:00:00', 5.9, 91, 4.86, 197);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '15:00:00', 6.2, 89, 5.03, 201);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '16:00:00', 6.0, 92, 5.08, 197);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '17:00:00', 6.1, 94, 5.12, 199);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '18:00:00', 6.2, 95, 4.85, 194);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '19:00:00', 6.2, 95, 5.03, 190);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '20:00:00', 6.4, 95, 4.99, 191);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '21:00:00', 6.5, 96, 4.79, 194);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '22:00:00', 6.7, 97, 4.65, 197);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-02', '23:00:00', 6.8, 98, 4.16, 207);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '00:00:00', 7.0, 97, 3.99, 204);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '01:00:00', 7.0, 97, 3.91, 201);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '02:00:00', 7.1, 97, 4.76, 199);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '03:00:00', 7.2, 97, 5.16, 202);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '04:00:00', 7.1, 98, 5.1, 201);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '05:00:00', 6.9, 95, 5.63, 202);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '06:00:00', 6.2, 92, 5.74, 200);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '07:00:00', 6.4, 93, 5.67, 196);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '08:00:00', 6.6, 92, 5.9, 191);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '09:00:00', 7.2, 91, 5.89, 190);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '10:00:00', 7.9, 89, 6.42, 198);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '11:00:00', 8.5, 88, 7.16, 200);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '12:00:00', 8.8, 87, 7.8, 201);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '13:00:00', 9.0, 86, 7.62, 197);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '14:00:00', 8.8, 88, 7.56, 198);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '15:00:00', 8.7, 88, 7.61, 196);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '16:00:00', 8.6, 87, 8.51, 199);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '17:00:00', 8.3, 88, 8.74, 198);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '18:00:00', 8.4, 88, 8.62, 198);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '19:00:00', 8.5, 88, 8.5, 194);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '20:00:00', 8.5, 88, 8.2, 191);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '21:00:00', 8.6, 87, 8.08, 188);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '22:00:00', 8.7, 88, 8.32, 190);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-03', '23:00:00', 8.7, 88, 8.7, 191);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '00:00:00', 8.8, 88, 8.75, 191);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '01:00:00', 8.9, 88, 9.15, 188);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '02:00:00', 8.9, 88, 9.31, 191);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '03:00:00', 8.9, 88, 9.81, 190);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '04:00:00', 8.8, 90, 9.57, 187);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '05:00:00', 8.9, 90, 9.96, 188);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '06:00:00', 8.2, 97, 9.58, 190);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '07:00:00', 8.2, 97, 9.47, 192);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '08:00:00', 8.3, 97, 9.84, 197);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '09:00:00', 8.6, 97, 10.33, 203);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '10:00:00', 8.7, 97, 10.47, 204);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '11:00:00', 8.7, 98, 8.17, 217);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '12:00:00', 8.8, 97, 6.25, 235);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '13:00:00', 8.7, 89, 6.55, 244);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '14:00:00', 8.4, 86, 6.68, 225);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '15:00:00', 8.8, 82, 7.91, 220);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '16:00:00', 7.3, 83, 8.27, 213);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '17:00:00', 7.1, 79, 7.57, 226);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '18:00:00', 7.0, 79, 7.19, 240);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '19:00:00', 6.4, 81, 6.45, 257);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '20:00:00', 5.5, 83, 5.47, 254);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '21:00:00', 4.9, 85, 4.79, 243);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '22:00:00', 5.1, 84, 5.24, 240);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-04', '23:00:00', 4.8, 85, 5.37, 243);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '00:00:00', 4.5, 86, 5.35, 245);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '01:00:00', 4.4, 90, 5.04, 241);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '02:00:00', 4.3, 88, 4.92, 242);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '03:00:00', 4.1, 87, 4.52, 245);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '04:00:00', 3.7, 87, 4.33, 248);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '05:00:00', 3.4, 88, 4.24, 251);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '06:00:00', 3.2, 89, 4.00, 252);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '07:00:00', 2.9, 91, 3.86, 253);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '08:00:00', 2.6, 91, 3.39, 261);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '09:00:00', 3.0, 91, 3.02, 263);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '10:00:00', 4.5, 86, 2.72, 253);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '11:00:00', 6.0, 77, 2.4, 239);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '12:00:00', 6.9, 76, 3.03, 232);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '13:00:00', 7.8, 70, 4.27, 254);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '14:00:00', 8.1, 64, 4.28, 259);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '15:00:00', 8.0, 65, 3.69, 257);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '16:00:00', 7.1, 72, 2.52, 256);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '17:00:00', 5.2, 80, 1.77, 254);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '18:00:00', 3.5, 86, 1.75, 246);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '19:00:00', 2.4, 90, 1.72, 244);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '20:00:00', 1.6, 93, 1.6, 256);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '21:00:00', 0.9, 95, 1.57, 261);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '22:00:00', 0.4, 96, 1.31, 277);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-05', '23:00:00', 0.0, 97, 1.19, 298);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '00:00:00', -0.5, 98, 0.85, 315);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '01:00:00', -0.4, 97, 0.07, 45);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '02:00:00', -0.6, 96, 0.65, 67);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '03:00:00', -0.5, 97, 0.51, 61);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '04:00:00', -0.7, 98, 0.5, 37);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '05:00:00', -0.7, 98, 0.49, 24);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '06:00:00', -0.5, 99, 0.73, 344);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '07:00:00', -0.3, 98, 1.08, 326);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '08:00:00', -0.1, 99, 0.7, 4);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '09:00:00', 0.6, 98, 1.41, 354);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '10:00:00', 1.5, 98, 1.39, 15);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '11:00:00', 2.7, 98, 1.4, 35);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '12:00:00', 4.5, 91, 1.15, 34);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '13:00:00', 5.7, 87, 1.13, 13);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '14:00:00', 6.3, 84, 0.74, 48);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '15:00:00', 6.1, 85, 0.94, 58);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '16:00:00', 4.3, 91, 0.6, 48);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '17:00:00', 2.2, 93, 0.74, 48);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '18:00:00', 1.1, 94, 1.45, 49);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '19:00:00', 0.5, 92, 1.99, 65);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '20:00:00', 0.1, 93, 2.01, 71);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '21:00:00', -0.3, 94, 2.03, 74);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '22:00:00', -0.7, 94, 2.01, 76);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-06', '23:00:00', -0.6, 95, 2.13, 81);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '00:00:00', -0.7, 96, 2.01, 76);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '01:00:00', -0.4, 96, 2.2, 73);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '02:00:00', -0.1, 96, 2.5, 74);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '03:00:00', 0.2, 96, 2.58, 68);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '04:00:00', 0.5, 94, 2.76, 55);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '05:00:00', 0.4, 94, 3.0, 53);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '06:00:00', 0.5, 93, 3.0, 57);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '07:00:00', 0.4, 93, 2.44, 47);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '08:00:00', 0.8, 94, 3.36, 46);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '09:00:00', 1.9, 94, 4.5, 48);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '10:00:00', 3.3, 92, 5.2, 52);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '11:00:00', 4.8, 87, 5.19, 58);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '12:00:00', 6.0, 81, 5.35, 62);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '13:00:00', 6.2, 78, 6.27, 69);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '14:00:00', 6.3, 73, 5.87, 76);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '15:00:00', 5.8, 73, 5.83, 81);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '16:00:00', 4.7, 73, 5.64, 80);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '17:00:00', 4.0, 74, 5.21, 74);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '18:00:00', 2.8, 81, 4.77, 66);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '19:00:00', 2.1, 86, 4.47, 58);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '20:00:00', 1.8, 87, 4.34, 57);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '21:00:00', 1.9, 84, 4.6, 59);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '22:00:00', 2.5, 81, 4.99, 61);
INSERT INTO weatherdata (sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, datetime, temp2m, relativeHumidity2m, windSpeed, windDirection) VALUES (3, 'weather', 8, 0, 'GMT', 'GMT''2025-02-07', '23:00:00', 2.0, 84, 4.86, 60);