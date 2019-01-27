using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialog : ScriptableObject
{
	[Serializable]
	public class DisplayArea
	{
		public enum Options
		{
			Base,
			Header,
			Aside
		}
		public enum TextSettings
		{
			Appear,
			Typewriter
		}

		[TextArea(3, 10)]
		public string Text;
		public Options Area;
		public TextSettings WriteMode;
		public float CharacterDisplay;

		[NonSerialized]
		public int CachePosiiton;
	}

	public DisplayArea[] Displays = new DisplayArea[1];
	public float Delay;
	public float Duration = 2.0f;
	public bool ClickToContinue;
	public bool AutoContinue = true;
	[Space]
	public AudioClip PlaySound;
	public GameObject DisplayObject;
	[Space]
	public Dialog Enqueues;
	public bool FadeToBlack;
}
