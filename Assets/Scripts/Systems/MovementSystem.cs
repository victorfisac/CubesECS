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
        #region Private Fields
        private bool m_canJump = false;
        #endregion

        
        #region Structs
        [BurstCompile]
        private struct MovementJob : IJobForEach<Translation, Movement, Bouncing, NonUniformScale>
        {
            public float deltaTime;
            public float heightFactor;

            private const float MAX_LENGTH = 25f;

            public void Execute(ref Translation pPosition, ref Movement pMovement, ref Bouncing pBouncing, ref NonUniformScale pScale)
            {
                pPosition.Value.z += pMovement.speed*deltaTime;
                
                if (pPosition.Value.z > MAX_LENGTH)
                    pPosition.Value.z = 1f;
                
                bool _canJump = (pScale.Value.x > 0.1f);
                pPosition.Value.y = (_canJump ? math.lerp(pPosition.Value.y, heightFactor*pBouncing.maxHeight, deltaTime*pBouncing.waveIntensity) : 0f);
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