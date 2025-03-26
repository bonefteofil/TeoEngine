
# C# console game engine - 2019

> [!WARNING]
> Project in development

I made a library that displays polygons, ellipses, text, and buttons with properties such as position, rotation, color, parent-child relationships, and mouse events. It also provides functions for a timer, mouse control, FPS adjustment, and console dimensions.

### Examples
<div float="left">
    <img src="https://bonefteofil.ro/projects/2019%20-%202D%20Game%20Engine%20for%20Console%20in%20C-Sharp/1.jpg" height="300" alt="image 1">
    <img src="https://bonefteofil.ro/projects/2019%20-%202D%20Game%20Engine%20for%20Console%20in%20C-Sharp/3.jpg" height="300" alt="image 2">
</div>

## Getting started
**1. Editor:** Visual studio

**2. Dependencies:** To use this library, you need to install the `LibKatana` library via NuGet Package Manager from visual studio.

**3. Requirements:** Running as administrator for console size adjustments. On your project, add new item: `Application Manifest File` and change the `asInvoker` to `requireAdministrator`.

**4. Code:** In the main function, call `Game.Initialize()` with two parameters: a start function (called once) and an update function (called every frame).


```
static void Main()
{
    Game.Initialize(Start, Update);
}
```

<sub>
This library is currently under development. 
Detailed documentation will be provided soon to explain its functions.
Feel free to explore.
</sub>