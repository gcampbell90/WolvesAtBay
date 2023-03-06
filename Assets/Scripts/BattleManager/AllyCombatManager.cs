using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AllyCombatManager : MonoBehaviour
{
    private void OnEnable()
    {
        AllyManager.OnAttackCommand += Attack;
        AllyManager.OnDefendCommand += Defend;
        //Player.OnDefend += Defend;
        //Player.OnAttack += Attack;
    }
    private void OnDisable()
    {
        AllyManager.OnAttackCommand -= Attack;
        AllyManager.OnDefendCommand -= Defend;

        //Player.OnDefend -= Defend;
        //Player.OnAttack -= Attack;
    }
    private void Attack()
    {
        Debug.Log("Attack Command");
        Ally.OnAttackCommand?.Invoke();
    }
    private void Defend()
    {
        Debug.Log("Defend Command");
        Ally.OnDefendCommand?.Invoke();
    }
}
