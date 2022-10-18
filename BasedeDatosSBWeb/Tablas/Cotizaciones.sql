CREATE TABLE [dbo].[Cotizaciones]
(
	[IDCotizacion] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Fecha] DATE NOT NULL, 
    [Servicio] NVARCHAR(255) NULL, 
    [TotalMateriales] FLOAT NULL, 
    [TotalPersonal] FLOAT NULL, 
    [TotalSeguridad] FLOAT NULL, 
    [TotalEquipos] FLOAT NULL, 
    [Total] FLOAT NULL, 
    [Cliente] NVARCHAR(50) NULL, 
    [Supervisor] NVARCHAR(50) NULL, 
    [Detalles] NVARCHAR(MAX) NULL, 
    [TiempoEjecucion] TIME NULL
)
