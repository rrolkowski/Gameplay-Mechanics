using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupManager : MonoBehaviour
{
    public List<EnemyAI> enemies;
    [SerializeField] float chainAggroRange = 5f;

    void Start()
    {
        foreach (EnemyAI enemy in enemies)
        {
            enemy.OnAggro += HandleChainAggro;
        }
    }

    void HandleChainAggro(EnemyAI aggroedEnemy)
    {
        Transform target = aggroedEnemy.GetChaseTarget();

        enemies.RemoveAll(enemy => enemy == null);

        foreach (EnemyAI enemy in enemies)
        {
            if (enemy != null)
            {
                if (enemy != aggroedEnemy && Vector3.Distance(enemy.transform.position, aggroedEnemy.transform.position) < chainAggroRange)
                {
                    enemy.SetChaseTarget(target);
                }
            }
        }
    }
}
