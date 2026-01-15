using System.Collections.Generic;
using UnityEngine;

namespace Santa.Core
{
    /// <summary>
    /// ScriptableObject that defines the data for a specific level or area.
    /// It holds references to visual elements for different states.
    /// </summary>
    [CreateAssetMenu(fileName = "New LevelData", menuName = "Santa/Level Data")]
    // Forces recompile
    public class LevelData : ScriptableObject
    {
        [Header("Level Information")]
        [Tooltip("The name of the level or area.")]
        public string levelName;

        [Header("Transition Settings")]
        [Tooltip("The world position where the liberation wave originates and the camera focuses.")]
        public Vector3 transitionCenter;
        [Tooltip("The radius of the area to be liberated (how far the wave goes).")]
        public float transitionRadius = 50f;

        [Tooltip("Offset relative to transitionCenter for the camera position.")]
        public Vector3 cameraOffset = new Vector3(5.5f, 4f, 21f);

        [Tooltip("Time in seconds to wait for the camera to focus before starting dissolve.")]
        public float postTransitionDelay = 2.0f;

        [Header("Visual State GameObjects")]
        [Tooltip("Visuals to be active when the area is in its initial, 'gentrified' state (e.g., North American Christmas).")]
        public List<GameObject> gentrifiedVisuals;

        [Tooltip("Visuals to be active when the area is 'liberated' (e.g., Colombian Christmas).")]
        public List<GameObject> liberatedVisuals;
    }
}