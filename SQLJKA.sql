-- Crear la base de datos JKASensorData
CREATE DATABASE JKASensorDataV2;
GO

-- Usar la base de datos JKASensorData
USE JKASensorDataV2;
GO

-- Crear la tabla Users
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50),
    Company NVARCHAR(100),
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Approval BIT NOT NULL,
    Status BIT NOT NULL
);
GO

-- Crear la tabla Sensors
CREATE TABLE Sensors (
    SensorId INT PRIMARY KEY IDENTITY(1,1),
    SerialNumber NVARCHAR(50) NOT NULL UNIQUE,
    UserId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
GO

-- Crear la tabla Subscriptions
CREATE TABLE Subscriptions (
    Folio INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT CHK_SubscriptionDates CHECK (EndDate > StartDate)
);
GO

-- Crear la tabla Readings
CREATE TABLE Readings (
    ReadingId INT PRIMARY KEY IDENTITY(1,1),
    SensorId INT NOT NULL,
    ReadingDate DATE NOT NULL,
    ReadingTime TIME NOT NULL,
    Temperature FLOAT NOT NULL,
    Humidity FLOAT NOT NULL,
    Counter INT NOT NULL,
    FOREIGN KEY (SensorId) REFERENCES Sensors(SensorId)
);
GO

-- Crear la tabla Payments
CREATE TABLE Payments (
    PaymentFolio INT PRIMARY KEY IDENTITY(1,1),
    SubscriptionFolio INT NOT NULL,
    TransactionDate DATE NOT NULL,
    Total DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (SubscriptionFolio) REFERENCES Subscriptions(Folio)
);
GO

-- Crear la tabla LocationHistory
CREATE TABLE LocationHistory (
    LocationHistoryId INT PRIMARY KEY IDENTITY(1,1),
    SensorId INT NOT NULL,
    Longitude DECIMAL(9, 6) NOT NULL,
    Latitude DECIMAL(9, 6) NOT NULL,
    Timestamp DATETIME NOT NULL,
    FOREIGN KEY (SensorId) REFERENCES Sensors(SensorId)
);
GO

-- Insertar datos de prueba en Users
INSERT INTO Users (FirstName, LastName, MiddleName, Company, Email, Password, Approval, Status)
VALUES 
('Jaime', 'Lazcano', 'Macareno', 'Don Pisto', 'dpisto@gmail.com', '12345678', 1, 1),
('Maria', 'Gonzalez', 'Perez', 'La Casa', 'maria.g@gmail.com', 'password123', 1, 1),
('Luis', 'Martinez', 'Rodriguez', 'Tech Solutions', 'luis.m@gmail.com', 'securepass', 1, 1),
('Ana', 'Lopez', 'Sanchez', 'Food Corp', 'ana.l@gmail.com', 'mypassword', 1, 1),
('Carlos', 'Ramirez', 'Torres', 'Retail World', 'carlos.r@gmail.com', 'adminpass', 1, 1),
('Elena', 'Diaz', 'Fernandez', 'Smart Home', 'elena.d@gmail.com', 'userpassword', 1, 1),
('Jose', 'Martinez', 'Lopez', 'Innovatech', 'jose.m@gmail.com', 'passjose', 1, 1),
('Laura', 'Hernandez', 'Diaz', 'Green Energy', 'laura.h@gmail.com', 'passlaura', 1, 1),
('Miguel', 'Sanchez', 'Fernandez', 'AutoTech', 'miguel.s@gmail.com', 'passmiguel', 1, 1),
('Lucia', 'Ortega', 'Perez', 'TechWorld', 'lucia.o@gmail.com', 'passlucia', 1, 1),
('Pedro', 'Alvarez', 'Garcia', 'Agro Solutions', 'pedro.a@gmail.com', 'passwordPedro', 1, 1),
('Juan', 'Rios', 'Mendoza', 'Logistics', 'juan.r@gmail.com', 'juanpass', 1, 1),
('Roberto', 'Mendez', 'Diaz', 'Travel Inc', 'roberto.m@gmail.com', 'passroberto', 1, 1),
('Sofia', 'Reyes', 'Nunez', 'EduTech', 'sofia.r@gmail.com', 'sofiapass', 1, 1),
('Diego', 'Pineda', 'Gomez', 'HealthCorp', 'diego.p@gmail.com', 'diego123', 1, 1);
GO

-- Insertar datos de prueba en Sensors (eliminando duplicados)
INSERT INTO Sensors (SerialNumber, UserId)
VALUES 
('SN12345', 1),
('SN23456', 1),
('SN34567', 2),
('SN45678', 2),
('SN56789', 3),
('SN67890', 3),
('SN78901', 4),
('SN89012', 4),
('SN90123', 5),
('SN01234', 5),
('SN11111', 6),
('SN22222', 6),
('SN33333', 7),
('SN44444', 7),
('SN55555', 8);
GO

-- Insertar datos de prueba en Subscriptions
INSERT INTO Subscriptions (UserId, StartDate, EndDate)
VALUES 
(1, '2024-01-01', '2024-05-05'),
(2, '2024-02-01', '2024-06-03'),
(3, '2024-03-01', '2024-06-03'),
(4, '2024-04-01', '2024-06-03'),
(5, '2024-05-01', '2024-09-08'),
(6, '2024-06-01', '2024-10-07'),
(7, '2024-07-01', '2024-11-09'),
(8, '2024-08-01', '2024-12-10'),
(9, '2024-09-01', '2025-01-11'),
(10, '2024-10-01', '2025-02-12'),
(11, '2024-11-01', '2025-03-13'),
(12, '2024-12-01', '2025-04-14'),
(13, '2024-01-15', '2024-05-19'),
(14, '2024-02-20', '2024-06-22'),
(15, '2024-03-25', '2024-07-27');
GO

-- Insertar datos de prueba en Readings
INSERT INTO Readings (SensorId, ReadingDate, ReadingTime, Temperature, Humidity, Counter)
VALUES 
(1, '2023-07-15', '12:00:00', 22.5, 60, 40),
(1, '2023-07-15', '14:00:00', 23, 58, 42),
(1, '2023-07-16', '09:00:00', 21, 65, 30),
(2, '2023-07-15', '12:00:00', 22.5, 60, 40),
(2, '2023-07-15', '14:00:00', 23, 58, 42),
(2, '2023-07-16', '09:00:00', 21, 65, 30),
(3, '2023-08-01', '10:00:00', 24, 55, 38),
(4, '2023-08-01', '10:00:00', 24, 55, 38),
(5, '2023-09-01', '11:00:00', 25, 50, 45),
(6, '2023-09-01', '11:00:00', 25, 50, 45),
(7, '2023-10-01', '09:00:00', 26, 45, 50),
(8, '2023-10-01', '09:00:00', 26, 45, 50),
(9, '2023-11-01', '08:00:00', 27, 40, 55),
(10, '2023-11-01', '08:00:00', 27, 40, 55),
(11, '2023-12-01', '07:00:00', 28, 35, 60),
(12, '2023-12-01', '07:00:00', 28, 35, 60),
(13, '2023-12-01', '08:00:00', 28, 35, 60),
(14, '2023-12-01', '08:00:00', 28, 35, 60),
(15, '2023-12-01', '09:00:00', 29, 30, 65);
GO

-- Insertar datos de prueba en Payments
INSERT INTO Payments (SubscriptionFolio, TransactionDate, Total)
VALUES 
(1, '2024-01-02', 100.00),
(2, '2024-02-03', 150.00),
(3, '2024-03-04', 200.00),
(4, '2024-04-05', 250.00),
(5, '2024-05-06', 300.00),
(6, '2024-06-07', 350.00),
(7, '2024-07-08', 400.00),
(8, '2024-08-09', 450.00),
(9, '2024-09-10', 500.00),
(10, '2024-10-11', 550.00),
(11, '2024-11-12', 600.00),
(12, '2024-12-13', 650.00),
(13, '2024-01-16', 120.00),
(14, '2024-02-21', 170.00),
(15, '2024-03-26', 220.00);
GO

-- Insertar datos de prueba en LocationHistory
INSERT INTO LocationHistory (SensorId, Longitude, Latitude, Timestamp)
VALUES 
(1, -99.1332, 19.4326, '2023-07-15 12:00:00'),
(1, -99.1332, 19.4326, '2023-07-15 14:00:00'),
(2, -99.1342, 19.4336, '2023-07-15 12:00:00'),
(3, -99.1352, 19.4346, '2023-08-01 10:00:00'),
(4, -99.1362, 19.4356, '2023-08-01 10:00:00'),
(5, -99.1372, 19.4366, '2023-09-01 11:00:00'),
(6, -99.1382, 19.4376, '2023-09-01 11:00:00'),
(7, -99.1392, 19.4386, '2023-10-01 09:00:00'),
(8, -99.1402, 19.4396, '2023-10-01 09:00:00'),
(9, -99.1412, 19.4406, '2023-11-01 08:00:00'),
(10, -99.1422, 19.4416, '2023-11-01 08:00:00'),
(11, -99.1432, 19.4426, '2023-12-01 07:00:00'),
(12, -99.1442, 19.4436, '2023-12-01 07:00:00'),
(13, -99.1452, 19.4446, '2023-12-01 08:00:00'),
(14, -99.1462, 19.4456, '2023-12-01 08:00:00'),
(15, -99.1472, 19.4466, '2023-12-01 09:00:00');
GO

INSERT INTO Readings (SensorId, ReadingDate, ReadingTime, Temperature, Humidity, Counter) VALUES
(1, '2023-07-15', '00:00:00', 22.5, 60, 40),
(1, '2023-07-15', '01:00:00', 23.0, 61, 41),
(1, '2023-07-15', '02:00:00', 22.8, 62, 42),
(1, '2023-07-15', '03:00:00', 23.2, 63, 43),
(1, '2023-07-15', '04:00:00', 22.7, 64, 44),
(1, '2023-07-15', '05:00:00', 23.1, 65, 45),
(1, '2023-07-15', '06:00:00', 22.6, 66, 46),
(1, '2023-07-15', '07:00:00', 23.3, 67, 47),
(1, '2023-07-15', '08:00:00', 22.4, 68, 48),
(1, '2023-07-15', '09:00:00', 23.5, 69, 49),
(1, '2023-07-15', '10:00:00', 22.9, 70, 50),
(1, '2023-07-15', '11:00:00', 23.0, 71, 51),
(1, '2023-07-15', '12:00:00', 23.1, 72, 52),
(1, '2023-07-15', '13:00:00', 22.8, 73, 53),
(1, '2023-07-15', '14:00:00', 23.2, 74, 54),
(1, '2023-07-15', '15:00:00', 22.7, 75, 55),
(1, '2023-07-15', '16:00:00', 23.1, 76, 56),
(1, '2023-07-15', '17:00:00', 22.6, 77, 57),
(1, '2023-07-15', '18:00:00', 23.3, 78, 58),
(1, '2023-07-15', '19:00:00', 22.4, 79, 59),
(1, '2023-07-15', '20:00:00', 23.5, 80, 60),
(1, '2023-07-15', '21:00:00', 22.9, 81, 61),
(1, '2023-07-15', '22:00:00', 23.0, 82, 62),
(1, '2023-07-15', '23:00:00', 23.1, 83, 63),
(1, '2023-07-15', '00:30:00', 22.5, 64, 64),
(1, '2023-07-15', '01:30:00', 23.0, 65, 65),
(1, '2023-07-15', '02:30:00', 22.8, 66, 66),
(1, '2023-07-15', '03:30:00', 23.2, 67, 67),
(1, '2023-07-15', '04:30:00', 22.7, 68, 68),
(1, '2023-07-15', '05:30:00', 23.1, 69, 69);

GO

-- Crear la tabla UsersSU
CREATE TABLE UsersSU (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50),
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL
);

