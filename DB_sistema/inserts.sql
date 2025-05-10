-- Inserts de país
INSERT INTO pais (nombre) VALUES ('Colombia');

-- Inserts de región
INSERT INTO region (nombre, paisId) VALUES ('Santander', 1);

-- Inserts de ciudad
INSERT INTO ciudad (nombre, regionId) VALUES ('Bucaramanga', 1);

-- Inserts de direcciones
INSERT INTO direccion (id, ciudadId, calleNumero, calleNombre) VALUES 
(1, 1, '45-23', 'Calle 45'),
(2, 1, '12-10', 'Carrera 27'),
(3, 1, '8-55', 'Avenida 50');

-- Inserts de empresa
INSERT INTO empresa (id, nombre, direccionId, fechaReg) VALUES
('EMP001', 'TecnoSoft', 1, '2023-01-10');

-- Inserts de tipos de documento
INSERT INTO tipo_documento (descripcion) VALUES ('Cédula de ciudadanía'), ('NIT');

-- Inserts de tipos de tercero
INSERT INTO tipoterceros (descripcion) VALUES ('Empleado'), ('Proveedor'), ('Cliente');

-- Inserts de terceros
INSERT INTO terceros (nombre, apellidos, email, tipoDocId, tipoTerceroId, direccionId, empresaId) VALUES
('Laura', 'Gómez Pérez', 'laura@gmail.com', 1, 1, 1, 'EMP001'),
('Carlos', 'Ramírez Díaz', 'carlos@gmail.com', 1, 2, 2, 'EMP001'),
('María', 'Rojas Peña', 'maria@gmail.com', 1, 3, 3, 'EMP001');

-- Inserts de teléfonos de terceros
INSERT INTO tercero_telefonos (numero, tipo, terceroId) VALUES
('3001234567', 'Celular', 1),
('3169876543', 'Celular', 2),
('3051122334', 'Celular', 3);

-- Inserts de EPS
INSERT INTO eps (nombre) VALUES ('Sanitas'), ('Sura');

-- Inserts de ARL
INSERT INTO arl (nombre) VALUES ('ARL SURA'), ('Colpatria');

-- Inserts de empleado
INSERT INTO empleado (terceroId, fechaIngreso, salarioBase, epsId, arlId) VALUES
(1, '2023-03-01', 2000000, 1, 1);

-- Inserts de proveedor
INSERT INTO proveedor (terceroId, scto, diaPago) VALUES
(2, 5.0, 15);

-- Inserts de productos
INSERT INTO productos (id, nombre, stock, stockMin, stockMax, createdAt, updatedAt, barcode) VALUES
('P001', 'Teclado Gamer', 30, 5, 50, '2024-01-10', '2024-05-01', '1234567890'),
('P002', 'Mouse Inalámbrico', 20, 3, 40, '2024-01-11', '2024-05-01', '0987654321');

-- Inserts de productos del proveedor
INSERT INTO productos_proveedor (terceroId, productoId) VALUES
(2, 'P001'),
(2, 'P002');

-- Inserts de cliente
INSERT INTO cliente (terceroId, fechaNac, fechaInforma) VALUES
(3, '1990-06-10', '2024-04-01');

-- Inserts de compras
INSERT INTO compras (terceroProvId, fecha, terceroEmpId, docCompra) VALUES
(2, '2024-04-15', 1, 'COMP001'),
(2, '2024-04-17', 1, 'COMP002');

-- Inserts de detalle de compra
INSERT INTO detalle_compra (fecha, productoId, cantidad, valor, compraId) VALUES
('2024-04-15', 'P001', 10, 50000, 1),
('2024-04-15', 'P002', 15, 30000, 1),
('2024-04-17', 'P001', 20, 50000, 2),
('2024-04-17', 'P002', 25, 30000, 2);

-- Inserts de facturación
INSERT INTO facturacion (fechaResolucion, numInicial, numFinal, factActual) VALUES
('2024-04-01', 1000, 1050, 1025);

-- Inserts de planes
INSERT INTO planes (nombre, fechaInicio, fechaFin, dcto) VALUES
('Plan Básico', '2024-01-01', '2024-12-31', 10.0),
('Plan Premium', '2024-01-01', '2024-12-31', 15.0);

-- Inserts de planes con productos
INSERT INTO planproducto (planId, productoId) VALUES
(1, 'P001'),
(2, 'P002');

-- Inserts de tipo de movimiento de caja
INSERT INTO tipomovcaja (nombre, tipo) VALUES
('Ingreso', 'Crédito'),
('Egreso', 'Débito');

-- Inserts de movimientos de caja
INSERT INTO movcaja (fecha, tipoMovId, valor, concepto, terceroId) VALUES
('2024-04-01', 1, 1000000, 'Venta de productos', 1),
('2024-04-02', 2, 500000, 'Compra de insumos', 2);

-- Inserts de ventas
INSERT INTO venta (factId, terceroEmpId, terceroCliId, fecha) VALUES
(1, 1, 3, '2024-04-10'),
(1, 1, 2, '2024-04-11');

-- Inserts de detalles de venta
INSERT INTO detalle_venta (factId, productoId, cantidad, valor) VALUES
(1, 'P001', 5, 50000),
(1, 'P002', 10, 30000),
(2, 'P001', 15, 50000),
(2, 'P002', 20, 30000);