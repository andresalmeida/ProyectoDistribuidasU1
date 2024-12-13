# Proyecto de Implementación de Capa de Seguridad

## 🚀 Descripción del Proyecto

Este proyecto implementa una capa de seguridad robusta para una aplicación web utilizando .NET Framework, con funcionalidades de autenticación, autorización y gestión de usuarios, productos y categorías.

## 📋 Objetivos

### Objetivo General
Desarrollar e implementar una capa de seguridad que proteja datos sensibles de usuarios, garantizando:
- Autenticación y autorización segura
- Control de acceso 
- Hashing de contraseñas
- Autenticación basada en tokens JWT
- Registro de auditoría

### Objetivos Específicos
- Implementar CRUD para productos
- Autenticación de usuarios con tokens JWT
- Protección de contraseñas mediante hashing seguro
- Control de acceso basado en roles
- Registro y auditoría de actividades críticas

## 🔐 Características de Seguridad

- **Autenticación JWT**: Tokens seguros para control de sesiones
- **Hashing de Contraseñas**: Uso de Bcrypt para almacenamiento seguro
- **Control de Roles**: Autorización basada en roles de usuario
- **Auditoría**: Registro de eventos críticos del sistema

## 🏗️ Arquitectura

Arquitectura de capas:
- **DAL (Data Access Layer)**: Comunicación con base de datos
- **BLL (Business Logic Layer)**: Reglas de negocio
- **SL (Service Layer)**: Servicios compartidos de autenticación y seguridad
- **Vistas**: Interfaces de usuario

## 🛠️ Tecnologías Utilizadas

- .NET Framework
- ASP.NET MVC
- Entity Framework
- JWT (JSON Web Tokens)
- Bcrypt
- SMTP para envío de correos
- SonarQube (análisis de código)

## 📦 Funcionalidades

### Gestión de Usuarios
- CRUD completo de usuarios
- Control de intentos de inicio de sesión
- Bloqueo de cuentas tras intentos fallidos

### Gestión de Productos y Categorías
- CRUD de productos
- CRUD de categorías
- Validaciones de datos
- Relaciones consistentes

## 🔒 Seguridad y Permisos

### Matriz de Roles
| Rol      | Permisos                           |
|----------|-------------------------------------|
| Admin    | CRUD total de usuarios, productos   |
| Editor   | CRUD parcial de productos           |
| Viewer   | Solo lectura                        |

## 🧪 Pruebas y Calidad

- Pruebas unitarias
- Análisis de código con SonarQube
- Verificación de:
  - Seguridad
  - Mantenibilidad
  - Duplicación de código
  - Cobertura de pruebas

## 📦 Instalación

1. Clonar el repositorio
2. Restaurar paquetes NuGet
3. Configurar cadena de conexión a base de datos
4. Configurar credenciales SMTP
5. Compilar y ejecutar

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Por favor, lee las guías de contribución antes de enviar un pull request.

## 👥 Autores

- Almeida Andrés
- Moncayo Paola
- Valdiviezo Darwin

## 🔗 Repositorio

[https://github.com/andresalmeida/ProyectoDistribuidasU1.git](https://github.com/andresalmeida/ProyectoDistribuidasU1.git)

---

**Nota**: Este proyecto fue desarrollado como parte del curso de Aplicaciones Distribuidas.
