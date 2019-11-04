using Unity.Entities;


namespace CubesECS.Components
{
    public struct Bouncing : IComponentData
    {
        public int canJump;
        public float maxHeight;
        public float waveIntensity;
    }
}