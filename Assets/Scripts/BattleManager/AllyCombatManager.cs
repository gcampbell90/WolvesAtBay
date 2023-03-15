using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AllyCombatManager : MonoBehaviour
{
    private void OnEnable()
    {
        AllyManager.OnAttackCommand += () => Ally.OnAttackCommand?.Invoke(); 
        AllyManager.OnDefendCommand += () => Ally.OnDefendCommand?.Invoke(); ;
        AllyManager.OnDefendAttackCommand += () => Ally.OnDefendAttackCommand?.Invoke();
        ;
    }
    private void OnDisable()
    {
        AllyManager.OnAttackCommand -= () => Ally.OnAttackCommand?.Invoke();
        AllyManager.OnDefendCommand -= () => Ally.OnDefendCommand?.Invoke(); ;
        AllyManager.OnDefendAttackCommand -= () => Ally.OnDefendAttackCommand?.Invoke();
    }

}
