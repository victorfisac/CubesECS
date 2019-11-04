using UnityEngine;
using Unity.Entities;
using CubesECS.Components;


namespace CubesECS.Authoring
{
    public class CubeAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        #region Inspector Fields
        [Header("Movement")]
        [SerializeField]
        private float m_speed;

        [Header("Bouncing")]
        [SerializeField]
        private float m_maxHeight;
        [SerializeField]
        private float m_heightIntensity = 30f;
        #endregion


        #region Authoring Methods
        public void Convert(Entity pEntity, EntityManager pManager, GameObjectConversionSystem pConversionSystem)
        {
            pManager.AddComponentData(pEntity, new Movement() {
                speed = m_speed
            });

            Unity.Mathematics.Random _random = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(0, int.MaxValue));

            pManager.AddComponentData(pEntity, new Bouncing() {
                maxHeight = Unity.Mathematics.math.clamp(_random.NextFloat(-1f, m_maxHeight), 0f, m_maxHeight)/m_maxHeight,
                waveIntensity = m_heightIntensity,
            });
        }
        #endregion
    }
}