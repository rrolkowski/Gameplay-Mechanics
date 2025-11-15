using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Statistics", menuName = "Stats/Statistics")]
public class Statistics : ScriptableObject
{
    public UnityEvent LevelUp;
    public UnityEvent StatPointsChanged;
    public UnityEvent XPChanged;

    public int currentlvl = 1;
    [field: SerializeField] public int CurrentXP { get; private set; }
    public float baseMovementSpeed = 200f;
    public float baseMaxHealth = 100f;
    public float baseMaxStamina = 50f;
    public float baseMaxMana = 50f;
    public float baseArmor = 10f;
    public int xpWorth = 10;

    public int BaseStatisticPointsToDistribute = 0;

    public Dictionary<BaseStatType, int> statPoints = new Dictionary<BaseStatType, int>()
    {
        {BaseStatType.VITALITY,     0 },
        {BaseStatType.INTELLIGENCE, 0 },
        {BaseStatType.ENDURANCE,    0 },
        {BaseStatType.STRENGTH,     0 },
        {BaseStatType.DEXTERITY,    0 },
        {BaseStatType.PRECISION,    0 },
    };

    public int GetLevelXP(int level)
    {
        if (level == 1) return 100;
        return (int)(100 * Mathf.Pow(1.2f, level - 2));


    }
    public int GetCurrentLevelXPRequirement()
    {
        return GetLevelXP(currentlvl + 1);
    }
    public void AddXP(int xp)
    {
        CurrentXP += xp;
        Debug.Log($"Added XP: {xp}, Total XP: {CurrentXP}");

        XPChanged?.Invoke();

        if (CurrentXP >= GetLevelXP(currentlvl + 1))
        {
            currentlvl++;
            BaseStatisticPointsToDistribute += 5;
            CurrentXP -= GetLevelXP(currentlvl);
            LevelUp?.Invoke();

            Debug.Log($"Leveled up! New level: {currentlvl}");
        }
    }
}
public enum BaseStatType
{
    STRENGTH,
    DEXTERITY,
    VITALITY,
    ENDURANCE,
    PRECISION,
    INTELLIGENCE,
}
