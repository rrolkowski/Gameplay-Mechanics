using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsUI : MonoBehaviour
{
    public static SkillsUI Instance { get; private set; }

    [System.Serializable]
    public class SkillUIElement
    {
        public Image skillIcon;
        public Image cooldownOverlay;
        public TextMeshProUGUI levelText;
        public GameObject overlayBlock;
    }

    [SerializeField] private SkillUIElement[] _skillElements = new SkillUIElement[3];
    [SerializeField] private GameObject _skillPointNotice;
    [SerializeField] private TextMeshProUGUI _skillPointCountText;
    [SerializeField] private PlayerSkills _playerSkills;

    private float _skillNoticeTimer = 0f;
    private const float SkillNoticeDuration = 3f;

    private float[] _cooldownTimes = new float[4];
    private float[] _cooldownDurations = new float[4];

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < 3; i++)
        {
            _cooldownTimes[i] = -999f;
            _cooldownDurations[i] = 0f;
        }
    }

    public void StartCooldown(int index, float duration)
    {
        _cooldownTimes[index] = Time.time;
        _cooldownDurations[index] = duration;
    }

    void Update()
    {
        if (_playerSkills == null) return;

        for (int i = 0; i < _skillElements.Length; i++)
        {
            int level = _playerSkills.GetSkillLevel(i);
            var el = _skillElements[i];

            el.levelText.text = level.ToString();
            el.overlayBlock.SetActive(level == 0);

            float elapsed = Time.time - _cooldownTimes[i];
            float duration = _cooldownDurations[i];
            float fillAmount = Mathf.Clamp01(1 - (elapsed / duration));
            el.cooldownOverlay.fillAmount = fillAmount;
        }

        int points = _playerSkills.SkillPointsToSpend;
        _skillPointNotice.SetActive(_skillNoticeTimer > 0);
        _skillPointCountText.text = $"Skill Points: <color=#FFD700>{points}</color>";

        if (_skillNoticeTimer > 0f)
        {
            _skillNoticeTimer -= Time.deltaTime;
        }
    }
    public void NotifySkillPointGained()
    {
        _skillNoticeTimer = SkillNoticeDuration;
    }

}
