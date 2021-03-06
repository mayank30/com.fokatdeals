USE [master]
GO
/****** Object:  Database [fd.com]    Script Date: 12/8/2015 4:07:57 AM ******/
CREATE DATABASE [fd.com]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'fd.com', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\fd.com.mdf' , SIZE = 766976KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'fd.com_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\fd.com_log.ldf' , SIZE = 136064KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [fd.com] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [fd.com].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [fd.com] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [fd.com] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [fd.com] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [fd.com] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [fd.com] SET ARITHABORT OFF 
GO
ALTER DATABASE [fd.com] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [fd.com] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [fd.com] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [fd.com] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [fd.com] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [fd.com] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [fd.com] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [fd.com] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [fd.com] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [fd.com] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [fd.com] SET  DISABLE_BROKER 
GO
ALTER DATABASE [fd.com] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [fd.com] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [fd.com] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [fd.com] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [fd.com] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [fd.com] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [fd.com] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [fd.com] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [fd.com] SET  MULTI_USER 
GO
ALTER DATABASE [fd.com] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [fd.com] SET DB_CHAINING OFF 
GO
ALTER DATABASE [fd.com] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [fd.com] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [fd.com]
GO
/****** Object:  FullTextCatalog [FullTextSearchCatalog]    Script Date: 12/8/2015 4:07:57 AM ******/
CREATE FULLTEXT CATALOG [FullTextSearchCatalog]WITH ACCENT_SENSITIVITY = OFF
AS DEFAULT

GO
/****** Object:  StoredProcedure [dbo].[InsertGenerator]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[InsertGenerator]  
(@tableName varchar(100)) as  
--Declare a cursor to retrieve column specific information   
--for the specified table  
DECLARE cursCol CURSOR FAST_FORWARD FOR   
SELECT column_name,data_type FROM information_schema.columns   
    WHERE table_name = @tableName  
OPEN cursCol  
DECLARE @string nvarchar(3000) --for storing the first half   
                               --of INSERT statement  
DECLARE @stringData nvarchar(3000) --for storing the data   
                                   --(VALUES) related statement  
DECLARE @dataType nvarchar(1000) --data types returned   
                                 --for respective columns  
SET @string='INSERT '+@tableName+'('  
SET @stringData=''  
  
DECLARE @colName nvarchar(50)  
  
FETCH NEXT FROM cursCol INTO @colName,@dataType  
  
IF @@fetch_status<>0  
    begin  
    print 'Table '+@tableName+' not found, processing skipped.'  
    close curscol  
    deallocate curscol  
    return  
END  
  
WHILE @@FETCH_STATUS=0  
BEGIN  
IF @dataType in ('varchar','char','nchar','nvarchar')  
BEGIN  
    SET @stringData=@stringData+'''''''''+  
            isnull('+@colName+','''')+'''''',''+'  
END  
ELSE  
if @dataType in ('text','ntext') --if the datatype   
                                 --is text or something else   
BEGIN  
    SET @stringData=@stringData+'''''''''+  
          isnull(cast('+@colName+' as varchar(2000)),'''')+'''''',''+'  
END  
ELSE  
IF @dataType = 'money' --because money doesn't get converted   
                       --from varchar implicitly  
BEGIN  
    SET @stringData=@stringData+'''convert(money,''''''+  
        isnull(cast('+@colName+' as varchar(200)),''0.0000'')+''''''),''+'  
END  
ELSE   
IF @dataType='datetime'  
BEGIN  
    SET @stringData=@stringData+'''convert(datetime,''''''+  
        isnull(cast('+@colName+' as varchar(200)),''0'')+''''''),''+'  
END  
ELSE   
IF @dataType='image'   
BEGIN  
    SET @stringData=@stringData+'''''''''+  
       isnull(cast(convert(varbinary,'+@colName+')   
       as varchar(6)),''0'')+'''''',''+'  
END  
ELSE --presuming the data type is int,bit,numeric,decimal   
BEGIN  
    SET @stringData=@stringData+'''''''''+  
          isnull(cast('+@colName+' as varchar(200)),''0'')+'''''',''+'  
END  
  
SET @string=@string+@colName+','  
  
FETCH NEXT FROM cursCol INTO @colName,@dataType  
END  
  
DECLARE @Query nvarchar(4000) -- provide for the whole query,   
                              -- you may increase the size  
  
SET @query ='SELECT '''+substring(@string,0,len(@string)) + ')   
    VALUES(''+ ' + substring(@stringData,0,len(@stringData)-2)+'''+'')''   
    FROM '+@tableName  
exec sp_executesql @query --load and run the built query  
GO
/****** Object:  StoredProcedure [dbo].[SearchAllTables]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SearchAllTables]
(
    @SearchStr nvarchar(100)
)
AS
BEGIN

DECLARE @Results TABLE(ColumnName nvarchar(370), ColumnValue nvarchar(3630))

SET NOCOUNT ON

DECLARE @TableName nvarchar(256), @ColumnName nvarchar(128), @SearchStr2 nvarchar(110)
SET  @TableName = ''
SET @SearchStr2 = QUOTENAME('%' + @SearchStr + '%','''')

WHILE @TableName IS NOT NULL
BEGIN
    SET @ColumnName = ''
    SET @TableName = 
    (
        SELECT MIN(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME))
        FROM    INFORMATION_SCHEMA.TABLES
        WHERE       TABLE_TYPE = 'BASE TABLE'
            AND QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) > @TableName
            AND OBJECTPROPERTY(
                    OBJECT_ID(
                        QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)
                         ), 'IsMSShipped'
                           ) = 0
    )

    WHILE (@TableName IS NOT NULL) AND (@ColumnName IS NOT NULL)
    BEGIN
        SET @ColumnName =
        (
            SELECT MIN(QUOTENAME(COLUMN_NAME))
            FROM    INFORMATION_SCHEMA.COLUMNS
            WHERE       TABLE_SCHEMA    = PARSENAME(@TableName, 2)
                AND TABLE_NAME  = PARSENAME(@TableName, 1)
                AND DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar')
                AND QUOTENAME(COLUMN_NAME) > @ColumnName
        )

        IF @ColumnName IS NOT NULL
        BEGIN
            INSERT INTO @Results
            EXEC
            (
                'SELECT ''' + @TableName + '.' + @ColumnName + ''', LEFT(' + @ColumnName + ', 3630) 
                FROM ' + @TableName + ' (NOLOCK) ' +
                ' WHERE ' + @ColumnName + ' LIKE ' + @SearchStr2
            )
        END
    END 
END

SELECT ColumnName, ColumnValue FROM @Results
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ChangeAccountPassword]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[usp_ChangeAccountPassword]
(
	@username varchar(max),
	@password varchar(max)
)AS
BEGIN
	Declare @count int
	Select @count = COUNT(*) from tbl_AccountUser where (username = @username or email = @username)
	if(@count = 1)
	BEGIN
		update tbl_AccountUser set password = @password where (username = @username or email = @username)
		return 0;
	END
	else
	BEGIN
		return -1;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[usp_CreateModifySession]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[usp_CreateModifySession]
(
	@sessionid varchar(900),
	@ipaddress varchar(100),
	@userid bigint, 
	@username varchar(max),
	@status varchar(max)
)AS
BEGIN
	Declare @count int
	Select @count = count(*) from tbl_Session where sessionid = @sessionid
	if(@count = 1)
	BEGIN
		--update
		update tbl_Session set ipaddress = @ipaddress,userid = @userid,username = @username,status = @status where sessionid = @sessionid
		return 1;
	END
	ELSE
	BEGIN
		--insert
		insert tbl_Session values (@sessionid,@ipaddress,@userid,@username,@status,default)
		return 0;

	END
END
GO
/****** Object:  StoredProcedure [dbo].[usp_DeleteCategory]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[usp_DeleteCategory]
(
	@catid varchar(30)
)AS
BEGIN
	BEGIN TRY
	DECLARE @count int
	SELECT @count =  COUNT(*) from tbl_Category where catid = @catid
	IF(@count = 0)
	BEGIN
		return -1; /*Already deleted*/
	END
	ELSE
	BEGIN
		DELETE from tbl_Subcategory where catid = @catid
		DELETE from tbl_Category where catid = @catid
		return 0;
	END
	END TRY
