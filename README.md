# Bookify API

API Web construida con **ASP.NET Core** para conceptos de desarrollo Backend, manejo de bases de datos relacionales ligeras y relaciones entre entidades.

## Tecnologías y Herramientas
* **Framework:** ASP.NET Core 10
* **ORM:** Entity Framework Core
* **Base de Datos:** SQLite

## Modelo de Datos
El proyecto maneja una relación de **Uno a Muchos (1:N)**:
* **Autor:** Un autor puede tener muchos libros registrados.
* **Libro:** Cada libro pertenece obligatoriamente a un autor.

##  Como ejecutar localmente
1. Clonar el repositorio.
2. Ejecutar las migraciones para crear la base de datos local:
```bash
   dotnet ef database update