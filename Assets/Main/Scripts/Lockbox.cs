using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockbox : MonoBehaviour
{
	[SerializeField] GameObject unlockedIndicator;
	[SerializeField] GameObject lockedIndicator;
	[Space]
	[SerializeField] GameObject openIndicator;
	[SerializeField] GameObject dangerIndicator;
	[SerializeField] Animator openAnimator;
	[SerializeField] Dialog TryOpenDialog;

	private bool canOpen;
	public bool IsOpening;

	[SerializeField] Cinemachine.CinemachineVirtualCamera _camera;

	private void Start ()
	{
		if (unlockedIndicator != null)
			unlockedIndicator.SetActive (CanOpen);
		if (lockedIndicator != null)
			lockedIndicator.SetActive (!CanOpen);

		openIndicator.SetActive (false);
		dangerIndicator.SetActive (false);
	}

	public bool CanOpen
	{
		get
		{
			return canOpen;
		}

		set
		{
			canOpen = value;
			if (unlockedIndicator != null)
				unlockedIndicator.SetActive (CanOpen);
			if (lockedIndicator != null)
				lockedIndicator.SetActive (!CanOpen);
		}
	}

	private void OnMouseEnter ()
	{
		if (IsOpening)
			return;

		if (CanOpen)
			openIndicator.SetActive (true);
		else
			dangerIndicator.SetActive (true);
	}

	private void OnMouseExit ()
	{
		if (IsOpening)
			return;

		if (CanOpen)
			openIndicator.SetActive (false);
		else
			dangerIndicator.SetActive (false);
	}

	private void OnMouseDown ()
	{
		Debug.Log ("Clicked");
		if(CanOpen)
		{
			IsOpening = true;
			openAnimator.SetTrigger ("Play");
			_camera.gameObject.SetActive (true);
			openIndicator.SetActive (false);
			dangerIndicator.SetActive (false);
		}
		else
		{
			if (TryOpenDialog != null)
				DialogManager.Instance.Play (TryOpenDialog);
		}
	}
}
