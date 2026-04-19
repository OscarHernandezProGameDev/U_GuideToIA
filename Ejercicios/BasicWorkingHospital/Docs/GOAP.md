# GOAP - Indice de documentacion

Directorio de codigo: `Assets/Scripts/GOAP`

Este archivo ahora funciona como punto de entrada para la documentacion del modulo GOAP.

## Documentos principales

- API (clases, metodos, propiedades): `Docs/GOAP-API.md`
- Tecnico (arquitectura, flujos, dependencias, riesgos): `Docs/GOAP-TECH.md`
- Sistema de agentes (roles, metas y ciclo de decision): `Docs/GOAP-Agents.md`
- Catalogo hospital (implementaciones concretas de agentes y acciones): `Docs/GOAP-Hospital-Catalog.md`

## Alcance de cada documento

- `Docs/GOAP-API.md`
  - descripcion del modulo,
  - desglose por archivo y clase,
  - inventario de metodos y propiedades.
- `Docs/GOAP-TECH.md`
  - flujo runtime,
  - interaccion de estados,
  - dependencias tecnicas,
  - riesgos y recomendaciones.
- `Docs/GOAP-Agents.md`
  - arquitectura del sistema de agentes,
  - ciclo de decision/ejecucion en `GAgent`,
  - roles (`Patient`, `Nurse`, `Doctor`, `Janitor`) y sus metas.
- `Docs/GOAP-Hospital-Catalog.md`
  - clasificacion de clases de `Assets/Scripts` por tipo (`GAgent` / `GAction`),
  - objetivo funcional de cada clase,
  - metodos clave y estados/colas afectados.

## Mapeo de contenido (version anterior -> version actual)

- Seccion `1) Descripcion` -> `Docs/GOAP-API.md`
- Seccion `2) Desglose de clases, metodos y propiedades` -> `Docs/GOAP-API.md`
- Seccion `3) Documentacion tecnica` -> `Docs/GOAP-TECH.md`

## Nota de migracion

- Motivo: separar API y detalle tecnico para facilitar mantenimiento y consulta.
- Estado: `Docs/GOAP.md` queda como indice oficial de navegacion.
