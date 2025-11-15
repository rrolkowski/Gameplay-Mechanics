using UnityEngine;

public class DamageUI : MonoBehaviour
{
    [SerializeField] float _destroyTime = 3f;
    [SerializeField] Vector3 _offset = new Vector3 (0, 2, 0);
    [SerializeField] Vector3 _randomIntensity = new Vector3(0.5f, 0, 0);

    void Start()
    {
        Destroy(gameObject, _destroyTime);
        transform.localPosition += _offset;
        transform.localPosition += new Vector3(Random.Range(-_randomIntensity.x, _randomIntensity.x), 0,
            Random.Range(-_randomIntensity.z, _randomIntensity.z));
    }
}
