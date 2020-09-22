-- Generates POCOs/models for all tables in the current database.
-- Useful for later processing e. g. with Dapper.
-- 
-- See https://stackoverflow.com/a/5873231/107625 for the original idea
-- See http://midnightprogrammer.net/post/use-sql-query-to-writecreate-a-file for the SP to write to file.
 
declare @TableName sysname
declare @Result nvarchar(MAX) = ''
 
 
DECLARE table_cursor CURSOR FOR
SELECT TABLE_NAME
FROM [INFORMATION_SCHEMA].[TABLES]
 
OPEN table_cursor
 
FETCH NEXT FROM table_cursor
INTO @tableName
 
WHILE @@FETCH_STATUS = 0
BEGIN
 
 
-- https://stackoverflow.com/a/5873231/107625
 
select @Result = @Result + '[Table(@"' + @TableName + '")]
public class ' + @TableName + '
{'
 
    select @Result = @Result + '
   public ' + ColumnType + NullableSign + ' ' + ColumnName + ' { get; set; }'
            from
            (
                select
                    replace(col.name, ' ', '_') ColumnName,
                    column_id ColumnId,
                    case typ.name
                        when 'bigint' then 'long'
                        when 'binary' then 'byte[]'
                        when 'bit' then 'bool'
                        when 'char' then 'string'
                        when 'date' then 'DateTime'
                        when 'datetime' then 'DateTime'
                        when 'datetime2' then 'DateTime'
                        when 'datetimeoffset' then 'DateTimeOffset'
                        when 'decimal' then 'decimal'
                        when 'float' then 'float'
                        when 'image' then 'byte[]'
                        when 'int' then 'int'
                        when 'money' then 'decimal'
                        when 'nchar' then 'string'
                        when 'ntext' then 'string'
                        when 'numeric' then 'decimal'
                        when 'nvarchar' then 'string'
                        when 'real' then 'double'
                        when 'smalldatetime' then 'DateTime'
                        when 'smallint' then 'short'
                        when 'smallmoney' then 'decimal'
                        when 'text' then 'string'
                        when 'time' then 'TimeSpan'
                        when 'timestamp' then 'DateTime'
                        when 'tinyint' then 'byte'
                        when 'uniqueidentifier' then 'Guid'
                        when 'varbinary' then 'byte[]'
                        when 'varchar' then 'string'
                        else 'UNKNOWN_' + typ.name
                    end ColumnType,
                    case
                        when col.is_nullable = 1 and typ.name in ('bigint', 'bit', 'date', 'datetime', 'datetime2', 'datetimeoffset', 'decimal', 'float', 'int', 'money', 'numeric', 'real', 'smalldatetime', 'smallint', 'smallmoney', 'time', 'tinyint', 'uniqueidentifier')
                        then '?'
                        else ''
                    end NullableSign
                from sys.columns col
                    join sys.types typ on
                        col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id
                where object_id = object_id(@TableName)
            ) t
            order by ColumnId
 
            set @Result = @Result  + '
}
           
'
 
 
    FETCH NEXT FROM table_cursor
    INTO @tableName
END
CLOSE table_cursor
DEALLOCATE table_cursor
 
---- http://midnightprogrammer.net/post/use-sql-query-to-writecreate-a-file
--EXEC USP_SaveFile @Result, 'C:\Ablage\POCOS.cs'
 
-- PRINT is truncated, therefore see the above saved file for the full POCOs.
print @Result