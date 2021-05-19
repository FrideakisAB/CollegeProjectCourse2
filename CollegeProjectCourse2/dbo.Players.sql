CREATE TABLE [dbo].[Players]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [name] NVARCHAR(80) NOT NULL, 
    [history] NVARCHAR(80) NOT NULL, 
    [legend] NVARCHAR(300) NOT NULL, 
    [forces] NVARCHAR(35) NOT NULL, 
    [family] NVARCHAR(35) NOT NULL, 
    [gender] BIT NOT NULL, 
    [fireball] BIT NOT NULL, 
    [vampire] BIT NOT NULL, 
    [image] INT NOT NULL
)
