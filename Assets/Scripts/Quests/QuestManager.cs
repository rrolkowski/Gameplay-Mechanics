using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public static event System.Action<QuestData, int, int> OnQuestUpdated;
    public static event System.Action<int> OnQuestCompleted;

    [SerializeField] QuestData currentQuest;
    [SerializeField] PlayerInfoUI _playerInfoUI;
    [SerializeField] QuestNPC _questNPC;

    private Statistics _statistics;

    private int _currentKillCount;

    private bool questActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        _statistics = GetComponent<StatisticsProvider>().GetStatistics();
    }

    public void StartQuest(QuestData quest)
    {
        currentQuest = quest;
        _currentKillCount = 0;
        questActive = true;
        Debug.Log($"Quest started: {quest.questName}");
        OnQuestUpdated?.Invoke(currentQuest, _currentKillCount, currentQuest.killTargetCount);
    }

    public void RegisterKill()
    {
        if (!questActive || currentQuest == null) return;

        _currentKillCount++;
        Debug.Log($"Enemy killed: {_currentKillCount}/{currentQuest.killTargetCount}");

        OnQuestUpdated?.Invoke(currentQuest, _currentKillCount, currentQuest.killTargetCount);

        if (_currentKillCount >= currentQuest.killTargetCount)
        {
            CompleteQuest();
        }
    }

    private void CompleteQuest()
    {
        questActive = false;

        if (_statistics != null && currentQuest != null)
        {
            _statistics.AddXP(currentQuest.rewardXP);
            OnQuestCompleted?.Invoke(currentQuest.rewardXP);
        }

        if (_playerInfoUI != null)
            _playerInfoUI.ShowPlayerInfo();

        QuestData nextQuest = currentQuest.nextQuest;
        currentQuest = null;
        OnQuestUpdated?.Invoke(null, 0, 0);

        if (nextQuest != null)
        {
            _questNPC?.SetNewQuest(nextQuest);
        }
            
    }
}
