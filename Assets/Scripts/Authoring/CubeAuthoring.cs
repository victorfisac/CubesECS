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

            pManager.AddComponentData(pEntity, new Bouncing() {
                canJump = Random.Range(0, 2),
                maxHeight = m_maxHeight,
                waveIntensity = m_heightIntensity
            });
        }
        #endregion
    }
}