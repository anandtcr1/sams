USE [master]
GO
/****** Object:  Database [sams]    Script Date: 1/19/2025 4:30:13 PM ******/
CREATE DATABASE [sams]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'sams', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\sams.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'sams_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\sams_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [sams] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [sams].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [sams] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [sams] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [sams] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [sams] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [sams] SET ARITHABORT OFF 
GO
ALTER DATABASE [sams] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [sams] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [sams] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [sams] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [sams] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [sams] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [sams] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [sams] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [sams] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [sams] SET  DISABLE_BROKER 
GO
ALTER DATABASE [sams] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [sams] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [sams] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [sams] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [sams] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [sams] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [sams] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [sams] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [sams] SET  MULTI_USER 
GO
ALTER DATABASE [sams] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [sams] SET DB_CHAINING OFF 
GO
ALTER DATABASE [sams] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [sams] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [sams] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [sams] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [sams] SET QUERY_STORE = ON
GO
ALTER DATABASE [sams] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [sams]
GO
/****** Object:  Table [dbo].[tbl_all_state]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_all_state](
	[state_id] [int] IDENTITY(1,1) NOT NULL,
	[state_name] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_all_state] PRIMARY KEY CLUSTERED 
(
	[state_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_asset_type]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_asset_type](
	[asset_type_id] [int] IDENTITY(1,1) NOT NULL,
	[asset_type_name] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_asset_type] PRIMARY KEY CLUSTERED 
(
	[asset_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_c_store]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_c_store](
	[c_store_id] [int] IDENTITY(1,1) NOT NULL,
	[state_id] [int] NULL,
	[city] [varchar](500) NULL,
	[zipcode] [varchar](500) NULL,
	[county] [varchar](500) NULL,
	[asset_id] [varchar](500) NULL,
	[property_type_id] [int] NULL,
	[property_description] [varchar](500) NULL,
	[asking_price] [varchar](500) NULL,
	[asset_type_id] [int] NULL,
	[land_size] [varchar](500) NULL,
	[building_area] [varchar](500) NULL,
	[property_taxes] [varchar](500) NULL,
	[year_built] [varchar](500) NULL,
	[known_environmental_conditions] [varchar](500) NULL,
	[emv_copliance] [varchar](500) NULL,
	[hours_of_operation] [varchar](500) NULL,
	[created_date] [datetime] NULL,
	[environent_nda_pdf_filename] [varchar](500) NULL,
	[property_header] [varchar](500) NULL,
	[is_deleted] [int] NULL,
	[asset_status] [int] NULL,
	[c_store_address] [varchar](500) NULL,
	[diligence_type] [int] NULL,
	[property_latitude] [varchar](500) NULL,
	[property_longitude] [varchar](500) NULL,
	[shopping_mart_plan_file] [varchar](500) NULL,
	[property_status_id] [int] NULL,
	[rent] [varchar](500) NULL,
	[check_if_oil_supply_contract_applicable] [int] NULL,
	[term_of_supply_contract] [varchar](500) NULL,
	[check_if_property_listed] [int] NULL,
	[listing_agent_name] [varchar](500) NULL,
	[listing_expiry] [datetime] NULL,
	[listing_price] [varchar](500) NULL,
	[term] [varchar](500) NULL,
	[asking_rent] [varchar](500) NULL,
	[lease_type] [int] NULL,
	[term_remaining] [varchar](500) NULL,
	[rental_income] [varchar](500) NULL,
	[lease_type_lease_and_fee] [int] NULL,
	[supply_contract_applicable_lease_and_fee] [int] NULL,
	[supply_contract_term_lease_and_fee] [varchar](500) NULL,
	[details] [text] NULL,
	[hide_notification] [int] NULL,
	[status_changed_date] [datetime] NULL,
	[is_closed] [int] NULL,
	[can_publish] [bit] NULL,
 CONSTRAINT [PK_tbl_c_store] PRIMARY KEY CLUSTERED 
(
	[c_store_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_c_store_files]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_c_store_files](
	[file_id] [int] IDENTITY(1,1) NOT NULL,
	[property_id] [int] NULL,
	[file_type] [varchar](500) NULL,
	[file_name] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_c_store_files] PRIMARY KEY CLUSTERED 
(
	[file_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_c_strore_plan]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_c_strore_plan](
	[c_store_plan_id] [int] IDENTITY(1,1) NOT NULL,
	[c_store_id] [int] NULL,
	[plan_file_name] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_c_strore_plan] PRIMARY KEY CLUSTERED 
(
	[c_store_plan_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_customer]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_customer](
	[customer_id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [varchar](500) NULL,
	[last_name] [varchar](500) NULL,
	[email_address] [varchar](500) NULL,
	[contact_number] [varchar](500) NULL,
	[signed_nda_file] [varchar](500) NULL,
	[user_name] [varchar](500) NULL,
	[customer_password] [varchar](500) NULL,
	[created_date] [datetime] NULL,
	[last_login_date] [datetime] NULL,
	[customer_sign] [varchar](500) NULL,
	[company_name] [varchar](500) NULL,
	[given_title] [varchar](500) NULL,
	[address] [varchar](500) NULL,
	[zipcode] [varchar](500) NULL,
	[city] [varchar](500) NULL,
	[state_id] [varchar](500) NULL,
	[cell_number] [varchar](500) NULL,
	[ip_address] [varchar](500) NULL,
	[signed_status] [varchar](500) NULL,
	[signed_date] [datetime] NULL,
	[director_signature] [varchar](500) NULL,
	[sh_verification_id] [varchar](500) NULL,
	[reset_password_id] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_customer] PRIMARY KEY CLUSTERED 
(
	[customer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_customer_log]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_customer_log](
	[login_id] [int] IDENTITY(1,1) NOT NULL,
	[customer_id] [int] NULL,
	[login_date] [datetime] NULL,
 CONSTRAINT [PK_tbl_customer_log] PRIMARY KEY CLUSTERED 
(
	[login_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_customer_message]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_customer_message](
	[contact_us_id] [int] IDENTITY(1,1) NOT NULL,
	[custumer_name] [varchar](500) NULL,
	[customer_email] [varchar](500) NULL,
	[customer_subject] [varchar](500) NULL,
	[customer_message] [varchar](500) NULL,
	[created_date] [datetime] NULL,
 CONSTRAINT [PK_tbl_customer_message] PRIMARY KEY CLUSTERED 
(
	[contact_us_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_diligence_acquisition]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_diligence_acquisition](
	[diligence_acquisition_id] [int] IDENTITY(1,1) NOT NULL,
	[property_id] [int] NULL,
	[property_type] [int] NULL,
	[purchase_price] [varchar](500) NULL,
	[earnest_money] [varchar](500) NULL,
	[exchange_1031] [varchar](500) NULL,
	[dead_line_1031] [varchar](500) NULL,
	[sellers] [varchar](500) NULL,
	[escrow_agent] [varchar](500) NULL,
	[sub_division] [varchar](500) NULL,
	[real_estate_agent] [varchar](500) NULL,
	[created_date] [datetime] NULL,
	[acquisition_status] [int] NULL,
	[terminated_date] [datetime] NULL,
	[closed_date] [datetime] NULL,
	[under_contract_date] [datetime] NULL,
	[due_diligence_expiry_date] [datetime] NULL,
	[ddp_extension] [datetime] NULL,
	[check_if_ddp_extension_opted] [int] NULL,
	[additional_earnest_money_deposit] [varchar](500) NULL,
	[permitting_period] [varchar](500) NULL,
	[buying_entity] [varchar](500) NULL,
	[buyers_attorney] [varchar](500) NULL,
	[sellers_attorney] [varchar](500) NULL,
	[buyers_agent] [varchar](500) NULL,
	[sellers_agent] [varchar](500) NULL,
	[sellers_agent_commission] [varchar](500) NULL,
	[buyers_agent_commission] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_diligence_acquisition] PRIMARY KEY CLUSTERED 
(
	[diligence_acquisition_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_diligence_dispositions]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_diligence_dispositions](
	[diligence_dispositions_id] [int] IDENTITY(1,1) NOT NULL,
	[property_id] [int] NULL,
	[property_type] [int] NULL,
	[sale_price] [varchar](500) NULL,
	[earnest_money] [varchar](500) NULL,
	[buyers] [varchar](500) NULL,
	[escrow_agent] [varchar](500) NULL,
	[buyers_attorney] [varchar](500) NULL,
	[options_to_extend] [varchar](500) NULL,
	[commissions] [varchar](500) NULL,
	[created_date] [datetime] NULL,
	[disposition_status] [int] NULL,
	[terminated_date] [datetime] NULL,
	[closed_date] [datetime] NULL,
	[under_contract_date] [datetime] NULL,
	[due_diligence_expairy_date] [datetime] NULL,
	[due_diligence_amount] [varchar](500) NULL,
	[emd] [varchar](500) NULL,
	[ddp_extension] [datetime] NULL,
	[dueDiligenceApplicableStatus] [int] NULL,
	[sellersAttorney] [varchar](500) NULL,
	[buyers_agent_commision] [varchar](500) NULL,
	[sellers_agent_commision] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_diligence_dispositions] PRIMARY KEY CLUSTERED 
(
	[diligence_dispositions_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_diligence_lease]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_diligence_lease](
	[diligence_lease_id] [int] IDENTITY(1,1) NOT NULL,
	[property_id] [int] NULL,
	[property_type] [int] NULL,
	[tenant_name] [varchar](500) NULL,
	[rent] [varchar](500) NULL,
	[under_contract_date] [datetime] NULL,
	[due_diligence_expiry_date] [datetime] NULL,
	[earnest_money_deposit] [varchar](500) NULL,
	[ddp_extension] [datetime] NULL,
	[tenant_attorney] [varchar](500) NULL,
	[tenant_agent_commission] [varchar](500) NULL,
	[land_lord_agent_commission] [varchar](500) NULL,
	[lease_security_deposit] [varchar](500) NULL,
	[created_date] [datetime] NULL,
	[terminated_date] [datetime] NULL,
	[closed_date] [datetime] NULL,
	[listing_price] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_diligence_lease] PRIMARY KEY CLUSTERED 
(
	[diligence_lease_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_future_tenant]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_future_tenant](
	[future_tenent_id] [int] IDENTITY(1,1) NOT NULL,
	[netlease_id] [int] NULL,
	[tenant_name] [varchar](500) NULL,
	[tenant_unit] [varchar](500) NULL,
	[term] [varchar](500) NULL,
	[rent] [varchar](500) NULL,
	[cam] [varchar](500) NULL,
	[under_contract_date] [datetime] NULL,
	[ddp] [varchar](500) NULL,
	[tenant_upfit_concession] [varchar](500) NULL,
	[rent_free_period] [int] NULL,
	[lease_commencement_date] [datetime] NULL,
	[lease_expiration_date] [datetime] NULL,
	[lease_options] [varchar](500) NULL,
	[rent_escalation] [varchar](500) NULL,
	[tenant_attorney] [varchar](500) NULL,
	[tenant_agent_commission] [varchar](500) NULL,
	[landlord_agent_commission] [varchar](500) NULL,
	[lease_security_deposit] [varchar](500) NULL,
	[deleted_status] [int] NULL,
	[free_rent_description] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_future_tenant] PRIMARY KEY CLUSTERED 
(
	[future_tenent_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_future_tenant_critical_dates]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_future_tenant_critical_dates](
	[critical_date_id] [int] IDENTITY(1,1) NOT NULL,
	[future_tenant_id] [int] NULL,
	[critical_date_master] [varchar](500) NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[critical_date_notes] [varchar](500) NULL,
	[hide_notification] [int] NULL,
 CONSTRAINT [PK_tbl_future_tenant_critical_dates] PRIMARY KEY CLUSTERED 
(
	[critical_date_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_lease_type]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_lease_type](
	[lease_type_id] [int] IDENTITY(1,1) NOT NULL,
	[lease_type] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_lease_type] PRIMARY KEY CLUSTERED 
(
	[lease_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_map_cordinates]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_map_cordinates](
	[cordinated_id] [int] IDENTITY(1,1) NOT NULL,
	[header_id] [int] NULL,
	[latitude] [varchar](500) NULL,
	[longitude] [varchar](500) NULL,
	[marker_color] [varchar](500) NULL,
	[marker_header] [varchar](500) NULL,
	[marker_address] [varchar](500) NULL,
	[marker_type] [varchar](500) NULL,
	[added_address] [varchar](500) NULL,
	[land_size] [varchar](500) NULL,
	[asking_price] [varchar](500) NULL,
	[zoning] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_map_cordinates] PRIMARY KEY CLUSTERED 
(
	[cordinated_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_map_header]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_map_header](
	[map_header_id] [int] IDENTITY(1,1) NOT NULL,
	[header_name] [varchar](500) NULL,
	[created_date] [datetime] NULL,
 CONSTRAINT [PK_tbl_map_header] PRIMARY KEY CLUSTERED 
(
	[map_header_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_market]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_market](
	[market_id] [int] IDENTITY(1,1) NOT NULL,
	[market_name] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[market_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_module_master]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_module_master](
	[module_id] [int] NULL,
	[module_name] [varchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_month]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_month](
	[month_id] [int] NOT NULL,
	[month_name] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_month] PRIMARY KEY CLUSTERED 
(
	[month_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_net_lease_files]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_net_lease_files](
	[file_id] [int] IDENTITY(1,1) NOT NULL,
	[property_id] [int] NULL,
	[file_type] [varchar](500) NULL,
	[file_name] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_net_lease_files] PRIMARY KEY CLUSTERED 
(
	[file_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_net_lease_property]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_net_lease_property](
	[net_lease_property_id] [int] IDENTITY(1,1) NOT NULL,
	[asset_id] [varchar](500) NULL,
	[asset_name] [varchar](500) NULL,
	[state_id] [int] NULL,
	[city] [varchar](500) NULL,
	[cap_rate] [float] NULL,
	[term] [varchar](500) NULL,
	[detail_pdf] [varchar](500) NULL,
	[created_date] [datetime] NULL,
	[property_price] [varchar](500) NULL,
	[asset_type_id] [int] NULL,
	[is_deleted] [int] NULL,
	[asset_status] [int] NULL,
	[is_shopping_center] [bit] NULL,
	[property_address] [varchar](500) NULL,
	[property_zipcode] [varchar](500) NULL,
	[diligence_type] [int] NULL,
	[property_latitude] [varchar](500) NULL,
	[property_longitude] [varchar](500) NULL,
	[property_status_id] [int] NULL,
	[check_if_property_listed] [int] NULL,
	[listing_agent_name] [varchar](500) NULL,
	[listing_expiry] [datetime] NULL,
	[listing_price] [varchar](500) NULL,
	[asking_rent] [varchar](500) NULL,
	[lease_type] [int] NULL,
	[shopping_mart_plan_file_name] [varchar](500) NULL,
	[details] [text] NULL,
	[hide_notification] [int] NULL,
	[status_changed_date] [datetime] NULL,
	[is_closed] [int] NULL,
	[can_publish] [bit] NULL,
 CONSTRAINT [PK_tbl_net_lease_property] PRIMARY KEY CLUSTERED 
(
	[net_lease_property_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_new_property_status]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_new_property_status](
	[new_property_status_id] [int] IDENTITY(1,1) NOT NULL,
	[new_property_status_name] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_new_property_status] PRIMARY KEY CLUSTERED 
(
	[new_property_status_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_period]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_period](
	[period_id] [int] IDENTITY(1,1) NOT NULL,
	[property_id] [int] NULL,
	[property_type] [int] NULL,
	[period_master] [varchar](500) NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[period_notes] [varchar](500) NULL,
	[period_type] [varchar](500) NULL,
	[hide_notification] [int] NULL,
 CONSTRAINT [PK_tbl_period] PRIMARY KEY CLUSTERED 
(
	[period_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_period_master]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_period_master](
	[period_master_id] [int] IDENTITY(1,1) NOT NULL,
	[period_name] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_period_master] PRIMARY KEY CLUSTERED 
(
	[period_master_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_property]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_property](
	[site_details_id] [int] IDENTITY(1,1) NOT NULL,
	[name_prefix] [varchar](500) NULL,
	[first_name] [varchar](500) NULL,
	[last_name] [varchar](500) NULL,
	[company_name] [varchar](500) NULL,
	[email_address] [varchar](500) NULL,
	[address] [varchar](500) NULL,
	[city_name] [varchar](500) NULL,
	[state_id] [varchar](500) NULL,
	[zip_code] [varchar](500) NULL,
	[contact_number] [varchar](500) NULL,
	[sams_holding_employee] [bit] NULL,
	[market_id] [int] NULL,
	[site_address] [varchar](500) NULL,
	[site_city] [varchar](500) NULL,
	[site_state_id] [int] NULL,
	[site_county] [varchar](500) NULL,
	[site_cross_street_name] [varchar](500) NULL,
	[is_property_available] [bit] NULL,
	[zoning] [varchar](500) NULL,
	[lot_size] [varchar](500) NULL,
	[sales_price] [varchar](500) NULL,
	[comments] [varchar](500) NULL,
	[created_date] [datetime] NULL,
	[property_type] [int] NULL,
	[image_name] [varchar](500) NULL,
	[property_header] [varchar](500) NULL,
	[asset_type_id] [int] NULL,
	[is_deleted] [int] NULL,
	[asset_status] [int] NULL,
	[diligence_type] [int] NULL,
	[property_latitude] [varchar](500) NULL,
	[property_longitude] [varchar](500) NULL,
	[asset_id] [varchar](500) NULL,
	[property_status_id] [int] NULL,
	[check_if_property_listed] [int] NULL,
	[listing_agent_name] [varchar](500) NULL,
	[listing_expiry] [datetime] NULL,
	[listing_price] [varchar](500) NULL,
	[term] [varchar](500) NULL,
	[asking_rent] [varchar](500) NULL,
	[lease_type] [int] NULL,
	[hide_notification] [int] NULL,
	[status_changed_date] [datetime] NULL,
	[is_closed] [int] NULL,
	[can_publish] [bit] NULL,
 CONSTRAINT [PK_tbl_property] PRIMARY KEY CLUSTERED 
(
	[site_details_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_property_images]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_property_images](
	[image_id] [int] IDENTITY(1,1) NOT NULL,
	[property_id] [int] NULL,
	[image_name] [varchar](500) NULL,
	[property_type] [int] NULL,
 CONSTRAINT [PK_tbl_property_images] PRIMARY KEY CLUSTERED 
(
	[image_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_property_status]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_property_status](
	[property_status_id] [int] IDENTITY(1,1) NOT NULL,
	[property_status] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_property_status] PRIMARY KEY CLUSTERED 
(
	[property_status_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_property_type]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_property_type](
	[property_type_id] [int] IDENTITY(1,1) NOT NULL,
	[property_type_name] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_property_type] PRIMARY KEY CLUSTERED 
(
	[property_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_role]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_role](
	[role_id] [int] IDENTITY(1,1) NOT NULL,
	[role_name] [varchar](500) NULL,
	[can_publish_listing] [bit] NULL,
 CONSTRAINT [PK_tbl_role] PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_role_permission]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_role_permission](
	[role_permission_id] [int] IDENTITY(1,1) NOT NULL,
	[role_id] [int] NULL,
	[module_id] [int] NULL,
	[can_read] [bit] NULL,
	[can_edit] [bit] NULL,
	[can_create] [bit] NULL,
	[can_delete] [bit] NULL,
 CONSTRAINT [PK_tbl_role_permission] PRIMARY KEY CLUSTERED 
(
	[role_permission_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_sams_locations]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_sams_locations](
	[location_id] [int] IDENTITY(1,1) NOT NULL,
	[sh_asset_id] [varchar](500) NULL,
	[location_address] [varchar](500) NULL,
	[city] [varchar](500) NULL,
	[state] [varchar](500) NULL,
	[zipcode] [varchar](500) NULL,
	[county] [varchar](500) NULL,
	[business_name] [varchar](500) NULL,
	[latitude] [varchar](500) NULL,
	[longitude] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_sams_locations] PRIMARY KEY CLUSTERED 
(
	[location_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_settings]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_settings](
	[settings_id] [int] IDENTITY(1,1) NOT NULL,
	[smtp_mail_server] [varchar](500) NULL,
	[smtp_port_number] [varchar](500) NULL,
	[smtp_email_address] [varchar](500) NULL,
	[smtp_password] [varchar](500) NULL,
	[email_header] [varchar](500) NULL,
	[email_body] [varchar](5000) NULL,
	[real_estate_director] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_settings] PRIMARY KEY CLUSTERED 
(
	[settings_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_shopping_center]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_shopping_center](
	[shopping_center_id] [int] IDENTITY(1,1) NOT NULL,
	[shopping_center_name] [varchar](500) NULL,
	[state_id] [int] NULL,
	[city_name] [varchar](500) NULL,
	[zip_code] [varchar](500) NULL,
	[property_status_id] [int] NULL,
	[rent_amount] [varchar](500) NULL,
	[property_type_id] [int] NULL,
	[spaces] [varchar](500) NULL,
	[spaces_available] [varchar](500) NULL,
	[building_size] [varchar](500) NULL,
	[asset_status] [int] NULL,
	[shop_description] [varchar](5000) NULL,
	[created_date] [datetime] NULL,
	[is_deleted] [int] NULL,
 CONSTRAINT [PK_tbl_shopping_center] PRIMARY KEY CLUSTERED 
(
	[shopping_center_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_shopping_center_clients]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_shopping_center_clients](
	[shopping_center_client_id] [int] IDENTITY(1,1) NOT NULL,
	[c_store_id] [int] NULL,
	[tenant_name] [varchar](500) NULL,
	[unit_selected] [varchar](500) NULL,
	[annual_rent] [varchar](500) NULL,
	[monthly_rent] [varchar](500) NULL,
	[cam_monthly] [varchar](500) NULL,
	[cam_yearly] [varchar](500) NULL,
	[set_or_adjust_automatically] [varchar](500) NULL,
	[rent_and_cam_monthly] [varchar](500) NULL,
	[rent_and_cam_yearly] [varchar](500) NULL,
	[piece_per_square_foot] [varchar](500) NULL,
	[lease_expires] [varchar](500) NULL,
	[date_rent_changed] [datetime] NULL,
	[annual_rent_changed_to] [varchar](500) NULL,
	[rent_per_month_changed_to] [varchar](500) NULL,
	[rent_and_cam_changed_to] [varchar](500) NULL,
	[piece_per_square_foot_changed_to] [varchar](500) NULL,
	[subspace_square_footage] [varchar](500) NULL,
	[notes] [varchar](500) NULL,
	[coi_expire] [datetime] NULL,
	[hide_notification] [int] NULL,
 CONSTRAINT [PK_tbl_shopping_center_clients] PRIMARY KEY CLUSTERED 
(
	[shopping_center_client_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_signedup_customer]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_signedup_customer](
	[custimer_id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [varchar](500) NULL,
	[last_name] [varchar](500) NULL,
	[email_address] [varchar](500) NULL,
	[contact_number] [varchar](500) NULL,
	[created_date] [datetime] NULL,
	[subscribe_status] [bit] NULL,
 CONSTRAINT [PK_tbl_signedup_customer] PRIMARY KEY CLUSTERED 
(
	[custimer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_state]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_state](
	[state_id] [int] IDENTITY(1,1) NOT NULL,
	[state_code] [varchar](500) NULL,
	[state_name] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[state_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_submitted_property]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_submitted_property](
	[site_details_id] [int] IDENTITY(1,1) NOT NULL,
	[name_prefix] [varchar](500) NULL,
	[first_name] [varchar](500) NULL,
	[last_name] [varchar](500) NULL,
	[company_name] [varchar](500) NULL,
	[email_address] [varchar](500) NULL,
	[address] [varchar](500) NULL,
	[city_name] [varchar](500) NULL,
	[state_id] [varchar](500) NULL,
	[zip_code] [varchar](500) NULL,
	[contact_number] [varchar](500) NULL,
	[sams_holding_employee] [bit] NULL,
	[market_id] [int] NULL,
	[site_address] [varchar](500) NULL,
	[site_city] [varchar](500) NULL,
	[site_state_id] [int] NULL,
	[site_county] [varchar](500) NULL,
	[site_cross_street_name] [varchar](500) NULL,
	[is_property_available] [bit] NULL,
	[zoning] [varchar](500) NULL,
	[lot_size] [varchar](500) NULL,
	[sales_price] [varchar](500) NULL,
	[comments] [varchar](500) NULL,
	[created_date] [datetime] NULL,
	[property_type] [int] NULL,
	[image_name] [varchar](500) NULL,
	[image_file_name] [varchar](500) NULL,
	[pdf_file_name] [varchar](500) NULL,
	[is_deleted] [int] NULL,
	[created_by] [int] NULL,
	[client_represented_by_broker] [int] NULL,
	[broker_firm_name] [varchar](500) NULL,
	[broker_email_address] [varchar](500) NULL,
	[broker_contact_number] [varchar](500) NULL,
	[potential_use] [varchar](500) NULL,
	[term] [varchar](500) NULL,
	[asking_rent] [varchar](500) NULL,
	[lease_type] [int] NULL,
	[asset_type_id] [int] NULL,
	[status_changed_date] [datetime] NULL,
	[is_closed] [int] NULL,
	[new_property_status_id] [int] NULL,
 CONSTRAINT [PK_tbl_sbmitted_property] PRIMARY KEY CLUSTERED 
(
	[site_details_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_surplus_files]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_surplus_files](
	[file_id] [int] IDENTITY(1,1) NOT NULL,
	[property_id] [int] NULL,
	[file_type] [varchar](500) NULL,
	[file_name] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_surplus_files] PRIMARY KEY CLUSTERED 
(
	[file_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_todo]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_todo](
	[todo_id] [int] IDENTITY(1,1) NOT NULL,
	[property_id] [int] NULL,
	[todo_text] [varchar](500) NULL,
	[property_type] [int] NULL,
	[created_date] [datetime] NULL,
 CONSTRAINT [PK_tbl_todo] PRIMARY KEY CLUSTERED 
(
	[todo_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_user]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_user](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [varchar](500) NULL,
	[last_name] [varchar](500) NULL,
	[user_name] [varchar](500) NULL,
	[password] [varchar](500) NULL,
	[role_id] [int] NULL,
	[password_reset_key] [varchar](500) NULL,
	[email_address] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_user] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tbl_all_state] ON 

INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (1, N'Alabama')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (2, N'Alaska')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (3, N'Arizona')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (4, N'Arkansas')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (5, N'California')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (6, N'Colorado')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (7, N'Connecticut')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (8, N'Delaware')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (9, N'Florida')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (10, N'Georgia')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (11, N'Hawaii')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (12, N'Idaho')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (13, N'Illinois')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (14, N'Indiana')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (15, N'Iowa')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (16, N'Kansas')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (17, N'Kentucky')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (18, N'Louisiana')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (19, N'Maine')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (20, N'Maryland')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (21, N'Massachusetts')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (22, N'Michigan')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (23, N'Minnesota')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (24, N'Mississippi')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (25, N'Missouri')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (26, N'Montana')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (27, N'Nebraska')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (28, N'Nevada')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (29, N'New Hampshire')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (30, N'New Jersey')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (31, N'New Mexico')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (32, N'New York')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (33, N'North Carolina')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (34, N'North Dakota')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (35, N'Ohio')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (36, N'Oklahoma')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (37, N'Oregon')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (38, N'Pennsylvania')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (39, N'Rhode Island')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (40, N'South Carolina')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (41, N'South Dakota')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (42, N'Tennessee')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (43, N'Texas')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (44, N'Utah')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (45, N'Vermont')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (46, N'Virginia')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (47, N'Washington')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (48, N'West Virginia')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (49, N'Wisconsin')
INSERT [dbo].[tbl_all_state] ([state_id], [state_name]) VALUES (50, N'Wyoming')
SET IDENTITY_INSERT [dbo].[tbl_all_state] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_asset_type] ON 

INSERT [dbo].[tbl_asset_type] ([asset_type_id], [asset_type_name]) VALUES (1, N'Lease')
INSERT [dbo].[tbl_asset_type] ([asset_type_id], [asset_type_name]) VALUES (2, N'Fee')
INSERT [dbo].[tbl_asset_type] ([asset_type_id], [asset_type_name]) VALUES (3, N'Fee Subject To Lease')
SET IDENTITY_INSERT [dbo].[tbl_asset_type] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_c_store] ON 

INSERT [dbo].[tbl_c_store] ([c_store_id], [state_id], [city], [zipcode], [county], [asset_id], [property_type_id], [property_description], [asking_price], [asset_type_id], [land_size], [building_area], [property_taxes], [year_built], [known_environmental_conditions], [emv_copliance], [hours_of_operation], [created_date], [environent_nda_pdf_filename], [property_header], [is_deleted], [asset_status], [c_store_address], [diligence_type], [property_latitude], [property_longitude], [shopping_mart_plan_file], [property_status_id], [rent], [check_if_oil_supply_contract_applicable], [term_of_supply_contract], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [term_remaining], [rental_income], [lease_type_lease_and_fee], [supply_contract_applicable_lease_and_fee], [supply_contract_term_lease_and_fee], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (1, 3, N'Trichur', N'680652', N'Virginia', N'58462', 0, N'des', N'854000', 2, N'2400 S/F', N'1800 S/F', N'2500', N'2000', N'', N'', N'', CAST(N'2020-05-01T14:37:56.373' AS DateTime), N'', N'Kent Station', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_c_store] ([c_store_id], [state_id], [city], [zipcode], [county], [asset_id], [property_type_id], [property_description], [asking_price], [asset_type_id], [land_size], [building_area], [property_taxes], [year_built], [known_environmental_conditions], [emv_copliance], [hours_of_operation], [created_date], [environent_nda_pdf_filename], [property_header], [is_deleted], [asset_status], [c_store_address], [diligence_type], [property_latitude], [property_longitude], [shopping_mart_plan_file], [property_status_id], [rent], [check_if_oil_supply_contract_applicable], [term_of_supply_contract], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [term_remaining], [rental_income], [lease_type_lease_and_fee], [supply_contract_applicable_lease_and_fee], [supply_contract_term_lease_and_fee], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (2, 1, N'Atlanta', N'680652', N'Georgia', N'SH-100255', 2, N'desc', N'34000', 2, N'3500 S/F', N'1500 S/F', N'', N'1998', N'', N'', N'10', CAST(N'2020-05-01T16:59:25.433' AS DateTime), N'', N'Auburn, WA', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_c_store] ([c_store_id], [state_id], [city], [zipcode], [county], [asset_id], [property_type_id], [property_description], [asking_price], [asset_type_id], [land_size], [building_area], [property_taxes], [year_built], [known_environmental_conditions], [emv_copliance], [hours_of_operation], [created_date], [environent_nda_pdf_filename], [property_header], [is_deleted], [asset_status], [c_store_address], [diligence_type], [property_latitude], [property_longitude], [shopping_mart_plan_file], [property_status_id], [rent], [check_if_oil_supply_contract_applicable], [term_of_supply_contract], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [term_remaining], [rental_income], [lease_type_lease_and_fee], [supply_contract_applicable_lease_and_fee], [supply_contract_term_lease_and_fee], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (3, 3, N'Augusta', N'34434', N'Augusta', N'100025', 1, N'Description lessens', N'Contact', 2, N'1800 S/F', N'1000 S/F', N'', N'2014', N'Condi - 1', N'Not compliant', N'Operation Red', CAST(N'2020-05-02T04:19:52.670' AS DateTime), N'', N'Chehalis, WA', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_c_store] ([c_store_id], [state_id], [city], [zipcode], [county], [asset_id], [property_type_id], [property_description], [asking_price], [asset_type_id], [land_size], [building_area], [property_taxes], [year_built], [known_environmental_conditions], [emv_copliance], [hours_of_operation], [created_date], [environent_nda_pdf_filename], [property_header], [is_deleted], [asset_status], [c_store_address], [diligence_type], [property_latitude], [property_longitude], [shopping_mart_plan_file], [property_status_id], [rent], [check_if_oil_supply_contract_applicable], [term_of_supply_contract], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [term_remaining], [rental_income], [lease_type_lease_and_fee], [supply_contract_applicable_lease_and_fee], [supply_contract_term_lease_and_fee], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (4, 0, N'Zafrabad', N'1102123', N'DUSA', N'H15400', 1, N'', N'854000', 1, N'2500 S/F', N'1500 S/F', N'', N'', N'', N'', N'', CAST(N'2020-05-02T05:46:49.530' AS DateTime), N'', N'710 Powder Springs Street', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_c_store] ([c_store_id], [state_id], [city], [zipcode], [county], [asset_id], [property_type_id], [property_description], [asking_price], [asset_type_id], [land_size], [building_area], [property_taxes], [year_built], [known_environmental_conditions], [emv_copliance], [hours_of_operation], [created_date], [environent_nda_pdf_filename], [property_header], [is_deleted], [asset_status], [c_store_address], [diligence_type], [property_latitude], [property_longitude], [shopping_mart_plan_file], [property_status_id], [rent], [check_if_oil_supply_contract_applicable], [term_of_supply_contract], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [term_remaining], [rental_income], [lease_type_lease_and_fee], [supply_contract_applicable_lease_and_fee], [supply_contract_term_lease_and_fee], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (5, 0, N'Columbus', N'25462', N'USA ', N'12546', 1, N'Checking for PDF Upload and property Image  upload', N'452000', 1, N'1546 S/F', N'1000 S/F', N'', N'2012', N'Condi - 1', N'Not compliant', N'Operation Red', CAST(N'2020-05-05T05:04:57.460' AS DateTime), N'', N'Federal Way, WA', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_c_store] ([c_store_id], [state_id], [city], [zipcode], [county], [asset_id], [property_type_id], [property_description], [asking_price], [asset_type_id], [land_size], [building_area], [property_taxes], [year_built], [known_environmental_conditions], [emv_copliance], [hours_of_operation], [created_date], [environent_nda_pdf_filename], [property_header], [is_deleted], [asset_status], [c_store_address], [diligence_type], [property_latitude], [property_longitude], [shopping_mart_plan_file], [property_status_id], [rent], [check_if_oil_supply_contract_applicable], [term_of_supply_contract], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [term_remaining], [rental_income], [lease_type_lease_and_fee], [supply_contract_applicable_lease_and_fee], [supply_contract_term_lease_and_fee], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (6, 1, N'Cornvalis', N'989929', N'USA', N'17800', 1, N'The C store has two parts :  Gas Station and a Convenient Store ', N'4544554', 2, N'2500 S/F', N'2000 S/F', N'', N'1997', N'Need to upload PDF', N'Yes : Forms attached', N'12 - 09 00', CAST(N'2020-05-06T10:50:04.567' AS DateTime), N'', N'Renton, WA', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_c_store] ([c_store_id], [state_id], [city], [zipcode], [county], [asset_id], [property_type_id], [property_description], [asking_price], [asset_type_id], [land_size], [building_area], [property_taxes], [year_built], [known_environmental_conditions], [emv_copliance], [hours_of_operation], [created_date], [environent_nda_pdf_filename], [property_header], [is_deleted], [asset_status], [c_store_address], [diligence_type], [property_latitude], [property_longitude], [shopping_mart_plan_file], [property_status_id], [rent], [check_if_oil_supply_contract_applicable], [term_of_supply_contract], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [term_remaining], [rental_income], [lease_type_lease_and_fee], [supply_contract_applicable_lease_and_fee], [supply_contract_term_lease_and_fee], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (7, 0, N'Trichur', N'680652', N'Kerala', N'SH-002', 1, N'', N'856', 1, N'152', N'458', N'', N'2001', N'', N'', N'', CAST(N'2020-05-06T12:56:17.313' AS DateTime), N'', N'c-store 1', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_c_store] ([c_store_id], [state_id], [city], [zipcode], [county], [asset_id], [property_type_id], [property_description], [asking_price], [asset_type_id], [land_size], [building_area], [property_taxes], [year_built], [known_environmental_conditions], [emv_copliance], [hours_of_operation], [created_date], [environent_nda_pdf_filename], [property_header], [is_deleted], [asset_status], [c_store_address], [diligence_type], [property_latitude], [property_longitude], [shopping_mart_plan_file], [property_status_id], [rent], [check_if_oil_supply_contract_applicable], [term_of_supply_contract], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [term_remaining], [rental_income], [lease_type_lease_and_fee], [supply_contract_applicable_lease_and_fee], [supply_contract_term_lease_and_fee], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (8, 2, N'Weddington', N'28104', N'union', N'SM#1001', 0, N'', N'15000', 2, N'1.25', N'3000', N'', N'2000', N'None', N'Yes', N'24 hours', CAST(N'2020-05-06T13:11:47.147' AS DateTime), N'', N'Shell Branded C-Store for Sale', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_c_store] ([c_store_id], [state_id], [city], [zipcode], [county], [asset_id], [property_type_id], [property_description], [asking_price], [asset_type_id], [land_size], [building_area], [property_taxes], [year_built], [known_environmental_conditions], [emv_copliance], [hours_of_operation], [created_date], [environent_nda_pdf_filename], [property_header], [is_deleted], [asset_status], [c_store_address], [diligence_type], [property_latitude], [property_longitude], [shopping_mart_plan_file], [property_status_id], [rent], [check_if_oil_supply_contract_applicable], [term_of_supply_contract], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [term_remaining], [rental_income], [lease_type_lease_and_fee], [supply_contract_applicable_lease_and_fee], [supply_contract_term_lease_and_fee], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (9, 2, N'Carolina', N'6623', N'CC', N'SH3002', 0, N'Data for descriptio', N'$ 758,22', 2, N'58622 S/F', N'4500 S/F', N'526', N'2000', N'', N'', N'8AM to 10PM', CAST(N'2020-05-07T09:15:55.503' AS DateTime), N'', N'Shell Gas Station', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_c_store] ([c_store_id], [state_id], [city], [zipcode], [county], [asset_id], [property_type_id], [property_description], [asking_price], [asset_type_id], [land_size], [building_area], [property_taxes], [year_built], [known_environmental_conditions], [emv_copliance], [hours_of_operation], [created_date], [environent_nda_pdf_filename], [property_header], [is_deleted], [asset_status], [c_store_address], [diligence_type], [property_latitude], [property_longitude], [shopping_mart_plan_file], [property_status_id], [rent], [check_if_oil_supply_contract_applicable], [term_of_supply_contract], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [term_remaining], [rental_income], [lease_type_lease_and_fee], [supply_contract_applicable_lease_and_fee], [supply_contract_term_lease_and_fee], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (10, 3, N'Trichur', N'680652', N'Kerala', N'SH-002', 0, N'', N'345', 1, N'234', N'da', N'2500', N'', N'', N'', N'', CAST(N'2020-05-07T09:19:36.630' AS DateTime), N'', N'710 Powder Springs Street', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_c_store] ([c_store_id], [state_id], [city], [zipcode], [county], [asset_id], [property_type_id], [property_description], [asking_price], [asset_type_id], [land_size], [building_area], [property_taxes], [year_built], [known_environmental_conditions], [emv_copliance], [hours_of_operation], [created_date], [environent_nda_pdf_filename], [property_header], [is_deleted], [asset_status], [c_store_address], [diligence_type], [property_latitude], [property_longitude], [shopping_mart_plan_file], [property_status_id], [rent], [check_if_oil_supply_contract_applicable], [term_of_supply_contract], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [term_remaining], [rental_income], [lease_type_lease_and_fee], [supply_contract_applicable_lease_and_fee], [supply_contract_term_lease_and_fee], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (11, 2, N'Burlington', N'27215', N'Alamance', N'SM # 709', 0, N'Valero Branded C-Store with 4 MPDs and a C-Store under the canopy with an additional building for future expansion for sale.  Excellent opportunity for an investor/operator to own and rebuild a great location in front of Holly Hill mall and proposed Publix grocery anchored shopping center.  Property is sold subject to a 20 year fuel supply contract with Seller.
', N'$650,000.00', 3, N'0.625 Acres', N'1023 Sq Ft and Additional former Car Wash Building 1500 Sq Ft', N'$3407.49', N'1977', N'None', N'No', N'5:00 AM to 11:00 PM', CAST(N'2020-05-11T16:37:45.507' AS DateTime), N'', N'Valero Branded C-Store For Sale', NULL, 0, N'336 Huffman Mill Rd, Burlington, NC, USA', 2, N'36.0770173', N'-79.4866248', N'Edappt-scr-flow-1(2)_3dab.pdf', 3, N'2500', 1, N'20 years', 0, N'', CAST(N'2020-10-28T12:24:20.000' AS DateTime), N'15001-3', N'20 Years', N'12522', 2, N'3 years', N'2500', 2, 0, N'', N'', 1, NULL, NULL, 1)
INSERT [dbo].[tbl_c_store] ([c_store_id], [state_id], [city], [zipcode], [county], [asset_id], [property_type_id], [property_description], [asking_price], [asset_type_id], [land_size], [building_area], [property_taxes], [year_built], [known_environmental_conditions], [emv_copliance], [hours_of_operation], [created_date], [environent_nda_pdf_filename], [property_header], [is_deleted], [asset_status], [c_store_address], [diligence_type], [property_latitude], [property_longitude], [shopping_mart_plan_file], [property_status_id], [rent], [check_if_oil_supply_contract_applicable], [term_of_supply_contract], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [term_remaining], [rental_income], [lease_type_lease_and_fee], [supply_contract_applicable_lease_and_fee], [supply_contract_term_lease_and_fee], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (12, 2, N'Charlotte', N'28223', N'University City Blvd', N'Test Value C Store #2', 0, N'Go beyond hypotheses and theory. Study in a place where on-campus research comes to life in off-campus applications throughout area communities, businesses and industries. Variety is more than the spice of life. It is life! The world offers a broader range of career opportunities than ever before, which is why we offer the way to explore and prepare for so many of them right.', N'$ 890,000 $', 1, N'23600', N'4600', N'3%', N'1997', N'Need to upload PDF', N'Yes : Forms attached', N'12 00 pm - 09 00 pm ', CAST(N'2020-05-12T05:14:29.280' AS DateTime), N'Sam''sHolding_Example_9208.pdf', N'C Store Sequel One -11 ', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_c_store] ([c_store_id], [state_id], [city], [zipcode], [county], [asset_id], [property_type_id], [property_description], [asking_price], [asset_type_id], [land_size], [building_area], [property_taxes], [year_built], [known_environmental_conditions], [emv_copliance], [hours_of_operation], [created_date], [environent_nda_pdf_filename], [property_header], [is_deleted], [asset_status], [c_store_address], [diligence_type], [property_latitude], [property_longitude], [shopping_mart_plan_file], [property_status_id], [rent], [check_if_oil_supply_contract_applicable], [term_of_supply_contract], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [term_remaining], [rental_income], [lease_type_lease_and_fee], [supply_contract_applicable_lease_and_fee], [supply_contract_term_lease_and_fee], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (13, 3, N'Trivandrum', N'589652', N'Georgia', N'CS - 710', 0, N'', N'34000', 1, N'234', N'1800 S/F', N'2500', N'', N'', N'', N'', CAST(N'2020-11-18T15:50:28.107' AS DateTime), N'', N'CS - 710 Powder Springs Street', 1, 0, N'address 1', NULL, N'', N'', NULL, 2, N'3600', 0, N'', 0, N'', CAST(N'2020-11-18T21:20:38.000' AS DateTime), N'', N'', N'', 0, N'', N'', 0, 0, N'', N'', NULL, CAST(N'2020-11-18T00:00:00.000' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[tbl_c_store] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_c_store_files] ON 

INSERT [dbo].[tbl_c_store_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (1, 0, N'file', N'b4_bde7.pdf')
INSERT [dbo].[tbl_c_store_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (2, 0, N'Sales - 2020', N'b3_b214.pdf')
INSERT [dbo].[tbl_c_store_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (5, 2, N'Sales - 2020', N'b4_0ee2.pdf')
INSERT [dbo].[tbl_c_store_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (6, 2, N'Sales 2019', N'b3_fd7b.pdf')
INSERT [dbo].[tbl_c_store_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (7, 6, N'Sales - 2020', N'b2_300f.pdf')
INSERT [dbo].[tbl_c_store_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (8, 3, N'Plat', N'b3_e833.pdf')
INSERT [dbo].[tbl_c_store_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (10, 11, N'Financials', N'C-Store Financial Projection SM709_f894.pdf')
INSERT [dbo].[tbl_c_store_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (11, 11, N'Additional Property information', N'336 Huffman Mill Rd_9c7b.pdf')
SET IDENTITY_INSERT [dbo].[tbl_c_store_files] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_customer] ON 

INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (1, N'Janet', N'Jones', N'jjones@gmail.com', N'0094959599', N'chart1_1bbf.jpg', N'please use my email Id', N'a', CAST(N'2020-05-11T17:59:38.717' AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (2, N'Arun', N'P', N'apboss@gmail.com', N'8788954555', N'b1_e9b2.jpg', N'ap', N'a', CAST(N'2020-05-12T18:43:22.763' AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (7, N'Paul', N'joseph', N'pjoseph@kw.com', N'7048196919', N'no_file', N'', N'', CAST(N'2020-06-11T18:15:20.353' AS DateTime), NULL, NULL, N'Keller Williams Realty', N'Broker', N'', N'28104', N'Weddington', N'0', N'7048196919', NULL, N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (8, N'george', N'Joseph', N'kjosephp@gmail.com', N'7048196919', N'no_file', N'', N'Iamgood1', CAST(N'2020-06-11T22:12:37.313' AS DateTime), NULL, NULL, N'Joseph Realty', N'Manager', N'', N'28104', N'Weddington', N'0', N'7048196919', NULL, N'Email Sent', NULL, NULL, NULL, N'')
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (9, N' Thenum', N'Vayam', N'almeda@padu.com', N'9050500454', N'no_file', N'', N'', CAST(N'2020-06-12T00:23:49.057' AS DateTime), NULL, NULL, N'Thaha Corp', N'', N'', N'', N'', N'0', N'343333', NULL, N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (10, N'Joe', N'Johnson', N'joe@jondoe.com', N'9806753434', N'no_file', N'', N'', CAST(N'2020-06-12T14:31:54.433' AS DateTime), NULL, NULL, N'john doe manangement', N'president', N'', N'28105', N'Matthews', N'0', N'98000098776', NULL, N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (11, N'Vipin', N'Thomas', N'vipint@gmail.com', N'08541223665', N'no_file', N'sams133', N'33', CAST(N'2020-06-13T13:08:45.563' AS DateTime), NULL, NULL, N'Dell International', N'', N'Orlando', N'556223', N'N Carolina', N'3', N'08546997998', NULL, N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (13, N'Samuel', N'Frederic', N'samuel@sams.com', N'4456652', N'no_file', N'frederic12', N'123', CAST(N'2020-06-15T06:30:57.593' AS DateTime), NULL, NULL, N'Knowminal', N'', N'Orlando', N'556223', N'N Carolina', N'3', N'1112223330123', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (15, N'Mathew', N'K', N'mk@gmail.com', N'08546997998', N'no_file', N'mk78', N'123', CAST(N'2020-06-15T07:50:53.030' AS DateTime), NULL, NULL, N'new', N'', N'1, 2', N'680652', N'Trichur', N'1', N'123456', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (17, N'Paul', N'joseph', N'pjoseph@kw.com', N'7048196919', N'no_file', N'ppk', N'paul', CAST(N'2020-08-20T14:03:21.850' AS DateTime), NULL, NULL, N'Keller Williams Realty', N'', N'7935 Council Place', N'28105', N'Matthews', N'33', N'7048196919', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (18, N'', N'', N'', N'', N'no_file', N'', N'', CAST(N'2020-08-21T03:32:39.327' AS DateTime), NULL, NULL, N'', N'', N'', N'', N'', N'0', N'', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (20, N'Paul', N'joseph', N'pjoseph@kw.com', N'7048196919', N'no_file', N'Paula', N'summa', CAST(N'2020-08-24T12:21:35.113' AS DateTime), NULL, NULL, N'Keller Williams Realty', N'Broker', N'7935 Council Place', N'28105', N'Matthews', N'33', N'7048196919', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (25, N'Samuel', N'Frederic', N'samuel@sams.com', N'4456652', N'no_file', N'qq1', N'a', CAST(N'2020-08-27T17:49:17.777' AS DateTime), NULL, NULL, N'Knowminal', N'', N'Orlando', N'556223', N'N Carolina', N'1', N'08546997998', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (27, N'ahu', N'lsr', N'anand@knowminal.com', N'08546997998', N'no_file', N'ddd', N'qqqqqqQ1', CAST(N'2020-08-27T18:36:32.320' AS DateTime), NULL, NULL, N'new', N'', N'1, 2', N'680652', N'Trichur', N'1', N'', N'', N'Signed', CAST(N'2020-08-27T20:46:22.517' AS DateTime), NULL, NULL, N'')
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (28, N'Paul', N'joseph', N'kjosephp@gmail.com', N'7048196919', N'no_file', N'kjosephp@gmail.com', N'Tumba1ishta', CAST(N'2020-09-04T12:04:09.327' AS DateTime), NULL, NULL, N'', N'', N'7935 Council Place', N'28105', N'Matthews', N'33', N'7048196919', N'', N'Signed', CAST(N'2020-09-04T12:22:15.007' AS DateTime), NULL, NULL, N'')
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (29, N'', N'', N'', N'', N'no_file', N'', N'', CAST(N'2020-09-21T06:18:03.203' AS DateTime), NULL, NULL, N'', N'', N'', N'', N'', N'0', N'', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (31, N'', N'', N'', N'', N'no_file', N'', N'', CAST(N'2020-09-21T10:47:49.820' AS DateTime), NULL, NULL, N'', N'', N'', N'', N'', N'0', N'', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (32, N'', N'', N'', N'', N'no_file', N'', N'', CAST(N'2020-09-21T10:47:50.220' AS DateTime), NULL, NULL, N'', N'', N'', N'', N'', N'0', N'', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (33, N'Paul', N'Joseph', N'kjosephp@gmail.com', N'7049403704', N'no_file', N'pkjreal', N'pkjreal', CAST(N'2020-09-29T12:00:29.463' AS DateTime), NULL, N'signature_33_.png', N'', N'', N'10039 University City Blvd, Suite # G, Suite # N', N'28262', N'Charlotte', N'33', N'7042466059', N'', N'Email Sent', NULL, N'director_signature_33_.png', NULL, N'CD994029-D7B2-41AB-9BA6-D719C13D8F96')
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (34, N'George', N'Joseph', N'pjoseph@kw.com', N'704-819-6919', N'no_file', N'pkjreal', N'pkjreal', CAST(N'2020-09-29T12:04:29.023' AS DateTime), NULL, NULL, N'', N'', N'10039 University City Blvd, Suite # G, Suite # N', N'28262', N'Charlotte', N'33', N'704-8196919', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (36, N'anand 2', N's', N'anand@knowminal.com', N'08546997998', N'no_file', N'anand1', N'123', CAST(N'2020-09-30T14:43:48.687' AS DateTime), NULL, N'signature_36_.png', N'Knowminal Technologies LLP', N'', N'Test address', N'680652', N'Trichur', N'1', N'08546997998', N'', N'Signed', NULL, N'director_signature_36_.png', N'TxYmX2Vy9FEW9F8U', N'996F925C-1315-438B-8531-E31450CED959')
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (37, N'Dean Jones', N'Queber', N'arun@knowminal.com', N'21211', N'no_file', N'dam', N'a', CAST(N'2020-10-01T04:44:02.077' AS DateTime), NULL, N'signature_37_.png', N'Org', N'Title', N'#1', N'545634', N'', N'33', N'21121', N'', N'Signed', NULL, N'director_signature_37_.png', N'52jiynJNgmsWgcU8', NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (38, N'John', N'Davis', N'kjosephp@gmail.com', N'7048223522', N'no_file', N'jdavis', N'jd', CAST(N'2020-10-02T20:29:13.650' AS DateTime), NULL, N'signature_38_.png', N'', N'', N'', N'', N'', N'1', N'7048223522', N'', N'Signed', NULL, N'director_signature_38_.png', N'S3eIu38L7LNEf9SX', N'D14F5BC7-EB3D-487C-8E48-FB1E2487A75E')
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (39, N'Sam', N'Nafisi', N'102692n@gmail.com', N'7045267753', N'no_file', N'Snafisi', N'jyjcen-sibvot-Kutde6', CAST(N'2020-10-08T18:51:27.470' AS DateTime), NULL, N'signature_39_.png', N'Ss', N'Ss', N'', N'', N'', N'1', N'7045267753', N'', N'Signed', NULL, N'director_signature_39_.png', N'gyRB6IlZpT6LMbum', NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (40, N'john', N'peters', N'kjosephp@gmail.com', N'7049403704', N'no_file', N'jpeters', N'Mykerala1', CAST(N'2020-10-27T15:22:31.943' AS DateTime), NULL, NULL, N'Sams Commercial Properties, LLC', N'Broker', N'10039 University City Blvd, Suite # G, Suite # N', N'28262', N'Charlotte', N'33', N'7049403704', N'', N'Email Sent', NULL, NULL, NULL, N'8E05F715-6DC0-4756-900D-1B919B7E49A6')
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (41, N'sam', N'nafisi', N'102692n@gmail.com', N'7045267753', N'no_file', N'snafisi2', N'Waseem23!', CAST(N'2020-10-27T18:26:30.680' AS DateTime), NULL, NULL, N'sam''s holdings', N'', N'6905 shinnecock hill ln', N'28277', N'charlotte', N'33', N'', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (42, N'sam', N'nafisi', N'102692n@gmail.com', N'7045267753', N'no_file', N'snafisi2', N'Waseem23!', CAST(N'2020-10-27T18:26:34.217' AS DateTime), NULL, NULL, N'sam''s holdings', N'', N'6905 shinnecock hill ln', N'28277', N'charlotte', N'33', N'', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (43, N'Paul', N'Joseph', N'pjoseph@samsholdings.com', N'7049403704', N'no_file', N'pkjoseph', N'Tumba1ishta', CAST(N'2020-10-28T13:17:55.487' AS DateTime), NULL, NULL, N'Sams Commercial Properties, LLC', N'', N'10039 University City Blvd, Suite # G, Suite # N', N'28262', N'Charlotte', N'33', N'7049403704', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (44, N'anand', N'ks', N'anand@knowminal.com', N'08546997998', N'no_file', N'sams555566666', N'Q12345q', CAST(N'2020-10-28T14:05:54.460' AS DateTime), NULL, NULL, N'comp', N'Software Developer', N'1', N'680652', N'Trichur', N'1', N'08546997998', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (45, N'Paul', N'Joseph', N'pjoseph@samsholdings.com', N'7049403704', N'no_file', N'pdjoseph', N'Tumba1ishta', CAST(N'2020-10-28T14:05:56.053' AS DateTime), NULL, NULL, N'Sams Commercial Properties, LLC', N'Broker', N'10039 University City Blvd, Suite # G, Suite # N', N'28262', N'Charlotte', N'2', N'7049403704', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (46, N'Mohan', N'Kumar', N'mohank@gmail.com', N'55895', N'no_file', N'sams366332', N'Q123456q', CAST(N'2020-10-28T14:14:48.747' AS DateTime), NULL, NULL, N'new', N'Marketing Officer', N'address 1, address 2', N'589652', N'Trivandrum', N'1', N'4458', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (47, N'anand', N'ks', N'anand@knowminal.com', N'08546997998', N'no_file', N'sams5555666661', N'Q12345q', CAST(N'2020-10-28T14:21:40.673' AS DateTime), NULL, NULL, N'comp', N'Software Developer', N'1', N'680652', N'Trichur', N'1', N'08546997998', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (48, N'tt', N'tt', N't@t.com', N'2', N'no_file', N'tttt', N'123456Aa', CAST(N'2020-10-28T14:24:14.260' AS DateTime), NULL, NULL, N'', N'', N'', N'', N'', N'1', N'2', N'', N'Email Sent', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_customer] ([customer_id], [first_name], [last_name], [email_address], [contact_number], [signed_nda_file], [user_name], [customer_password], [created_date], [last_login_date], [customer_sign], [company_name], [given_title], [address], [zipcode], [city], [state_id], [cell_number], [ip_address], [signed_status], [signed_date], [director_signature], [sh_verification_id], [reset_password_id]) VALUES (49, N'Customer Test', N'Names', N'email@domain.com', N'232', N'no_file', N'123', N'123456Aa', CAST(N'2020-10-29T10:28:27.307' AS DateTime), NULL, NULL, N'oRG', N'master', N'ww', N'ww', N'w', N'1', N'123', N'', N'Email Sent', NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[tbl_customer] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_customer_message] ON 

INSERT [dbo].[tbl_customer_message] ([contact_us_id], [custumer_name], [customer_email], [customer_subject], [customer_message], [created_date]) VALUES (6, N'Sam', N'SSnafisi@samsmartinc.com', N'Hello', N'Hello', CAST(N'2020-10-08T18:46:22.427' AS DateTime))
INSERT [dbo].[tbl_customer_message] ([contact_us_id], [custumer_name], [customer_email], [customer_subject], [customer_message], [created_date]) VALUES (17, N'anand 2 s', N'anand@knowminal.com', N'anand@knowminal.com', N'te', CAST(N'2020-10-29T16:34:55.800' AS DateTime))
INSERT [dbo].[tbl_customer_message] ([contact_us_id], [custumer_name], [customer_email], [customer_subject], [customer_message], [created_date]) VALUES (18, N'Paul K Joseph', N'pjoseph@samsholdings.com', N'pjoseph@samsholdings.com', N'adsf', CAST(N'2020-11-16T13:59:39.820' AS DateTime))
SET IDENTITY_INSERT [dbo].[tbl_customer_message] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_diligence_acquisition] ON 

INSERT [dbo].[tbl_diligence_acquisition] ([diligence_acquisition_id], [property_id], [property_type], [purchase_price], [earnest_money], [exchange_1031], [dead_line_1031], [sellers], [escrow_agent], [sub_division], [real_estate_agent], [created_date], [acquisition_status], [terminated_date], [closed_date], [under_contract_date], [due_diligence_expiry_date], [ddp_extension], [check_if_ddp_extension_opted], [additional_earnest_money_deposit], [permitting_period], [buying_entity], [buyers_attorney], [sellers_attorney], [buyers_agent], [sellers_agent], [sellers_agent_commission], [buyers_agent_commission]) VALUES (3, 33, 1, N'', N'', N'', N'', N'', N'', N'', N'', CAST(N'2020-10-16T19:35:53.237' AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_diligence_acquisition] ([diligence_acquisition_id], [property_id], [property_type], [purchase_price], [earnest_money], [exchange_1031], [dead_line_1031], [sellers], [escrow_agent], [sub_division], [real_estate_agent], [created_date], [acquisition_status], [terminated_date], [closed_date], [under_contract_date], [due_diligence_expiry_date], [ddp_extension], [check_if_ddp_extension_opted], [additional_earnest_money_deposit], [permitting_period], [buying_entity], [buyers_attorney], [sellers_attorney], [buyers_agent], [sellers_agent], [sellers_agent_commission], [buyers_agent_commission]) VALUES (4, 18, 5, N'', N'', N'', N'', N'', N'', N'', N'', CAST(N'2020-11-03T14:48:10.673' AS DateTime), NULL, NULL, NULL, CAST(N'2020-11-03T00:00:00.000' AS DateTime), CAST(N'2020-11-03T00:00:00.000' AS DateTime), NULL, 1, N'', N'', N'', N'', N'', N'', N'', N'', N'')
SET IDENTITY_INSERT [dbo].[tbl_diligence_acquisition] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_diligence_dispositions] ON 

INSERT [dbo].[tbl_diligence_dispositions] ([diligence_dispositions_id], [property_id], [property_type], [sale_price], [earnest_money], [buyers], [escrow_agent], [buyers_attorney], [options_to_extend], [commissions], [created_date], [disposition_status], [terminated_date], [closed_date], [under_contract_date], [due_diligence_expairy_date], [due_diligence_amount], [emd], [ddp_extension], [dueDiligenceApplicableStatus], [sellersAttorney], [buyers_agent_commision], [sellers_agent_commision]) VALUES (4, 30, 1, N'$325,000.00', N'0.0', N'Carolina Blue Sky Property, LLC / Kevin J Brown', N'Miller, Walker and Austin, Attorney at Law, Stephanie Barckman', N'', N'', N'4% to Tracy Effird /Nichols Company', CAST(N'2020-10-16T18:58:27.437' AS DateTime), NULL, NULL, NULL, CAST(N'2020-10-13T00:00:00.000' AS DateTime), CAST(N'2020-12-13T00:00:00.000' AS DateTime), N'', N'$5,000.00', CAST(N'2020-10-16T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_diligence_dispositions] ([diligence_dispositions_id], [property_id], [property_type], [sale_price], [earnest_money], [buyers], [escrow_agent], [buyers_attorney], [options_to_extend], [commissions], [created_date], [disposition_status], [terminated_date], [closed_date], [under_contract_date], [due_diligence_expairy_date], [due_diligence_amount], [emd], [ddp_extension], [dueDiligenceApplicableStatus], [sellersAttorney], [buyers_agent_commision], [sellers_agent_commision]) VALUES (5, 32, 1, N'$450,000.00', N'', N'Mark Brummond or assigns / Moby242@yahoo.com', N'Blanco Tackabery & Matamoros, PC Attn: Amy Lanning', N'Kennery J Abner/704-945-9855/kabner@bdjalaw.com', N'', N'', CAST(N'2020-10-16T19:23:33.973' AS DateTime), NULL, NULL, NULL, CAST(N'2020-08-24T00:00:00.000' AS DateTime), CAST(N'2020-11-24T00:00:00.000' AS DateTime), N'', N'$25,000.00', CAST(N'2020-10-16T00:00:00.000' AS DateTime), 0, N'', N'', N'')
INSERT [dbo].[tbl_diligence_dispositions] ([diligence_dispositions_id], [property_id], [property_type], [sale_price], [earnest_money], [buyers], [escrow_agent], [buyers_attorney], [options_to_extend], [commissions], [created_date], [disposition_status], [terminated_date], [closed_date], [under_contract_date], [due_diligence_expairy_date], [due_diligence_amount], [emd], [ddp_extension], [dueDiligenceApplicableStatus], [sellersAttorney], [buyers_agent_commision], [sellers_agent_commision]) VALUES (6, 11, 3, N'625', N'', N'', N'yes', N'test', N'', N'', CAST(N'2020-10-21T02:24:50.857' AS DateTime), NULL, NULL, NULL, CAST(N'2020-10-21T02:24:50.830' AS DateTime), CAST(N'2020-10-21T02:24:50.830' AS DateTime), N'', N'', CAST(N'2020-10-21T02:24:50.830' AS DateTime), 0, N'111', N'222', N'333')
INSERT [dbo].[tbl_diligence_dispositions] ([diligence_dispositions_id], [property_id], [property_type], [sale_price], [earnest_money], [buyers], [escrow_agent], [buyers_attorney], [options_to_extend], [commissions], [created_date], [disposition_status], [terminated_date], [closed_date], [under_contract_date], [due_diligence_expairy_date], [due_diligence_amount], [emd], [ddp_extension], [dueDiligenceApplicableStatus], [sellersAttorney], [buyers_agent_commision], [sellers_agent_commision]) VALUES (7, 16, 5, N'', N'', N'', N'', N'', N'', N'', CAST(N'2020-10-23T12:39:19.353' AS DateTime), NULL, NULL, NULL, CAST(N'2020-10-23T12:39:19.327' AS DateTime), CAST(N'2020-10-23T12:39:19.327' AS DateTime), N'', N'', CAST(N'2020-10-23T12:39:19.327' AS DateTime), 0, N'', N'', N'')
INSERT [dbo].[tbl_diligence_dispositions] ([diligence_dispositions_id], [property_id], [property_type], [sale_price], [earnest_money], [buyers], [escrow_agent], [buyers_attorney], [options_to_extend], [commissions], [created_date], [disposition_status], [terminated_date], [closed_date], [under_contract_date], [due_diligence_expairy_date], [due_diligence_amount], [emd], [ddp_extension], [dueDiligenceApplicableStatus], [sellersAttorney], [buyers_agent_commision], [sellers_agent_commision]) VALUES (8, 34, 1, N'', N'', N'', N'', N'', N'', N'', CAST(N'2020-10-27T15:56:57.003' AS DateTime), NULL, NULL, NULL, CAST(N'2020-10-27T15:56:56.993' AS DateTime), CAST(N'2020-10-27T15:56:56.993' AS DateTime), N'', N'', CAST(N'2020-10-27T15:56:56.993' AS DateTime), 0, N'', N'', N'')
INSERT [dbo].[tbl_diligence_dispositions] ([diligence_dispositions_id], [property_id], [property_type], [sale_price], [earnest_money], [buyers], [escrow_agent], [buyers_attorney], [options_to_extend], [commissions], [created_date], [disposition_status], [terminated_date], [closed_date], [under_contract_date], [due_diligence_expairy_date], [due_diligence_amount], [emd], [ddp_extension], [dueDiligenceApplicableStatus], [sellersAttorney], [buyers_agent_commision], [sellers_agent_commision]) VALUES (9, 5, 2, N'', N'', N'', N'', N'', N'', N'', CAST(N'2020-11-03T09:49:36.440' AS DateTime), NULL, NULL, NULL, CAST(N'2020-11-03T09:49:36.317' AS DateTime), CAST(N'2020-11-03T09:49:36.317' AS DateTime), N'', N'', CAST(N'2020-11-03T09:49:36.317' AS DateTime), 0, N'', N'', N'')
INSERT [dbo].[tbl_diligence_dispositions] ([diligence_dispositions_id], [property_id], [property_type], [sale_price], [earnest_money], [buyers], [escrow_agent], [buyers_attorney], [options_to_extend], [commissions], [created_date], [disposition_status], [terminated_date], [closed_date], [under_contract_date], [due_diligence_expairy_date], [due_diligence_amount], [emd], [ddp_extension], [dueDiligenceApplicableStatus], [sellersAttorney], [buyers_agent_commision], [sellers_agent_commision]) VALUES (10, 43, 1, N'', N'', N'', N'', N'', N'', N'', CAST(N'2020-11-19T01:24:17.360' AS DateTime), NULL, NULL, NULL, NULL, NULL, N'', N'', NULL, 0, N'', N'', N'')
INSERT [dbo].[tbl_diligence_dispositions] ([diligence_dispositions_id], [property_id], [property_type], [sale_price], [earnest_money], [buyers], [escrow_agent], [buyers_attorney], [options_to_extend], [commissions], [created_date], [disposition_status], [terminated_date], [closed_date], [under_contract_date], [due_diligence_expairy_date], [due_diligence_amount], [emd], [ddp_extension], [dueDiligenceApplicableStatus], [sellersAttorney], [buyers_agent_commision], [sellers_agent_commision]) VALUES (11, 44, 1, N'', N'', N'', N'', N'', N'', N'', CAST(N'2020-11-19T01:31:46.413' AS DateTime), NULL, NULL, NULL, NULL, NULL, N'', N'', NULL, 0, N'', N'', N'')
INSERT [dbo].[tbl_diligence_dispositions] ([diligence_dispositions_id], [property_id], [property_type], [sale_price], [earnest_money], [buyers], [escrow_agent], [buyers_attorney], [options_to_extend], [commissions], [created_date], [disposition_status], [terminated_date], [closed_date], [under_contract_date], [due_diligence_expairy_date], [due_diligence_amount], [emd], [ddp_extension], [dueDiligenceApplicableStatus], [sellersAttorney], [buyers_agent_commision], [sellers_agent_commision]) VALUES (12, 45, 1, N'', N'', N'', N'', N'', N'', N'', CAST(N'2020-11-19T01:56:39.267' AS DateTime), NULL, NULL, NULL, NULL, NULL, N'', N'', NULL, 0, N'', N'', N'')
SET IDENTITY_INSERT [dbo].[tbl_diligence_dispositions] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_diligence_lease] ON 

INSERT [dbo].[tbl_diligence_lease] ([diligence_lease_id], [property_id], [property_type], [tenant_name], [rent], [under_contract_date], [due_diligence_expiry_date], [earnest_money_deposit], [ddp_extension], [tenant_attorney], [tenant_agent_commission], [land_lord_agent_commission], [lease_security_deposit], [created_date], [terminated_date], [closed_date], [listing_price]) VALUES (1, 8, 2, N'', N'', NULL, NULL, N'', NULL, N'', N'', N'', N'', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_diligence_lease] ([diligence_lease_id], [property_id], [property_type], [tenant_name], [rent], [under_contract_date], [due_diligence_expiry_date], [earnest_money_deposit], [ddp_extension], [tenant_attorney], [tenant_agent_commission], [land_lord_agent_commission], [lease_security_deposit], [created_date], [terminated_date], [closed_date], [listing_price]) VALUES (2, 8, 2, N'', N'', CAST(N'2020-10-22T10:07:08.000' AS DateTime), CAST(N'2020-10-22T10:07:08.000' AS DateTime), N'', CAST(N'2020-10-22T10:07:08.000' AS DateTime), N'', N'', N'', N'', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_diligence_lease] ([diligence_lease_id], [property_id], [property_type], [tenant_name], [rent], [under_contract_date], [due_diligence_expiry_date], [earnest_money_deposit], [ddp_extension], [tenant_attorney], [tenant_agent_commission], [land_lord_agent_commission], [lease_security_deposit], [created_date], [terminated_date], [closed_date], [listing_price]) VALUES (3, 8, 2, N'', N'', CAST(N'2020-10-22T10:07:08.000' AS DateTime), CAST(N'2020-10-22T10:07:08.000' AS DateTime), N'', CAST(N'2020-10-22T10:07:08.000' AS DateTime), N'', N'', N'', N'', NULL, NULL, NULL, N'')
INSERT [dbo].[tbl_diligence_lease] ([diligence_lease_id], [property_id], [property_type], [tenant_name], [rent], [under_contract_date], [due_diligence_expiry_date], [earnest_money_deposit], [ddp_extension], [tenant_attorney], [tenant_agent_commission], [land_lord_agent_commission], [lease_security_deposit], [created_date], [terminated_date], [closed_date], [listing_price]) VALUES (4, 8, 2, N'adec', N'2500', CAST(N'2020-10-28T00:00:00.000' AS DateTime), CAST(N'2020-12-23T00:00:00.000' AS DateTime), N'2000', CAST(N'2021-02-01T00:00:00.000' AS DateTime), N'adbe', N'4%', N'4%', N'5000', NULL, NULL, NULL, N'')
INSERT [dbo].[tbl_diligence_lease] ([diligence_lease_id], [property_id], [property_type], [tenant_name], [rent], [under_contract_date], [due_diligence_expiry_date], [earnest_money_deposit], [ddp_extension], [tenant_attorney], [tenant_agent_commission], [land_lord_agent_commission], [lease_security_deposit], [created_date], [terminated_date], [closed_date], [listing_price]) VALUES (5, 8, 2, N'what a world', N'2500', CAST(N'2020-10-30T00:00:00.000' AS DateTime), CAST(N'2020-12-23T00:00:00.000' AS DateTime), N'2000', CAST(N'2021-02-01T00:00:00.000' AS DateTime), N'adbe', N'4%', N'4%', N'5000', NULL, NULL, NULL, N'')
INSERT [dbo].[tbl_diligence_lease] ([diligence_lease_id], [property_id], [property_type], [tenant_name], [rent], [under_contract_date], [due_diligence_expiry_date], [earnest_money_deposit], [ddp_extension], [tenant_attorney], [tenant_agent_commission], [land_lord_agent_commission], [lease_security_deposit], [created_date], [terminated_date], [closed_date], [listing_price]) VALUES (6, 30, 1, N'', N'', NULL, NULL, N'', NULL, N'', N'', N'', N'', NULL, NULL, NULL, N'')
SET IDENTITY_INSERT [dbo].[tbl_diligence_lease] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_future_tenant] ON 

INSERT [dbo].[tbl_future_tenant] ([future_tenent_id], [netlease_id], [tenant_name], [tenant_unit], [term], [rent], [cam], [under_contract_date], [ddp], [tenant_upfit_concession], [rent_free_period], [lease_commencement_date], [lease_expiration_date], [lease_options], [rent_escalation], [tenant_attorney], [tenant_agent_commission], [landlord_agent_commission], [lease_security_deposit], [deleted_status], [free_rent_description]) VALUES (1, 8, N'test', N's', N'10 Years', N'9633', N'555', CAST(N'2020-10-22T00:00:00.000' AS DateTime), N'666', N'777', 0, CAST(N'2020-10-05T00:00:00.000' AS DateTime), CAST(N'2020-11-04T00:00:00.000' AS DateTime), N'888', N'999', N'1010', N's', N'dfs', N'1013', NULL, N'')
INSERT [dbo].[tbl_future_tenant] ([future_tenent_id], [netlease_id], [tenant_name], [tenant_unit], [term], [rent], [cam], [under_contract_date], [ddp], [tenant_upfit_concession], [rent_free_period], [lease_commencement_date], [lease_expiration_date], [lease_options], [rent_escalation], [tenant_attorney], [tenant_agent_commission], [landlord_agent_commission], [lease_security_deposit], [deleted_status], [free_rent_description]) VALUES (2, 8, N'this is the lease', N'', N'', N'', N'', NULL, N'', N'', 1, NULL, NULL, N'888', N'', N'', N'', N'', N'', NULL, N'test desc')
INSERT [dbo].[tbl_future_tenant] ([future_tenent_id], [netlease_id], [tenant_name], [tenant_unit], [term], [rent], [cam], [under_contract_date], [ddp], [tenant_upfit_concession], [rent_free_period], [lease_commencement_date], [lease_expiration_date], [lease_options], [rent_escalation], [tenant_attorney], [tenant_agent_commission], [landlord_agent_commission], [lease_security_deposit], [deleted_status], [free_rent_description]) VALUES (3, 8, N'not so important', N'F', N'5 years', N'20050', N'300', NULL, N'60 days', N'$60,000.00', 0, CAST(N'2020-10-31T00:00:00.000' AS DateTime), CAST(N'2023-11-30T00:00:00.000' AS DateTime), N'', N'', N'rookie law', N'4%', N'4%', N'5000', NULL, NULL)
SET IDENTITY_INSERT [dbo].[tbl_future_tenant] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_future_tenant_critical_dates] ON 

INSERT [dbo].[tbl_future_tenant_critical_dates] ([critical_date_id], [future_tenant_id], [critical_date_master], [start_date], [end_date], [critical_date_notes], [hide_notification]) VALUES (1, 1, N'etest', CAST(N'2020-10-28T00:00:00.000' AS DateTime), CAST(N'2020-10-29T00:00:00.000' AS DateTime), N'kjm', NULL)
INSERT [dbo].[tbl_future_tenant_critical_dates] ([critical_date_id], [future_tenant_id], [critical_date_master], [start_date], [end_date], [critical_date_notes], [hide_notification]) VALUES (3, 3, N'Landlord upfit', CAST(N'2020-11-26T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), N'not so important', NULL)
INSERT [dbo].[tbl_future_tenant_critical_dates] ([critical_date_id], [future_tenant_id], [critical_date_master], [start_date], [end_date], [critical_date_notes], [hide_notification]) VALUES (4, 2, N'Landlord upfit', CAST(N'2020-11-23T00:00:00.000' AS DateTime), CAST(N'2020-12-23T00:00:00.000' AS DateTime), N'Check back for completion', NULL)
SET IDENTITY_INSERT [dbo].[tbl_future_tenant_critical_dates] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_lease_type] ON 

INSERT [dbo].[tbl_lease_type] ([lease_type_id], [lease_type]) VALUES (1, N'Gross Lease')
INSERT [dbo].[tbl_lease_type] ([lease_type_id], [lease_type]) VALUES (2, N'NNN Lease')
INSERT [dbo].[tbl_lease_type] ([lease_type_id], [lease_type]) VALUES (3, N'NN Lease')
SET IDENTITY_INSERT [dbo].[tbl_lease_type] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_map_cordinates] ON 

INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (619, 24, N'35.32122079747043', N'-80.64307100643055', N'850685', N'', N'Sams Proposed', N'sh_proposed', N'4029 NC-49, Harrisburg NC', N'1.37 Acres', N'$850,000.00 / GL', N'C-1')
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (620, 24, N'35.30836465941505', N'-80.71963063944713', N'850685', N'', N'Sams Proposed', N'sh_proposed', N'9810 University City Blvd, Charlotte NC Blvd', N'1.1', N'$1,200,000.00/ GL', N'B-1')
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (621, 24, N'35.211431343961166', N'-80.89746565296294', N'850685', N'', N'Sams Proposed', N'sh_proposed', N'2169 West Blvd, Charlotte NC', N'1.1 Acres', N'$350,000.00/GL', N'B-1')
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (622, 24, N'35.128819104860796', N'-80.73022310405959', N'850685', N'', N'Sams Proposed', N'sh_proposed', N'10701 Monroe Rd, Charlotte NC', N'1/2 acres', N'$4000.00 GL', N'I-1/ Commercial')
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (623, 24, N'35.255153107626015', N'-80.88996755744226', N'850685', N'42', N'Sams Proposed', N'sh_proposed', N'4201 Glenwood Dr, Charlotte NC', N'1.14 Acres', N'$285,000.00 / GL', N'Commercial')
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (624, 24, N'34.97947799879783', N'-80.98050754269237', N'850685', N'28', N'Sams Proposed', N'sh_proposed', N'2875 Cherry Rd, Rock Hill SC', N'1.1', N'750,000.00/GL', N'Commercial')
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (625, 24, N'35.176912647560606', N'-80.87617059682576', N'850685', N'28', N'Sams Proposed', N'sh_proposed', N'4646 South Blvd, Charlotte NC', N'0.6 acres', N'$7000.00/GL', N'B-2')
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (626, 24, N'35.17167864177685', N'-80.95970453868465', N'850685', N'Sh', N'Sams Proposed', N'sh_proposed', N'4704 Shopton Rd, Charlotte NC', N'0.7 Acres', N'$450,000 / GL', N'B-1')
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (644, 26, N'35.07228211076467', N'-80.73598093933106', N'850685', N'', N'Sams Proposed', N'sh_proposed', N'address 123', N'1250', N'56', N'')
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (645, 26, N'35.158218173853676', N'-80.72465128845215', N'850685', N'', N'Sams Proposed', N'sh_proposed', N'address 1', N'1500', N'5', N'96')
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (646, 26, N'35.1180284', N'-80.698252', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'11208 E Independence Blvd, Matthews, NC 28105, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (647, 26, N'35.1241186', N'-80.6540184', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'15000 Idlewild Rd, Stallings, NC 28104, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (648, 26, N'35.0758912', N'-80.65101829999999', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'304 Unionville Indian Trail Rd, Indian Trail, NC 28079, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (649, 26, N'35.1332403', N'-80.71117029999999', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'1700 Windsor Square Dr, Matthews, NC 28105, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (650, 26, N'35.135656', N'-80.783341', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'5701 Old Providence Rd, Charlotte, NC 28226, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (651, 26, N'35.0628844', N'-80.7710386', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'10806 Providence Rd, Charlotte, NC 28277, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (652, 26, N'35.0612248', N'-80.81282639999999', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'5200 Piper Station Dr, Charlotte, NC 28277, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (653, 26, N'35.0873333', N'-80.84602559999999', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'7511 Pineville-Matthews Rd, Charlotte, NC 28226, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (654, 26, N'35.1808787', N'-80.6473759', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'4300 Wilgrove Mint Hill Rd, Charlotte, NC 28227, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (655, 26, N'35.1714725', N'-80.8505577', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'4401 Park Rd, Charlotte, NC 28209, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (656, 26, N'35.1902523', N'-80.77304269999999', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'5343 Monroe Rd, Charlotte, NC 28205, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (657, 26, N'35.1197738', N'-80.8802616', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'8641 South Blvd, Charlotte, NC 28273, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (658, 26, N'35.1907663', N'-80.7982931', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'801 N Wendover Rd, Charlotte, NC 28211, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (659, 26, N'35.0847359', N'-80.8862963', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'105 S Polk St, Pineville, NC 28134, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (660, 26, N'35.1650031', N'-80.881012', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'838 Tyvola Rd, Charlotte, NC 28217, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (661, 26, N'35.2032405', N'-80.7994453', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'3301 Monroe Rd, Charlotte, NC 28205, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (662, 26, N'35.135965', N'-80.8915195', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'8925 Nations Ford Rd, Charlotte, NC 28217, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (663, 26, N'35.2148431', N'-80.7802534', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'3800 Central Ave, Charlotte, NC 28205, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (664, 26, N'35.2172413', N'-80.7936059', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'3024 Central Ave, Charlotte, NC 28205, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (665, 26, N'35.2175904', N'-80.7809661', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'7-Eleven', N'2840 Eastway Dr, Charlotte, NC 28205, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (666, 26, N'35.03574590213126', N'-80.88978953308106', N'850685', N'Sh', N'Sams Proposed', N'sh_proposed', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (667, 26, N'35.00144321040551', N'-80.53136057800293', N'850685', N'Sh', N'Sams Proposed', N'sh_proposed', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (668, 27, N'36.0568321', N'-79.8891302', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'4319 W Wendover Ave, Greensboro, NC 27407, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (669, 27, N'36.0636403', N'-79.81842329999999', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'1639 Spring Garden St RD, Greensboro, NC 27403, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (670, 27, N'36.0812074', N'-79.9304655', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'6930 W Market St, Greensboro, NC 27409, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (671, 27, N'36.0905914', N'-79.7010726', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'4401 Burlington Rd, Greensboro, NC 27405, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (672, 27, N'36.1166783', N'-79.8736473', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'1620 New Garden Rd, Greensboro, NC 27410, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (673, 27, N'36.059231', N'-79.9089503', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'5421 Hornaday Rd, Greensboro, NC 27407, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (674, 27, N'36.0867979', N'-79.8056411', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'1300 Battleground Ave, Greensboro, NC 27408, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (675, 27, N'36.0684963', N'-79.8543505', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'3941 W Market St, Greensboro, NC 27407, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (676, 27, N'36.165413', N'-79.7178917', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'4736 US-29, Greensboro, NC 27405, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (677, 27, N'36.0568836', N'-79.88907909999999', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'4319 W Wendover Ave, Greensboro, NC 274071910, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (678, 27, N'36.0949499', N'-79.99555869999999', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'3202 Sandy Ridge Rd, Colfax, NC 27235, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (679, 27, N'36.0014955', N'-79.9079813', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'1001 Gardner Hill Drive, Jamestown, NC 27282, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (680, 27, N'36.0596269', N'-79.9606902', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'2980 NC-68, High Point, NC 27265, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (681, 27, N'36.0332294', N'-79.955614', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'4120 Brian Jordan Pl, High Point, NC 27265, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (682, 27, N'36.1284999', N'-80.06064169999999', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'790 N Main St, Kernersville, NC 27284, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (683, 27, N'36.0936543', N'-80.0623969', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'1400 NC HWY 66 SOUTH, Kernersville, NC 27284, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (684, 27, N'36.0051414', N'-80.0382894', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'3350 N Main St, High Point, NC 27265, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (685, 27, N'35.9742632', N'-80.0344926', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'802 Westchester Dr, High Point, NC 27262, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (686, 27, N'35.8990957', N'-79.9467256', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'Sheetz', N'10206 S Main St, Archdale, NC 27263, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (687, 27, N'36.0651672', N'-79.9596621', N'http://maps.google.com/mapfiles/ms/icons/blue.png', N'ATM', N'514 Gallimore Dairy Rd, Greensboro, NC 27409, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (688, 27, N'36.062192', N'-79.8165142', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Circle K', N'1550 West Lee Street, Greensboro, NC 27403, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (689, 27, N'36.0215448', N'-79.8399391', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Circle K', N'2810 Pinecroft Rd, Greensboro, NC 27407, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (690, 27, N'36.1261183', N'-79.9044672', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Circle K', N'2200 Fleming Rd, Greensboro, NC 27410, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (691, 27, N'36.02388760000001', N'-79.8194717', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Circle K', N'3602 Rehobeth Church Rd, Greensboro, NC 27406, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (692, 27, N'36.1301078', N'-79.85506579999999', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Circle K', N'3701 Battleground Ave, Greensboro, NC 27410, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (693, 27, N'36.11326740000001', N'-79.8807964', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Circle K', N'1585 New Garden Rd, Greensboro, NC 27410, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (694, 27, N'36.0193261', N'-79.8464307', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Circle K', N'3302 S Holden Rd, Greensboro, NC 27407, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (695, 27, N'36.015044', N'-79.865996', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Circle K', N'3700 Groometown Rd, Greensboro, NC 27407, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (696, 27, N'36.02797', N'-79.76914699999999', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Circle K', N'3001 Pleasant Garden Rd, Greensboro, NC 27406, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (697, 27, N'36.0879378', N'-79.79403909999999', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Circle K', N'337 W Wendover Ave, Greensboro, NC 27408, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (698, 27, N'36.0306981', N'-79.8018657', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'CIRCLE K', N'2522 Randleman Rd, Greensboro, NC 27406, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (699, 27, N'36.1335134', N'-79.7900918', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Circle K', N'101 Pisgah Church Rd #2515, Greensboro, NC 27455, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (700, 27, N'36.0865624', N'-79.827713', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Circle K', N'621 Green Valley Rd, Greensboro, NC 27408, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (701, 27, N'36.1135025', N'-79.7756955', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Kangaroo Express', N'3101 Yanceyville St, Greensboro, NC 27405, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (702, 27, N'36.0908079', N'-79.9970639', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'Circle K', N'8400 Norcross Rd, Colfax, NC 27235, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (703, 27, N'36.0281414', N'-79.7692998', N'http://maps.google.com/mapfiles/ms/icons/green.png', N'CIRCLE K 2723782', N'3001 Pleasant Garden Rd, Greensboro, NC 27406, United States', N'competitor', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (704, 27, N'36.08651030115912', N'-79.80086469852377', N'850685', N'CI', N'Sams Proposed', N'sh_proposed', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (705, 27, N'36.06487063945148', N'-79.85147887663912', N'850685', N'CI', N'Sams Proposed', N'sh_proposed', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_map_cordinates] ([cordinated_id], [header_id], [latitude], [longitude], [marker_color], [marker_header], [marker_address], [marker_type], [added_address], [land_size], [asking_price], [zoning]) VALUES (706, 27, N'36.06133211249276', N'-79.8326175828464', N'850685', N'CI', N'Sams Proposed', N'sh_proposed', NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[tbl_map_cordinates] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_map_header] ON 

INSERT [dbo].[tbl_map_header] ([map_header_id], [header_name], [created_date]) VALUES (24, N'Proposed Scooters Coffee locations in Charlotte MSA', CAST(N'2020-09-30T12:45:43.577' AS DateTime))
INSERT [dbo].[tbl_map_header] ([map_header_id], [header_name], [created_date]) VALUES (26, N'ChangedMap', CAST(N'2020-10-16T05:24:02.653' AS DateTime))
INSERT [dbo].[tbl_map_header] ([map_header_id], [header_name], [created_date]) VALUES (27, N'Greensboro 7-Eleven', CAST(N'2020-10-22T17:55:40.880' AS DateTime))
SET IDENTITY_INSERT [dbo].[tbl_map_header] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_market] ON 

INSERT [dbo].[tbl_market] ([market_id], [market_name]) VALUES (1, N'South Carolina')
INSERT [dbo].[tbl_market] ([market_id], [market_name]) VALUES (2, N'North Carolina')
INSERT [dbo].[tbl_market] ([market_id], [market_name]) VALUES (3, N'Georgia')
INSERT [dbo].[tbl_market] ([market_id], [market_name]) VALUES (4, N'Virginia')
SET IDENTITY_INSERT [dbo].[tbl_market] OFF
GO
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (1, N'Dashboard')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (2, N'Surplus Properties')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (3, N'Net Lease Properties')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (4, N'C-Store Properties')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (5, N'New Property Dashboard')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (7, N'User List')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (8, N'C-Store Customers')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (9, N'Signedup Customers')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (10, N'Customer Message')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (11, N'Asset Type')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (12, N'Role List')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (13, N'SH Asset List')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (14, N'State List')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (15, N'MSA Map List')
INSERT [dbo].[tbl_module_master] ([module_id], [module_name]) VALUES (16, N'Admin Settings')
GO
INSERT [dbo].[tbl_month] ([month_id], [month_name]) VALUES (1, N'Jan')
INSERT [dbo].[tbl_month] ([month_id], [month_name]) VALUES (2, N'Feb')
INSERT [dbo].[tbl_month] ([month_id], [month_name]) VALUES (3, N'Mar')
INSERT [dbo].[tbl_month] ([month_id], [month_name]) VALUES (4, N'Apr')
INSERT [dbo].[tbl_month] ([month_id], [month_name]) VALUES (5, N'May')
INSERT [dbo].[tbl_month] ([month_id], [month_name]) VALUES (6, N'Jun')
INSERT [dbo].[tbl_month] ([month_id], [month_name]) VALUES (7, N'Jul')
INSERT [dbo].[tbl_month] ([month_id], [month_name]) VALUES (8, N'Aug')
INSERT [dbo].[tbl_month] ([month_id], [month_name]) VALUES (9, N'Sep')
INSERT [dbo].[tbl_month] ([month_id], [month_name]) VALUES (10, N'Oct')
INSERT [dbo].[tbl_month] ([month_id], [month_name]) VALUES (11, N'Nov')
INSERT [dbo].[tbl_month] ([month_id], [month_name]) VALUES (12, N'Dec')
GO
SET IDENTITY_INSERT [dbo].[tbl_net_lease_files] ON 

INSERT [dbo].[tbl_net_lease_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (3, 2, N'file', N'b4_a47b.pdf')
INSERT [dbo].[tbl_net_lease_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (5, 8, N'Layout', N'10039 University City Blvd - 01_05ce.pdf')
SET IDENTITY_INSERT [dbo].[tbl_net_lease_files] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_net_lease_property] ON 

INSERT [dbo].[tbl_net_lease_property] ([net_lease_property_id], [asset_id], [asset_name], [state_id], [city], [cap_rate], [term], [detail_pdf], [created_date], [property_price], [asset_type_id], [is_deleted], [asset_status], [is_shopping_center], [property_address], [property_zipcode], [diligence_type], [property_latitude], [property_longitude], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [asking_rent], [lease_type], [shopping_mart_plan_file_name], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (1, N'SM # 6001', N'Beatties Ford Express Shopping Center', 2, N'Charlotte', 0, N'10 Years', N'v5_5a19.pdf', CAST(N'2020-05-01T08:45:18.200' AS DateTime), N'2000.00 per month', 1, 1, 0, 1, N'1121 Beatties Ford Rd', N'28216', NULL, N'35.25376699999999', N'-80.856466', 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_net_lease_property] ([net_lease_property_id], [asset_id], [asset_name], [state_id], [city], [cap_rate], [term], [detail_pdf], [created_date], [property_price], [asset_type_id], [is_deleted], [asset_status], [is_shopping_center], [property_address], [property_zipcode], [diligence_type], [property_latitude], [property_longitude], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [asking_rent], [lease_type], [shopping_mart_plan_file_name], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (2, N'SM # 4', N'7-11', 2, N'Mint Hill', 5.6, N'15 Years', N'b2_8ee7.pdf', CAST(N'2020-05-01T09:30:15.663' AS DateTime), N'4,750,000', 2, 1, 0, 0, N'4300 Wilgrove-Mint Hill Rd', N'28227', NULL, N'35.1808787', N'-80.6473759', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_net_lease_property] ([net_lease_property_id], [asset_id], [asset_name], [state_id], [city], [cap_rate], [term], [detail_pdf], [created_date], [property_price], [asset_type_id], [is_deleted], [asset_status], [is_shopping_center], [property_address], [property_zipcode], [diligence_type], [property_latitude], [property_longitude], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [asking_rent], [lease_type], [shopping_mart_plan_file_name], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (3, N'SH-10044', N'VERIZON AND DUNKIN CENTER', 2, N'Latham, NY', 8, N'Various', N'Sam''sHolding_Example_7116.pdf', CAST(N'2020-05-02T04:16:53.747' AS DateTime), N'0', 2, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_net_lease_property] ([net_lease_property_id], [asset_id], [asset_name], [state_id], [city], [cap_rate], [term], [detail_pdf], [created_date], [property_price], [asset_type_id], [is_deleted], [asset_status], [is_shopping_center], [property_address], [property_zipcode], [diligence_type], [property_latitude], [property_longitude], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [asking_rent], [lease_type], [shopping_mart_plan_file_name], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (4, N'SH-002', N'Asset', 3, N'Trichur', 680652, N'', N'mE UI-09_31fc.png', CAST(N'2020-05-06T13:13:13.123' AS DateTime), N'25600', 1, 1, 0, 1, N'', N'', NULL, NULL, NULL, 1, 0, N'', CAST(N'2020-11-16T15:22:02.000' AS DateTime), N'', N'', 0, NULL, N'', NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_net_lease_property] ([net_lease_property_id], [asset_id], [asset_name], [state_id], [city], [cap_rate], [term], [detail_pdf], [created_date], [property_price], [asset_type_id], [is_deleted], [asset_status], [is_shopping_center], [property_address], [property_zipcode], [diligence_type], [property_latitude], [property_longitude], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [asking_rent], [lease_type], [shopping_mart_plan_file_name], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (5, N'SM # 15', N'7-11', 2, N'Charlotte', 5.25, N'12', N'', CAST(N'2020-05-07T06:09:25.557' AS DateTime), N'4,735,000', 1, 0, 0, 0, N'9025 Mallard Creek Rd', N'28262', 2, N'35.32723185375574', N'-80.77369451522827', 3, 1, N'test', CAST(N'2020-10-28T12:22:45.000' AS DateTime), N'', N'3', 2, NULL, N'details', NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_net_lease_property] ([net_lease_property_id], [asset_id], [asset_name], [state_id], [city], [cap_rate], [term], [detail_pdf], [created_date], [property_price], [asset_type_id], [is_deleted], [asset_status], [is_shopping_center], [property_address], [property_zipcode], [diligence_type], [property_latitude], [property_longitude], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [asking_rent], [lease_type], [shopping_mart_plan_file_name], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (6, N'SM # 9', N'7-11', 2, N'Charlotte', 4.75, N'', N'', CAST(N'2020-05-21T01:31:31.893' AS DateTime), N'5,100,000', 2, 1, 0, 0, N'1120 W. Sugar Creek Rd', N'28208', NULL, N'35.2761039', N'-80.7926889', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_net_lease_property] ([net_lease_property_id], [asset_id], [asset_name], [state_id], [city], [cap_rate], [term], [detail_pdf], [created_date], [property_price], [asset_type_id], [is_deleted], [asset_status], [is_shopping_center], [property_address], [property_zipcode], [diligence_type], [property_latitude], [property_longitude], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [asking_rent], [lease_type], [shopping_mart_plan_file_name], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (7, N'SH-002', N'Asset', 3, N'Trivandrum', 589652, N'10 Years', N'', CAST(N'2020-10-14T09:35:44.457' AS DateTime), N'10,023,01', 1, 1, 0, 1, N'address 1', N'ee', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_net_lease_property] ([net_lease_property_id], [asset_id], [asset_name], [state_id], [city], [cap_rate], [term], [detail_pdf], [created_date], [property_price], [asset_type_id], [is_deleted], [asset_status], [is_shopping_center], [property_address], [property_zipcode], [diligence_type], [property_latitude], [property_longitude], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [asking_rent], [lease_type], [shopping_mart_plan_file_name], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (8, N'SM # 6005', N'49er  Plaza', 2, N'Charlotte', 28262, N'', N'', CAST(N'2020-10-17T14:37:22.493' AS DateTime), N'0.0', 1, 0, 0, 1, N'10039 University City Blvd', N'28269', 3, N'35.3107921', N'-80.7161778', 1, 1, N'', CAST(N'2020-10-28T00:00:00.000' AS DateTime), N'', N'', 0, NULL, N'test details', 1, CAST(N'2020-11-18T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tbl_net_lease_property] ([net_lease_property_id], [asset_id], [asset_name], [state_id], [city], [cap_rate], [term], [detail_pdf], [created_date], [property_price], [asset_type_id], [is_deleted], [asset_status], [is_shopping_center], [property_address], [property_zipcode], [diligence_type], [property_latitude], [property_longitude], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [asking_rent], [lease_type], [shopping_mart_plan_file_name], [details], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (9, N'NL-SH-002', N'Asset', 3, N'Trichur', 680652, N'', N'', CAST(N'2020-11-18T15:53:24.750' AS DateTime), N'10,023,01', 1, NULL, 0, 0, N'Test address', N'28205', NULL, N'', N'', 2, 0, N'', CAST(N'2020-11-18T21:23:34.000' AS DateTime), N'', N'', 0, NULL, N'', NULL, CAST(N'2020-11-18T00:00:00.000' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[tbl_net_lease_property] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_new_property_status] ON 

INSERT [dbo].[tbl_new_property_status] ([new_property_status_id], [new_property_status_name]) VALUES (1, N'Research/Vetting')
INSERT [dbo].[tbl_new_property_status] ([new_property_status_id], [new_property_status_name]) VALUES (2, N'Under LOI')
INSERT [dbo].[tbl_new_property_status] ([new_property_status_id], [new_property_status_name]) VALUES (3, N'Under Contract')
INSERT [dbo].[tbl_new_property_status] ([new_property_status_id], [new_property_status_name]) VALUES (4, N'Closed Acquisitions')
INSERT [dbo].[tbl_new_property_status] ([new_property_status_id], [new_property_status_name]) VALUES (5, N'Terminated Acquisitions')
SET IDENTITY_INSERT [dbo].[tbl_new_property_status] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_period] ON 

INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (2, 5, 2, N'First Extended Permitting Period', CAST(N'2020-10-20T00:00:00.000' AS DateTime), CAST(N'2020-10-28T00:00:00.000' AS DateTime), N'tes', NULL, 1)
INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (3, 40, 1, N'test', CAST(N'2020-10-12T00:00:00.000' AS DateTime), CAST(N'2020-10-22T00:00:00.000' AS DateTime), N'test notes', NULL, NULL)
INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (6, 32, 1, N'Survey/Title comments', CAST(N'2020-08-24T00:00:00.000' AS DateTime), CAST(N'2020-10-08T00:00:00.000' AS DateTime), N'Survey/Title', NULL, 1)
INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (7, 32, 1, N'DDP', CAST(N'2020-08-24T00:00:00.000' AS DateTime), CAST(N'2020-11-22T00:00:00.000' AS DateTime), N'DDP 90 days', NULL, 1)
INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (8, 32, 1, N'Closing', CAST(N'2020-11-24T00:00:00.000' AS DateTime), CAST(N'2020-12-24T00:00:00.000' AS DateTime), N'Closing to happen after QT Waiver', NULL, NULL)
INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (9, 33, 1, N'Listing with Tracy', CAST(N'2020-07-31T00:00:00.000' AS DateTime), CAST(N'2020-12-31T00:00:00.000' AS DateTime), N'Listing Expires Dec 31, 2020 with Tracy', NULL, NULL)
INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (10, 11, 3, N'tesb', CAST(N'2020-10-15T00:00:00.000' AS DateTime), CAST(N'2020-10-30T00:00:00.000' AS DateTime), N'e', NULL, 1)
INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (12, 32, 1, N'Title & Survey', CAST(N'2020-10-19T00:00:00.000' AS DateTime), CAST(N'2020-11-05T00:00:00.000' AS DateTime), N'test', N'Disposition', NULL)
INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (13, 18, 5, N'First Extended Permitting Period', CAST(N'2020-10-20T00:00:00.000' AS DateTime), CAST(N'2020-10-28T00:00:00.000' AS DateTime), N'test', N'Lease', NULL)
INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (14, 18, 5, N'third party dd', CAST(N'2020-11-01T00:00:00.000' AS DateTime), CAST(N'2020-12-15T00:00:00.000' AS DateTime), N'dsafljasdf', N'Acquisition', NULL)
INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (15, 5, 2, N'DDP', CAST(N'2020-11-02T00:00:00.000' AS DateTime), CAST(N'2020-11-28T00:00:00.000' AS DateTime), N'ljj', N'Disposition', NULL)
INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (16, 30, 1, N'Survey/Title comments', CAST(N'2020-11-10T00:00:00.000' AS DateTime), CAST(N'2020-11-30T00:00:00.000' AS DateTime), N'hh', N'Lease', NULL)
INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (17, 32, 1, N'Closing', CAST(N'2020-11-25T00:00:00.000' AS DateTime), CAST(N'2020-12-25T00:00:00.000' AS DateTime), N'30 days after the expiration of DDP', N'Disposition', NULL)
INSERT [dbo].[tbl_period] ([period_id], [property_id], [property_type], [period_master], [start_date], [end_date], [period_notes], [period_type], [hide_notification]) VALUES (18, 38, 1, N'Closing', CAST(N'2020-11-09T00:00:00.000' AS DateTime), CAST(N'2020-11-12T00:00:00.000' AS DateTime), N'closing', N'Disposition', NULL)
SET IDENTITY_INSERT [dbo].[tbl_period] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_property] ON 

INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (30, N'', N'', N'', N'', N'', N'', N'', N'0', N'28208', N'', 0, 0, N'2169 West Blvd.', N'Charlotte', 2, N'Mecklenburg', N'', 0, N'B-1 & R-5', N'1.1 Acres', N'350,000', N'Vacant Land', CAST(N'2020-05-18T15:22:28.853' AS DateTime), 1, NULL, N'1.1 Acres Vacant Land', 1, NULL, 0, 3, N'35.211253899999996', N'-80.89735920365409', N'SM 4002', 1, 1, N'', CAST(N'2020-10-28T06:29:24.000' AS DateTime), N'550,000.00', N'10 Years', N'12522', 2, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (31, N'', N'', N'', N'', N'', N'', N'', N'0', N'', N'', 0, 0, N'3100 N. Sharon Amity Rd.', N'Charlotte, NC 28205', 2, N'Mecklenburg', N'', 1, N'B-1  ', N'0.91 Acres', N'14,50,000', N'Vacant Land', CAST(N'2020-05-18T15:22:28.853' AS DateTime), 1, NULL, N'SM 4009', 2, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (32, N'', N'', N'', N'', N'', N'', N'', N'0', N'28105', N'', 0, 0, N'10701 Monroe Road', N'Matthews', 2, N'Mecklenburg', N'', 0, N'I-1(CD)', N'2.87 Acres 1', N'550,000', N'Vacant Land Behind a QuikTrip development', CAST(N'2020-05-18T15:22:28.853' AS DateTime), 1, NULL, N'SM 4011 ', 1, NULL, 0, 2, N'35.128899004981285', N'-80.73090320145958', N'SM 4011 ', 1, 1, N'Abc realty', CAST(N'2021-02-23T00:00:00.000' AS DateTime), N'550,000.00', N'10 Years', N'test', 1, NULL, CAST(N'2020-11-10T00:00:00.000' AS DateTime), 1, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (33, N'', N'', N'', N'', N'', N'', N'', N'0', N'28105', N'', 0, 0, N'10701 - Tract 2 Monroe Road Matthews', N'Matthews', 2, N'Mecklenburg', N'', 0, N'I-1', N'1.243 Acres', N'625,000', N'Vacant Land', CAST(N'2020-05-18T15:22:28.853' AS DateTime), 1, NULL, N'1.243 Acres Vacant Land', 2, NULL, 0, 1, N'35.1282277', N'-80.7309913', N'SM 4307.2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (34, N'', N'', N'', N'', N'', N'', N'', N'0', N'28213', N'', 0, 0, N'9810 University City Blvd.', N'Charlotte', 2, N'Mecklenburg', N'', 0, N'B-1', N'1.1 Acres', N'1,200,000', N'Closed Store', CAST(N'2020-05-18T15:22:28.853' AS DateTime), 1, NULL, N'1.1 Acres Closed Store', 2, NULL, 0, 2, N'35.308163', N'-80.71967599999999', N'SM 4074', 1, 1, N'', CAST(N'2020-11-02T13:37:44.000' AS DateTime), N'', N'', N'', 0, 1, NULL, NULL, 1)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (35, N'', N'', N'', N'', N'', N'', N'', N'0', N'28110', N'', 0, 0, N'1004 W. Roosevelt Blvd.', N'Monroe', 2, N'Union', N'', 0, N'CBD', N'0.92 Acres', N'559,000', N'Vacant Land', CAST(N'2020-05-18T15:22:28.853' AS DateTime), 1, NULL, N'0.92 Acres Vacant Land', 2, NULL, 0, NULL, N'34.9927742', N'-80.5370301', N'SM 4091', 3, 0, N'', CAST(N'2020-11-14T11:50:16.000' AS DateTime), N'', N'', N'', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (36, N'', N'', N'', N'', N'', N'', N'', N'0', N'28075', N'', 0, 0, N'4025 NC Highway 49 (Tract 2) ', N'Harrisburg', 2, N'Cabarrus', N'', 0, N'C-1', N'1.37 Acres', N'822,000', N'Vacant Land', CAST(N'2020-05-18T15:22:28.853' AS DateTime), 1, NULL, N'1.37 Acres Vacant Land', 2, NULL, 0, NULL, N'35.3220973', N'-80.643227', N'SM 4095.2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (37, N'', N'', N'', N'', N'', N'', N'', N'0', N'28212', N'', 0, 0, N'9258 Lawyers Road', N'Mint Hill', 2, N'Mecklenburg', N'', 0, N'Commercial', N'1.721 Acres', N'1,250,000', N'Vacant Land', CAST(N'2020-05-18T15:22:28.853' AS DateTime), 1, NULL, N'1.721 Acres Vacant Land', 2, NULL, 0, NULL, N'35.1851758', N'-80.6872205', N'SM 4314', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (38, N'', N'', N'', N'', N'', N'', N'', N'0', N'28208', N'', 0, 0, N'4201 Glenwood Drive', N'Charlotte', 2, N'Mecklenburg', N'', 0, N'B-1', N'1.14 Acres', N'285,000', N'Vacant Land', CAST(N'2020-05-18T15:22:28.853' AS DateTime), 1, NULL, N'1.14 Acres Vacant Land', 2, NULL, 0, NULL, N'35.2549596', N'-80.8899417', N'SM 4721', 3, 0, N'', CAST(N'2020-11-13T13:51:02.000' AS DateTime), N'', N'', N'', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (39, N'', N'', N'', N'', N'', N'', N'', N'0', N'28208', N'', 0, 0, N'Thomasboro Drive', N'Charlotte', 2, N'Mecklenburg', N'', 0, N'B-1', N'0.61 Acres', N'80,000', N'Vacant Land', CAST(N'2020-05-18T15:22:28.853' AS DateTime), 1, NULL, N'0.61 Acres Vacant Land', 2, 1, 0, NULL, N'35.2522851', N'-80.8935656', N'SM 4722', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (40, N'', N'', N'', N'', N'', N'', N'', N'0', N'28205', N'', 0, 0, N'3100 N. Sharon Amity Rd.', N'Charlotte', 2, N'Mecklenburg', N'Albemarle Rd', 0, N'B-1', N'0.91 Acres', N'1,450,000.00', N'Vacant Land', CAST(N'2020-06-11T17:14:34.920' AS DateTime), 1, NULL, N'0.91 Acres Vacant Land ', 2, 1, 0, NULL, N'35.20199618445749', N'-80.7602531711132', N'SM 4009', 3, 0, N'', CAST(N'2020-11-16T07:10:54.000' AS DateTime), N'', N'', N'', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (41, N'', N'', N'', N'', N'', N'', N'', N'0', N'589652', N'', 0, 0, N'test locatopn', N'test', 2, N'North Carolina', N'3100 N. Sharon Amity Rd.', 0, N'zone', N'2400', N'110000', N'', CAST(N'2020-11-18T15:52:11.800' AS DateTime), 1, NULL, N'710 Powder Springs Street', 1, 1, 0, NULL, N'', N'', N'SH-002 - 556', 2, 0, N'', CAST(N'2020-11-18T21:22:25.000' AS DateTime), N'', N'', N'', 0, NULL, CAST(N'2020-11-27T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (42, N'', N'', N'', N'', N'', N'', N'', N'0', N'28213', N'', 0, 0, N'9810 University City Boulevard, Charlotte, NC, USA', N'Charlotte', 2, N'Meck', N'', 0, N'', N'1.1', N'1,200,000', N'', CAST(N'2020-11-19T00:53:42.203' AS DateTime), 1, NULL, N'For Sale', 1, 1, 0, NULL, N'35.308163', N'-80.71967599999999', N'', 0, 0, N'', NULL, N'', N'', N'', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (43, N'', N'', N'', N'', N'', N'', N'', N'0', N'28213', N'', 0, 0, N'9810 University City Boulevard, Charlotte, NC, USA', N'charlotte', 2, N'Mecklenburg', N'', 0, N'B1', N'1.1', N'1,200,000', N'', CAST(N'2020-11-19T01:17:47.337' AS DateTime), 1, NULL, N'9810 University City Blvd Charlotte, NC 28213', 2, NULL, 0, 2, N'35.308163', N'-80.71967599999999', N'', 0, 0, N'', NULL, N'', N'', N'', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (44, N'', N'', N'', N'', N'', N'', N'', N'0', N'28205', N'', 0, 0, N'3400-3410 The Plaza, Charlotte, NC, USA', N'Charlotte', 2, N'Mecklenburg', N'Herrin Avenue', 0, N'Commercial', N'1.58', N'1,900,00', N'-Corner parcel just north of 36th Street
-250'' ± frontage on The Plaza
-152'' ± frontage on Herrin Avenue
- Quick access to Interstate 277 & Highway 74', CAST(N'2020-11-19T01:30:22.077' AS DateTime), 1, NULL, N'For Sale', 2, NULL, 0, 2, N'35.2420979', N'-80.79514569999999', N'4027', 0, 0, N'', NULL, N'', N'', N'', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[tbl_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [property_header], [asset_type_id], [is_deleted], [asset_status], [diligence_type], [property_latitude], [property_longitude], [asset_id], [property_status_id], [check_if_property_listed], [listing_agent_name], [listing_expiry], [listing_price], [term], [asking_rent], [lease_type], [hide_notification], [status_changed_date], [is_closed], [can_publish]) VALUES (45, N'', N'', N'', N'', N'', N'', N'', N'0', N'28217', N'', 0, 0, N'101 W Woodlawn Rd, Charlotte, NC, USA', N'Charlotte', 2, N'Mecklenburg', N'', 0, N'I-2 Heavy Industrial', N'.95', N'1,450,000', N'Road frontage on both sides
Quick access to Airport, Uptown', CAST(N'2020-11-19T01:55:30.587' AS DateTime), 1, NULL, N'101 West Woodlawn Rd', 1, NULL, 0, 2, N'35.1789805', N'-80.8841715', N'4012', 0, 0, N'', NULL, N'', N'', N'', 0, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[tbl_property] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_property_images] ON 

INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (29, 2, N'WillardStreet_f5f3.jpg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (30, 27, N'2_3b63.jpeg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (31, 26, N'3_feef.jpeg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (32, 16, N'4_a8a3.jpg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (33, 20, N'5_832b.jpg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (34, 22, N'6_2c6b.jpg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (35, 25, N'7_0917.jpg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (36, 24, N'9_e828.jpg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (37, 23, N'10_fa80.jpg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (38, 19, N'11_9a2d.jpg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (39, 21, N'12_e2ed.jpeg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (40, 3, N'1_44e4.jpg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (41, 5, N'2_1d2b.jpg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (42, 1, N'3_a313.jpg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (43, 6, N'4_5f19.jpg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (44, 4, N'5_1388.jpg', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (45, 28, N'CHICK+FIL+A+DESIGN+DOC_7c2b.jfif', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (47, 8, N'CHICK+FIL+A+DESIGN+DOC_8a1e.jfif', NULL)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (48, 24, N'2_e414.jpeg', 1)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (52, 3, N'9_9714.jpg', 3)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (53, 3, N'12_7760.jpeg', 3)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (57, 1, N'Beattis Ford Shopping Center_5afb.jpg', 4)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (59, 1, N'Beattisford signs_622c.jpg', 2)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (61, 30, N'7249 - CSP 1- 08-26-20_09ba.jpg', 1)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (63, 43, N'DJI_0112_0ca3.JPG', 1)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (64, 43, N'DJI_0116_d881.JPG', 1)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (65, 43, N'DJI_0111_6932.JPG', 1)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (66, 43, N'DJI_0115_96b5.JPG', 1)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (67, 44, N'DJI_0082_9711.JPG', 1)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (68, 44, N'DJI_0081_b4ca.JPG', 1)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (69, 44, N'DJI_0078_4b01.JPG', 1)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (70, 44, N'DJI_0077_fa48.JPG', 1)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (71, 45, N'DJI_0075_0ea7.JPG', 1)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (72, 45, N'DJI_0071_f630.JPG', 1)
INSERT [dbo].[tbl_property_images] ([image_id], [property_id], [image_name], [property_type]) VALUES (73, 45, N'DJI_0068_2d2a.JPG', 1)
SET IDENTITY_INSERT [dbo].[tbl_property_images] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_property_status] ON 

INSERT [dbo].[tbl_property_status] ([property_status_id], [property_status]) VALUES (1, N'Available')
INSERT [dbo].[tbl_property_status] ([property_status_id], [property_status]) VALUES (2, N'Under Contract')
INSERT [dbo].[tbl_property_status] ([property_status_id], [property_status]) VALUES (3, N'Sold')
SET IDENTITY_INSERT [dbo].[tbl_property_status] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_property_type] ON 

INSERT [dbo].[tbl_property_type] ([property_type_id], [property_type_name]) VALUES (1, N'Property Type 1')
INSERT [dbo].[tbl_property_type] ([property_type_id], [property_type_name]) VALUES (2, N'Property Type 5')
SET IDENTITY_INSERT [dbo].[tbl_property_type] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_role] ON 

INSERT [dbo].[tbl_role] ([role_id], [role_name], [can_publish_listing]) VALUES (4, N'Admin', 1)
INSERT [dbo].[tbl_role] ([role_id], [role_name], [can_publish_listing]) VALUES (5, N'Accountant', NULL)
INSERT [dbo].[tbl_role] ([role_id], [role_name], [can_publish_listing]) VALUES (6, N'Surplus Manager', NULL)
INSERT [dbo].[tbl_role] ([role_id], [role_name], [can_publish_listing]) VALUES (7, N'Net Lease Manager', NULL)
INSERT [dbo].[tbl_role] ([role_id], [role_name], [can_publish_listing]) VALUES (8, N'Shopping Center Manager', NULL)
INSERT [dbo].[tbl_role] ([role_id], [role_name], [can_publish_listing]) VALUES (9, N'C Store Manager', NULL)
INSERT [dbo].[tbl_role] ([role_id], [role_name], [can_publish_listing]) VALUES (10, N'Test Role', NULL)
SET IDENTITY_INSERT [dbo].[tbl_role] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_role_permission] ON 

INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (28, 5, 6, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (205, 4, 6, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (456, 4, 1, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (457, 4, 2, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (458, 4, 3, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (459, 4, 4, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (460, 4, 5, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (461, 4, 7, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (462, 4, 8, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (463, 4, 9, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (464, 4, 10, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (465, 4, 11, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (466, 4, 12, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (467, 4, 13, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (468, 4, 14, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (469, 4, 15, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (470, 4, 16, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (486, 7, 1, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (487, 7, 2, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (488, 7, 3, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (489, 7, 4, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (490, 7, 5, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (491, 7, 7, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (492, 7, 8, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (493, 7, 9, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (494, 7, 10, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (495, 7, 11, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (496, 7, 12, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (497, 7, 13, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (498, 7, 14, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (499, 7, 15, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (500, 7, 16, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (501, 8, 1, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (502, 8, 2, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (503, 8, 3, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (504, 8, 4, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (505, 8, 5, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (506, 8, 7, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (507, 8, 8, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (508, 8, 9, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (509, 8, 10, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (510, 8, 11, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (511, 8, 12, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (512, 8, 13, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (513, 8, 14, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (514, 8, 15, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (515, 8, 16, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (516, 9, 1, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (517, 9, 2, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (518, 9, 3, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (519, 9, 4, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (520, 9, 5, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (521, 9, 7, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (522, 9, 8, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (523, 9, 9, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (524, 9, 10, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (525, 9, 11, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (526, 9, 12, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (527, 9, 13, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (528, 9, 14, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (529, 9, 15, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (530, 9, 16, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (546, 10, 1, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (547, 10, 2, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (548, 10, 3, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (549, 10, 4, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (550, 10, 5, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (551, 10, 7, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (552, 10, 8, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (553, 10, 9, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (554, 10, 10, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (555, 10, 11, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (556, 10, 12, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (557, 10, 13, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (558, 10, 14, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (559, 10, 15, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (560, 10, 16, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (576, 5, 1, 1, 1, 1, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (577, 5, 2, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (578, 5, 3, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (579, 5, 4, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (580, 5, 5, 1, 1, 1, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (581, 5, 7, 1, 1, 1, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (582, 5, 8, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (583, 5, 9, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (584, 5, 10, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (585, 5, 11, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (586, 5, 12, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (587, 5, 13, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (588, 5, 14, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (589, 5, 15, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (590, 5, 16, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (606, 6, 1, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (607, 6, 2, 1, 1, 1, 1)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (608, 6, 3, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (609, 6, 4, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (610, 6, 5, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (611, 6, 7, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (612, 6, 8, 1, 0, 0, 0)
GO
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (613, 6, 9, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (614, 6, 10, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (615, 6, 11, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (616, 6, 12, 0, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (617, 6, 13, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (618, 6, 14, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (619, 6, 15, 1, 0, 0, 0)
INSERT [dbo].[tbl_role_permission] ([role_permission_id], [role_id], [module_id], [can_read], [can_edit], [can_create], [can_delete]) VALUES (620, 6, 16, 0, 0, 0, 0)
SET IDENTITY_INSERT [dbo].[tbl_role_permission] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_sams_locations] ON 

INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (250, N'4', N'4300 Wilgrove-Mint Hill Road', N'Mint Hill', N'North Carolina', N'28227', N'Mecklenburg', N'', N'35.1807478', N'-80.6475189')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (251, N'6', N'6233 Albemarle Road.', N'Charlotte', N'North Carolina', N'28212', N'Mecklenburg', N'', N'35.2033580', N'-80.7391541')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (252, N'9', N'1120 W. Sugar Creek Road', N'Charlotte', N'North Carolina', N'28213', N'Mecklenburg', N'', N'35.2762189', N'-80.7932201')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (253, N'11', N'9701 Sam Furr Road', N'Huntersville', N'North Carolina', N'28078', N'Mecklenburg', N'', N'35.4425182', N'-80.8637673')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (254, N'12', N'10700 Reames Road', N'Charlotte', N'North Carolina', N'28269', N'Mecklenburg', N'', N'35.3456380', N'-80.8393358')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (255, N'14', N'9608 University City Blvd.', N'Charlotte', N'North Carolina', N'28213', N'Mecklenburg', N'', N'35.3066604', N'-80.7222213')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (256, N'15', N'9025 Mallard Creek Road', N'Charlotte', N'North Carolina', N'28262', N'Mecklenburg', N'', N'35.3270148', N'-80.7735718')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (257, N'20', N'7740 Bruton Smith Blvd.', N'Concord', N'North Carolina', N'28027', N'Cabarrus', N'', N'35.3659921', N'-80.7101800')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (258, N'21', N'9759 Charlotte Highway', N'Indian Land', N'South Carolina', N'29707', N'Lancaster', N'', N'35.0010896', N'-80.8562873')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (259, N'23', N'668 West John Street ', N'Matthews', N'North Carolina', N'28105', N'Mecklenburg', N'', N'35.1220040', N'-80.7281764')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (260, N'25', N'6201 N. Tryon Street', N'Charlotte', N'North Carolina', N'28213', N'Mecklenburg', N'', N'35.2713861', N'-80.7681396')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (261, N'29', N'3024 Central Avenue', N'Charlotte', N'North Carolina', N'28205', N'Mecklenburg', N'', N'35.2172792', N'-80.7936528')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (262, N'35', N'8925 Nations Ford Road', N'Charlotte', N'North Carolina', N'28217', N'Mecklenburg', N'', N'35.1360814', N'-80.8915192')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (263, N'38', N'6886 Poplar Tent Road', N'Concord', N'North Carolina', N'28027', N'Cabarrus', N'', N'35.4028910', N'-80.6932540')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (264, N'39', N'6401 Old Statesville Rd.', N'Charlotte', N'North Carolina', N'28269', N'Mecklenburg', N'', N'35.3108587', N'-80.8383710')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (265, N'40', N'4808 Brookshire Blvd', N'Charlotte', N'North Carolina', N'28216', N'Mecklenburg', N'', N'35.2738119', N'-80.8849279')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (266, N'41', N'2901 Yorkmont Road', N'Charlotte', N'North Carolina', N'28208', N'Mecklenburg', N'', N'35.1898304', N'-80.9298029')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (267, N'42', N'2825 Little Rock Road', N'Charlotte', N'North Carolina', N'28214', N'Mecklenburg', N'', N'35.2375038', N'-80.9392629')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (268, N'43', N'3800 Central Avenue', N'Charlotte', N'North Carolina', N'28205', N'Mecklenburg', N'', N'35.2148527', N'-80.7802805')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (269, N'46', N'105 South Polk Street', N'Pineville', N'North Carolina', N'28134', N'Mecklenburg', N'', N'35.0847291', N'-80.8863111')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (270, N'48', N'2840 Eastway Drive', N'Charlotte', N'North Carolina', N'28205', N'Mecklenburg', N'', N'35.2175512', N'-80.7810595')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (271, N'49', N'5235 South Boulevard', N'Charlotte', N'North Carolina', N'28217', N'Mecklenburg', N'', N'35.1651621', N'-80.8757640')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (272, N'52', N'5343 Monroe Road ', N'Charlotte', N'North Carolina', N'28205', N'Mecklenburg', N'', N'35.1902674', N'-80.7730308')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (273, N'53', N'5115 Old Dowd Road', N'Charlotte', N'North Carolina', N'28208', N'Mecklenburg', N'', N'35.2300138', N'-80.9254180')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (274, N'54', N'2415 Little Rock Rd.', N'Charlotte', N'North Carolina', N'28214', N'Mecklenburg', N'', N'35.2461035', N'-80.9361745')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (275, N'55', N'5455 Brookshire Boulevard ', N'Charlotte', N'North Carolina', N'28216', N'Mecklenburg', N'', N'35.2780584', N'-80.8946742')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (276, N'56', N'2932 Mt. Holly-Huntersville Rd', N'Charlotte', N'North Carolina', N'28214', N'Mecklenburg', N'', N'35.3201722', N'-80.9526303')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (277, N'60', N'527 Providence Road ', N'Charlotte', N'North Carolina', N'28207', N'Mecklenburg', N'', N'35.2026913', N'-80.8245436')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (278, N'63', N'1920 Central Avenue', N'Charlotte', N'North Carolina', N'28205', N'Mecklenburg', N'', N'35.2196713', N'-80.8071528')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (279, N'67', N'4847 Charlotte Hwy', N'Lake Wylie', N'South Carolina', N'29710', N'York', N'', N'35.1152076', N'-81.0693826')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (280, N'69', N'2701 The Plaza', N'Charlotte', N'North Carolina', N'28205', N'Mecklenburg', N'', N'35.2367187', N'-80.8029992')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (281, N'77', N'3305 Scott Futrell Drive', N'Charlotte', N'North Carolina', N'28208', N'Mecklenburg', N'', N'35.2369705', N'-80.9214218')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (282, N'79', N'10222 Johnston Road', N'Pineville', N'North Carolina', N'28210', N'Mecklenburg', N'', N'35.0918609', N'-80.8568494')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (283, N'80', N'8325 Old Statesville Road', N'Charlotte', N'North Carolina', N'28269', N'Mecklenburg', N'', N'35.3359721', N'-80.8254304')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (284, N'81', N'10130 Charlotte Highway', N'Indian Land', N'South Carolina', N'29707', N'Lancaster', N'', N'35.0133289', N'-80.8510984')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (285, N'87', N'923 S. Kings Drive', N'Charlotte', N'North Carolina', N'28204', N'Mecklenburg', N'', N'35.2061009', N'-80.8357894')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (286, N'92', N'5701 NC Hwy 74 W', N'Indian Trail', N'North Carolina', N'28079', N'Union', N'', N'35.0622456', N'-80.6358964')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (287, N'95', N'4025 Highway 49 South', N'Harrisburg', N'North Carolina', N'28075', N'Cabarrus', N'', N'35.3220973', N'-80.6432270')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (288, N'98', N'3650 N. Highway 16 Business ', N'Denver', N'North Carolina', N'28037', N'Lincoln', N'', N'35.5308124', N'-81.0265302')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (289, N'99', N'100 E. Woodlawn Road', N'Charlotte', N'North Carolina', N'28217', N'Mecklenburg', N'', N'35.1786792', N'-80.8832854')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (290, N'100', N'4646 South Boulevard', N'Charlotte', N'North Carolina', N'28209', N'Mecklenburg', N'', N'35.1769779', N'-80.8761663')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (291, N'101', N'7340 Poplar Tent Rd.', N'Concord', N'North Carolina', N'28027', N'Cabarrus', N'', N'35.4029799', N'-80.7003109')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (292, N'102', N'1021 Aspinal Street ', N'Waxhaw', N'North Carolina', N'', N'Union', N'', N'34.9245935', N'-80.7434019')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (293, N'105', N'9308 Steele Creek Road', N'Charlotte', N'North Carolina', N'28273', N'Mecklenburg', N'', N'35.1638380', N'-80.9697304')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (294, N'106', N'4235 Providence Road', N'Charlotte', N'North Carolina', N'28211', N'Mecklenburg', N'', N'35.1558740', N'-80.7958165')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (295, N'107', N'2312 Westchester Dr.', N'High Point', N'North Carolina', N'27262', N'Guilford', N'', N'35.9379016', N'-80.0351917')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (296, N'108', N'10343 Cane Creek Drive', N'Huntersville', N'North Carolina', N'28078', N'Mecklenburg', N'', N'35.3720020', N'-80.8313640')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (297, N'109', N'391 W. Plaza Drive', N'Mooresville', N'North Carolina', N'28117', N'Iredell', N'', N'35.5933781', N'-80.8578222')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (298, N'110', N'8016 N. Tryon Street', N'Charlotte', N'North Carolina', N'28262', N'Mecklenburg', N'', N'35.2972987', N'-80.7541785')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (299, N'111', N'1614 Spring Garden Street', N'Greensboro', N'North Carolina', N'27403', N'Guilford', N'', N'36.0644930', N'-79.8171113')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (300, N'201', N'5200 Piper Station Drive', N'Charlotte', N'North Carolina', N'28277', N'Mecklenburg', N'', N'35.0612640', N'-80.8126906')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (301, N'202', N'10806 Providence Rd.', N'Charlotte', N'North Carolina', N'28277', N'Mecklenburg', N'', N'35.0629060', N'-80.7710181')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (302, N'203', N'7511 Pineville-Matthews Rd.', N'Charlotte', N'North Carolina', N'26226', N'Mecklenburg', N'', N'35.0872115', N'-80.8461255')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (303, N'204', N'8924 Pineville-Matthews Rd.', N'Charlotte', N'North Carolina', N'28226', N'Mecklenburg', N'', N'35.0895495', N'-80.8699712')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (304, N'205', N'240 Carowinds Blvd.', N'Fort Mill', N'South Carolina', N'29708', N'York', N'', N'35.0950229', N'-80.9352964')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (305, N'206', N'12710 South Tryon St.', N'Charlotte', N'North Carolina', N'28273', N'Mecklenburg', N'', N'35.1048356', N'-80.9852997')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (306, N'207', N'8010 South Tryon St', N'Charlotte', N'North Carolina', N'28273', N'Mecklenburg', N'', N'35.1448489', N'-80.9284421')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (307, N'208', N'8315 Steele Creek Rd.', N'Charlotte', N'North Carolina', N'28217', N'Mecklenburg', N'', N'35.1719491', N'-80.9599933')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (308, N'209', N'4401 Park Rd.', N'Charlotte', N'North Carolina', N'28209', N'Mecklenburg', N'', N'35.1717493', N'-80.8505363')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (309, N'210', N'5701 Old Providence Rd.', N'Charlotte', N'North Carolina', N'28078', N'Mecklenburg', N'', N'35.1358576', N'-80.7833317')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (310, N'211', N'4300 North Graham St.', N'Charlotte', N'North Carolina', N'28206', N'Mecklenburg', N'', N'35.2739051', N'-80.8105491')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (311, N'212', N'10023 North Tryon St.', N'Charlotte', N'North Carolina', N'28262', N'Mecklenburg', N'', N'35.3222823', N'-80.7340876')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (312, N'213', N'1901 Pavilion Blvd.', N'Charlotte', N'North Carolina', N'28262', N'Mecklenburg', N'', N'35.3120197', N'-80.7117827')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (313, N'214', N'3301 Monroe Road', N'Charlotte', N'North Carolina', N'28205', N'Mecklenburg', N'', N'35.2031730', N'-80.7998580')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (314, N'215', N'5124 Central Avenue', N'Charlotte', N'North Carolina', N'28205', N'Mecklenburg', N'', N'35.2095670', N'-80.7576636')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (315, N'216', N'9502 Mt. Holly Huntersville Rd.', N'Charlotte', N'North Carolina', N'28226', N'Mecklenburg', N'', N'35.3573919', N'-80.8687391')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (316, N'217', N'1700 Windsor Square Drive', N'Matthews', N'North Carolina', N'28105', N'Mecklenburg', N'', N'35.1332603', N'-80.7112182')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (317, N'218', N'11208 East Independence Blvd.', N'Matthews', N'North Carolina', N'28105', N'Mecklenburg', N'', N'35.1183012', N'-80.6980502')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (318, N'220', N'304 Unionville Indian Trail Rd West', N'Indian Trail', N'North Carolina', N'28079', N'Union', N'', N'35.0758202', N'-80.6512470')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (319, N'301', N'2354 Parchment Blvd (Hwy 160)', N'Fort Mill', N'South Carolina', N'29708', N'York', N'', N'35.0366001', N'-80.9756239')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (320, N'302', N'1531 Sam''s Lane (Hwy 49)', N'Charlotte', N'North Carolina', N'28262', N'Mecklenburg', N'', N'35.3110298', N'-80.7153454')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (321, N'304', N'16814 Caldwell Creek', N'Huntersville', N'North Carolina', N'28078', N'Mecklenburg', N'', N'35.4438012', N'-80.8663691')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (322, N'305', N'4901 Old York Road ', N'Rock Hill', N'South Carolina', N'29730', N'York', N'', N'34.9817670', N'-81.0902366')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (323, N'306', N'10701 Monroe Road', N'Matthews', N'North Carolina', N'28105', N'Mecklenburg', N'', N'35.1282277', N'-80.7309913')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (324, N'307', N'7169 NC Hwy 73', N'Denver', N'North Carolina', N'28037', N'Lincoln', N'', N'35.4500136', N'-81.0026031')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (325, N'308', N'645 W. Fleming Drive ', N'Morganton', N'North Carolina', N'28655', N'Burke', N'', N'35.7286473', N'-81.7005795')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (326, N'309', N'25 Raiford Drive NW', N'Concord', N'North Carolina', N'28027', N'Cabarrus', N'', N'35.3947286', N'-80.6183169')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (327, N'310', N'11124 Old Statesville Road', N'Huntersville', N'North Carolina', N'28078', N'Mecklenburg', N'', N'35.3718956', N'-80.8320266')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (328, N'312', N'10225 Park Road', N'Pineville', N'North Carolina', N'28210', N'Mecklenburg', N'', N'35.0948569', N'-80.8648914')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (329, N'313', N'1817 East Franklin Boulevard', N'Gastonia', N'North Carolina', N'28054', N'Gaston', N'', N'35.2610593', N'-81.1448788')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (330, N'314', N'9248 Lawyers Road', N'Mint Hill', N'North Carolina', N'28227', N'Mecklenburg', N'', N'35.1854010', N'-80.6875944')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (331, N'315', N'"2371 Springs Road', N' NE"', N'Hickory', N'Catawba', N'North Carolina', N'', N'35.7506740', N'-81.2893676')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (332, N'316', N'2920 Rogers Road ', N'Wake Forest', N'North Carolina', N'27587', N'Wake', N'', N'35.9514778', N'-78.5236056')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (333, N'317', N'4603 South Boulevard', N'Charlotte', N'North Carolina', N'28209', N'Mecklenburg', N'', N'35.1783138', N'-80.8753534')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (334, N'318', N'624 River Highway', N'Mooresville', N'North Carolina', N'', N'Iredell', N'', N'35.5962252', N'-80.8737971')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (335, N'319', N'1151 E. Williams Street ', N'Apex', N'North Carolina', N'27502', N'Wake', N'', N'35.7197036', N'-78.8428039')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (336, N'320', N'990 Hodge Road', N'Knightdale', N'North Carolina', N'27545', N'Wake', N'', N'35.7955909', N'-78.5213823')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (337, N'321', N'2224 Dixie Forest Road ', N'Raleigh', N'North Carolina', N'27615', N'Wake', N'', N'35.8627370', N'-78.6015400')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (338, N'322', N'4232 Charlotte Hwy (Lake Wylie)', N'Lake Wylie', N'South Carolina', N'', N'York', N'', N'35.1131446', N'-81.0487702')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (339, N'323', N'1000 Technology Blvd. (Indian Trail)', N'Indian Trail', N'North Carolina', N'28079', N'Union', N'', N'35.0715658', N'-80.6466735')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (340, N'324', N'1025 Aspinal Street (same parcel as SM102)', N'Waxhaw', N'North Carolina', N'', N'Union', N'', N'34.9245935', N'-80.7434019')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (341, N'325', N'6231 N. Tryon Street (Car Wash) (Tract 2)', N'Charlotte', N'North Carolina', N'28213', N'Mecklenburg', N'', N'35.2717995', N'-80.7684841')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (342, N'327', N'920 N. Wesleyan Blvd.', N'Rocky Mount', N'North Carolina', N'', N'Nash', N'', N'35.9693532', N'-77.8139893')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (343, N'329', N'4965 University Parkway', N'Winston Salem', N'North Carolina', N'27101', N'Forsyth', N'', N'36.1571757', N'-80.2779520')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (344, N'330', N'N. Bailey Bridge Road', N'Midlothian', N'Virginia', N'23112', N'Chesterfield', N'', N'37.4268002', N'-77.6117453')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (345, N'331', N'3264 Holland Road', N'Virginia Beach', N'Virginia', N'23453', N'City of Virginia Beach', N'', N'36.7988310', N'-76.0836160')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (346, N'333', N'2811 & 2815 Jones Franklin Road', N'Cary', N'North Carolina', N'27606', N'Wake', N'', N'35.7447495', N'-78.7371480')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (347, N'334', N'8182 Charlotte Highway', N'Indian Land', N'South Carolina', N'29707', N'Lancaster', N'', N'34.9506900', N'-80.8467500')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (348, N'335', N'Bypass Road', N'Williamsburg', N'Virginia', N'', N'York', N'', N'37.2845250', N'-76.7085855')
GO
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (349, N'336', N'1528 & 1532 N. Main Street', N'Fuquay-Varina', N'North Carolina', N'', N'Wake', N'', N'35.5951324', N'-78.7636616')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (350, N'337', N'Raleigh Road', N'Wilson', N'North Carolina', N'', N'Wilson', N'', N'35.7480496', N'-77.9681052')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (351, N'339', N'Carolina Lily', N'Charlotte', N'North Carolina', N'', N'Mecklenburg', N'', N'35.3720398', N'-80.7348007')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (352, N'340', N'2700 South Horner Boulevard', N'Sanford', N'North Carolina', N'', N'Lee', N'', N'35.4562566', N'-79.1417123')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (353, N'341', N'700 Firetower Road', N'Greenville', N'South Carolina', N'28590', N'Pitt', N'', N'35.5557251', N'-77.3721296')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (354, N'342', N'11301 Ironbridge Road', N'Chester', N'Virginia', N'', N'Chesterfield', N'', N'37.3601764', N'-77.4978332')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (355, N'343', N'Cherry Road', N'Rock Hill', N'South Carolina', N'', N'York', N'', N'34.9640982', N'-81.0000382')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (356, N'344', N'Chapel Hill Road & Green Drive', N'Morrisville', N'North Carolina', N'', N'Wake', N'', N'35.8307116', N'-78.8270907')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (357, N'345', N'Albemarle Road', N'Mint Hill', N'North Carolina', N'', N'Mecklenburg', N'', N'35.2129543', N'-80.6613698')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (358, N'504', N'6185 Windward Pkwy', N'Alpharetta', N'Georgia', N'30005', N'Fulton', N'', N'34.0939655', N'-84.2385816')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (359, N'4321', N'2316 Dixie Forest Road', N'Raleigh', N'North Carolina', N'27615', N'Wake', N'', N'35.8629802', N'-78.6004807')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (360, N'4336', N'1520 N. Main Street', N'Fuquay-Varina', N'North Carolina', N'27526', N'Wake', N'', N'35.5944298', N'-78.7643350')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (361, N'4506', N'3325 Old Milton Pkwy', N'Alpharetta', N'Georgia', N'30005', N'Fulton', N'', N'34.0681270', N'-84.2673305')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (362, N'6008', N'3900 Corporation Circle', N'Charlotte', N'North Carolina', N'28216', N'Mecklenburg', N'', N'35.2704010', N'-80.8712680')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (363, N'6011', N'7730 Bruton Smith Boulevard', N'Concord', N'North Carolina', N'28027', N'Cabarrus', N'', N'35.3650627', N'-80.7099218')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (364, N'6065', N'13800 E. Independence Blvd.', N'Indian Trail', N'North Carolina', N'28079', N'Union', N'', N'35.0832643', N'-80.6607251')
INSERT [dbo].[tbl_sams_locations] ([location_id], [sh_asset_id], [location_address], [city], [state], [zipcode], [county], [business_name], [latitude], [longitude]) VALUES (365, N'9141', N'"1784 Heritage Center Drive', N' Ste 204 B&C"', N'Wake Forest', N'Wake', N'North Carolina', N'', N'35.9532444', N'-78.5228214')
SET IDENTITY_INSERT [dbo].[tbl_sams_locations] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_settings] ON 

INSERT [dbo].[tbl_settings] ([settings_id], [smtp_mail_server], [smtp_port_number], [smtp_email_address], [smtp_password], [email_header], [email_body], [real_estate_director]) VALUES (1, N'smtp.office365.com', N'587', N'infosh@samsholdings.com', N'FMf5IY78JnSlolc2', N'', N'', N'Paul Joseph')
SET IDENTITY_INSERT [dbo].[tbl_settings] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_shopping_center] ON 

INSERT [dbo].[tbl_shopping_center] ([shopping_center_id], [shopping_center_name], [state_id], [city_name], [zip_code], [property_status_id], [rent_amount], [property_type_id], [spaces], [spaces_available], [building_size], [asset_status], [shop_description], [created_date], [is_deleted]) VALUES (1, N'Beattis Ford Express Shopping Center', 2, N'Charlotte', N'28216', 0, N'2000.00 per Month ($6.00 Per SF)', 1, N'Single Story Shopping Center Space', N'2275 SF', N'6000SF', 0, N'1121 Beattis Ford Rd, Charlotte NC is located near the Beattis Ford Rd exit of Brookshire blvd.  This 2275 Sq ft space was used as a clothing/fashion store for long time serving the neighborhood in the area.  This space is ideal for hair salon, tax offices, restaurant or any kind of neighborhood business except for a convenience store.', CAST(N'2020-05-19T14:43:30.010' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[tbl_shopping_center] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_shopping_center_clients] ON 

INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (1, 11, N'H & C Books', N'A & B', N'120000', N'52633', N'885.85', N'10629.60', N'', N'', N'', N'', N'', NULL, N'', N'', N'', N'', N'', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (2, 7, N'H & C Books', N'A & B', N'1200', N'52633', N'125621', N'', N'', N'', N'', N'2500', N'', NULL, N'', N'', N'', N'', N'', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (3, 8, N'Mattress Warehouse-Matthews, LLC', N'A&B', N'$ 47,422.24', N'$3953.52', N'$ 885.80', N'$ 10,629.60', N'Adjusted Annually', N'$  4,839.32', N'$58,071.84', N'$ 19.77', N'12/31/2022', NULL, N'$ 48.391.08', N'$ 4,032.59', N'$4918.39', N'$ 20.16', N'2,470 ', N'4/10/19 Extra CAM drafted for 2018 $2810.96', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (4, 5, N'H & C Books', N'A & B', N'1200', N'', N'', N'', N'', N'', N'', N'', N'', NULL, N'', N'', N'', N'', N'', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (5, 1, N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', NULL, N'', N'', N'', N'', N'', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (6, 1, N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', NULL, N'', N'', N'', N'', N'', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (7, 1, N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', NULL, N'', N'', N'', N'', N'', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (8, 1, N'test', N'2', N'5', N'54', N'', N'', N'', N'', N'', N'', N'', NULL, N'', N'', N'', N'', N'', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (9, 8, N'E & F Pizza Perfect, LLC dba Hugry Howies''s Pizza', N'C', N' $22,440.00 ', N' $1,870.00', N' $250.00 ', N' $3,000.00 ', N' Set ', N' $12,120.00 ', N'$25,440.00', N' $18.70', N'3/31/2025', NULL, N'N/A', N'N/A', N'N/A', N'N/A', N'1,248', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (10, 8, N'Charlotte Chopsticks, Inc.', N'D', N'$25,836.00', N'$2,153.00', N' $300.00 ', N' $3,600.00 ', N'Set', N' $2,329.00', N'$29,436.00', N'$ 21.53', N'4/30/2022', NULL, N'$26,616.00 ', N'$2,218.00 ', N'$2,518.00 ', N'$22.18 ', N'1,215', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (11, 8, N'Clean Cut Barber Shop', N'E', N'$27,450.00', N'$2,287.50 ', N' $300.00 ', N' $3,600.00 ', N' Set ', N'$2,587.50', N'$31,050.00', N'$22.50', N'06/30/2025', NULL, N'N/A', N'N/A', N'N/A', N'N/A', N'1220', N'Signed new lease on 06/24/2020', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (12, 8, N'UV Nails', N'F', N'$23,280.00', N'$1,940.00', N' $300.00 ', N' $3,600.00 ', N'Set', N'$2,240.00', N'$26,880.00', N'$19.00', N'03/31/2025', NULL, N'N/A', N'N/A', N'N/A', N'N/A', N'1225', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (14, 8, N'Virtuous Vines', N'G', N' $22,525.20 ', N' $1,877.10 ', N' $325.00 ', N' $3,900.00 ', N'Set', N' $2,202.10 ', N' $26,425.20 ', N' $18.77 ', N'10/15/2025', NULL, N' $22,973.00 ', N' $1,914.63 ', N' $2,239.63 ', N' $19.14 ', N'1220', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (15, 8, N'Bodies Illustrated, Inc.', N'H', N'$25,215.00', N'$2,101.25', N' $300.00 ', N' $3,600.00 ', N'Adjusted Annually', N'$2,401.25', N'$28,815.00', N'$21.01', N'9/30/2023', NULL, N'$25,845.36', N'$2,153.78', N'$2,453.78', N'$21.54', N'1,220', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (16, 8, N'VACANT', N'I', N'N/A', N'N/A', N'N/A', N'N/A', N'N/A', N'N/A', N'N/A', N'N/A', N'N/A', NULL, N'N/A', N'N/A', N'N/A', N'N/A', N'1221', N'', CAST(N'2020-11-02T13:28:17.000' AS DateTime), NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (17, 8, N'High Life Enterprises, Inc.		', N'J&K', N'$52,583.04', N'$4,381.92', N' $643.25 ', N' $7,719.00 ', N'Adjusted Annually', N'$5,025.17', N'$60,302.04', N'$20.44 ', N'8/31/2022', NULL, N'$54,160.56', N'$4,513.38', N'$5,156.63', N'$21.05', N'2573', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (18, 8, N'Lily Huy Do, dba Quickly', N'L', N'$27,867.00', N'$2,322.25', N' $331.75 ', N' $3,981.00 ', N'Set', N'$2,654.00', N'$31,848.00', N'$21.00', N'2/23/2025', NULL, N'N/A ', N'N/A', N'N/A', N'N/A', N'1328', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (19, 8, N'Sam''s Xpress Car Wash, LLC', N'M', N'$26,175.60', N'$2,181.30', N'$330.50', N'$3,966.00', N'Set', N'$2,511.80', N'$30,141.60', N'$19.80', N'9/30/2025', NULL, N'N/A ', N'N/A', N'N/A', N'N/A', N'1,322', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (20, 8, N'VACANT', N'N', N'N/A', N'N/A', N'N', N'N/A', N'N/A', N'N/A', N'N/A', N'N/A', N'N/A', NULL, N'N/A', N'N/A', N'N/A', N'N/A', N'1325', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (21, 8, N'VACANT', N'O', N'N/A', N'N/A', N'N/A', N'N/A', N'N/A', N'N/A', N'N/A', N'N/A', N'N/A', NULL, N'N/A', N'N/A', N'N/A', N'N/A', N'1325', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (22, 8, N'VACANT', N'P', N'N/A', N'N/A', N'N/A', N'N/A', N'Set', N'N/A', N'N/A', N'N/A', N'N/A', NULL, N'N/A', N'N/A', N'N/A', N'N/A', N'1325', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (23, 8, N'VACANT', N'Q', N'N/A', N'N/A', N'N/A', N'N/A', N'Set', N'N/A', N'N/A', N'N/A', N'N/A', NULL, N'N/A', N'N/A', N'N/A', N'N/A', N'1325', N'', NULL, NULL)
INSERT [dbo].[tbl_shopping_center_clients] ([shopping_center_client_id], [c_store_id], [tenant_name], [unit_selected], [annual_rent], [monthly_rent], [cam_monthly], [cam_yearly], [set_or_adjust_automatically], [rent_and_cam_monthly], [rent_and_cam_yearly], [piece_per_square_foot], [lease_expires], [date_rent_changed], [annual_rent_changed_to], [rent_per_month_changed_to], [rent_and_cam_changed_to], [piece_per_square_foot_changed_to], [subspace_square_footage], [notes], [coi_expire], [hide_notification]) VALUES (24, 8, N'Upstage Center of Performing Arts, LLC', N'R&S', N'$47,424.96', N'$3,952.08', N' $999.40 ', N' $11,992.80 ', N'Adjusted Annually', N'$4,951.48', N'$59,417.76', N' $17.50', N'2/28/2023', NULL, N'$48,373.56', N'$4,031.13', N'$5,030.53', N' $17.85', N'2,711', N'', NULL, NULL)
SET IDENTITY_INSERT [dbo].[tbl_shopping_center_clients] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_signedup_customer] ON 

INSERT [dbo].[tbl_signedup_customer] ([custimer_id], [first_name], [last_name], [email_address], [contact_number], [created_date], [subscribe_status]) VALUES (7, N'anand', N's', N'anand@knowminal.com', N'08546997998', CAST(N'2020-10-09T11:36:40.620' AS DateTime), 1)
INSERT [dbo].[tbl_signedup_customer] ([custimer_id], [first_name], [last_name], [email_address], [contact_number], [created_date], [subscribe_status]) VALUES (8, N'anand', N's', N'anand@knowminal.com', N'08546997998', CAST(N'2020-10-09T11:48:51.933' AS DateTime), 1)
INSERT [dbo].[tbl_signedup_customer] ([custimer_id], [first_name], [last_name], [email_address], [contact_number], [created_date], [subscribe_status]) VALUES (9, N'Balan', N'Biny', N'aaaaa@22', N'964464666', CAST(N'2020-10-09T13:13:27.217' AS DateTime), 1)
INSERT [dbo].[tbl_signedup_customer] ([custimer_id], [first_name], [last_name], [email_address], [contact_number], [created_date], [subscribe_status]) VALUES (10, N'Mathew', N'K', N'mk@gmail.com', N'08546997998', CAST(N'2020-10-09T13:18:20.500' AS DateTime), 1)
INSERT [dbo].[tbl_signedup_customer] ([custimer_id], [first_name], [last_name], [email_address], [contact_number], [created_date], [subscribe_status]) VALUES (11, N'anand', N's', N'anand@knowminal.com', N'08546997998', CAST(N'2020-10-09T13:21:21.997' AS DateTime), 1)
INSERT [dbo].[tbl_signedup_customer] ([custimer_id], [first_name], [last_name], [email_address], [contact_number], [created_date], [subscribe_status]) VALUES (12, N's', N's', N's', N'd', CAST(N'2020-10-09T13:23:18.067' AS DateTime), 1)
INSERT [dbo].[tbl_signedup_customer] ([custimer_id], [first_name], [last_name], [email_address], [contact_number], [created_date], [subscribe_status]) VALUES (14, N'Paul', N'Joseph', N'pjoseph@samsholdings.com', N'7049403704', CAST(N'2020-10-09T14:22:52.833' AS DateTime), 1)
INSERT [dbo].[tbl_signedup_customer] ([custimer_id], [first_name], [last_name], [email_address], [contact_number], [created_date], [subscribe_status]) VALUES (15, N'Paul', N'Joseph', N'pjoseph@samsholdings.com', N'7049403704', CAST(N'2020-10-30T14:51:55.247' AS DateTime), 1)
INSERT [dbo].[tbl_signedup_customer] ([custimer_id], [first_name], [last_name], [email_address], [contact_number], [created_date], [subscribe_status]) VALUES (16, N'Paul', N'Joseph', N'kjosephp@gmail.com', N'7049403704', CAST(N'2020-11-16T14:00:34.853' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[tbl_signedup_customer] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_state] ON 

INSERT [dbo].[tbl_state] ([state_id], [state_code], [state_name]) VALUES (1, N'SC', N'South Carolina')
INSERT [dbo].[tbl_state] ([state_id], [state_code], [state_name]) VALUES (2, N'NC', N'North Carolina')
INSERT [dbo].[tbl_state] ([state_id], [state_code], [state_name]) VALUES (3, N'GA', N'Georgia')
INSERT [dbo].[tbl_state] ([state_id], [state_code], [state_name]) VALUES (4, N'VA', N'Virginia')
SET IDENTITY_INSERT [dbo].[tbl_state] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_submitted_property] ON 

INSERT [dbo].[tbl_submitted_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [image_file_name], [pdf_file_name], [is_deleted], [created_by], [client_represented_by_broker], [broker_firm_name], [broker_email_address], [broker_contact_number], [potential_use], [term], [asking_rent], [lease_type], [asset_type_id], [status_changed_date], [is_closed], [new_property_status_id]) VALUES (9, N'', N'Paul', N'joseph', N'keller', N'pjoseph@kw.com', N'', N'charlotte', N'4', N'', N'7048196919', 0, 0, N'1008 James Madison Dr', N'Weddington', 0, N'NC', N'', 0, N'Commercial', N'1 acre', N'$135,000.00', N'', CAST(N'2020-05-18T13:23:14.547' AS DateTime), NULL, NULL, N'', N'', 2, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_submitted_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [image_file_name], [pdf_file_name], [is_deleted], [created_by], [client_represented_by_broker], [broker_firm_name], [broker_email_address], [broker_contact_number], [potential_use], [term], [asking_rent], [lease_type], [asset_type_id], [status_changed_date], [is_closed], [new_property_status_id]) VALUES (13, N'Mr', N'James', N'Camroon', N'keller', N'pjoseph@kw.com', N'1008 James Madison Dr', N'Weddington', N'2', N'28104', N'7048196919', 0, 0, N'1008 James Madison Dr', N'Weddington', 2, N'NC', N'', 0, N'Commercial', N'1 acre', N'$135,000.00', N'', CAST(N'2020-05-22T18:09:10.530' AS DateTime), NULL, NULL, N'', N'', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_submitted_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [image_file_name], [pdf_file_name], [is_deleted], [created_by], [client_represented_by_broker], [broker_firm_name], [broker_email_address], [broker_contact_number], [potential_use], [term], [asking_rent], [lease_type], [asset_type_id], [status_changed_date], [is_closed], [new_property_status_id]) VALUES (14, N'', N'Jake', N'Aniston', N'Related Realty', N'test@test.com', N'#1, test address', N'Virginia Test', N'3', N'123 445', N'4544457', 0, 0, N'7778', N'Floridale', 0, N'Georgia', N'Test ', 0, N'1', N'12', N'90590', N'The values are important for validation puroose', CAST(N'2020-06-01T01:08:36.933' AS DateTime), NULL, NULL, N'', N'', 2, 0, 0, N'', N'', N'', N'', N'', N'', 0, 1, NULL, NULL, 3)
INSERT [dbo].[tbl_submitted_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [image_file_name], [pdf_file_name], [is_deleted], [created_by], [client_represented_by_broker], [broker_firm_name], [broker_email_address], [broker_contact_number], [potential_use], [term], [asking_rent], [lease_type], [asset_type_id], [status_changed_date], [is_closed], [new_property_status_id]) VALUES (16, N'', N'1', N'1', N'', N'1', N'', N'', N'1', N'', N'1', 0, 0, N'', N'', 0, N'', N'', 0, N'', N'', N'', N'', CAST(N'2020-10-23T10:19:46.470' AS DateTime), NULL, NULL, N'', N'', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_submitted_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [image_file_name], [pdf_file_name], [is_deleted], [created_by], [client_represented_by_broker], [broker_firm_name], [broker_email_address], [broker_contact_number], [potential_use], [term], [asking_rent], [lease_type], [asset_type_id], [status_changed_date], [is_closed], [new_property_status_id]) VALUES (17, N'', N'Jake', N'Fischer Test', N'Dew Jones', N'tt@dj.com', N'#1', N'', N'32', N'', N'4322-0886', 0, 0, N'', N'', 0, N'', N'address 1', 0, N'', N'', N'test', N'comments', CAST(N'2020-10-23T13:18:43.680' AS DateTime), NULL, NULL, N'', N'', 2, 0, 1, N'test', N'4458', N'pramodkn05@gmail.com', N'test use', N'10 Years', N'ts', 2, 1, NULL, NULL, 1)
INSERT [dbo].[tbl_submitted_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [image_file_name], [pdf_file_name], [is_deleted], [created_by], [client_represented_by_broker], [broker_firm_name], [broker_email_address], [broker_contact_number], [potential_use], [term], [asking_rent], [lease_type], [asset_type_id], [status_changed_date], [is_closed], [new_property_status_id]) VALUES (18, N'', N'John', N'Samuel', N'new', N'jonh.samuel@gmail', N'Charlotte', N'Charlotte', N'2', N'680652', N'08546997998', 0, 0, N'1', N'Trichur', 0, N'Kerala', N'address 1', 0, N'zone', N'', N'110000', N'te', CAST(N'2020-10-24T01:25:58.777' AS DateTime), NULL, NULL, N'', N'', NULL, 0, 1, N'', N'', N'', N'test use', N'', N'', 0, 2, NULL, NULL, 1)
INSERT [dbo].[tbl_submitted_property] ([site_details_id], [name_prefix], [first_name], [last_name], [company_name], [email_address], [address], [city_name], [state_id], [zip_code], [contact_number], [sams_holding_employee], [market_id], [site_address], [site_city], [site_state_id], [site_county], [site_cross_street_name], [is_property_available], [zoning], [lot_size], [sales_price], [comments], [created_date], [property_type], [image_name], [image_file_name], [pdf_file_name], [is_deleted], [created_by], [client_represented_by_broker], [broker_firm_name], [broker_email_address], [broker_contact_number], [potential_use], [term], [asking_rent], [lease_type], [asset_type_id], [status_changed_date], [is_closed], [new_property_status_id]) VALUES (20, N'Mr', N'Paul', N'Joseph', N'Sams Commercial Properties, LLC', N'pjoseph@samsholdings.com', N'Charlotte', N'Charlotte', N'33', N'28262', N'7049403704', 0, 0, N'10039 University City Blvd, Suite # G, Suite # N', N'', 0, N'', N'', 0, N'', N'', N'', N'', CAST(N'2020-11-16T13:58:48.600' AS DateTime), NULL, NULL, N'', N'', NULL, 0, 0, N'', N'', N'', N'', N'', N'', 0, 0, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[tbl_submitted_property] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_surplus_files] ON 

INSERT [dbo].[tbl_surplus_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (1, 24, N'Sales - 2020', N'v2_4d70.pdf')
INSERT [dbo].[tbl_surplus_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (2, 24, N'Sales - 2020--s', N'v4_17c1.pdf')
INSERT [dbo].[tbl_surplus_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (3, 24, N'Plat', N'b4_c15f.pdf')
INSERT [dbo].[tbl_surplus_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (4, 26, N'Plat', N'b3_4690.pdf')
INSERT [dbo].[tbl_surplus_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (5, 26, N'Sales - 2020', N'b3_361b.pdf')
INSERT [dbo].[tbl_surplus_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (6, 30, N'Survey', N'7249 - CSP 1- 08-26-20_88c7.pdf')
INSERT [dbo].[tbl_surplus_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (7, 32, N'Survey', N'SM411 - ALTA Survey - 03232016_ffce.pdf')
INSERT [dbo].[tbl_surplus_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (8, 43, N'Regional Ariel', N'9810Universityblvd_aef2.jpg')
INSERT [dbo].[tbl_surplus_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (9, 44, N'Regional Ariel', N'3400Plaza_f0f9.jpg')
INSERT [dbo].[tbl_surplus_files] ([file_id], [property_id], [file_type], [file_name]) VALUES (10, 45, N'101WWoodlawn', N'101woodlawn_0541.jpg')
SET IDENTITY_INSERT [dbo].[tbl_surplus_files] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_todo] ON 

INSERT [dbo].[tbl_todo] ([todo_id], [property_id], [todo_text], [property_type], [created_date]) VALUES (1, 34, N'Meeting scheduled on 20/10/2020', 1, CAST(N'2020-05-18T19:04:58.463' AS DateTime))
SET IDENTITY_INSERT [dbo].[tbl_todo] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_user] ON 

INSERT [dbo].[tbl_user] ([user_id], [first_name], [last_name], [user_name], [password], [role_id], [password_reset_key], [email_address]) VALUES (1, N'Super', N'Admin', N'sams', N'a', 4, N'0e2e22e3-ab63-4f6f-b5e8-6bbb213019d7', N'infosh@samsholdings.com')
INSERT [dbo].[tbl_user] ([user_id], [first_name], [last_name], [user_name], [password], [role_id], [password_reset_key], [email_address]) VALUES (17, N'John', N'B', N'arun', N'123456Aa', 8, N'', N'arun@knowminal.com')
INSERT [dbo].[tbl_user] ([user_id], [first_name], [last_name], [user_name], [password], [role_id], [password_reset_key], [email_address]) VALUES (18, N'Mathew', N'Dean', N'apboss', N'123456Aa', 6, N'97297116-1474-4c98-9c80-0d90e1dc995a', N'apboss@gmail.com')
INSERT [dbo].[tbl_user] ([user_id], [first_name], [last_name], [user_name], [password], [role_id], [password_reset_key], [email_address]) VALUES (19, N'anand', N's', N'anand', N'Gautham1', 6, N'62bc6391-4746-4fb0-b937-c119e420ded6', N'anand@knowminal.com')
SET IDENTITY_INSERT [dbo].[tbl_user] OFF
GO
/****** Object:  StoredProcedure [dbo].[CloseDiligenceAcquisition]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[CloseDiligenceAcquisition]
	@diligence_acquisition_id int
as
begin
	update tbl_diligence_acquisition set acquisition_status = 1
	where diligence_acquisition_id = @diligence_acquisition_id
end
GO
/****** Object:  StoredProcedure [dbo].[CloseDiligenceDisposition]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[CloseDiligenceDisposition]
	@diligence_dispositions_id int
as
begin
	update tbl_diligence_dispositions set disposition_status = 1
	where diligence_dispositions_id = @diligence_dispositions_id
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteAssetType]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[DeleteAssetType]
	@asset_type_id int
as
begin
	delete from dbo.tbl_asset_type
	where asset_type_id = @asset_type_id
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteCStore]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[DeleteCStore]
	@c_store_id int
as
begin
	update dbo.tbl_c_store 
	set is_deleted = 1
	where c_store_id = @c_store_id
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteCStoreFile]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[DeleteCStoreFile]
	@file_id int
as
begin
	delete from tbl_c_store_files
	where [file_id] = @file_id
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteNetLeaseFile]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[DeleteNetLeaseFile]
	@file_id int
as
begin
	delete from tbl_net_lease_files
	where [file_id] = @file_id
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteNetleaseProperty]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[DeleteNetleaseProperty]
	@net_lease_property_id int
as
begin
	update dbo.tbl_net_lease_property 
	set is_deleted = 1
	where net_lease_property_id = @net_lease_property_id
end
GO
/****** Object:  StoredProcedure [dbo].[DeletePeriod]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[DeletePeriod]
	@period_id int
as
begin
	delete from dbo.tbl_period where period_id = @period_id
end
GO
/****** Object:  StoredProcedure [dbo].[DeletePropertyType]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[DeletePropertyType]
	@property_type_id int 
as
begin

	delete from tbl_property_type 
	where property_type_id = @property_type_id		
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteRole]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[DeleteRole]
	@role_id int
as
begin
	delete from tbl_role where role_id=@role_id 
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteSurplusFile]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[DeleteSurplusFile]
	@file_id int
as
begin
	delete from tbl_surplus_files
	where [file_id] = @file_id
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteSurplusProperties]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[DeleteSurplusProperties]
	@site_details_id int
as
begin
	update dbo.tbl_property
	set is_deleted=1 
	where site_details_id = @site_details_id
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteUploadedImage]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[DeleteUploadedImage]
	@image_id int
as
begin
	delete from tbl_property_images
	where image_id = @image_id
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteUser]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[DeleteUser]
	@user_id int
as
begin
	delete from [tbl_user]
	where user_id = @user_id
end
GO
/****** Object:  StoredProcedure [dbo].[GetAssetType]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetAssetType]
as
begin
	SELECT asset_type_id
		,asset_type_name
	FROM tbl_asset_type
end
GO
/****** Object:  StoredProcedure [dbo].[GetAssetTypeById]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetAssetTypeById]
	@asset_type_id int
as
begin
	SELECT asset_type_id
		,asset_type_name
	FROM tbl_asset_type
	where asset_type_id = @asset_type_id
end
GO
/****** Object:  StoredProcedure [dbo].[GetClosedPropertyListByCategory]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetClosedPropertyListByCategory]
	@property_type int
as
begin
	SELECT [site_details_id]
      ,[name_prefix]
      ,[first_name]
      ,[last_name]
      ,[company_name]
      ,[email_address]
      ,[address]
      ,[city_name]
      ,p.state_id
      ,customerState.state_name
      ,[zip_code]
      ,[contact_number]
      ,[sams_holding_employee]
      ,[market_id]
      ,[site_address]
      ,[site_city]
      ,[site_state_id]
      ,mState.state_name as site_state_name
      ,[site_county]
      ,[site_cross_street_name]
      ,[is_property_available]
      ,[zoning]
      ,[lot_size]
      ,[sales_price]
      ,[comments]
      ,[created_date]
      ,[property_type]
      ,[image_name]
      ,[image_file_name]
      ,[pdf_file_name]
      ,[is_deleted]
  FROM [tbl_submitted_property] p
  left join tbl_state customerState
  on p.site_state_id = customerState.state_id
  left join tbl_state mState 
  on p.site_state_id = mState.state_id
  where is_deleted = 1
end
GO
/****** Object:  StoredProcedure [dbo].[GetCStoreById]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetCStoreById]
	@c_store_id int
as
begin
	SELECT cs.c_store_id
	  ,cs.property_header
      ,cs.state_id
      ,cs.city
      ,cs.zipcode
      ,cs.county
      ,cs.asset_id
      ,cs.property_type_id
      ,cs.property_description
      ,cs.asking_price
      ,cs.asset_type_id
      ,cs.land_size
      ,cs.building_area
      ,cs.property_taxes
      ,cs.year_built
      ,cs.known_environmental_conditions
      ,cs.emv_copliance
      ,cs.hours_of_operation
      ,cs.created_date
      ,cs.environent_nda_pdf_filename
      ,cs.c_store_address
      ,cs.diligence_type
      ,pt.property_type_name
      ,ast.asset_type_name
      ,s.state_name
      ,cs.property_latitude
	  ,cs.property_longitude
	FROM tbl_c_store cs
	left join tbl_state s
	on cs.state_id = s.state_id
	left join tbl_property_type pt
	on cs.property_type_id = pt.property_type_id
	left join tbl_asset_type ast
	on cs.asset_type_id = ast.asset_type_id
	
	WHERE c_store_id = @c_store_id
end
GO
/****** Object:  StoredProcedure [dbo].[GetCstoreComplianceFiles]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetCstoreComplianceFiles]
	@property_id int
as
begin
	select [file_id],
			[property_id],
			[file_type],
			[file_name]
	from tbl_c_store_files
	where [property_id] = @property_id
end
GO
/****** Object:  StoredProcedure [dbo].[GetCStoreList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCStoreList]
	@asset_status int
as
begin
	SELECT cs.c_store_id
	  ,cs.property_header
      ,cs.state_id
      ,cs.city
      ,cs.zipcode
      ,cs.county
      ,cs.asset_id
      ,cs.property_type_id
      ,cs.property_description
      ,cs.asking_price
      ,cs.asset_type_id
      ,cs.land_size
      ,cs.building_area
      ,cs.property_taxes
      ,cs.year_built
      ,cs.known_environmental_conditions
      ,cs.emv_copliance
      ,cs.hours_of_operation
      ,cs.created_date
      ,cs.environent_nda_pdf_filename
      ,cs.c_store_address
      ,pt.property_type_name
      ,ast.asset_type_name
      ,s.state_name
	FROM tbl_c_store cs
	left join tbl_state s
	on cs.state_id = s.state_id
	left join tbl_property_type pt
	on cs.property_type_id = pt.property_type_id
	left join tbl_asset_type ast
	on cs.asset_type_id = ast.asset_type_id
	where (is_deleted is null OR is_deleted = 0)
	and asset_status = @asset_status
end
GO
/****** Object:  StoredProcedure [dbo].[GetCStoreListByState]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCStoreListByState]
	@stateId int
as
begin
	if(@stateId = 0)
	begin
		SELECT cs.c_store_id
		  ,cs.property_header
		  ,cs.state_id
		  ,cs.city
		  ,cs.zipcode
		  ,cs.county
		  ,cs.asset_id
		  ,cs.property_type_id
		  ,cs.property_description
		  ,cs.asking_price
		  ,cs.asset_type_id
		  ,cs.land_size
		  ,cs.building_area
		  ,cs.property_taxes
		  ,cs.year_built
		  ,cs.known_environmental_conditions
		  ,cs.emv_copliance
		  ,cs.hours_of_operation
		  ,cs.created_date
		  ,cs.environent_nda_pdf_filename
		  ,cs.c_store_address
		  ,pt.property_type_name
		  ,cs.c_store_address
		  ,ast.asset_type_name
		  ,s.state_name
		FROM tbl_c_store cs
		left join tbl_state s
		on cs.state_id = s.state_id
		left join tbl_property_type pt
		on cs.property_type_id = pt.property_type_id
		left join tbl_asset_type ast
		on cs.asset_type_id = ast.asset_type_id
	end
	else 
	begin
		SELECT cs.c_store_id
		  ,cs.property_header
		  ,cs.state_id
		  ,cs.city
		  ,cs.zipcode
		  ,cs.county
		  ,cs.asset_id
		  ,cs.property_type_id
		  ,cs.property_description
		  ,cs.asking_price
		  ,cs.asset_type_id
		  ,cs.land_size
		  ,cs.building_area
		  ,cs.property_taxes
		  ,cs.year_built
		  ,cs.known_environmental_conditions
		  ,cs.emv_copliance
		  ,cs.hours_of_operation
		  ,cs.created_date
		  ,cs.environent_nda_pdf_filename
		  ,cs.c_store_address
		  ,pt.property_type_name
		  ,cs.c_store_address
		  ,ast.asset_type_name
		  ,s.state_name
		FROM tbl_c_store cs
		left join tbl_state s
		on cs.state_id = s.state_id
		left join tbl_property_type pt
		on cs.property_type_id = pt.property_type_id
		left join tbl_asset_type ast
		on cs.asset_type_id = ast.asset_type_id
		
		where cs.state_id = @stateId
	end
	
end
GO
/****** Object:  StoredProcedure [dbo].[GetCStoreMonthlyData]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetCStoreMonthlyData] 
	@asset_status int
as
begin
	select 
		m.month_id,
		m.month_name,
		Year(created_date) as createdYear,
		count(c_store_id) as totalRecords
	from tbl_month m
	left join tbl_c_store p
	on Month(created_date) = m.month_id 
	AND year(created_date) = year(getdate())
	AND p.asset_status = @asset_status 

	group by m.month_id, Year(created_date), m.month_name
	order by m.month_id, Year(created_date)

end
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerById]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetCustomerById]
	@customer_id varchar(500)
as
begin
	
	SELECT [customer_id]
      ,[first_name]
      ,[last_name]
      ,[email_address]
      ,[contact_number]
      ,[signed_nda_file]
      ,[user_name]
      ,[customer_password]
      ,[created_date]
      ,[last_login_date]
      ,[customer_sign]
      ,[company_name]
      ,[given_title]
      ,[address]
      ,[zipcode]
      ,[city]
      ,c.state_id
      ,s.state_name
      ,cell_number
  FROM [tbl_customer] c
	LEFT join tbl_state s
	on s.state_id = c.state_id

	
	
  WHERE [customer_id] = @customer_id
end
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerByUserName]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetCustomerByUserName]
	@user_name varchar(500)
as
begin
	SELECT [customer_id]
      ,[first_name]
      ,[last_name]
      ,[email_address]
      ,[contact_number]
      ,[signed_nda_file]
      ,[user_name]
      ,[customer_password]
      ,[created_date]
      ,[last_login_date]
      ,[customer_sign]
  FROM [tbl_customer]
  WHERE [user_name] = @user_name
end
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerMessageList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetCustomerMessageList]
as
begin
	SELECT [contact_us_id]
		  ,[custumer_name]
		  ,[customer_email]
		  ,[customer_subject]
		  ,[customer_message]
		  ,[created_date]
	FROM [tbl_customer_message]
end
GO
/****** Object:  StoredProcedure [dbo].[GetDiligenceAcquisition]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetDiligenceAcquisition]
	@property_id int,
	@property_type int
as
begin
	SELECT [diligence_acquisition_id]
		  ,[property_id]
		  ,[property_type]
		  ,[purchase_price]
		  ,[earnest_money]
		  ,[exchange_1031]
		  ,[dead_line_1031]
		  ,[sellers]
		  ,[escrow_agent]
		  ,[sub_division]
		  ,[real_estate_agent]
		  ,[created_date]
		  ,[acquisition_status]
	FROM [tbl_diligence_acquisition]
	where property_id = @property_id
	AND property_type = @property_type
end
GO
/****** Object:  StoredProcedure [dbo].[GetDiligenceDispositions]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetDiligenceDispositions]
	@property_id int,
	@property_type int
as
begin
	SELECT [diligence_dispositions_id]
		  ,[property_id]
		  ,[property_type]
		  ,[sale_price]
		  ,[earnest_money]
		  ,[buyers]
		  ,[escrow_agent]
		  ,[buyers_attorney]
		  ,[options_to_extend]
		  ,[commissions]
		  ,[created_date]
		  ,[disposition_status]
	FROM [tbl_diligence_dispositions]
	where property_id = @property_id and property_type = @property_type
end
GO
/****** Object:  StoredProcedure [dbo].[GetDiligenceLease]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetDiligenceLease]
	@property_id int,
	@property_type int
as
begin
	SELECT [diligence_lease_id]
		  ,[property_id]
		  ,[property_type]
		  ,[tenant_name]
		  ,[created_date]
	FROM [tbl_diligence_lease]
	WHERE property_id = @property_id
	AND property_type = @property_type
end
GO
/****** Object:  StoredProcedure [dbo].[GetInProgressPropertyListByCategory]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetInProgressPropertyListByCategory]
	@property_type int
as
begin
	SELECT [site_details_id]
      ,[name_prefix]
      ,[first_name]
      ,[last_name]
      ,[company_name]
      ,[email_address]
      ,[address]
      ,[city_name]
      ,p.state_id
      ,customerState.state_name
      ,[zip_code]
      ,[contact_number]
      ,[sams_holding_employee]
      ,[market_id]
      ,[site_address]
      ,[site_city]
      ,[site_state_id]
      ,mState.state_name as site_state_name
      ,[site_county]
      ,[site_cross_street_name]
      ,[is_property_available]
      ,[zoning]
      ,[lot_size]
      ,[sales_price]
      ,[comments]
      ,[created_date]
      ,[property_type]
      ,[image_name]
      ,[image_file_name]
      ,[pdf_file_name]
      ,[is_deleted]
  FROM [tbl_submitted_property] p
  left join tbl_state customerState
  on p.site_state_id = customerState.state_id
  left join tbl_state mState 
  on p.site_state_id = mState.state_id
  where is_deleted = 2
end
GO
/****** Object:  StoredProcedure [dbo].[GetMarketList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetMarketList]
as
begin
	select market_id, market_name from tbl_market order by market_name
end
GO
/****** Object:  StoredProcedure [dbo].[GetNetLeaseFiles]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetNetLeaseFiles]
	@property_id int
as
begin
	select [file_id],
			[property_id],
			[file_type],
			[file_name]
	from tbl_net_lease_files
	where [property_id] = @property_id
end
GO
/****** Object:  StoredProcedure [dbo].[GetNetLeaseMonthlyData]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetNetLeaseMonthlyData]
	@asset_status int
as
begin
	select 
		m.month_id,
		m.month_name,
		Year(created_date) as createdYear,
		count(net_lease_property_id) as totalRecords
	from tbl_month m
	left join tbl_net_lease_property p
	on Month(created_date) = m.month_id 
	AND year(created_date) = year(getdate())
	AND p.asset_status = @asset_status 

	group by m.month_id, Year(created_date), m.month_name
	order by m.month_id, Year(created_date)

end
GO
/****** Object:  StoredProcedure [dbo].[GetNetleasePropertyById]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetNetleasePropertyById]
	@net_lease_property_id int
as
begin
	SELECT p.net_lease_property_id
      ,p.asset_id
      ,p.asset_name
      ,p.state_id
      ,p.city
      ,p.property_price
      ,p.cap_rate
      ,p.term
      ,p.detail_pdf
      ,p.created_date
      ,s.state_name
      ,p.asset_type_id
      ,p.is_shopping_center
      ,p.property_address
	  ,p.property_zipcode
	  ,p.diligence_type
      ,ast.asset_type_name
      
      ,p.property_latitude
	  ,p.property_longitude
      
	FROM tbl_net_lease_property p
	left join tbl_state s
	on p.state_id = s.state_id
	left join tbl_asset_type ast
	on p.asset_type_id = ast.asset_type_id
	
	WHERE p.net_lease_property_id = @net_lease_property_id
end
GO
/****** Object:  StoredProcedure [dbo].[GetNetleasePropertyList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetNetleasePropertyList]
	@asset_status int
as
begin
	SELECT p.net_lease_property_id
      ,p.asset_id
      ,p.asset_name
      ,p.state_id
      ,p.city
      ,p.property_price
      ,p.cap_rate
      ,p.term
      ,p.detail_pdf
      ,p.created_date
      ,s.state_name
      ,p.asset_type_id
      ,p.is_shopping_center
      ,p.property_address
      ,p.property_zipcode
      ,ast.asset_type_name
	FROM tbl_net_lease_property p
	left join tbl_state s
	on p.state_id = s.state_id
	left join tbl_asset_type ast
	on p.asset_type_id = ast.asset_type_id
	
	where (is_deleted is null OR is_deleted = 0)
	and asset_status = @asset_status 
	
end
GO
/****** Object:  StoredProcedure [dbo].[GetNetleasePropertyListByState]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetNetleasePropertyListByState]
	@stateId int
as
begin
	if(@stateId = 0)
	begin
		SELECT p.net_lease_property_id
		  ,p.asset_id
		  ,p.asset_name
		  ,p.state_id
		  ,p.city
		  ,p.property_price
		  ,p.cap_rate
		  ,p.term
		  ,p.detail_pdf
		  ,p.created_date
		  ,s.state_name
		  ,p.asset_type_id
		  ,p.property_address
		  ,p.property_zipcode
		  ,ast.asset_type_name
		FROM tbl_net_lease_property p
		left join tbl_state s
		on p.state_id = s.state_id
		left join tbl_asset_type ast
		on p.asset_type_id = ast.asset_type_id
	end
	else
	begin
		SELECT p.net_lease_property_id
		  ,p.asset_id
		  ,p.asset_name
		  ,p.state_id
		  ,p.city
		  ,p.property_price
		  ,p.cap_rate
		  ,p.term
		  ,p.detail_pdf
		  ,p.created_date
		  ,s.state_name
		  ,p.asset_type_id
		  ,p.property_address
		  ,p.property_zipcode
		  ,ast.asset_type_name
		FROM tbl_net_lease_property p
		left join tbl_state s
		on p.state_id = s.state_id
		left join tbl_asset_type ast
		on p.asset_type_id = ast.asset_type_id
		where p.state_id = @stateId
	end
	

end
GO
/****** Object:  StoredProcedure [dbo].[GetNetleaseShoppingCenterList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetNetleaseShoppingCenterList]
	@asset_status int
as
begin
	SELECT p.net_lease_property_id
      ,p.asset_id
      ,p.asset_name
      ,p.state_id
      ,p.city
      ,p.property_price
      ,p.cap_rate
      ,p.term
      ,p.detail_pdf
      ,p.created_date
      ,s.state_name
      ,p.asset_type_id
      ,p.is_shopping_center
      ,p.property_address
	  ,p.property_zipcode
      ,ast.asset_type_name
	FROM tbl_net_lease_property p
	left join tbl_state s
	on p.state_id = s.state_id
	left join tbl_asset_type ast
	on p.asset_type_id = ast.asset_type_id
	
	where (is_deleted is null OR is_deleted = 0)
	and asset_status = @asset_status 
	AND is_shopping_center = 1
end
GO
/****** Object:  StoredProcedure [dbo].[GetNetLeaseShoppingCenterListByState]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetNetLeaseShoppingCenterListByState]
	@stateId int
as
begin
	if(@stateId = 0)
	begin
		SELECT p.net_lease_property_id
		  ,p.asset_id
		  ,p.asset_name
		  ,p.state_id
		  ,p.city
		  ,p.property_price
		  ,p.cap_rate
		  ,p.term
		  ,p.detail_pdf
		  ,p.created_date
		  ,s.state_name
		  ,p.asset_type_id
		  ,p.property_address
		  ,p.property_zipcode
		  ,ast.asset_type_name
		FROM tbl_net_lease_property p
		left join tbl_state s
		on p.state_id = s.state_id
		left join tbl_asset_type ast
		on p.asset_type_id = ast.asset_type_id
		where is_shopping_center = 1 AND (is_deleted is null OR is_deleted = 0)
	end
	else
	begin
		SELECT p.net_lease_property_id
		  ,p.asset_id
		  ,p.asset_name
		  ,p.state_id
		  ,p.city
		  ,p.property_price
		  ,p.cap_rate
		  ,p.term
		  ,p.detail_pdf
		  ,p.created_date
		  ,s.state_name
		  ,p.asset_type_id
		  ,p.property_address
		  ,p.property_zipcode
		  ,ast.asset_type_name
		FROM tbl_net_lease_property p
		left join tbl_state s
		on p.state_id = s.state_id
		left join tbl_asset_type ast
		on p.asset_type_id = ast.asset_type_id
		where p.state_id = @stateId AND is_shopping_center = 1 AND (is_deleted is null OR is_deleted = 0)
	end
	

end
GO
/****** Object:  StoredProcedure [dbo].[GetNewProertiesSummary]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetNewProertiesSummary]
as
begin
	select count(1) as TotalData, 'TotalNewProperties' as pType from
	dbo.tbl_submitted_property
	where is_deleted = 0 or is_deleted is null

	union

	select count(1) as TotalData, 'TotalInProgressProperties' as pType from
	dbo.tbl_submitted_property
	where is_deleted = 2

	union

	select count(1) as TotalData, 'TotalDeletedProperties' as pType from
	dbo.tbl_submitted_property
	where is_deleted = 1

end
GO
/****** Object:  StoredProcedure [dbo].[GetPeriodList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetPeriodList]
	@property_id int,
	@property_type int
as
begin
	SELECT period_id
      ,property_id
      ,property_type
      ,period_master
      ,start_date
      ,end_date
      ,period_notes
	FROM tbl_period
	WHERE property_id = @property_id 
	AND property_type = @property_type
end
GO
/****** Object:  StoredProcedure [dbo].[GetPropertyDashboard]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetPropertyDashboard]
as
begin
	select count(1) as totalCount, 'surplus' as pType
	from  dbo.tbl_property where (is_deleted is null OR is_deleted = 0)
	union
	select count(1) as totalCount, 'net_lease' as pType
	from  dbo.tbl_net_lease_property where (is_deleted is null OR is_deleted = 0)
	union
	select count(1) as totalCount, 'c_store' as pType
	from  dbo.tbl_c_store where (is_deleted is null OR is_deleted = 0)
	union
	select count(1) as totalCount, 'from_web' as pType
	from tbl_submitted_property 
end
GO
/****** Object:  StoredProcedure [dbo].[GetPropertyImageList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetPropertyImageList]
	@property_id int,
	@property_type int
as
begin
	SELECT [image_id]
		  ,[property_id]
		  ,[image_name]
	FROM [tbl_property_images]
	where property_id = @property_id AND property_type = @property_type
end
GO
/****** Object:  StoredProcedure [dbo].[GetPropertyItemById]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetPropertyItemById]
	@site_details_id int
as
begin
	SELECT p.site_details_id
      ,p.name_prefix
      ,p.first_name
      ,p.last_name
      ,p.company_name
      ,p.email_address
      ,p.address
      ,p.city_name
      ,p.state_id
      ,customerState.state_name
      ,p.zip_code
      ,p.contact_number
      ,p.sams_holding_employee
      ,p.market_id
      ,p.property_header
      ,p.site_address
      ,p.site_city
      ,p.site_state_id
      ,mState.state_name as site_state_name
      ,p.site_county
      ,p.site_cross_street_name
      ,p.is_property_available
      ,p.zoning
      ,p.lot_size
      ,p.sales_price
      ,p.comments
      ,p.created_date
      ,p.property_type
      ,p.image_name
      ,p.asset_type_id
      ,p.asset_status
      ,p.diligence_type
      ,ast.asset_type_name
      ,p.property_latitude
	  ,p.property_longitude
  FROM tbl_property p
  left join tbl_state customerState
  on p.state_id = customerState.state_id
  left join tbl_state mState 
  on p.site_state_id = mState.state_id
  
  left join tbl_asset_type ast
  on p.asset_type_id = ast.asset_type_id
  
  WHERE p.site_details_id = @site_details_id

end
GO
/****** Object:  StoredProcedure [dbo].[GetPropertyList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetPropertyList]
as
begin
		SELECT p.site_details_id
      ,p.name_prefix
      ,p.first_name
      ,p.last_name
      ,p.company_name
      ,p.email_address
      ,p.address
      ,p.city_name
      ,p.state_id
      ,customerState.state_name
      ,p.zip_code
      ,p.contact_number
      ,p.sams_holding_employee
      ,p.market_id
      ,p.site_address
      ,p.site_city
      ,p.site_state_id
      ,mState.state_name as site_state_name
      ,p.site_county
      ,p.site_cross_street_name
      ,p.is_property_available
      ,p.zoning
      ,p.lot_size
      ,p.sales_price
      ,p.comments
      ,p.created_date
      ,p.property_type
      ,p.image_name
      ,p.property_header
  FROM tbl_property p
  left join tbl_state customerState
  on p.state_id = customerState.state_id
  left join tbl_state mState 
  on p.site_state_id = mState.state_id

end
GO
/****** Object:  StoredProcedure [dbo].[GetPropertyListByCategory]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetPropertyListByCategory]
	@asset_status int
as
begin
	SELECT [site_details_id]
      ,[name_prefix]
      ,[first_name]
      ,[last_name]
      ,[company_name]
      ,[email_address]
      ,[address]
      ,[city_name]
      ,p.state_id
      ,customerState.state_name
      ,[zip_code]
      ,[contact_number]
      ,[sams_holding_employee]
      ,[market_id]
      ,[property_header]
      ,[site_address]
      ,[site_city]
      ,[site_state_id]
      ,mState.state_name as site_state_name
      ,[site_county]
      ,[site_cross_street_name]
      ,[is_property_available]
      ,[zoning]
      ,[lot_size]
      ,[sales_price]
      ,[comments]
      ,[created_date]
      ,[property_type]
      ,[image_name]
      ,[asset_status]
      ,p.asset_type_id
      ,ast.asset_type_name
  FROM [tbl_property] p
  left join tbl_state customerState
  on p.state_id = customerState.state_id
  left join tbl_state mState 
  on p.site_state_id = mState.state_id
  
  left join tbl_asset_type ast
  on p.asset_type_id = ast.asset_type_id
	
  where (is_deleted is null OR is_deleted = 0) 
  AND asset_status=@asset_status
  

end
GO
/****** Object:  StoredProcedure [dbo].[GetPropertyListByState]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetPropertyListByState]
	@stateId int
as
begin
	if(@stateId = 0)
	begin
		SELECT [site_details_id]
		  ,[name_prefix]
		  ,[first_name]
		  ,[last_name]
		  ,[company_name]
		  ,[email_address]
		  ,[address]
		  ,[city_name]
		  ,p.state_id
		  ,customerState.state_name
		  ,[zip_code]
		  ,[contact_number]
		  ,[sams_holding_employee]
		  ,[market_id]
		  ,[property_header]
		  ,[site_address]
		  ,[site_city]
		  ,[site_state_id]
		  ,mState.state_name as site_state_name
		  ,[site_county]
		  ,[site_cross_street_name]
		  ,[is_property_available]
		  ,[zoning]
		  ,[lot_size]
		  ,[sales_price]
		  ,[comments]
		  ,[created_date]
		  ,[property_type]
		  ,[image_name]
		  ,p.asset_type_id
		  ,ast.asset_type_name
	  FROM [tbl_property] p
	  left join tbl_state customerState
	  on p.state_id = customerState.state_id
	  left join tbl_state mState 
	  on p.site_state_id = mState.state_id
	  
	  left join tbl_asset_type ast
	  on p.asset_type_id = ast.asset_type_id
	end
	else
	begin
		SELECT [site_details_id]
		  ,[name_prefix]
		  ,[first_name]
		  ,[last_name]
		  ,[company_name]
		  ,[email_address]
		  ,[address]
		  ,[city_name]
		  ,p.state_id
		  ,customerState.state_name
		  ,[zip_code]
		  ,[contact_number]
		  ,[sams_holding_employee]
		  ,[market_id]
		  ,[property_header]
		  ,[site_address]
		  ,[site_city]
		  ,[site_state_id]
		  ,mState.state_name as site_state_name
		  ,[site_county]
		  ,[site_cross_street_name]
		  ,[is_property_available]
		  ,[zoning]
		  ,[lot_size]
		  ,[sales_price]
		  ,[comments]
		  ,[created_date]
		  ,[property_type]
		  ,[image_name]
		  ,p.asset_type_id
		  ,ast.asset_type_name
	  FROM [tbl_property] p
	  left join tbl_state customerState
	  on p.state_id = customerState.state_id
	  left join tbl_state mState 
	  on p.site_state_id = mState.state_id
	  
	  left join tbl_asset_type ast
	  on p.asset_type_id = ast.asset_type_id
	  where p.site_state_id = @stateId
	end
	
	
  

end
GO
/****** Object:  StoredProcedure [dbo].[GetPropertyType]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetPropertyType]
as
begin
	SELECT property_type_id
		,property_type_name
	FROM tbl_property_type
end
GO
/****** Object:  StoredProcedure [dbo].[GetPropertyTypeById]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetPropertyTypeById]
	@property_type_id int
as
begin
	SELECT property_type_id
      ,property_type_name
	FROM tbl_property_type
	where property_type_id = @property_type_id
end
GO
/****** Object:  StoredProcedure [dbo].[GetPropertyTypeList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetPropertyTypeList]
as
begin
	SELECT property_type_id
      ,property_type_name
	FROM tbl_property_type
end
GO
/****** Object:  StoredProcedure [dbo].[GetRoleById]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetRoleById]
	@role_id int
as
begin
	select role_id, role_name from tbl_role
	where role_id = @role_id
end
GO
/****** Object:  StoredProcedure [dbo].[GetRolePermission]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetRolePermission]
	@role_id int
as
begin
	SELECT mm.module_id
		  ,mm.module_name
		  ,rp.role_permission_id
		  ,rp.role_id
		  ,rp.module_id
		  ,rp.can_read
		  ,rp.can_edit
		  ,rp.can_create
		  ,rp.can_delate
	FROM tbl_module_master mm
	left join tbl_role_permission rp
	on mm.module_id = rp.module_id and role_id = @role_id
	
end
GO
/****** Object:  StoredProcedure [dbo].[GetRoles]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetRoles]
as
begin
	select role_id, role_name from tbl_role
end
GO
/****** Object:  StoredProcedure [dbo].[GetSamsSettings]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetSamsSettings]
as
begin
	SELECT [settings_id]
      ,[smtp_mail_server]
      ,[smtp_port_number]
      ,[smtp_email_address]
      ,[smtp_password]
      ,[email_header]
      ,[email_body]
  FROM [tbl_settings]
end
GO
/****** Object:  StoredProcedure [dbo].[GetShoppingCenterById]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetShoppingCenterById]
	@shopping_center_id int
as
begin
	SELECT sc.shopping_center_id
      ,sc.shopping_center_name
      ,sc.state_id
      ,sc.city_name
      ,sc.zip_code
      ,sc.property_status_id
      ,sc.rent_amount
      ,sc.property_type_id
      ,sc.spaces
      ,sc.spaces_available
      ,sc.building_size
      ,sc.asset_status
      ,sc.created_date
      ,s.state_name
      ,sc.shop_description
      ,pt.asset_type_name
  FROM tbl_shopping_center sc
	left join tbl_state s
	on sc.state_id = s.state_id
	left join tbl_asset_type pt
	on sc.property_type_id = pt.asset_type_id
  where shopping_center_id = @shopping_center_id
end
GO
/****** Object:  StoredProcedure [dbo].[GetShoppingCenterList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetShoppingCenterList]
	@asset_status int
as
begin
	SELECT sc.shopping_center_id
      ,sc.shopping_center_name
      ,sc.state_id
      ,sc.city_name
      ,sc.zip_code
      ,sc.property_status_id
      ,sc.rent_amount
      ,sc.property_type_id
      ,sc.spaces
      ,sc.spaces_available
      ,sc.building_size
      ,sc.asset_status
      ,sc.created_date
      ,sc.shop_description
      ,s.state_name
      ,pt.asset_type_name
  FROM tbl_shopping_center sc
	left join tbl_state s
	on sc.state_id = s.state_id
	left join tbl_asset_type pt
	on sc.property_type_id = pt.asset_type_id
  where (is_deleted is null OR is_deleted = 0)
	AND asset_status = @asset_status
end
GO
/****** Object:  StoredProcedure [dbo].[GetShoppingCenterListByState]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetShoppingCenterListByState]
	@state_id int
as
begin
	if(@state_id = 0)
	begin
		SELECT sc.shopping_center_id
		  ,sc.shopping_center_name
		  ,sc.state_id
		  ,sc.city_name
		  ,sc.zip_code
		  ,sc.property_status_id
		  ,sc.rent_amount
		  ,sc.property_type_id
		  ,sc.spaces
		  ,sc.spaces_available
		  ,sc.building_size
		  ,sc.asset_status
		  ,sc.created_date
		  ,sc.shop_description
		  ,s.state_name
		  ,pt.asset_type_name
	  FROM tbl_shopping_center sc
		left join tbl_state s
		on sc.state_id = s.state_id
		left join tbl_asset_type pt
		on sc.property_type_id = pt.asset_type_id
	  where (is_deleted is null OR is_deleted = 0)
	end
	else
	begin
		SELECT sc.shopping_center_id
		  ,sc.shopping_center_name
		  ,sc.state_id
		  ,sc.city_name
		  ,sc.zip_code
		  ,sc.property_status_id
		  ,sc.rent_amount
		  ,sc.property_type_id
		  ,sc.spaces
		  ,sc.spaces_available
		  ,sc.building_size
		  ,sc.asset_status
		  ,sc.created_date
		  ,sc.shop_description
		  ,s.state_name
		  ,pt.asset_type_name
	  FROM tbl_shopping_center sc
		left join tbl_state s
		on sc.state_id = s.state_id
		left join tbl_asset_type pt
		on sc.property_type_id = pt.asset_type_id
	  where (is_deleted is null OR is_deleted = 0)
		AND sc.state_id = @state_id
	end
	
end
GO
/****** Object:  StoredProcedure [dbo].[GetSignedUpCustomerList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetSignedUpCustomerList]
as
begin
	SELECT [custimer_id]
		  ,[first_name]
		  ,[last_name]
		  ,[email_address]
		  ,[contact_number]
		  ,[created_date]
	FROM [tbl_signedup_customer]
end
GO
/****** Object:  StoredProcedure [dbo].[GetStateList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetStateList]
as
begin
	select state_id, state_code, state_name from tbl_state
	order by state_name
end
GO
/****** Object:  StoredProcedure [dbo].[GetStateListById]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetStateListById]
	@stateId int
as
begin
	select state_id, state_code, state_name from tbl_state
	where state_id=@stateId
end
GO
/****** Object:  StoredProcedure [dbo].[GetSubittedPropertyItemById]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetSubittedPropertyItemById]
	@site_details_id int
as
begin
	SELECT p.site_details_id
      ,p.name_prefix
      ,p.first_name
      ,p.last_name
      ,p.company_name
      ,p.email_address
      ,p.address
      ,p.city_name
      ,p.state_id
      ,customerState.state_name
      ,p.zip_code
      ,p.contact_number
      ,p.sams_holding_employee
      ,p.market_id
      ,p.site_address
      ,p.site_city
      ,p.site_state_id
      ,mState.state_name as site_state_name
      ,p.site_county
      ,p.site_cross_street_name
      ,p.is_property_available
      ,p.zoning
      ,p.lot_size
      ,p.sales_price
      ,p.comments
      ,p.created_date
      ,p.property_type
      ,p.image_name
  FROM tbl_submitted_property p
  left join tbl_state customerState
  on p.state_id = customerState.state_id
  left join tbl_state mState 
  on p.site_state_id = mState.state_id
  WHERE p.site_details_id = @site_details_id

end
GO
/****** Object:  StoredProcedure [dbo].[GetSubittedPropertyListByCategory]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetSubittedPropertyListByCategory]
	@property_type int
as
begin
	SELECT [site_details_id]
      ,[name_prefix]
      ,[first_name]
      ,[last_name]
      ,[company_name]
      ,[email_address]
      ,[address]
      ,[city_name]
      ,p.state_id
      ,customerState.state_name
      ,[zip_code]
      ,[contact_number]
      ,[sams_holding_employee]
      ,[market_id]
      ,[site_address]
      ,[site_city]
      ,[site_state_id]
      ,mState.state_name as site_state_name
      ,[site_county]
      ,[site_cross_street_name]
      ,[is_property_available]
      ,[zoning]
      ,[lot_size]
      ,[sales_price]
      ,[comments]
      ,[created_date]
      ,[property_type]
      ,[image_name]
      ,[image_file_name]
      ,[pdf_file_name]
      ,[is_deleted]
  FROM [tbl_submitted_property] p
  left join tbl_state customerState
  on p.site_state_id = customerState.state_id
  left join tbl_state mState 
  on p.site_state_id = mState.state_id
  where is_deleted = 0 OR is_deleted is null
end
GO
/****** Object:  StoredProcedure [dbo].[GetSubittedPropertyListById]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetSubittedPropertyListById] 
	@propertyId int
as
begin
	SELECT [site_details_id]
      ,[name_prefix]
      ,[first_name]
      ,[last_name]
      ,[company_name]
      ,[email_address]
      ,[address]
      ,[city_name]
      ,p.state_id
      ,customerState.state_name
      ,[zip_code]
      ,[contact_number]
      ,[sams_holding_employee]
      ,[market_id]
      ,[site_address]
      ,[site_city]
      ,[site_state_id]
      ,mState.state_name as site_state_name
      ,[site_county]
      ,[site_cross_street_name]
      ,[is_property_available]
      ,[zoning]
      ,[lot_size]
      ,[sales_price]
      ,[comments]
      ,[created_date]
      ,[property_type]
      ,[image_name]
      ,[image_file_name]
      ,[pdf_file_name]
      ,[is_deleted]
  FROM [tbl_submitted_property] p
  left join tbl_state customerState
  on p.site_state_id = customerState.state_id
  left join tbl_state mState 
  on p.site_state_id = mState.state_id
  where site_details_id = @propertyId
end
GO
/****** Object:  StoredProcedure [dbo].[GetSurplusFiles]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetSurplusFiles]
	@property_id int
as
begin
	select [file_id],
			[property_id],
			[file_type],
			[file_name]
	from tbl_surplus_files
	where [property_id] = @property_id
end
GO
/****** Object:  StoredProcedure [dbo].[GetSurplusMonthlyData]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetSurplusMonthlyData]
	@asset_status int
as
begin
	select 
		m.month_id,
		m.month_name,
		Year(created_date) as createdYear,
		count(site_details_id) as totalRecords
	from tbl_month m
	left join tbl_property p
	on Month(created_date) = m.month_id 
	AND year(created_date) = year(getdate())
	AND p.asset_status = @asset_status 

	group by m.month_id, Year(created_date), m.month_name
	order by m.month_id, Year(created_date)

end
GO
/****** Object:  StoredProcedure [dbo].[GetTodoList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetTodoList]
	@property_id int,
	@property_type int
as
begin
	SELECT [todo_id]
      ,[property_id]
      ,[todo_text]
      ,[property_type]
      ,[created_date]
  FROM [tbl_todo]
	where property_id = @property_id
	AND property_type = @property_type
order by created_date desc
end
GO
/****** Object:  StoredProcedure [dbo].[GetUserById]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetUserById]
	@user_id int
as
begin
SELECT u.[user_id]
      ,u.[first_name]
      ,u.[last_name]
      ,u.[user_name]
      ,u.[password]
      ,u.[role_id]
      ,r.[role_name]
  FROM [dbo].[tbl_user] u
  left join [tbl_role] r
  on r.role_id = r.role_id
  where [user_id] = @user_id
end
GO
/****** Object:  StoredProcedure [dbo].[GetUserDetails]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetUserDetails]
	@userName varchar(500),
	@password varchar(500)
as
begin
	select user_id, 
		first_name, 
		last_name,
		user_name,
		role_id
	from dbo.tbl_user
	where user_name = @userName AND password = @password
end
GO
/****** Object:  StoredProcedure [dbo].[GetUserForLogin]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetUserForLogin]
	@user_name varchar(500),
	@customer_password varchar(500)
as
begin
	SELECT customer_id
      ,first_name
      ,last_name
      ,email_address
      ,contact_number
      ,user_name
      ,customer_password
      ,created_date
      ,last_login_date
  FROM tbl_customer
  where user_name = @user_name
  AND customer_password = @customer_password
end
GO
/****** Object:  StoredProcedure [dbo].[GetUserList]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetUserList]
as
begin
SELECT u.[user_id]
      ,u.[first_name]
      ,u.[last_name]
      ,u.[user_name]
      ,u.[password]
      ,u.[role_id]
      ,r.[role_name]
  FROM [dbo].[tbl_user] u
  left join [tbl_role] r
  on r.role_id = r.role_id
end
GO
/****** Object:  StoredProcedure [dbo].[RegisterCustomer]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[RegisterCustomer]
	@customer_id int
   ,@first_name varchar(500)
   ,@last_name varchar(500)
   ,@email_address varchar(500)
   ,@contact_number varchar(500)
   ,@signed_nda_file varchar(500)
   ,@user_name varchar(500)
   ,@customer_password varchar(500)
   ,@company_name varchar(500)
   ,@given_title varchar(500)
   
   ,@address varchar(500)
   ,@zipcode varchar(500)
   ,@city varchar(500)
   ,@state_id varchar(500)
   ,@cell_number varchar(500)
   
as
begin
	if(@customer_id=0)
	begin
		INSERT INTO [tbl_customer]
           ([first_name]
           ,[last_name]
           ,[email_address]
           ,[contact_number]
           ,[signed_nda_file]
           ,[user_name]
           ,[customer_password]
           ,[created_date]
           ,[company_name]
           ,[given_title]
           
           ,[address]
           ,[zipcode]
           ,[city]
           ,[state_id]
           ,[cell_number])
		VALUES
           (@first_name  
           ,@last_name  
           ,@email_address  
           ,@contact_number  
           ,@signed_nda_file
           ,@user_name  
           ,@customer_password
           ,getdate()
           ,@company_name
           ,@given_title
           ,@address
           ,@zipcode
           ,@city
           ,@state_id
           ,@cell_number
           )
           
        select scope_identity()
	end
	else
	begin
		UPDATE [tbl_customer]
		SET
            [first_name]=@first_name
           ,[last_name]=@last_name
           ,[email_address]=@email_address
           ,[contact_number]=@contact_number
           ,[signed_nda_file]=@signed_nda_file
           ,[user_name]=@user_name
           ,[customer_password]=@customer_password
           ,[company_name] = @company_name
           ,[given_title] = @given_title
           ,[address] = @address
           ,[zipcode] = @zipcode
           ,[city] = @city
           ,[state_id] = @state_id
           ,[cell_number] = @cell_number
        WHERE customer_id = @customer_id
        
        select @customer_id
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SaveAssetType]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SaveAssetType]
	@asset_type_id int,
	@asset_type_name varchar(500)
as
begin
	if(@asset_type_id = 0)
	begin
		insert into tbl_asset_type (asset_type_name) values(@asset_type_name)
	end
	else 
	begin
		update tbl_asset_type set asset_type_name = @asset_type_name
		where asset_type_id= @asset_type_id
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SaveCStore]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveCStore]
	@c_store_id int = 0
   ,@state_id int = 0
   ,@property_header varchar(500) = '' 
   ,@city varchar(500) = '' 
   ,@zipcode varchar(500)  = ''
   ,@county varchar(500)  = ''
   ,@asset_id varchar(500)  = ''
   ,@property_type_id int = 0
   ,@property_description varchar(500)  = ''
   ,@asking_price varchar(500)  = ''
   ,@asset_type_id int = 0
   ,@land_size varchar(500)  = ''
   ,@building_area varchar(500)  = ''
   ,@property_taxes varchar(500)  = ''
   ,@year_built varchar(500)  = ''
   ,@known_environmental_conditions varchar(500)  = ''
   ,@emv_copliance varchar(500)  = ''
   ,@hours_of_operation varchar(500)  = ''
   ,@environent_nda_pdf_filename varchar(500)  = ''
   ,@asset_status int = 0
   ,@c_store_address varchar(500)
as
begin
	if(@c_store_id = 0)
	begin
		INSERT INTO tbl_c_store
			   (state_id
			   ,property_header
			   ,city
			   ,zipcode
			   ,county
			   ,asset_id
			   ,property_type_id
			   ,property_description
			   ,asking_price
			   ,asset_type_id
			   ,land_size
			   ,building_area
			   ,property_taxes
			   ,year_built
			   ,known_environmental_conditions
			   ,emv_copliance
			   ,hours_of_operation
			   ,environent_nda_pdf_filename
			   ,created_date
			   ,asset_status
			   ,c_store_address)
		 VALUES
			   (@state_id 
			   ,@property_header
			   ,@city
			   ,@zipcode 
			   ,@county 
			   ,@asset_id 
			   ,@property_type_id 
			   ,@property_description  
			   ,@asking_price 
			   ,@asset_type_id
			   ,@land_size 
			   ,@building_area 
			   ,@property_taxes
			   ,@year_built 
			   ,@known_environmental_conditions  
			   ,@emv_copliance 
			   ,@hours_of_operation
			   ,@environent_nda_pdf_filename
			   ,getdate()
			   ,@asset_status
			   ,@c_store_address)
			   
			select scope_identity()
	end
	else
	begin
		UPDATE tbl_c_store
		SET
			    state_id = @state_id 
			   ,property_header = @property_header
			   ,city = @city
			   ,zipcode = @zipcode
			   ,county = @county
			   ,asset_id = @asset_id
			   ,property_type_id = @property_type_id
			   ,property_description = @property_description
			   ,asking_price = @asking_price
			   ,asset_type_id = @asset_type_id
			   ,land_size = @land_size
			   ,building_area = @building_area
			   ,property_taxes = @property_taxes
			   ,year_built = @year_built
			   ,known_environmental_conditions = @known_environmental_conditions
			   ,emv_copliance = @emv_copliance
			   ,hours_of_operation = @hours_of_operation
			   ,environent_nda_pdf_filename = @environent_nda_pdf_filename
			   ,c_store_address = @c_store_address
		WHERE c_store_id = @c_store_id
		
		select @c_store_id
	end

end
GO
/****** Object:  StoredProcedure [dbo].[SaveCstoreComplianceFiles]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SaveCstoreComplianceFiles]
	@file_id int,
	@property_id int,
	@file_type varchar(500),
	@file_name varchar(500)
as
begin
	if(@file_id = 0)
	begin
		insert into tbl_c_store_files
		(
			[property_id],
			[file_type],
			[file_name]
		)
		values
		(
			@property_id,
			@file_type,
			@file_name
		)
	end
	else
	begin
		update tbl_c_store_files
		set
			[property_id] = @property_id,
			[file_type] = @file_type,
			[file_name] = @file_name
		where 
			[file_id] = @file_id
	end
	
end
GO
/****** Object:  StoredProcedure [dbo].[SaveCustomerMessage]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SaveCustomerMessage]
	@custumer_name varchar(500)
   ,@customer_email varchar(500)
   ,@customer_subject varchar(500)
   ,@customer_message varchar(500)
as
begin
	INSERT INTO [tbl_customer_message]
           ([custumer_name]
           ,[customer_email]
           ,[customer_subject]
           ,[customer_message]
           ,[created_date])
     VALUES
           (@custumer_name 
           ,@customer_email
           ,@customer_subject 
           ,@customer_message 
           ,GETDATE())
end
GO
/****** Object:  StoredProcedure [dbo].[SaveCustomerSignature]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SaveCustomerSignature]
	@customer_sign varchar(500),
	@customer_id int
as
begin
	update tbl_customer
	set customer_sign = @customer_sign
	where customer_id = @customer_id
end
GO
/****** Object:  StoredProcedure [dbo].[SaveDiligenceAcquisition]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveDiligenceAcquisition]
	 @diligence_acquisition_id int
	,@property_id int = 0
	,@property_type int = 0
	,@purchase_price varchar(500) = ''
	,@earnest_money varchar(500) = ''
	,@exchange_1031 varchar(500) = ''
	,@dead_line_1031 varchar(500) = ''
	,@sellers varchar(500) = ''
	,@escrow_agent varchar(500) = ''
	,@sub_division varchar(500) = ''
	,@real_estate_agent varchar(500) = ''
as
begin

	if(@property_type = 1)
	begin
		update dbo.tbl_property set diligence_type = 1
		where site_details_id = @property_id
	end
	else if(@property_type = 2)
	begin
		update dbo.tbl_net_lease_property set diligence_type = 1
		where net_lease_property_id = @property_id
	end
	else if(@property_type = 3)
	begin
		update dbo.tbl_c_store set diligence_type = 1
		where c_store_id = @property_id
	end
	
	if(@diligence_acquisition_id = 0)
	begin
		insert into tbl_diligence_acquisition 
		(
			 property_id 
			,property_type 
			,purchase_price
			,earnest_money 
			,exchange_1031 
			,dead_line_1031 
			,sellers 
			,escrow_agent 
			,sub_division 
			,real_estate_agent 
			,created_date
		)
		values
		(
			 @property_id 
			,@property_type 
			,@purchase_price
			,@earnest_money 
			,@exchange_1031 
			,@dead_line_1031 
			,@sellers 
			,@escrow_agent 
			,@sub_division 
			,@real_estate_agent 
			,getdate()
		)
		select scope_identity()
	end
	else
	begin
		update tbl_diligence_acquisition set
			 property_id = @property_id 
			,property_type = @property_type 
			,purchase_price = @purchase_price
			,earnest_money = @earnest_money 
			,exchange_1031 = @exchange_1031 
			,dead_line_1031 = @dead_line_1031 
			,sellers = @sellers 
			,escrow_agent = @escrow_agent 
			,sub_division = @sub_division 
			,real_estate_agent = @real_estate_agent 
		where diligence_acquisition_id = @diligence_acquisition_id
		
		select @diligence_acquisition_id
	end
	
	
end
GO
/****** Object:  StoredProcedure [dbo].[SaveDiligenceDispositions]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveDiligenceDispositions]
	 @diligence_dispositions_id int = 0
	,@property_id int = 0
	,@property_type int = 0
	,@sale_price varchar(500) = ''
	,@earnest_money varchar(500) = ''
	,@buyers varchar(500) = ''
	,@escrow_agent varchar(500) = ''
	,@buyers_attorney varchar(500) = ''
	,@options_to_extend varchar(500) = ''
	,@commissions varchar(500) = ''
as
begin
	
	if(@property_type = 1)
	begin
		update dbo.tbl_property set diligence_type = 2
		where site_details_id = @property_id
	end
	else if(@property_type = 2)
	begin
		update dbo.tbl_net_lease_property set diligence_type = 2
		where net_lease_property_id = @property_id
	end
	else if(@property_type = 3)
	begin
		update dbo.tbl_c_store set diligence_type = 2
		where c_store_id = @property_id
	end
	
	if(@diligence_dispositions_id = 0)
	begin
		insert into tbl_diligence_dispositions
		(
			 property_id
			,property_type
			,sale_price
			,earnest_money
			,buyers
			,escrow_agent
			,buyers_attorney
			,options_to_extend
			,commissions
			,created_date
		)
		values
		(
			 @property_id
			,@property_type
			,@sale_price
			,@earnest_money
			,@buyers
			,@escrow_agent
			,@buyers_attorney
			,@options_to_extend
			,@commissions
			,getdate()
		)
		select scope_identity()
	end
	else
	begin
		UPDATE tbl_diligence_dispositions
		set  property_id = @property_id
			,property_type = @property_type
			,sale_price = @sale_price
			,earnest_money = @earnest_money
			,buyers = @buyers
			,escrow_agent = @escrow_agent
			,buyers_attorney = @buyers_attorney
			,options_to_extend = @options_to_extend
			,commissions = @commissions
		where diligence_dispositions_id = @diligence_dispositions_id
		
		select @diligence_dispositions_id
	end
	
end
GO
/****** Object:  StoredProcedure [dbo].[SaveDiligenceLease]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveDiligenceLease]
	@diligence_lease_id int,
	@property_id int,
	@property_type int,
	@tenant_name varchar(500)
as
begin

	if(@property_type = 1)
	begin
		update dbo.tbl_property set diligence_type = 3
		where site_details_id = @property_id
	end
	else if(@property_type = 2)
	begin
		update dbo.tbl_net_lease_property set diligence_type = 3
		where net_lease_property_id = @property_id
	end
	else if(@property_type = 3)
	begin
		update dbo.tbl_c_store set diligence_type = 3
		where c_store_id = @property_id
	end
	
	if(@diligence_lease_id = 0)
	begin
		insert into [tbl_diligence_lease]
		(
			property_id,
			property_type,
			tenant_name
		)
		values
		(
			@property_id,
			@property_type,
			@tenant_name
		)
		select scope_identity()
	end
	else
	begin
		update [tbl_diligence_lease]
		set property_id = @property_id,
			property_type = @property_type,
			tenant_name = @tenant_name
		where diligence_lease_id = @diligence_lease_id
		
		select @diligence_lease_id
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SaveMapLocation]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveMapLocation]
	@propertyId int,
	@property_latitude varchar(500),
	@property_longitude varchar(500),
	@propertyType int
	
as
begin
	if(@propertyType = 1)
	begin
		update tbl_property set property_latitude = @property_latitude, property_longitude = @property_longitude
		where site_details_id = @propertyId
	end
	else if(@propertyType = 2)
	begin
		update tbl_net_lease_property set property_latitude = @property_latitude, property_longitude = @property_longitude
		where net_lease_property_id = @propertyId
	end
	else if(@propertyType = 3)
	begin
	 update tbl_c_store set property_latitude = @property_latitude, property_longitude = @property_longitude
		where c_store_id = @propertyId
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SaveNetLeaseFiles]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SaveNetLeaseFiles]
	@file_id int,
	@property_id int,
	@file_type varchar(500),
	@file_name varchar(500)
as
begin
	if(@file_id = 0)
	begin
		insert into tbl_net_lease_files
		(
			[property_id],
			[file_type],
			[file_name]
		)
		values
		(
			@property_id,
			@file_type,
			@file_name
		)
	end
	else
	begin
		update tbl_net_lease_files
		set
			[property_id] = @property_id,
			[file_type] = @file_type,
			[file_name] = @file_name
		where 
			[file_id] = @file_id
	end
	
end
GO
/****** Object:  StoredProcedure [dbo].[SaveNetleaseProperty]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveNetleaseProperty]
	@net_lease_property_id int
   ,@asset_id varchar(500) = ''
   ,@asset_name varchar(500) = ''
   ,@state_id int = 0
   ,@city varchar(500) = ''
   ,@property_price varchar(500) = ''
   ,@cap_rate float = 0
   ,@term varchar(500) = ''
   ,@detail_pdf varchar(500) = ''
   ,@asset_type_id int = 0
   ,@asset_status int
   ,@is_shopping_center int
   ,@property_address varchar(500)
   ,@property_zipcode varchar(500)
as
begin
	if(@net_lease_property_id = 0)
	begin
		INSERT INTO tbl_net_lease_property
           ([asset_id]
           ,[asset_name]
           ,[state_id]
           ,[city]
           ,[property_price]
           ,[cap_rate]
           ,[term]
           ,[detail_pdf]
           ,[created_date]
           ,[asset_type_id]
           ,[asset_status]
           ,[is_shopping_center]
           ,[property_address]
           ,[property_zipcode])
		VALUES
           (@asset_id
           ,@asset_name
           ,@state_id
           ,@city
           ,@property_price
           ,@cap_rate
           ,@term
           ,@detail_pdf
           ,getdate()
           ,@asset_type_id
           ,@asset_status
           ,@is_shopping_center
           ,@property_address
           ,@property_zipcode)
           
        select scope_identity()
	end
	else
	begin
		UPDATE tbl_net_lease_property
		SET
		 asset_id = @asset_id
		,asset_name = @asset_name
		,state_id = @state_id
		,city = @city
		,property_price = @property_price
		,cap_rate = @cap_rate
		,term = @term
		,detail_pdf = @detail_pdf
		,asset_type_id = @asset_type_id
		,is_shopping_center = @is_shopping_center
		,property_address = @property_address
        ,property_zipcode = @property_zipcode
		WHERE net_lease_property_id = @net_lease_property_id
		
		select @net_lease_property_id
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SavePeriod]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SavePeriod]
	   @period_id int = 0
      ,@property_id int = 0
      ,@property_type int = 0
      ,@period_master varchar(500) = ''
      ,@start_date datetime
      ,@end_date datetime
      ,@period_notes varchar(500) = ''
as
begin
	if(@period_id = 0)
	begin
		insert into tbl_period
		(
			 property_id 
			,property_type 
			,period_master 
			,start_date 
			,end_date 
			,period_notes 
		)
		values
		(
			 @property_id 
			,@property_type 
			,@period_master 
			,@start_date 
			,@end_date 
			,@period_notes 
		)
		
		select scope_identity()
	end
	else
	begin
		update tbl_period
		set
			 property_id = @property_id
			,property_type = @property_type
			,period_master = @period_master
			,start_date = @start_date
			,end_date = @end_date
			,period_notes = @period_notes
		where period_id = @period_id
		
		select @period_id
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SaveProperty]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveProperty]
	@site_details_id int,
	@name_prefix varchar(500) = '',
	@first_name varchar(500) = '',
	@last_name varchar(500) = '',
	@company_name varchar(500) = '',
	@email_address varchar(500) = '',
	@address varchar(500) = '',
	@city_name varchar(500) = '',
	@state_id varchar(500) = '',
	@zip_code varchar(500) = '',
	@contact_number varchar(500) = '',
	@sams_holding_employee int,
	@market_id int,
	@site_address varchar(500) = '',
	@site_city varchar(500) = '',
	@site_state_id varchar(500) = '',
	@site_county varchar(500) = '',
	@site_cross_street_name varchar(500) = '',
	@is_property_available bit,
	@zoning varchar(500) = '',
	@lot_size varchar(500) = '',
	@sales_price float,
	@comments varchar(500) = ''
as
begin
	if(@site_details_id = 0)
	begin
		insert into tbl_property 
		(
			name_prefix,
			first_name,
			last_name,
			company_name,
			email_address,
			address,
			city_name,
			state_id,
			zip_code,
			contact_number,
			sams_holding_employee,
			market_id,
			site_address,
			site_city,
			site_state_id,
			site_county,
			site_cross_street_name,
			is_property_available,
			zoning,
			lot_size,
			sales_price,
			comments,
			created_date
		)
		values
		(
			@name_prefix,
			@first_name,
			@last_name,
			@company_name,
			@email_address,
			@address,
			@city_name,
			@state_id,
			@zip_code,
			@contact_number,
			@sams_holding_employee,
			@market_id,
			@site_address,
			@site_city,
			@site_state_id,
			@site_county,
			@site_cross_street_name,
			@is_property_available,
			@zoning,
			@lot_size,
			@sales_price,
			@comments,
			getdate()
		)
		
		select scope_identity()
	end
	else
	begin
		update tbl_property set
			name_prefix = @name_prefix,
			first_name = @first_name,
			last_name = @last_name,
			company_name = @company_name,
			email_address = @email_address,
			address = @address,
			city_name = @city_name,
			state_id = @state_id,
			zip_code = @zip_code,
			contact_number = @contact_number,
			sams_holding_employee = @sams_holding_employee,
			market_id = @market_id,
			site_address = @site_address,
			site_city = @site_city,
			site_state_id = @site_state_id,
			site_county = @site_county,
			site_cross_street_name = @site_cross_street_name,
			is_property_available = @is_property_available,
			zoning = @zoning,
			lot_size = @lot_size,
			sales_price = @sales_price,
			comments = @comments
		where site_details_id = @site_details_id
		
		select @site_details_id
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SavePropertyAdmin]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SavePropertyAdmin]
	@site_details_id int,
	@name_prefix varchar(500) = '',
	@first_name varchar(500) = '',
	@last_name varchar(500) = '',
	@company_name varchar(500) = '',
	@email_address varchar(500) = '',
	@address varchar(500) = '',
	@city_name varchar(500) = '',
	@state_id varchar(500) = '',
	@zip_code varchar(500) = '',
	@contact_number varchar(500) = '',
	@sams_holding_employee int,
	@market_id int,
	@property_header varchar(500),
	@site_address varchar(500) = '',
	@site_city varchar(500) = '',
	@site_state_id varchar(500) = '',
	@site_county varchar(500) = '',
	@site_cross_street_name varchar(500) = '',
	@is_property_available bit,
	@zoning varchar(500) = '',
	@lot_size varchar(500) = '',
	@sales_price varchar(500) = '',
	@comments varchar(500) = '',
	@property_type int,
	@asset_type_id int,
	@asset_status int
as
begin
	if(@site_details_id = 0)
	begin
		insert into tbl_property 
		(
			name_prefix,
			first_name,
			last_name,
			company_name,
			email_address,
			address,
			city_name,
			state_id,
			zip_code,
			contact_number,
			sams_holding_employee,
			market_id,
			property_header,
			site_address,
			site_city,
			site_state_id,
			site_county,
			site_cross_street_name,
			is_property_available,
			zoning,
			lot_size,
			sales_price,
			comments,
			property_type,
			asset_type_id,
			asset_status,
			created_date
		)
		values
		(
			@name_prefix,
			@first_name,
			@last_name,
			@company_name,
			@email_address,
			@address,
			@city_name,
			@state_id,
			@zip_code,
			@contact_number,
			@sams_holding_employee,
			@market_id,
			@property_header,
			@site_address,
			@site_city,
			@site_state_id,
			@site_county,
			@site_cross_street_name,
			@is_property_available,
			@zoning,
			@lot_size,
			@sales_price,
			@comments,
			@property_type,
			@asset_type_id,
			@asset_status,
			getdate()
		)
		
		select scope_identity()
	end
	else
	begin
		update tbl_property set
			name_prefix = @name_prefix,
			first_name = @first_name,
			last_name = @last_name,
			company_name = @company_name,
			email_address = @email_address,
			address = @address,
			city_name = @city_name,
			state_id = @state_id,
			zip_code = @zip_code,
			contact_number = @contact_number,
			sams_holding_employee = @sams_holding_employee,
			market_id = @market_id,
			property_header = @property_header,
			site_address = @site_address,
			site_city = @site_city,
			site_state_id = @site_state_id,
			site_county = @site_county,
			site_cross_street_name = @site_cross_street_name,
			is_property_available = @is_property_available,
			zoning = @zoning,
			lot_size = @lot_size,
			sales_price = @sales_price,
			comments = @comments,
			property_type = @property_type,
			asset_type_id = @asset_type_id,
			asset_status = @asset_status
		where site_details_id = @site_details_id
		
		select @site_details_id
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SavePropertyImage]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SavePropertyImage]
	@property_id int,
	@image_name varchar(500),
	@property_type int
as
begin
	INSERT INTO [tbl_property_images]
           (property_id
           ,image_name
           ,property_type)
     VALUES
           (@property_id
           ,@image_name
           ,@property_type)
end
GO
/****** Object:  StoredProcedure [dbo].[SavePropertyType]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SavePropertyType]
	@property_type_id int 
   ,@property_type_name varchar(500)
as
begin
	if(@property_type_id = 0)
	begin
		insert into tbl_property_type
		(
			property_type_name
		)
		values
		(
			@property_type_name
		)
		select scope_identity()
	end
	else
	begin
		update tbl_property_type 
		set property_type_name = @property_type_name
		where property_type_id = @property_type_id
		
		select @property_type_id
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SaveRole]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SaveRole]
	@role_id int, 
	@role_name varchar(500)
as
begin
	if(@role_id = 0)
	begin
		insert into tbl_role
		(
			role_name
		)
		values
		(
			@role_name
		)
		select scope_identity()
	end
	else
	begin
		update tbl_role set role_name = @role_name
		where role_id = @role_id
		
		select @role_id
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SaveSamsSettings]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SaveSamsSettings]
	@settings_id int = 0
   ,@smtp_mail_server varchar(500) = ''
   ,@smtp_port_number varchar(500) = ''
   ,@smtp_email_address varchar(500) = ''
   ,@smtp_password varchar(500) = ''
   ,@email_header varchar(500) = ''
   ,@email_body varchar(5000) = ''
as
begin
	if(@settings_id = 0)
	begin
		INSERT INTO [tbl_settings]
           ([smtp_mail_server]
           ,[smtp_port_number]
           ,[smtp_email_address]
           ,[smtp_password]
           ,[email_header]
           ,[email_body])
     VALUES
           (@smtp_mail_server
           ,@smtp_port_number 
           ,@smtp_email_address 
           ,@smtp_password 
           ,@email_header 
           ,@email_body)
           
      select scope_identity()
	end
	else
	begin
		UPDATE [tbl_settings]
           SET [smtp_mail_server] = @smtp_mail_server
           ,[smtp_port_number] = @smtp_port_number
           ,[smtp_email_address] = @smtp_email_address
           ,[smtp_password] = @smtp_password
           ,[email_header] = @email_header
           ,[email_body] = @email_body
           WHERE settings_id = @settings_id
           
           select @settings_id
	end
	
end
GO
/****** Object:  StoredProcedure [dbo].[SaveSateDetails]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveSateDetails]
	@stateId int,
	@stateCode varchar(500),
	@stateName varchar(500)
as
begin
	if(@stateId = 0)
	begin
		insert into tbl_state
		(
			state_code,
			state_name
		)
		values
		(
			@stateCode,
			@stateName
		)
		
		select scope_identity()
	end
	else 
	begin
		update tbl_state
		set state_code = @stateCode,
			state_name = @stateName
		where state_id = @stateId
		
		select @stateId
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SaveShoppingCenter]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveShoppingCenter]
	 @shopping_center_id int
	,@shopping_center_name varchar(500) = ''
	,@state_id int = 0
	,@city_name varchar(500) = ''
	,@zip_code varchar(500) = ''
	,@property_status_id int = 0
	,@rent_amount varchar(500) = ''
	,@property_type_id int = 0
	,@spaces varchar(500) = ''
	,@spaces_available varchar(500) = ''
	,@building_size varchar(500) = ''
	,@asset_status int = 0
	,@shop_description varchar(5000) = ''
	,@is_deleted int = 0
as
begin
	if(@shopping_center_id = 0)
	begin
		INSERT INTO [tbl_shopping_center]
           ([shopping_center_name]
           ,[state_id]
           ,[city_name]
           ,[zip_code]
           ,[property_status_id]
           ,[rent_amount]
           ,[property_type_id]
           ,[spaces]
           ,[spaces_available]
           ,[building_size]
           ,[asset_status]
           ,[shop_description]
           ,[created_date]
           ,[is_deleted])
     VALUES
           ( @shopping_center_name 
			,@state_id 
			,@city_name 
			,@zip_code 
			,@property_status_id 
			,@rent_amount 
			,@property_type_id 
			,@spaces 
			,@spaces_available 
			,@building_size
			,@asset_status 
			,@shop_description
			,getdate()
			,@is_deleted)
			
		select scope_identity()

	end
	else
	begin
		update [tbl_shopping_center]
		SET
		[shopping_center_name] = @shopping_center_name,
		[state_id] = @state_id,
		[city_name] = @city_name,
		[zip_code] = @zip_code,
		[property_status_id] = @property_status_id,
		[rent_amount] = @rent_amount,
		[property_type_id] = @property_type_id,
		[spaces] = @spaces,
		[spaces_available] = @spaces_available,
		[building_size] = @building_size,
		[shop_description] = @shop_description
		WHERE
		[shopping_center_id] = @shopping_center_id
		
		select @shopping_center_id
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SaveSignedupCustomer]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SaveSignedupCustomer]
	@first_name varchar(500)
   ,@last_name varchar(500)
   ,@email_address varchar(500)
   ,@contact_number varchar(500)
as
begin
	INSERT INTO [tbl_signedup_customer]
           ([first_name]
           ,[last_name]
           ,[email_address]
           ,[contact_number]
           ,[created_date])
	 VALUES
		   (@first_name
		   ,@last_name
		   ,@email_address
		   ,@contact_number
		   ,getdate())
end
GO
/****** Object:  StoredProcedure [dbo].[SaveSubittedPropertyAdmin]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveSubittedPropertyAdmin]
	@site_details_id int,
	@name_prefix varchar(500) = '',
	@first_name varchar(500) = '',
	@last_name varchar(500) = '',
	@company_name varchar(500) = '',
	@email_address varchar(500) = '',
	@address varchar(500) = '',
	@city_name varchar(500) = '',
	@state_id varchar(500) = '',
	@zip_code varchar(500) = '',
	@contact_number varchar(500) = '',
	@sams_holding_employee int,
	@market_id int,
	@site_address varchar(500) = '',
	@site_city varchar(500) = '',
	@site_state_id varchar(500) = '',
	@site_county varchar(500) = '',
	@site_cross_street_name varchar(500) = '',
	@is_property_available bit,
	@zoning varchar(500) = '',
	@lot_size varchar(500) = '',
	@sales_price float,
	@comments varchar(500) = '',
	@property_type int
as
begin
	if(@site_details_id = 0)
	begin
		insert into tbl_submitted_property 
		(
			name_prefix,
			first_name,
			last_name,
			company_name,
			email_address,
			address,
			city_name,
			state_id,
			zip_code,
			contact_number,
			sams_holding_employee,
			market_id,
			site_address,
			site_city,
			site_state_id,
			site_county,
			site_cross_street_name,
			is_property_available,
			zoning,
			lot_size,
			sales_price,
			comments,
			property_type
		)
		values
		(
			@name_prefix,
			@first_name,
			@last_name,
			@company_name,
			@email_address,
			@address,
			@city_name,
			@state_id,
			@zip_code,
			@contact_number,
			@sams_holding_employee,
			@market_id,
			@site_address,
			@site_city,
			@site_state_id,
			@site_county,
			@site_cross_street_name,
			@is_property_available,
			@zoning,
			@lot_size,
			@sales_price,
			@comments,
			@property_type
		)
		
		select scope_identity()
	end
	else
	begin
		update tbl_submitted_property set
			name_prefix = @name_prefix,
			first_name = @first_name,
			last_name = @last_name,
			company_name = @company_name,
			email_address = @email_address,
			address = @address,
			city_name = @city_name,
			state_id = @state_id,
			zip_code = @zip_code,
			contact_number = @contact_number,
			sams_holding_employee = @sams_holding_employee,
			market_id = @market_id,
			site_address = @site_address,
			site_city = @site_city,
			site_state_id = @site_state_id,
			site_county = @site_county,
			site_cross_street_name = @site_cross_street_name,
			is_property_available = @is_property_available,
			zoning = @zoning,
			lot_size = @lot_size,
			sales_price = @sales_price,
			comments = @comments,
			property_type = @property_type
		where site_details_id = @site_details_id
		
		select @site_details_id
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SaveSubmittedProperty]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveSubmittedProperty]
	@site_details_id int,
	@name_prefix varchar(500) = '',
	@first_name varchar(500) = '',
	@last_name varchar(500) = '',
	@company_name varchar(500) = '',
	@email_address varchar(500) = '',
	@address varchar(500) = '',
	@city_name varchar(500) = '',
	@state_id varchar(500) = '',
	@zip_code varchar(500) = '',
	@contact_number varchar(500) = '',
	@sams_holding_employee int,
	@market_id int,
	@site_address varchar(500) = '',
	@site_city varchar(500) = '',
	@site_state_id varchar(500) = '',
	@site_county varchar(500) = '',
	@site_cross_street_name varchar(500) = '',
	@is_property_available bit,
	@zoning varchar(500) = '',
	@lot_size varchar(500) = '',
	@sales_price varchar(500) = '',
	@comments varchar(500) = '',
	@image_file_name varchar(500) = '',
	@pdf_file_name varchar(500) = ''
as
begin
	if(@site_details_id = 0)
	begin
		insert into tbl_submitted_property 
		(
			name_prefix,
			first_name,
			last_name,
			company_name,
			email_address,
			address,
			city_name,
			state_id,
			zip_code,
			contact_number,
			sams_holding_employee,
			market_id,
			site_address,
			site_city,
			site_state_id,
			site_county,
			site_cross_street_name,
			is_property_available,
			zoning,
			lot_size,
			sales_price,
			comments,
			image_file_name,
			pdf_file_name,
			created_date
		)
		values
		(
			@name_prefix,
			@first_name,
			@last_name,
			@company_name,
			@email_address,
			@address,
			@city_name,
			@state_id,
			@zip_code,
			@contact_number,
			@sams_holding_employee,
			@market_id,
			@site_address,
			@site_city,
			@site_state_id,
			@site_county,
			@site_cross_street_name,
			@is_property_available,
			@zoning,
			@lot_size,
			@sales_price,
			@comments,
			@image_file_name,
			@pdf_file_name,
			getdate()
		)
		
		select scope_identity()
	end
	else
	begin
		update tbl_submitted_property set
			name_prefix = @name_prefix,
			first_name = @first_name,
			last_name = @last_name,
			company_name = @company_name,
			email_address = @email_address,
			address = @address,
			city_name = @city_name,
			state_id = @state_id,
			zip_code = @zip_code,
			contact_number = @contact_number,
			sams_holding_employee = @sams_holding_employee,
			market_id = @market_id,
			site_address = @site_address,
			site_city = @site_city,
			site_state_id = @site_state_id,
			site_county = @site_county,
			site_cross_street_name = @site_cross_street_name,
			is_property_available = @is_property_available,
			zoning = @zoning,
			lot_size = @lot_size,
			sales_price = @sales_price,
			comments = @comments,
			image_file_name = @image_file_name,
			pdf_file_name = @pdf_file_name
		where site_details_id = @site_details_id
		
		select @site_details_id
	end
end
GO
/****** Object:  StoredProcedure [dbo].[SaveSurplusFiles]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SaveSurplusFiles]
	@file_id int,
	@property_id int,
	@file_type varchar(500),
	@file_name varchar(500)
as
begin
	if(@file_id = 0)
	begin
		insert into tbl_surplus_files
		(
			[property_id],
			[file_type],
			[file_name]
		)
		values
		(
			@property_id,
			@file_type,
			@file_name
		)
	end
	else
	begin
		update tbl_surplus_files
		set
			[property_id] = @property_id,
			[file_type] = @file_type,
			[file_name] = @file_name
		where 
			[file_id] = @file_id
	end
	
end
GO
/****** Object:  StoredProcedure [dbo].[SaveTodo]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveTodo]
	@property_id int
   ,@todo_text varchar(500)
   ,@property_type int
as
begin
	INSERT INTO [tbl_todo]
           ([property_id]
           ,[todo_text]
           ,[property_type]
           ,[created_date])
     VALUES
           (@property_id
           ,@todo_text
           ,@property_type 
           ,getdate())
end
GO
/****** Object:  StoredProcedure [dbo].[SaveUser]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SaveUser]
	@user_id int
   ,@first_name varchar(500)
   ,@last_name varchar(500)
   ,@user_name varchar(500)
   ,@password varchar(500)
   ,@role_id int
as
begin
	if(@user_id = 0)
	begin
		INSERT INTO [tbl_user]
           ([first_name]
           ,[last_name]
           ,[user_name]
           ,[password]
           ,[role_id])
     VALUES
           (@first_name 
           ,@last_name 
           ,@user_name 
           ,@password 
           ,@role_id )
           
      select scope_identity()
	end
	else
	begin
		UPDATE [tbl_user] SET
            [first_name] = @first_name 
           ,[last_name] = @last_name
           ,[user_name] = @user_name
           ,[password] = @password
           ,[role_id] = @role_id
		 WHERE [user_id] = @user_id
		 
		 select @user_id
	end
	
end
GO
/****** Object:  StoredProcedure [dbo].[SellCStore]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SellCStore]
	@c_store_id int
as
begin
	update tbl_c_store
	set asset_status = 1
	where c_store_id = @c_store_id
end
GO
/****** Object:  StoredProcedure [dbo].[SellNetLeaseProperty]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SellNetLeaseProperty]
	@net_lease_property_id int
as
begin
	update tbl_net_lease_property
	set asset_status = 1
	where net_lease_property_id = @net_lease_property_id
end
GO
/****** Object:  StoredProcedure [dbo].[SellSuplusProperty]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SellSuplusProperty]
	@site_details_id int
as
begin
	update tbl_property
	set asset_status = 1
	where site_details_id = @site_details_id
end
GO
/****** Object:  StoredProcedure [dbo].[TerminateDiligenceAcquisition]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TerminateDiligenceAcquisition]
	@diligence_acquisition_id int
as
begin
	update tbl_diligence_acquisition set acquisition_status = 2
	where diligence_acquisition_id = @diligence_acquisition_id
end
GO
/****** Object:  StoredProcedure [dbo].[TerminateDiligenceDisposition]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[TerminateDiligenceDisposition]
	@diligence_dispositions_id int
as
begin
	update tbl_diligence_dispositions set disposition_status = 2
	where diligence_dispositions_id = @diligence_dispositions_id
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateNewPropertyStatus]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[UpdateNewPropertyStatus]
	@propertyId int,
	@status int
as
begin
	update tbl_submitted_property 
	set is_deleted = @status
	where site_details_id = @propertyId
end
GO
/****** Object:  StoredProcedure [dbo].[uspLogError]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- uspLogError logs error information in the ErrorLog table about the
-- error that caused execution to jump to the CATCH block of a
-- TRY...CATCH construct. This should be executed from within the scope
-- of a CATCH block otherwise it will return without inserting error
-- information.
CREATE PROCEDURE [dbo].[uspLogError]
    @ErrorLogID int = 0 OUTPUT -- contains the ErrorLogID of the row inserted
AS                             -- by uspLogError in the ErrorLog table
BEGIN
    SET NOCOUNT ON;

    -- Output parameter value of 0 indicates that error
    -- information was not logged
    SET @ErrorLogID = 0;

    BEGIN TRY
        -- Return if there is no error information to log
        IF ERROR_NUMBER() IS NULL
            RETURN;

        -- Return if inside an uncommittable transaction.
        -- Data insertion/modification is not allowed when
        -- a transaction is in an uncommittable state.
        IF XACT_STATE() = -1
        BEGIN
            PRINT 'Cannot log error since the current transaction is in an uncommittable state. '
                + 'Rollback the transaction before executing uspLogError in order to successfully log error information.';
            RETURN;
        END

        INSERT [dbo].[ErrorLog]
            (
            [UserName],
            [ErrorNumber],
            [ErrorSeverity],
            [ErrorState],
            [ErrorProcedure],
            [ErrorLine],
            [ErrorMessage]
            )
        VALUES
            (
            CONVERT(sysname, CURRENT_USER),
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE()
            );

        -- Pass back the ErrorLogID of the row inserted
        SET @ErrorLogID = @@IDENTITY;
    END TRY
    BEGIN CATCH
        PRINT 'An error occurred in stored procedure uspLogError: ';
        EXECUTE [dbo].[uspPrintError];
        RETURN -1;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[uspPrintError]    Script Date: 1/19/2025 4:30:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- uspPrintError prints error information about the error that caused
-- execution to jump to the CATCH block of a TRY...CATCH construct.
-- Should be executed from within the scope of a CATCH block otherwise
-- it will return without printing any error information.
CREATE PROCEDURE [dbo].[uspPrintError]
AS
BEGIN
    SET NOCOUNT ON;

    -- Print error information.
    PRINT 'Error ' + CONVERT(varchar(50), ERROR_NUMBER()) +
          ', Severity ' + CONVERT(varchar(5), ERROR_SEVERITY()) +
          ', State ' + CONVERT(varchar(5), ERROR_STATE()) +
          ', Procedure ' + ISNULL(ERROR_PROCEDURE(), '-') +
          ', Line ' + CONVERT(varchar(5), ERROR_LINE());
    PRINT ERROR_MESSAGE();
END;
GO
USE [master]
GO
ALTER DATABASE [sams] SET  READ_WRITE 
GO
