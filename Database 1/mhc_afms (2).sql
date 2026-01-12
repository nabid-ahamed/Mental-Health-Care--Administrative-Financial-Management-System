-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 05, 2026 at 08:11 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `mhc_afms`
--

-- --------------------------------------------------------

--
-- Table structure for table `appointments`
--

CREATE TABLE `appointments` (
  `Id` int(11) NOT NULL,
  `PatientId` varchar(255) NOT NULL,
  `AppointmentDate` datetime(6) NOT NULL,
  `Status` longtext NOT NULL,
  `CreatedAt` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Description` longtext NOT NULL,
  `DoctorId` varchar(255) NOT NULL,
  `Diagnosis` longtext NOT NULL,
  `Prescription` longtext NOT NULL,
  `ConsultationFee` decimal(18,2) NOT NULL DEFAULT 0.00,
  `EndTime` time(6) NOT NULL DEFAULT '00:00:00.000000',
  `StartTime` time(6) NOT NULL DEFAULT '00:00:00.000000',
  `FollowUpDate` datetime(6) DEFAULT NULL,
  `IsInvoiceGenerated` tinyint(1) NOT NULL DEFAULT 0,
  `TotalAmount` decimal(18,2) NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `appointments`
--

INSERT INTO `appointments` (`Id`, `PatientId`, `AppointmentDate`, `Status`, `CreatedAt`, `Description`, `DoctorId`, `Diagnosis`, `Prescription`, `ConsultationFee`, `EndTime`, `StartTime`, `FollowUpDate`, `IsInvoiceGenerated`, `TotalAmount`) VALUES
(1, '287a0a45-5890-474c-9f66-488b780b481e', '2025-12-27 01:23:00.000000', 'Completed', '2025-12-17 22:34:28.495647', 'osusthota', '31d377b2-a336-48f1-bb46-a2eb6dfb0818', 'Gastric ', 'Entacyd Plus', 0.00, '00:00:00.000000', '00:00:00.000000', NULL, 0, 0.00),
(2, '287a0a45-5890-474c-9f66-488b780b481e', '2026-01-01 01:14:00.000000', 'Cancelled', '2025-12-18 01:17:27.993882', 'Headache \r\n', '31d377b2-a336-48f1-bb46-a2eb6dfb0818', '', '', 0.00, '00:00:00.000000', '00:00:00.000000', NULL, 0, 0.00),
(3, '0471357e-89e1-413b-a49f-e0b6f8ee14a0', '2025-12-30 00:00:00.000000', 'Pending', '2025-12-19 19:06:00.839444', '', 'b533fbef-6ede-4794-ae25-335f3fa3c976', '', '', 1000.00, '19:45:00.000000', '19:30:00.000000', NULL, 0, 0.00),
(4, '287a0a45-5890-474c-9f66-488b780b481e', '2025-12-30 00:00:00.000000', 'Pending', '2025-12-19 19:06:34.035736', '', 'b533fbef-6ede-4794-ae25-335f3fa3c976', '', '', 1000.00, '19:30:00.000000', '19:15:00.000000', NULL, 0, 0.00),
(5, '0471357e-89e1-413b-a49f-e0b6f8ee14a0', '2025-12-20 00:00:00.000000', 'Completed', '2025-12-19 19:24:16.069048', '', '31d377b2-a336-48f1-bb46-a2eb6dfb0818', 'Osteoporosis', 'Calbo D', 0.00, '09:15:00.000000', '09:00:00.000000', '2025-12-21 00:00:00.000000', 0, 0.00),
(6, '0471357e-89e1-413b-a49f-e0b6f8ee14a0', '2025-12-20 00:00:00.000000', 'Completed', '2025-12-19 19:27:13.740692', '', '31d377b2-a336-48f1-bb46-a2eb6dfb0818', 'qwearstyuhjk', 'asedrftghj', 0.00, '11:15:00.000000', '11:00:00.000000', NULL, 0, 0.00),
(7, '287a0a45-5890-474c-9f66-488b780b481e', '2025-12-30 00:00:00.000000', 'Approved', '2025-12-19 19:59:21.405867', 'asdfghjk', 'b533fbef-6ede-4794-ae25-335f3fa3c976', '', '', 1000.00, '20:30:00.000000', '20:15:00.000000', NULL, 0, 0.00),
(8, '461f81ec-3d61-4aef-9526-62484e45084a', '2025-12-30 00:00:00.000000', 'Completed', '2025-12-19 22:06:22.310022', 'Emni', 'b533fbef-6ede-4794-ae25-335f3fa3c976', 'Anxiety disorders', 'Lexapro, Valium', 1000.00, '20:00:00.000000', '19:45:00.000000', NULL, 1, 600.00),
(9, '461f81ec-3d61-4aef-9526-62484e45084a', '2025-12-20 00:00:00.000000', 'Completed', '2025-12-20 20:07:35.672192', 'Mental Problem', 'b533fbef-6ede-4794-ae25-335f3fa3c976', 'Brain Tumor', 'lomustine (CCNU)', 1000.00, '15:15:00.000000', '15:00:00.000000', '2025-12-22 00:00:00.000000', 0, 0.00),
(10, '461f81ec-3d61-4aef-9526-62484e45084a', '2025-12-20 00:00:00.000000', 'Completed', '2025-12-20 21:01:37.952392', 'sdfghjkl;', '31d377b2-a336-48f1-bb46-a2eb6dfb0818', 'bbbbbb', 'bbbbb', 800.00, '10:15:00.000000', '10:00:00.000000', '2025-12-31 00:00:00.000000', 1, 800.00),
(11, '461f81ec-3d61-4aef-9526-62484e45084a', '2025-12-23 00:00:00.000000', 'Completed', '2025-12-22 13:47:06.581090', 'manosik somossa', '31d377b2-a336-48f1-bb46-a2eb6dfb0818', 'hgfdsfgh', 'fgfggffgfgfggfgfgf', 800.00, '13:15:00.000000', '13:00:00.000000', NULL, 0, 0.00),
(12, '461f81ec-3d61-4aef-9526-62484e45084a', '2025-12-26 00:00:00.000000', 'Completed', '2025-12-25 02:09:00.453617', 'aaaaa', '58fddc7b-ca7f-4223-89d4-ce513daf5a17', 'aaaa', 'aaaaaa', 1200.00, '21:15:00.000000', '21:00:00.000000', NULL, 1, 3200.00),
(13, '461f81ec-3d61-4aef-9526-62484e45084a', '2025-12-27 00:00:00.000000', 'Completed', '2025-12-26 01:28:04.486458', 'ababababab', '31d377b2-a336-48f1-bb46-a2eb6dfb0818', 'abababab', 'abababab', 800.00, '09:30:00.000000', '09:15:00.000000', NULL, 1, 480.00);

-- --------------------------------------------------------

--
-- Table structure for table `appointmenttests`
--

CREATE TABLE `appointmenttests` (
  `Id` int(11) NOT NULL,
  `AppointmentId` int(11) NOT NULL,
  `MedicalTestId` int(11) NOT NULL,
  `Status` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `appointmenttests`
--

INSERT INTO `appointmenttests` (`Id`, `AppointmentId`, `MedicalTestId`, `Status`) VALUES
(1, 11, 5, 'Suggested'),
(2, 12, 5, 'Completed'),
(3, 10, 6, 'Rejected');

-- --------------------------------------------------------

--
-- Table structure for table `doctorschedules`
--

CREATE TABLE `doctorschedules` (
  `Id` int(11) NOT NULL,
  `DoctorId` varchar(255) NOT NULL,
  `DayOfWeek` longtext NOT NULL,
  `StartHour` int(11) NOT NULL,
  `EndHour` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `doctorschedules`
--

INSERT INTO `doctorschedules` (`Id`, `DoctorId`, `DayOfWeek`, `StartHour`, `EndHour`) VALUES
(1, '31d377b2-a336-48f1-bb46-a2eb6dfb0818', 'Monday', 9, 16),
(2, '31d377b2-a336-48f1-bb46-a2eb6dfb0818', 'Tuesday', 9, 16),
(3, '31d377b2-a336-48f1-bb46-a2eb6dfb0818', 'Wednesday', 9, 16),
(4, '31d377b2-a336-48f1-bb46-a2eb6dfb0818', 'Thursday', 9, 16),
(6, '31d377b2-a336-48f1-bb46-a2eb6dfb0818', 'Saturday', 9, 12),
(7, '31d377b2-a336-48f1-bb46-a2eb6dfb0818', 'Friday', 9, 12),
(8, 'b533fbef-6ede-4794-ae25-335f3fa3c976', 'Monday', 15, 22),
(9, 'b533fbef-6ede-4794-ae25-335f3fa3c976', 'Tuesday', 15, 22),
(10, 'b533fbef-6ede-4794-ae25-335f3fa3c976', 'Wednesday', 15, 22),
(11, 'b533fbef-6ede-4794-ae25-335f3fa3c976', 'Thursday', 15, 22),
(12, 'b533fbef-6ede-4794-ae25-335f3fa3c976', 'Friday', 15, 22),
(13, 'b533fbef-6ede-4794-ae25-335f3fa3c976', 'Saturday', 15, 17),
(20, '58fddc7b-ca7f-4223-89d4-ce513daf5a17', 'Saturday', 21, 22),
(21, '58fddc7b-ca7f-4223-89d4-ce513daf5a17', 'Sunday', 21, 22),
(22, '58fddc7b-ca7f-4223-89d4-ce513daf5a17', 'Friday', 21, 22);

-- --------------------------------------------------------

--
-- Table structure for table `invoices`
--

CREATE TABLE `invoices` (
  `Id` int(11) NOT NULL,
  `AppointmentId` int(11) NOT NULL,
  `Amount` decimal(18,2) NOT NULL,
  `Status` longtext NOT NULL,
  `DueDate` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `PatientId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `invoices`
--

INSERT INTO `invoices` (`Id`, `AppointmentId`, `Amount`, `Status`, `DueDate`, `PatientId`) VALUES
(1, 1, 62.00, 'Paid', '2026-01-17 01:47:54.453008', '287a0a45-5890-474c-9f66-488b780b481e'),
(2, 9, 400.00, 'Paid', '2026-01-19 20:12:22.669483', '461f81ec-3d61-4aef-9526-62484e45084a'),
(3, 12, 3200.00, 'Unpaid', '2026-01-24 02:18:46.357803', '461f81ec-3d61-4aef-9526-62484e45084a'),
(4, 10, 800.00, 'Paid', '2026-01-25 01:26:43.512035', '461f81ec-3d61-4aef-9526-62484e45084a'),
(5, 13, 480.00, 'Paid', '2026-01-25 01:29:05.649176', '461f81ec-3d61-4aef-9526-62484e45084a'),
(6, 8, 600.00, 'Unpaid', '2026-02-03 17:13:22.502622', '461f81ec-3d61-4aef-9526-62484e45084a');

-- --------------------------------------------------------

--
-- Table structure for table `medicaltests`
--

CREATE TABLE `medicaltests` (
  `Id` int(11) NOT NULL,
  `TestName` longtext NOT NULL,
  `Price` decimal(18,2) NOT NULL,
  `IsActive` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `medicaltests`
--

INSERT INTO `medicaltests` (`Id`, `TestName`, `Price`, `IsActive`) VALUES
(1, 'Blood Tests', 200.00, 1),
(2, 'Urine Tests', 300.00, 1),
(3, 'Electroencephalogram', 1000.00, 1),
(4, 'MRI', 1200.00, 1),
(5, 'CT Scan', 2000.00, 1),
(6, 'X-Ray', 690.00, 1);

-- --------------------------------------------------------

--
-- Table structure for table `payments`
--

CREATE TABLE `payments` (
  `Id` int(11) NOT NULL,
  `InvoiceId` int(11) NOT NULL,
  `PaymentMethod` longtext NOT NULL,
  `PaymentStatus` longtext NOT NULL,
  `PaymentDate` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `roleclaims`
--

CREATE TABLE `roleclaims` (
  `Id` int(11) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `roles`
--

CREATE TABLE `roles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `roles`
--

INSERT INTO `roles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`) VALUES
('2347f83d-2a4d-4489-acc2-908784feeb83', 'Admin', 'ADMIN', NULL),
('33b05560-8cbb-4956-bf19-de42205e304a', 'Staff', 'STAFF', NULL),
('6a3ef29a-b9ff-4aea-8ee7-f9868942e9e3', 'Doctor', 'DOCTOR', NULL),
('a42dc825-f0dd-4e26-a15c-ed6419c217bc', 'Patient', 'PATIENT', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `userclaims`
--

CREATE TABLE `userclaims` (
  `Id` int(11) NOT NULL,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `userlogins`
--

CREATE TABLE `userlogins` (
  `LoginProvider` varchar(255) NOT NULL,
  `ProviderKey` varchar(255) NOT NULL,
  `ProviderDisplayName` longtext DEFAULT NULL,
  `UserId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `userroles`
--

CREATE TABLE `userroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `userroles`
--

INSERT INTO `userroles` (`UserId`, `RoleId`) VALUES
('0471357e-89e1-413b-a49f-e0b6f8ee14a0', 'a42dc825-f0dd-4e26-a15c-ed6419c217bc'),
('116d3e8d-61bf-4724-9d4d-a19b3e1c147f', '2347f83d-2a4d-4489-acc2-908784feeb83'),
('287a0a45-5890-474c-9f66-488b780b481e', 'a42dc825-f0dd-4e26-a15c-ed6419c217bc'),
('31d377b2-a336-48f1-bb46-a2eb6dfb0818', '6a3ef29a-b9ff-4aea-8ee7-f9868942e9e3'),
('432440b7-404b-4b0f-9955-d0de0fa422b2', '33b05560-8cbb-4956-bf19-de42205e304a'),
('461f81ec-3d61-4aef-9526-62484e45084a', 'a42dc825-f0dd-4e26-a15c-ed6419c217bc'),
('58fddc7b-ca7f-4223-89d4-ce513daf5a17', '6a3ef29a-b9ff-4aea-8ee7-f9868942e9e3'),
('b533fbef-6ede-4794-ae25-335f3fa3c976', '6a3ef29a-b9ff-4aea-8ee7-f9868942e9e3');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `Id` varchar(255) NOT NULL,
  `FullName` longtext NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext DEFAULT NULL,
  `SecurityStamp` longtext DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  `PhoneNumber` longtext DEFAULT NULL,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `Specialization` longtext NOT NULL,
  `ConsultationFee` decimal(18,2) NOT NULL DEFAULT 0.00,
  `Address` longtext NOT NULL,
  `ContactNumber` longtext NOT NULL,
  `Age` int(11) DEFAULT NULL,
  `Gender` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`Id`, `FullName`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`, `Specialization`, `ConsultationFee`, `Address`, `ContactNumber`, `Age`, `Gender`) VALUES
('0471357e-89e1-413b-a49f-e0b6f8ee14a0', 'JAWADUR RAFID', 'patient01@hospital.com', 'PATIENT01@HOSPITAL.COM', 'patient01@hospital.com', 'PATIENT01@HOSPITAL.COM', 0, 'AQAAAAIAAYagAAAAEBP+g6pxbhWpI1bC5AF0IoPV9p4YI+eToI6ySvLzHszZRWs665E+hSG8Awg6yUky8g==', '5GMKOZROBYIUSYSCXUZX344B3T3KLVGE', '82d3308c-4332-41fb-a77d-6a79b7d1ce2f', NULL, 0, 0, NULL, 1, 0, '', 0.00, '', '', NULL, NULL),
('116d3e8d-61bf-4724-9d4d-a19b3e1c147f', 'Super Administrator', 'admin@hospital.com', 'ADMIN@HOSPITAL.COM', 'admin@hospital.com', 'ADMIN@HOSPITAL.COM', 1, 'AQAAAAIAAYagAAAAENVYYyN4yJjy/79wENg6Z6uEoBbFsh9rHMYcUBL2qJM3NAQdMK5lr4hmLXFdRjRABw==', 'ZP567SL73S6KJ76CWTDDXM32UDGWYRD4', 'ea1a4125-b8b6-45aa-959e-951f8d42c03f', NULL, 0, 0, NULL, 1, 0, '', 0.00, '', '', NULL, NULL),
('287a0a45-5890-474c-9f66-488b780b481e', 'John Doe', 'patient@hospital.com', 'PATIENT@HOSPITAL.COM', 'patient@hospital.com', 'PATIENT@HOSPITAL.COM', 1, 'AQAAAAIAAYagAAAAECAbbIrE6Q7CoP/ukPXdmZlYLYTmHAPDrkTldWwv7jVsldfANe9KDEXMpoHxtH2hBQ==', 'Q2FSAQ2DHCQ7LAKOT4DW3IBELTVQGWJP', '9fd2a2ad-6765-4e05-bbcd-e833d7d073c1', NULL, 0, 0, NULL, 1, 0, '', 0.00, '', '', NULL, NULL),
('31d377b2-a336-48f1-bb46-a2eb6dfb0818', 'Doctor Strange', 'doctor@hospital.com', 'DOCTOR@HOSPITAL.COM', 'doctor@hospital.com', 'DOCTOR@HOSPITAL.COM', 1, 'AQAAAAIAAYagAAAAECXlNHsqO7YlYKkL/D06XOuLrm/nPlXojxxLQxbfsY2Wlo/p27HctE08hN7vgHVpNA==', 'VFLZCBN66EHFU2OBDKXRSUJI6MX2MCE4', '7cfe35d5-959c-4ac9-9dee-4ac6b9dcd62f', NULL, 0, 0, NULL, 1, 0, 'Psychotherapist', 800.00, '13 California 1', '01700022244', NULL, NULL),
('432440b7-404b-4b0f-9955-d0de0fa422b2', 'Nurse Joya', 'staff@hospital.com', 'STAFF@HOSPITAL.COM', 'staff@hospital.com', 'STAFF@HOSPITAL.COM', 1, 'AQAAAAIAAYagAAAAEGOu9/1bAGu2klVxDsgfpoPOn3CQvg/eOli8UWGrZN5f1HnwAReE+IzYWyq4a2zt6A==', 'RX4X7WMZHUEM3YQJNMSZP5Z2GQDV2TSA', 'a7ce27a4-f1e1-49ab-81f8-f4d7b84b16ea', NULL, 0, 0, NULL, 1, 0, '', 0.00, '', '', 25, 'Female'),
('461f81ec-3d61-4aef-9526-62484e45084a', 'Nabid Ahamed Noushad', 'gmail@nabid.com', 'GMAIL@NABID.COM', 'gmail@nabid.com', 'GMAIL@NABID.COM', 0, 'AQAAAAIAAYagAAAAEEV/bkbaL0DeUHY3goZxJ8iVPuZZgys1IRBXiYtNmA0RF3yrBOSCwLslBri/+wRogg==', 'IFKHRLC622YNWUZW3OKR52ULVVWGGHWQ', 'b74a2083-7e56-4118-b903-2a5d76eacb98', NULL, 0, 0, NULL, 1, 0, '', 0.00, 'Road 9, Sector 9, Uttara', '01766833432', NULL, NULL),
('58fddc7b-ca7f-4223-89d4-ce513daf5a17', 'Myron Rolle', 'myronrolle@doctor.com', 'MYRONROLLE@DOCTOR.COM', 'myronrolle@doctor.com', 'MYRONROLLE@DOCTOR.COM', 1, 'AQAAAAIAAYagAAAAEAj8KEBpaAYmmSsJ//yjdHLvcsvUURSSumrNXMv9w7+HBREIjhEPN+wMWR09OFQmCw==', 'BVK7DMMNG4OEJ3MHRYTGRTWM43ADP7KG', '9c9b3b63-3768-478b-8f8f-d18043948006', NULL, 0, 0, NULL, 1, 0, 'Neurosurgeon', 1200.00, '', '', NULL, NULL),
('b533fbef-6ede-4794-ae25-335f3fa3c976', 'Dr. Elena Ramirez', 'dr.elena@gmail.com', 'DR.ELENA@GMAIL.COM', 'dr.elena@gmail.com', 'DR.ELENA@GMAIL.COM', 1, 'AQAAAAIAAYagAAAAENT4EtFsh2pJgwJqji/aOGdBIR28oPMUeLH2fgzO5njeLWYwXNNBEYGHEfMJjMgPsA==', 'SJT4Y44W34MYEWFREDQNERIUW5QTO2FW', '0d2bd67d-30ee-4c71-95ab-0f13a6bd06a7', NULL, 0, 0, NULL, 1, 0, 'Psychiatrist', 1000.00, '', '', NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `usertokens`
--

CREATE TABLE `usertokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Value` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20251217154221_InitialCreate', '8.0.11'),
('20251217155015_RenameIdentityTables', '8.0.11'),
('20251217163016_AddAppointmentFields', '8.0.11'),
('20251217192447_AddDiagnosis', '8.0.11'),
('20251217194218_UpdateInvoice', '8.0.11'),
('20251217201645_AddSpecialization', '8.0.11'),
('20251218130303_AddedFinancialModels', '8.0.11'),
('20251218152554_AddedDoctorSchedule', '8.0.11'),
('20251218154651_UpdateAppointmentTable', '8.0.11'),
('20251219130113_AddedAddressAndPhone', '8.0.11'),
('20251220130639_AddTestsAndFollowUp', '8.0.11'),
('20251224194009_AddAdvancedBillingLogic', '8.0.11'),
('20251225192143_AddInvoiceRelations', '8.0.11'),
('20251225203102_AddAgeGenderToUser', '8.0.11');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `appointments`
--
ALTER TABLE `appointments`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Appointments_DoctorId` (`DoctorId`),
  ADD KEY `IX_Appointments_PatientId` (`PatientId`);

--
-- Indexes for table `appointmenttests`
--
ALTER TABLE `appointmenttests`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AppointmentTests_AppointmentId` (`AppointmentId`),
  ADD KEY `IX_AppointmentTests_MedicalTestId` (`MedicalTestId`);

--
-- Indexes for table `doctorschedules`
--
ALTER TABLE `doctorschedules`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_DoctorSchedules_DoctorId` (`DoctorId`);

--
-- Indexes for table `invoices`
--
ALTER TABLE `invoices`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Invoices_AppointmentId` (`AppointmentId`),
  ADD KEY `IX_Invoices_PatientId` (`PatientId`);

--
-- Indexes for table `medicaltests`
--
ALTER TABLE `medicaltests`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `payments`
--
ALTER TABLE `payments`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `roleclaims`
--
ALTER TABLE `roleclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_RoleClaims_RoleId` (`RoleId`);

--
-- Indexes for table `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `RoleNameIndex` (`NormalizedName`);

--
-- Indexes for table `userclaims`
--
ALTER TABLE `userclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_UserClaims_UserId` (`UserId`);

--
-- Indexes for table `userlogins`
--
ALTER TABLE `userlogins`
  ADD PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  ADD KEY `IX_UserLogins_UserId` (`UserId`);

--
-- Indexes for table `userroles`
--
ALTER TABLE `userroles`
  ADD PRIMARY KEY (`UserId`,`RoleId`),
  ADD KEY `IX_UserRoles_RoleId` (`RoleId`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  ADD KEY `EmailIndex` (`NormalizedEmail`);

--
-- Indexes for table `usertokens`
--
ALTER TABLE `usertokens`
  ADD PRIMARY KEY (`UserId`,`LoginProvider`,`Name`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `appointments`
--
ALTER TABLE `appointments`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `appointmenttests`
--
ALTER TABLE `appointmenttests`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `doctorschedules`
--
ALTER TABLE `doctorschedules`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT for table `invoices`
--
ALTER TABLE `invoices`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `medicaltests`
--
ALTER TABLE `medicaltests`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `payments`
--
ALTER TABLE `payments`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `roleclaims`
--
ALTER TABLE `roleclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `userclaims`
--
ALTER TABLE `userclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `appointments`
--
ALTER TABLE `appointments`
  ADD CONSTRAINT `FK_Appointments_Users_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `users` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Appointments_Users_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `appointmenttests`
--
ALTER TABLE `appointmenttests`
  ADD CONSTRAINT `FK_AppointmentTests_Appointments_AppointmentId` FOREIGN KEY (`AppointmentId`) REFERENCES `appointments` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_AppointmentTests_MedicalTests_MedicalTestId` FOREIGN KEY (`MedicalTestId`) REFERENCES `medicaltests` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `doctorschedules`
--
ALTER TABLE `doctorschedules`
  ADD CONSTRAINT `FK_DoctorSchedules_Users_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `invoices`
--
ALTER TABLE `invoices`
  ADD CONSTRAINT `FK_Invoices_Appointments_AppointmentId` FOREIGN KEY (`AppointmentId`) REFERENCES `appointments` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Invoices_Users_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `roleclaims`
--
ALTER TABLE `roleclaims`
  ADD CONSTRAINT `FK_RoleClaims_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `roles` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `userclaims`
--
ALTER TABLE `userclaims`
  ADD CONSTRAINT `FK_UserClaims_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `userlogins`
--
ALTER TABLE `userlogins`
  ADD CONSTRAINT `FK_UserLogins_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `userroles`
--
ALTER TABLE `userroles`
  ADD CONSTRAINT `FK_UserRoles_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `roles` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_UserRoles_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `usertokens`
--
ALTER TABLE `usertokens`
  ADD CONSTRAINT `FK_UserTokens_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
