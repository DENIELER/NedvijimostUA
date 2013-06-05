
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 06/05/2013 23:06:15
-- Generated from EDMX file: E:\Projects\Nedvijimost-ua\Site\App_Code\Model\RealEstateSaleSearchResult.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [NedvijimostDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AdvertismentAdvertismentsPhoto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AdvertismentsPhotoes] DROP CONSTRAINT [FK_AdvertismentAdvertismentsPhoto];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvertismentAdvertismentSection]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advertisments] DROP CONSTRAINT [FK_AdvertismentAdvertismentSection];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvertismentAdvertismentPhone]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AdvertismentPhones] DROP CONSTRAINT [FK_AdvertismentAdvertismentPhone];
GO
IF OBJECT_ID(N'[dbo].[FK_SearchResultAdvertisment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advertisments] DROP CONSTRAINT [FK_SearchResultAdvertisment];
GO
IF OBJECT_ID(N'[dbo].[FK_SearchResultAdvertismentSection]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SearchResults] DROP CONSTRAINT [FK_SearchResultAdvertismentSection];
GO
IF OBJECT_ID(N'[dbo].[FK_SubPurchaseSubPurchasePhone]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubPurchasePhones] DROP CONSTRAINT [FK_SubPurchaseSubPurchasePhone];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvertismentSubPurchase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advertisments] DROP CONSTRAINT [FK_AdvertismentSubPurchase];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvertismentSubSectionAdvertismentSection]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AdvertismentSubSection] DROP CONSTRAINT [FK_AdvertismentSubSectionAdvertismentSection];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvertismentAdvertismentSubSection]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advertisments] DROP CONSTRAINT [FK_AdvertismentAdvertismentSubSection];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[SubPurchasesList]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubPurchasesList];
GO
IF OBJECT_ID(N'[dbo].[Advertisments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Advertisments];
GO
IF OBJECT_ID(N'[dbo].[AdvertismentsPhotoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdvertismentsPhotoes];
GO
IF OBJECT_ID(N'[dbo].[AdvertismentSections]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdvertismentSections];
GO
IF OBJECT_ID(N'[dbo].[AdvertismentPhones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdvertismentPhones];
GO
IF OBJECT_ID(N'[dbo].[SearchResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SearchResults];
GO
IF OBJECT_ID(N'[dbo].[WebSearchFilterAdvertisments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WebSearchFilterAdvertisments];
GO
IF OBJECT_ID(N'[dbo].[SubPurchases]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubPurchases];
GO
IF OBJECT_ID(N'[dbo].[SubPurchasePhones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubPurchasePhones];
GO
IF OBJECT_ID(N'[dbo].[viewAdvertismentPhotos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[viewAdvertismentPhotos];
GO
IF OBJECT_ID(N'[dbo].[Articles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Articles];
GO
IF OBJECT_ID(N'[dbo].[AdvertismentSubSection]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdvertismentSubSection];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'SubPurchasesList'
CREATE TABLE [dbo].[SubPurchasesList] (
    [id] uniqueidentifier  NOT NULL,
    [phone] varchar(20)  NOT NULL,
    [name] varchar(30)  NULL,
    [surname] varchar(30)  NULL,
    [not_checked] bit  NULL,
    [createDate] datetime  NOT NULL
);
GO

-- Creating table 'Advertisments'
CREATE TABLE [dbo].[Advertisments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [text] nvarchar(max)  NOT NULL,
    [createDate] datetime  NOT NULL,
    [modifyDate] datetime  NOT NULL,
    [searchresult_id] int  NULL,
    [link] nvarchar(max)  NULL,
    [siteName] nvarchar(max)  NULL,
    [subpurchaseAdvertisment] bit  NOT NULL,
    [not_realestate] bit  NOT NULL,
    [not_show_advertisment] bit  NOT NULL,
    [isSpecial] bit  NOT NULL,
    [isSpecialDateTime] datetime  NULL,
    [AdvertismentSection_Id] int  NOT NULL,
    [SubPurchase_Id] uniqueidentifier  NULL,
    [AdvertismentSubSection_Id] int  NULL
);
GO

-- Creating table 'AdvertismentsPhotoes'
CREATE TABLE [dbo].[AdvertismentsPhotoes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [filename] nvarchar(max)  NOT NULL,
    [createDate] datetime  NOT NULL,
    [Advertisment_Id] int  NOT NULL
);
GO

-- Creating table 'AdvertismentSections'
CREATE TABLE [dbo].[AdvertismentSections] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [code] nvarchar(30)  NOT NULL,
    [displayName] nvarchar(max)  NOT NULL,
    [friendlyUrl] nvarchar(max)  NULL
);
GO

-- Creating table 'AdvertismentPhones'
CREATE TABLE [dbo].[AdvertismentPhones] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [phone] nvarchar(20)  NOT NULL,
    [AdvertismentId] int  NOT NULL
);
GO

-- Creating table 'SearchResults'
CREATE TABLE [dbo].[SearchResults] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [createDate] datetime  NOT NULL,
    [allParsedAdvertismentsCount] int  NOT NULL,
    [modifyDate] datetime  NOT NULL,
    [AdvertismentSection_Id] int  NOT NULL
);
GO

-- Creating table 'WebSearchFilterAdvertisments'
CREATE TABLE [dbo].[WebSearchFilterAdvertisments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [text] nvarchar(max)  NULL,
    [title] nvarchar(max)  NULL,
    [createDate] datetime  NOT NULL,
    [subPurchasePhone] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SubPurchases'
CREATE TABLE [dbo].[SubPurchases] (
    [Id] uniqueidentifier  NOT NULL,
    [name] nvarchar(30)  NULL,
    [surname] nvarchar(30)  NULL,
    [not_checked] bit  NOT NULL,
    [createDate] datetime  NOT NULL,
    [modifyDate] datetime  NOT NULL
);
GO

-- Creating table 'SubPurchasePhones'
CREATE TABLE [dbo].[SubPurchasePhones] (
    [Id] uniqueidentifier  NOT NULL,
    [phone] nvarchar(20)  NOT NULL,
    [SubPurchaseId] uniqueidentifier  NOT NULL,
    [createDate] datetime  NOT NULL
);
GO

-- Creating table 'viewAdvertismentPhotos'
CREATE TABLE [dbo].[viewAdvertismentPhotos] (
    [Id] int  NOT NULL,
    [text] nvarchar(max)  NOT NULL,
    [createDate] datetime  NOT NULL,
    [link] nvarchar(max)  NULL,
    [siteName] nvarchar(100)  NULL,
    [subpurchaseAdvertisment] bit  NOT NULL,
    [SubPurchase_Id] uniqueidentifier  NULL,
    [PhotoId] int  NOT NULL,
    [PhotoFileName] nvarchar(max)  NOT NULL,
    [PhotoCreateDate] datetime  NULL,
    [SectionCode] nvarchar(30)  NOT NULL,
    [SectionDisplayName] nvarchar(max)  NOT NULL,
    [SectionFriendlyUrl] nvarchar(max)  NULL
);
GO

-- Creating table 'Articles'
CREATE TABLE [dbo].[Articles] (
    [article_id] uniqueidentifier  NOT NULL,
    [header] nvarchar(max)  NOT NULL,
    [text] nvarchar(max)  NOT NULL,
    [createDate] datetime  NOT NULL,
    [author] nvarchar(max)  NULL,
    [link] nvarchar(max)  NOT NULL,
    [description] nvarchar(max)  NOT NULL,
    [title] nvarchar(max)  NOT NULL,
    [keywords] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AdvertismentSubSection'
CREATE TABLE [dbo].[AdvertismentSubSection] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [code] nvarchar(30)  NOT NULL,
    [displayName] nvarchar(max)  NOT NULL,
    [friendlyUrl] nvarchar(max)  NULL,
    [AdvertismentSection_Id] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NULL,
    [Password] nvarchar(max)  NULL,
    [FirstName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [VkontakteID] nvarchar(max)  NULL,
    [IsSubPurchase] bit  NOT NULL,
    [SubPurchaseID] uniqueidentifier  NOT NULL,
    [Login] nvarchar(max)  NULL,
    [IsAdmin] bit  NOT NULL,
    [Phone] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'SubPurchasesList'
ALTER TABLE [dbo].[SubPurchasesList]
ADD CONSTRAINT [PK_SubPurchasesList]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'Advertisments'
ALTER TABLE [dbo].[Advertisments]
ADD CONSTRAINT [PK_Advertisments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AdvertismentsPhotoes'
ALTER TABLE [dbo].[AdvertismentsPhotoes]
ADD CONSTRAINT [PK_AdvertismentsPhotoes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AdvertismentSections'
ALTER TABLE [dbo].[AdvertismentSections]
ADD CONSTRAINT [PK_AdvertismentSections]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AdvertismentPhones'
ALTER TABLE [dbo].[AdvertismentPhones]
ADD CONSTRAINT [PK_AdvertismentPhones]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SearchResults'
ALTER TABLE [dbo].[SearchResults]
ADD CONSTRAINT [PK_SearchResults]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WebSearchFilterAdvertisments'
ALTER TABLE [dbo].[WebSearchFilterAdvertisments]
ADD CONSTRAINT [PK_WebSearchFilterAdvertisments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SubPurchases'
ALTER TABLE [dbo].[SubPurchases]
ADD CONSTRAINT [PK_SubPurchases]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SubPurchasePhones'
ALTER TABLE [dbo].[SubPurchasePhones]
ADD CONSTRAINT [PK_SubPurchasePhones]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [PhotoId] in table 'viewAdvertismentPhotos'
ALTER TABLE [dbo].[viewAdvertismentPhotos]
ADD CONSTRAINT [PK_viewAdvertismentPhotos]
    PRIMARY KEY CLUSTERED ([PhotoId] ASC);
GO

-- Creating primary key on [article_id] in table 'Articles'
ALTER TABLE [dbo].[Articles]
ADD CONSTRAINT [PK_Articles]
    PRIMARY KEY CLUSTERED ([article_id] ASC);
GO

-- Creating primary key on [Id] in table 'AdvertismentSubSection'
ALTER TABLE [dbo].[AdvertismentSubSection]
ADD CONSTRAINT [PK_AdvertismentSubSection]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Advertisment_Id] in table 'AdvertismentsPhotoes'
ALTER TABLE [dbo].[AdvertismentsPhotoes]
ADD CONSTRAINT [FK_AdvertismentAdvertismentsPhoto]
    FOREIGN KEY ([Advertisment_Id])
    REFERENCES [dbo].[Advertisments]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvertismentAdvertismentsPhoto'
CREATE INDEX [IX_FK_AdvertismentAdvertismentsPhoto]
ON [dbo].[AdvertismentsPhotoes]
    ([Advertisment_Id]);
GO

-- Creating foreign key on [AdvertismentSection_Id] in table 'Advertisments'
ALTER TABLE [dbo].[Advertisments]
ADD CONSTRAINT [FK_AdvertismentAdvertismentSection]
    FOREIGN KEY ([AdvertismentSection_Id])
    REFERENCES [dbo].[AdvertismentSections]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvertismentAdvertismentSection'
CREATE INDEX [IX_FK_AdvertismentAdvertismentSection]
ON [dbo].[Advertisments]
    ([AdvertismentSection_Id]);
GO

-- Creating foreign key on [AdvertismentId] in table 'AdvertismentPhones'
ALTER TABLE [dbo].[AdvertismentPhones]
ADD CONSTRAINT [FK_AdvertismentAdvertismentPhone]
    FOREIGN KEY ([AdvertismentId])
    REFERENCES [dbo].[Advertisments]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvertismentAdvertismentPhone'
CREATE INDEX [IX_FK_AdvertismentAdvertismentPhone]
ON [dbo].[AdvertismentPhones]
    ([AdvertismentId]);
GO

-- Creating foreign key on [searchresult_id] in table 'Advertisments'
ALTER TABLE [dbo].[Advertisments]
ADD CONSTRAINT [FK_SearchResultAdvertisment]
    FOREIGN KEY ([searchresult_id])
    REFERENCES [dbo].[SearchResults]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SearchResultAdvertisment'
CREATE INDEX [IX_FK_SearchResultAdvertisment]
ON [dbo].[Advertisments]
    ([searchresult_id]);
GO

-- Creating foreign key on [AdvertismentSection_Id] in table 'SearchResults'
ALTER TABLE [dbo].[SearchResults]
ADD CONSTRAINT [FK_SearchResultAdvertismentSection]
    FOREIGN KEY ([AdvertismentSection_Id])
    REFERENCES [dbo].[AdvertismentSections]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SearchResultAdvertismentSection'
CREATE INDEX [IX_FK_SearchResultAdvertismentSection]
ON [dbo].[SearchResults]
    ([AdvertismentSection_Id]);
GO

-- Creating foreign key on [SubPurchaseId] in table 'SubPurchasePhones'
ALTER TABLE [dbo].[SubPurchasePhones]
ADD CONSTRAINT [FK_SubPurchaseSubPurchasePhone]
    FOREIGN KEY ([SubPurchaseId])
    REFERENCES [dbo].[SubPurchases]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SubPurchaseSubPurchasePhone'
CREATE INDEX [IX_FK_SubPurchaseSubPurchasePhone]
ON [dbo].[SubPurchasePhones]
    ([SubPurchaseId]);
GO

-- Creating foreign key on [SubPurchase_Id] in table 'Advertisments'
ALTER TABLE [dbo].[Advertisments]
ADD CONSTRAINT [FK_AdvertismentSubPurchase]
    FOREIGN KEY ([SubPurchase_Id])
    REFERENCES [dbo].[SubPurchases]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvertismentSubPurchase'
CREATE INDEX [IX_FK_AdvertismentSubPurchase]
ON [dbo].[Advertisments]
    ([SubPurchase_Id]);
GO

-- Creating foreign key on [AdvertismentSection_Id] in table 'AdvertismentSubSection'
ALTER TABLE [dbo].[AdvertismentSubSection]
ADD CONSTRAINT [FK_AdvertismentSubSectionAdvertismentSection]
    FOREIGN KEY ([AdvertismentSection_Id])
    REFERENCES [dbo].[AdvertismentSections]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvertismentSubSectionAdvertismentSection'
CREATE INDEX [IX_FK_AdvertismentSubSectionAdvertismentSection]
ON [dbo].[AdvertismentSubSection]
    ([AdvertismentSection_Id]);
GO

-- Creating foreign key on [AdvertismentSubSection_Id] in table 'Advertisments'
ALTER TABLE [dbo].[Advertisments]
ADD CONSTRAINT [FK_AdvertismentAdvertismentSubSection]
    FOREIGN KEY ([AdvertismentSubSection_Id])
    REFERENCES [dbo].[AdvertismentSubSection]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvertismentAdvertismentSubSection'
CREATE INDEX [IX_FK_AdvertismentAdvertismentSubSection]
ON [dbo].[Advertisments]
    ([AdvertismentSubSection_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------