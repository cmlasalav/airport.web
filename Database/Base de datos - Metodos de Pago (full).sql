--Base de datos para los metodos de pago

--Tarjeta
CREATE TABLE Tarjeta (
  Id_Registro NVARCHAR(5) NOT NULL,
  Numero_Tarjeta varbinary(max) NOT NULL,
  Mes_Exp varbinary(max) NOT NULL,
  Year_Exp varbinary(max) NOT NULL,
  CVV varbinary(max)  NOT NULL,
  Total varbinary(max) NOT NULL,
  Estado_Pago varbinary(max) NOT NULL,
  Tipo_Tarjeta varbinary(max) NOT NULL,
  CONSTRAINT TarjetaPK PRIMARY KEY (Id_Registro),
);



--Cuenta_Banco
CREATE TABLE Cuenta_Banco (
  Id_RegistroCuenta NVARCHAR(5) NOT NULL,
  Num_Cuenta varbinary(max) NOT NULL,
  Codigo varbinary(max) NOT NULL,
  Password_Cuenta varbinary(max) NOT NULL,
  CONSTRAINT Cuenta_BancoPK PRIMARY KEY (Id_RegistroCuenta)
);

--Insert
--1. Tarjeta
INSERT INTO Tarjeta (Id_Registro, Numero_Tarjeta, Mes_Exp, Year_Exp, CVV, Total, Estado_Pago, Tipo_Tarjeta)
VALUES (
	'T001',  
	EncryptByPassPhrase('ServiciosWeb2023.','1234567890123456'), 
	EncryptByPassPhrase('ServiciosWeb2023.','12'), 
	EncryptByPassPhrase('ServiciosWeb2023.','2025'), 
	EncryptByPassPhrase('ServiciosWeb2023.','123'),
	EncryptByPassPhrase('ServiciosWeb2023.','900'),
	EncryptByPassPhrase('ServiciosWeb2023.','Pendiente'),
	EncryptByPassPhrase('ServiciosWeb2023.','Debito')
);

INSERT INTO Tarjeta (Id_Registro, Numero_Tarjeta, Mes_Exp, Year_Exp, CVV, Total, Estado_Pago, Tipo_Tarjeta)
VALUES (
	'T002',  
	EncryptByPassPhrase('ServiciosWeb2023.','9876543210987654'),
	EncryptByPassPhrase('ServiciosWeb2023.','06'), 
	EncryptByPassPhrase('ServiciosWeb2023.','2024'),
	EncryptByPassPhrase('ServiciosWeb2023.','456'),
	EncryptByPassPhrase('ServiciosWeb2023.','600'),
	EncryptByPassPhrase('ServiciosWeb2023.','Pagado'),
	EncryptByPassPhrase('ServiciosWeb2023.','Credito')
);

--2. Cuenta de banco
INSERT INTO Cuenta_Banco (Id_RegistroCuenta, Num_Cuenta, Codigo, Password_Cuenta) 
VALUES(
	'0001',
	EncryptByPassPhrase('ServiciosWeb2023.','12131415161718'),
	EncryptByPassPhrase('ServiciosWeb2023.','123456'),
	EncryptByPassPhrase('ServiciosWeb2023.','ServiciosWeb.')
);

INSERT INTO Cuenta_Banco (Id_RegistroCuenta, Num_Cuenta, Codigo, Password_Cuenta) 
VALUES(
	'0002',
	EncryptByPassPhrase('ServiciosWeb2023.','98979695949392'),
	EncryptByPassPhrase('ServiciosWeb2023.','98765'),
	EncryptByPassPhrase('ServiciosWeb2023.','Nose123.')
);




--Select 
SELECT * FROM Tarjeta;
SELECT * FROM Cuenta_Banco;

--Select para desencriptar
--1. Tarjeta
SELECT
	Id_Registro,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Tarjeta) as varchar(max)) as Numero_Tarjeta,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Mes_Exp) as varchar(50)) as int) as Mes_Exp,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Year_Exp) as varchar(50)) as int) as Year_Exp,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', CVV) as varchar(50)) as int) as CVV,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Total) as varchar(50)) as int) as Total,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Pago) as varchar(max)) as Estado_Pago,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Tipo_Tarjeta) as varchar(max)) as Tipo_Tarjeta
From Tarjeta;

--2. Cuenta de banco
SELECT 
	Id_RegistroCuenta,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Num_Cuenta) as varchar(max)) as Numero_Cuenta,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Codigo) as varchar(50)) as int) as Codigo,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Password_Cuenta) as varchar(max)) as Password_Cuenta
FROM Cuenta_Banco;


--Drop
DROP TABLE Tarjeta;
DROP TABLE Cuenta_Banco;


--Encriptar la base de datos

--Activar MasterKey
USE Master;
GO
CREATE MASTER KEY ENCRYPTION
BY PASSWORD='CxA2023.';
GO

-- CREAMOS UN CERTIFICADO DESDE EL MISMO MSSQL
CREATE CERTIFICATE CertificadoMetodosPago
WITH 
SUBJECT='DATABASE ENCRYPTION';
GO

-- HACEMOS UN BACKUP DEL CERTIFICADO
BACKUP CERTIFICATE CertificadoMetodosPago
TO FILE = 'C:\Certificados\Certificado_TDE1' --Deben crear la carpeta Certificados en C:
WITH PRIVATE KEY (FILE='C:\Certificados\Certificado_TDE1.pri',
ENCRYPTION BY PASSWORD='MetodosPago123.') 

-- CREAMOS EL DATABASE ENCRYPTION SOBRE LA BASE
USE MetodosPago 
GO

CREATE DATABASE ENCRYPTION KEY
WITH ALGORITHM = AES_256 -- ALGORITMO
ENCRYPTION BY SERVER CERTIFICATE CertificadoMetodosPago;

-- VERIFICAMOS EL ESTADO DE LAS BASES DE DATOS
SELECT NAME,IS_ENCRYPTED  
FROM SYS.DATABASES 
WHERE NAME = 'MetodosPago' -- 0 = No encriptada y 1 = Encriptada

-- ENCENDEMOS EL TDE SOBRE LA BASE
ALTER DATABASE MetodosPago
SET ENCRYPTION ON;
GO

-- VERIFICAMOS EL ESTADO DE LAS BASES DE DATOS
SELECT NAME,IS_ENCRYPTED  
FROM SYS.DATABASES 
WHERE NAME = 'MetodosPago' -- 0 = No encriptada y 1 = Encriptada