using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkills : MonoBehaviour
{
    [System.Serializable]
    public class SkillSlot
    {
        public SkillData skill;
        public int level = 0;
    }

    [SerializeField] private SkillSlot[] skills = new SkillSlot[4];
    [SerializeField] private PMana _mana;

    private float[] _lastUsedTime = new float[4];

    private PlayerControls _playerControls;
    private Statistics _stats;
    public int SkillPointsToSpend { get; private set; } = 0;

    private void Awake()
    {
        _playerControls = new PlayerControls();

        for (int i = 0; i < _lastUsedTime.Length; i++)
            _lastUsedTime[i] = -999f;
    }

    void Start()
    {
        _stats = GetComponent<StatisticsProvider>().GetStatistics();
        _stats.LevelUp.AddListener(() => { SkillPointsToSpend++; SkillsUI.Instance?.NotifySkillPointGained();});

    }

    private void OnEnable()
    {
        _playerControls.Enable();

        _playerControls.Player.SkillUse1.performed += ctx => TryUseSkill(0);
        _playerControls.Player.SkillUse2.performed += ctx => TryUseSkill(1);
        _playerControls.Player.SkillUse3.performed += ctx => TryUseSkill(2);
        _playerControls.Player.SkillUse4.performed += ctx => TryUseSkill(3);

        _playerControls.Player.SkillUpgrade1.performed += ctx => UpgradeSkill(0);
        _playerControls.Player.SkillUpgrade2.performed += ctx => UpgradeSkill(1);
        _playerControls.Player.SkillUpgrade3.performed += ctx => UpgradeSkill(2);
        _playerControls.Player.SkillUpgrade4.performed += ctx => UpgradeSkill(3);
    }
    private void OnDisable()
    {
        _playerControls.Disable();
    }
    private void TryUseSkill(int index)
    {
        if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
            return;

        UseSkill(index);
    }

    public void UseSkill(int index)
    {
        if (index < 0 || index >= skills.Length) return;
        var slot = skills[index];
        if (slot.level <= 0) return;

        var skill = slot.skill;
        float manaCost = skill.manaCosts[slot.level - 1];
        float damage = skill.damages[slot.level - 1];
        float cooldown = skill.cooldown[slot.level - 1];

        if (Time.time - _lastUsedTime[index] < cooldown)
        {
            Debug.Log("coldown");
            return;
        }

        if (_mana.TryConsumeMana(manaCost))
        {
            _lastUsedTime[index] = Time.time;

            if (skill.skillEffectPrefab != null)
            {

                GameObject go = Instantiate(skill.skillEffectPrefab, transform.position + transform.forward * 1f, transform.rotation);
                var effect = go.GetComponent<ISkillEffect>();
                effect?.Activate(damage, slot.level, transform);
            }
            SkillsUI.Instance?.StartCooldown(index, cooldown);
        }
    }

    public void UpgradeSkill(int index)
    {
        if (SkillPointsToSpend <= 0) return;
        var slot = skills[index];
        if (slot.level < slot.skill.maxLevel)
        {
            slot.level++;
            SkillPointsToSpend--;
        }
    }

    public int GetSkillLevel(int index)
    {
        if (index < 0 || index >= skills.Length) return 0;
        return skills[index].level;
    }

}
