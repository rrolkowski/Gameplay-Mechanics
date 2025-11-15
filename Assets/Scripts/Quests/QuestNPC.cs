using UnityEngine;

public class QuestNPC : MonoBehaviour
{
    [SerializeField] QuestData _questToGive;
    [SerializeField] private GameObject _questMarker;

    private bool _hasGivenQuest = false;

    private void Start()
    {
        UpdateQuestMarker();
    }

    private void OnMouseDown()
    {
        if (QuestManager.Instance != null && !_hasGivenQuest && _questToGive != null)
        {
            QuestManager.Instance.StartQuest(_questToGive);
            _hasGivenQuest = true;
            UpdateQuestMarker();
        }
    }

    public void SetNewQuest(QuestData newQuest)
    {
        _questToGive = newQuest;
        _hasGivenQuest = false;
        UpdateQuestMarker();
    }

    private void UpdateQuestMarker()
    {
        if (_questMarker != null)
            _questMarker.SetActive(_questToGive != null && !_hasGivenQuest);
    }

}
