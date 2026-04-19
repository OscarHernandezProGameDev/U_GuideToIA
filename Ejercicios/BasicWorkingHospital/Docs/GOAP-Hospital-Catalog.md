# GOAP Hospital - Catalogo de agentes y acciones

Codigo analizado: `Assets/Scripts`
Base GOAP: `Assets/Scripts/GOAP`
Indice general: `Docs/GOAP.md`
API base: `Docs/GOAP-API.md`
Tecnico: `Docs/GOAP-TECH.md`

## Criterio de clasificacion

- **Agente**: clase que hereda de `GAgent`.
- **Accion**: clase que hereda de `GAction`.

## 1) Agentes (`GAgent`)

### `Patient` - `Assets/Scripts/Patient.cs`

Descripcion:
- Agente paciente con ciclo completo: esperar, ser tratado y volver a casa.

Metodos clave:
- `Start()`
- `NeedRefief()`

Subgoals configurados:
- `isWaiting` (prioridad 3, removible)
- `isTreated` (prioridad 5, removible)
- `isHome` (prioridad 1, removible)
- `refief` (prioridad 2, no removible)

Creencias/estados relevantes:
- Agrega/renueva creencia `busting` periodicamente.

---

### `Nurse` - `Assets/Scripts/Nurse.cs`

Descripcion:
- Agente enfermera orientado a tratar pacientes, con necesidades de descanso y bano.

Metodos clave:
- `Start()`
- `GetTired()`
- `NeedRefief()`

Subgoals configurados:
- `treatPatient` (prioridad 3, no removible)
- `rested` (prioridad 1, no removible)
- `refief` (prioridad 5, no removible)

Creencias/estados relevantes:
- Agrega/renueva `exhausted` y `busting` en intervalos aleatorios.

---

### `Doctor` - `Assets/Scripts/Doctor.cs`

Descripcion:
- Agente doctor orientado a investigacion y mantenimiento de necesidades personales.

Metodos clave:
- `Start()`
- `GetTired()`
- `NeedRefief()`

Subgoals configurados:
- `research` (prioridad 1, no removible)
- `refief` (prioridad 2, no removible)
- `rested` (prioridad 3, no removible)

Creencias/estados relevantes:
- Agrega/renueva `exhausted` y `busting` en intervalos aleatorios.

---

### `Janitor` - `Assets/Scripts/Janitor.cs`

Descripcion:
- Agente de limpieza enfocado en mantener el hospital sin charcos.

Metodos clave:
- `Start()`
- `GetTired()` (definido, no activado en el flujo actual)
- `NeedRefief()` (definido, no activado en el flujo actual)

Subgoals configurados:
- `clean` (prioridad 3, no removible)

Creencias/estados relevantes:
- Tiene metodos para `exhausted` y `busting`, pero en el estado actual no se invocan.

## 2) Acciones (`GAction`)

### `GoToHospital` - `Assets/Scripts/GoToHospital.cs`

Descripcion:
- Accion de desplazamiento base hacia el hospital.

Metodos:
- `PrePerform()` -> `true`
- `PostPerform()` -> `true`

Efecto funcional:
- No modifica estados directamente; funciona como paso de plan.

---

### `Register` - `Assets/Scripts/Register.cs`

Descripcion:
- Accion de registro del paciente.

Metodos:
- `PrePerform()` -> `true`
- `PostPerform()` -> `beliefs.ModifyState("atHospital", 0)`

Efecto funcional:
- Marca/actualiza creencia de hospital en el agente.

---

### `GoToWaitingRoom` - `Assets/Scripts/GoToWaitingRoom.cs`

Descripcion:
- Lleva al paciente a sala de espera y lo mete en la cola global de pacientes.

Metodos:
- `PrePerform()` -> `true`
- `PostPerform()`

Cambios de estado/recursos:
- `GWorld.GetWorld().ModifyState("Waiting", 1)`
- `GWorld.GetQueue("patients").AddResource(gameObject)`
- `beliefs.ModifyState("atHospital", 1)`