BEGIN CATCH
		  SELECT
          ERROR_NUMBER() as ErrorNumber,
          ERROR_MESSAGE() as ErrorMessage;
          if(ERROR_NUMBER() = 547)
          BEGIN
			return -20 /*foriegn key constarint*/
          END
          else if(ERROR_NUMBER() = 2627)
          BEGIN
			return -30 /*unique key constarint*/
          END
          else
          BEGIN
			RETURN -40;
		  END
	END CATCH
END



GO
/****** Object:  StoredProcedure [dbo].[usp_DeleteSubCategory]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[usp_DeleteSubCategory]
(
	@scatid varchar(30)
)AS
BEGIN
	BEGIN TRY
	DECLARE @count int
	SELECT @count =  COUNT(*) from tbl_SubCategory where subcatid = @scatid
	IF(@count = 0)
	BEGIN
		return -1; /*Already deleted*/
	END
	ELSE
	BEGIN
		DELETE from tbl_Subcategory where subcatid = @scatid
		return 0;
	END
	END TRY
BEGIN CATCH
		  SELECT
          ERROR_NUMBER() as ErrorNumber,
          ERROR_MESSAGE() as ErrorMessage;
          if(ERROR_NUMBER() = 547)
          BEGIN
			return -20 /*foriegn key constarint*/
          END
          else if(ERROR_NUMBER() = 2627)
          BEGIN
			return -30 /*unique key constarint*/
          END
          else
          BEGIN
			RETURN -40;
		  END
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[usp_DeleteWishList]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[usp_DeleteWishList](
	@userid varchar(max),
	@prdid varchar(max)
)AS
BEGIN
	Delete from tbl_WishList where userid = @userid and prdid = @prdid
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAccountUser]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_GetAccountUser]
(
	@username varchar(max),
	@password varchar(max)
)AS
BEGIN
	Select * from tbl_AccountUser where (username = @username or email = @username) and password = @password and status = 'A'
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GETAFFILIATEDSTORES]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[usp_GETAFFILIATEDSTORES] AS
BEGIN
Select * from tbl_AffiliatedStores
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GETCATEGORY]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_GETCATEGORY]
(
	@isAll varchar(1),
	@status varchar(1)
)AS
BEGIN
select * from [dbo].[tbl_Category] where (@isAll = '' or @isAll = null or cathotstatus = @isAll) and (@status  = '' or @status = null or catstatus = @status )
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetCoupon]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_GetCoupon]
(
	@indx int,
	@size int,
	@value varchar(max)
)AS
BEGIN
	SET NOCOUNT ON;
	Declare @startIndx int, @endIndx int;
	SET @startIndx = @indx;
	SET @endIndx = @indx + @size;

	WITH CTEResults AS
	(
		Select  ROW_NUMBER() OVER (order by VoucherCodeid) AS RowNum,* from tbl_Coupon_CSV 
		Where (Status = 'Active' and Product not in ('http://localhost:8080/images/brand/no-brand.png'))
	)
	SELECT * FROM CTEResults WHERE RowNum BETWEEN @startIndx AND @endIndx ;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetProduct]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[usp_GetProduct]  



(  



 @value varchar(max) = null  



)  



AS    



BEGIN 



Select  imageUrl,prdRedirectUrl,seourl,name, 



		subCatid,storeid,regularprice,offerprice,uniqueId,prdid,



		prdUrl,status,custom1 from tbl_Products



 Where prdid = @value or uniqueid = @value



