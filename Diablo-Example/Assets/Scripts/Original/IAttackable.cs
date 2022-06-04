using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kang.Characters
{


    public interface IAttackable 
    {   
        AttackBehaviour CurrentAttackBehaviour
        {
            get;
        }

    void OnExecuteAttack(int attackIndex);
    
    }

}
