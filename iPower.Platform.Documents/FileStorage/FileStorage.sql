/*
//================================================================================
//  FileName: FileStorage_Tables.sql
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/22
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
//  ----    ------  -----------------
//
//================================================================================
//  Copyright (C) 2004-2009 Jeason Young Corporation
//================================================================================
*/
------------------------------------------------------------------------------------------------------------------
--文件存储到数据库中的表结构。
if exists(select 0 from sysobjects where xtype = 'u' and name = 'tblPlatformFileStorage')
begin
	print 'drop table tblPlatformFileStorage'
	drop table tblPlatformFileStorage
end
go
	print 'create table tblPlatformFileStorage'
go
create table tblPlatformFileStorage
(
	FileID			nvarchar(32),--文件ID。
	FileName		nvarchar(128),--文件名称。
	ContentType		nvarchar(128),--MIME内容类型。
	FileSize		float default(0),--文件大小。
	Suffix			nvarchar(10),--文件后缀名。
	CheckCode		nvarchar(32),--校验码。
	LastModify		datetime default(getdate()),--最近修改时间。
	
	FileContent		image,--文件数据。
	
	constraint PK_tblPlatformFileStorage primary key(FileID) --主键约束。
)
go
------------------------------------------------------------------------------------------------------------------
---文件存储操作存储过程。
if exists(select 0 from sysobjects where xtype = 'p' and name = 'spPlatformFileStorage')
begin
	print 'drop procedure spPlatformFileStorage'
	drop procedure spPlatformFileStorage
end
go
	print 'create procedure spPlatformFileStorage'
go
create procedure spPlatformFileStorage
(
	@FileID			nvarchar(32),--文件ID。
	@FileName		nvarchar(128),--文件名称。
	@ContentType	nvarchar(128),--MIME内容类型。
	@FileSize		float,--文件大小。
	@Suffix			nvarchar(10),--文件后缀名。
	@CheckCode		nvarchar(32),--文件校验码。
	@FileContent	image=null -- 文件数据。
)
as
begin
	--判断文件是否存在。
	if(exists(select 0 from tblPlatformFileStorage where FileID = @FileID))
	begin
		--判断数据是否更新。
		if(not exists(select 0 from tblPlatformFileStorage where FileID = @FileID and CheckCode = @CheckCode))
		begin
			--更新数据。
			if(isnull(@FileName,'') <> '')
			begin
				update tblPlatformFileStorage
				set	FileName = @FileName,ContentType = @ContentType,  FileSize = @FileSize,Suffix = @Suffix,CheckCode = @CheckCode, LastModify = getdate(),
					FileContent = @FileContent
				where FileID = @FileID
			end else begin
				update tblPlatformFileStorage
				set ContentType = @ContentType,  FileSize = @FileSize,CheckCode = @CheckCode, LastModify = getdate(),
					FileContent = @FileContent
				where FileID = @FileID
			end
		end
	end else begin
		--插入新数据。
		insert into tblPlatformFileStorage(FileID,FileName,ContentType,FileSize,Suffix,CheckCode,LastModify,FileContent)
		values(@FileID,@FileName,@ContentType,@FileSize,@Suffix,@CheckCode,getdate(),@FileContent)
	end
end
go
------------------------------------------------------------------------------------------------------------------
--删除文件。
if exists(select 0 from sysobjects where xtype = 'p' and name = 'spPlatformDeleteFileStorage')
begin
	print 'drop procedure spPlatformDeleteFileStorage'
	drop procedure spPlatformDeleteFileStorage
end
go	
	print 'create procedure spPlatformDeleteFileStorage'
go
create procedure spPlatformDeleteFileStorage
(
	@FileID	nvarchar(32)--文件ID。
)
as
begin
	delete from tblPlatformFileStorage where FileID = @FileID
end
go
------------------------------------------------------------------------------------------------------------------
--下载文件数据。
if exists(select 0 from sysobjects where xtype = 'p' and name = 'spPlatformFileStorageDownload')
begin
	print 'drop procedure spPlatformFileStorageDownload'
	drop procedure spPlatformFileStorageDownload
end
go
	print 'create procedure spPlatformFileStorageDownload'
go
create procedure spPlatformFileStorageDownload
(
	@FileID	nvarchar(32)--文件ID。
)
as
begin
	select case when charindex(Suffix,FileName) > 0 then FileName else FileName + Suffix end as FullFileName,
			ContentType,FileContent
	from tblPlatformFileStorage
	where FileID = @FileID
end
go
------------------------------------------------------------------------------------------------------------------
