### Requisitos y funcionalidades del **Microservicio de Productos**


#### 1. Registro (alta) de productos

* **Ruta / Comando “Registrar Producto”**
* Campos obligatorios: nombre, descripción, imágenes, precio base y categoría.
* Validaciones de dominio: todos los campos requeridos completos y valores coherentes.
* **Restricción de rol:** sólo usuarios con rol **Subastador** autenticados por Keycloak.
* Persistencia en **PostgreSQL** (datos) y **Firebase Storage** (imágenes).
* Publicar evento **ProductCreated** por RabbitMQ / MassTransit para notificar a otros microservicios.


#### 2. Edición de productos

* **Ruta / Comando “Editar Producto”**.
* Sólo lo puede ejecutar el **propietario** del producto.
* Prohibido editar productos vinculados a una subasta **activa o finalizada** (verificación contra Microservicio de Subastas).
* Actualiza información en BD y emite evento **ProductUpdated**.


#### 3. Eliminación de productos

* **Ruta / Comando “Eliminar Producto”**.
* Requiere confirmación explícita.
* Sólo permitido si el producto **no** está asociado a ninguna subasta en curso o pasada.
* Borra los metadatos y las imágenes (hard-delete) o marca como “Deleted” (soft-delete) según política de retención.
* Emite evento **ProductDeleted**.


#### 4. Consulta y gestión del inventario de productos

* **Ruta / Query “Listar Productos”** con filtros: categoría, precio mín/max, estado.
* Ordenamiento y búsqueda textual por nombre / descripción.
* Sólo devuelve los productos del subastador autenticado.
* Respuesta paginada (por defecto 20 ítems).


#### 5. Reglas de negocio compartidas

1. **Integridad referencial con Subastas**

   * El microservicio debe exponer un endpoint o evento para que el MS de Subastas verifique la elegibilidad de un producto antes de crear la subasta.
2. **Estados del producto**: *Draft* → *Ready* → *InAuction* → *Sold* / *Deleted*.
3. **Immutabilidad parcial**: cuando el estado es **InAuction** o **Sold**, sólo se permite lectura.
4. **Consistencia eventual**: los cambios se propagan vía eventos; consumidores idempotentes.

#### 6. Requisitos no funcionales específicos

| Categoría      | Requisito                                                                             |
| -------------- | ------------------------------------------------------------------------------------- |
| Arquitectura   | Hexagonal (puertos y adaptadores), CQRS + Mediator, .NET Core 8                       |
| Persistencia   | Postgres (datos relacionales), Firebase Storage (blobs)                               |
| Seguridad      | OAuth2 / OIDC con Keycloak – scopes basados en rol                                    |
| Mensajería     | MassTransit + RabbitMQ – colas “product-events”                                       |
| Observabilidad | Logging estructurado (Serilog), métricas Prometheus, trazas OpenTelemetry             |
| Rendimiento    | ≤ 200 ms P95 para consultas de listado (< 100 registros)                              |
| Pruebas        | Cobertura ≥ 90 % en unitarias de dominio y 80 % en end-to-end                         |
| Deploy         | Contenedor Docker; orquestado por YARP/API-Gateway; pipelines CI/CD en GitHub Actions |
