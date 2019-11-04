using CubesECS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace CubesECS.Systems
{
    public class SpawnSystem : JobComponentSystem
    {
        #region Private Fields
        private EndSimulationEntityCommandBufferSystem m_commandBufferSystem;
        #endregion


        #region Structs
        [BurstCompile]
        private struct SpawnJob : IJobForEachWithEntity<SpawnData, LocalToWorld>
        {
            private EntityCommandBuffer.Concurrent m_buffer;
            private Random m_random;
            private readonly float m_deltaTime;

            private const float MIN_HEIGHT = 0.5f;
            private const float MIN_SCALE = 0.005f;
            private const float MAX_SCALE = 0.25f;


            public SpawnJob(EntityCommandBuffer.Concurrent pCommandBuffer, Random pRandom, float pDeltaTime)
            {
                m_buffer = pCommandBuffer;
                m_random = pRandom;
                m_deltaTime = pDeltaTime;
            }

            public void Execute(Entity pEntity, int pIndex, ref SpawnData pSpawner, [ReadOnly] ref LocalToWorld pLocalWorld)
            {
                pSpawner.timeCounter -= m_deltaTime;
                
                if (pSpawner.timeCounter > 0f)
                    return;

                pSpawner.timeCounter = pSpawner.frequency;

                Entity _instance = m_buffer.Instantiate(pIndex, pSpawner.prefab);
                m_buffer.SetComponent(pIndex, _instance, new Translation() {
                    Value = pLocalWorld.Position + new float3(m_random.NextFloat()*pSpawner.distance, MIN_HEIGHT, 0f)
                });

                m_buffer.AddComponent(pIndex, _instance, new NonUniformScale() {
                    Value = new float3(MIN_SCALE + m_random.NextFloat()*MAX_SCALE,
                        MIN_SCALE + m_random.NextFloat()*MAX_SCALE,
                        MIN_SCALE + m_random.NextFloat()*MAX_SCALE)
                });
            }
        }
        #endregion


        #region Job Methods
        protected override void OnCreate()
        {
            m_commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle pInputDeps)
        {
            SpawnJob _job = new SpawnJob(
                m_commandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                new Random((uint)UnityEngine.Random.Range(0, int.MaxValue)),
                UnityEngine.Time.deltaTime
            );

            JobHandle _jobHandle = _job.Schedule(this, pInputDeps);
            m_commandBufferSystem.AddJobHandleForProducer(_jobHandle);

            return _jobHandle;
        }
        #endregion 
    }
}