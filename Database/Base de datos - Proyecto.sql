-- Tablas
--Version 2 Pivote
CREATE TABLE Pivote(
	ID varbinary(8000) NOT NULL,
	Descripcion nvarchar(max) NOT NULL,
	Consecutivo varbinary(max) NOT NULL,
	Prefijo nvarchar(max) NOT NULL,
	RangoInicial varbinary(max) NOT NULL,
	RangoFinal varbinary(max) NOT NULL,
	CONSTRAINT PivotePK  PRIMARY KEY (ID)
);

--Pregunta_Seguridad
CREATE TABLE Pregunta_Seguridad(
	Id_Pregunta int NOT NULL, 
	Descripcion_Pregunta NVARCHAR(max) NOT NULL
	CONSTRAINT PreguntaPK PRIMARY KEY (Id_Pregunta)
);

--Roles
CREATE TABLE Roles(
	Id_Rol int NOT NULL,
	Nombre_Rol NVARCHAR(max) NOT NULL, 
	Descripcion_Rol NVARCHAR(max) NOT NULL
	CONSTRAINT RolPK PRIMARY KEY (Id_Rol)
);

--Tipo de vuelo
CREATE TABLE Tipo_Vuelo(
	Id_Tipo int NOT NULL,
	Descripcion_Tipo NVARCHAR(max) NOT NULL
	CONSTRAINT TipoPK PRIMARY KEY (Id_Tipo)
);

--Aerolinea
CREATE TABLE Aerolinea(
	ID_Aerolinea NVARCHAR(5), 
	Siglas_Aerolinea NVARCHAR(max) NOT NULL, 
	Descripcion_Aerolinea NVARCHAR(max),
	Imagen_Aerolinea NVARCHAR(max),
	CONSTRAINT AerolinaPK PRIMARY KEY (ID_Aerolinea)
);

--Puertas_Aeropuerto
CREATE TABLE Puertas_Aeropuerto (
	ID_Puerta NVARCHAR(6), 
	Numero_Puerta NVARCHAR(max),
	Estado_Puerta NVARCHAR(max)
	CONSTRAINT PuertasPK PRIMARY KEY (ID_Puerta)
);

--Paises
CREATE TABLE Paises (
  Id_Pais NVARCHAR(5) NOT NULL,
  Siglas_Pais NVARCHAR(max) NOT NULL, 
  Descripcion_Pais NVARCHAR(max) NOT NULL,
  Imagen_Pais NVARCHAR(max) NOT NULL,
  CONSTRAINT PaisesPK PRIMARY KEY (Id_Pais)
);

--Vuelos
CREATE TABLE Vuelos(
	Id_Vuelo NVARCHAR(8) NOT NULL,
	Aerolinea NVARCHAR(5) NOT NULL, 
	Id_Pais NVARCHAR(5) NOT NULL, 
	Fecha_Vuelo NVARCHAR(max) NOT NULL, 
	Hora_Vuelo NVARCHAR(max) NOT NULL, 
	Estado_Vuelo NVARCHAR(max) NOT NULL, 
	Precio_Vuelo varbinary(8000) NOT NULL, 
	Puerta_Vuelo NVARCHAR(6) NOT NULL,
	Tipo_Vuelo int NOT NULL,
	CONSTRAINT VuelosPK PRIMARY KEY (Id_Vuelo),
	FOREIGN KEY (Aerolinea) REFERENCES Aerolinea (Id_Aerolinea),
	FOREIGN KEY (Puerta_Vuelo) REFERENCES Puertas_Aeropuerto (ID_Puerta),
	FOREIGN KEY (Id_Pais) REFERENCES Paises (Id_Pais),
	FOREIGN KEY (Tipo_Vuelo) REFERENCES Tipo_Vuelo (Id_Tipo)
);

--Usuarios
CREATE TABLE Usuarios (
  Id_Usuario NVARCHAR(5) NOT NULL,
  Primer_Apellido NVARCHAR(max) NOT NULL,
  Segundo_Apellido NVARCHAR(max),
  Nombre NVARCHAR(max) NOT NULL,
  Usuario NVARCHAR(max) NOT NULL, 
  Correo_Usuario NVARCHAR(max) NOT NULL,
  Password_Usuario NVARCHAR(max) NOT NULL,
  Id_Pregunta int NOT NULL,
  Respuesta_Seguridad NVARCHAR(max) NOT NULL,
  Id_Rol int NOT NULL,
  CONSTRAINT TarjetaK PRIMARY KEY (Id_Usuario),
  FOREIGN KEY (Id_Pregunta) REFERENCES Pregunta_Seguridad(Id_Pregunta),
  FOREIGN KEY (Id_Rol) REFERENCES Roles (Id_Rol)
);

--Rerservaciones
CREATE TABLE Reservaciones (
  Id_Reservacion NVARCHAR(5) NOT NULL,
  Id_Usuario NVARCHAR(5) NOT NULL,
  Id_Vuelo NVARCHAR(8) NOT NULL,
  Numero_Boletos varbinary(8000) NOT NULL,
  Total varbinary(8000),
  Estado_Reservacion NVARCHAR(max) NOT NULL,
  CONSTRAINT ReservacionPK PRIMARY KEY (Id_Reservacion),
  FOREIGN KEY (Id_Usuario) REFERENCES Usuarios (Id_Usuario),
  FOREIGN KEY (Id_Vuelo) REFERENCES Vuelos (Id_Vuelo)
);


--Tarjeta
CREATE TABLE Tarjeta (
  Id_RegistroTarjeta NVARCHAR(5) NOT NULL,
  Id_Usuario NVARCHAR(5) NOT NULL, 
  Numero_Tarjeta varbinary(max) NOT NULL,
  Mes_Exp varbinary(max) NOT NULL,
  Year_Exp varbinary(max) NOT NULL,
  CVV varbinary(max)  NOT NULL,
  Tipo_Tarjeta varbinary(max) NOT NULL,
  CONSTRAINT TarjetaPK PRIMARY KEY (Id_RegistroTarjeta),
  FOREIGN KEY (Id_Usuario) REFERENCES Usuarios (Id_Usuario)
);


