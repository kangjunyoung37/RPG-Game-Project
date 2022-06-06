using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowProjectile : Projectile
{
    public float destroyDelay = 5.0f;
    protected override void Start()
    {
        base.Start();

        StartCoroutine(DestroyParticle(destroyDelay));
    }
    protected override void FixedUpdate()
    {
        if (target)
        {
            Vector3 dest = target.transform.position;
            dest.y += 1.5f;
            transform.LookAt(dest);
        }
        base.FixedUpdate();
    }
}
