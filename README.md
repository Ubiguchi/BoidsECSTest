# Boids ECS Test
This is a test comparing the performance of a Boids simulation using the [DefaultECS](https://github.com/Doraku/DefaultEcs) [Entity Component System](https://en.wikipedia.org/wiki/Entity_component_system) framework and the ECS embedded in the [Nez](https://github.com/prime31/Nez) Framework.

It is intended as a preliminary investigation to decide if there's enough performance benefit for adding support for DefaultECS into Nez. However, it's currently not a fair test, since DefaultECS uses multiple threads and also a bespoke spatial grid for the Boids simulation. 

<a name='Performance'></a>
# Performance
While the testing is extremely unfair and very much finger-in-the-air, the initial results are below. I should be clear that I fully expect the Nez numbers will see a significant improvement once I've verified the implementation, added multiple threads, and balanced the spatial grid. However, given DefaultECS has a data-driven implementation, my hypothesis is it'll do significnatly better when simulating large numbers of entities.

```
OS=Windows 10.x
Intel Core i7-6700HQ CPU 2.6GHz, 1 CPU, 4 physical and 8 logical cores
  [Host]     : .NET Core 3.1
```
|                      Method | 30,000 boids |   3,000 boids |    300 boids | 
|---------------------------- |-------------:|--------------:|-------------:|
|                  DefaultEcs |    45-70 fps |  200-380 fps  |  400-650 fps |
|                         Nez |          N/A |      3-5 fps  |  110-120 fps |