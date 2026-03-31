# 📦 AppWebDespachos

**AppWebDespachos** es una solución integral para la gestión y seguimiento de logística de mercancías. Este proyecto fue desarrollado como parte de 
la cátedra de **Programación III** en la **UTN**, aplicando patrones de diseño modernos y persistencia de datos robusta.

---

## 🛠️ Stack Tecnológico

* **Lenguaje:** C#
* **Framework:** ASP.NET Core MVC 8.0
* **Base de Datos:** Microsoft SQL Server
* **ORM:** Entity Framework Core
* **Frontend:** Razor Pages, Bootstrap, CSS, JavaScript

---

## ⚙️ Configuración Local

Para ejecutar este proyecto en tu entorno de desarrollo:

1. **Clonar el repositorio:**
   ```bash
   git clone [https://github.com/tu-usuario/AppWebDespachos.git](https://github.com/tu-usuario/AppWebDespachos.git)
2. **Configurar la Base de Datos**
   *Localiza el archivo appsettings.Example.json en la raíz del proyecto.
   *Crea una copia y renómbrala a appsettings.json.
   *Edita la sección DefaultConnection con tu cadena de conexión local de SQL Server.
3. **Aplicar Migraciones**
   Desde la Consola de Administrador de Paquetes en Visual Studio, ejecuta:
    *Update-Database*
4. **Ejecutar** Presionar F5

----

## Gemini ha dicho
¡Ah, ya entiendo lo que está pasando! Te está fallando el renderizado del bloque de código porque el formato se está "rompiendo" a la mitad. Perdón por la confusión, Pablo.

Acá te lo paso de nuevo, pero dividido en bloques más cortos para que no se corte y puedas copiar una parte tras otra.

Parte 1: Título y Descripción (Copiá esto primero)
Markdown
# 📦 AppWebDespachos

[![.NET](https://img.shields.io/badge/.NET%208.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/sql-server)

**AppWebDespachos** es una solución integral para la gestión y seguimiento de logística de mercancías. Este proyecto fue desarrollado como parte de la cátedra de **Programación III** en la **UTN**, aplicando patrones de diseño modernos y persistencia de datos robusta.

---

## 🚀 Características Principales

* **Gestión de Despachos:** Control total sobre el flujo de salida y entrada de mercadería.
* **Arquitectura MVC:** Implementación del patrón Modelo-Vista-Controlador para una separación clara de responsabilidades.
* **Persistencia con EF Core:** Uso de **Entity Framework Core** (Code First) para el mapeo y gestión de la base de datos SQL Server.
* **Validaciones Dinámicas:** Reglas de negocio aplicadas mediante Data Annotations y ViewModels.
* **Interfaz Responsiva:** Diseño adaptable a diferentes resoluciones utilizando Bootstrap.

## 🛠️ Stack Tecnológico

* **Lenguaje:** C#
* **Framework:** ASP.NET Core MVC 8.0
* **Base de Datos:** Microsoft SQL Server
* **ORM:** Entity Framework Core
* **Frontend:** Razor Pages, Bootstrap, CSS, JavaScript
Parte 2: Configuración y Equipo (Pegalo debajo de lo anterior)
Markdown
---

## ⚙️ Configuración Local

Para ejecutar este proyecto en tu entorno de desarrollo:

1. **Clonar el repositorio:**
   ```bash
   git clone [https://github.com/tu-usuario/AppWebDespachos.git](https://github.com/tu-usuario/AppWebDespachos.git)
Configurar la Base de Datos:

Localiza el archivo appsettings.Example.json en la raíz del proyecto.

Crea una copia y renómbrala a appsettings.json.

Edita la sección DefaultConnection con tu cadena de conexión local de SQL Server.

Aplicar Migraciones:
Desde la Consola de Administrador de Paquetes en Visual Studio, ejecuta:

PowerShell
Update-Database
Ejecutar:
Presiona F5 o usa el comando dotnet run.

## 📁 Estructura del Proyecto
*Controllers/:* Manejo de peticiones y lógica de control.
*Models/:* Entidades de dominio y esquemas de base de datos.
*ViewModels/:* Modelos de datos específicos para las vistas.
*Views/:* Vistas dinámicas renderizadas con el motor Razor.
*Data/:* Contexto de datos (DbContext) y configuraciones.

----

## 👥 Equipo de Desarrollo
Este proyecto fue realizado de forma colaborativa por:
Mario Ricotti - github.com/mardigitals
Pablo Tocchetti - github.com/Tocchetti-Pablo

## Proyecto académico - UTN 2026
