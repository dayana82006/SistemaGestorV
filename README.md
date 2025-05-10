# Sistema de Gestión de Compras e Inventario

Este proyecto en C# es una aplicación de consola destinada a gestionar compras, ventas, inventarios, y planes promocionales. Permite el registro de productos, la gestión de terceros (empleados, clientes, proveedores), y el manejo de transacciones comerciales de manera eficiente.

## Características

### 1. **Gestión de Productos**
   - Permite agregar, editar, eliminar y consultar productos en el sistema.
   - Los productos pueden estar asociados a promociones en fechas específicas.

### 2. **Gestión de Terceros**
   - Permite registrar y gestionar diferentes tipos de terceros como proveedores, clientes y empleados.
   - Cada tercero tiene un tipo y puede estar vinculado a transacciones de compras o ventas.

### 3. **Planes Promocionales**
   - Generación de planes promocionales que incluyen productos con descuentos, con fechas de inicio y final.
   - Los productos incluidos en la promoción son específicos y se gestionan de manera autónoma.

### 4. **Compras**
   - El sistema permite registrar compras, especificando el proveedor y el empleado que realiza la compra.
   - Al registrar la compra, el inventario de productos se actualiza automáticamente.

### 5. **Ventas**
   - Registro de ventas con el cliente y el empleado que realiza la venta.
   - Los productos vendidos son descontados del inventario automáticamente.

### 6. **Tipos de Movimientos**
   - El sistema gestiona diferentes tipos de movimientos como pagos de recibos y otros movimientos relacionados con la caja.
   - Según el tipo de movimiento, el dinero es descontado o agregado a la caja.

## Funcionalidades

1. **Gestión de Productos:**
   - Agregar, editar y eliminar productos.
   - Visualización de la lista de productos registrados.

2. **Gestión de Terceros:**
   - Registrar y gestionar proveedores, empleados, clientes y otros tipos de terceros.
   - Gestionar la información de contacto y relaciones comerciales.

3. **Planes Promocionales:**
   - Crear y administrar planes promocionales.
   - Establecer las fechas de inicio y fin de cada promoción, y los productos que participarán.

4. **Registro de Compras:**
   - Registrar las compras realizadas a proveedores.
   - Actualización automática de stock al registrar una compra.

5. **Registro de Ventas:**
   - Registrar ventas a clientes.
   - Actualización automática de stock al registrar una venta.

6. **Movimientos de Caja:**
   - Registrar movimientos relacionados con la caja (entradas y salidas de dinero).
   - Permite registrar pagos de recibos y otros movimientos financieros.

## Estructura del Proyecto

- **Program.cs**: Punto de entrada de la aplicación, que gestiona el ciclo principal del programa, mostrando el menú y gestionando las opciones seleccionadas por el usuario.
  
- **UIProductos**: Interfaz para gestionar los productos, incluir opciones como agregar, editar y consultar productos.
  
- **UITerceros**: Interfaz para gestionar los diferentes tipos de terceros (empleados, clientes, proveedores, dealers).
  
- **UIPlanes**: Interfaz para crear y gestionar planes promocionales.

## Instrucciones de Uso

1. **Iniciar el Programa:**
   - Al ejecutar la aplicación, se muestra el menú principal con opciones para gestionar productos, terceros, planes promocionales, compras y ventas.

2. **Gestión de Productos:**
   - Al seleccionar la opción de "Gestión de Productos", se pueden agregar, editar o eliminar productos del sistema.

3. **Gestión de Terceros:**
   - Seleccionando "Gestión de Terceros", el usuario puede registrar y gestionar proveedores, cliente y empleados.

4. **Planes Promocionales:**
   - En la opción de "Planes de Promoción", el usuario puede crear nuevos planes promocionales, asociando productos específicos y fechas de promoción.

5. **Registro de Compras:**
   - Selecciona la opción de "Compras" para registrar las compras realizadas a los proveedores, actualizando automáticamente el inventario.

6. **Registro de Ventas:**
   - Selecciona la opción de "Ventas" para registrar las ventas realizadas a los clientes, actualizando el inventario de productos.

## Requisitos

- **Sistema Operativo**: Windows/Linux (compatible con .NET Core).
- **.NET Framework**: .NET 6.0 o superior.
- **Base de Datos**: MySQL (o cualquier base de datos compatible).
