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

	public SofaView View;

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

	private bool hovered;

	public bool Hovered
	{
		get
		{
			return hovered;
		}
		set
		{
			hovered = value;

			if (CanOpen)
				openIndicator.SetActive (value);
			else
				dangerIndicator.SetActive (value);
		}
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

		Hovered = true;
	}

	private void OnMouseExit ()
	{
		if (IsOpening)
			return;

		Hovered = false;
	}

	private void Update ()
	{
		Hovered = View.isActiveAndEnabled && Hovered;
	}

	private void OnMouseDown ()
	{
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
