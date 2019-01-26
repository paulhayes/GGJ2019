using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialog : ScriptableObject
{
	public struct DisplayArea
	{
		public enum Options
		{
			Base,
			Header,
			Aside
		}

		public string Text;
	}

	public DisplayArea[] Displays;
}
