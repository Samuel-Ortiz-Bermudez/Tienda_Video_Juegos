
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
	[FechaVenta] DATETIME,
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
	[Estado] BIT,
	[ImagenUrl] NVARCHAR (100)
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

CREATE TABLE [CuentasEmpleados] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[Correo] NVARCHAR (100),
	[Contrasena] NVARCHAR (200),
	[Rol] NVARCHAR (100),
	
	[Empleado] INT NOT NULL REFERENCES [Empleados]([Id])
)

CREATE TABLE [CuentasClientes] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[Correo] NVARCHAR (100),
	[Contrasena] NVARCHAR (200),
	
	[Cliente] INT NOT NULL REFERENCES [Clientes]([Id])
)

INSERT INTO [Clientes] ([Nombre], [Cedula], [Direccion], [Telefono]) 
VALUES 
('Andres', '231542', 'Calle 13', '123'),
('Alejandra', '326541', 'Carrera 24', '456'),
('Tomas', '74125', 'Avenida 54', '789'),
('Miguel', '124578', 'Calle 32', '159'),
('Sara', '12356', 'Carrera 27', '753');

INSERT INTO [Empleados] ([Nombre], [Cedula], [Salario], [Telefono]) 
VALUES 
('Juan', '006', 500, '126'),
('Esteban', '007', 450, '459'),
('Alex', '008', 650, '786'),
('Admin', '009', 1000, '1234')

INSERT INTO [Videojuegos] ([Nombre], [Desarrolladora], [Precio], [Codigo], [Estado], [ImagenUrl]) 
VALUES 
('Factorio', 'Steam', 60, 'VJ-001', 1, '/images/factorio.jpg'),
('Satisfactory', 'Epicgames', 50, 'VJ-002', 1, '/images/satisfactory.jpeg'),
('FIFA', 'EA', 50, 'VJ-003', 1, '/images/fifa.jpg'), 
('Sims4', 'EA', 60, 'VJ-004', 1, '/images/sims4.jpg'),
('EFT', 'BSG', 120, 'VJ-005', 1,'/images/Tarkov.jpg'), 
('Bully', 'Rockstar Games', 40, 'VJ-006', 1, '/images/bully.jpg'),
('GTA 6', 'Rockstar Games', 80, 'VJ-008', 1, '/images/gta6.jpg');




INSERT INTO [Compras] ([Cliente], [FechaVenta], [MetodoPago], [Empleado], [Total], [Codigo]) 
VALUES 
(1, '2025-02-24', 'Efectivo', 1, 220, 'C-001'),
(2, '2025-02-25', 'Tarjeta', 2, 180, 'C-002'),
(3, '2025-02-25', 'Tarjeta', 1, 120, 'C-003'),
(4, '2025-02-25', 'Efectivo', 2, 110, 'C-004'),
(5, '2025-02-27', 'Tarjeta', 3, 240, 'C-005'),
(1, '2025-02-28', 'Tarjeta', 3, 290, 'C-006');

INSERT INTO [DetallesCompras] ([Compra], [Videojuego], [Cantidad], [Subtotal], [Codigo]) 
VALUES 
(1, 1, 2, 120, 'DC-001'),
(1, 2, 1, 50, 'DC-002'),
(1, 3, 1, 50, 'DC-003'),
(2, 4, 3, 180, 'DC-004'),
(3, 5, 1, 120, 'DC-005'),
(4, 3, 1, 50, 'DC-006'),
(4, 1, 1, 60, 'DC-007'),
(5, 5, 2, 240, 'DC-008'),
(6, 3, 1, 50, 'DC-009'),
(6, 5, 2, 240, 'DC-010');

INSERT INTO [Inventarios] ([Videojuego], [Cantidad], [Codigo]) 
VALUES 
(1, 20, 'INV-001'),
(2, 27, 'INV-002'),
(3, 18, 'INV-003'),
(4, 12, 'INV-004'),
(5, 60, 'INV-005');

