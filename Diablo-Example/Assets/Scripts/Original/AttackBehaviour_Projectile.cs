using kang.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour_Projectile : AttackBehaviour
{
    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        
        Debug.Log("½Ã°£" + calcCoolTime + "ÄðÅ¸ÀÓ" + coolTime);
        //if (target == null)
        //{
            
        //    return;
        //}
        
        Vector3 projectilePosition = startPoint?.position ?? transform.position;
        if(effectPrefab)
        {
        
            GameObject projectileGO = GameObject.Instantiate<GameObject>(effectPrefab, projectilePosition, Quaternion.identity);
            projectileGO.transform.forward = transform.forward;

            Projectile projectile = projectileGO.GetComponent<Projectile>();
            if (projectile)
            {
                projectile.owner = this.gameObject;
                projectile.target = target;
                projectile.attackBeHaviour = this;
            }
        }
        calcCoolTime = 0.0f;
    }
}
