using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMLevelAbstract : MonoBehaviour
{
    protected GameManager GM;

    public int TotalSwordsmen { get; set; }
    public int TotalSpearmen { get; set; }

    protected int TotalEnemies;

    protected int TotalEnemiesKilled = 0;

    [SerializeField]
    GameObject NextLevelTrigger; //The collider that will act as a trigger at the end of each level to allow the player to advance

    private void OnEnable()
    {
        Enemy.deathEvent += EnemyKilled;
    }
    private void OnDisable()
    {
        Enemy.deathEvent -= EnemyKilled;
    }

    protected void Awake()
    {
        GM = GameManager.Instance;

        TotalEnemies = TotalSpearmen + TotalSwordsmen;

    }

    public void CompleteGameState()
    {
        NextLevelTrigger.SetActive(true);
        Destroy(gameObject);
        OnCompletion();
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
        //EffectController.Instance.PlayDeathSound();
    }

    public virtual void OnCompletion() { }
}
