CREATE DATABASE db_tienda_videojuegos

GO
USE db_tienda_videojuegos;
GO

CREATE TABLE [Clientes] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[Nombre] NVARCHAR (100),
	[Cedula] NVARCHAR (100),
	[Direccion] NVARCHAR (100),
	[Telefono] NVARCHAR (100),
);

CREATE TABLE [Empleados] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[Nombre] NVARCHAR (100),
	[Cedula] NVARCHAR (100),
	[Salaio] DECIMAL(10,2),
	[Telefono] NVARCHAR (100),
);

CREATE TABLE [Compras] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[MetodoPago] NVARCHAR (100),
	[FechaVenta] DATE,
	[Total] DECIMAL (10,2),
	
	[Cliente] INT NOT NULL REFERENCES [Clientes]([Id]),
	[Empleado] INT NOT NULL REFERENCES [Empleados]([Id]),
);

CREATE TABLE [Videojuegos] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[Nombre] NVARCHAR (100),
	[Precio] DECIMAL (10,2),
	[Desarrolladora] NVARCHAR (100),
);

CREATE TABLE [DetallesCompras] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[Cantidad] INT,
	[Subtotal] DECIMAL (10,2),
	
	[Videojuego] INT NOT NULL REFERENCES [Videojuegos]([Id]),
	[Compra] INT NOT NULL REFERENCES [Compras]([Id]),
);



CREATE TABLE [Inventarios] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[Cantidad] INT,

	[Videojuego] INT NOT NULL REFERENCES [Videojuegos]([Id]),
);

CREATE TABLE [Proveedores] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[Nombre] NVARCHAR (100),
	[Direccion] NVARCHAR (100),
	[Telefono] NVARCHAR (100),
);

CREATE TABLE [Suministro] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[FechaSuministro] DATE,
	[Proveedor] INT NOT NULL REFERENCES [Proveedores]([Id]),
	[Videojuego] INT NOT NULL REFERENCES [Videojuegos]([Id]),
);




INSERT INTO [Clientes] ([Nombre], [Cedula], [Direccion], [Telefono]) 
VALUES 
('Andres', 'C001', 'Calle 13', '123'),
('Alejandra', 'C002', 'Carrera 24', '456'),
('Tomas', 'C003', 'Avenida 54', '789'),
('Miguel', 'C004', 'Calle 32', '159'),
('Sara', 'C005', 'Carrera 27', '753');

INSERT INTO [Empleados] ([Nombre], [Cedula], [Salaio], [Telefono]) 
VALUES 
('Juan', 'C006', 500, '126'),
('Esteban', 'C007', 450, '459'),
('Alex', 'C008', 650, '786');

INSERT INTO [Videojuegos] ([Nombre], [Desarrolladora], [Precio]) 
VALUES 
('Factorio', 'Steam', 60),
('Satisfactory', 'Epicgames', 50),
('FIFA', 'EA', 50),
('Sims4', 'EA', 60),
('EFT', 'BSG', 120);

INSERT INTO [Compras] ([Cliente], [FechaVenta], [MetodoPago], [Empleado], [Total]) 
VALUES 
(1, '2025-02-24', 'Efectivo', 1, 220),
(2, '2025-02-25', 'Tarjeta', 2, 180),
(3, '2025-02-25', 'Tarjeta', 1, 120),
(4, '2025-02-25', 'Efectivo', 2, 110),
(5, '2025-02-27', 'Tarjeta', 3, 240),
(1, '2025-02-28', 'Tarjeta', 3, 290);

INSERT INTO [DetallesCompras] ([Compra], [Videojuego], [Cantidad], [Subtotal]) 
VALUES 
(1, 1, 2, 120),
(1, 2, 1, 50),
(1, 3, 1, 50),
(2, 4, 3, 180),
(3, 5, 1, 120),
(4, 3, 1, 50),
(4, 1, 1, 60),
(5, 5, 2, 240),
(6, 3, 1, 50),
(6, 5, 2, 240);

INSERT INTO [Inventarios] ([Videojuego], [Cantidad]) 
VALUES 
(1, 20),
(2, 27),
(3, 18),
(4, 12),
(5, 60);

INSERT INTO [Proveedores] ([Nombre], [Direccion], [Telefono]) 
VALUES 
('Steam', 'Calle 74', '741'),
('Eneba', 'Calle 21', '852'),
('G2A', 'Carrera 89', '963'),
('BGS', 'Avenida 16', '486');

INSERT INTO [Suministro] ([Proveedor], [Videojuego], [FechaSuministro]) 
VALUES 
(1, 1, '2020-01-20'),
(2, 2, '2020-01-23'),
(3, 3, '2020-01-24'),
(4, 5, '2020-01-25'),
(1, 4, '2020-01-26');


Select * FROM [Videojuegos];
Select * FROM [Clientes]
Select * FROM [Compras]
Select * FROM [DetallesCompras];
Select * FROM [Proveedores];
Select * FROM [Suministro];
Select * FROM [Inventarios];

