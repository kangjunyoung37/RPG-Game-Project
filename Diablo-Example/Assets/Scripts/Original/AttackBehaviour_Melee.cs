using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kang.Characters;
public class AttackBehaviour_Melee : AttackBehaviour
{
    public ManualCollision attackCollision;

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Collider[] colliders = attackCollision?.CheckoverlapBox(targetMask);
        foreach( Collider collider in colliders)
        {
          
            collider.gameObject.GetComponent<IDamageable>()?.TakveDamage(damage, effectPrefab);

        }
        calcCoolTime = 0.0f;
    }
}
