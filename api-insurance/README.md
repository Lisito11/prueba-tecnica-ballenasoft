# Api Insurance

Es una api que se encarga del mantenimiento de una aseguradora. Crear, editar, eliminar y listar. 

## Tecnologías

- NET 6
- Entity Framework Core
- Automapper
- Sqlite
- MsTest
- Swagger

## Correr el proyecto

### Visual studio

Se puede correr cualquiera de los proyectos de manera gráfica usando Visual Studio. Haciendo un build del proyecto.

### Consola

#### Api

```bash
solution/api/
```

```bash
dotnet run 
```
**Swagger**:
https://localhost:PORT/swagger/index.html

**Api**:
https://localhost:PORT/api/insurance

Enviar en los headers {token: 1234} 

#### Api-Test

```bash
solution/api-test/
```
```bash
dotnet test 
```

## Licencia

[MIT](https://choosealicense.com/licenses/mit/)
