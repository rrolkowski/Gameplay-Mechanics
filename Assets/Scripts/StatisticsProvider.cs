using UnityEngine;

public class StatisticsProvider : MonoBehaviour
{
    [SerializeField] private bool useAsSingleton; 
    public static StatisticsProvider Instance;

    [SerializeField] private Statistics _statistics;

    private void Awake()
    {
        if (useAsSingleton)
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            
        }
        _statistics = Instantiate(_statistics);
    }
    
    public Statistics GetStatistics() => _statistics;
}