END 
GO
/****** Object:  StoredProcedure [dbo].[USP_GetProductsByPagination]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetProductsByPagination]
(
	@indx int,
	@size int,
	@value varchar(30) = null
)AS
BEGIN
	SET NOCOUNT ON;
	Declare @startIndx int, @endIndx int;
	SET @startIndx = @indx;
	SET @endIndx = @indx + @size;
	--SET @nextIndx = @endIndx + 1;
	WITH CTEResults AS
	(
		Select ROW_NUMBER() OVER (order by rand(checksum(*))) AS RowNum, imageUrl,prdRedirectUrl,seourl,name, 
		subCatid,storeid,regularprice,offerprice,uniqueId,prdid,
		description,prdUrl from tbl_Products tablesample(1 percent)
		Where ((status = 'A') and (seourl = @value or uniqueId = 
		@value or prdid = @value or subCatid in (SELECT * FROM dbo.Split(@value,',') ) 
		or storeid = @value)) 
	)

SELECT * FROM CTEResults WHERE RowNum BETWEEN @startIndx AND @endIndx ;

END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetRandomProducts]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetRandomProducts]
AS
BEGIN
		--Home Page URL
		--Top Record Count Records Random to show case from different cateory
		Select TOP(30) imageUrl,prdRedirectUrl,seourl,name, 
		subCatid,storeid,regularprice,offerprice,uniqueId,prdid,
		prdUrl FROM tbl_Products
			tablesample(1 percent)
			where status = 'A'
			order by rand(checksum(*))
END
	
GO
/****** Object:  StoredProcedure [dbo].[usp_GetSubCategory]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_GetSubCategory]
(
	@catid varchar(max)
)AS
BEGIN
	select * from tbl_Subcategory where (@catid = '' or @catid= null or catid = @catid or subcattaglist = @catid)
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetWishList]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[usp_GetWishList]
(
	@indx int,
	@size int,
	@value varchar(30) = null
)AS
BEGIN
	SET NOCOUNT ON;
	Declare @startIndx int, @endIndx int;
	SET @startIndx = @indx;
	SET @endIndx = @indx + @size;
	--SET @nextIndx = @endIndx + 1;
	WITH CTEResults AS
	(
		Select ROW_NUMBER() OVER (order by rand(checksum(*))) AS RowNum, imageUrl,prdRedirectUrl,seourl,name, 
		subCatid,storeid,regularprice,offerprice,uniqueId,prdid,
		description,prdUrl from tbl_Products
		Where uniqueId in (  SELECT prdid FROM tbl_WishList where userid = @value) 
	)

SELECT * FROM CTEResults WHERE RowNum BETWEEN @startIndx AND @endIndx ;

END
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertAccountUser]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[usp_InsertAccountUser]
(
	@username varchar(900),
	@email varchar(900),
	@phone varchar(900),
	@password varchar(900),
	@loggedSource varchar(max),
	@userType varchar(20),
	@status char(1)
)

AS

BEGIN

	Declare @count int

	Select @count = count(*) from tbl_AccountUser where username = @username or email = @email

	if(@count >= 1)

	BEGIN

		return -1 

	END

	ELSE
	Select @count = count(*) from tbl_AccountUser where phone = @phone
	if(@count >= 1)

	BEGIN

		return -1 

	END
	ELSE
	BEGIN

		insert into tbl_AccountUser values(@username,@email,@password,@phone,default,default,'','','','','',@loggedSource,@userType,@status,default);

		return SCOPE_IDENTITY()

	END

END

GO
/****** Object:  StoredProcedure [dbo].[USP_INSERTAFFILIATEDSTORE]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[USP_INSERTAFFILIATEDSTORE]
(
	@storename varchar(max),
	@link varchar(max),
	@desc varchar(max),
	@source varchar(max),
	@affUrl varchar(max),
	@affCode varchar(max),
	@status varchar(max),
	@logo varchar(max)
)AS
BEGIN
	Declare @count int,@seo varchar(max)
	Select @seo =  dbo.ufn_SEOUrl(@storename);
	Select @count = count(*) from tbl_AffiliatedStores where seourl = @seo;
	if(@count=1)
	BEGIN
		set @seo = @seo+'-'+CAST(@count as varchar(3));
	END
	insert into tbl_AffiliatedStores (storeid,storename,seourl,websiteLink,description,source,affUrl,affCode,follower,ratings,status,logo)
	 values (@seo,@storename,@seo,@link,@desc,@source,@affUrl,@affCode,0,0,@status,@logo);
	 return 0;
END


GO
/****** Object:  StoredProcedure [dbo].[usp_InsertCategory]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[usp_InsertCategory]
(
	@catid varchar(20) out,
	@catname varchar(500),
	@catdesc varchar(max),
	@catimage varchar(max),
	@cattaglist varchar(max)
)AS
BEGIN
	BEGIN TRY
		DECLARE @count int,@i int,@alias varchar(max)
		SET @i =	cast((90000* Rand() + 10000) as int)
		SET @catid = 'C'+CAST(@i as VARCHAR)
		SELECT @count = COUNT(*) from tbl_Category where catid = @catid
		IF(@count = 1)
		BEGIN	
			return -1; /*Category already exisit*/
		END
		ELSE
		BEGIN
			SELECT @alias = dbo.ufn_SEOUrl(@catname)
			Select @count  = COUNT(*) from tbl_Category  where categoryAlias = @alias
			if(@count > 0)
			BEGIN
				set @count = @count + 1;
				set @alias = CAST(@alias+'-'+CAST(@count as varchar) as VARCHAR);
			END
			WHILE PATINDEX( '%[~,@,#,$,%,&,*,(,)]%', @alias ) > 0 
			SET @alias = Replace(REPLACE( @alias, SUBSTRING( @alias, PATINDEX( '%[~,@,#,$,%,&,*,(,)]%', @alias ), 1 ),''),'','')
			insert into tbl_Category values (@catid,@catname,@catdesc,@catimage,@cattaglist,DEFAULT,DEFAULT,DEFAULT,@alias)
			return 0;
		END		
	END TRY
	BEGIN CATCH
		  SELECT
          ERROR_NUMBER() as ErrorNumber,
          ERROR_MESSAGE() as ErrorMessage;
          if(ERROR_NUMBER() = 547)
          BEGIN
			return -20 /*foriegn key constarint*/
          END
          else if(ERROR_NUMBER() = 2627)
          BEGIN
			return -30 /*unique key constarint*/
          END
          else
          BEGIN
			RETURN -40;
		  END
	END CATCH
END



