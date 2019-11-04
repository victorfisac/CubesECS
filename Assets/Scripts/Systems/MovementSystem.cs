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
        private struct MovementJob : IJobForEach<Translation, Movement, NonUniformScale>
        {
            public float deltaTime;
            public float heightFactor;
            public float maxHeight;
            public float waveIntensity;

            private const float MAX_LENGTH = 25f;

            public void Execute(ref Translation pPosition, ref Movement pMovement, ref NonUniformScale pScale)
            {
                pPosition.Value.z += pMovement.speed*deltaTime;
                
                if (pPosition.Value.z > MAX_LENGTH)
                    pPosition.Value.z = 1f;
                
                bool _canJump = pMovement.bouncing;
                pPosition.Value.y = (_canJump ? math.lerp(pPosition.Value.y, heightFactor*maxHeight, deltaTime*waveIntensity) : 0f);
            }
        }
        #endregion


        #region Job Methods
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new MovementJob {
                deltaTime = Time.deltaTime,
                heightFactor = 0f, // AudioWaveProvider.Instance.CurrentWave,
                maxHeight = 1f, // AudioWaveProvider.Instance.MaxHeight,
                waveIntensity = 20f // AudioWaveProvider.Instance.WaveIntensity
            };
            
            return job.Schedule(this, inputDeps);
        }
        #endregion
    }
}