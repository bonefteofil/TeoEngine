# C# console game engine - 2019

> WARNING This library is currently under development. Detailed documentation will be provided soon to explain its functions. Feel free to explore.

I made a library that displays polygons, ellipses, text, and buttons with properties such as position, rotation, color, parent-child relationships, and mouse events. It also provides functions for a timer, mouse control, FPS adjustment, and console dimensions.

## Examples
[Tic Tac Toe Game](https://github.com/bonefteofil/TicTacToe-Console)

## Getting started
### 1. Editor
Visual Studio

### 2. Install with
```
dotnet add package TeoEngine --version 1.0.0-beta
```
or download from [Nuget](https://www.nuget.org/packages/TeoEngine/1.0.0-beta) or [Github](https://github.com/bonefteofil/TeoEngine/releases/tag/v1.0.0-beta)

### 3. Requirements
Running as administrator for console size adjustments. On your project, add new item: `Application Manifest File` and change the `asInvoker` to `requireAdministrator`.

### 4. Code
In the main function, call `Game.Initialize()` with two parameters: a start function (called once) and an update function (called every frame).

```
static void Main()
{
    Game.Initialize(Start, Update);
}
```