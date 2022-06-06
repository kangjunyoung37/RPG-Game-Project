using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kang.Characters;
public class AttackStateController : MonoBehaviour
{

    public delegate void OnEnterAttackState();
    public delegate void OnExitAttackState();

    public OnEnterAttackState enterAttackStateHandler;
    public OnExitAttackState exitAttackStateHandler;

    public bool IsInAttackState
    {
        get;
        private set;

    }
    void Start()
    {
        enterAttackStateHandler = new OnEnterAttackState(EnterAttackState);
        exitAttackStateHandler = new OnExitAttackState(ExitAttackState);
    }

    #region Helper Methods
    public void OnStartOfAttackState()
    {
        IsInAttackState = true;
        enterAttackStateHandler();
    }
    public void OnEndOfAttackState()
    {
        IsInAttackState = false;
        exitAttackStateHandler();
    }
    private void EnterAttackState()
    {

    }
    private void ExitAttackState()
    {

    }
    public void OnCheckAttackCollier(int attackIndex)
    {
    
        GetComponent<IAttackable>()?.OnExecuteAttack(attackIndex);
    }
    #endregion Helper Methods

}
