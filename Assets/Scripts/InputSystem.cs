using Unity.Entities;
using UnityEngine;

// this class's sole purpose is to transform player input into component data

public class InputSystem : ComponentSystem
{

    private struct Data
    {
        public readonly int Length;
        public ComponentArray<InputComponent> InputComponents;
    }

    [Inject] private Data _data;
    
    protected override void OnUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        for (int i = 0; i < _data.Length; i++)
        {
            _data.InputComponents[i].Horizontal = horizontal;
            _data.InputComponents[i].Vertical = vertical;
        }
    }
    
}