--Bitacora
CREATE TABLE Bitacora(
	Id_Bitacora NVARCHAR(5) NOT NULL, 
	Id_Usuario NVARCHAR(5) NOT NULL, 
	Fecha VARBINARY(max) NOT NULL, 
	Tipo_Movimiento NVARCHAR(max) NOT NULL, 
	Descripcion_Movimiento NVARCHAR(max) NOT NULL
	CONSTRAINT BitacoraPK PRIMARY KEY (Id_Bitacora),
	FOREIGN KEY (Id_Usuario) REFERENCES Usuarios (Id_Usuario)
);



-- Inserts
--1. Pivote
--Aerolinea
INSERT Pivote
(ID, Descripcion, Consecutivo, Prefijo, RangoInicial, RangoFinal) 
VALUES(
	EncryptByPassPhrase('ServiciosWeb2023.','1'),
	EncryptByPassPhrase('ServiciosWeb2023.','Aerolinea'),
	EncryptByPassPhrase('ServiciosWeb2023.', '7'),
	EncryptByPassPhrase('ServiciosWeb2023.','AE'),
	EncryptByPassPhrase('ServiciosWeb2023.', '1'),
	EncryptByPassPhrase('ServiciosWeb2023.', '10')
);

--Paises
INSERT Pivote
(ID, Descripcion, Consecutivo, Prefijo, RangoInicial, RangoFinal) 
VALUES(
	EncryptByPassPhrase('ServiciosWeb2023.', '2'),
	EncryptByPassPhrase('ServiciosWeb2023.','Paises'),
	EncryptByPassPhrase('ServiciosWeb2023.', '6'),
	EncryptByPassPhrase('ServiciosWeb2023.','PA'),
	EncryptByPassPhrase('ServiciosWeb2023.', '1'),
	EncryptByPassPhrase('ServiciosWeb2023.', '10')
);

--Puertas
INSERT Pivote
(ID, Descripcion, Consecutivo, Prefijo, RangoInicial, RangoFinal) 
VALUES(
	EncryptByPassPhrase('ServiciosWeb2023.','3'),
	EncryptByPassPhrase('ServiciosWeb2023.','Puertas_Aeropuerto'),
	EncryptByPassPhrase('ServiciosWeb2023.', '7'),
	EncryptByPassPhrase('ServiciosWeb2023.','PUA'),
	EncryptByPassPhrase('ServiciosWeb2023.', '1'),
	EncryptByPassPhrase('ServiciosWeb2023.', '10')
);

--Vuelos
INSERT Pivote
(ID, Descripcion, Consecutivo, Prefijo, RangoInicial, RangoFinal) 
VALUES(
	EncryptByPassPhrase('ServiciosWeb2023.','4'),
	EncryptByPassPhrase('ServiciosWeb2023.','Vuelos'),
	EncryptByPassPhrase('ServiciosWeb2023.', '6'),
	EncryptByPassPhrase('ServiciosWeb2023.','Aerolinea'),
	EncryptByPassPhrase('ServiciosWeb2023.', '1'),
	EncryptByPassPhrase('ServiciosWeb2023.', '100')
);

--Reservaciones
INSERT Pivote
(ID, Descripcion, Consecutivo, Prefijo, RangoInicial, RangoFinal) 
VALUES(
	EncryptByPassPhrase('ServiciosWeb2023.','5'),
	EncryptByPassPhrase('ServiciosWeb2023.','Reservaciones'),
	EncryptByPassPhrase('ServiciosWeb2023.', '6'),
	EncryptByPassPhrase('ServiciosWeb2023.','RES'),
	EncryptByPassPhrase('ServiciosWeb2023.', '1'),
	EncryptByPassPhrase('ServiciosWeb2023.', '100')
);

--2. Pregunta_Seguridad
INSERT INTO Pregunta_Seguridad (Id_Pregunta, Descripcion_Pregunta)
VALUES (
	1,
	EncryptByPassPhrase('ServiciosWeb2023.', '¿Dónde creció?')
);

INSERT INTO Pregunta_Seguridad (Id_Pregunta, Descripcion_Pregunta)
VALUES (
	2,
	EncryptByPassPhrase('ServiciosWeb2023.','¿Cómo se llama su mascota?')
);

INSERT INTO Pregunta_Seguridad (Id_Pregunta, Descripcion_Pregunta)
VALUES (
	3,
	EncryptByPassPhrase('ServiciosWeb2023.', '¿Cuál es su color favorito?')
);

INSERT INTO Pregunta_Seguridad (Id_Pregunta, Descripcion_Pregunta)
VALUES (
	4,
	EncryptByPassPhrase('ServiciosWeb2023.', '¿Cuántos hermanos tienes?')
);

INSERT INTO Pregunta_Seguridad (Id_Pregunta, Descripcion_Pregunta)
VALUES (
	5,
	EncryptByPassPhrase('ServiciosWeb2023.','¿Cómo se llama la escuela a la cual asistias?')
);


