
USE TEST_DBVENTA1
GO

drop table if exists DETALLE_COMPRA;
drop table if exists DETALLE_VENTA;
drop table if exists NEGOCIO;
drop table if exists COMPRA;
drop table if exists VENTA;
drop table if exists PRODUCTO;
drop table if exists USUARIO;
drop table if exists PERMISO;
drop table if exists ROL;
drop table if exists PROVEEDOR;
drop table if exists CLIENTE;

drop table if exists USUARIO;
drop table if exists CATEGORIA;
drop procedure if exists usp_RegistrarVenta;
drop TYPE if exists [dbo].[EDetalle_Venta];
drop procedure if exists sp_RegistrarCompra;
drop TYPE if exists dbo.EDetalle_Compra;


create table ROL(
IdRol int primary key identity,
Descripcion varchar(50),
FechaRegistro datetime default getdate(),
FechaEdicion datetime default getdate(),
Estado bit default 1
)
go

create table PERMISO(
IdPermiso int primary key identity,
IdRol int references ROL(IdRol),
NombreMenu varchar(100),
FechaRegistro datetime default getdate(),
FechaEdicion datetime default getdate(),
Estado bit default 1
)
go

create table PROVEEDOR(
IdProveedor int primary key identity,
Documento varchar(50),
RazonSocial varchar(50),
Correo varchar(50),
Telefono varchar(50),
Estado bit default 1,
FechaRegistro datetime default getdate(),
FechaEdicion datetime default getdate(),
)
go

create table CLIENTE(
IdCliente int primary key identity,
Documento varchar(50),
NombreCompleto varchar(50),
Correo varchar(50),
Telefono varchar(50),
Estado bit DEFAULT 1,
FechaRegistro datetime default getdate(),
FechaEdicion datetime default getdate()
)

 
create table USUARIO(
IdUsuario int primary key identity,
Documento varchar(50),
NombreCompleto varchar(50),
Correo varchar(50),
Clave varchar(50),
IdRol int references ROL(IdRol),
Estado bit default 1,
FechaRegistro datetime default getdate(),
FechaEdicion datetime default getdate()
)

create table CATEGORIA(
IdCategoria int primary key identity,
Descripcion varchar(100),
Estado bit default 1 ,
FechaRegistro datetime default getdate(),
FechaEdicion datetime default getdate()
)
 
create table PRODUCTO(
IdProducto int primary key identity,
Codigo varchar(50),
Nombre varchar(50),
Descripcion varchar(200),
IdCategoria int references CATEGORIA(IdCategoria),
Stock int not null default 0,
PrecioCompra decimal(10,2) default 0,
PrecioVenta decimal(10,2) default 0,
Estado bit default 1 NOT NULL,
FechaRegistro datetime default getdate(),
FechaEdicion datetime default getdate()
)

create table COMPRA(
IdCompra int primary key identity,
IdUsuario int references USUARIO(IdUsuario),
IdProveedor int references PROVEEDOR(IdProveedor),
TipoDocumento varchar(50),
NumeroDocumento varchar(50),
MontoTotal decimal(10,2),
FechaRegistro datetime default getdate()
)
go


create table DETALLE_COMPRA(
IdDetalleCompra int primary key identity,
IdCompra int references COMPRA(IdCompra),
IdProducto int references PRODUCTO(IdProducto),
PrecioCompra decimal(10,2) default 0,
PrecioVenta decimal(10,2) default 0,
Cantidad int,
MontoTotal decimal(10,2),
FechaRegistro datetime default getdate()
)

go

create table VENTA(
IdVenta int primary key identity,
IdUsuario int references USUARIO(IdUsuario),
TipoDocumento varchar(50),
NumeroDocumento varchar(50),
DocumentoCliente varchar(50),
NombreCliente varchar(100),
MontoPago decimal(10,2),
MontoCambio decimal(10,2),
MontoTotal decimal(10,2),
FechaRegistro datetime default getdate()
)


go


create table DETALLE_VENTA(
IdDetalleVenta int primary key identity,
IdVenta int references VENTA(IdVenta),
IdProducto int references PRODUCTO(IdProducto),
PrecioVenta decimal(10,2),
Cantidad int,
SubTotal decimal(10,2),
FechaRegistro datetime default getdate()
)

go

