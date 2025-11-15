using UnityEngine;

public interface ISkillEffect
{
    void Activate(float damage, int level, Transform player);
}