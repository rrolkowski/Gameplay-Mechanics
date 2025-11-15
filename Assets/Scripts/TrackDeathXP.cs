using UnityEngine;

[RequireComponent (typeof(StatisticsProvider))]
public class TrackDeathXP : MonoBehaviour
{
    private Statistics _statistics;

    private void Start()
    {
        _statistics = GetComponent<StatisticsProvider>().GetStatistics();
        HealthController.OnDied += OnSomethingDied;
    }
    void OnSomethingDied(int xp)
    {
        _statistics.AddXP(xp);
        Debug.Log("Died! -  xp added");
    }
}
