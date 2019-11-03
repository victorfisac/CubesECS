using System.Collections.Generic;
using CubesECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


namespace CubesECS.Authoring
{
    public class SpawnerAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
    {
        #region Inspector Fields
        [SerializeField]
        private GameObject m_prefab;
        [SerializeField]
        private float m_frequency;
        [SerializeField]
        private float m_maxDistance;
        [SerializeField]
        private float m_degreesPerSecond;
        #endregion


        #region Authoring Methods
        public void DeclareReferencedPrefabs(List<GameObject> pReferencedPrefabs)
        {
            pReferencedPrefabs.Add(m_prefab);
        }

        public void Convert(Entity pEntity, EntityManager pManager, GameObjectConversionSystem pConversionSystem)
        {
            pManager.AddComponentData(pEntity, new CubeSpawn() {
                prefab = pConversionSystem.GetPrimaryEntity(m_prefab),
                distance = m_maxDistance,
                frequency = m_frequency,
                timeCounter = 0f,
                degreesPerSeconds = m_degreesPerSecond
            });
        }
        #endregion
    }
}