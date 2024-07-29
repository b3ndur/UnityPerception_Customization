using System;
using System.Linq;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Samplers;
using UnityEngine.Perception.Randomization.Utilities;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Perception.Randomization.Randomizers
{
    /// <summary>
    /// Randomly places a single GameObject from a given list of prefabs
    /// </summary>
    [Serializable]
    [AddRandomizerMenu("Perception/HokBen Placement Randomizer")]
    [MovedFrom("UnityEngine.Perception.Randomization.Randomizers.SampleRandomizers")]
    public class HokBenPlacementRandomizer : Randomizer
    {
        [Tooltip("The Z offset applied to the position of the placed object.")]
        public float depth;

        [Tooltip("The lower and upper limits of the X coordinates for object placement.")]
        public FloatParameter minX;
        public FloatParameter maxX;

        [Tooltip("The lower and upper limits of the Y coordinates for object placement.")]
        public FloatParameter minY;
        public FloatParameter maxY;

        [Tooltip("The minimum and maximum values for the scale of the foreground object.")]
        public FloatParameter minScale;
        public FloatParameter maxScale;

        [Tooltip("The list of Prefabs to be placed by this Randomizer.")]
        public CategoricalParameter<GameObject> prefabs;

        GameObject m_Container;
        GameObjectOneWayCache m_GameObjectOneWayCache;
        GameObject currentForegroundObject;

        protected override void OnAwake()
        {
            m_Container = new GameObject("Foreground Objects");
            m_Container.transform.parent = scenario.transform;
            m_GameObjectOneWayCache = new GameObjectOneWayCache(
                m_Container.transform, prefabs.categories.Select(element => element.Item1).ToArray(), this);
        }

        protected override void OnIterationStart()
        {
            if (currentForegroundObject != null)
            {
                m_GameObjectOneWayCache.ResetObject(currentForegroundObject);
            }

            var prefab = prefabs.Sample();
            var position = new Vector3(
                Random.Range(minX.Sample(), maxX.Sample()),
                Random.Range(minY.Sample(), maxY.Sample()),
                depth
            );
            currentForegroundObject = m_GameObjectOneWayCache.GetOrInstantiate(prefab);
            currentForegroundObject.transform.position = position;

            float randomScale = Random.Range(minScale.Sample(), maxScale.Sample());
            currentForegroundObject.transform.localScale = new Vector3(randomScale, randomScale, currentForegroundObject.transform.localScale.z);
            Debug.Log($"Instantiated {currentForegroundObject.name} at position {position} with scale ({randomScale}, {randomScale})");
        }

        protected override void OnIterationEnd()
        {
            if (currentForegroundObject != null)
            {
                m_GameObjectOneWayCache.ResetObject(currentForegroundObject);
                currentForegroundObject = null;
            }
        }
    }
}