--3. Roles
INSERT INTO Roles (Id_Rol, Nombre_Rol, Descripcion_Rol) 
VALUES (
	1, 
	EncryptByPassPhrase('ServiciosWeb2023.','Administrador'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Acceso a todas las funcionalidades del sistema.')
);

INSERT INTO Roles (Id_Rol, Nombre_Rol, Descripcion_Rol) 
VALUES (
	2,
	EncryptByPassPhrase('ServiciosWeb2023.','Seguridad'),
	EncryptByPassPhrase('ServiciosWeb2023.','Acceso solamente a la parte de creación de nuevos usuarios y ver los usuarios.')
);

INSERT INTO Roles (Id_Rol, Nombre_Rol, Descripcion_Rol) 
VALUES (
	3,
	EncryptByPassPhrase('ServiciosWeb2023.','Consecutivo'),
	EncryptByPassPhrase('ServiciosWeb2023.','Acceso solamente a la parte de consecutivos.')
);

INSERT INTO Roles (Id_Rol, Nombre_Rol, Descripcion_Rol) 
VALUES (
	4,
	EncryptByPassPhrase('ServiciosWeb2023.', 'Mantenimiento'),
	EncryptByPassPhrase('ServiciosWeb2023.', 'Acceso a crear, modificar y eliminar registros del menú administración.')
);

INSERT INTO Roles (Id_Rol, Nombre_Rol, Descripcion_Rol) 
VALUES (
	5, 
	EncryptByPassPhrase('ServiciosWeb2023.','Consulta'),
	EncryptByPassPhrase('ServiciosWeb2023.','Acceso solamente a las consultas que posea el sistema.')
);

INSERT INTO Roles (Id_Rol, Nombre_Rol, Descripcion_Rol) 
VALUES (
	6, 
	EncryptByPassPhrase('ServiciosWeb2023.','Cliente'),
	EncryptByPassPhrase('ServiciosWeb2023.','Acceso solamente al apartado de clientes')
);

--4. Tipo Vuelo
INSERT INTO Tipo_Vuelo (Id_Tipo, Descripcion_Tipo)
VALUES(
	1,
	EncryptByPassPhrase('ServiciosWeb2023.','Entrante')
);

INSERT INTO Tipo_Vuelo (Id_Tipo, Descripcion_Tipo)
VALUES(
	2,
	EncryptByPassPhrase('ServiciosWeb2023.','Saliente')
);

--5. Aerolinea
INSERT INTO Aerolinea (ID_Aerolinea, Siglas_Aerolinea, Descripcion_Aerolinea, Imagen_Aerolinea) 
VALUES (
	'AE-1',
	EncryptByPassPhrase('ServiciosWeb2023.','AA'),
	EncryptByPassPhrase('ServiciosWeb2023.','American Airlines'),
	EncryptByPassPhrase('ServiciosWeb2023.','https://drive.google.com/uc?export=download&id=1foe-175fu4PYwUAFv3tBb1rmc1TYZX35')
);

INSERT INTO Aerolinea (ID_Aerolinea, Siglas_Aerolinea, Descripcion_Aerolinea, Imagen_Aerolinea) 
VALUES (
	'AE-2', 
	EncryptByPassPhrase('ServiciosWeb2023.','DA'),
	EncryptByPassPhrase('ServiciosWeb2023.','Delta Airlines'),
	EncryptByPassPhrase('ServiciosWeb2023.','https://drive.google.com/uc?export=download&id=1Xml666yAmX-uwaZVi3e5L6MNX3LRLlVu')
);

INSERT INTO Aerolinea (ID_Aerolinea, Siglas_Aerolinea, Descripcion_Aerolinea, Imagen_Aerolinea) 
VALUES (
	'AE-3',
	EncryptByPassPhrase('ServiciosWeb2023.','CA'),
	EncryptByPassPhrase('ServiciosWeb2023.','Copa Airlines'),
	EncryptByPassPhrase('ServiciosWeb2023.','https://drive.google.com/uc?export=download&id=1YHnAD_49XNvR4ddWjHO6CpKhsfCKL76-')
);

INSERT INTO Aerolinea (ID_Aerolinea, Siglas_Aerolinea, Descripcion_Aerolinea, Imagen_Aerolinea) 
VALUES (
	'AE-4', 
	EncryptByPassPhrase('ServiciosWeb2023.','SA'),
	EncryptByPassPhrase('ServiciosWeb2023.', 'Spirit Airlines'),
	EncryptByPassPhrase('ServiciosWeb2023.','https://drive.google.com/uc?export=download&id=1T-k5MgxlKj9BKCzMgkfVVMEDVBu2y5AZ')
);

INSERT INTO Aerolinea (ID_Aerolinea, Siglas_Aerolinea, Descripcion_Aerolinea, Imagen_Aerolinea) 
VALUES (
	'AE-5',
	EncryptByPassPhrase('ServiciosWeb2023.','AF'),
	EncryptByPassPhrase('ServiciosWeb2023.','Air France'),
	EncryptByPassPhrase('ServiciosWeb2023.','https://drive.google.com/uc?export=download&id=1m25Xxf2TaJ7BTXHHGXJE3mscDnG0FKhp')
);

INSERT INTO Aerolinea (ID_Aerolinea, Siglas_Aerolinea, Descripcion_Aerolinea, Imagen_Aerolinea) 
VALUES (
	'AE-6',
	EncryptByPassPhrase('ServiciosWeb2023.','IB'),
	EncryptByPassPhrase('ServiciosWeb2023.','Iberia'),
	EncryptByPassPhrase('ServiciosWeb2023.','https://drive.google.com/uc?export=download&id=1qaZ1sWFt0UhQ2zx-Z_PkwlBVTOwbpNhS')
);

--6. Puertas_Aeropuerto
INSERT INTO Puertas_Aeropuerto (ID_Puerta, Numero_Puerta, Estado_Puerta) 
VALUES (
	'PUA-1',
	EncryptByPassPhrase('ServiciosWeb2023.','1'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Abierta')
);

INSERT INTO Puertas_Aeropuerto (ID_Puerta, Numero_Puerta, Estado_Puerta) 
VALUES (
	'PUA-2',
	EncryptByPassPhrase('ServiciosWeb2023.','2'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Abierta')
);

INSERT INTO Puertas_Aeropuerto (ID_Puerta, Numero_Puerta, Estado_Puerta) 
VALUES (
	'PUA-3',
	EncryptByPassPhrase('ServiciosWeb2023.','3'),
	EncryptByPassPhrase('ServiciosWeb2023.','Cerrada')
);

INSERT INTO Puertas_Aeropuerto (ID_Puerta, Numero_Puerta, Estado_Puerta) 
VALUES (
	'PUA-4', 
	EncryptByPassPhrase('ServiciosWeb2023.','4'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Cerrada')
);

INSERT INTO Puertas_Aeropuerto (ID_Puerta, Numero_Puerta, Estado_Puerta) 
VALUES (
	'PUA-5', 
	EncryptByPassPhrase('ServiciosWeb2023.','5'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Abierta')
);

INSERT INTO Puertas_Aeropuerto (ID_Puerta, Numero_Puerta, Estado_Puerta) 
VALUES (
	'PUA-6', 
	EncryptByPassPhrase('ServiciosWeb2023.','6'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Fuera de Servicio')
);

--7. Paises
INSERT INTO Paises (Id_Pais, Siglas_Pais, Descripcion_Pais, Imagen_Pais) 
VALUES(
	'PA-1',
	EncryptByPassPhrase('ServiciosWeb2023.','CRC'),
	EncryptByPassPhrase('ServiciosWeb2023.','Costa Rica'),
	EncryptByPassPhrase('ServiciosWeb2023.','https://drive.google.com/uc?export=download&id=146efd0em6KR5r7CC32w0LmhZGucRKZgh')
);

INSERT INTO Paises (Id_Pais, Siglas_Pais, Descripcion_Pais, Imagen_Pais) 
VALUES(
	'PA-2',
	EncryptByPassPhrase('ServiciosWeb2023.','USA'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Estados Unidos'),
	EncryptByPassPhrase('ServiciosWeb2023.','https://drive.google.com/uc?export=download&id=176HuKDEhgFXYXs0t_jqh4mKCoBm_FRNh')
);

INSERT INTO Paises (Id_Pais, Siglas_Pais, Descripcion_Pais, Imagen_Pais) 
VALUES(
	'PA-3',
	EncryptByPassPhrase('ServiciosWeb2023.','CAN'),
	EncryptByPassPhrase('ServiciosWeb2023.','Canada'),
	EncryptByPassPhrase('ServiciosWeb2023.','https://drive.google.com/uc?export=download&id=1bJA5kRk5A-4x5Y69i4WzujyEGChw2B7A')
);

INSERT INTO Paises (Id_Pais, Siglas_Pais, Descripcion_Pais, Imagen_Pais) 
VALUES(
	'PA-4', 
	EncryptByPassPhrase('ServiciosWeb2023.','EGYP'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Egipto'), 
	EncryptByPassPhrase('ServiciosWeb2023.','https://drive.google.com/uc?export=download&id=1Pa5jVPFEBgWTibuxEQIBoQKnVTV1XQHE')
);

INSERT INTO Paises (Id_Pais, Siglas_Pais, Descripcion_Pais, Imagen_Pais) 
VALUES(
	'PA-5',
	EncryptByPassPhrase('ServiciosWeb2023.','JPN'),
	EncryptByPassPhrase('ServiciosWeb2023.','Japon'),
	EncryptByPassPhrase('ServiciosWeb2023.','https://drive.google.com/uc?export=download&id=1ot7394JSbeB1-FaCwc0t2ytdz7cWvee4')
);

--8. Vuelos
INSERT INTO Vuelos (Id_Vuelo, Aerolinea, Id_Pais, Fecha_Vuelo, Hora_Vuelo, Estado_Vuelo, Precio_Vuelo, Puerta_Vuelo, Tipo_Vuelo)
VALUES (
	'AA-200',
	'AE-1',
	'PA-2', 
	EncryptByPassPhrase('ServiciosWeb2023.','2023-11-15'),
	EncryptByPassPhrase('ServiciosWeb2023.','14:30'),
	EncryptByPassPhrase('ServiciosWeb2023.','Programado'),
	EncryptByPassPhrase('ServiciosWeb2023.','300.00'),
	'PUA-1',
	1
);

INSERT INTO Vuelos (Id_Vuelo, Aerolinea, Id_Pais, Fecha_Vuelo, Hora_Vuelo, Estado_Vuelo, Precio_Vuelo, Puerta_Vuelo, Tipo_Vuelo)
VALUES (
	'DA-120', 
	'AE-2',
	'PA-1',
	EncryptByPassPhrase('ServiciosWeb2023.','2023-11-20'),
	EncryptByPassPhrase('ServiciosWeb2023.','10:00'),
	EncryptByPassPhrase('ServiciosWeb2023.','Demorado'),
	EncryptByPassPhrase('ServiciosWeb2023.', '450.00'), 
	'PUA-2',
	1
);

INSERT INTO Vuelos (Id_Vuelo, Aerolinea, Id_Pais, Fecha_Vuelo, Hora_Vuelo, Estado_Vuelo, Precio_Vuelo, Puerta_Vuelo, Tipo_Vuelo)
VALUES (
	'AF-47', 
	'AE-5', 
	'PA-3', 
	EncryptByPassPhrase('ServiciosWeb2023.','2023-12-25'),
	EncryptByPassPhrase('ServiciosWeb2023.','16:45'),
	EncryptByPassPhrase('ServiciosWeb2023.','Cancelado'),
	EncryptByPassPhrase('ServiciosWeb2023.', '350.00'), 
	'PUA-3',
	2
);

INSERT INTO Vuelos (Id_Vuelo, Aerolinea, Id_Pais, Fecha_Vuelo, Hora_Vuelo, Estado_Vuelo, Precio_Vuelo, Puerta_Vuelo, Tipo_Vuelo)
VALUES (
	'SA-536', 
	'AE-4', 
	'PA-4', 
	EncryptByPassPhrase('ServiciosWeb2023.','2023-12-10'), 
	EncryptByPassPhrase('ServiciosWeb2023.','08:00'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Programado'), 
	EncryptByPassPhrase('ServiciosWeb2023.', '400.00'), 
	'PUA-4',
	2
);

INSERT INTO Vuelos (Id_Vuelo, Aerolinea, Id_Pais, Fecha_Vuelo, Hora_Vuelo, Estado_Vuelo, Precio_Vuelo, Puerta_Vuelo, Tipo_Vuelo)
VALUES (
	'IB-700', 
	'AE-6',	
	'PA-5', 
	EncryptByPassPhrase('ServiciosWeb2023.','2023-01-15'), 
	EncryptByPassPhrase('ServiciosWeb2023.','12:30'),
	EncryptByPassPhrase('ServiciosWeb2023.','A tiempo'), 
	EncryptByPassPhrase('ServiciosWeb2023.', '500.00'), 
	'PUA-5',
	1
);

--9. Usuarios
INSERT INTO Usuarios (Id_Usuario, Primer_Apellido, Segundo_Apellido, Nombre, Usuario, Correo_Usuario, Password_Usuario, Id_Pregunta, Respuesta_Seguridad, Id_Rol) 
VALUES (
	'0001', 
	EncryptByPassPhrase('ServiciosWeb2023.','Perez'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Jackson'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Ximena'), 
	EncryptByPassPhrase('ServiciosWeb2023.','XimenaPJ'), 
	EncryptByPassPhrase('ServiciosWeb2023.','xime.perezja@gmail.com'),
	EncryptByPassPhrase('ServiciosWeb2023.', 'CristianoRonaldo123'),
	1, 
	EncryptByPassPhrase('ServiciosWeb2023.','Esparza'),
	1
);

INSERT INTO Usuarios (Id_Usuario, Primer_Apellido, Segundo_Apellido, Nombre, Usuario, Correo_Usuario, Password_Usuario, Id_Pregunta, Respuesta_Seguridad, Id_Rol) 
VALUES (
	'0002', 
	EncryptByPassPhrase('ServiciosWeb2023.','Lasala'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Villegas'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Carlos'), 
	EncryptByPassPhrase('ServiciosWeb2023.', 'CarlosLV'), 
	EncryptByPassPhrase('ServiciosWeb2023.','carlos.lavja@gmail.com'),
	EncryptByPassPhrase('ServiciosWeb2023.','Eurocopa2018'),
	2, 
	EncryptByPassPhrase('ServiciosWeb2023.','Lester'), 
	2
);

INSERT INTO Usuarios (Id_Usuario, Primer_Apellido, Segundo_Apellido, Nombre, Usuario, Correo_Usuario, Password_Usuario, Id_Pregunta, Respuesta_Seguridad, Id_Rol) 
VALUES (
	'0003', 
	EncryptByPassPhrase('ServiciosWeb2023.','Gregory'),
	EncryptByPassPhrase('ServiciosWeb2023.','Allen'),
	EncryptByPassPhrase('ServiciosWeb2023.','Aisha'), 
	EncryptByPassPhrase('ServiciosWeb2023.','AishaGA'), 
	EncryptByPassPhrase('ServiciosWeb2023.','aish.gregall@gmail.com'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Barcelona82'),
	3, 
	EncryptByPassPhrase('ServiciosWeb2023.','Rosado'),
	3
);

INSERT INTO Usuarios (Id_Usuario, Primer_Apellido, Segundo_Apellido, Nombre, Usuario, Correo_Usuario, Password_Usuario, Id_Pregunta, Respuesta_Seguridad, Id_Rol) 
VALUES (
	'0004', 
	EncryptByPassPhrase('ServiciosWeb2023.','Chaves'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Chivi'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Caroline'), 
	EncryptByPassPhrase('ServiciosWeb2023.', 'CarolineCC'), 
	EncryptByPassPhrase('ServiciosWeb2023.','caro.chav@gmail.com'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Isaac123.'),
	1, 
	EncryptByPassPhrase('ServiciosWeb2023.','Alajuela'), 
	4
);

INSERT INTO Usuarios (Id_Usuario, Primer_Apellido, Segundo_Apellido, Nombre, Usuario, Correo_Usuario, Password_Usuario, Id_Pregunta, Respuesta_Seguridad, Id_Rol) 
VALUES (
	'0005', 
	EncryptByPassPhrase('ServiciosWeb2023.','Johnson'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Gray'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Maria'),
	EncryptByPassPhrase('ServiciosWeb2023.','MariaJG'),
	EncryptByPassPhrase('ServiciosWeb2023.','maria.johnG@gmail.com'),
	EncryptByPassPhrase('ServiciosWeb2023.','CampNou4-0.'),
	2, 
	EncryptByPassPhrase('ServiciosWeb2023.','Panther'), 
	5
);

INSERT INTO Usuarios (Id_Usuario, Primer_Apellido, Segundo_Apellido, Nombre, Usuario, Correo_Usuario, Password_Usuario, Id_Pregunta, Respuesta_Seguridad, Id_Rol) 
VALUES (
	'0006', 
	EncryptByPassPhrase('ServiciosWeb2023.','Cuchittini'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Aveiros'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Rodrygo'),
	EncryptByPassPhrase('ServiciosWeb2023.','RodrygoCA'),
	EncryptByPassPhrase('ServiciosWeb2023.','rodrygo.CR7@gmail.com'),
	EncryptByPassPhrase('ServiciosWeb2023.','City92'),
	4, 
	EncryptByPassPhrase('ServiciosWeb2023.','Dos'), 
	6
);

--10. Reservaciones
INSERT INTO Reservaciones (Id_Reservacion, Id_Usuario, Id_Vuelo, Numero_Boletos, Total, Estado_Reservacion)
VALUES (
	'RES-1', 
	'0001', 
	'AA-200', 
	EncryptByPassPhrase('ServiciosWeb2023.','3'), 
	EncryptByPassPhrase('ServiciosWeb2023.', '900.00'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Pendiente')
);


INSERT INTO Reservaciones (Id_Reservacion, Id_Usuario, Id_Vuelo, Numero_Boletos, Total, Estado_Reservacion)
VALUES (
	'RES-2',
	'0002', 
	'AF-47', 
	EncryptByPassPhrase('ServiciosWeb2023.','2'),
	EncryptByPassPhrase('ServiciosWeb2023.','500.00'),
	EncryptByPassPhrase('ServiciosWeb2023.','Confirmada')
);

INSERT INTO Reservaciones (Id_Reservacion, Id_Usuario, Id_Vuelo, Numero_Boletos, Total, Estado_Reservacion)
VALUES (
	'RES-3', 
	'0003', 
	'DA-120',
	EncryptByPassPhrase('ServiciosWeb2023.','1'),
	EncryptByPassPhrase('ServiciosWeb2023.','450.00'),
	EncryptByPassPhrase('ServiciosWeb2023.','Pendiente')
);

INSERT INTO Reservaciones (Id_Reservacion, Id_Usuario, Id_Vuelo, Numero_Boletos, Total, Estado_Reservacion)
VALUES (
	'RES-4',
	'0004', 
	'AA-200', 
	EncryptByPassPhrase('ServiciosWeb2023.','4'), 
	EncryptByPassPhrase('ServiciosWeb2023.','1200.00'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Confirmada')
);

INSERT INTO Reservaciones (Id_Reservacion, Id_Usuario, Id_Vuelo, Numero_Boletos, Total, Estado_Reservacion)
VALUES (
	'RES-5', 
	'0005', 
	'AF-47', 
	EncryptByPassPhrase('ServiciosWeb2023.','2'),
	EncryptByPassPhrase('ServiciosWeb2023.','700.00'),
	EncryptByPassPhrase('ServiciosWeb2023.','Pendiente')
);

--11. Tarjeta
INSERT INTO Tarjeta (Id_RegistroTarjeta, Id_Usuario, Numero_Tarjeta, Mes_Exp, Year_Exp, CVV, Tipo_Tarjeta)
VALUES (
	'T001', 
	'0001', 
	EncryptByPassPhrase('ServiciosWeb2023.','1234567890123456'), 
	EncryptByPassPhrase('ServiciosWeb2023.','12'), 
	EncryptByPassPhrase('ServiciosWeb2023.','2025'), 
	EncryptByPassPhrase('ServiciosWeb2023.','123'),
	EncryptByPassPhrase('ServiciosWeb2023.','Débito')
);

INSERT INTO Tarjeta (Id_RegistroTarjeta, Id_Usuario, Numero_Tarjeta, Mes_Exp, Year_Exp, CVV, Tipo_Tarjeta)
VALUES (
	'T002', 
	'0002', 
	EncryptByPassPhrase('ServiciosWeb2023.','9876543210987654'),
	EncryptByPassPhrase('ServiciosWeb2023.','06'), 
	EncryptByPassPhrase('ServiciosWeb2023.','2024'),
	EncryptByPassPhrase('ServiciosWeb2023.','456'),
	EncryptByPassPhrase('ServiciosWeb2023.','Crédito')
);

INSERT INTO Tarjeta (Id_RegistroTarjeta, Id_Usuario, Numero_Tarjeta, Mes_Exp, Year_Exp, CVV, Tipo_Tarjeta)
VALUES (
	'T003', 
	'0003', 
	EncryptByPassPhrase('ServiciosWeb2023.','5678901234567890'), 
	EncryptByPassPhrase('ServiciosWeb2023.','12'),
	EncryptByPassPhrase('ServiciosWeb2023.','2023'),
	EncryptByPassPhrase('ServiciosWeb2023.','789'),
	EncryptByPassPhrase('ServiciosWeb2023.','Crédito')
);

INSERT INTO Tarjeta (Id_RegistroTarjeta, Id_Usuario, Numero_Tarjeta, Mes_Exp, Year_Exp, CVV, Tipo_Tarjeta)
VALUES (
	'T004', 
	'0004', 
	EncryptByPassPhrase('ServiciosWeb2023.','3456789012345678'), 
	EncryptByPassPhrase('ServiciosWeb2023.','03'), 
	EncryptByPassPhrase('ServiciosWeb2023.','2026'), 
	EncryptByPassPhrase('ServiciosWeb2023.','234'),
	EncryptByPassPhrase('ServiciosWeb2023.','Débito')
);

INSERT INTO Tarjeta (Id_RegistroTarjeta, Id_Usuario, Numero_Tarjeta, Mes_Exp, Year_Exp, CVV, Tipo_Tarjeta)
VALUES (
	'T005', 
	'0005', 
	EncryptByPassPhrase('ServiciosWeb2023.','9012345678901234'), 
	EncryptByPassPhrase('ServiciosWeb2023.','11'), 
	EncryptByPassPhrase('ServiciosWeb2023.','2024'), 
	EncryptByPassPhrase('ServiciosWeb2023.','567'),
	EncryptByPassPhrase('ServiciosWeb2023.','Débito')
);

--12. Bitacora
INSERT INTO Bitacora (Id_Bitacora, Id_Usuario, Fecha, Tipo_Movimiento, Descripcion_Movimiento)
VALUES (
	'B001', 
	'0001', 
	EncryptByPassPhrase('ServiciosWeb2023.','2023-11-16'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Modificacion'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Modificacion de los precios de los vuelos') 
);

INSERT INTO Bitacora (Id_Bitacora, Id_Usuario, Fecha, Tipo_Movimiento, Descripcion_Movimiento)
VALUES (
	'B002', 
	'0001', 
	EncryptByPassPhrase('ServiciosWeb2023.','2023-11-12'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Modificacion'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Modificacion de los precios de los vuelos') 
);

INSERT INTO Bitacora (Id_Bitacora, Id_Usuario, Fecha, Tipo_Movimiento, Descripcion_Movimiento)
VALUES (
	'B003', 
	'0003', 
	EncryptByPassPhrase('ServiciosWeb2023.','2023-11-7'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Modificacion'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Cambio de consecutivos en la BD') 
);

INSERT INTO Bitacora (Id_Bitacora, Id_Usuario, Fecha, Tipo_Movimiento, Descripcion_Movimiento)
VALUES (
	'B004', 
	'0001', 
	EncryptByPassPhrase('ServiciosWeb2023.','2023-10-11'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Errores'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Error para agregar vuelos') 
);

INSERT INTO Bitacora (Id_Bitacora, Id_Usuario, Fecha, Tipo_Movimiento, Descripcion_Movimiento)
VALUES (
	'B005', 
	'0004', 
	EncryptByPassPhrase('ServiciosWeb2023.','2023-10-28'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Mantenimiento'), 
	EncryptByPassPhrase('ServiciosWeb2023.','Revision de procesos internos') 
);

SELECT Id_Bitacora, 
	             Id_Usuario,
	    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Fecha) as varchar(max)) as date) as Fecha,
	    CAST(DecryptByPassPhrase('ServiciosWeb2023.',Tipo_Movimiento) as varchar(max)) as Tipo_Movimiento,
        CAST(DecryptByPassPhrase('ServiciosWeb2023.',Descripcion_Movimiento) as varchar(max)) as Descripcion_Movimiento
        FROM Bitacora
        ORDER BY Fecha asc;
--Selects para Desenciptar
--1. Pivote
SELECT 
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',ID) as varchar(50)) as int) as ID, 
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion) as varchar(50)) as Descripcion,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Prefijo) as varchar(50)) as Prefijo,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',Consecutivo) as varchar(50)) as int) as Consecutivo,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',RangoInicial) as varchar(50)) as int) as RangoInicial,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',RangoFinal) as varchar(50)) as int) as RangoFinal
FROM Pivote
ORDER BY ID;

--2. Pregunta_Seguridad
SELECT 
	Id_Pregunta,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Pregunta) as varchar(max)) as Descripcion_Pregunta
FROM Pregunta_Seguridad;

--3. Roles
SELECT 
	Id_Rol,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Nombre_Rol) as varchar(max)) as Nombre_Rol,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Rol) as varchar(max)) as Descripcion_Rol
