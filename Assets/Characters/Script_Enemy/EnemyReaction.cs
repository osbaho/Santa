using UnityEngine;
public class EnemyReaction : MonoBehaviour
{
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null) _animator = GetComponentInChildren<Animator>();
    }
    // Se activa cuando el jugador entra en el Sphere Collider (Trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Disparamos el ataque en el Animator
            if (_animator != null)
            {
                _animator.SetTrigger("Attack");
                Debug.Log("¡Enemigo atacando!");
            }
        }
    }
}