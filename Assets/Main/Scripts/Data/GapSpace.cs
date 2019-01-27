using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GapSpace : ScriptableObject
{
    [UnityEngine.Serialization.FormerlySerializedAs("positions")]
    public ItemPosition[] items;

    public Item GetClosestItem(Vector2 pos, ref float distance){
        var itemPos = GetClosestItemPosition(pos, ref distance);
        if(itemPos==null){
            return null;
        }
        else {
            return itemPos.item;
        }
    }

    public ItemPosition GetClosestItemPosition(Vector2 pos, ref float distance)
    {
        ItemPosition closestItem = null;
        float closestDistSqr = float.MaxValue;
        for(int i=0;i<items.Length;i++){
            if(!items[i].item.CanBeFound()){
                continue;
            }
            float distSqr = (pos-items[i].position).sqrMagnitude;
            if( distSqr < closestDistSqr ){
                closestItem = items[i];
                closestDistSqr = distSqr;
            }
        }

        distance = Mathf.Sqrt(closestDistSqr);
        return closestItem;
    }    

}
