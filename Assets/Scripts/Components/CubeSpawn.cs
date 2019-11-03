using Unity.Entities;


namespace CubesECS.Components
{
    public struct CubeSpawn : IComponentData
    {
        public Entity prefab;
        public float distance;
        public float frequency;
        public float timeCounter;
        public float degreesPerSeconds;
    }
}