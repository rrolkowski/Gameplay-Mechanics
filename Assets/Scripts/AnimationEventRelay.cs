using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    [SerializeField] PlayerAttack _playerAttack;

    public void EndDamagePhase()
    {
        if (_playerAttack != null)
        {
            _playerAttack.EndDamagePhase();
        }
    }

    public void StartDamagePhase()
    {
        if (_playerAttack != null)
        {
            _playerAttack.StartDamagePhase();
        }
    }
}
