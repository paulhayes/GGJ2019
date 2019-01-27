using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
	public static DialogManager Instance;
	public static Dialog CurrentMessage;

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
	private Coroutine routine;

	public void Play(Dialog dialog)
	{
		if( dialog == null){
			Debug.LogWarning("DialogManager received null dialog");
			return;	
		}
		messageQueue.Enqueue (dialog);
		if (routine == null)
			routine = StartCoroutine (Process());
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
		soundPlayer = gameObject.AddComponent<AudioSource> ();
		
		if(PlayOnStart)
			Play(PlayOnStart);
	}

	private void Update ()
	{
		ClickToContinueCurve.Update (Time.deltaTime);
		ClickToContinue.alpha = ClickToContinueCurve.Value;
	}

	IEnumerator<YieldInstruction> Process()
	{
		while (true)
		{
			if (messageQueue.Count == 0)
			{
				IsProcessing = false;
				if (OnFinished != null)
					OnFinished ();
				routine = null;
				yield break;
			}

			CurrentMessage = messageQueue.Dequeue ();

			if (CurrentMessage.Enqueues != null)
				messageQueue.Enqueue (CurrentMessage.Enqueues);

			foreach (var displayOptions in CurrentMessage.Displays)
			{
				displayOptions.CachePosiiton = 0;
			}
			
			yield return new WaitForSeconds (CurrentMessage.Delay);
			if (CurrentMessage.PlaySound != null)
				soundPlayer.PlayOneShot (CurrentMessage.PlaySound);

			if(CurrentMessage.DisplayObject != null && ObjectCanvas.Instance.LastTarget != CurrentMessage.DisplayObject)
				ObjectCanvas.Instance.Render (CurrentMessage.DisplayObject);

			foreach (var findDisplay in Displays)
			{
				findDisplay.Display.gameObject.SetActive (false);
			}
			foreach (var currentMessageDisplay in CurrentMessage.Displays)
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
					
					if (CurrentMessage != null)
					{
						foreach (var displayOptions in CurrentMessage.Displays)
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
				foreach (var displayOptions in CurrentMessage.Displays)
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
			
			yield return new WaitForSeconds (CurrentMessage.Duration);

			if(CurrentMessage.ClickToContinue)
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
			
			if (nextMessage != null && nextMessage.DisplayObject != CurrentMessage.DisplayObject)
			{
				if (ObjectCanvas.Instance.LastTarget == CurrentMessage.DisplayObject)
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
