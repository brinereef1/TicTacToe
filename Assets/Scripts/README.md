# Game Scripts

This folder contains the main scripts for the networked Tic-Tac-Toe board. The scripts inside the `Test` folder are intentionally not described here.

## Big picture

The scripts have three different jobs:

- `GridPosition.cs` knows when one board square is clicked.
- `GameManager.cs` shares information about that click and the player's symbol.
- `GameVisualManager.cs` asks the server to create the correct symbol for everyone.

The scripts use Unity Netcode so that a move created by one player can appear for all connected players.

## GameManager.cs

`GameManager` is the central communication script. It does not draw anything on the board. Instead, it receives click information and tells other scripts about the click.

### Why it inherits from `NetworkBehaviour`

`NetworkBehaviour` is Unity Netcode's version of a component that can participate in a networked game. This allows `GameManager` to use network information such as the local player's client ID.

### Shared instance

`Instance` stores the one active `GameManager` so other scripts can find it easily with `GameManager.Instance`.

In `Awake`, the script checks whether another `GameManager` already exists. If there is a duplicate, the duplicate is destroyed. This helps prevent two managers from sending duplicate events.

### The click event

`onClickedOnGridPosition` is an event. An event is a notification that other scripts can listen to.

When the event is sent, it contains an `OnClickedOnPositionEventArgs` object with:

- `x`: the horizontal grid coordinate.
- `y`: the vertical grid coordinate.
- `playerType`: the symbol belonging to the player who clicked.

The event lets `GameManager` report a click without needing to know exactly how the board should display it.

### `PlayerType`

`PlayerType` is an enum, which is a small list of named choices:

- `None`: no symbol has been selected.
- `Circle`: the player uses a circle.
- `Cross`: the player uses a cross.

### Player assignment

When the network object is spawned, `OnNetworkSpawn` checks the local client ID:

- Client ID `0` is assigned `Cross`.
- Other client IDs are assigned `Circle`.

This is the current assignment rule. The script does not currently rotate turns or check whether a player is allowed to click.

### Custom methods

#### `ClickedOnGridPosition(int x, int y)`

This method is called by `GridPosition` after a square is clicked.

It creates the event data, adds the local player's symbol by calling `GetPlayerType`, and sends `onClickedOnGridPosition`. Every script listening to that event can then react.

#### `GetPlayerType()`

This method returns the symbol assigned to the local player. It returns either `PlayerType.Cross` or `PlayerType.Circle` after network setup has assigned a value.

## GridPosition.cs

`GridPosition` is attached to an individual board square. Each square has its own coordinates, so the game can tell which position was clicked.

### Fields

- `x`: the square's horizontal grid coordinate.
- `y`: the square's vertical grid coordinate.

These values are set in the Unity Inspector for each square. For example, one square might have coordinates `(0, 0)` and another might have `(2, 1)`.

### What happens when the square is clicked

Unity calls `OnMouseDown` when the player clicks the square's collider. The method:

1. Prints the square's coordinates to the Unity Console.
2. Calls `GameManager.Instance.ClickedOnGridPosition(x, y)`.

`GridPosition` does not choose a cross or circle and does not create a visual object. It only reports the location of the click.

## GameVisualManager.cs

`GameVisualManager` listens for click events and creates the visual symbol on the board. It also inherits from `NetworkBehaviour` because the created symbol must be visible to every connected player.

### Fields and constant

- `GRID_SIZE`: the distance between neighboring grid positions. It is currently `3.1` Unity units.
- `crossPrefab`: the prefab used for a cross move.
- `circlePrefab`: the prefab used for a circle move.

The cross and circle prefabs should be assigned in the Unity Inspector. They should also contain a `NetworkObject`, because the server calls `Spawn` on the created object.

### Listening for clicks

In `Start`, `GameVisualManager` subscribes to `GameManager`'s `onClickedOnGridPosition` event. This means its `GameManager_OnClickedGridPosition` method runs whenever `GameManager` reports a click.

### Custom methods

#### `GameManager_OnClickedGridPosition(...)`

This is the event handler. It receives the clicked coordinates and the player's symbol from the event data.

It then calls `SpawnObjectRpc`, passing along:

- The `x` coordinate.
- The `y` coordinate.
- The player's `Cross` or `Circle` type.

#### `SpawnObjectRpc(...)`

This method has `[Rpc(SendTo.Server)]`, which means the method request is sent to the server. The server is responsible for creating the networked object.

The method:

1. Chooses `circlePrefab` for a circle or `crossPrefab` for a cross.
2. Converts the grid coordinates into a world position.
3. Instantiates the selected prefab at that position.
4. Gets its `NetworkObject` component.
5. Calls `Spawn(true)` so the object is synchronized with the connected players.

The `default` case currently uses the circle prefab. This means an unknown or `None` player type will be treated like a circle.

#### `GetGridWorldPosition(int x, int y)`

This method changes board coordinates into Unity world coordinates. The calculation is:

- World X = `-GRID_SIZE + x * GRID_SIZE`
- World Y = `-GRID_SIZE + y * GRID_SIZE`

With the current grid size, coordinate `(0, 0)` becomes `(-3.1, -3.1)`. Increasing `x` moves right, and increasing `y` moves up.

## Complete click flow

1. The player clicks a board square.
2. Unity calls that square's `GridPosition.OnMouseDown` method.
3. `GridPosition` sends the square's `x` and `y` values to `GameManager`.
4. `GameManager` finds the local player's symbol.
5. `GameManager` sends the `onClickedOnGridPosition` event.
6. `GameVisualManager` receives the event.
7. `GameVisualManager` requests `SpawnObjectRpc` on the server.
8. The server selects the correct prefab and calculates its world position.
9. The server spawns the network object so all players can see it.

## Current limitations

The scripts currently display a symbol for every click, but they do not yet:

- Prevent a player from clicking an occupied square.
- Enforce alternating turns.
- Check whether a click comes from the correct player.
- Detect a winner or a draw.
- Unsubscribe `GameVisualManager` from the event when it is disabled.

Those rules would need to be added separately if they are required for the finished game.