GO
/****** Object:  StoredProcedure [dbo].[usp_InsertProduct]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[usp_InsertProduct]
(
	@uniquId varchar(max),
	@prdid varchar(max),
	@name varchar(max),
	@desc varchar(max),
	@prdUrl varchar(max),
	@prdre varchar(max),
	@img varchar(max),
	@w varchar(max),
	@h varchar(max),
	@storeid varchar(max),
	@reg varchar(max),
	@sell varchar(max),
	@catid varchar(max),
	@status varchar(max), 
	@c1 varchar(max),
	@c2 varchar(max)
)AS
BEGIN
	Declare @count int, @seo varchar(max)
	select @seo = [dbo].[ufn_SEOUrl] (@name)
	set @seo = @seo+'-'+@prdid
	Select @count =COUNT(*) from tbl_Products where prdid = @prdid
	if(@count=0)
	BEGIN
	insert into tbl_Products values (@uniquId,@prdid,@name,REPLACE(@desc,'"',''),@seo,@prdUrl,@prdre,@img,@storeid,@reg,@sell,@catid,default,default,@status,@c1,@c2,@w,@h);
	return SCOPE_IDENTITY()
	END
	else
	BEGIN
	update tbl_Products set name = @name,description = REPLACE(@desc,'"',''),
	seourl = @seo,prdUrl = @prdUrl,prdRedirectUrl = @prdre,imageurl = @img,storeid = @storeid,regularprice = @reg,
	offerprice = @sell,subCatid = @catid,status = @status where prdid = @prdid

	return 0;
	END 
END
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertSubcategory]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[usp_InsertSubcategory]
(
	@subcatid varchar(20) out,
	@subcatname varchar(500),
	@subcatdesc varchar(max),
	@subcatimage varchar(max),
	@subcattaglist varchar(max),
	@catid varchar(30)
)AS
BEGIN
	BEGIN TRY
		DECLARE @count int,@i int,@alias  varchar(max)
		SET @i =	cast((9000* Rand() + 1000) as int)
		SET @subcatid = 'SC'+CAST(@i as VARCHAR)
		SELECT @count = COUNT(*) from tbl_Subcategory where subcatid = @subcatid
		IF(@count = 1)
		BEGIN	
			return -1; /*sub category already exisit*/
		END
		ELSE
		BEGIN
			SELECT @alias = dbo.ufn_SEOUrl(@subcatname)
			Select @count  = COUNT(*) from tbl_Subcategory  where @alias = @alias
			if(@count > 0)
			BEGIN
				set @count = @count + 1;
				set @alias = CAST(@alias+'-'+CAST(@count as varchar) as VARCHAR);
			END
			WHILE PATINDEX( '%[~,@,#,$,%,&,*,(,)]%', @alias ) > 0 
			SET @alias = Replace(REPLACE( @alias, SUBSTRING( @alias, PATINDEX( '%[~,@,#,$,%,&,*,(,)]%', @alias ), 1 ),''),'','')
			insert into tbl_Subcategory values (@subcatid,@subcatname,@subcatdesc,@subcatimage,@subcattaglist,default,DEFAULt,DEFAULT,@catid,@alias)
			return 0;
		END		
	END TRY
	BEGIN CATCH
		  SELECT
          ERROR_NUMBER() as ErrorNumber,
          ERROR_MESSAGE() as ErrorMessage;
          if(ERROR_NUMBER() = 547)
          BEGIN
			return -20 /*foriegn key constarint*/
          END
          else if(ERROR_NUMBER() = 2627)
          BEGIN
			return -30 /*unique key constarint*/
          END
          else
          BEGIN
			RETURN -40;
		  END
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[usp_InsertSubcategoryNew]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[usp_InsertSubcategoryNew]
(
	@subcatname varchar(500),
	@catid varchar(30)
)AS
BEGIN
	BEGIN TRY
		DECLARE @count int,@i int,@alias  varchar(max),@subcatid varchar(max)
		SET @i =	cast((9000* Rand() + 1000) as int)
		SET @subcatid = 'SC'+CAST(@i as VARCHAR)
		SELECT @count = COUNT(*) from tbl_Subcategory where subcatid = @subcatid
		IF(@count = 1)
		BEGIN	
			return -1; /*sub category already exisit*/
		END
		ELSE
		BEGIN
			--SELECT @alias = dbo.ufn_SEOUrl(@subcatname)
			--Select @count  = COUNT(*) from tbl_Subcategory  where @alias = @alias
			--if(@count > 0)
			--BEGIN
			--	set @count = @count + 1;
			--	set @alias = CAST(@alias+'-'+CAST(@count as varchar) as VARCHAR);
			--END
			
			insert into tbl_Subcategory values (@subcatid,Replace(@subcatname,'_',' '),@subcatname,'C-0001','C-0001',default,DEFAULt,DEFAULT,@catid,Replace(@subcatname,'_','-'))
			return 0;
		END		
	END TRY
	BEGIN CATCH
		  SELECT
          ERROR_NUMBER() as ErrorNumber,
          ERROR_MESSAGE() as ErrorMessage;
          if(ERROR_NUMBER() = 547)
          BEGIN
			return -20 /*foriegn key constarint*/
          END
          else if(ERROR_NUMBER() = 2627)
          BEGIN
			return -30 /*unique key constarint*/
          END
          else
          BEGIN
			RETURN -40;
		  END
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[USP_INSERTTRACKING]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[USP_INSERTTRACKING]
(
	@userid varchar(max),
	@prdid varchar(max),
	@sessionid varchar(max),
	@brand varchar(max),
	@status varchar(max)
)AS
BEGIN
	DeCLARE @COUNT INT 
	select @COUNT = COUNT(*) from tbl_tracking where userid = @userid and prdid = @prdid and sessionid = @sessionid
	if(@count = 1)
	BEGIN
		return -1;
	END
	else
	BEGIN
		insert into tbl_tracking values (@userid,@prdid,@sessionid,@brand,@status,default)
		return 0;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertWishList]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_InsertWishList](
	@userid varchar(max),
	@prdid varchar(max)
)AS
BEGIN
	Declare @count int 
	Select @count = count(*) from tbl_WishList where userid = @userid and prdid = @prdid
	if(@count!=1)
	BEGIN
		insert into tbl_WishList values(@userid,@prdid,default,default);
		return 0;
	END
	
END

