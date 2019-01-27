using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GapSpace : ScriptableObject
{
    [UnityEngine.Serialization.FormerlySerializedAs("positions")]
    public ItemPosition[] items;

    public Item GetClosestItem(Vector2 pos, ref float distance)
    {
        Item closestItem = null;
        float closestDistSqr = float.MaxValue;
        for(int i=0;i<items.Length;i++){
            if(items[i].item.hasBeenFound){
                continue;
            }
            float distSqr = (pos-items[i].position).sqrMagnitude;
            if( distSqr < closestDistSqr ){
                closestItem = items[i].item;
                closestDistSqr = distSqr;
            }
        }

        distance = Mathf.Sqrt(closestDistSqr);
        return closestItem;
    }

}
