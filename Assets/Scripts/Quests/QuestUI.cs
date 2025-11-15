using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{   
    [SerializeField] private TextMeshProUGUI _questText;
    [SerializeField] private GameObject _questUI;

    private void OnEnable()
    {
        QuestManager.OnQuestUpdated += UpdateQuestUI;
    }

    private void OnDisable()
    {
        QuestManager.OnQuestUpdated -= UpdateQuestUI;
    }

    private void UpdateQuestUI(QuestData quest, int current, int required)
    {
        if (quest == null)
        {
            _questText.text = "";
            _questUI.SetActive(false);
        }
        else
        {
            _questUI.SetActive(true);
            string progress = $"<color=#FFD700>{current}/{required}</color>";
            _questText.text = $"{quest.questName}\n{quest.description}\n Killed: {progress}";
        }
    }
}

