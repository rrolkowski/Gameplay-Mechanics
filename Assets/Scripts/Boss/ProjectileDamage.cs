using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<HealthController>()?.TakeDamage(_damage, false);
            Destroy(gameObject);
        }
    }
}
