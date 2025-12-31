/*
 Navicat Premium Data Transfer

 Source Server         : 本地库
 Source Server Type    : SQL Server
 Source Server Version : 16001000
 Source Host           : 127.0.0.1:1433
 Source Catalog        : MetMall
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 16001000
 File Encoding         : 65001

 Date: 29/12/2025 21:08:50
*/


-- ----------------------------
-- Table structure for Data_Product
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Data_Product]') AND type IN ('U'))
	DROP TABLE [dbo].[Data_Product]
GO

CREATE TABLE [dbo].[Data_Product] (
  [Id] bigint IDENTITY(1,1) NOT NULL,
  [ProductId] bigint NULL,
  [ProductName] varchar(255) COLLATE Chinese_PRC_CI_AS NULL
)
GO

ALTER TABLE [dbo].[Data_Product] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Data_ProductSku
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Data_ProductSku]') AND type IN ('U'))
	DROP TABLE [dbo].[Data_ProductSku]
GO

CREATE TABLE [dbo].[Data_ProductSku] (
  [Id] bigint IDENTITY(1,1) NOT NULL,
  [Pid] bigint NULL,
  [SkuCode] varchar(255) COLLATE Chinese_PRC_CI_AS NULL,
  [OriginSkuName] varchar(1000) COLLATE Chinese_PRC_CI_AS NULL,
  [SkuName] varchar(1000) COLLATE Chinese_PRC_CI_AS NULL,
  [StockNum] int NULL
)
GO

ALTER TABLE [dbo].[Data_ProductSku] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for JD_Product
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[JD_Product]') AND type IN ('U'))
	DROP TABLE [dbo].[JD_Product]
GO

CREATE TABLE [dbo].[JD_Product] (
  [Id] bigint IDENTITY(1,1) NOT NULL,
  [JDName] varchar(1000) COLLATE Chinese_PRC_CI_AS NULL,
  [JDUrl] varchar(2000) COLLATE Chinese_PRC_CI_AS NULL,
  [CreateTime] datetime NULL,
  [ModifiedTime] datetime NULL
)
GO

ALTER TABLE [dbo].[JD_Product] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for JD_ProductImg
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[JD_ProductImg]') AND type IN ('U'))
	DROP TABLE [dbo].[JD_ProductImg]
GO

CREATE TABLE [dbo].[JD_ProductImg] (
  [Id] bigint IDENTITY(1,1) NOT NULL,
  [JDId] bigint NULL,
  [ImgType] int NULL,
  [ImgUrl] varchar(2000) COLLATE Chinese_PRC_CI_AS NULL,
  [SavePath] varchar(2000) COLLATE Chinese_PRC_CI_AS NULL,
  [CreateTime] datetime NULL
)
GO

ALTER TABLE [dbo].[JD_ProductImg] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for JD_ProductSku
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[JD_ProductSku]') AND type IN ('U'))
	DROP TABLE [dbo].[JD_ProductSku]
GO

CREATE TABLE [dbo].[JD_ProductSku] (
  [Id] bigint IDENTITY(1,1) NOT NULL,
  [JDPid] bigint NULL,
  [SkuName] varchar(1000) COLLATE Chinese_PRC_CI_AS NULL,
  [SalePrice] decimal(18,2) NULL
)
GO

ALTER TABLE [dbo].[JD_ProductSku] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Sup_Product
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Sup_Product]') AND type IN ('U'))
	DROP TABLE [dbo].[Sup_Product]
GO

CREATE TABLE [dbo].[Sup_Product] (
  [Id] bigint IDENTITY(1,1) NOT NULL,
  [ProductId] bigint NULL,
  [ProductName] varchar(255) COLLATE Chinese_PRC_CI_AS NULL,
  [SalePrice] decimal(18,2) NULL,
  [SaleNum] int NULL,
  [Stock] int NULL,
  [TagName] varchar(500) COLLATE Chinese_PRC_CI_AS NULL
)
GO

ALTER TABLE [dbo].[Sup_Product] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Sup_ProductMatch
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Sup_ProductMatch]') AND type IN ('U'))
	DROP TABLE [dbo].[Sup_ProductMatch]
GO

CREATE TABLE [dbo].[Sup_ProductMatch] (
  [Id] bigint NOT NULL,
  [SupPid] bigint NULL,
  [DataPid] bigint NULL
)
GO

ALTER TABLE [dbo].[Sup_ProductMatch] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Sup_ProductSku
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Sup_ProductSku]') AND type IN ('U'))
	DROP TABLE [dbo].[Sup_ProductSku]
GO

CREATE TABLE [dbo].[Sup_ProductSku] (
  [Id] bigint IDENTITY(1,1) NOT NULL,
  [Pid] bigint NULL,
  [SkuCode] varchar(255) COLLATE Chinese_PRC_CI_AS NULL,
  [SkuName] varchar(1000) COLLATE Chinese_PRC_CI_AS NULL,
  [SaleNum] int NULL,
  [StockNum] int NULL,
  [OriginSalePrice] decimal(18,2) NULL,
  [SalePrice] decimal(18,2) NULL
)
GO

ALTER TABLE [dbo].[Sup_ProductSku] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Primary Key structure for table Data_Product
-- ----------------------------
ALTER TABLE [dbo].[Data_Product] ADD CONSTRAINT [PK__Data_Pro__3214EC073855D915] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = OFF, ALLOW_PAGE_LOCKS = OFF)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Data_ProductSku
-- ----------------------------
ALTER TABLE [dbo].[Data_ProductSku] ADD CONSTRAINT [PK__Data_Pro__3214EC077A9F2146] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = OFF, ALLOW_PAGE_LOCKS = OFF)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table JD_Product
-- ----------------------------
ALTER TABLE [dbo].[JD_Product] ADD CONSTRAINT [PK__JD_Produ__3214EC072C818DB9] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = OFF, ALLOW_PAGE_LOCKS = OFF)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table JD_ProductImg
-- ----------------------------
ALTER TABLE [dbo].[JD_ProductImg] ADD CONSTRAINT [PK__JD_Produ__3214EC07EC810E95] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = OFF, ALLOW_PAGE_LOCKS = OFF)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table JD_ProductSku
-- ----------------------------
ALTER TABLE [dbo].[JD_ProductSku] ADD CONSTRAINT [PK__JD_Produ__3214EC07405400CB] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = OFF, ALLOW_PAGE_LOCKS = OFF)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Sup_Product
-- ----------------------------
ALTER TABLE [dbo].[Sup_Product] ADD CONSTRAINT [PK__Sup_Prod__3214EC07826261A2] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = OFF, ALLOW_PAGE_LOCKS = OFF)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Sup_ProductMatch
-- ----------------------------
ALTER TABLE [dbo].[Sup_ProductMatch] ADD CONSTRAINT [PK__Sup_Prod__3214EC076732717B] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = OFF, ALLOW_PAGE_LOCKS = OFF)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Sup_ProductSku
-- ----------------------------
ALTER TABLE [dbo].[Sup_ProductSku] ADD CONSTRAINT [PK__Sup_Prod__3214EC072CF4C8D0] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = OFF, ALLOW_PAGE_LOCKS = OFF)  
ON [PRIMARY]
GO

