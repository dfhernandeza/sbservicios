CREATE TABLE [dbo].[Personal] (
    [Id]       NVARCHAR (50) NOT NULL,
    [Nombre]   NVARCHAR (50) NULL,
    [Apellido] NVARCHAR (50) NULL,
    [Fono] NVARCHAR(50) NULL, 
    [Direccion] NVARCHAR(50) NULL, 
    [Email] NVARCHAR(50) NULL, 
    [FechaDeNacimiento] DATE NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

