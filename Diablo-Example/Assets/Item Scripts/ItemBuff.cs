using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using kang.Core;
using Newtonsoft.Json;
namespace kang.InventorySystem.Items
{



    [Serializable]
    public class ItemBuff : IModifier
    {
        public AttributeType state;
        public int value;
        [SerializeField ,JsonProperty]
        private int min;
        [SerializeField ,JsonProperty]
        private int max;

        [JsonIgnore]
        public int Min => min;
        [JsonIgnore]
        public int Max => max;

        [JsonConstructor]
        public ItemBuff(int min, int max,int value)
        {
            this.min = min;
            this.max = max;
            this.value = value;

        }

        public ItemBuff(int min , int max)
        {
            this.min = min;
            this.max = max;

            GenerateValue();
        }
        public void GenerateValue()
        {
            value = UnityEngine.Random.Range(min, max);
        }
         public void AddValue(ref int v)
        {
            v += value;
        }

    }
}
