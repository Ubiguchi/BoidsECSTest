﻿using System;
using BoidsECSTest.DefaultEcs.Components;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;

namespace BoidsECSTest.DefaultEcs.Systems
{
    [With(typeof(CellId), typeof(Behavior))]
    public sealed class BehaviorSystem : AEntitySystem<float>
    {
        private readonly World _world;
        private readonly EntitiesMap<CellId> _grid;

        public BehaviorSystem(World world, IParallelRunner runner) : base(world, runner)
        {
            _world = world;
            _grid = world.GetEntities().With<CellId>().With<Velocity>().With<DrawInfo>().AsMultiMap<CellId>();
        }

        protected override void Update(float state, ReadOnlySpan<Entity> entities)
        {
            Components<CellId> gridIds = _world.GetComponents<CellId>();
            Components<Behavior> behaviors = _world.GetComponents<Behavior>();
            Components<Velocity> velocities = _world.GetComponents<Velocity>();
            Components<DrawInfo> drawInfos = _world.GetComponents<DrawInfo>();

            foreach (ref readonly Entity entity in entities)
            {
                Vector2 center = Vector2.Zero;
                Vector2 direction = Vector2.Zero;
                int count = 0;

                if (_grid.TryGetEntities(gridIds[entity], out ReadOnlySpan<Entity> neighbors))
                {
                    foreach (ref readonly Entity neighbor in neighbors)
                    {
                        center += drawInfos[neighbor].Position;
                        direction += velocities[neighbor].Value;
                    }

                    count = neighbors.Length;
                }

                behaviors[entity] = new Behavior(center, direction, count);
            }
        }
    }
}