---

### `GetPatient` - `Assets/Scripts/GetPatient.cs`

Descripcion:
- Accion de enfermeria: toma un paciente de cola y reserva cubiculo.

Metodos:
- `PrePerform()`
- `PostPerform()`

Cambios de estado/recursos:
- Toma paciente de `patients`.
- Reserva cubiculo de `cubicles` y lo guarda en inventario.
- `ModifyState("FreeCubicle", -1)`
- `ModifyState("Waiting", -1)` al finalizar.
- Entrega cubiculo al inventario del paciente.

---

### `GoToCubicle` - `Assets/Scripts/GoToCubicle.cs`

Descripcion:
- Accion de traslado a cubiculo y liberacion controlada del recurso.

Metodos:
- `PrePerform()`
- `PostPerform()`

Cambios de estado/recursos:
- Busca `Cubicle` en inventario.
- `ModifyState("TreatingPatient", 1)`
- Devuelve cubiculo a `cubicles`.
- Elimina cubiculo de inventario.
- `ModifyState("FreeCubicle", 1)`

---

### `GetTreated` - `Assets/Scripts/GetTreated.cs`

Descripcion:
- Accion del paciente para completar tratamiento.

Metodos:
- `PrePerform()`
- `PostPerform()`

Cambios de estado/recursos:
- Usa `Cubicle` del inventario.
- `ModifyState("Treated", 1)`
- `beliefs.ModifyState("isCured", 1)`
- Remueve cubiculo del inventario.

---

### `GoHome` - `Assets/Scripts/GoHome.cs`

Descripcion:
- Accion final del paciente: abandona hospital y destruye el GameObject.

Metodos:
- `PrePerform()`
- `PostPerform()`

Cambios de estado/recursos:
- `beliefs.RemoveState("atHospital")`
- `Destroy(gameObject, 1)`

---

### `Rest` - `Assets/Scripts/Rest.cs`

Descripcion:
- Accion de descanso para eliminar fatiga del agente.

Metodos:
- `PrePerform()` -> `true`
- `PostPerform()`

Cambios de estado/recursos:
- `beliefs.RemoveState("exhausted")`

---

### `GoToToilet` - `Assets/Scripts/GoToToilet.cs`

Descripcion:
- Reserva y libera toilet para satisfacer necesidad `busting`.

Metodos:
- `PrePerform()`
- `PostPerform()`

Cambios de estado/recursos:
- Reserva `toilets` y reduce `FreeToilet`.
- Devuelve `toilets` e incrementa `FreeToilet`.
- `beliefs.RemoveState("busting")`.

---

### `Research` - `Assets/Scripts/Research.cs`

Descripcion:
- Accion del doctor para investigar usando oficinas.

Metodos:
- `PrePerform()`
- `PostPerform()`

Cambios de estado/recursos:
- Reserva oficina de `offices` y reduce `FreeOffice`.
- Devuelve oficina e incrementa `FreeOffice`.

---

### `CleanUpPuddle` - `Assets/Scripts/CleanUpPuddle.cs`

Descripcion:
- Accion del janitor para limpiar charcos.

Metodos:
- `PrePerform()`
- `PostPerform()`

Cambios de estado/recursos:
- Toma recurso de `puddles` y reduce `FreePuddle`.
- Agrega charco al inventario temporal.
- Lo remueve del inventario y destruye el objeto al finalizar.

## 3) Clases del directorio `Assets/Scripts` fuera de la clasificacion agente/accion

Estas clases no heredan de `GAgent` ni `GAction`, pero soportan la simulacion:
- `Drive` (`Assets/Scripts/Drive.cs`)
- `Resource` (`Assets/Scripts/Resource.cs`)
- `ResourceData` (`Assets/Scripts/ResourceData.cs`)
- `Spawn` (`Assets/Scripts/Spawn.cs`)
- `UpdateWorld` (`Assets/Scripts/UpdateWorld.cs`)
- `Winterface` (`Assets/Scripts/Winterface.cs`)

