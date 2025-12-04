-- Проверим наличие важных полей в Products
SELECT 
    CASE WHEN EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Products' AND COLUMN_NAME = 'IsDeleted') 
         THEN 'Есть' ELSE 'Нет' END as IsDeleted_Exists,
    CASE WHEN EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Products' AND COLUMN_NAME = 'CreatedAt') 
         THEN 'Есть' ELSE 'Нет' END as CreatedAt_Exists,
    CASE WHEN EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Products' AND COLUMN_NAME = 'UpdatedAt') 
         THEN 'Есть' ELSE 'Нет' END as UpdatedAt_Exists;
GO