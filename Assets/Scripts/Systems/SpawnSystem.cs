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
        private struct SpawnJob : IJobForEachWithEntity<CubeSpawn, LocalToWorld>
        {
            private EntityCommandBuffer.Concurrent m_buffer;
            private Random m_random;
            private readonly float m_deltaTime;


            public SpawnJob(EntityCommandBuffer.Concurrent pCommandBuffer, Random pRandom, float pDeltaTime)
            {
                m_buffer = pCommandBuffer;
                m_random = pRandom;
                m_deltaTime = pDeltaTime;
            }

            public void Execute(Entity pEntity, int pIndex, ref CubeSpawn pSpawner, [ReadOnly] ref LocalToWorld pLocalWorld)
            {
                pSpawner.timeCounter -= m_deltaTime;
                
                if (pSpawner.timeCounter >= 0f)
                    return;

                pSpawner.timeCounter += pSpawner.frequency;

                Entity _instance = m_buffer.Instantiate(pIndex, pSpawner.prefab);
                m_buffer.SetComponent(pIndex, _instance, new Translation() {
                    Value = pLocalWorld.Position + m_random.NextFloat3Direction()*m_random.NextFloat()*pSpawner.distance
                });

                m_buffer.AddComponent(pIndex, _instance, new RotationEulerXYZ() {
                Value = new float3(0, m_random.NextFloat(), 0)
                });

                m_buffer.AddComponent(pIndex, _instance, new Rotate() {
                    radiansPerSecond = math.radians(pSpawner.degreesPerSeconds)
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