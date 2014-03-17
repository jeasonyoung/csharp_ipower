 ---------------------------------------------------------------------------------------
 ----  Jeason Platform 表结构.
 ---------------------------------------------------------------------------------------
 --枚举表结构
 if exists(select * from sysobjects where xtype = 'u' and name='tblCommonEnums')
 begin
	print 'drop table tblCommonEnums'
	drop table tblCommonEnums
 end
 go
	print 'create table tblCommonEnums'
 go
 create table tblCommonEnums
 (
	EnumName	nvarchar(256),--枚举名称.
	Member		nvarchar(50),--枚举成员
	MemberName	nvarchar(256),--成员中文名称
	IntValue	int,		  --成员值
	OrderNo		int,		  --序号
	
	constraint PK_tblCommonEnums primary key(EnumName,Member)
 )
 go
 --------------------------------------------------------------------------------------
 --------------------------------------------------------------------------------------