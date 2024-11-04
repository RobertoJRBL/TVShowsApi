# TV Shows API

## Descripción

La API de TV Shows permite gestionar una lista de programas de televisión, permitiendo realizar operaciones CRUD (Crear, Leer, Actualizar y Eliminar).

## Instrucciones para Configuración de la Base de Datos

1. Clona el repositorio y navega al directorio donde se encuentra el archivo `DB_TvShows.sql`.
   
2. Abre SQL Server Management Studio (SSMS) o tu herramienta de gestión SQL preferida.

3. Carga el archivo `DB_TvShows.sql` y ejecútalo. Esto creará:
   - La base de datos `TvShowsDB`
   - La tabla `TvShows`
   - Los datos iniciales de prueba
   - Los procedimientos almacenados necesarios para la API.

# TV Shows API

API para gestionar una lista de programas de televisión, permitiendo realizar operaciones CRUD (Crear, Leer, Actualizar y Eliminar) con soporte para paginación, búsqueda y filtrado por favoritos.

## Endpoints

1. GET `/api/tvshows`

   Obtiene una lista de programas de televisión con soporte para paginación, búsqueda y filtro por favoritos.

   - Parámetros de consulta (Query Params):
     - `isFavorite` (opcional): Filtra los programas por favoritos (`true` o `false`).
     - `search` (opcional): Filtra los programas cuyo nombre contenga el texto proporcionado.
     - `pageNumber` (opcional): Número de página para la paginación (por defecto, `1`).
     - `pageSize` (opcional): Tamaño de la página para la paginación (por defecto, `10`).

   - Ejemplo de solicitud:
     ```http
     GET /api/tvshows?isFavorite=true&search=El&pageNumber=1&pageSize=5
     ```

   - Respuesta exitosa (200):
     ```json
     [
       {
         "id": 1,
         "name": "El Chavo del 8",
         "favorite": true
       },
       {
         "id": 2,
         "name": "El Juego de las Llaves",
         "favorite": true
       }
     ]
     ```

2. POST `/api/tvshows`

   Agrega un nuevo programa de televisión.

   - Cuerpo de la solicitud (Payload):
     ```json
     {
       "name": "Nuevo Programa",
       "favorite": true
     }
     ```

   - Respuesta exitosa (200):
     ```json
     {
       "message": "El programa se agregó correctamente."
     }
     ```

   - Respuesta en caso de conflicto (409):
     ```json
     {
       "message": "El programa ya existe y no puede ser duplicado."
     }
     ```

3. PUT `/api/tvshows/{id}`

   Actualiza un programa de televisión existente.

   - Parámetro de ruta: `id` - El ID del programa que se desea actualizar.

   - Cuerpo de la solicitud (Payload):
     ```json
     {
       "name": "Programa Actualizado",
       "favorite": false
     }
     ```

   - Respuesta exitosa (200):
     ```json
     {
       "message": "El programa se actualizó correctamente."
     }
     ```

   - Respuesta en caso de conflicto (409):
     ```json
     {
       "message": "El programa ya existe y no puede ser duplicado."
     }
     ```

   - Respuesta en caso de no encontrar el programa (404):
     ```json
     {
       "message": "No fue encontrado el programa de televisión."
     }
     ```

4. DELETE `/api/tvshows/{id}`

   Elimina un programa de televisión por su ID.

   - Parámetro de ruta: `id` - El ID del programa que se desea eliminar.

   - Respuesta exitosa (200):
     ```json
     {
       "message": "El programa se eliminó correctamente."
     }
     ```

   - Respuesta en caso de no encontrar el programa (404):
     ```json
     {
       "message": "No fue encontrado el programa de televisión."
     }
     ```
     
## Notas

- Asegúrate de tener la base de datos configurada correctamente antes de utilizar la API.
- Los procedimientos almacenados gestionan conflictos y errores, devolviendo mensajes claros sobre el estado de las operaciones.

