using System.Collections;
using UnityEngine;

public class SkillBuff : MonoBehaviour, ISkillEffect
{
    [SerializeField] GameObject _vfxPrefab;

    [SerializeField] float _damageReductionMultiplier = 0.5f;
    [SerializeField] float _effectDuration = 5f;

    public void Activate(float damage, int level, Transform player)
    {
        var healthController = player.GetComponent<HealthController>();
        if(healthController != null) 
        {
           healthController.StartCoroutine(ApplyDamageReduction(healthController, _effectDuration, _damageReductionMultiplier));
        }

        if(_vfxPrefab != null)
        {
           GameObject effect =  Instantiate(_vfxPrefab,player.position + Vector3.up * 1.5f,Quaternion.identity,player);
            Destroy(effect, _effectDuration);
        }    

    }

    private IEnumerator ApplyDamageReduction(HealthController healthcontroller, float duration, float damageMultiplier)
    {
        healthcontroller.UpdateDamageMultiplier(_damageReductionMultiplier);

        yield return new WaitForSeconds(_effectDuration);

        healthcontroller.UpdateDamageMultiplier(1f);
        Destroy(this.gameObject);
    }

}