create table NEGOCIO(
IdNegocio int primary key,
Nombre varchar(60),
RUC varchar(60),
Direccion varchar(60),
Logo varbinary(max) NULL
)

go


/*************************** CREACION DE PROCEDIMIENTOS ALMACENADOS ***************************/
/*--------------------------------------------------------------------------------------------*/

go


create or alter PROC SP_REGISTRARUSUARIO(
@Documento varchar(50),
@NombreCompleto varchar(100),
@Correo varchar(100),
@Clave varchar(100),
@IdRol int,
@Estado bit,
@IdUsuarioResultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @IdUsuarioResultado = 0
	set @Mensaje = ''


	if not exists(select * from USUARIO where Documento = @Documento)
	begin
		insert into usuario(Documento,NombreCompleto,Correo,Clave,IdRol,Estado) values
		(@Documento,@NombreCompleto,@Correo,@Clave,@IdRol,@Estado)

		set @IdUsuarioResultado = SCOPE_IDENTITY()
		
	end
	else
		set @Mensaje = 'No se puede repetir el documento para m�s de un usuario'


end

go

create or alter PROC SP_EDITARUSUARIO(
@IdUsuario int,
@Documento varchar(50),
@NombreCompleto varchar(100),
@Correo varchar(100),
@Clave varchar(100),
@IdRol int,
@Estado bit,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''


	if not exists(select * from USUARIO where Documento = @Documento and idusuario != @IdUsuario)
	begin
		update  usuario set
		Documento = @Documento,
		NombreCompleto = @NombreCompleto,
		Correo = @Correo,
		Clave = @Clave,
		IdRol = @IdRol,
		Estado = @Estado
		where IdUsuario = @IdUsuario

		set @Respuesta = 1
		
	end
	else
		set @Mensaje = 'No se puede repetir el documento para m�s de un usuario'


end
go

create or alter PROC SP_ELIMINARUSUARIO(
@IdUsuario int,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''
	declare @pasoreglas bit = 1

	IF EXISTS (SELECT * FROM COMPRA C 
	INNER JOIN USUARIO U ON U.IdUsuario = C.IdUsuario
	WHERE U.IDUSUARIO = @IdUsuario
	)
	BEGIN
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar porque el usuario se encuentra relacionado a una COMPRA\n' 
	END

	IF EXISTS (SELECT * FROM VENTA V
	INNER JOIN USUARIO U ON U.IdUsuario = V.IdUsuario
	WHERE U.IDUSUARIO = @IdUsuario
	)
	BEGIN
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar porque el usuario se encuentra relacionado a una VENTA\n' 
	END

	if(@pasoreglas = 1)
	begin
		delete from USUARIO where IdUsuario = @IdUsuario
		set @Respuesta = 1 
	end

end

go

/* ---------- PROCEDIMIENTOS PARA CATEGORIA -----------------*/


create or alter PROC SP_RegistrarCategoria(
@Descripcion varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion)
	begin
		insert into CATEGORIA(Descripcion,Estado) values (@Descripcion,@Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	ELSE
		set @Mensaje = 'No se puede repetir la descripcion de una categoria'
	
end


go

create or alter procedure sp_EditarCategoria(
@IdCategoria int,
@Descripcion varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion =@Descripcion and IdCategoria != @IdCategoria)
		update CATEGORIA set
		Descripcion = @Descripcion,
		Estado = @Estado
		where IdCategoria = @IdCategoria
	ELSE
	begin
		SET @Resultado = 0
		set @Mensaje = 'No se puede repetir la descripcion de una categoria'
	end

end

go

create or alter procedure sp_EliminarCategoria(
@IdCategoria int,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (
	 select *  from CATEGORIA c
	 inner join PRODUCTO p on p.IdCategoria = c.IdCategoria
	 where c.IdCategoria = @IdCategoria
	)
	begin
	 delete top(1) from CATEGORIA where IdCategoria = @IdCategoria
	end
	ELSE
	begin
		SET @Resultado = 0
		set @Mensaje = 'La categoria se encuentara relacionada a un producto'
	end

end

GO

/* ---------- PROCEDIMIENTOS PARA PRODUCTO -----------------*/

create or alter PROC sp_RegistrarProducto(
@Codigo varchar(20),
@Nombre varchar(30),
@Descripcion varchar(30),
@IdCategoria int,
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM producto WHERE Codigo = @Codigo)
	begin
		insert into producto(Codigo,Nombre,Descripcion,IdCategoria,Estado) values (@Codigo,@Nombre,@Descripcion,@IdCategoria,@Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	ELSE
	 SET @Mensaje = 'Ya existe un producto con el mismo codigo' 
	
end

GO

create or alter procedure sp_ModificarProducto(
@IdProducto int,
@Codigo varchar(20),
@Nombre varchar(30),
@Descripcion varchar(30),
@IdCategoria int,
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE codigo = @Codigo and IdProducto != @IdProducto)
		
		update PRODUCTO set
		codigo = @Codigo,
		Nombre = @Nombre,
		Descripcion = @Descripcion,
		IdCategoria = @IdCategoria,
		Estado = @Estado
		where IdProducto = @IdProducto
	ELSE
	begin
		SET @Resultado = 0
		SET @Mensaje = 'Ya existe un producto con el mismo codigo' 
	end
end

go


create or alter PROC SP_EliminarProducto(
@IdProducto int,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''
	declare @pasoreglas bit = 1

	IF EXISTS (SELECT * FROM DETALLE_COMPRA dc 
	INNER JOIN PRODUCTO p ON p.IdProducto = dc.IdProducto
	WHERE p.IdProducto = @IdProducto
	)
	BEGIN
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar porque se encuentra relacionado a una COMPRA\n' 
	END

	IF EXISTS (SELECT * FROM DETALLE_VENTA dv
	INNER JOIN PRODUCTO p ON p.IdProducto = dv.IdProducto
	WHERE p.IdProducto = @IdProducto
	)
	BEGIN
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar porque se encuentra relacionado a una VENTA\n' 
	END

	if(@pasoreglas = 1)
	begin
		delete from PRODUCTO where IdProducto = @IdProducto
		set @Respuesta = 1 
	end

end
go

/* ---------- PROCEDIMIENTOS PARA CLIENTE -----------------*/

create or alter PROC sp_RegistrarCliente(
@Documento varchar(50),
@NombreCompleto varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 0
	DECLARE @IDPERSONA INT 
	IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Documento = @Documento)
	begin
		insert into CLIENTE(Documento,NombreCompleto,Correo,Telefono,Estado) values (
		@Documento,@NombreCompleto,@Correo,@Telefono,@Estado)

		set @Resultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'El numero de documento ya existe'
end

go

create or alter PROC sp_ModificarCliente(
@IdCliente int,
@Documento varchar(50),
@NombreCompleto varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 1
	DECLARE @IDPERSONA INT 
	IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Documento = @Documento and IdCliente != @IdCliente)
	begin
		update CLIENTE set
		Documento = @Documento,
		NombreCompleto = @NombreCompleto,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		where IdCliente = @IdCliente
	end
	else
	begin
		SET @Resultado = 0
		set @Mensaje = 'El numero de documento ya existe'
	end
end

GO

/* ---------- PROCEDIMIENTOS PARA PROVEEDOR -----------------*/

create or alter PROC sp_RegistrarProveedor(
@Documento varchar(50),
@RazonSocial varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 0
	DECLARE @IDPERSONA INT 
	IF NOT EXISTS (SELECT * FROM PROVEEDOR WHERE Documento = @Documento)
	begin
		insert into PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado) values (
		@Documento,@RazonSocial,@Correo,@Telefono,@Estado)

		set @Resultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'El numero de documento ya existe'
end

GO

create or alter PROC sp_ModificarProveedor(
@IdProveedor int,
@Documento varchar(50),
@RazonSocial varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 1
	DECLARE @IDPERSONA INT 
	IF NOT EXISTS (SELECT * FROM PROVEEDOR WHERE Documento = @Documento and IdProveedor != @IdProveedor)
	begin
		update PROVEEDOR set
		Documento = @Documento,
		RazonSocial = @RazonSocial,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		where IdProveedor = @IdProveedor
	end
	else
	begin
		SET @Resultado = 0
		set @Mensaje = 'El numero de documento ya existe'
	end
end


go

create or alter procedure sp_EliminarProveedor(
@IdProveedor int,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (
	 select *  from PROVEEDOR p
	 inner join COMPRA c on p.IdProveedor = c.IdProveedor
	 where p.IdProveedor = @IdProveedor
	)
	begin
	 delete top(1) from PROVEEDOR where IdProveedor = @IdProveedor
	end
	ELSE
	begin
		SET @Resultado = 0
		set @Mensaje = 'El proveedor se encuentara relacionado a una compra'
	end

end

go

/* PROCESOS PARA REGISTRAR UNA COMPRA */

CREATE TYPE [dbo].[EDetalle_Compra] AS TABLE(
	[IdProducto] int NULL,
	[PrecioCompra] decimal(18,2) NULL,
	[PrecioVenta] decimal(18,2) NULL,
	[Cantidad] int NULL,
	[MontoTotal] decimal(18,2) NULL
)

GO


create or alter PROCEDURE sp_RegistrarCompra(
@IdUsuario int,
@IdProveedor int,
@TipoDocumento varchar(500),
@NumeroDocumento varchar(500),
@MontoTotal decimal(18,2),
@DetalleCompra [EDetalle_Compra] READONLY,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	
	begin try

		declare @idcompra int = 0
		set @Resultado = 1
		set @Mensaje = ''

		begin transaction registro

		insert into COMPRA(IdUsuario,IdProveedor,TipoDocumento,NumeroDocumento,MontoTotal)
		values(@IdUsuario,@IdProveedor,@TipoDocumento,@NumeroDocumento,@MontoTotal)

		set @idcompra = SCOPE_IDENTITY()

		insert into DETALLE_COMPRA(IdCompra,IdProducto,PrecioCompra,PrecioVenta,Cantidad,MontoTotal)
		select @idcompra,IdProducto,PrecioCompra,PrecioVenta,Cantidad,MontoTotal from @DetalleCompra


		update p set p.Stock = p.Stock + dc.Cantidad, 
		p.PrecioCompra = dc.PrecioCompra,
		p.PrecioVenta = dc.PrecioVenta
		from PRODUCTO p
		inner join @DetalleCompra dc on dc.IdProducto= p.IdProducto

		commit transaction registro


	end try
	begin catch
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro
	end catch

end


GO

/* PROCESOS PARA REGISTRAR UNA VENTA */
CREATE TYPE [dbo].[EDetalle_Venta] AS TABLE(
	[IdProducto] int NULL,
	[PrecioVenta] decimal(18,2) NULL,
	[Cantidad] int NULL,
	[SubTotal] decimal(18,2) NULL
)

GO


create or alter procedure usp_RegistrarVenta(
@IdUsuario int,
@TipoDocumento varchar(500),
@NumeroDocumento varchar(500),
@DocumentoCliente varchar(500),
@NombreCliente varchar(500),
@MontoPago decimal(18,2),
@MontoCambio decimal(18,2),
@MontoTotal decimal(18,2),
@DetalleVenta [EDetalle_Venta] READONLY,                                      
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	
	begin try

		declare @idventa int = 0
		set @Resultado = 1
		set @Mensaje = ''

		begin  transaction registro

		insert into VENTA(IdUsuario,TipoDocumento,NumeroDocumento,DocumentoCliente,NombreCliente,MontoPago,MontoCambio,MontoTotal)
		values(@IdUsuario,@TipoDocumento,@NumeroDocumento,@DocumentoCliente,@NombreCliente,@MontoPago,@MontoCambio,@MontoTotal)

		set @idventa = SCOPE_IDENTITY()

		insert into DETALLE_VENTA(IdVenta,IdProducto,PrecioVenta,Cantidad,SubTotal)
		select @idventa,IdProducto,PrecioVenta,Cantidad,SubTotal from @DetalleVenta

		commit transaction registro

	end try
	begin catch
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro
	end catch

end

go


create or alter PROC sp_ReporteCompras(
 @fechainicio varchar(10),
 @fechafin varchar(10),
 @idproveedor int
 )
  as
 begin

  SET DATEFORMAT dmy;
   select 
 convert(char(10),c.FechaRegistro,103)[FechaRegistro],c.TipoDocumento,c.NumeroDocumento,c.MontoTotal,
 u.NombreCompleto[UsuarioRegistro],
 pr.Documento[DocumentoProveedor],pr.RazonSocial,
 p.Codigo[CodigoProducto],p.Nombre[NombreProducto],ca.Descripcion[Categoria],dc.PrecioCompra,dc.PrecioVenta,dc.Cantidad,dc.MontoTotal[SubTotal]
 from COMPRA c
 inner join USUARIO u on u.IdUsuario = c.IdUsuario
 inner join PROVEEDOR pr on pr.IdProveedor = c.IdProveedor
 inner join DETALLE_COMPRA dc on dc.IdCompra = c.IdCompra
 inner join PRODUCTO p on p.IdProducto = dc.IdProducto
 inner join CATEGORIA ca on ca.IdCategoria = p.IdCategoria
 where CONVERT(date,c.FechaRegistro) between @fechainicio and @fechafin
 and pr.IdProveedor = iif(@idproveedor=0,pr.IdProveedor,@idproveedor)
 end

 go

 create or alter PROC sp_ReporteVentas(
 @fechainicio varchar(10),
 @fechafin varchar(10)
 )
 as
 begin
 SET DATEFORMAT dmy;  
 select 
 convert(char(10),v.FechaRegistro,103)[FechaRegistro],v.TipoDocumento,v.NumeroDocumento,v.MontoTotal,
 u.NombreCompleto[UsuarioRegistro],
 v.DocumentoCliente,v.NombreCliente,
 p.Codigo[CodigoProducto],p.Nombre[NombreProducto],ca.Descripcion[Categoria],dv.PrecioVenta,dv.Cantidad,dv.SubTotal
 from VENTA v
 inner join USUARIO u on u.IdUsuario = v.IdUsuario
 inner join DETALLE_VENTA dv on dv.IdVenta = v.IdVenta
 inner join PRODUCTO p on p.IdProducto = dv.IdProducto
 inner join CATEGORIA ca on ca.IdCategoria = p.IdCategoria
 where CONVERT(date,v.FechaRegistro) between @fechainicio and @fechafin
end




/****************** INSERTAMOS REGISTROS A LAS TABLAS ******************/
/*---------------------------------------------------------------------*/

GO

 insert into rol (Descripcion)
 values('ADMINISTRADOR')

 GO

  insert into rol (Descripcion)
 values('EMPLEADO')

 GO

 insert into USUARIO(Documento,NombreCompleto,Correo,Clave,IdRol,Estado)
 values 
 ('101010','ADMIN','ADMIN@GMAIL.COM','123',1,1)

 GO


 insert into USUARIO(Documento,NombreCompleto,Correo,Clave,IdRol,Estado)
 values 
 ('20','EMPLEADO','EMPLEADO@GMAIL.COM','456',2,1)

 GO

  insert into PERMISO(IdRol,NombreMenu) values
  (1,'menuusuarios'),
  (1,'menumantenedor'),
  (1,'menuventas'),
  (1,'menucompras'),
  (1,'menuclientes'),
  (1,'menuproveedores'),
  (1,'menureportes'),
  (1,'menuacercade')

  GO

  insert into PERMISO(IdRol,NombreMenu) values
  (2,'menuventas'),
  (2,'menucompras'),
  (2,'menuclientes'),
  (2,'menuproveedores'),
  (2,'menuacercade')

  GO

insert into NEGOCIO(IdNegocio,Nombre,RUC,Direccion,Logo) values
  (1,'Codigo Estudiante','20202020','av. codigo estudiante 123',null)

-- CREACION CATEGORIA PIZZAS
INSERT INTO CATEGORIA (Descripcion, Estado, FechaRegistro) VALUES ('Pizzas', 1, GETDATE());

-- Ejemplo 1: Pizza Margarita (con descripci�n acortada)
INSERT INTO PRODUCTO (Codigo, Nombre, Descripcion, IdCategoria, Stock, PrecioCompra, PrecioVenta, Estado, FechaRegistro)
VALUES ('PM001', 'Pizza Margarita', 'Pizza cl�sica con salsa de tomate' ,1, 50, 4.99, 9.99, 1, GETDATE());


-- Ejemplo 2: Pizza Pepperoni
INSERT INTO PRODUCTO (Codigo, Nombre, Descripcion, IdCategoria, Stock, PrecioCompra, PrecioVenta, Estado, FechaRegistro)
VALUES ('PP002', 'Pizza Pepperoni', 'Pizza con salsa de tomate, mozzarella y rodajas de pepperoni',1, 40, 5.99, 11.99, 1, GETDATE());

-- Ejemplo 3: Pizza Vegetariana
INSERT INTO PRODUCTO (Codigo, Nombre, Descripcion, IdCategoria, Stock, PrecioCompra, PrecioVenta, Estado, FechaRegistro)
VALUES ('PV003', 'Pizza Vegetariana', 'Pizza con salsa de tomate, mozzarella, champi�ones, pimientos y aceitunas',1, 30, 6.99, 13.99, 1, GETDATE());

-- Ejemplo 4: Pizza Carnicera
INSERT INTO PRODUCTO (Codigo, Nombre, Descripcion, IdCategoria, Stock, PrecioCompra, PrecioVenta, Estado, FechaRegistro)
VALUES ('PV004', 'Pizza Carnicera', 'Pizza con salsa de tomate, mozzarella, pepperoni, jamon ,carne ,salchicha italiana, res y cerdo',1, 20, 7.99, 20.99, 1, GETDATE());

-- Ejemplo 5: Pizza Tres quesos
INSERT INTO PRODUCTO (Codigo, Nombre, Descripcion, IdCategoria, Stock, PrecioCompra, PrecioVenta, Estado, FechaRegistro)
VALUES ('PV005', 'Pizza Tres quesos', 'Pizza con salsa de tomate, mozzarella, cheddar, parmesano y gorgonzola',1, 10, 8.99, 25.99, 1, GETDATE());

-- Ejemplo 6: Pizza Queso
INSERT INTO PRODUCTO (Codigo, Nombre, Descripcion, IdCategoria, Stock, PrecioCompra, PrecioVenta, Estado, FechaRegistro)
VALUES ('PV006', 'Pizza Queso', 'Pizza con salsa de tomate, mozzarella, ',1, 50, 4.99, 10.99, 1, GETDATE());

-- Ejemplo 7: Pizza Hawaiana
INSERT INTO PRODUCTO (Codigo, Nombre, Descripcion, IdCategoria, Stock, PrecioCompra, PrecioVenta, Estado, FechaRegistro)
VALUES ('PV007', 'Pizza Hawaiana', 'Pizza con salsa de tomate, mozzarella, pi�a, jamon y pepperoni',1, 40, 5.99, 15.99, 1, GETDATE());

-- Ejemplo 8: Pizza 4 estaciones
INSERT INTO PRODUCTO (Codigo, Nombre, Descripcion, IdCategoria, Stock, PrecioCompra, PrecioVenta, Estado, FechaRegistro)
VALUES ('PV008', 'Pizza 4 estaciones', 'Pizza con salsa de tomate, mozzarella, champi�ones, pimientos,jamon,salchicha italiana ,pepperoni,pi�ay aceitunas',1, 20, 8.99, 25.99, 1, GETDATE());

-- DATOS CLIENTES
INSERT INTO CLIENTE (Documento, NombreCompleto, Correo, Telefono, Estado, FechaRegistro)
VALUES
('12345678', 'Juan P�rez', 'juanperez@example.com', '987654321', 1, GETDATE()),
('23456789', 'Mar�a Garc�a', 'mariagarcia@example.com', '123456789', 1, GETDATE()),
('34567890', 'Pedro Ram�rez', 'pedroramirez@example.com', '555555555', 1, GETDATE()),
('45678901', 'Ana L�pez', 'analopez@example.com', '666666666', 1, GETDATE()),
('56789012', 'Carlos Mart�nez', 'carlosmartinez@example.com', '333333333', 1, GETDATE()),
('67890123', 'Laura S�nchez', 'laurasanchez@example.com', '999999999', 1, GETDATE()),
('78901234', 'Diego Rodr�guez', 'diegorodriguez@example.com', '777777777', 1, GETDATE()),
('89012345', 'Sof�a Hern�ndez', 'sofiahernandez@example.com', '444444444', 1, GETDATE()),
('90123456', 'Luisa G�mez', 'luisagomez@example.com', '222222222', 1, GETDATE()),
('01234567', 'Javier Vargas', 'javiervargas@example.com', '111111111', 1, GETDATE());



select * from CATEGORIA
select * from PRODUCTO
select * from PROVEEDOR
select * from USUARIO
select * from CLIENTE
select * from ROL