FROM Roles;

--4. Tipo de vuelo
SELECT 
	Id_Tipo,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Tipo) as varchar(max)) as Descripcion_Tipo
FROM Tipo_Vuelo;

--5. Aerolinea
SELECT ID_Aerolinea, 
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Siglas_Aerolinea) as varchar(max)) as Siglas_Aerolinea,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Aerolinea) as varchar(max)) as Descripcion_Aerolinea,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.',Imagen_Aerolinea) as varchar(max)) as Imagen_Aerolinea
FROM Aerolinea;

--6. Puertas_Aeropuerto
SELECT ID_Puerta,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',Numero_Puerta) as varchar(50)) as int) as Numero_Puerta,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Puerta) as varchar(max)) as Estado_Puerta
FROM Puertas_Aeropuerto;

--7. Paises
SELECT Id_Pais, 
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Siglas_Pais) as varchar(max)) as Siglas_Pais,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Pais) as varchar(max)) as Descripcion_Pais,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Imagen_Pais) as varchar(max)) as Imagen_Pais
FROM Paises;

--8. Vuelos
SELECT 
	Id_Vuelo,
	Aerolinea,
	Id_Pais,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Fecha_Vuelo) as varchar(50)) as date) as Fecha_Vuelo,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Hora_Vuelo) as varchar(50)) as time) as Hora_Vuelo,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Vuelo) as varchar(max)) as Estado_Vuelo,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Precio_Vuelo) as varchar(50)) as decimal(6,2)) as Precio_Vuelo,
	Puerta_Vuelo,
    Tipo_Vuelo
