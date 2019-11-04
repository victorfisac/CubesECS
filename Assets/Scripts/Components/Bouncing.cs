using Unity.Entities;


namespace CubesECS.Components
{
    public struct Bouncing : IComponentData
    {
        public float maxHeight;
        public float waveIntensity;
    }
}