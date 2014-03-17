-----------------------------------------------------------------------------------------------------
---Platform 单点登录服务器单所必须的表结构。
---
------------------------------------------------------------------------------------------------------
---票据数据表。
if exists(select * from sysobjects where xtype = 'u' and name = 'tblSSOTicket')
begin
	print 'drop table tblSSOTicket'
	drop table tblSSOTicket
end
go
	print 'create table tblSSOTicket'
go
create table tblSSOTicket
(
	token			GUIDEx,--令牌，票据的唯一标识。
	userData		nvarchar(100),--用户数据，用于存储用户数据。
	issueDate		datetime default(getdate()),--令牌发布时间。
	expiration		datetime default(getdate()),--令牌过期时间。
	
	issueIP			nvarchar(50),--申请票据的客户机IP。
	renewalCount	int default(0),--票据续约次数。
	lastRenewalIP	nvarchar(50),--最后续约的客户机IP。
	hasValid as (case when expiration > getdate() then 1 else 0 end), --是否有效。
	
	constraint PK_tblSSOTicket_token primary key(token)
)
go
-----------------------------------------------------------------------------------------------------
--应用系统注册表
if exists(select * from sysobjects where xtype = 'u' and name = 'tblSysAppRegister')
begin 
	print 'drop table tblSysAppRegister'
	drop table tblSysAppRegister
end
go
	print 'create table tblSysAppRegister'
go
create table tblSysAppRegister
(
	SystemID			GUIDEx,--系统ID。
	SystemSign			nvarchar(255),--系统标识。
	SystemName			nvarchar(255),--系统名称。
	SystemURL			nvarchar(512),--系统URL。
	SystemPassword		nvarchar(512),--接入密码（明文）。
	SystemDescription	nvarchar(512),--描述。
	RegisterDate		datetime default(getdate()),--注册时间。
	
	constraint PK_tblSysAppRegister_SystemID primary key(SystemID),
	constraint UK_tblSysAppRegister_SystemSign unique(SystemSign)
)
go
-----------------------------------------------------------------------------------------------------
--用户帐号表
if exists(select * from sysobjects where xtype = 'u' and name = 'tblSysUserAccount')
begin 
	print 'drop table tblSysUserAccount'
	drop table tblSysUserAccount
end
go
	print 'create table tblSysUserAccount'
go
create table tblSysUserAccount
(
	UserID			GUIDEx,--用户ID。
	UserSign		GUIDEx null,--用户标识。
	UserName		nvarchar(50),--用户名称。
	UserEmail		nvarchar(512),--用户邮件地址。
	UserMobile		nvarchar(50),--用户手机号码。
	UserPassword	nvarchar(64),--用户密码。
	UserPassword2	nvarchar(64),--用户密码2。
	CreateUserDate	datetime default(getdate()),--创建用户时间。
	LastChangePasswordDate datetime default(getdate()),--最后一次修改密码的时间。
	
	constraint PK_tblSysUserAccount_UserID primary key(UserID),
	constraint UK_tblSysUserAccount_UserSign unique(UserSign)
)
go
-----------------------------------------------------------------------------------------------------
--
-----------------------------------------------------------------------------------------------------
  

 