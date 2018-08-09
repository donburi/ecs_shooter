using Unity.Entities;
using UnityEngine;

public class PlayerRotationSystem : ComponentSystem {

	private struct Filter
	{
		public Transform Transform;
		public RotationComponent RotationComponent;
	}

	protected override void OnUpdate()
	{
		var mousePosition = Input.mousePosition;
		var cameraRay = Camera.main.ScreenPointToRay(mousePosition);
		var layerMask = LayerMask.GetMask("Floor");

		if (Physics.Raycast(cameraRay, out var hit, 100, layerMask))
		{
			foreach (var entity in GetEntities<Filter>())
			{
				var forward = hit.point - entity.Transform.position;
				var rotation = Quaternion.LookRotation(forward);

				entity.RotationComponent.Value = new Quaternion(0, rotation.y, 0, rotation.w).normalized;
			}
		}
	}
}
