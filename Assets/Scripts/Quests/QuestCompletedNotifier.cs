using System.Collections;
using TMPro;
using UnityEngine;

public class QuestCompletedNotifier : MonoBehaviour
{
    [SerializeField] private GameObject _popupObject;
    [SerializeField] private TextMeshProUGUI _popupText;
    [SerializeField] private float _displayTime = 3f;

    private void OnEnable()
    {
        QuestManager.OnQuestCompleted += ShowPopup;
    }

    private void OnDisable()
    {
        QuestManager.OnQuestCompleted -= ShowPopup;
    }

    public void ShowPopup(int xpReward)
    {
        _popupText.text = $"<b>Quest Completed!</b>\n<color=#FFD700>+{xpReward} XP</color>";
        StartCoroutine(ShowTemporarily());
    }

    private IEnumerator ShowTemporarily()
    {
        _popupObject.SetActive(true);
        yield return new WaitForSeconds(_displayTime);
        _popupObject.SetActive(false);
    }
}
