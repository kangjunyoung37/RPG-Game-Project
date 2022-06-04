using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace kang.Characters
{

    public interface IDamageable
    {
        bool IsAlive
        {
            get;
        }
        void TakveDamage(int damage, GameObject hitEffectPrefabs);

    }
}