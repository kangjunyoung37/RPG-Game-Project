using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using kang.UIs;
public class NPCBattleUI : MonoBehaviour
{
    private Slider hpSlider;
    [SerializeField]
    private GameObject damgeTextPrefab;
    public float MinimumValue
    {
        get => hpSlider.minValue;
        set => hpSlider.minValue = value;
    }
    public float MaximumValue
    {
        get => hpSlider.maxValue;
        set => hpSlider.maxValue = value;
    }
    public float Value
    {
        get => hpSlider.value;
        set => hpSlider.value = value;
    }

    private void Awake()
    {
        hpSlider = gameObject.GetComponentInChildren<Slider>();
    }
    private void OnEnable()
    {
        GetComponent<Canvas>().enabled = true;
    }
    private void OnDisable()
    {
        GetComponent<Canvas>().enabled = false;
    }
    public void CreateDamageText(int damage)
    {
        if(damgeTextPrefab != null)
        {
            GameObject damageTextGO = Instantiate(damgeTextPrefab, transform);
            DamageText damageText = damageTextGO.GetComponent<DamageText>();
            if(damageText == null)
            {
                Destroy(damageTextGO);
            }
            damageText.Damage = damage;

        }
    }
}