FROM Vuelos;

--9. Usuarios
SELECT 
	Id_Usuario,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Primer_Apellido) as varchar(max)) as Primero_Apellido,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Segundo_Apellido) as varchar(max)) as Segundo_Apellido,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Nombre) as varchar(max)) as Nombre,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Usuario) as varchar(max)) as Usuario,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Correo_Usuario) as varchar(max)) as Correo_Usuario,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Password_Usuario) as varchar(max)) as Password_Usuario,
	Id_Pregunta,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Respuesta_Seguridad) as varchar(max)) as Respuesta_Seguridad,
	Id_Rol
From Usuarios;

--10. Reservaciones
SELECT
	Id_Reservacion,
	Id_Usuario,
	Id_Vuelo,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Boletos) as varchar(50)) as int) as Numero_Boletos,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Total) as varchar(50)) as decimal(6,2)) as Total,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Reservacion) as varchar(max)) as Estado_Reservacion
FROM Reservaciones;

--11. Tarjeta
SELECT
	Id_RegistroTarjeta,
	Id_Usuario,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Tarjeta) as varchar(max)) as Numero_Tarjeta,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Mes_Exp) as varchar(50)) as int) as Mes_Exp,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Year_Exp) as varchar(50)) as int) as Year_Exp,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', CVV) as varchar(50)) as int) as CVV,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Tipo_Tarjeta) as varchar(max)) as Tipo_Tarjeta
