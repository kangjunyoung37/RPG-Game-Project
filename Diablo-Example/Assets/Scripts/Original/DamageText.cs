using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace kang.UIs
{


    public class DamageText : MonoBehaviour
    {

        public float delayTimeToDestroy = 1.0f;
        private TextMeshProUGUI textMeshPro;
    
        private void Start()
        {
            Destroy(gameObject, delayTimeToDestroy);
        }
        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }
        public int Damage
        {
            get
            {
                if (textMeshPro != null)
                {
                    return int.Parse(textMeshPro.text);
                }
                return 0;
            }
            set
            {
                if(textMeshPro != null)
                {
                    textMeshPro.text = value.ToString();
                }
            }
        }


    }


}
