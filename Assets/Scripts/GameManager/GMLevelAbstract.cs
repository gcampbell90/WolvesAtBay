using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMLevelAbstract : MonoBehaviour
{
    protected GameManager GM;

    [SerializeField] protected int totalSpearmen;
    [SerializeField] protected int totalSwordsmen;
    protected int TotalEnemies;

    [SerializeField] protected int TotalEnemiesKilled = 0;

    [SerializeField]
    GameObject NextLevelTrigger; //The collider that will act as a trigger at the end of each level to allow the player to advance

    public delegate void DeathEvent();
    public static event DeathEvent event_death;

    protected void Awake()
    {
        GM = GameManager.Instance;

        TotalEnemies = totalSpearmen + totalSwordsmen;

    }

    public void CompleteGameState()
    {
        NextLevelTrigger.SetActive(true);
    }

    public void CheckIfAllEnemiesKilled()
    {
        if (TotalEnemiesKilled == TotalEnemies)
        {
            CompleteGameState();
        }
    }

    public virtual void EnemyKilled()
    {
        TotalEnemiesKilled++;
        CheckIfAllEnemiesKilled();
        Debug.Log("GM EnemyKilled");
        //Play potential audio effect/particle effect here?
    }

    public void CheckIfComplete()
    {
        if (TotalEnemiesKilled == TotalEnemies)
        {
            OnCompletion();
        }
        else
        {
            return;
        }
    }

    public virtual void OnCompletion() { }
}
