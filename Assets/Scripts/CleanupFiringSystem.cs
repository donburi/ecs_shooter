using Unity.Collections;
using Unity.Jobs;
using Unity.Entities;
using UnityEngine;

public class CleanupFiringBarrier : BarrierSystem { }
 

public class CleanupFiringSystem : JobComponentSystem {

    private struct CleanupFiringJob : IJobParallelFor
    {
        [ReadOnly] public EntityArray Entities;
        public EntityCommandBuffer.Concurrent EntityCommandBuffer;
        public float CurrentTime;
        public ComponentDataArray<Firing> Firings;

        public void Execute(int index)
        {
            if (CurrentTime - Firings[index].FiredAt < 0.5) return;
            EntityCommandBuffer.RemoveComponent<Firing>(Entities[index]);
        }
    }
    
    private struct Data
    {
        public readonly int Length;
        public EntityArray Entities;
        public ComponentDataArray<Firing> Firings;
    }

    [Inject] private Data _data;
    [Inject] private CleanupFiringBarrier _barrier;

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return new CleanupFiringJob
        {
            Entities = _data.Entities,
            CurrentTime = Time.time,
            EntityCommandBuffer = _barrier.CreateCommandBuffer(),
            Firings = _data.Firings
        }.Schedule(_data.Length, 64, inputDeps);
    }
}
