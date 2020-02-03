using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{

    public static int unlocked;
    public Dialog dialog;
    // text data
    
    // hint data
    // 3d model
    public int unlockedRequired = 0;
    

    // Resetable data
    public bool hasBeenFound;


    public bool CanBeFound(){
        Debug.LogFormat("unlocked={0}",unlocked);
        return unlocked>=unlockedRequired && !hasBeenFound;
    }

    public void Reset()
    {
        hasBeenFound = false;
    }

    public static void ResetAll(){
        unlocked = 0;
        var items = Resources.FindObjectsOfTypeAll<Item>();
        Debug.LogFormat("Resetting {0}",items.Length);
        System.Array.ForEach(items,(i)=>i.Reset());
    }

}
