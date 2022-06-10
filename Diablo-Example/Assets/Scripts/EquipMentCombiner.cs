using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipMentCombiner 
{
    private readonly Dictionary<int , Transform> rootBoneDictionary = new Dictionary<int, Transform>();

    private readonly Transform transform;

    public EquipMentCombiner(GameObject rootGO)
    {
        transform = rootGO.transform;
       
    }
}
