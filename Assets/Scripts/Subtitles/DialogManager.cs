using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
	[Serializable]
	public struct DisplayMapping
	{
		public Dialog.DisplayArea.Options Area;
		public DialogDisplay Display;
	}
	public CanvasGroup Fader;

	public DisplayMapping[] Displays;
	public float fadeInTime = 0.5f;
	public float fadeOutTime = 0.5f;
	public Queue<Dialog> MessageQueue = new Queue<Dialog> ();

	private AudioSource soundPlayer;

	IEnumerator<YieldInstruction> Start()
	{
		soundPlayer = gameObject.AddComponent<AudioSource> ();

		while (true)
		{
			while (MessageQueue.Count == 0)
				yield return null;

			var currentMessage = MessageQueue.Dequeue ();

			soundPlayer.PlayOneShot (currentMessage.PlaySound);

			foreach(var time in new TimedLoop (fadeInTime))
			{
				Fader.alpha = time;
				yield return null;
			}



			foreach (var time in new TimedLoop (fadeOutTime))
			{
				Fader.alpha = 1.0f - time;
				yield return null;
			}

			yield return null;
		}
	}
}
