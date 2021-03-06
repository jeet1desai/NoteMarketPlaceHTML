USE [master]
GO
/****** Object:  Database [NoteMarketPlace]    Script Date: 11-04-2021 12:33:38 ******/
CREATE DATABASE [NoteMarketPlace]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NoteMarketPlace', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.JEETDESAISQL\MSSQL\DATA\NoteMarketPlace.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NoteMarketPlace_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.JEETDESAISQL\MSSQL\DATA\NoteMarketPlace_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [NoteMarketPlace] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NoteMarketPlace].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NoteMarketPlace] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ARITHABORT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NoteMarketPlace] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NoteMarketPlace] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET  ENABLE_BROKER 
GO
ALTER DATABASE [NoteMarketPlace] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NoteMarketPlace] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET RECOVERY FULL 
GO
ALTER DATABASE [NoteMarketPlace] SET  MULTI_USER 
GO
ALTER DATABASE [NoteMarketPlace] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NoteMarketPlace] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NoteMarketPlace] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NoteMarketPlace] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'NoteMarketPlace', N'ON'
GO
ALTER DATABASE [NoteMarketPlace] SET QUERY_STORE = OFF
GO
USE [NoteMarketPlace]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 11-04-2021 12:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[CountryCode] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Downloads]    Script Date: 11-04-2021 12:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Downloads](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[Seller] [int] NOT NULL,
	[Downloader] [int] NOT NULL,
	[IsSellerHasAllowedDownload] [bit] NOT NULL,
	[AttachmentPath] [varchar](max) NULL,
	[IsAttachmentDownloaded] [bit] NOT NULL,
	[AttachmentDownloadedDate] [datetime] NULL,
	[IsPaid] [bit] NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
	[NoteTitle] [varchar](100) NOT NULL,
	[NoteCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteCategories]    Script Date: 11-04-2021 12:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteCategories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteTypes]    Script Date: 11-04-2021 12:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReferenceData]    Script Date: 11-04-2021 12:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReferenceData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](100) NOT NULL,
	[DataValue] [varchar](100) NOT NULL,
	[RefCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotes]    Script Date: 11-04-2021 12:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SellerID] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ActionedBy] [int] NULL,
	[AdminRemarks] [varchar](max) NULL,
	[PublishedDate] [datetime] NULL,
	[Title] [varchar](100) NOT NULL,
	[Category] [int] NOT NULL,
	[DisplayPicture] [varchar](500) NULL,
	[NoteType] [int] NULL,
	[NumberofPages] [int] NULL,
	[Description] [varchar](max) NOT NULL,
	[UniversityName] [varchar](200) NULL,
	[Country] [int] NULL,
	[Course] [varchar](100) NULL,
	[CourseCode] [varchar](100) NULL,
	[Professor] [varchar](100) NULL,
	[IsPaid] [bit] NOT NULL,
	[SellingPrice] [decimal](18, 0) NULL,
	[NotesPreview] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesAttachements]    Script Date: 11-04-2021 12:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesAttachements](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[FileName] [varchar](100) NOT NULL,
	[FilePath] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReportedIssues]    Script Date: 11-04-2021 12:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReportedIssues](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReportedByID] [int] NOT NULL,
	[AgainstDownloadID] [int] NOT NULL,
	[Remarks] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReviews]    Script Date: 11-04-2021 12:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReviews](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReviewedByID] [int] NOT NULL,
	[AgainstDownloadsID] [int] NOT NULL,
	[Ratings] [decimal](18, 0) NOT NULL,
	[Comments] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemConfigurations]    Script Date: 11-04-2021 12:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemConfigurations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 11-04-2021 12:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[DOB] [datetime] NULL,
	[Gender] [int] NULL,
	[SecondaryEmailAddress] [varchar](100) NULL,
	[PhoneNumberCountryCode] [varchar](5) NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[ProfilePicture] [varchar](500) NULL,
	[AddressLine1] [varchar](100) NOT NULL,
	[AddressLine2] [varchar](100) NOT NULL,
	[City] [varchar](50) NOT NULL,
	[State] [varchar](50) NOT NULL,
	[ZipCode] [varchar](50) NOT NULL,
	[Country] [varchar](50) NOT NULL,
	[University] [varchar](100) NULL,
	[College] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11-04-2021 12:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](24) NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersRole]    Script Date: 11-04-2021 12:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersRole](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 

INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'India', N'+91', NULL, 8, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'US', N'+1', NULL, 8, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'Canada', N'+2', NULL, 8, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'UK', N'+44', NULL, 8, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (7, N'Germany', N'+49', NULL, 8, NULL, NULL, 0)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, N'Australia', N'+61', NULL, 8, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1004, N'abcd', N'+21', CAST(N'2021-04-08T15:04:15.043' AS DateTime), 8, CAST(N'2021-04-08T15:09:22.773' AS DateTime), 8, 0)
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[Downloads] ON 

INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1043, 2076, 2002, 2002, 1, N'~/Members/2002/2076/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', 1, CAST(N'2021-03-23T15:20:12.013' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Note 13 - asdfgh', N'CA', CAST(N'2021-03-23T15:20:12.013' AS DateTime), 8, CAST(N'2021-03-23T16:03:11.630' AS DateTime), 8)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1044, 2054, 8, 2002, 1, N'~/Members/8/2054/Attachements/sample.pdf', 0, CAST(N'2021-03-23T18:35:28.590' AS DateTime), 1, CAST(126 AS Decimal(18, 0)), N'Note 1 - qwerty', N'Computer Eng', CAST(N'2021-03-23T16:01:22.317' AS DateTime), 8, CAST(N'2021-03-23T18:35:28.590' AS DateTime), 8)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1045, 2059, 8, 2002, 1, N'~/Members/8/2059/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', 1, CAST(N'2021-03-23T16:05:43.087' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Note 5 - qwerty', N'MBA', CAST(N'2021-03-23T16:05:43.087' AS DateTime), 8, CAST(N'2021-03-23T16:05:43.087' AS DateTime), 8)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1046, 2080, 2003, 2003, 1, N'~/Members/2003/2080/Attachements/Notes Marketplace - Admin.pdf', 0, CAST(N'2021-03-23T18:40:12.877' AS DateTime), 1, CAST(110 AS Decimal(18, 0)), N'note 17 - zxcvbn', N'MBA', CAST(N'2021-03-23T18:39:20.630' AS DateTime), 8, CAST(N'2021-03-23T18:41:26.203' AS DateTime), 2003)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1047, 2055, 8, 2003, 1, NULL, 0, NULL, 1, CAST(123 AS Decimal(18, 0)), N'Note 2 - qwerty', N'IT', CAST(N'2021-03-27T10:56:26.133' AS DateTime), 8, CAST(N'2021-03-27T10:56:26.133' AS DateTime), 8)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1048, 2058, 8, 2003, 1, N'~/Members/8/2058/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', 1, CAST(N'2021-03-27T10:56:50.540' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Note 4 - qwerty', N'IT', CAST(N'2021-03-27T10:56:50.543' AS DateTime), 8, CAST(N'2021-03-27T10:56:50.543' AS DateTime), 8)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1049, 2060, 8, 8, 1, NULL, 0, NULL, 1, CAST(123 AS Decimal(18, 0)), N'Note 6 - qwerty', N'Computer Eng', CAST(N'2021-03-27T10:58:20.573' AS DateTime), 8, CAST(N'2021-03-27T10:58:20.573' AS DateTime), 8)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1050, 2061, 8, 8, 1, NULL, 0, NULL, 1, CAST(134 AS Decimal(18, 0)), N'Note 7 - qwerty', N'CA', CAST(N'2021-03-27T10:58:38.767' AS DateTime), 8, CAST(N'2021-03-27T10:58:38.767' AS DateTime), 8)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1051, 2054, 8, 8, 1, NULL, 0, NULL, 1, CAST(126 AS Decimal(18, 0)), N'Note 1 - qwerty', N'Computer Eng', CAST(N'2021-03-27T10:59:36.877' AS DateTime), 2003, CAST(N'2021-03-27T10:59:36.877' AS DateTime), 2003)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1052, 2055, 8, 8, 1, NULL, 0, NULL, 1, CAST(123 AS Decimal(18, 0)), N'Note 2 - qwerty', N'IT', CAST(N'2021-03-27T10:59:52.500' AS DateTime), 2003, CAST(N'2021-03-27T10:59:52.500' AS DateTime), 2003)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1053, 2060, 8, 8, 1, NULL, 0, NULL, 1, CAST(123 AS Decimal(18, 0)), N'Note 6 - qwerty', N'Computer Eng', CAST(N'2021-03-27T11:00:12.720' AS DateTime), 2003, CAST(N'2021-03-27T11:00:12.720' AS DateTime), 2003)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1054, 2061, 8, 8, 1, NULL, 0, NULL, 1, CAST(134 AS Decimal(18, 0)), N'Note 7 - qwerty', N'CA', CAST(N'2021-03-27T11:00:28.177' AS DateTime), 2003, CAST(N'2021-03-27T11:00:28.177' AS DateTime), 2003)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1060, 2058, 8, 8, 1, NULL, 0, NULL, 1, CAST(134 AS Decimal(18, 0)), N'Note 1 - qwerty', N'Computer Eng', NULL, NULL, NULL, NULL)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1061, 2054, 8, 8, 1, NULL, 0, NULL, 1, CAST(126 AS Decimal(18, 0)), N'Note 1 - qwerty', N'Computer Eng', CAST(N'2021-04-03T15:03:51.293' AS DateTime), 8, CAST(N'2021-04-03T15:03:51.293' AS DateTime), 8)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1062, 2063, 2002, 8, 1, N'~/Members/2002/2063/Attachements/Notes Marketplace - Admin.pdf', 0, CAST(N'2021-04-03T22:03:05.220' AS DateTime), 1, CAST(123 AS Decimal(18, 0)), N'Note 9 - asdfgh', N'Computer Eng', CAST(N'2021-04-03T22:02:25.743' AS DateTime), 2002, CAST(N'2021-04-03T22:03:05.220' AS DateTime), 2002)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2061, 2059, 8, 3007, 1, N'~/Members/8/2059/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', 1, CAST(N'2021-04-09T10:45:11.457' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Note 5 - qwerty', N'MBA', CAST(N'2021-04-09T10:45:11.460' AS DateTime), 3007, CAST(N'2021-04-09T10:45:11.460' AS DateTime), 3007)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2071, 2055, 8, 2002, 0, NULL, 0, NULL, 1, CAST(123 AS Decimal(18, 0)), N'Note 2 - qwerty', N'IT', CAST(N'2021-04-09T16:17:12.767' AS DateTime), 2002, CAST(N'2021-04-09T16:17:12.767' AS DateTime), 2002)
SET IDENTITY_INSERT [dbo].[Downloads] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteCategories] ON 

INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Computer Eng', N'Computer Engineering', CAST(N'2021-04-07T21:37:03.000' AS DateTime), 8, CAST(N'2021-04-07T21:37:03.580' AS DateTime), 2004, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'IT', N'Information Technology', CAST(N'2021-04-07T21:37:03.000' AS DateTime), 8, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'CA', N'Chartered Accountant', CAST(N'2021-04-07T21:37:03.000' AS DateTime), 8, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'MBA', N'Master of Business Administration', CAST(N'2021-04-07T21:37:03.000' AS DateTime), 8, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'Story Book', N'Story Books', CAST(N'2021-04-07T21:37:03.000' AS DateTime), 8, NULL, NULL, 0)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1002, N'acf', N'qwertyu', CAST(N'2021-04-07T22:04:22.967' AS DateTime), 2004, CAST(N'2021-04-08T13:50:37.087' AS DateTime), 8, 0)
SET IDENTITY_INSERT [dbo].[NoteCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteTypes] ON 

INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Handwritten Book', N'handwritten books', NULL, 8, CAST(N'2021-04-08T14:06:26.267' AS DateTime), 8, 0)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'University Note', N'University Note', NULL, 8, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'Self-Write', N'self-write', NULL, 8, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'Novel', N'novel', NULL, 8, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1003, N'ad', N'qwertyq', CAST(N'2021-04-08T14:15:30.813' AS DateTime), 8, CAST(N'2021-04-08T14:25:17.217' AS DateTime), 8, 1)
SET IDENTITY_INSERT [dbo].[NoteTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[ReferenceData] ON 

INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Draft', N'Save', N'NoteStatus', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Submitted For Review', N'Submitted For Review', N'NoteStatus', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'In Review', N'In Review', N'NoteStatus', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'Published', N'Approved', N'NoteStatus', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'Rejected', N'Rejected', N'NoteStatus', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'Removed', N'Removed', N'NoteStatus', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (7, N'Male', N'M', N'Gender', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, N'Female', N'Fe', N'Gender', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, N'Unknown', N'U', N'Gender', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, N'Paid', N'P', N'SellingMode', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (11, N'Free', N'Fr', N'SellingMode', NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[ReferenceData] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotes] ON 

INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2054, 8, 4, 8, N'qertyu', CAST(N'2021-04-03T17:21:43.587' AS DateTime), N'Note 1 - qwerty', 1, N'~/Members/8/2054/Note1.jpg', 2, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 5, N'Enginnering', N'112233', N'Mr Desai', 1, CAST(126 AS Decimal(18, 0)), N'~/Members/8/2054/sample.pdf', CAST(N'2021-03-23T09:09:29.917' AS DateTime), 8, CAST(N'2021-04-04T12:04:24.850' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2055, 8, 4, 8, N'asdfgh', CAST(N'2021-04-03T17:21:59.833' AS DateTime), N'Note 2 - qwerty', 2, N'~/Members/8/2055/Note2.jfif', 2, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 2, N'Enginnering', N'112233', N'Mr Desai', 1, CAST(123 AS Decimal(18, 0)), N'~/Members/8/2055/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T09:22:25.930' AS DateTime), 8, CAST(N'2021-04-03T17:21:59.833' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2056, 8, 4, 8, N'asdfgh', CAST(N'2021-03-31T00:00:00.000' AS DateTime), N'Note 3 - qwerty', 3, N'~/Members/8/2056/Note4.jfif', 3, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 5, N'Enginnering', N'112233', N'Mr Desai', 0, CAST(0 AS Decimal(18, 0)), N'~/Members/8/2056/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T09:35:42.320' AS DateTime), 8, CAST(N'2021-04-03T17:23:42.650' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2058, 8, 4, 8, N'asdfgh', CAST(N'2021-03-31T00:00:00.000' AS DateTime), N'Note 4 - qwerty', 2, N'~/Members/8/2058/Note1.jpg', 2, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 8, N'CA', N'112233', N'Mr Desai', 0, CAST(0 AS Decimal(18, 0)), N'~/Members/8/2058/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T10:53:23.670' AS DateTime), 8, CAST(N'2021-03-23T10:53:23.670' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2059, 8, 4, 2003, NULL, CAST(N'2021-04-04T16:17:41.743' AS DateTime), N'Note 5 - qwerty', 4, N'~/Members/8/2059/Note4.jfif', 3, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 6, N'Enginnering', N'586497', N'Mr Desai', 0, CAST(0 AS Decimal(18, 0)), N'~/Members/8/2059/sample.pdf', CAST(N'2021-03-23T10:54:14.127' AS DateTime), 8, CAST(N'2021-04-04T16:17:41.743' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2060, 8, 4, 8, N'asdfgh', CAST(N'2021-03-31T00:00:00.000' AS DateTime), N'Note 6 - qwerty', 1, N'~/Members/8/2060/Note1.jpg', 3, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 2, N'Enginnering', N'586497', N'Mr Desai', 1, CAST(123 AS Decimal(18, 0)), N'~/Members/8/2060/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T10:55:31.267' AS DateTime), 8, CAST(N'2021-03-23T10:55:31.267' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2061, 8, 4, 8, N'asdfgh', CAST(N'2021-03-31T00:00:00.000' AS DateTime), N'Note 7 - qwerty', 3, N'~/Members/8/2061/Note6.jfif', 2, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 5, N'MBA', N'586497', N'Mr Desai', 1, CAST(134 AS Decimal(18, 0)), N'~/Members/8/2061/sample.pdf', CAST(N'2021-03-23T10:57:00.357' AS DateTime), 8, CAST(N'2021-03-23T10:57:00.357' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2063, 2002, 4, 8, N'asdfgh', CAST(N'2021-03-31T00:00:00.000' AS DateTime), N'Note 9 - asdfgh', 1, N'~/Members/2002/2063/Note7.jfif', 2, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 5, N'MBA', N'00112233', N'Mr Desai', 1, CAST(123 AS Decimal(18, 0)), N'~/Members/2002/2063/sample.pdf', CAST(N'2021-03-23T12:22:23.680' AS DateTime), 2002, CAST(N'2021-04-02T11:56:31.883' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2064, 2002, 4, 2002, N'asdfgh', CAST(N'2021-03-31T00:00:00.000' AS DateTime), N'Note 10 - asdfgh', 4, N'~/Members/2002/2064/Note1.jpg', 1, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 6, N'CA', N'586497', N'Mr Desai', 1, CAST(123 AS Decimal(18, 0)), N'~/Members/2002/2064/sample.pdf', CAST(N'2021-03-23T12:40:33.090' AS DateTime), 2002, CAST(N'2021-03-23T12:40:33.090' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2065, 2002, 4, 2002, N'asdfgh', CAST(N'2021-03-21T00:00:00.000' AS DateTime), N'Note 11 - asdfgh', 2, N'~/Members/2002/2065/Note4.jfif', 4, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 8, N'Enginnering', N'112233', N'Mr Desai', 0, CAST(0 AS Decimal(18, 0)), N'~/Members/2002/2065/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T12:42:32.437' AS DateTime), 2002, CAST(N'2021-03-23T12:42:32.437' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2066, 2002, 4, 2002, N'asdfgh', CAST(N'2021-02-21T00:00:00.000' AS DateTime), N'Note 12 - asdfgh', 4, N'~/Members/2002/2066/Note5.jpg', 3, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 8, N'MBA', N'586497', N'Mr Desai', 1, CAST(123 AS Decimal(18, 0)), N'~/Members/2002/2066/sample.pdf', CAST(N'2021-03-23T12:49:46.573' AS DateTime), 2002, CAST(N'2021-03-23T12:49:46.573' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2075, 2002, 4, 2002, N'asdfgh', CAST(N'2021-03-31T00:00:00.000' AS DateTime), N'Note 8 - asdfgh', 1, N'~/Members/2002/2075/Note1.jpg', 2, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 2, N'Enginnering', N'112233', N'Mr Desai', 1, CAST(122 AS Decimal(18, 0)), N'~/Members/2002/2075/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T14:20:20.450' AS DateTime), 2002, CAST(N'2021-03-23T14:20:20.450' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2076, 2002, 4, 2002, N'asdfgh', CAST(N'2021-03-31T00:00:00.000' AS DateTime), N'Note 13 - asdfgh', 3, N'~/Members/2002/2076/Note4.jfif', 3, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 8, N'Enginnering', N'112233', N'Mr Desai', 0, CAST(0 AS Decimal(18, 0)), N'~/Members/2002/2076/sample.pdf', CAST(N'2021-03-23T14:22:44.310' AS DateTime), 2002, CAST(N'2021-03-23T14:22:44.310' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2077, 2002, 4, 2002, N'asdfgh', NULL, N'Note 14 - asdfgh', 2, N'~/Members/2002/2077/Note1.jpg', 2, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 1, N'Enginnering', N'112233', N'Mr Desai', 1, CAST(112 AS Decimal(18, 0)), N'~/Members/2002/2077/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T14:23:49.557' AS DateTime), 2002, CAST(N'2021-03-23T14:23:49.557' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2078, 2003, 4, 2002, N'asdfgh', NULL, N'note 15 - zxcvbn', 3, N'~/Members/2003/2078/Note1.jpg', 1, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 1, N'Enginnering', N'112233', N'Mr Desai', 0, CAST(0 AS Decimal(18, 0)), N'~/Members/2003/2078/sample.pdf', CAST(N'2021-03-23T14:25:41.907' AS DateTime), 2003, CAST(N'2021-03-23T14:25:41.907' AS DateTime), 2003, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2079, 2003, 4, 2003, N'asdfgh', NULL, N'note 16 - zxcvbn', 2, N'~/Members/2003/2079/Note4.jfif', 2, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 6, N'CA', N'112233', N'Mr Desai', 1, CAST(132 AS Decimal(18, 0)), N'~/Members/2003/2079/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T14:26:36.140' AS DateTime), 2003, CAST(N'2021-03-23T14:26:36.140' AS DateTime), 2003, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2080, 2003, 4, 2003, N'asdfgh', NULL, N'note 17 - zxcvbn', 4, N'~/Members/2003/2080/Note5.jpg', 1, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 1, N'MBA', N'586497', N'Mr Desai', 1, CAST(110 AS Decimal(18, 0)), N'~/Members/2003/2080/sample.pdf', CAST(N'2021-03-23T14:27:24.100' AS DateTime), 2003, CAST(N'2021-03-23T14:27:24.100' AS DateTime), 2003, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2081, 8, 4, 2003, N'asdfgh', NULL, N'note 18 - zxcvbn', 1, N'~/Members/2003/2081/Note6.jfif', 3, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 2, N'CA', N'112233', N'Mr Desai', 1, CAST(115 AS Decimal(18, 0)), N'~/Members/2003/2081/sample.pdf', CAST(N'2021-03-23T14:28:22.287' AS DateTime), 2003, CAST(N'2021-03-23T14:28:22.287' AS DateTime), 2003, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2082, 8, 4, 2002, N'asdfgh', NULL, N'Note 2 - qwerty', 2, N'~/Members/8/2082/Note2.jfif', 2, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 2, N'Enginnering', N'112233', N'Mr Desai', 1, CAST(123 AS Decimal(18, 0)), N'~/Members/8/2082/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T19:13:30.430' AS DateTime), 8, CAST(N'2021-03-23T19:13:30.430' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2083, 8, 4, 2003, N'asdfgh', NULL, N'Note 6 - qwerty', 1, N'~/Members/8/2083/Note1.jpg', 3, 123, N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.', N'GEC - Rajkot', 2, N'Enginnering', N'586497', N'Mr Desai', 1, CAST(123 AS Decimal(18, 0)), N'~/Members/8/2083/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T19:16:55.433' AS DateTime), 8, CAST(N'2021-03-23T19:16:55.433' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2084, 8, 4, 2002, N'asdfgh', NULL, N'note', 2, N'~/Members/8/2084/Note6.jfif', 3, 123, N'gchvbnmmnmcc', N'GEC Rajkot', 5, N'Enginnering', N'00112233', N'Mr Desai', 1, CAST(777 AS Decimal(18, 0)), N'~/Members/8/2084/sample.pdf', CAST(N'2021-03-27T13:30:35.437' AS DateTime), 8, CAST(N'2021-03-27T13:30:35.437' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2085, 2003, 3, 2004, NULL, NULL, N'Note ajndk', 3, N'~/Members/AdminSystemConfiguration/ImageNote/Note1.jpg', 3, 123, N'DefaultNoteDisplayPicture,DefaultNoteDisplayPicture,DefaultNoteDisplayPicture,DefaultNoteDisplayPicture,DefaultNoteDisplayPicture,', N'GEC  Rajkot', 1, N'Enginnering', N'112233', N'Mr Desai', 1, CAST(33 AS Decimal(18, 0)), N'~/Members/2003/2085/sample.pdf', CAST(N'2021-04-06T12:46:33.497' AS DateTime), 2003, CAST(N'2021-04-09T10:57:57.370' AS DateTime), 2004, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3085, 3007, 4, 2004, NULL, CAST(N'2021-04-09T11:10:29.847' AS DateTime), N'wef', 2, N'~/Members/3007/3085/Note6.jfif', 3, NULL, N'Dhareshwar plot, near jakat naka, chitalDhareshwar plot, near jakat naka, chitalDhareshwar plot, near jakat naka, chitalDhareshwar plot, near jakat naka, chitalDhareshwar plot, near jakat naka, chital', NULL, 2, NULL, NULL, NULL, 1, CAST(12 AS Decimal(18, 0)), N'~/Members/3007/3085/sample.pdf', CAST(N'2021-04-09T10:43:47.137' AS DateTime), 3007, CAST(N'2021-04-09T11:10:29.847' AS DateTime), 2004, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3086, 3007, 2, NULL, NULL, NULL, N'Note 4', 2, N'~/Members/AdminSystemConfiguration/ImageNote/Note6.jfif', 3, 123, N'.customers .customer.customers .customer.customers .customer.customers .c', N'aaa', 2, N'Enginnering', NULL, NULL, 1, NULL, N'~/Members/3007/3086/sample.pdf', CAST(N'2021-04-09T10:53:59.773' AS DateTime), 3007, CAST(N'2021-04-09T10:54:26.867' AS DateTime), 3007, 1)
SET IDENTITY_INSERT [dbo].[SellerNotes] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesAttachements] ON 

INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2052, 2055, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/8/2055/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T09:22:27.053' AS DateTime), 8, CAST(N'2021-03-23T09:22:27.053' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2053, 2056, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/8/2056/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T09:35:42.917' AS DateTime), 8, CAST(N'2021-03-23T09:35:42.917' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2054, 2054, N'sample.pdf', N'~/Members/8/2054/Attachements/sample.pdf', CAST(N'2021-03-23T09:46:10.110' AS DateTime), 8, CAST(N'2021-03-23T09:46:10.110' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2057, 2058, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/8/2058/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T10:53:24.717' AS DateTime), 8, CAST(N'2021-03-23T10:53:24.717' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2058, 2059, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/8/2059/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T10:54:14.227' AS DateTime), 8, CAST(N'2021-03-23T10:54:14.227' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2059, 2060, N'sample.pdf', N'~/Members/8/2060/Attachements/sample.pdf', CAST(N'2021-03-23T10:55:31.723' AS DateTime), 8, CAST(N'2021-03-23T10:55:31.723' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2060, 2060, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/8/2060/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T10:55:31.783' AS DateTime), 8, CAST(N'2021-03-23T10:55:31.783' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2061, 2061, N'Notes Marketplace - Admin.pdf', N'~/Members/8/2061/Attachements/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T10:57:00.460' AS DateTime), 8, CAST(N'2021-03-23T10:57:00.460' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2062, 2061, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/8/2061/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T10:57:00.480' AS DateTime), 8, CAST(N'2021-03-23T10:57:00.480' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2063, 2063, N'Notes Marketplace - Admin.pdf', N'~/Members/2002/2063/Attachements/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T12:22:23.933' AS DateTime), 2002, CAST(N'2021-03-23T12:22:23.933' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2064, 2063, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/2002/2063/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T12:22:23.973' AS DateTime), 2002, CAST(N'2021-03-23T12:22:23.973' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2065, 2064, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/2002/2064/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T12:40:33.243' AS DateTime), 2002, CAST(N'2021-03-23T12:40:33.243' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2066, 2065, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/2002/2065/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T12:42:33.223' AS DateTime), 2002, CAST(N'2021-03-23T12:42:33.223' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2067, 2066, N'Notes Marketplace - Admin.pdf', N'~/Members/2002/2066/Attachements/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T12:49:46.763' AS DateTime), 2002, CAST(N'2021-03-23T12:49:46.763' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2069, 2075, N'sample.pdf', N'~/Members/2002/2075/Attachements/sample.pdf', CAST(N'2021-03-23T14:20:21.163' AS DateTime), 2002, CAST(N'2021-03-23T14:20:21.163' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2070, 2075, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/2002/2075/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T14:20:21.210' AS DateTime), 2002, CAST(N'2021-03-23T14:20:21.210' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2071, 2076, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/2002/2076/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T14:22:44.520' AS DateTime), 2002, CAST(N'2021-03-23T14:22:44.520' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2072, 2077, N'sample.pdf', N'~/Members/2002/2077/Attachements/sample.pdf', CAST(N'2021-03-23T14:23:49.667' AS DateTime), 2002, CAST(N'2021-03-23T14:23:49.667' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2073, 2077, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/2002/2077/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T14:23:49.693' AS DateTime), 2002, CAST(N'2021-03-23T14:23:49.693' AS DateTime), 2002, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2074, 2078, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/2003/2078/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T14:25:41.973' AS DateTime), 2003, CAST(N'2021-03-23T14:25:41.973' AS DateTime), 2003, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2075, 2079, N'sample.pdf', N'~/Members/2003/2079/Attachements/sample.pdf', CAST(N'2021-03-23T14:26:36.423' AS DateTime), 2003, CAST(N'2021-03-23T14:26:36.423' AS DateTime), 2003, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2076, 2079, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/2003/2079/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T14:26:36.453' AS DateTime), 2003, CAST(N'2021-03-23T14:26:36.453' AS DateTime), 2003, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2077, 2080, N'Notes Marketplace - Admin.pdf', N'~/Members/2003/2080/Attachements/Notes Marketplace - Admin.pdf', CAST(N'2021-03-23T14:27:24.333' AS DateTime), 2003, CAST(N'2021-03-23T14:27:24.333' AS DateTime), 2003, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2078, 2080, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/2003/2080/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T14:27:24.340' AS DateTime), 2003, CAST(N'2021-03-23T14:27:24.340' AS DateTime), 2003, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2079, 2081, N'sample.pdf', N'~/Members/2003/2081/Attachements/sample.pdf', CAST(N'2021-03-23T14:28:22.533' AS DateTime), 2003, CAST(N'2021-03-23T14:28:22.533' AS DateTime), 2003, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2080, 2081, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/2003/2081/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-23T14:28:22.540' AS DateTime), 2003, CAST(N'2021-03-23T14:28:22.540' AS DateTime), 2003, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2081, 2082, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/8/2082/Attachements/', CAST(N'2021-03-23T19:13:31.690' AS DateTime), 8, CAST(N'2021-03-23T19:13:31.690' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2082, 2083, N'sample.pdf', N'~/Members/8/2083/Attachements/', CAST(N'2021-03-23T19:16:55.803' AS DateTime), 8, CAST(N'2021-03-23T19:16:55.803' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2083, 2083, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/8/2083/Attachements/', CAST(N'2021-03-23T19:16:55.820' AS DateTime), 8, CAST(N'2021-03-23T19:16:55.820' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2084, 2084, N'Notes Marketplace - Admin.pdf', N'~/Members/8/2084/Attachements/Notes Marketplace - Admin.pdf', CAST(N'2021-03-27T13:30:37.337' AS DateTime), 8, CAST(N'2021-03-27T13:30:37.337' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2085, 2084, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/8/2084/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-03-27T13:30:37.427' AS DateTime), 8, CAST(N'2021-03-27T13:30:37.427' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2086, 2085, N'TS_SRS_Notes_marketplace_v1.0.pdf', N'~/Members/2003/2085/Attachements/TS_SRS_Notes_marketplace_v1.0.pdf', CAST(N'2021-04-06T12:46:35.507' AS DateTime), 2003, CAST(N'2021-04-06T12:46:35.507' AS DateTime), 2003, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3086, 3085, N'Notes Marketplace - Admin.pdf', N'~/Members/3007/3085/Attachements/Notes Marketplace - Admin.pdf', CAST(N'2021-04-09T10:43:47.653' AS DateTime), 3007, CAST(N'2021-04-09T10:43:47.653' AS DateTime), 3007, 1)
SET IDENTITY_INSERT [dbo].[SellerNotesAttachements] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesReportedIssues] ON 

INSERT [dbo].[SellerNotesReportedIssues] ([ID], [NoteID], [ReportedByID], [AgainstDownloadID], [Remarks], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1022, 2054, 2002, 1044, N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.', CAST(N'2021-04-05T22:13:52.383' AS DateTime), 2002, CAST(N'2021-04-05T22:13:52.383' AS DateTime), 2002)
INSERT [dbo].[SellerNotesReportedIssues] ([ID], [NoteID], [ReportedByID], [AgainstDownloadID], [Remarks], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1023, 2059, 2002, 1045, N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.', CAST(N'2021-04-05T22:14:08.897' AS DateTime), 2002, CAST(N'2021-04-05T22:14:08.897' AS DateTime), 2002)
INSERT [dbo].[SellerNotesReportedIssues] ([ID], [NoteID], [ReportedByID], [AgainstDownloadID], [Remarks], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1024, 2076, 2002, 1043, N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.', CAST(N'2021-04-05T22:14:18.353' AS DateTime), 2002, CAST(N'2021-04-05T22:14:18.353' AS DateTime), 2002)
SET IDENTITY_INSERT [dbo].[SellerNotesReportedIssues] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesReviews] ON 

INSERT [dbo].[SellerNotesReviews] ([ID], [NoteID], [ReviewedByID], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1002, 2054, 8, 1044, CAST(3 AS Decimal(18, 0)), N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Mi proin sed libero enim sed. ', CAST(N'2021-03-23T18:36:51.810' AS DateTime), 8, CAST(N'2021-03-23T18:36:51.810' AS DateTime), 8, 1)
INSERT [dbo].[SellerNotesReviews] ([ID], [NoteID], [ReviewedByID], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2006, 2059, 3007, 2061, CAST(3 AS Decimal(18, 0)), N'aqswdefr gtbbh ngnd', CAST(N'2021-04-09T10:46:42.447' AS DateTime), 3007, CAST(N'2021-04-09T10:46:42.447' AS DateTime), 3007, 1)
SET IDENTITY_INSERT [dbo].[SellerNotesReviews] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemConfigurations] ON 

INSERT [dbo].[SystemConfigurations] ([ID], [Name], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (39, N'SupportEmailAddress', N'jeetdesai995@gmail.com', CAST(N'2021-04-06T20:55:48.390' AS DateTime), 8, CAST(N'2021-04-06T20:55:48.390' AS DateTime), 8, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Name], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (40, N'SupportContactAddress', N'1234567895', CAST(N'2021-04-06T20:55:49.123' AS DateTime), 8, CAST(N'2021-04-06T20:55:49.123' AS DateTime), 8, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Name], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (41, N'EmailAddresssesForNotify', N'desjeeet000@gmail.com', CAST(N'2021-04-06T20:55:49.133' AS DateTime), 8, CAST(N'2021-04-06T20:55:49.133' AS DateTime), 8, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Name], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (42, N'FbUrl', N'https://www.facebook.com/', CAST(N'2021-04-06T20:55:49.153' AS DateTime), 8, CAST(N'2021-04-06T20:55:49.153' AS DateTime), 8, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Name], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (43, N'LinkedInUrl', N'https://www.linkedin.com/feed/', CAST(N'2021-04-06T20:55:49.153' AS DateTime), 8, CAST(N'2021-04-06T20:55:49.153' AS DateTime), 8, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Name], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (44, N'TwitterUrl', N'https://twitter.com/home', CAST(N'2021-04-06T20:55:49.157' AS DateTime), 8, CAST(N'2021-04-06T20:55:49.157' AS DateTime), 8, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Name], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (45, N'DefaultNoteDisplayPicture', N'~/Members/AdminSystemConfiguration/ImageNote/Note6.jfif', CAST(N'2021-04-06T20:55:49.200' AS DateTime), 8, CAST(N'2021-04-06T20:55:49.200' AS DateTime), 8, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Name], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (46, N'DefaultMemberDisplayPicture', N'~/Members/AdminSystemConfiguration/ImageProfile/Note6.jfif', CAST(N'2021-04-06T20:55:49.230' AS DateTime), 8, CAST(N'2021-04-06T20:55:49.230' AS DateTime), 8, 1)
SET IDENTITY_INSERT [dbo].[SystemConfigurations] OFF
GO
SET IDENTITY_INSERT [dbo].[UserProfile] ON 

INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (21, 8, CAST(N'2021-03-11T00:00:00.000' AS DateTime), 8, NULL, N'5', N'1234567890', N'~/images/NotesDetails/avatar.png', N'Dhareshwar plot, ', N'near jakat naka, chital', N'Amreli', N'Gujarat', N'365620', N'2', N'GTu', N'Gec - Rajkot', CAST(N'2021-03-11T17:05:57.160' AS DateTime), 8, CAST(N'2021-03-12T11:19:12.823' AS DateTime), 8)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (23, 2002, CAST(N'2021-03-11T00:00:00.000' AS DateTime), 8, NULL, N'2', N'1234567890', NULL, N'Dhareshwar plot, near jakat naka, chital', N'wertrtetertertretre', N'Amreli', N'Gujarat', N'365620', N'5', NULL, NULL, CAST(N'2021-04-03T22:02:17.533' AS DateTime), 2002, CAST(N'2021-04-03T22:02:17.533' AS DateTime), 2002)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1024, 2003, CAST(N'2021-04-08T00:00:00.000' AS DateTime), 8, NULL, N'2', N'7568952789', N'~/Members/AdminSystemConfiguration/ImageProfile/Note6.jfif', N'Dhareshwar plot, near jakat naka, chital', N'Dhareshwar plot, near jakat naka, chital', N'Amreli', N'Gujarat', N'365620', N'2', NULL, N'Gec Rajkot', CAST(N'2021-04-06T13:35:23.487' AS DateTime), 2003, CAST(N'2021-04-06T13:35:23.487' AS DateTime), 2003)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2023, 2004, NULL, NULL, NULL, N'6', N'1234567890', NULL, N'Null', N'Null', N'Null', N'Null', N'Null', N'Null', NULL, NULL, CAST(N'2021-04-07T11:52:31.493' AS DateTime), 8, CAST(N'2021-04-07T11:52:31.493' AS DateTime), 8)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3025, 3006, NULL, NULL, N'chintan9721@gmail.com', N'1', N'1234567896', N'~/Members/3006/DP_08042021.jpg', N'Null', N'Null', N'Null', N'Null', N'Null', N'Null', NULL, NULL, CAST(N'2021-04-08T16:49:03.090' AS DateTime), 8, CAST(N'2021-04-08T16:49:03.090' AS DateTime), 8)
SET IDENTITY_INSERT [dbo].[UserProfile] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [Email], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, 3, N'Desai', N'qq', N'qq@qq.com', N'Qq@1', 1, CAST(N'2021-02-24T11:51:43.090' AS DateTime), NULL, CAST(N'2021-03-12T11:19:12.823' AS DateTime), 8, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [Email], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2002, 1, N'qq', N'qq', N'desjeeet000@gmail.com', N'Jeet@149', 1, CAST(N'2021-03-02T17:33:16.440' AS DateTime), NULL, CAST(N'2021-04-03T22:02:17.533' AS DateTime), 2002, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [Email], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2003, 2, N'Jeet', N'Desai', N'jeetdesai995@gmail.com', N'Jeet@149', 1, CAST(N'2021-03-04T22:22:49.390' AS DateTime), NULL, CAST(N'2021-04-06T13:35:23.487' AS DateTime), 2003, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [Email], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2004, 2, N'Desai', N'Pratik R', N'jeetdesai895@gmail.com', N'Admin@123', 1, CAST(N'2021-04-07T11:52:30.583' AS DateTime), 8, CAST(N'2021-04-07T20:54:04.907' AS DateTime), 8, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [Email], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3006, 2, N'Desai', N'Chintan', N'chintan972@gmail.com', N'Admin@123', 1, CAST(N'2021-04-08T16:49:02.793' AS DateTime), 8, CAST(N'2021-04-08T16:49:02.793' AS DateTime), 8, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [Email], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3007, 1, N'Desai', N'Chintan', N'chintan9721@gmail.com', N'Cd@12345', 1, CAST(N'2021-04-09T10:35:41.010' AS DateTime), NULL, CAST(N'2021-04-09T10:42:31.807' AS DateTime), 3007, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[UsersRole] ON 

INSERT [dbo].[UsersRole] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Member', N'User', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[UsersRole] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Admin', N'Admin', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[UsersRole] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'SuperAdmin', N'Super Admin', NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[UsersRole] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Countrie__5D9B0D2C9A8D23B6]    Script Date: 11-04-2021 12:33:39 ******/
ALTER TABLE [dbo].[Countries] ADD UNIQUE NONCLUSTERED 
(
	[CountryCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Countrie__737584F6D67F4E14]    Script Date: 11-04-2021 12:33:39 ******/
ALTER TABLE [dbo].[Countries] ADD UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__NoteCate__737584F6169C9AB6]    Script Date: 11-04-2021 12:33:39 ******/
ALTER TABLE [dbo].[NoteCategories] ADD UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__NoteType__737584F6B997E922]    Script Date: 11-04-2021 12:33:39 ******/
ALTER TABLE [dbo].[NoteTypes] ADD UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Referenc__D222D33B14C421C2]    Script Date: 11-04-2021 12:33:39 ******/
ALTER TABLE [dbo].[ReferenceData] ADD UNIQUE NONCLUSTERED 
(
	[DataValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D10534165260E9]    Script Date: 11-04-2021 12:33:39 ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Countries] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NoteCategories] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NoteTypes] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ReferenceData] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotes] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotesAttachements] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotesReviews] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SystemConfigurations] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsEmailVerified]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UsersRole] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD FOREIGN KEY([Downloader])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([ID])
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD FOREIGN KEY([Seller])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD FOREIGN KEY([ActionedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD FOREIGN KEY([Category])
REFERENCES [dbo].[NoteCategories] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD FOREIGN KEY([Country])
REFERENCES [dbo].[Countries] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD FOREIGN KEY([NoteType])
REFERENCES [dbo].[NoteTypes] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD FOREIGN KEY([SellerID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[ReferenceData] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesAttachements]  WITH CHECK ADD FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD FOREIGN KEY([AgainstDownloadID])
REFERENCES [dbo].[Downloads] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD FOREIGN KEY([ReportedByID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD FOREIGN KEY([AgainstDownloadsID])
REFERENCES [dbo].[Downloads] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD FOREIGN KEY([ReviewedByID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD FOREIGN KEY([Gender])
REFERENCES [dbo].[ReferenceData] ([ID])
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[UsersRole] ([ID])
GO
USE [master]
GO
ALTER DATABASE [NoteMarketPlace] SET  READ_WRITE 
GO
