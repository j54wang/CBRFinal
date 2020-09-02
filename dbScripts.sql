CREATE TABLE [dbo].[Manager](
 [ManID] [varchar] (100) NOT NULL
)
 
 ALTER TABLE [dbo].[Manager] ADD  CONSTRAINT [PK_Manager] PRIMARY KEY CLUSTERED
 ([ManID] ASC)
 GO
 
 CREATE TABLE [dbo].[Emp](
 [EmpID] [varchar] (100) NOT NULL,
 [FirstName] [varchar](100) NOT NULL,
 [LastName] [varchar](100) NOT NULL,
 [Manager] [varchar](100) NULL)
 
 ALTER TABLE [dbo].[Emp] ADD  CONSTRAINT [PK_Emp] PRIMARY KEY CLUSTERED
 ([EmpID] ASC)
 GO
