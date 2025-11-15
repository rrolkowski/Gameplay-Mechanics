using System.Collections;
using UnityEngine;

public class BurnEffect : MonoBehaviour
{
    private Coroutine _burnRoutine;

    public GameObject burnVFXPrefab;
    private GameObject _burnVFXInstance;
    public void StartBurn(int level)
    {
        if (_burnRoutine != null)
            StopCoroutine(_burnRoutine);

        if (burnVFXPrefab != null && _burnVFXInstance == null)
        {
            _burnVFXInstance = Instantiate(burnVFXPrefab, transform);
            _burnVFXInstance.transform.localPosition = Vector3.zero;
        }

        _burnRoutine = StartCoroutine(BurnDamage(level));
    }

    private IEnumerator BurnDamage(int level)
    {
        yield return new WaitForSeconds(1f);

        int ticks = 5;
        float tickInterval = 1f;
        float tickDamage = 2f + level;

        for (int i = 0; i < ticks; i++)
        {
            var health = GetComponent<HealthController>();
            if (health != null)
                health.TakeDamage(tickDamage, false);

            yield return new WaitForSeconds(tickInterval);
        }

        if (_burnVFXInstance != null)
            Destroy(_burnVFXInstance);

        _burnRoutine = null;
    }
}
