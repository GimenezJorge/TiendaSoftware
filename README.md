# TiendaSoftware

-- Creación de la base de datos
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'bdg3')
BEGIN
    CREATE DATABASE bdg3;
END;
GO

USE bdg3;
GO

-- Tabla de Roles
CREATE TABLE roles (
    id INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50) NOT NULL
);

-- Tabla de Usuarios
CREATE TABLE usuarios (
    id INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    email VARCHAR(150) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    telefono VARCHAR(20),
    direccion VARCHAR(255),
    rol_id INT NOT NULL,
    fecha_creacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (rol_id) REFERENCES roles(id)
);

-- Tabla de Productos
CREATE TABLE productos (
    id INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(150) NOT NULL,
    descripcion TEXT,
    precio DECIMAL(10, 2) NOT NULL,
    stock INT NOT NULL,
    fecha_lanzamiento DATE,
    tipo VARCHAR(50) CHECK (tipo IN ('juego', 'software')),
    imagen_url VARCHAR(255)
);

-- Tabla de Carritos
CREATE TABLE carritos (
    id INT PRIMARY KEY IDENTITY(1,1),
    usuario_id INT NOT NULL,
    fecha_creacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id)
);

-- Tabla de Carrito_Productos (relación entre Carritos y Productos)
CREATE TABLE carrito_productos (
    id INT PRIMARY KEY IDENTITY(1,1),
    carrito_id INT NOT NULL,
    producto_id INT NOT NULL,
    cantidad INT NOT NULL,
    FOREIGN KEY (carrito_id) REFERENCES carritos(id),
    FOREIGN KEY (producto_id) REFERENCES productos(id)
);

-- Tabla de Pedidos
CREATE TABLE pedidos (
    id INT PRIMARY KEY IDENTITY(1,1),
    usuario_id INT NOT NULL,
    fecha_pedido DATETIME DEFAULT GETDATE(),
    total DECIMAL(10, 2) NOT NULL,
    estado VARCHAR(50) CHECK (estado IN ('pendiente', 'completado', 'cancelado')),
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id)
);

-- Tabla de Pedido_Productos (relación entre Pedidos y Productos)
CREATE TABLE pedido_productos (
    id INT PRIMARY KEY IDENTITY(1,1),
    pedido_id INT NOT NULL,
    producto_id INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10, 2) NOT NULL,
    precio_total AS (cantidad * precio_unitario) PERSISTED, -- Calcula el precio total como cantidad * precio unitario
    FOREIGN KEY (pedido_id) REFERENCES pedidos(id),
    FOREIGN KEY (producto_id) REFERENCES productos(id)
);

-- Tabla de Métodos de Pago
CREATE TABLE metodos_pago (
    id INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL
);

-- Tabla de Pedidos_MetodosPago (relación entre Pedidos y Métodos de Pago)
CREATE TABLE pedidos_metodos_pago (
    id INT PRIMARY KEY IDENTITY(1,1),
    pedido_id INT NOT NULL,
    metodo_pago_id INT NOT NULL,
    FOREIGN KEY (pedido_id) REFERENCES pedidos(id),
    FOREIGN KEY (metodo_pago_id) REFERENCES metodos_pago(id)
);
