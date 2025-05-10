CREATE DATABASE db_sistema;
USE db_sistema;

-- Tabla pais
CREATE TABLE pais (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50)
);

-- Tabla region
CREATE TABLE region (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50),
    paisId INT,
    FOREIGN KEY (paisId) REFERENCES pais(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla ciudad
CREATE TABLE ciudad (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50),
    regionId INT,
    FOREIGN KEY (regionId) REFERENCES region(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla direccion
CREATE TABLE direccion(
    id INT PRIMARY KEY,
    ciudadId INT,
    calleNumero VARCHAR(12),
    calleNombre VARCHAR(50),
    FOREIGN KEY (ciudadId) REFERENCES ciudad(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla empresa
CREATE TABLE empresa (
    id VARCHAR(20) PRIMARY KEY,
    nombre VARCHAR(20),
    direccionId INT,
    fechaReg DATE,
    FOREIGN KEY (direccionId) REFERENCES direccion(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla tipo_documento
CREATE TABLE tipo_documento (
    id INT AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(50)
);

-- Tabla tipoterceros
CREATE TABLE tipoterceros (
    id INT AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(50)
);

-- Tabla terceros
CREATE TABLE terceros (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(20),
    apellidos VARCHAR(50),
    email VARCHAR(50),
    tipoDocId INT,
    tipoTerceroId INT,
    direccionId INT,
    empresaId VARCHAR(20),
    FOREIGN KEY (tipoDocId) REFERENCES tipo_documento(id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (tipoTerceroId) REFERENCES tipoterceros(id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (direccionId) REFERENCES direccion(id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (empresaId) REFERENCES empresa(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla tercero_telefonos
CREATE TABLE tercero_telefonos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    numero VARCHAR(20),
    tipo VARCHAR(50),
    terceroId INT,
    FOREIGN KEY (terceroId) REFERENCES terceros(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla proveedor
CREATE TABLE proveedor (
    id INT AUTO_INCREMENT PRIMARY KEY,
    terceroId INT,
    scto DOUBLE,
    diaPago INT,
    FOREIGN KEY (terceroId) REFERENCES terceros(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla eps
CREATE TABLE eps (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50)
);

-- Tabla arl
CREATE TABLE arl (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50)
);

-- Tabla empleado
CREATE TABLE empleado (
    id INT AUTO_INCREMENT PRIMARY KEY,
    terceroId INT,
    fechaIngreso DATE,
    salarioBase DOUBLE,
    epsId INT,
    arlId INT,
    FOREIGN KEY (terceroId) REFERENCES terceros(id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (epsId) REFERENCES eps(id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (arlId) REFERENCES arl(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla productos
CREATE TABLE productos (
    id VARCHAR(20) PRIMARY KEY,
    nombre VARCHAR(50),
    stock INT,
    stockMin INT,
    stockMax INT,
    createdAt DATE,
    updatedAt DATE,
    barcode VARCHAR(50)
);

-- Tabla productos_proveedor
CREATE TABLE productos_proveedor (
    terceroId INT,
    productoId VARCHAR(20),
    PRIMARY KEY (terceroId, productoId),
    FOREIGN KEY (terceroId) REFERENCES terceros(id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (productoId) REFERENCES productos(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla cliente
CREATE TABLE cliente (
    id INT AUTO_INCREMENT PRIMARY KEY,
    terceroId INT,
    fechaNac DATE,
    fechaInforma DATE,
    FOREIGN KEY (terceroId) REFERENCES terceros(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla compras
CREATE TABLE compras (
    id INT AUTO_INCREMENT PRIMARY KEY,
    terceroProvId INT,
    fecha DATE,
    terceroEmpId INT,
    docCompra VARCHAR(50),
    FOREIGN KEY (terceroProvId) REFERENCES terceros(id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (terceroEmpId) REFERENCES terceros(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla detalle_compra
CREATE TABLE detalle_compra (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha DATE,
    productoId VARCHAR(20),
    cantidad INT,
    valor DOUBLE,
    compraId INT,
    FOREIGN KEY (productoId) REFERENCES productos(id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (compraId) REFERENCES compras(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla facturacion
CREATE TABLE facturacion (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fechaResolucion DATE,
    numInicial INT,
    numFinal INT,
    factActual INT
);

-- Tabla planes
CREATE TABLE planes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50),
    fechaInicio DATE,
    fechaFin DATE,
    dcto DOUBLE
);

-- Tabla planproducto
CREATE TABLE planproducto (
    planId INT,
    productoId VARCHAR(20),
    PRIMARY KEY (planId, productoId),
    FOREIGN KEY (planId) REFERENCES planes(id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (productoId) REFERENCES productos(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla tipomovcaja
CREATE TABLE tipomovcaja (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50),
    tipo VARCHAR(10)
);

-- Tabla movcaja
CREATE TABLE movcaja (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha DATE,
    tipoMovId INT,
    valor DOUBLE,
    concepto VARCHAR(50),
    terceroId INT,
    FOREIGN KEY (tipoMovId) REFERENCES tipomovcaja(id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (terceroId) REFERENCES terceros(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla venta
CREATE TABLE venta (
    id INT AUTO_INCREMENT PRIMARY KEY,
    factId INT,
    terceroEmpId INT,
    terceroCliId INT,
    fecha DATE,
    FOREIGN KEY (factId) REFERENCES facturacion(id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (terceroEmpId) REFERENCES terceros(id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (terceroCliId) REFERENCES terceros(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tabla detalle_venta
CREATE TABLE detalle_venta (
    id INT AUTO_INCREMENT PRIMARY KEY,
    factId INT,
    productoId VARCHAR(20),
    cantidad INT,
    valor DOUBLE,
    FOREIGN KEY (factId) REFERENCES venta(id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (productoId) REFERENCES productos(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);
