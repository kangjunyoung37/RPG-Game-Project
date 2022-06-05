using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace kang.Characters
{


public abstract class AttackBehaviour : MonoBehaviour
{

    #region Variables
#if UNITY_EDITOR
    [Multiline]//인스펙터창에서 주석을 넣어줄 수 있음
    public string delopmentDescription = "";
#endif
    public int animationIndex;

    
    [SerializeField]
    public bool IsAvailable => calcCoolTime >= coolTime;

    public int priority;

    public int damage;
    public float range = 1.5f;

    [SerializeField]
    protected float coolTime;
    protected float calcCoolTime = 0.0f;

    public GameObject effectPrefab;


    [HideInInspector]
    public LayerMask targetMask;

       



        #endregion Variables
    void Start()
    {
        calcCoolTime = coolTime;
    }


    void Update()
    {
        if(calcCoolTime < coolTime)
        {
            calcCoolTime += Time.deltaTime;
        }
     
    }

    public abstract void ExecuteAttack(GameObject target = null, Transform startPoint = null);

    }
}