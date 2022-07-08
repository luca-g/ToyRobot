# ToyRobot

You control a robot movements on a map using a Vue2 front end or a console application. 

The application is composed by 
- REST API using JWT authentication exposing objects and endpoints with Swagger
- SQL Server database project with models generated with EF Core Power Tools
- Vue2 project with networking code generated using OpenAPI tools
- Unit test projects

All projects are implemented following loosely coupled design, dependency inversion with all interactions done by interfaces and object factories. 

## Design patterns 

- Dependency injection
- Builder
- Factory
- Command
