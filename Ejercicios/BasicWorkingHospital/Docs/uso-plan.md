# Elementos usados por `GPlanner.plan(...)`

Archivo fuente analizado: `Assets/Scripts/GOAP/GPlanner.cs`  
Metodo: `public Queue<GAction> plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefStates)`

## 1) Clases y tipos utilizados

- `GPlanner` (clase que contiene el metodo `plan`)
- `GAction`
- `WorldStates`
- `Node`
- `GWorld`
- `Queue<T>` (`Queue<GAction>`)
- `List<T>` (`List<GAction>`, `List<Node>`)
- `Dictionary<TKey, TValue>` (`Dictionary<string, int>`)
- `Debug` (`UnityEngine.Debug`)

## 2) Miembros (propiedades/campos) usados por `plan`

### De `Node`

- `Node.cost`
- `Node.action`
- `Node.parent`

### De `GAction`

- `GAction.actionName`

### De `GWorld`

- `GWorld.Instance` (propiedad estatica singleton)

> Nota: en el metodo `plan` no se accede directamente a mas campos de `GAction`; solo a `actionName` para logging y a metodos (listados abajo).

## 3) Metodos usados por `plan`

### Metodos de `GPlanner` (internos)

- `BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, int> goal)`

### Metodos de `GAction`

- `IsAchievable()`

### Metodos de `WorldStates`

- `GetStates()`

### Metodos de `GWorld` / cadena de llamada

- `GWorld.Instance.GetWorld().GetStates()`

### Metodos/constructores de `Node`

- Constructor: `Node(Node parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> beliefStates, GAction action)`

### Metodos de `Debug`

- `Debug.Log(...)`

### Metodos de colecciones (`System.Collections.Generic`)

- `List<GAction>.Add(...)`
- `List<GAction>.Insert(...)`
- `Queue<GAction>.Enqueue(...)`
- Iteracion `foreach` sobre `List<>` y `Queue<>`

## 4) Flujo resumido de uso en `plan`

1. Filtra `actions` llamando `a.IsAchievable()` para construir `usableActions`.
2. Crea nodo inicial `start` con:
   - estados del mundo: `GWorld.Instance.GetWorld().GetStates()`
   - estados de creencias: `beliefStates.GetStates()`
3. Llama a `BuildGraph(...)` para generar hojas (`leaves`) con planes posibles.
4. Si no hay plan (`success == false`), hace `Debug.Log("NO PLAN")` y devuelve `null`.
5. Selecciona la hoja mas barata comparando `Node.cost`.
6. Reconstruye el plan recorriendo `Node.parent` y tomando `Node.action`.
7. Encola acciones en `Queue<GAction>` y loggea cada `GAction.actionName`.
8. Devuelve `Queue<GAction>`.

