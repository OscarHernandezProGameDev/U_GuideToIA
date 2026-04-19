# GOAP TECH - Arquitectura, flujos y consideraciones

Fuente principal: `Docs/GOAP.md`
API complementaria: `Docs/GOAP-API.md`
Codigo fuente: `Assets/Scripts/GOAP`

## 1) Flujo tecnico principal (runtime)

1. `GAgent.Start()` descubre todas las `GAction` adjuntas al mismo GameObject.
2. En `GAgent.LateUpdate()`:
   - si hay accion corriendo, verifica distancia al destino y completa con `Invoke("CompleteAction", duration)`.
   - si no hay plan, crea `GPlanner` y prueba metas por prioridad (`goals` descendente).
3. `GPlanner.plan(...)`:
   - filtra acciones alcanzables (`IsAchievable`).
   - crea nodo inicial con estado global (`GWorld`) + creencias del agente.
   - expande el grafo recursivamente (`BuildGraph`) aplicando precondiciones y efectos.
   - elige hoja de menor costo y devuelve `Queue<GAction>`.
4. `GAgent` extrae acciones de la cola, ejecuta `PrePerform()`, resuelve target y mueve el `NavMeshAgent`.
5. al terminar duracion de accion, `CompleteAction()` llama `PostPerform()` y libera la ejecucion.

## 2) Interaccion entre estados

- Estado global: `GWorld.GetWorld()` (`WorldStates` compartido).
- Estado local de agente: `GAgent.beliefs`.
- El plan parte de la combinacion de ambos estados en `Node` inicial.
- `GStateMonitor` puede:
  - remover creencias locales (`beliefs.RemoveState(state)`),
  - crear recursos (`Instantiate(resourcePrefab, ...)`),
  - encolar recursos globales (`GWorld.Instance.GetQueue(queueName).AddResource(...)`),
  - incrementar estado global (`ModifyState(worldState, 1)`).

## 3) Dependencias tecnicas

- Unity Engine (`MonoBehaviour`, `GameObject`, `Transform`, `Time`, `Debug`, `Invoke`, `Instantiate`).
- Unity NavMesh (`UnityEngine.AI.NavMeshAgent`) para movimiento.
- LINQ en `GAgent` (orden de metas) y `ResourceQueue.RemoveResource(GameObject)`.
- Convencion de escena por tags (`Cubicle`, `Office`, `Toilet`, `Puddle`, etc.).

## 4) Contratos de extension para nuevas acciones

Para crear una accion nueva:
1. Heredar de `GAction`.
2. Definir precondiciones/efectos por inspector (`preConditions`, `afterEffects`) o por codigo.
3. Implementar:
   - `PrePerform()` para reservar recursos/validar contexto,
   - `PostPerform()` para aplicar resultados/liberar recursos.
4. Asegurar `target` o `targetTag` valido para navegacion.

## 5) Riesgos, limites y observaciones de implementacion

- `GPlanner.GoalAchieved(...)` valida solo existencia de keys, no compara valores esperados.
- `GPlanner.BuildGraph(...)` agrega efectos solo si la key no existe; no actualiza valores existentes.
- `GAction.IsAhievableGiven(...)` y `GWorld.modSate` tienen errores tipograficos en nombres, pero funcionalmente compilan si se usan de forma consistente.
- `GInventory.RemoveItem(...)` puede eliminar el ultimo elemento si no encuentra el objeto (indice termina en `items.Count - 1`).
- `GWorld.GetQueue(string)` usa acceso directo por key; keys invalidas lanzan excepcion.
- `Time.timeScale = 5.0f` en `GWorld` afecta toda la simulacion global.

## 6) Recomendaciones tecnicas

- Validar valores en metas/efectos (no solo presencia de estado).
- Reemplazar accesos por key directa con `TryGetValue` donde aplique.
- Corregir `RemoveItem` para no borrar cuando no hay coincidencia.
- Definir convenciones de nombres de estados y tags en un catalogo central.
- Cubrir `GPlanner` y `WorldStates` con tests unitarios de regresion.

