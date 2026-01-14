using Santa.Core;
using Santa.Core.Config;
using UnityEngine;

namespace Santa.Characters
{
    // Good practice: Ensure dependencies are present
    [RequireComponent(typeof(Collider))]
    public class EnemyReaction : MonoBehaviour
    {
        [Tooltip("The Animator component that controls enemy animations.")]
        [SerializeField] private Animator _animator;

        // Use constants to avoid magic strings
        private const string PLAYER_TAG = "Player";
        private const string ATTACK_TRIGGER = "Attack";
        private static readonly int AttackTriggerHash = Animator.StringToHash(ATTACK_TRIGGER);

        private void Awake()
        {
            // Fallback: look in children if not assigned
            if (_animator == null)
            {
                _animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
            }

            // Validation: Log error if missing
            if (_animator == null)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                GameLog.LogError($"Animator component not found on {gameObject.name} or its children.", this);
#endif
                enabled = false;
            }
        }

        // Triggered when the player enters the Sphere Collider (Trigger)
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PLAYER_TAG)) // Could also use GameConstants.Tags.Player
            {
                // Trigger the attack in the Animator
                if (_animator != null)
                {
                    _animator.SetTrigger(AttackTriggerHash);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                    GameLog.Log("Enemy attacking!");
#endif
                }
            }
        }
    }
}