using UnityEngine;
using Unity.Transforms;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using CubesECS.Components;
using CubesECS.Behaviours;
using Unity.Mathematics;

namespace CubesECS.Systems
{
    public class MovementSystem : JobComponentSystem
    {
        #region Structs
        [BurstCompile]
        private struct MovementJob : IJobForEach<Translation, Movement, Bouncing>
        {
            public float deltaTime;
            public float heightFactor;

            private const float MAX_LENGTH = 25f;

            public void Execute(ref Translation pPosition, ref Movement pMovement, ref Bouncing pBouncing)
            {
                pPosition.Value.z += pMovement.speed*deltaTime;
                
                if (pPosition.Value.z > MAX_LENGTH)
                    pPosition.Value.z = 1f;
                
                pPosition.Value.y = math.lerp(pPosition.Value.y, heightFactor, deltaTime*pBouncing.waveIntensity);
            }
        }
        #endregion


        #region Job Methods
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new MovementJob {
                deltaTime = Time.deltaTime,
                heightFactor = AudioWaveProvider.Instance.CurrentWave
            };
            
            return job.Schedule(this, inputDeps);
        }
        #endregion
    }
}