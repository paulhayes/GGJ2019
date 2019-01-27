using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeToBlack : MonoBehaviour
{
	public static FadeToBlack Instance;

	public float FadeSpeed = 1.0f;
	public float CurrentValue;
	public bool Faded;

	private CanvasGroup canvasGroup;

	private void Awake ()
	{
		Instance = this;
		canvasGroup = GetComponent<CanvasGroup> ();
	}

	private void Update ()
	{
		if (Faded)
		{
			CurrentValue += Time.deltaTime * FadeSpeed;
		}
		else
		{
			CurrentValue -= Time.deltaTime * FadeSpeed;
		}

		CurrentValue = Mathf.Clamp01 (CurrentValue);
		canvasGroup.alpha = CurrentValue;
	}
}
