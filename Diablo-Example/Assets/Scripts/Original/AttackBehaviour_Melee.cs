using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kang.Characters;
public class AttackBehaviour_Melee : AttackBehaviour
{
    public ManualCollision attackCollision;

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        
        Debug.Log("½Ã°£" + calcCoolTime + "ÄðÅ¸ÀÓ" + coolTime);
        Collider[] colliders = attackCollision?.CheckoverlapBox(targetMask);
        foreach( Collider collider in colliders)
        {
          
            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage, effectPrefab);

        }
        calcCoolTime = 0.0f;

    }
}
