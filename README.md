#Net Core Application

Permite buscar provincias de Argentina

#Stack
- NET Core: 5.0
- SQL : 2017 +

#Arquitectura

REST. 
Dividido en capas

- API : Cliente, controllers 
- Crosscutting: Herramientas cross a todas las capas
- Domain: Entidades
- Infrastructure: Acceso a la base de datos, DbContext, Migrations
- Security: Autenticación
- Services: Lógica de negocio
- Tests: Tests unitarios

#Aclaraciones

- Refit para conexión con servicios REST
- Serilog, Logging
- xUnit, Testing
- Inyección de dependencias
- Entity Framework Core, Code First
- JWT Middleware, Authentication
- Swagger, Documentación de APIs

#Uso
Al correr el proyecto se produce una migración automática de la migration "InitialCreate" y se inserta en la base el usuario de prueba
