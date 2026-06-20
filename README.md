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
1. Antes de ejecutar el proyecto, necesitas tener instalada la herramienta global de Entity Framework Core en tu sistema operativo para poder gestionar las migraciones de la base de datos:
2. Clonar el repositorio.
3. Ejecutar las migraciones para crear la base de datos local:
```bash
   dotnet ef database update
## 🛠️ Requisitos previos



```