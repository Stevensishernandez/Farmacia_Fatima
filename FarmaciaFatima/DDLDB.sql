
SELECT idCasaMedica AS Identificador, nombre AS CasaMedica FROM casaMedica;

CREATE TABLE Temporal (
    nombreP varchar(100),
    presentacion varchar(100),
    cantidad varchar(220),
    cantidadCa varchar(20),
    costoV varchar(100),
    casaMedica varchar(100),
    fechaV varchar(100)
);

CREATE TRIGGER  restarInventario
on DetalleVenta
AFTER INSERT 
AS 
UPDATE B set B.cantidad = B.cantidad - D.restado 
from bodega B
INNER JOIN inserted D ON (D.idBodega=B.IdBodega)

CREATE TRIGGER  sumarInventario
on DetalleVenta
AFTER DELETE 
AS 
UPDATE B set B.cantidad = B.cantidad + D.restado 
from bodega B
INNER JOIN DELETED D ON (D.idBodega=B.IdBodega)