GO

-- Insertar usuarios admin de prueba
INSERT INTO UsersSU (FirstName, LastName, MiddleName, Email, Password)
VALUES 
('John', 'Doe', 'Alexander', 'john.doe@gmail.com', 'password123'),
('Jane', 'Smith', 'Francine', 'jane.smith@gmail.com', 'password456'),
('Michael', 'Brown', 'Benjamin', 'michael.brown@gmail.com', 'password789'),
('Emily', 'Davis', 'Samantha', 'emily.davis@gmail.com', 'password101'),
('David', 'Wilson', 'Christopher', 'david.wilson@gmail.com', 'password202'),
('Sarah', 'Miller', 'Danielle', 'sarah.miller@gmail.com', 'password303'),
('Robert', 'Moore', 'Edward', 'robert.moore@gmail.com', 'password404'),
('Jessica', 'Taylor', 'Quinn', 'jessica.taylor@gmail.com', 'password505'),
('Daniel', 'Anderson', 'Frederick', 'daniel.anderson@gmail.com', 'password606'),
('Laura', 'Thomas', 'Whitney', 'laura.thomas@gmail.com', 'password707'),
('James', 'Jackson', 'Gregory', 'james.jackson@gmail.com', 'password808'),
('Emma', 'White', 'Quinlan', 'emma.white@gmail.com', 'password909');

GO