From Tarjeta;

--12. Bitacora
SELECT 
	Id_Bitacora,
	Id_Usuario,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Fecha) as varchar(50)) as date) as Fecha,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Tipo_Movimiento) as varchar(max)) as Tipo_Movimiento,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Movimiento) as varchar(max)) as Descripcion_Movimiento
FROM Bitacora;


SELECT Id_Bitacora, 
	   Id_Usuario,
	    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Fecha) as varchar(max)) as date) as Fecha,
	    CAST(DecryptByPassPhrase('ServiciosWeb2023.',Tipo_Movimiento) as varchar(max)) as Tipo_Movimiento,
        CAST(DecryptByPassPhrase('ServiciosWeb2023.',Descripcion_Movimiento) as varchar(max)) as Descripcion_Movimiento
        FROM Bitacora
        ORDER BY Id_Bitacora;

-- Selects
SELECT * FROM Pivote;
SELECT * FROM Aerolinea;
SELECT * FROM Paises;
SELECT * FROM Pregunta_Seguridad;
SELECT * FROM Puertas_Aeropuerto;
SELECT * FROM Reservaciones;
SELECT * FROM Roles; 
SELECT * FROM Tipo_Vuelo;
SELECT * FROM Tarjeta;
SELECT * FROM Usuarios;
SELECT * FROM Vuelos;


