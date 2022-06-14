using kang.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public float damageInterval = 0.5f;
    public float damageDuration = 5f;
    public int damage = 5;

    private float calcDuration = 0.0f;
    [SerializeField]
    private ParticleSystem effect;

    private IDamageable damageble;

    private void Update()
    {
        if(damageble != null)
        {
            calcDuration -= Time.deltaTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        damageble = other.GetComponent<IDamageable>();
        if(damageble != null )
        {
            calcDuration = damageDuration;

            //effect.Play();
            StartCoroutine(ProcessDamage());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        damageble = null;
        StopAllCoroutines();
       // effect.Stop();
    }
    IEnumerator ProcessDamage()
    {
        while(calcDuration > 0 && damageble != null)
        {
            damageble.TakeDamage(damage, null);
            yield return new WaitForSeconds(damageInterval);
        }
        damageble = null;
       /// effect.Stop();
    }

}
