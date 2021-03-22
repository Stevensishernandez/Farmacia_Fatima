CREATE TABLE Temporal (
    nombreP varchar(100),
    presentacion varchar(100),
    cantidad varchar(20),
    costoV varchar(100),
    casaMedica varchar(100),
    fechaV varchar(30)
);

BULK INSERT Temporal 
FROM 'C:\Users\Steven Sis\Desktop\Eliminar\originales.csv'
WITH  
( 
FIELDTERMINATOR = ',',   
ROWTERMINATOR = '\n'  
)

INSERT INTO presentacion (nombre) (SELECT DISTINCT presentacion FROM Temporal);

INSERT INTO producto (nombre) (SELECT DISTINCT nombreP FROM Temporal);

INSERT INTO casaMedica (nombre) 
	(SELECT DISTINCT casaMedica FROM Temporal);

INSERT INTO bodega ( idProducto,idCasaMedica, idPresentacion,cantidad, precio, estado, fechaCaducidad, tipo) 
(SELECT DISTINCT P.idProducto,C.idCasaMedica, PS.IdPresentacion, T.cantidad, T.costoV,'1', '11/05/2020','2'  From Temporal T
INNER JOIN producto P on (P.nombre=T.nombreP)
INNER JOIN casaMedica C on (C.nombre=T.casaMedica)
INNER JOIN presentacion PS on (PS.nombre = T.presentacion)
);

DROP TABLE temporal;

CREATE TABLE Temporal (
    nombreP varchar(100),
    presentacion varchar(100),
    representa varchar(20),
    cantidad varchar(20),
    costoV varchar(100),
    casaMedica varchar(100),
    fechaV varchar(30)
);

BULK INSERT Temporal 
FROM 'C:\Users\Steven Sis\Desktop\Eliminar\genericos.csv'
WITH  
( 
FIELDTERMINATOR = ',',   
ROWTERMINATOR = '\n'  
)

INSERT INTO presentacion (nombre) (SELECT DISTINCT presentacion FROM Temporal);

SELECT  nombre, COUNT(*) AS repite FROM producto GROUP BY nombre;

INSERT INTO producto (nombre) (SELECT DISTINCT nombreP FROM Temporal);

INSERT INTO bodega ( idProducto,idCasaMedica,cantidad, estado, fechaCaducidad, tipo) 
(SELECT DISTINCT P.idProducto,C.idCasaMedica, T.cantidad,'1', '11/05/2020','1'  From Temporal T
INNER JOIN producto P on (P.nombre=T.nombreP)
INNER JOIN casaMedica C on (C.nombre=T.casaMedica)
);

INSERT INTO PresedenciaProducto (idPresedencia, idBodega, representacion, Precio)
(SELECT DISTINCT PS.IdPresentacion,B.IdBodega, T.representa, T.costoV  From Temporal T
INNER JOIN producto P on (P.nombre=T.nombreP)
INNER JOIN presentacion PS on (PS.nombre=T.presentacion)
INNER JOIN casaMedica C on (C.nombre=T.casaMedica)
INNER JOIN bodega B ON (B.idProducto = P.idProducto AND B.idCasaMedica = C.idCasaMedica AND B.tipo='1')
);
