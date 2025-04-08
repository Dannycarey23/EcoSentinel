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
	latitude DECIMAL NOT NULL,
	longitude DECIMAL NOT NULL,
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
	datetime DATETIME NOT NULL,
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