--Drops
DROP TABLE Pivote;
DROP TABLE Reservaciones;
DROP TABLE Tarjeta;
DROP TABLE Bitacora;
DROP TABLE Vuelos;
DROP TABLE Aerolinea;
DROP TABLE Paises;
DROP TABLE Usuarios;
DROP TABLE Pregunta_Seguridad;
DROP TABLE Roles;
DROP TABLE Tipo_Vuelo;
DROP TABLE Puertas_Aeropuerto;





--Select completo para ver todos los vuelos disponibles
SELECT
    V.Id_Vuelo,
    CAST(DecryptByPassPhrase('ServiciosWeb2023.', A.Descripcion_Aerolinea) as varchar(max)) AS Aerolinea,
    CAST(DecryptByPassPhrase('ServiciosWeb2023.',P.Descripcion_Pais) as varchar(max)) AS Pais_Destino,
    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',V.Fecha_Vuelo) as varchar(50)) as date)  AS Fecha,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',V.Hora_Vuelo) as varchar(50)) as time)  AS Hora,
    CAST(DecryptByPassPhrase('ServiciosWeb2023.',V.Estado_Vuelo)as varchar(max)) AS Estado,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', PA.Numero_Puerta) as varchar(50)) as int) as Puerta,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.',TP.Descripcion_Tipo)as varchar(max)) AS Tipo_Vuelo
FROM
    Vuelos V
JOIN
    Aerolinea A ON V.Aerolinea = A.ID_Aerolinea
JOIN
    Paises P ON V.Id_Pais = P.Id_Pais
JOIN
    Puertas_Aeropuerto PA ON V.Puerta_Vuelo = PA.ID_Puerta
JOIN
	Tipo_Vuelo TP ON V.Tipo_Vuelo = TP.Id_Tipo;