GO
/****** Object:  StoredProcedure [dbo].[usp_ProductInsertFromFlipkart]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_ProductInsertFromFlipkart]
As BEGIN
	Declare @count int;
	select @count = COUNT(*) from temp_Flipkart2;
	if(@count = 0)
	BEGIN
		return -1; --no product found
	END		
	else 
	BEGIN
	 while(@count >= 1)  
	 BEGIN  
		Declare @uniqueId varchar(max),@prdid varchar(max),@nm varchar(max),
		@desc varchar(max),@seoUrl varchar(max),@prdUrl varchar(max),
		@imgUrl varchar(max),@StoreId varchar(max) -- flipkart
		,@rPrice varchar(max),@offerPrice varchar(max),@scatid varchar(max),@status varchar(max),
		@c1 varchar(max)
		
		select @prdid = 'P'+CONVERT(varchar(10), right(newid(),10))
		
		Select top(1) @uniqueId = productId,@nm = title,@desc = description,
		@imgUrl = imageUrlStr,@rPrice= mrp,@offerPrice = price,
		@prdUrl = productUrl,@scatid = category,@c1 = productBrand,@status = inStock 
		from temp_Flipkart2
		
		if(@status = 'FALSE')
		BEGIN
			set @status = 'D'
		END
		ELSE 
		BEGIN
			set @status = 'A'
		END
		
		SET @imgUrl = SUBSTRING(@imgUrl,0,CHARINDEX(',',@imgUrl,0))
		IF(LEN(@imgUrl) = 0)
		BEGIN
			set @imgUrl = 'no-product.png'
		END
		else
		BEGIN
			SET @imgUrl = RIGHT(@imgUrl , LEN(@imgUrl)-1)
		end
		
		SET @rPrice = REPLACE(@rPrice,'"','');
		SET @rPrice = REPLACE(@rPrice,',INR',' Rs');
		
		
		SET @offerPrice = REPLACE(@offerPrice,'"','');
		SET @offerPrice = REPLACE(@offerPrice,',INR',' Rs');
		
		Select @StoreId = storeid from tbl_AffiliatedStores where storename = 'FLIPKART'
		
		SET @seoUrl =  dbo.ufn_SEOUrl(@nm);
		--@seourl
		insert into tbl_Products values(@uniqueId,@prdid,@nm,@desc,@seoUrl,@prdUrl,@prdUrl,@imgUrl,@StoreId,@rPrice,@offerPrice,@scatid,GETDATE(),GETDATE()+365,@status,@c1,'');
		
		Delete from temp_Flipkart2 where productId  = @uniqueId and title = @nm ;
		
		Select @count = COUNT(*) from temp_Flipkart2;
		
		print 'row inseret'
	end
	return 0;
	END
END

GO
/****** Object:  StoredProcedure [dbo].[usp_RechargeOrderGet]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[usp_RechargeOrderGet]
(
	@value varchar(max)
)AS
BEGIN
	select * from tbl_RechargeOrder where (@value = '' or @value = null or orderId = @value or email = @value or phone = @value);
	return 0;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_RechargeOrderInsert]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[usp_RechargeOrderInsert]
(
	@orderId varchar(20),
	@userid varchar(20),
	@phone varchar(20),
	@amount varchar(20),
	@info varchar(900),
	@opCode varchar(20)
)AS
BEGIN
	Declare @count int
	Declare @email varchar(max)
	Declare @status varchar(20) = 'PENDING'
	Select @count  = count(*) from tbl_AccountUser where id = @userid
	if(@count = 1)
	BEGIN
			Select @email = email from tbl_AccountUser where id = @userid
			select @count  = count(*) from tbl_RechargeOrder where phone = @phone and amount = @amount and userid = @userid and status = @status
			if(@count = 1)
			BEGIN
				return -2 --already exist
			END
			else
			BEGIN
				insert into tbl_RechargeOrder values (@orderId,@userid,@email,@phone,@amount,@info,@opCode,@status,getdate(),0,'');
				return 0;
			END
	END
	else
	BEGIN
		return -1 --invalid userid
	END
END

GO
/****** Object:  StoredProcedure [dbo].[usp_ReturnMoneyUrl]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_ReturnMoneyUrl]  
(  
 @source varchar(max),  
 @prdurl varchar(max),  
 @affurl varchar(max),  
 @affcode varchar(max),  
 @prdRUrl varchar(max) out  
)AS  
BEGIN  
   if(@source = 'Direct')  
    BEGIN  
     IF (@prdurl LIKE '%[?]%')  
     BEGIN  
      set @prdRUrl = @prdurl+'&'+@affcode  
     END  
     else   
     BEGIN  
      set @prdRUrl = @prdurl+'?'+@affcode  
     END  
    END  
    ELSE if(@source = 'OMGPM')  
    BEGIN  
     set @prdRUrl = @affurl+'?'+@affcode+'&redirect='+@prdurl  
    END  
    ELSE if(@source = 'PAYOOM')  
    BEGIN  
     set @prdRUrl = @affurl+'?'+@affcode+'&url='+@prdurl  
    END  
    ELSE if(@source = 'CUELINK')  
    BEGIN  
     set @prdRUrl = @affurl+'?'+@affcode+'&url='+@prdurl  
    END  
    else  
    BEGIN  
     set @prdRUrl = @prdurl  
    END  
END
GO
/****** Object:  StoredProcedure [dbo].[USP_SearchProductsByPagination]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[USP_SearchProductsByPagination]
(
	@indx int,
	@size int,
	@value varchar(30) = null
)AS
BEGIN
	SET NOCOUNT ON;
	Declare @startIndx int, @endIndx int;
	SET @startIndx = @indx;
	SET @endIndx = @indx + @size;
	--SET @nextIndx = @endIndx + 1;
	WITH CTEResults AS
	(
		Select ROW_NUMBER() OVER (order by rand(checksum(*))) AS RowNum, imageUrl,prdRedirectUrl,seourl,name, 
		subCatid,storeid,regularprice,offerprice,uniqueId,prdid,
		description,prdUrl from tbl_Products
		Where ((status = 'A') and FREETEXT(name, @value)) 
	)

SELECT * FROM CTEResults WHERE RowNum BETWEEN @startIndx AND @endIndx ;

END
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateProductImageData]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_UpdateProductImageData]
(
	@prdid varchar(max),
	@img varchar(max),
	@w varchar(max),
	@h varchar(max)
)AS
BEGIN
	update tbl_Products set imageurl = @img, width = @w,height = @h where uniqueId = @prdid
	return 0;
END
GO
/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[Split] (
      @InputString                  VARCHAR(8000),
      @Delimiter                    VARCHAR(50)
)

RETURNS @Items TABLE (
      Item                          VARCHAR(8000)
)

AS
BEGIN
      IF @Delimiter = ' '
      BEGIN
            SET @Delimiter = ','
            SET @InputString = REPLACE(@InputString, ' ', @Delimiter)
      END

      IF (@Delimiter IS NULL OR @Delimiter = '')
            SET @Delimiter = ','

--INSERT INTO @Items VALUES (@Delimiter) -- Diagnostic
--INSERT INTO @Items VALUES (@InputString) -- Diagnostic

      DECLARE @Item                 VARCHAR(8000)
      DECLARE @ItemList       VARCHAR(8000)
      DECLARE @DelimIndex     INT

      SET @ItemList = @InputString
      SET @DelimIndex = CHARINDEX(@Delimiter, @ItemList, 0)
      WHILE (@DelimIndex != 0)
      BEGIN
            SET @Item = SUBSTRING(@ItemList, 0, @DelimIndex)
            INSERT INTO @Items VALUES (@Item)

            -- Set @ItemList = @ItemList minus one less item
            SET @ItemList = SUBSTRING(@ItemList, @DelimIndex+1, LEN(@ItemList)-@DelimIndex)
            SET @DelimIndex = CHARINDEX(@Delimiter, @ItemList, 0)
      END -- End WHILE

      IF @Item IS NOT NULL -- At least one delimiter was encountered in @InputString
      BEGIN
            SET @Item = @ItemList
            INSERT INTO @Items VALUES (@Item)
      END

      -- No delimiters were encountered in @InputString, so just return @InputString
      ELSE INSERT INTO @Items VALUES (@InputString)

      RETURN

END -- End Function

GO
/****** Object:  UserDefinedFunction [dbo].[ufn_Decryption]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_Decryption](@EncrypteValue NVARCHAR(100))
RETURNS NVARCHAR(100)
AS
    BEGIN
        
        DECLARE @DecrypteValue NVARCHAR(100)
        DECLARE @Index INT
        DECLARE @Increment INT
        
         SET @DecrypteValue = ''
         SET @Index = 1
         SET @Increment = 128
         
         WHILE @Index <= LEN(@EncrypteValue)
            BEGIN
                SET @DecrypteValue = @DecrypteValue + 
                        CHAR(UNICODE(SUBSTRING(@EncrypteValue, @Index, 1)) - 
                        @Increment - @Index + 1)
                SET @Index = @Index + 1
            END
         
         RETURN @DecrypteValue
    END


GO
/****** Object:  UserDefinedFunction [dbo].[ufn_Encryption]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_Encryption](@Value as NVARCHAR(100))
RETURNS NVARCHAR(100)
AS

    BEGIN
        
        DECLARE @EncryptedValue NVARCHAR(100)
        DECLARE @Index INT
        DECLARE @Increment INT
        
         SET @EncryptedValue = ''
         SET @Index = 1
         SET @Increment = 128
         
        
        WHILE @Index <= LEN(@Value)
            BEGIN
                SET @EncryptedValue = @EncryptedValue + 
                                NCHAR(ASCII(SUBSTRING(@Value, @Index, 1)) +
                                @Increment + @Index - 1)
                 SET @Index = @Index + 1
            END
        
         RETURN @EncryptedValue
        
    END 


GO
/****** Object:  UserDefinedFunction [dbo].[ufn_SEOUrl]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_SEOUrl](@keywords varchar(max))
RETURNS varchar(max)
AS
BEGIN
		declare @name varchar(max) = @keywords
		SET @name = REPLACE(LTRIM(RTRIM(@name)),' ','-');
		WHILE PATINDEX( '%[~,@,#,$,+,-,%,&,*,(,),.,--]%', @name ) > 0 
		SET @name = Replace(REPLACE( @name, SUBSTRING( @name, PATINDEX( '%[~,@,#,$,+,-,%,&,*,(,),.,--]%', @name ), 1 ),''),'','')
		set @name = Lower(@name)
		SET @name = REPLACE(LTRIM(RTRIM(@name)),'--','-');
		SET @name = REPLACE(@name,'"','');
		SET @name = REPLACE(@name,'--','-');
		SET @name = REPLACE(@name,'/','-');
		SET @name = REPLACE(@name,'''','-');
        RETURN @name
END

GO
/****** Object:  UserDefinedFunction [dbo].[usp_GenerateUniqueSEOURL]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  FUNCTION [dbo].[usp_GenerateUniqueSEOURL]
(
	@tablename varchar(max) = 'P', --P- tbl_Product, C- tbl_Category, S - tbl_SubCategory , B- brand
	@name varchar(max)
)
RETURNS varchar(max)
AS
BEGIN
	DECLARE @count int;
	Declare @seourl varchar(max);
	Declare @uid varchar(max);
	SELECT @seourl = dbo.ufn_SEOUrl(@name);
	if(@tablename = 'P')
	BEGIN
		SELECT @uid = uniqueId from tbl_Products where seourl like @seourl+'%'
	END
	else if(@tablename = 'C')
	BEGIN
		SELECT @count = COUNT(*) from tbl_Category where categoryAlias like @seourl+'%'
	END
	else if(@tablename = 'S')
	BEGIN
		SELECT @count = COUNT(*) from tbl_Subcategory where  scatAlias like @seourl+'%'
	END
	else if(@tablename = 'B')
	BEGIN
		SELECT @count = COUNT(*) from tbl_AffiliatedStores where seourl = @seourl
	END
	if(@count > 1)
	BEGIN
		set @count = @count+1;
		set @seourl = @seourl +'-'+ @uid;
	END
	return @seourl;		
END


GO
/****** Object:  Table [dbo].[mobile_Operator]    Script Date: 12/8/2015 4:07:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mobile_Operator](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [varchar](900) NULL,
	[code] [varchar](900) NULL,
	[type] [varchar](100) NULL,
	[status] [char](1) NULL,
	[createdOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_AccountUser]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_AccountUser](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[username] [varchar](900) NOT NULL,
	[email] [varchar](900) NOT NULL,
	[password] [varchar](900) NOT NULL,
	[phone] [varchar](10) NULL,
	[isEmailVerfied] [char](1) NULL DEFAULT ('N'),
	[isPhoneVerfied] [char](1) NULL DEFAULT ('N'),
	[emailOTP] [varchar](max) NULL,
	[phoneOTP] [varchar](max) NULL,
	[name] [varchar](max) NULL,
	[dob] [varchar](max) NULL,
	[profile] [varchar](max) NULL,
	[loggedSource] [varchar](max) NULL,
	[userType] [varchar](20) NULL,
	[status] [char](1) NULL,
	[createdDate] [datetime] NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_AffiliatedStores]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_AffiliatedStores](
	[storeid] [varchar](100) NOT NULL,
	[storename] [varchar](500) NOT NULL,
	[seourl] [varchar](500) NOT NULL,
	[websiteLink] [varchar](500) NULL,
	[description] [varchar](max) NULL,
	[source] [varchar](max) NULL,
	[affUrl] [varchar](max) NULL,
	[affCode] [varchar](max) NULL,
	[follower] [bigint] NULL,
	[ratings] [float] NULL,
	[status] [char](1) NULL,
	[logo] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[storeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[storename] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[seourl] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_Category]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_Category](
	[catid] [varchar](20) NOT NULL,
	[catname] [varchar](500) NOT NULL,
	[catdesc] [varchar](max) NULL,
	[catimage] [varchar](max) NOT NULL,
	[cattaglist] [varchar](max) NULL,
	[catstatus] [char](1) NULL DEFAULT ('A'),
	[cathotstatus] [char](1) NULL DEFAULT ('N'),
	[catdoc] [datetime] NOT NULL DEFAULT (getdate()),
	[categoryAlias] [varchar](max) NOT NULL DEFAULT ('fokat-deals'),
PRIMARY KEY CLUSTERED 
(
	[catid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[catname] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_Coupon_CSV]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_Coupon_CSV](
	[VoucherCodeId] [bigint] NOT NULL,
	[Code] [varchar](max) NULL,
	[Title] [varchar](max) NULL,
	[Description] [varchar](max) NULL,
	[ActivationDate] [varchar](max) NULL,
	[ExpiryDate] [varchar](max) NULL,
	[TrackingUrl] [varchar](max) NULL,
	[CategoryName] [varchar](max) NULL,
	[Status] [varchar](max) NULL,
	[Addedon] [varchar](max) NULL,
	[Merchant] [varchar](max) NULL,
	[Product] [varchar](max) NULL,
	[Type] [varchar](max) NULL,
	[Discount] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[VoucherCodeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_ProductCategoryMapping]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_ProductCategoryMapping](
	[pcid] [bigint] IDENTITY(1,1) NOT NULL,
	[prdid] [varchar](20) NULL,
	[scatid] [varchar](20) NULL,
	[status] [char](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[pcid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_Products]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_Products](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[uniqueId] [varchar](900) NOT NULL,
	[prdid] [varchar](20) NOT NULL,
	[name] [varchar](max) NULL,
	[description] [varchar](max) NULL,
	[seourl] [varchar](max) NULL DEFAULT ('fokatdeals.com'),
	[prdUrl] [varchar](max) NULL DEFAULT ('fokat-deals'),
	[prdRedirectUrl] [varchar](max) NULL DEFAULT ('www.fokatdeals.com'),
	[imageurl] [varchar](max) NULL,
	[storeid] [varchar](500) NULL,
	[regularprice] [varchar](max) NULL DEFAULT ('0.0'),
	[offerprice] [varchar](max) NULL DEFAULT ('0.0'),
	[subCatid] [varchar](max) NULL,
	[createdon] [date] NULL DEFAULT (getdate()),
	[expiredate] [date] NULL DEFAULT (getdate()+(90)),
	[status] [char](1) NULL,
	[custom1] [varchar](900) NULL DEFAULT (NULL),
	[custom2] [varchar](900) NULL DEFAULT (NULL),
	[width] [varchar](20) NULL DEFAULT ('100%'),
	[height] [varchar](20) NULL DEFAULT ('100%'),
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[uniqueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[prdid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_RechargeOrder]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_RechargeOrder](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[orderId] [varchar](900) NOT NULL,
	[userid] [bigint] NULL,
	[email] [varchar](900) NULL,
	[phone] [varchar](900) NULL,
	[amount] [varchar](900) NULL,
	[info] [varchar](900) NULL,
	[opCode] [varchar](20) NULL,
	[status] [varchar](30) NULL,
	[dateOfRecharge] [datetime] NULL DEFAULT (getdate()),
	[discount] [varchar](30) NULL DEFAULT ((0)),
	[counponCode] [varchar](30) NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[orderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_Session]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_Session](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[sessionid] [varchar](900) NOT NULL,
	[ipaddress] [varchar](100) NOT NULL,
	[userid] [bigint] NULL,
	[username] [varchar](max) NULL,
	[status] [varchar](20) NULL,
	[creratedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[sessionid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ipaddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_Subcategory]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_Subcategory](
	[subcatid] [varchar](20) NOT NULL,
	[subcatname] [varchar](500) NOT NULL,
	[subcatdesc] [varchar](max) NULL,
	[subcatimage] [varchar](max) NOT NULL,
	[subcattaglist] [varchar](max) NULL,
	[subcatstatus] [char](1) NULL DEFAULT ('A'),
	[subcathotstatus] [char](1) NULL DEFAULT ('N'),
	[subcatdoc] [datetime] NOT NULL DEFAULT (getdate()),
	[catid] [varchar](20) NULL,
	[scatAlias] [varchar](max) NOT NULL DEFAULT ('fokat-deals'),
PRIMARY KEY CLUSTERED 
(
	[subcatid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[subcatname] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_tracking]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_tracking](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[userid] [varchar](max) NULL,
	[prdid] [varchar](max) NULL,
	[sessionid] [varchar](max) NULL,
	[brand] [varchar](max) NULL,
	[status] [varchar](max) NULL,
	[trackOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_WishList]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[tbl_WishList](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[userid] [bigint] NOT NULL,
	[prdid] [varchar](max) NULL,
	[code] [varchar](max) NULL,
	[createdDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[temp_Flipkart]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[temp_Flipkart](
	[productId] [varchar](500) NULL,
	[title] [varchar](max) NULL,
	[description] [varchar](max) NULL,
	[imageUrlStr] [varchar](max) NULL,
	[mrp] [varchar](max) NULL,
	[price] [varchar](max) NULL,
	[productUrl] [varchar](max) NULL,
	[category] [varchar](max) NULL,
	[productBrand] [varchar](max) NULL,
	[inStock] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[temp_Flipkart2]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[temp_Flipkart2](
	[productId] [varchar](500) NULL,
	[title] [varchar](max) NULL,
	[description] [varchar](max) NULL,
	[imageUrlStr] [varchar](max) NULL,
	[mrp] [varchar](max) NULL,
	[price] [varchar](max) NULL,
	[productUrl] [varchar](max) NULL,
	[category] [varchar](max) NULL,
	[productBrand] [varchar](max) NULL,
	[inStock] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[temp_SnapDeal]    Script Date: 12/8/2015 4:07:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[temp_SnapDeal](
	[productId] [varchar](500) NULL,
	[title] [varchar](max) NULL,
	[description] [varchar](max) NULL,
	[productBrand] [varchar](max) NULL,
	[productUrl] [varchar](max) NULL,
	[imageUrlStr] [varchar](max) NULL,
	[sid] [varchar](max) NULL,
	[scatname] [varchar](max) NULL,
	[cid] [varchar](max) NULL,
	[catname] [varchar](max) NULL,
	[price] [varchar](max) NULL,
	[mrp] [varchar](max) NULL,
	[inStock] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[mobile_Operator] ADD  DEFAULT ('A') FOR [status]
GO
ALTER TABLE [dbo].[mobile_Operator] ADD  DEFAULT (getdate()) FOR [createdOn]
GO
ALTER TABLE [dbo].[tbl_ProductCategoryMapping] ADD  DEFAULT ('A') FOR [status]
GO
ALTER TABLE [dbo].[tbl_Session] ADD  DEFAULT (NULL) FOR [userid]
GO
ALTER TABLE [dbo].[tbl_Session] ADD  DEFAULT (getdate()) FOR [creratedOn]
GO
ALTER TABLE [dbo].[tbl_tracking] ADD  DEFAULT (getdate()) FOR [trackOn]
GO
ALTER TABLE [dbo].[tbl_WishList] ADD  DEFAULT ('<i class="fa fa-heart"></i>') FOR [code]
GO
ALTER TABLE [dbo].[tbl_WishList] ADD  DEFAULT (getdate()) FOR [createdDate]
GO
ALTER TABLE [dbo].[tbl_ProductCategoryMapping]  WITH CHECK ADD FOREIGN KEY([prdid])
REFERENCES [dbo].[tbl_Products] ([prdid])
GO
ALTER TABLE [dbo].[tbl_ProductCategoryMapping]  WITH CHECK ADD FOREIGN KEY([scatid])
REFERENCES [dbo].[tbl_Subcategory] ([subcatid])
GO
ALTER TABLE [dbo].[tbl_Products]  WITH CHECK ADD  CONSTRAINT [STOREID_CONSTRAINT] FOREIGN KEY([storeid])
REFERENCES [dbo].[tbl_AffiliatedStores] ([storename])
GO
ALTER TABLE [dbo].[tbl_Products] CHECK CONSTRAINT [STOREID_CONSTRAINT]
GO
ALTER TABLE [dbo].[tbl_RechargeOrder]  WITH CHECK ADD FOREIGN KEY([userid])
REFERENCES [dbo].[tbl_AccountUser] ([id])
GO
ALTER TABLE [dbo].[tbl_Session]  WITH CHECK ADD FOREIGN KEY([userid])
REFERENCES [dbo].[tbl_AccountUser] ([id])
GO
ALTER TABLE [dbo].[tbl_Subcategory]  WITH CHECK ADD FOREIGN KEY([catid])
REFERENCES [dbo].[tbl_Category] ([catid])
GO
ALTER TABLE [dbo].[tbl_WishList]  WITH CHECK ADD FOREIGN KEY([userid])
REFERENCES [dbo].[tbl_AccountUser] ([id])
GO
ALTER TABLE [dbo].[tbl_AccountUser]  WITH CHECK ADD CHECK  (([isEmailVerfied]='N' OR [isEmailVerfied]='Y'))
GO
ALTER TABLE [dbo].[tbl_AccountUser]  WITH CHECK ADD CHECK  (([isPhoneVerfied]='N' OR [isPhoneVerfied]='Y'))
GO
ALTER TABLE [dbo].[tbl_AccountUser]  WITH CHECK ADD CHECK  (([status]='D' OR [status]='A'))
GO
ALTER TABLE [dbo].[tbl_AffiliatedStores]  WITH CHECK ADD CHECK  (([status]='D' OR [status]='A'))
GO
ALTER TABLE [dbo].[tbl_Category]  WITH CHECK ADD CHECK  (([cathotstatus]='N' OR [cathotstatus]='Y'))
GO
ALTER TABLE [dbo].[tbl_Category]  WITH CHECK ADD CHECK  (([cathotstatus]='N' OR [cathotstatus]='Y'))
GO
ALTER TABLE [dbo].[tbl_Category]  WITH CHECK ADD CHECK  (([cathotstatus]='N' OR [cathotstatus]='Y'))
GO
ALTER TABLE [dbo].[tbl_Category]  WITH CHECK ADD CHECK  (([catid] like 'C%'))
GO
ALTER TABLE [dbo].[tbl_Category]  WITH CHECK ADD CHECK  (([catid] like 'C%'))
GO
ALTER TABLE [dbo].[tbl_Category]  WITH CHECK ADD CHECK  (([catid] like 'C%'))
GO
ALTER TABLE [dbo].[tbl_Category]  WITH CHECK ADD CHECK  (([catstatus]='D' OR [catstatus]='A'))
GO
ALTER TABLE [dbo].[tbl_Category]  WITH CHECK ADD CHECK  (([catstatus]='D' OR [catstatus]='A'))
GO
ALTER TABLE [dbo].[tbl_Category]  WITH CHECK ADD CHECK  (([catstatus]='D' OR [catstatus]='A'))
GO
ALTER TABLE [dbo].[tbl_ProductCategoryMapping]  WITH CHECK ADD CHECK  (([status]='D' OR [status]='A'))
GO
ALTER TABLE [dbo].[tbl_Products]  WITH CHECK ADD CHECK  (([status]='D' OR [status]='A'))
GO
ALTER TABLE [dbo].[tbl_Session]  WITH CHECK ADD CHECK  (([status]='CLOSE' OR [status]='OPEN'))
GO
ALTER TABLE [dbo].[tbl_Subcategory]  WITH CHECK ADD CHECK  (([subcatid] like 'SC%'))
GO
ALTER TABLE [dbo].[tbl_Subcategory]  WITH CHECK ADD CHECK  (([subcatstatus]='D' OR [subcatstatus]='A'))
GO
ALTER TABLE [dbo].[tbl_Subcategory]  WITH CHECK ADD CHECK  (([subcathotstatus]='N' OR [subcathotstatus]='Y'))
GO
ALTER TABLE [dbo].[tbl_tracking]  WITH CHECK ADD CHECK  (([status]='PAID' OR [status]='INVALID' OR [status]='REJECT' OR [status]='APPROVED' OR [status]='PENDING'))
GO
USE [master]
GO
ALTER DATABASE [fd.com] SET  READ_WRITE 
GO
