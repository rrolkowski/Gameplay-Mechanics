using UnityEngine;

[RequireComponent(typeof(StatisticsProvider))]
public class LevelUPEffect : MonoBehaviour
{
    private Statistics _statistics;

    [SerializeField] private ParticleSystem levelUpEffect;

    private void Start()
    {
        _statistics = GetComponent<StatisticsProvider>().GetStatistics();
        _statistics.LevelUp.AddListener(OnLevelUp);
    }

    private void OnDestroy()
    {
        _statistics.LevelUp.RemoveListener(OnLevelUp);
    }

    private void OnLevelUp()
    {
        Debug.Log("Level Up");

        if (levelUpEffect != null)
        {
            levelUpEffect.Play();
        }
    }
}
