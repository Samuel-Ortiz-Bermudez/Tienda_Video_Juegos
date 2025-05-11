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
	[Salario] DECIMAL(10,2),
	[Telefono] NVARCHAR (100),
);

CREATE TABLE [Compras] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[MetodoPago] NVARCHAR (100),
	[FechaVenta] DATE,
	[Total] DECIMAL (10,2),
	[Codigo] NVARCHAR (100),

	[Cliente] INT NOT NULL REFERENCES [Clientes]([Id]),
	[Empleado] INT NOT NULL REFERENCES [Empleados]([Id]),
);

CREATE TABLE [Videojuegos] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[Nombre] NVARCHAR (100),
	[Precio] DECIMAL (10,2),
	[Desarrolladora] NVARCHAR (100),
	[Codigo] NVARCHAR (100),
	[Estado] BIT
);

CREATE TABLE [DetallesCompras] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[Cantidad] INT,
	[Subtotal] DECIMAL (10,2),
	[Codigo] NVARCHAR (100),
	
	[Videojuego] INT NOT NULL REFERENCES [Videojuegos]([Id]),
	[Compra] INT NOT NULL REFERENCES [Compras]([Id]),
);

CREATE TABLE [Inventarios] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[Cantidad] INT,
	[Codigo] NVARCHAR (100),

	[Videojuego] INT NOT NULL REFERENCES [Videojuegos]([Id]),
);

CREATE TABLE [Proveedores] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[Nombre] NVARCHAR (100),
	[Direccion] NVARCHAR (100),
	[Telefono] NVARCHAR (100),
);

CREATE TABLE [Suministros] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[FechaSuministro] DATE,
	[Codigo] NVARCHAR (100),

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

INSERT INTO [Empleados] ([Nombre], [Cedula], [Salario], [Telefono]) 
VALUES 
('Juan', 'C006', 500, '126'),
('Esteban', 'C007', 450, '459'),
('Alex', 'C008', 650, '786');

INSERT INTO [Videojuegos] ([Nombre], [Desarrolladora], [Precio], [Codigo], [Estado]) 
VALUES 
('Factorio', 'Steam', 60, 'VJ001', 1),
('Satisfactory', 'Epicgames', 50, 'VJ002', 1),
('FIFA', 'EA', 50, 'VJ003', 1),
('Sims4', 'EA', 60, 'VJ004', 1),
('EFT', 'BSG', 120, 'VJ005', 1);

INSERT INTO [Compras] ([Cliente], [FechaVenta], [MetodoPago], [Empleado], [Total], [Codigo]) 
VALUES 
(1, '2025-02-24', 'Efectivo', 1, 220, 'C001'),
(2, '2025-02-25', 'Tarjeta', 2, 180, 'C002'),
(3, '2025-02-25', 'Tarjeta', 1, 120, 'C003'),
(4, '2025-02-25', 'Efectivo', 2, 110, 'C004'),
(5, '2025-02-27', 'Tarjeta', 3, 240, 'C005'),
(1, '2025-02-28', 'Tarjeta', 3, 290, 'C006');

INSERT INTO [DetallesCompras] ([Compra], [Videojuego], [Cantidad], [Subtotal], [Codigo]) 
VALUES 
(1, 1, 2, 120, 'DC001'),
(1, 2, 1, 50, 'DC002'),
(1, 3, 1, 50, 'DC003'),
(2, 4, 3, 180, 'DC004'),
(3, 5, 1, 120, 'DC005'),
(4, 3, 1, 50, 'DC006'),
(4, 1, 1, 60, 'DC007'),
(5, 5, 2, 240, 'DC008'),
(6, 3, 1, 50, 'DC009'),
(6, 5, 2, 240, 'DC010');

INSERT INTO [Inventarios] ([Videojuego], [Cantidad], [Codigo]) 
VALUES 
(1, 20, 'INV001'),
(2, 27, 'INV002'),
(3, 18, 'INV003'),
(4, 12, 'INV004'),
(5, 60, 'INV005');

INSERT INTO [Proveedores] ([Nombre], [Direccion], [Telefono]) 
VALUES 
('Steam', 'Calle 74', '741'),
('Eneba', 'Calle 21', '852'),
('G2A', 'Carrera 89', '963'),
('BGS', 'Avenida 16', '486');

INSERT INTO [Suministros] ([Proveedor], [Videojuego], [FechaSuministro], [Codigo]) 
VALUES 
(1, 1, '2020-01-20', 'SUM001'),
(2, 2, '2020-01-23', 'SUM002'),
(3, 3, '2020-01-24', 'SUM003'),
(4, 5, '2020-01-25', 'SUM004'),
(1, 4, '2020-01-26', 'SUM005');

Select * FROM [Videojuegos];
Select * FROM [Clientes];
Select * FROM [Compras];
Select * FROM [DetallesCompras];
Select * FROM [Proveedores];
Select * FROM [Suministros];
Select * FROM [Inventarios];
Select * FROM [Empleados];


