# Tareas Pendientes

## 1. Autenticación y Autorización
- [ ] Integración con Keycloak para autenticación OAuth2/OIDC
- [ ] Validación de roles (específicamente rol "Subastador")
- [ ] Verificación de propiedad del producto para edición/eliminación

## 2. Validaciones de Negocio
- [ ] Verificación contra el Microservicio de Subastas para productos en subasta
- [ ] Validación de que solo se puedan editar productos en estados permitidos
- [ ] Verificación de que solo se puedan eliminar productos no asociados a subastas

## 3. Gestión de Imágenes
- [ ] Integración con Firebase Storage para almacenamiento de imágenes
- [ ] Lógica para subir/eliminar imágenes en Firebase

## 4. Consultas y Filtros
- [ ] Implementación de filtros avanzados (categoría, rango de precios, estado)
- [ ] Búsqueda textual por nombre/descripción
- [ ] Ordenamiento de resultados
- [ ] Paginación de resultados (20 ítems por defecto)

## 5. Integración con Subastas
- [ ] Endpoint/evento para verificar elegibilidad de productos para subastas
- [ ] Sincronización de estados entre productos y subastas

## 6. Manejo de Errores y Validaciones
- [ ] Validación de datos de entrada
- [ ] Manejo adecuado de errores y códigos de estado HTTP
- [ ] Mensajes de error descriptivos

## 8. Pruebas
- [ ] Pruebas unitarias (cobertura ≥ 90%)
- [ ] Pruebas de integración
- [ ] Pruebas end-to-end (cobertura ≥ 80%)

