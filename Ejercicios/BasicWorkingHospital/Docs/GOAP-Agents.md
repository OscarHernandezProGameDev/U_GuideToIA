# GOAP Hospital - Sistema de agentes

Codigo base:
- `Assets/Scripts/GOAP/GAgent.cs`
- `Assets/Scripts/GOAP/GPlanner.cs`
- `Assets/Scripts/GOAP/GAction.cs`

Roles del hospital:
- `Assets/Scripts/Patient.cs`
- `Assets/Scripts/Nurse.cs`
- `Assets/Scripts/Doctor.cs`
- `Assets/Scripts/Janitor.cs`

Documentos relacionados:
- Indice: `Docs/GOAP.md`
- API base GOAP: `Docs/GOAP-API.md`
- Tecnico: `Docs/GOAP-TECH.md`
- Catalogo de clases: `Docs/GOAP-Hospital-Catalog.md`

## 1) Vision general

El sistema de agentes de GOAP Hospital usa `GAgent` como orquestador de decisiones.
Cada agente:
- mantiene un conjunto de metas (`SubGoal`) con prioridad,
- consulta el planificador `GPlanner` para construir una cola de acciones (`Queue<GAction>`),
- ejecuta acciones con `PrePerform()` y `PostPerform()`,
- actualiza creencias locales (`beliefs`) y estados globales (`GWorld`).

## 2) Ciclo de decision y ejecucion del agente

Flujo en `GAgent`:
1. `Start()` recopila las acciones `GAction` adjuntas al GameObject.
2. `LateUpdate()` controla la accion en curso:
   - si `currentAction.running == true`, supervisa llegada al destino y llama `CompleteAction()` tras `duration`.
3. Si no hay plan activo (`planner == null` o `actionQueue == null`):
   - ordena `goals` por prioridad descendente,
   - intenta `planner.plan(actions, goal, beliefs)` para cada meta,
   - selecciona la primera meta con plan viable.
4. Ejecuta la siguiente accion de `actionQueue`:
   - `currentAction = actionQueue.Dequeue()`,
   - valida `PrePerform()`,
   - resuelve `target`/`targetTag`,
   - mueve `NavMeshAgent` al destino.
5. `CompleteAction()` finaliza la accion:
   - `running = false`,
   - ejecuta `PostPerform()`,
   - libera bandera interna de invocacion.
6. Cuando la cola termina:
   - elimina meta si `remove == true`,
   - fuerza replanificacion poniendo `planner = null`.

## 3) Roles de agentes en GOAP Hospital

### `Patient` (`Assets/Scripts/Patient.cs`)

Objetivo funcional:
- recorrer el flujo de paciente: esperar, tratamiento y regreso a casa.

Metas configuradas:
- `isTreated` (prioridad 5, removible)
- `isWaiting` (prioridad 3, removible)
- `refief` (prioridad 2, no removible)
- `isHome` (prioridad 1, removible)

Dinamica de creencias:
- `NeedRefief()` inyecta `busting` periodicamente.

### `Nurse` (`Assets/Scripts/Nurse.cs`)

Objetivo funcional:
- atender pacientes y gestionar necesidades internas.

Metas configuradas:
- `refief` (prioridad 5, no removible)
- `treatPatient` (prioridad 3, no removible)
- `rested` (prioridad 1, no removible)

Dinamica de creencias:
- `GetTired()` agrega `exhausted`.
- `NeedRefief()` agrega `busting`.

### `Doctor` (`Assets/Scripts/Doctor.cs`)

Objetivo funcional:
- investigar y mantener estados internos (descanso/bano).

Metas configuradas:
- `rested` (prioridad 3, no removible)
- `refief` (prioridad 2, no removible)
- `research` (prioridad 1, no removible)

Dinamica de creencias:
- `GetTired()` agrega `exhausted`.
- `NeedRefief()` agrega `busting`.

### `Janitor` (`Assets/Scripts/Janitor.cs`)

Objetivo funcional:
- limpiar recursos de tipo `puddles`.

Metas configuradas:
- `clean` (prioridad 3, no removible)

Dinamica de creencias:
- metodos `GetTired()` y `NeedRefief()` existen, pero no se invocan en `Start()` actual.

## 4) Relacion entre agentes, planner y acciones

- `GAgent` delega planificacion a `GPlanner`.
- `GPlanner` evalua precondiciones de `GAction` y busca secuencia de menor costo.
- `GAction` aplica cambios de estado en `PostPerform()` y reserva/libera recursos en `PrePerform()`/`PostPerform()`.
- Recursos compartidos y contadores globales viven en `GWorld` (`patients`, `cubicles`, `offices`, `toilets`, `puddles`).
- Las creencias locales del agente viven en `WorldStates beliefs`.

## 5) Mapa de acciones por rol (resumen)

- Flujo paciente:
  - `GoToHospital` -> `Register` -> `GoToWaitingRoom` -> `GetTreated` -> `GoHome`
- Flujo enfermeria:
  - `GetPatient` -> `GoToCubicle` (+ `Rest`, `GoToToilet` segun creencias)
- Flujo doctor:
  - `Research` (+ `Rest`, `GoToToilet` segun creencias)
- Flujo janitor:
  - `CleanUpPuddle`

## 6) Observaciones tecnicas del sistema de agentes

- La seleccion de metas depende estrictamente de prioridad numerica.
- La replanificacion ocurre cuando no hay cola o falla `PrePerform()`.
- El control de recursos compartidos se basa en colas globales y estados `Free*`.
- El comportamiento emergente depende de creencias periodicas (`exhausted`, `busting`) y disponibilidad de recursos.

