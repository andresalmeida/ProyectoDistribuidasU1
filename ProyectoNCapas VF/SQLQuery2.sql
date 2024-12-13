SELECT TOP (1000) [UserID]
      ,[UserName]
      ,[Email]
      ,[PasswordHash]
      ,[Role]
      ,[AccountLocked]
      ,[CreatedAt]
      ,[Status]
  FROM [Sales_DB].[dbo].[UserAccount]

SELECT UserID, Email, PasswordHash, AccountLocked, Status
FROM UserAccount
WHERE Email = 'almeidaandres12@gmail.com';


DELETE FROM UserAccount
WHERE Email = 'asalmeida4@espe.edu.ec';

DELETE FROM UserAccount
WHERE Email = 'andres_jara154@hotmail.com';


INSERT INTO Category (CategoryName, Description)
VALUES
('Electr�nica', 'Dispositivos y gadgets'),
('Ropa', 'Prendas de vestir y accesorios'),
('Libros', 'Material educativo y libros'),
('Electrodom�sticos', 'Aparatos para el hogar'),
('Deportes', 'Equipos deportivos y para exteriores');


INSERT INTO Product (ProductName, CategoryID, UnitPrice, UnitsInStock, UserID)
VALUES
-- Productos de Electr�nica
('Tel�fono Inteligente', 1, 699.99, 50, 13),
('Laptop', 1, 1200.00, 30, 13),
('Auriculares Bluetooth', 1, 59.99, 100, 12),
('C�mara Digital', 1, 450.00, 15, 12),
('Tablet', 1, 300.00, 25, 12),

-- Productos de Ropa
('Camisa Casual', 2, 25.00, 200, 13),
('Pantal�n de Mezclilla', 2, 40.00, 150, 13),
('Zapatos Deportivos', 2, 60.00, 80, 12),
('Abrigo de Invierno', 2, 120.00, 40, 12),
('Sombrero de Verano', 2, 15.00, 100, 13),

-- Productos de Libros
('Introducci�n a la Programaci�n', 3, 35.00, 60, 12),
('Historia Universal', 3, 25.00, 80, 12),
('Cocina Gourmet', 3, 50.00, 20, 13),
('Matem�ticas Avanzadas', 3, 45.00, 30, 12),
('Gu�a de Viajes', 3, 20.00, 50, 13),

-- Productos de Electrodom�sticos
('Refrigerador', 4, 800.00, 10, 12),
('Horno Microondas', 4, 120.00, 20, 12),
('Lavadora', 4, 500.00, 8, 12),
('Aspiradora', 4, 150.00, 25, 13),
('Cafetera', 4, 70.00, 40, 13),

-- Productos de Deportes
('Bicicleta de Monta�a', 5, 500.00, 10, 13),
('Pelota de F�tbol', 5, 25.00, 50, 13),
('Raqueta de Tenis', 5, 80.00, 30, 12),
('Set de Pesas', 5, 100.00, 20, 12),
('Cinta de Correr', 5, 900.00, 5, 13);
