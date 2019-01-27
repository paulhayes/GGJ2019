using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GapSpace : ScriptableObject
{
    public ItemPosition[] positions;

    public Item GetClosestItem(Vector2 pos, ref float distance)
    {
        throw new NotImplementedException();     
    }

}
