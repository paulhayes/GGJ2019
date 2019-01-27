using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockbox : MonoBehaviour
{
    [SerializeField] GameObject gapIndicator;
	[SerializeField] GameObject dangerIndicator;
	[SerializeField] Animator openAnimator;
	[SerializeField] Dialog TryOpenDialog;

	public bool CanOpen;

	[SerializeField] Cinemachine.CinemachineVirtualCamera _camera;

	private void OnMouseEnter ()
	{
		if (CanOpen)
			gapIndicator.SetActive (true);
		else
			dangerIndicator.SetActive (true);
	}

	private void OnMouseExit ()
	{
		if (CanOpen)
			gapIndicator.SetActive (false);
		else
			dangerIndicator.SetActive (false);
	}

	private void OnMouseDown ()
	{
		Debug.Log ("Clicked");
		if(CanOpen)
		{
			openAnimator.SetTrigger ("Play");
		}
		else
		{
			if (TryOpenDialog != null)
				DialogManager.Instance.Play (TryOpenDialog);
		}
	}
}
