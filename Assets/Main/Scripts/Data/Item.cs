using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{

    public Dialog dialog;
    // text data
    
    // hint data
    // 3d model
    
    

    // Resetable data
    public bool hasBeenFound;

    public void Reset()
    {
        hasBeenFound = false;
    }

    public static void ResetAll(){
        var items = Resources.FindObjectsOfTypeAll<Item>();
        Debug.LogFormat("Resetting {0}",items.Length);
        System.Array.ForEach(items,(i)=>i.Reset());
    }

}
