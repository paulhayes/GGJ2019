using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialog : ScriptableObject
{
	[Serializable]
	public struct DisplayArea
	{
		public enum Options
		{
			Base,
			Header,
			Aside
		}

		public string Text;
		public Options Area;
	}
	
	public DisplayArea[] Displays = new DisplayArea[1];
	public float Duration = 2.0f;
	public AudioClip PlaySound;
}
