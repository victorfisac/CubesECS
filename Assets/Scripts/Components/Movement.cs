using Unity.Entities;


namespace CubesECS.Components
{
    public struct Movement : IComponentData
    {
        public float speed;
        public bool bouncing;
    }
}