--Select completo para ver todos los vuelos entrantes
SELECT
    V.Id_Vuelo,
    CAST(DecryptByPassPhrase('ServiciosWeb2023.', A.Descripcion_Aerolinea) as varchar(max)) AS Aerolinea,
    CAST(DecryptByPassPhrase('ServiciosWeb2023.',P.Descripcion_Pais) as varchar(max)) AS Proveniente,
    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',V.Fecha_Vuelo) as varchar(50)) as date)  AS Fecha,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',V.Hora_Vuelo) as varchar(50)) as time)  AS Hora,
    CAST(DecryptByPassPhrase('ServiciosWeb2023.',V.Estado_Vuelo)as varchar(max)) AS Estado,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', PA.Numero_Puerta) as varchar(50)) as int) as Puerta,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.',TP.Descripcion_Tipo)as varchar(max)) AS Tipo_Vuelo
FROM
    Vuelos V
JOIN
    Aerolinea A ON V.Aerolinea = A.ID_Aerolinea
JOIN
    Paises P ON V.Id_Pais = P.Id_Pais
JOIN
    Puertas_Aeropuerto PA ON V.Puerta_Vuelo = PA.ID_Puerta
JOIN
	Tipo_Vuelo TP ON V.Tipo_Vuelo = TP.Id_Tipo
WHERE Tipo_Vuelo = 1;

--Select completo para ver todos los vuelos salientes
SELECT
    V.Id_Vuelo,
    CAST(DecryptByPassPhrase('ServiciosWeb2023.', A.Descripcion_Aerolinea) as varchar(max)) AS Aerolinea,
    CAST(DecryptByPassPhrase('ServiciosWeb2023.',P.Descripcion_Pais) as varchar(max)) AS Pais_Destino,
    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',V.Fecha_Vuelo) as varchar(50)) as date)  AS Fecha,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',V.Hora_Vuelo) as varchar(50)) as time)  AS Hora,
    CAST(DecryptByPassPhrase('ServiciosWeb2023.',V.Estado_Vuelo)as varchar(max)) AS Estado,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', PA.Numero_Puerta) as varchar(50)) as int) as Puerta,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.',TP.Descripcion_Tipo)as varchar(max)) AS Tipo_Vuelo
FROM
    Vuelos V
JOIN
    Aerolinea A ON V.Aerolinea = A.ID_Aerolinea
JOIN
    Paises P ON V.Id_Pais = P.Id_Pais
JOIN
    Puertas_Aeropuerto PA ON V.Puerta_Vuelo = PA.ID_Puerta
JOIN
	Tipo_Vuelo TP ON V.Tipo_Vuelo = TP.Id_Tipo
WHERE Tipo_Vuelo = 2;




--Encriptar la base de datos

--Activar MasterKey
USE Master;
GO
CREATE MASTER KEY ENCRYPTION
BY PASSWORD='CxA2023.';
GO

-- CREAMOS UN CERTIFICADO DESDE EL MISMO MSSQL
CREATE CERTIFICATE DatabaseEncrypted
WITH 
SUBJECT='TRIGGERDB DATABASE ENCRYPTION';
GO

-- HACEMOS UN BACKUP DEL CERTIFICADO
BACKUP CERTIFICATE DatabaseEncrypted
TO FILE = 'C:\Certificados\Certificado_TDE' --Deben crear la carpeta Certificados en C:
WITH PRIVATE KEY (FILE='C:\Certificados\Certificado_TDE.pri',
ENCRYPTION BY PASSWORD='ServiciosWeb123.') 

-- CREAMOS EL DATABASE ENCRYPTION SOBRE LA BASE
USE ServiciosWeb 
GO

CREATE DATABASE ENCRYPTION KEY
WITH ALGORITHM = AES_256 -- ALGORITMO
ENCRYPTION BY SERVER CERTIFICATE DatabaseEncrypted;

-- VERIFICAMOS EL ESTADO DE LAS BASES DE DATOS
SELECT NAME,IS_ENCRYPTED  
FROM SYS.DATABASES 
WHERE NAME = 'ServiciosWeb' -- 0 = No encriptada y 1 = Encriptada

-- ENCENDEMOS EL TDE SOBRE LA BASE
ALTER DATABASE ServiciosWeb
SET ENCRYPTION ON;
GO

-- VERIFICAMOS EL ESTADO DE LAS BASES DE DATOS
SELECT NAME,IS_ENCRYPTED  
FROM SYS.DATABASES 
WHERE NAME = 'ServiciosWeb' -- 0 = No encriptada y 1 = Encriptada


SELECT
        V.Id_Vuelo,
        CAST(DecryptByPassPhrase('ServiciosWeb2023.', A.Descripcion_Aerolinea) as varchar(max)) AS Aerolinea,
        CAST(DecryptByPassPhrase('ServiciosWeb2023.',P.Descripcion_Pais) as varchar(max)) AS Pais_Destino,
        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',V.Fecha_Vuelo) as varchar(50)) as date)  AS Fecha,
	    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',V.Hora_Vuelo) as varchar(50)) as time)  AS Hora,
        CAST(DecryptByPassPhrase('ServiciosWeb2023.',V.Estado_Vuelo) as varchar(max)) AS Estado,
        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', V.Precio_Vuelo) as varchar(50)) as decimal(6,2)) as Precio_Vuelo,
	    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', PA.Numero_Puerta) as varchar(50)) as int) as Puerta
    FROM
        Vuelos V
    JOIN
        Aerolinea A ON V.Aerolinea = A.ID_Aerolinea
    JOIN
        Paises P ON V.Id_Pais = P.Id_Pais
    JOIN
        Puertas_Aeropuerto PA ON V.Puerta_Vuelo = PA.ID_Puerta;




SELECT 
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Prefijo) as varchar(50)) as Prefijo,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',Consecutivo) as varchar(50)) as int) as Consecutivo,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',RangoInicial) as varchar(50)) as int) as RangoInicial,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',RangoFinal) as varchar(50)) as int) as RangoFinal
FROM Pivote
WHERE CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',ID) as varchar(50)) as int) = 1;




UPDATE Pivote 
SET 
    Descripcion = EncryptByPassPhrase('ServiciosWeb2023.', 'Aerolinea'),
    Consecutivo = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(6 AS nvarchar(max)) AS varchar(max))),
    Prefijo = EncryptByPassPhrase('ServiciosWeb2023.', 'AE'),
    RangoInicial = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(1 AS nvarchar(max)) AS varchar(max))),
    RangoFinal = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(10 AS nvarchar(max)) AS varchar(max)))
WHERE CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',ID) as varchar(50)) as int) = 1;