INSERT INTO [Proveedores] ([Nombre], [Direccion], [Telefono]) 
VALUES 
('Steam', 'Calle 74', '741'),
('Eneba', 'Calle 21', '852'),
('G2A', 'Carrera 89', '963'),
('BGS', 'Avenida 16', '486');

INSERT INTO [Suministros] ([Proveedor], [Videojuego], [FechaSuministro], [Codigo]) 
VALUES 
(1, 1, '2020-01-20', 'SUM-001'),
(2, 2, '2020-01-23', 'SUM-002'),
(3, 3, '2020-01-24', 'SUM-003'),
(4, 5, '2020-01-25', 'SUM-004'),
(1, 4, '2020-01-26', 'SUM-005');

INSERT INTO [CuentasClientes] ( [Correo], [Contrasena], [Cliente])
VALUES 
( 'andres@gmail.com', 'AQAAAAEAACcQAAAAEHlVZjqXsw5aJ3Xr7mKknHXgOqOVSWhznF+gC2iLW8iZVNzl+bw3Yz425Rg9G8Ii+g==', 1),
( 'alejandra@gmail.com', 'AQAAAAEAACcQAAAAENcV6sgzXgLOI0HN1aJpnXZPOxIQ/JzNqNtRs6cDw8rS8NqXNeFGL9iA8MtxJHsA/A==', 2),
( 'tomas@gmail.com', 'AQAAAAEAACcQAAAAEOzIILRRNe9Fzz8CiyTAqDyI4o/f/wDuTxcY8WXOElVtzQiG4xZNpGrZMtxCueUarQ==', 3),
( 'miguel@gmail.com', 'AQAAAAEAACcQAAAAEIBLUx4cDvBVGxsGJamlpVBM7w8FiUudcaiyo5jwXp4RvtkS1Kmh2fUgIKbxEL34bQ==', 4),
( 'sara@gmail.com', 'AQAAAAEAACcQAAAAEA2Cbo1qyOwsHEv47ot+krXeqoOeitdZuxsp+GbHPncBUsKswZXAW2mFpQT9NGAMNw==', 5);

INSERT INTO [CuentasEmpleados] ( [Correo], [Contrasena], [Empleado], [Rol])
VALUES 
( 'juan@tienda.com', 'AQAAAAEAACcQAAAAEMqDcz4ZR4qNOiem+wxan6GmganLw1sq9c7rfrxlZ3sBg3ktzw+h6G/JHW3RjWhWEA==', 1, 'Admin'),
( 'esteban@tienda.com', 'AQAAAAEAACcQAAAAELhuQ1pqTGtHrueGvV/25ltquoJNHrJKJ9PgII52kg2ROcPIJUC7xPYCE6UKr+gLRg==', 2, 'Empleado'),
( 'Alex@tienda.com', 'AQAAAAEAACcQAAAAEN/O8/rmxZMIYB3d47cpi3FAWXnsoxydsthMOdZUg6U+S0l73ldH5wA28BsGgamimw==', 3, 'Empleado'),
( 'Admin@tienda.com', 'AQAAAAEAACcQAAAAEIEcE4wggzSYsJoUm/B5bgxgFEDzPIaAfrkvlWIaYgmOXKjYMzuDMzU2uPh2HGtQ+Q==', 4, 'Admin');

Select * FROM [Videojuegos];
Select * FROM [Clientes];
Select * FROM [Compras];
Select * FROM [DetallesCompras];
Select * FROM [Proveedores];
Select * FROM [Suministros];
Select * FROM [Inventarios];
Select * FROM [Empleados];
Select * FROM [CuentasClientes];
Select * FROM [CuentasEmpleados];

--Tablas de auditoria

CREATE TABLE [Auditorias] (
	[Id] INT PRIMARY KEY IDENTITY (1,1),
	[Fecha] DATETIME,
	[Accion] NVARCHAR (50),
	[Tabla]NVARCHAR (50),
)
