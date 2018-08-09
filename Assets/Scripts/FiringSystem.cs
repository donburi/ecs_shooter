using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEditor;
using static Unity.Entities.ComponentType;

public class FiringSystem : JobComponentSystem
{
    private ComponentGroup _componentGroup;
    [Inject] private FiringBarrier _barrier;

    protected override void OnCreateManager(int capacity)
    {
        _componentGroup = GetComponentGroup(
            Create<Firing>(),
            Create<Position>(),
            Create<Rotation>());
            
        _componentGroup.SetFilterChanged(Create<Firing>()); // only operate when this component is added
            
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return new FiringJob
        {
            EntityCommandBuffer = _barrier.CreateCommandBuffer(),
            Positions = _componentGroup.GetComponentDataArray<Position>(),
            Rotations = _componentGroup.GetComponentDataArray<Rotation>()
        }.Schedule(_componentGroup.CalculateLength(), 64, inputDeps);
    }

    private struct FiringJob : IJobParallelFor
    {
        public EntityCommandBuffer.Concurrent EntityCommandBuffer;
        public ComponentDataArray<Position> Positions;
        public ComponentDataArray<Rotation> Rotations;
        
        public void Execute(int index)
        {
            EntityCommandBuffer.CreateEntity();
            EntityCommandBuffer.AddSharedComponent(Bootstrap.BulletRenderer);
            EntityCommandBuffer.AddComponent(new TransformMatrix());
            EntityCommandBuffer.AddSharedComponent(new MoveForward());
            EntityCommandBuffer.AddComponent(new MoveSpeed {speed = 6});
            EntityCommandBuffer.AddComponent(Positions[index]);
            EntityCommandBuffer.AddComponent(Rotations[index]);
        }

     
    }
    
    private class FiringBarrier : BarrierSystem
    {
    }

}