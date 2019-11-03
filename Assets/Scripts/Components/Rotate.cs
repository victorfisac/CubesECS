using Unity.Entities;


namespace CubesECS.Components
{
    public struct Rotate : IComponentData
    {
        public float radiansPerSecond;
    }
}