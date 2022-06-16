using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kang.Characters;

public class PlayerAttackBehavoir_Melee : AttackBehaviour

{
    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        target.GetComponent<IDamageable>()?.TakeDamage(damage, effectPrefab);
    }
}
