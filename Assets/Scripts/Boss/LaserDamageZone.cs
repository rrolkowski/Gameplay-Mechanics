using UnityEngine;

public class LaserDamageZone : MonoBehaviour
{
    [SerializeField] private int _damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<HealthController>()?.TakeDamage(_damage, false);
        }
    }

}
