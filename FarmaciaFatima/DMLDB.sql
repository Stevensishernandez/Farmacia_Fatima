CREATE TABLE presentacion (
    idPresentacion INT PRIMARY KEY IDENTITY (1, 1),
    nombre VARCHAR (50) NOT NULL
);

CREATE TABLE casaMedica (
    idCasaMedica INT PRIMARY KEY IDENTITY (1, 1),
    nombre VARCHAR (50) NOT NULL
);

CREATE TABLE producto (
    idProducto INT PRIMARY KEY IDENTITY (1, 1),
    nombre VARCHAR (50) NOT NULL,
    fecha date
);

CREATE TABLE bodega (
    idBodega INT PRIMARY KEY IDENTITY (1, 1),
    cantidad INT,
    idCasaMedica INT,
    idProducto INT

    FOREIGN KEY (idCasaMedica) REFERENCES casaMedica (idCasaMedica),
    FOREIGN KEY (idProducto) REFERENCES producto (idProducto),
);

CREATE TABLE tipoVenta (
    representacion INT,
    idPresentacion INT,
    idBodega INT,
    precio DECIMAL

    FOREIGN KEY (idPresentacion) REFERENCES presentacion (idPresentacion),
    FOREIGN KEY (idBodega) REFERENCES bodega (idBodega),
);

CREATE TABLE Temporal (
    nombreP varchar(100),
    presentacion varchar(100),
    cantidad varchar(20),
    cantidadCa varchar(20),
    costoV varchar(100),
    casaMedica varchar(100),
    fechaV varchar(30)
);
