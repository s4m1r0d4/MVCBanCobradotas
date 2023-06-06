--
-- File generated with SQLiteStudio v3.4.4 on Mon Jun 5 18:43:16 2023
--
-- Text encoding used: UTF-8
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Boleto
DROP TABLE IF EXISTS Boleto;

CREATE TABLE IF NOT EXISTS Boleto (
    IDBoleto   INTEGER PRIMARY KEY AUTOINCREMENT
                       NOT NULL,
    Fecha      TEXT    NOT NULL,
    IDPrestamo INTEGER REFERENCES Prestamo (IDPrestamo) ON DELETE SET NULL
                                                        ON UPDATE CASCADE
);


-- Table: CuentaBancaria
DROP TABLE IF EXISTS CuentaBancaria;

CREATE TABLE IF NOT EXISTS CuentaBancaria (
    IDCuentaBancaria INTEGER PRIMARY KEY AUTOINCREMENT
                             NOT NULL
                             CHECK (IDCuentaBancaria >= 0 AND 
                                    IDCuentaBancaria <= 9999999999),
    Saldo            REAL    CHECK (Saldo >= 0) 
                             NOT NULL
                             DEFAULT (10000) 
);

INSERT INTO CuentaBancaria (IDCuentaBancaria, Saldo) VALUES (1, 10000.0);

-- Table: CuentaIngreso
DROP TABLE IF EXISTS CuentaIngreso;

CREATE TABLE IF NOT EXISTS CuentaIngreso (
    IDCuentaIngreso    INTEGER PRIMARY KEY AUTOINCREMENT
                               NOT NULL,
    Usuario            TEXT    UNIQUE
                               NOT NULL,
    Contrasena         TEXT    NOT NULL,
    FechaInicioFallido TEXT,
    NumInicioFallido   INTEGER CHECK (NumInicioFallido >= 0) 
                               NOT NULL
                               DEFAULT (0) 
);

INSERT INTO CuentaIngreso (IDCuentaIngreso, Usuario, Contrasena, FechaInicioFallido, NumInicioFallido) VALUES (1, 'dummy', 'b5a2c96250612366ea272ffac6d9744aaf4b45aacd96aa7cfcb931ee3b558259', NULL, 0);
INSERT INTO CuentaIngreso (IDCuentaIngreso, Usuario, Contrasena, FechaInicioFallido, NumInicioFallido) VALUES (2, 'GIGB4372', '553369863d1fd1e052ee108199228c89342a02ae9e9bbe5b8ae333f0b163ca04', NULL, 0);
INSERT INTO CuentaIngreso (IDCuentaIngreso, Usuario, Contrasena, FechaInicioFallido, NumInicioFallido) VALUES (4, 'GIGB4371', 'c6d1caea58f949ae589a74157888acc73216602d49e541b4bb545954e1be19c7', NULL, 0);

-- Table: DiaVacacion
DROP TABLE IF EXISTS DiaVacacion;

CREATE TABLE IF NOT EXISTS DiaVacacion (
    IDDiaVacacion INTEGER PRIMARY KEY AUTOINCREMENT
                          NOT NULL,
    Fecha         TEXT    NOT NULL,
    IDGerente     INTEGER REFERENCES Gerente (IDGerente) ON DELETE CASCADE
                                                         ON UPDATE CASCADE
);


-- Table: Empleado
DROP TABLE IF EXISTS Empleado;

CREATE TABLE IF NOT EXISTS Empleado (
    IDEmpleado      INTEGER PRIMARY KEY AUTOINCREMENT
                            NOT NULL,
    Nombre          TEXT    NOT NULL,
    Apellido        TEXT    NOT NULL,
    FechaNacimiento TEXT    NOT NULL,
    IDCuentaIngreso INTEGER REFERENCES CuentaIngreso (IDCuentaIngreso) ON DELETE RESTRICT
                                                                       ON UPDATE CASCADE
                            NOT NULL
                            UNIQUE,
    IDNomina        INTEGER REFERENCES Nomina (IDNomina) ON DELETE RESTRICT
                                                         ON UPDATE CASCADE
                            NOT NULL
                            UNIQUE
);

INSERT INTO Empleado (IDEmpleado, Nombre, Apellido, FechaNacimiento, IDCuentaIngreso, IDNomina) VALUES (1, 'Giordanna Maria Goretti', 'García Bojorquez', '2004-04-23 00:00:00', 2, 1);

-- Table: Estado
DROP TABLE IF EXISTS Estado;

CREATE TABLE IF NOT EXISTS Estado (
    IDEstado    INTEGER PRIMARY KEY AUTOINCREMENT
                        NOT NULL,
    Descripcion TEXT    NOT NULL
);

INSERT INTO Estado (IDEstado, Descripcion) VALUES (1, 'En espera');
INSERT INTO Estado (IDEstado, Descripcion) VALUES (2, 'Aceptado');
INSERT INTO Estado (IDEstado, Descripcion) VALUES (3, 'Rechazado');
INSERT INTO Estado (IDEstado, Descripcion) VALUES (4, 'Cancelado');
INSERT INTO Estado (IDEstado, Descripcion) VALUES (5, 'Pausado');
INSERT INTO Estado (IDEstado, Descripcion) VALUES (6, 'Finalizado');

-- Table: Gerente
DROP TABLE IF EXISTS Gerente;

