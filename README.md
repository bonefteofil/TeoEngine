
# C# console game engine - 2019

> [!WARNING]
> Project in development

I made a library that displays polygons, ellipses, text, and buttons with properties such as position, rotation, color, parent-child relationships, and mouse events. It also provides functions for a timer, mouse control, FPS adjustment, and console dimensions.

### Examples
<center float="left">
    <img src="https://bonefteofil.ro/projects/2019%20-%202D%20Game%20Engine%20for%20Console%20in%20C-Sharp/1.jpg" height="300" alt="image 1">
    <img src="https://bonefteofil.ro/projects/2019%20-%202D%20Game%20Engine%20for%20Console%20in%20C-Sharp/3.jpg" height="300" alt="image 2">
</center>

## Getting started
**Editor:** Visual studio

**Dependencies:** To use this library, you need to install the `LibKatana` library via NuGet Package Manager.

**Code:**
In the main function, call `Game.Initialize()` with two parameters: a start function (called once) and an update function (called every frame).


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