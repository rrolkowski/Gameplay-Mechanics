using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _height = 20f;
    [SerializeField] private Vector3 _offset = Vector3.zero;
    [SerializeField] private bool _rotateWithTarget = false;

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector3 targetPosition = _target.position + _offset;
        targetPosition.y = _height;
        transform.position = targetPosition;

        if (_rotateWithTarget)
        {
            transform.rotation = Quaternion.Euler(90f, _target.eulerAngles.y, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
