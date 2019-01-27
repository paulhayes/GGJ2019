using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
	public static DialogManager Instance;

	[Serializable]
	public struct DisplayMapping
	{
		public Dialog.DisplayArea.Options Area;
		public DialogDisplay Display;
	}

	public Dialog PlayOnStart;
	public DisplayMapping[] Displays;
	[Space]
	public CanvasGroup ClickToContinue;
	public CurveInterpolator ClickToContinueCurve;
	[Space]
	public float fadeInTime = 0.5f;
	public float fadeOutTime = 0.5f;

	public bool IsProcessing;
	public event Action OnFinished;

	private Queue<Dialog> messageQueue = new Queue<Dialog> ();
	private AudioSource soundPlayer;

	public void Play(Dialog dialog)
	{
		if( dialog == null){
			Debug.LogWarning("DialogManager received null dialog");
			return;	
		}
		messageQueue.Enqueue (dialog);
	}

	private void Awake ()
	{
		Instance = this;
	}

	private void Start ()
	{
		ClickToContinueCurve.TargetValue = 0.0f;
		foreach (var findDisplay in Displays)
		{
			findDisplay.Display.gameObject.SetActive (false);
		}

		StartCoroutine (Process());
	}

	private void Update ()
	{
		ClickToContinueCurve.Update (Time.deltaTime);
		ClickToContinue.alpha = ClickToContinueCurve.Value;
	}

	IEnumerator<YieldInstruction> Process()
	{
		if(PlayOnStart)
			messageQueue.Enqueue (PlayOnStart);
		soundPlayer = gameObject.AddComponent<AudioSource> ();

		while (true)
		{
			while (messageQueue.Count == 0)
			{
				IsProcessing = false;
				if (OnFinished != null)
					OnFinished ();
				yield return null;
				IsProcessing = true;
			}

			var currentMessage = messageQueue.Dequeue ();

			if (currentMessage.Enqueues != null)
				messageQueue.Enqueue (currentMessage.Enqueues);

			foreach (var displayOptions in currentMessage.Displays)
			{
				displayOptions.CachePosiiton = 0;
			}

			yield return new WaitForSeconds (currentMessage.Delay);
			if (currentMessage.PlaySound != null)
				soundPlayer.PlayOneShot (currentMessage.PlaySound);

			if(currentMessage.DisplayObject != null && ObjectCanvas.Instance.LastTarget != currentMessage.DisplayObject)
				ObjectCanvas.Instance.Render (currentMessage.DisplayObject);

			foreach (var findDisplay in Displays)
			{
				findDisplay.Display.gameObject.SetActive (false);
			}
			foreach (var currentMessageDisplay in currentMessage.Displays)
			{
				foreach(var findDisplay in Displays)
				{
					if (findDisplay.Area == currentMessageDisplay.Area)
					{
						findDisplay.Display.gameObject.SetActive (true);
						if (currentMessageDisplay.WriteMode == Dialog.DisplayArea.TextSettings.Appear)
							findDisplay.Display.text.text = currentMessageDisplay.Text;
						else
							findDisplay.Display.text.text = "";
					}
				}
			}
			

			foreach (var time in new TimedLoop (fadeOutTime))
			{
				foreach (var findDisplay in Displays)
				{
					findDisplay.Display.fader.alpha = time;
					
					if (currentMessage != null)
					{
						foreach (var displayOptions in currentMessage.Displays)
						{
							if (findDisplay.Area == displayOptions.Area)
							{
								if (findDisplay.Display.text.text == displayOptions.Text)
								{
									findDisplay.Display.fader.alpha = 1.0f;
								}
							}
						}
					}
				}
				yield return null;
			}

			while (true)
			{
				bool hasTyped = false;
				foreach (var displayOptions in currentMessage.Displays)
				{
					foreach (var findDisplay in Displays)
					{
						if (findDisplay.Area == displayOptions.Area)
						{
							if (displayOptions.WriteMode == Dialog.DisplayArea.TextSettings.Typewriter &&
								displayOptions.CachePosiiton != displayOptions.Text.Length)
							{
								findDisplay.Display.text.text += displayOptions.Text[displayOptions.CachePosiiton];
								displayOptions.CachePosiiton++;
								hasTyped = true;
								yield return new WaitForSeconds (displayOptions.CharacterDisplay);
							}
						}
					}
				}
				if (!hasTyped)
					break;
			}
			
			yield return new WaitForSeconds (currentMessage.Duration);

			if(currentMessage.ClickToContinue)
			{
				ClickToContinueCurve.TargetValue = 1.0f;
				while (!PlayerInput.GetLeftMouseDown ())
					yield return null;
				ClickToContinueCurve.TargetValue = 0.0f;
			}

			// Get the next message and only fade out if the next message is not the same.
			Dialog nextMessage = null;
			if (messageQueue.Count != 0)
				nextMessage = messageQueue.Peek ();
			
			if (nextMessage != null && nextMessage.DisplayObject != currentMessage.DisplayObject)
			{
				if (ObjectCanvas.Instance.LastTarget == currentMessage.DisplayObject)
				{
					ObjectCanvas.Instance.Clear ();
				}
			}
			else if (nextMessage == null)
				ObjectCanvas.Instance.Clear ();

			foreach (var time in new TimedLoop (fadeOutTime))
			{
				foreach (var findDisplay in Displays)
				{
					findDisplay.Display.fader.alpha = 1.0f - time;
					if (nextMessage != null)
					{
						foreach (var displayOptions in nextMessage.Displays)
						{
							if (findDisplay.Area == displayOptions.Area)
							{
								if (findDisplay.Display.text.text == displayOptions.Text)
								{
									findDisplay.Display.fader.alpha = 1.0f;
								}
							}
						}
					}
				}
				yield return null;
			}

			yield return null;
		}
	}
}
