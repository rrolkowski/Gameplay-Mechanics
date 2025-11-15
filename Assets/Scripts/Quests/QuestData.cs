using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest System/Quest")]
public class QuestData : ScriptableObject
{
    [Header("Quest Info")]
    public string questName;
    public string description;
    public int killTargetCount;

    [Header("Quest Rewards")]
    public int rewardXP;

    [Header("Next Quest")]
    public QuestData nextQuest;
}
