using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMLevelAbstract : MonoBehaviour
{
    protected GameManager GM;

    [SerializeField] protected int TotalEnemies;
    [SerializeField] protected int TotalEnemiesKilled = 0;

    [SerializeField]
    GameObject NextLevelTrigger; //The collider that will act as a trigger at the end of each level to allow the player to advance


    void Awake()
    {
        GM = GameManager.Instance;
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
