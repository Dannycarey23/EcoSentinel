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
	latitude FLOAT NOT NULL,
	longitude FLOAT NOT NULL,
	siteName VARCHAR (50) NOT NULL,
	siteType VARCHAR (25) NOT NULL,
	PRIMARY KEY (sensorID, sensorType),
	CONSTRAINT chk_status CHECK (sensorStatus IN ('ON', 'OFF')),
	CONSTRAINT chk_siteType CHECK (siteType IN ('Urban', 'Country', 'Residential'))
);

--Create waterdata table
CREATE TABLE waterdata (
    samplePeriodStart DATE NOT NULL,
	samplePeriodEnd DATE NOT NULL,
	sensorID INT NOT NULL,
	sensorType VARCHAR(25) NOT NULL,
	date DATE NOT NULL,
	time TIME NOT NULL,
	nitrateMgl1 FLOAT,
	nitrateLessThanMgL1 FLOAT,
	phosphateMgl1 FLOAT,
	ecCfu100ml FLOAT,
	PRIMARY KEY (samplePeriodStart, samplePeriodEnd),
    FOREIGN KEY (sensorID, sensorType) REFERENCES sensors(sensorID, sensorType),
	CONSTRAINT chk_date CHECK (date BETWEEN samplePeriodStart AND samplePeriodEnd)
);

--Create airdata table
CREATE TABLE airdata (
    samplePeriodStart DATE NOT NULL,
	samplePeriodEnd DATE NOT NULL,
	sensorID INT NOT NULL,
	sensorType VARCHAR(25) NOT NULL,
	zone VARCHAR (25),
	agglomeration VARCHAR(25),
	localAuthority VARCHAR (25),
	date DATE NOT NULL,
	time TIME NOT NULL,
	nitrogenDioxide FLOAT,
	sulfurDioxide FLOAT,
	pmTwoPointFive FLOAT,
	pmTen FLOAT,
	PRIMARY KEY (samplePeriodStart, samplePeriodEnd),
    FOREIGN KEY (sensorID, sensorType) REFERENCES sensors(sensorID, sensorType),
	CONSTRAINT chk_dateAir CHECK (date BETWEEN samplePeriodStart AND samplePeriodEnd)
);

--Create weatherdata table
CREATE TABLE weatherdata (
	samplePeriodStart DATE NOT NULL,
	samplePeriodEnd DATE NOT NULL,
    sensorID INT NOT NULL,
	sensorType VARCHAR(25) NOT NULL,
	elevation FLOAT,
	utcOffsetSeconds TIME,
	timezone VARCHAR(25),
	timezoneAbr VARCHAR(5),
	datetime DATETIME NOT NULL,
	temp2m FLOAT,
	relativeHumidity2m FLOAT,
	windSpeed FLOAT,
	windDirection INT,
	PRIMARY KEY (samplePeriodStart, samplePeriodEnd),
    FOREIGN KEY (sensorID, sensorType) REFERENCES sensors(sensorID, sensorType),
	CONSTRAINT chk_dateWeather CHECK (datetime BETWEEN samplePeriodStart AND samplePeriodEnd)
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
    FOREIGN KEY (sensorID, sensorType) REFERENCES sensors(sensorID, sensorType),
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
    FOREIGN KEY (sensorID, sensorType) REFERENCES sensors(sensorID, sensorType),
	FOREIGN KEY (performedBy) REFERENCES users (username),
	FOREIGN KEY (scheduledBy) REFERENCES users (username)
);