CREATE TABLE IF NOT EXISTS Gerente (
    IDGerente        INTEGER PRIMARY KEY AUTOINCREMENT
                             NOT NULL,
    Nombre           TEXT    NOT NULL,
    Apellido         TEXT    NOT NULL,
    FechaNacimiento  TEXT    NOT NULL,
    IDNomina         INTEGER NOT NULL
                             REFERENCES Nomina (IDNomina) ON DELETE RESTRICT
                                                          ON UPDATE CASCADE
                             UNIQUE,
    IDCuentaIngreso  INTEGER REFERENCES CuentaIngreso (IDCuentaIngreso) ON DELETE RESTRICT
                                                                        ON UPDATE CASCADE
                             NOT NULL
                             UNIQUE,
    IDCuentaBancaria INTEGER REFERENCES CuentaBancaria (IDCuentaBancaria) ON DELETE RESTRICT
                                                                          ON UPDATE CASCADE
                             NOT NULL
                             UNIQUE
);

INSERT INTO Gerente (IDGerente, Nombre, Apellido, FechaNacimiento, IDNomina, IDCuentaIngreso, IDCuentaBancaria) VALUES (1, 'Giordanna Maria Goretti', 'García Bojorquez', '2004-04-23 00:00:00', 2, 4, 1);

-- Table: Nomina
DROP TABLE IF EXISTS Nomina;

CREATE TABLE IF NOT EXISTS Nomina (
    IDNomina     INTEGER PRIMARY KEY AUTOINCREMENT
                         NOT NULL,
    FechaIngreso TEXT    NOT NULL
);

INSERT INTO Nomina (IDNomina, FechaIngreso) VALUES (1, '2023-06-05 17:45:09.7199227');
INSERT INTO Nomina (IDNomina, FechaIngreso) VALUES (2, '2023-06-05 17:45:09.9694611');

-- Table: Pago
DROP TABLE IF EXISTS Pago;

CREATE TABLE IF NOT EXISTS Pago (
    IDPago     INTEGER PRIMARY KEY AUTOINCREMENT
                       NOT NULL,
    Fecha      TEXT    NOT NULL,
    Cantidad   REAL    NOT NULL
                       CHECK (Cantidad > 0),
    Numero     INTEGER NOT NULL
                       CHECK (Numero >= 1 AND 
                              Numero <= 36),
    IDPrestamo INTEGER REFERENCES Prestamo (IDPrestamo) ON DELETE SET NULL
                                                        ON UPDATE CASCADE
);


-- Table: Prestamo
DROP TABLE IF EXISTS Prestamo;

CREATE TABLE IF NOT EXISTS Prestamo (
    IDPrestamo       INTEGER PRIMARY KEY AUTOINCREMENT
                             NOT NULL,
    FechaSolicitud   TEXT    NOT NULL,
    FechaAprobacion  TEXT,
    FechaLiquidacion TEXT,
    NumMeses         INTEGER NOT NULL
                             CHECK (NumMeses >= 6 AND 
                                    NumMeses <= 36),
    PagoMensual      REAL    NOT NULL
                             CHECK (PagoMensual > 0),
    Interes                  NOT NULL
                             CHECK (Interes > 10),
    Cantidad         REAL    NOT NULL
                             CHECK (Cantidad > 0),
    IDEstado         INTEGER REFERENCES Estado (IDEstado) ON DELETE SET NULL
                                                          ON UPDATE CASCADE
                             NOT NULL,
    IDCuentaBancaria INTEGER REFERENCES CuentaBancaria (IDCuentaBancaria) ON DELETE SET NULL
                                                                          ON UPDATE CASCADE
                             NOT NULL,
    IDNomina         INTEGER REFERENCES Nomina (IDNomina) ON DELETE SET NULL
                                                          ON UPDATE CASCADE
                             NOT NULL
);


-- Table: Usuario
DROP TABLE IF EXISTS Usuario;

CREATE TABLE IF NOT EXISTS Usuario (
    IDUsuario        INTEGER PRIMARY KEY AUTOINCREMENT
                             NOT NULL,
    Nombre           TEXT    NOT NULL,
    Apellido         TEXT    NOT NULL,
    FechaNacimiento  TEXT    NOT NULL,
    CURP             TEXT    NOT NULL,
    IDEstado         INTEGER REFERENCES Estado (IDEstado) ON DELETE RESTRICT
                                                          ON UPDATE CASCADE
                             NOT NULL,
    IDCuentaIngreso  INTEGER REFERENCES CuentaIngreso (IDCuentaIngreso) ON DELETE RESTRICT
                                                                        ON UPDATE CASCADE
                             UNIQUE,
    IDCuentaBancaria INTEGER REFERENCES CuentaBancaria (IDCuentaBancaria) ON DELETE RESTRICT
                                                                          ON UPDATE CASCADE
                             UNIQUE,
    IDNomina         INTEGER REFERENCES Nomina (IDNomina) ON DELETE SET NULL
                                                          ON UPDATE CASCADE
);

INSERT INTO Usuario (IDUsuario, Nombre, Apellido, FechaNacimiento, CURP, IDEstado, IDCuentaIngreso, IDCuentaBancaria, IDNomina) VALUES (1, 'Giordanna Maria Goretti', 'Garcia Bojorquez', '2004-04-23 00:00:00', 'GABG040423MJCRJRA2', 1, NULL, NULL, NULL);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
