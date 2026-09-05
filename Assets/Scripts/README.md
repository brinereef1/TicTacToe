# Game Scripts

This folder contains the main scripts for the Tic-Tac-Toe game. Scripts inside the `Test` folder are not included here.

## GameManager.cs

`GameManager` is the central messenger for the game.

- Keeps one shared `GameManager` in the scene.
- Receives a grid position when a player clicks a square.
- Sends an event containing the square's `x` and `y` coordinates.
- Other scripts can listen to this event and react to the click.

In simple terms: **it tells the rest of the game which square was clicked.**

## GridPosition.cs

`GridPosition` belongs to one square on the board.

- Stores the square's `x` and `y` coordinates.
- Detects when the player clicks the square.
- Sends those coordinates to `GameManager`.

In simple terms: **it knows where a square is and reports when that square is clicked.**

## GameVisualManager.cs

`GameVisualManager` is responsible for showing the visual result of a move.

- Has references for the cross and circle prefabs.
- Listens for click events from `GameManager`.
- Converts the clicked grid coordinates into a world position.
- Creates a circle at that position.
- Unsubscribes from the event when it is disabled.

In simple terms: **it puts a circle on the board where the player clicked.**

The cross prefab is already available, but the current code does not use it yet. The game currently creates a circle for every click.

## How the scripts work together

1. The player clicks a board square.
2. `GridPosition` sends that square's coordinates to `GameManager`.
3. `GameManager` broadcasts the click event.
4. `GameVisualManager` receives the event.
5. `GameVisualManager` converts the coordinates into a world position and creates a circle there.
