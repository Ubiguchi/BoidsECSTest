# Boids ECS Test
This is a test comparing the performance of a Boids simulation using the [DefaultECS](https://github.com/Doraku/DefaultEcs) [Entity Component System](https://en.wikipedia.org/wiki/Entity_component_system) framework and the ECS embedded in the [Nez](https://github.com/prime31/Nez) Framework.

It is intended as a preliminary investigation to decide if there's enough performance benefit for adding support for DefaultECS into Nez. However, it should be noted this is a rough test rather than a scientific and functionally fair test. In particular the DefaultECS implementation is minimal, while the Nez implementation uses a number of generic framework features useful for games, such as Scenes, and a generic SpatialHash. DefaultECS is also designed to easily take advantage of multiple threads, while Nez has a number of single-threaded features that can't easily take advantage of parallel processing.

The multi-threaded Nez implementation is broadly naive, however, given DefaultECS has a data-driven implementation, my hypothesis is it'll do significnatly better when simulating large numbers of entities.

<a name='Performance'></a>
# Performance Results
```
OS=Windows 10.x
Intel Core i7-6700HQ CPU 2.6GHz, 1 CPU, 4 physical and 8 logical cores
  [Host]     : .NET Core 3.1
```
|                      Method | 30,000 boids |   3,000 boids |    300 boids | 
|---------------------------- |-------------:|--------------:|-------------:|
| (Multi-threaded) DefaultEcs |    45-70 fps |  200-380 fps  |  400-650 fps |
|        (Multi-threaded) Nez |          N/A |    30-40 fps  |  140-170 fps |
|(Single-threaded) DefaultEcs |    15-20 fps |  130-170 fps  |  420-620 fps |
|       (Single-threaded) Nez |          N/A |    25-35 fps  |  140-155 fps |