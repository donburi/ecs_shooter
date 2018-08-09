using Unity.Entities;
using UnityEngine;

public class RotationSystem : ComponentSystem
{
   private struct Data
   {
      public readonly int Length;
      public ComponentArray<RotationComponent> RotationComponents;
      public ComponentArray<Rigidbody> Rigidbodies;
   }

   [Inject] private Data _data;

   protected override void OnUpdate()
   {
      for (int i = 0; i < _data.Length; i++)
      {
         var rotation = _data.RotationComponents[i].Value;
         _data.Rigidbodies[i].MoveRotation(rotation.normalized);
      }
   }
}
   