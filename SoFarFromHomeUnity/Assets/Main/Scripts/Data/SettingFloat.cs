using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SettingFloat : ScriptableObject
{
    public float value;

    private void OnEnable()
    {
        PlayerPrefs.GetFloat("MouseSensitivity");
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
    }
}
