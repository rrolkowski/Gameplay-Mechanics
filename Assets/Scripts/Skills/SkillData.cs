using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skill System/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public int maxLevel = 5;
    public GameObject skillEffectPrefab;

    [Header("Scaling")]
    public float[] manaCosts = new float[5];
    public float[] damages = new float[5];
    public float[] cooldown = new float[5];

}
