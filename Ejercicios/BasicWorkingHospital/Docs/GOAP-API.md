# GOAP API - Clases, metodos y propiedades

Fuente principal: `Docs/GOAP.md`
Codigo fuente: `Assets/Scripts/GOAP`
Documento tecnico complementario: `Docs/GOAP-TECH.md`
Catalogo de implementaciones (hospital): `Docs/GOAP-Hospital-Catalog.md`

## 1) Descripcion del modulo

El modulo GOAP (Goal Oriented Action Planning) implementa un sistema de planificacion para agentes NPC en Unity.

Objetivo del modulo:
- Elegir una meta (`SubGoal`) priorizada.
- Construir un plan (`Queue<GAction>`) con el menor costo posible mediante `GPlanner`.
- Ejecutar cada accion (`GAction`) en el ciclo del agente (`GAgent`).
- Mantener estados de mundo y creencias por medio de `WorldStates` y `GWorld`.
- Gestionar recursos compartidos (colas de pacientes, cubiculos, oficinas, etc.) con `ResourceQueue`.

Arquitectura general:
- `GAgent` coordina el flujo completo: seleccion de meta, planificacion y ejecucion.
- `GPlanner` arma el grafo de acciones y selecciona la ruta mas barata.
- `GAction` define la interfaz base para acciones concretas (`PrePerform` y `PostPerform`).
- `WorldStates` encapsula estados clave-valor.
- `GWorld` centraliza estado global y recursos compartidos.
- `GStateMonitor` aplica degradacion de estado y crea recursos dinamicos cuando corresponde.
- `GInventory` administra objetos del inventario del agente.

---

## 2) Desglose de clases, metodos y propiedades

### `Assets/Scripts/GOAP/WorldStates.cs`

#### Clase `WorldState`
Descripcion: DTO serializable para exponer estados en inspector.

Propiedades:
- `string key`
- `int value`

Metodos:
- No tiene.

#### Clase `WorldStates`
Descripcion: contenedor y utilidades para estados del mundo/creencias.

Propiedades:
- `Dictionary<string, int> states`

Metodos:
- `WorldStates()`
- `bool HasState(string key)`
- `void ModifyState(string key, int value)`
- `void RemoveState(string key)`
- `void SetState(string key, int value)`
- `Dictionary<string, int> GetStates()`

Metodos privados:
- `void AddState(string key, int value)`

---

### `Assets/Scripts/GOAP/GInventory.cs`

#### Clase `GInventory`
Descripcion: inventario simple del agente basado en `List<GameObject>`.

Propiedades:
- `List<GameObject> items`

Metodos:
- `void AddItem(GameObject i)`
- `GameObject FindItemWithTag(string tag)`
- `void RemoveItem(GameObject i)`

---

### `Assets/Scripts/GOAP/GAction.cs`

#### Clase abstracta `GAction : MonoBehaviour`
Descripcion: contrato base para una accion GOAP ejecutable por un `GAgent`.

Propiedades:
- `string actionName`
- `float cost`
- `GameObject target`
- `string targetTag`
- `float duration`
- `WorldState[] preConditions`
- `WorldState[] afterEffects`
- `NavMeshAgent agent`
- `Dictionary<string, int> preconditions`
- `Dictionary<string, int> effects`
- `WorldStates agentBeliefs`
- `GInventory inventory`
- `WorldStates beliefs`
- `bool running`

Metodos:
- `GAction()`
- `bool IsAchievable()`
- `bool IsAhievableGiven(Dictionary<string, int> conditions)`
- `abstract bool PrePerform()`
- `abstract bool PostPerform()`

Metodos de Unity:
- `Awake()` (inicializa referencias y carga diccionarios de precondiciones/efectos)

---

### `Assets/Scripts/GOAP/GPlanner.cs`

#### Clase `Node`
Descripcion: nodo del grafo de planificacion.

Propiedades:
- `Node parent`
- `float cost`
- `Dictionary<string, int> state`
- `GAction action`

Constructores:
- `Node(Node parent, float cost, Dictionary<string, int> allStates, GAction action)`
- `Node(Node parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> beliefStates, GAction action)`

#### Clase `GPlanner`
Descripcion: construye planes desde estado inicial hasta meta.

Metodos:
- `Queue<GAction> plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefStates)`

Metodos privados:
- `bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, int> goal)`
- `List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)`
- `bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)`

---

### `Assets/Scripts/GOAP/GWorld.cs`

#### Clase `ResourceQueue`
Descripcion: cola de recursos compartidos (`Queue<GameObject>`) con soporte de inicializacion por tag.

Propiedades:
- `string tag`
- `string modSate`

Metodos:
- `ResourceQueue(string t, string ms, WorldStates w)`
- `void AddResource(GameObject r)`
- `void RemoveResource(GameObject r)`
- `GameObject RemoveResource()`

Campos privados relevantes:
- `Queue<GameObject> queue`

#### Clase `GWorld`
Descripcion: singleton global para estados y colas de recursos.

Propiedades:
- `static GWorld Instance`

Metodos:
- `ResourceQueue GetQueue(string r)`
- `WorldStates GetWorld()`

Campos estaticos relevantes:
- `WorldStates world`
- `Dictionary<string, ResourceQueue> resources`
- colas: `patients`, `cubicles`, `offices`, `toilets`, `puddles`

Notas:
- El constructor estatico crea colas iniciales y ajusta `Time.timeScale = 5.0f`.

---

### `Assets/Scripts/GOAP/GAgent.cs`

#### Clase `SubGoal`
Descripcion: representa una meta concreta con bandera de eliminacion al cumplirse.

Propiedades:
- `Dictionary<string, int> sGoals`
- `bool remove`

Metodos:
- `SubGoal(string s, int i, bool r)`

#### Clase `GAgent : MonoBehaviour`
Descripcion: orquestador principal de decisiones y ejecucion de acciones.

Propiedades:
- `List<GAction> actions`
- `Dictionary<SubGoal, int> goals`
- `GInventory inventory`
- `WorldStates beliefs`
- `GAction currentAction`

Campos privados relevantes:
- `GPlanner planner`
- `Queue<GAction> actionQueue`
- `SubGoal currentGoal`
- `Vector3 destination`
- `bool invoked`

Metodos:
- `void CompleteAction()`

Metodos de Unity:
- `protected virtual void Start()`
- `void LateUpdate()`

---

### `Assets/Scripts/GOAP/GStateMonitor.cs`

#### Clase `GStateMonitor : MonoBehaviour`
Descripcion: monitoriza un estado de creencia temporal y genera recurso cuando se agota.

Propiedades:
- `string state`
- `float stateStrength`
- `float stateDecayRate`
- `WorldStates beliefs`
- `GameObject resourcePrefab`
- `string queueName`
- `string worldState`
- `GAction action`

Campos privados relevantes:
- `bool stateFound`
- `float initialStrength`

Metodos de Unity:
- `void Awake()`
- `void LateUpdate()`
