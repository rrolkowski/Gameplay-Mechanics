using System.Collections;
using UnityEngine;

public class AttackLaser : MonoBehaviour, IBossAttack
{
    [SerializeField] private GameObject _laserRotatorPrefab;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private float _rotateSpeed = 90f;

    public void Execute(Vector3 targetPosition)
    {
        StartCoroutine(SpinLaserArm());
    }

    private IEnumerator SpinLaserArm()
    {
        GameObject rotator = Instantiate(_laserRotatorPrefab, transform.position + Vector3.down * 1.5f, Quaternion.identity);

        float timer = 0f;
        while (timer < _duration)
        {
            rotator.transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(rotator);
    }
}

