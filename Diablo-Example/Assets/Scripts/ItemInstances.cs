using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstances 
{
    public List<Transform> itemTransfroms = new List<Transform>();

    public void Destroy()
    {
        foreach(Transform item in itemTransfroms)
        {
           GameObject.Destroy(item.gameObject);
        }
    }
}
