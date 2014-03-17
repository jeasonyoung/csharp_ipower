---------------------------------------------------------------------------------------
---Jeason Platform 自定义用户类型。
---------------------------------------------------------------------------------------
---GUIDEx 
if exists (select * from systypes where name = 'GUIDEx')
begin
	print 'drop type GUIDEx'
	exec sp_droptype N'GUIDEx'
end
go
print 'create type GUIDEx'

EXEC sp_addtype 'GUIDEx', 'nvarchar (32)', 'not null'
GO
----------------------------------------------------------------------------------------