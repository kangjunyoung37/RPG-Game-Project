using kang.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public GameObject muzzlePrefabs;
    public GameObject hitPrefabs;

    public AudioClip shotSFX;
    public AudioClip hitSFX;

    private bool collided;
    private Rigidbody rigidbody;

    [HideInInspector]
    public AttackBehaviour attackBeHaviour;

    [HideInInspector]
    public GameObject owner;

    [HideInInspector]
    public GameObject target;
    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        if(target != null)
        {
            Vector3 dest = target.transform.position;
            dest.y += 1.5f;
            transform.LookAt(dest);
        }
        if(owner)
        {
            Collider projectileCollier = GetComponent<Collider>();
            Collider[] ownerColliders = owner.GetComponentsInChildren<Collider>();
            foreach(Collider collider in ownerColliders)
            {
                Physics.IgnoreCollision(projectileCollier, collider);
            }
        }
        if (muzzlePrefabs)
        {
            GameObject muzzleVFX = Instantiate(muzzlePrefabs, transform.position,Quaternion.identity);
            muzzlePrefabs.transform.forward = gameObject.transform.forward;
            ParticleSystem particleSystem = muzzleVFX.GetComponent<ParticleSystem>();
            if (particleSystem)
            {
                Destroy(muzzleVFX, particleSystem.main.duration);
            }
            else
            {
                ParticleSystem childPaticleSystem = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                if (childPaticleSystem)
                {
                    Destroy(muzzleVFX, childPaticleSystem.main.duration);
                }
            }

        }
        if(shotSFX != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(shotSFX);
        }
    }
    protected virtual void FixedUpdate()//리지드바디같은 물리시스템을 사용할 때 사용
    {
        if(speed != 0 && rigidbody != null)
        {
            rigidbody.position += (transform.forward) * (speed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collided )
        {
            return;
        }
        collided = true;
        Collider projectileCollider = GetComponent<Collider>();
        projectileCollider.enabled = false;

        if(hitSFX != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(hitSFX);
        }
        speed = 0;
        rigidbody.isKinematic = true;

        ContactPoint contact = collision.contacts[0];
        Quaternion contactRotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 contextPosition = contact.point;
        if (hitPrefabs)
        {
            GameObject hitVFX = Instantiate(hitPrefabs, contextPosition, contactRotation);
            ParticleSystem particleSystem = hitVFX.GetComponent<ParticleSystem>();
            if (particleSystem)
            {
                Destroy(hitVFX, particleSystem.main.duration);
            }
            else
            {
                ParticleSystem childPaticleSystem = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                if (childPaticleSystem)
                {
                    Destroy(hitVFX, childPaticleSystem.main.duration);
                }
            }

        }
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(attackBeHaviour?.damage ?? 0,null);
        }
        StartCoroutine(DestroyParticle(0.0f));
        
    }
    public IEnumerator DestroyParticle(float waitTime)
    {
        if(transform.childCount>0 && waitTime !=0)
        {
            List<Transform> childs = new List<Transform>();
            foreach(Transform t in transform.GetChild(0).transform)
            {
                childs.Add(t);
            }
            while(transform.GetChild(0).localScale.x>0)
            {
                yield return new WaitForSeconds(0.01f);
                transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                for (int i = 0; i < childs.Count; ++i)
                {
                    childs[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
        }
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
