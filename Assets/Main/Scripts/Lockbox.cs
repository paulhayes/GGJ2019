using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockbox : MonoBehaviour
{
	public Item RequiredItem;
	[Space]
	[SerializeField] GameObject unlockedIndicator;
	[SerializeField] GameObject lockedIndicator;
	[Space]
	[SerializeField] GameObject openIndicator;
	[SerializeField] GameObject dangerIndicator;
	[Space]
	[SerializeField] Animator openAnimator;
	[SerializeField] Dialog TryOpenDialog;
	[SerializeField] CanvasGroup counterFader;

	public Dialog DialogAfterOpen;

	public SofaView View;
	[SerializeField]
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

    public bool GetHovered ()
    {
        return hovered;
    }

    public void SetHover (bool value)
    {
        hovered = value;

        Debug.Log(canOpen);

        if (CanOpen)
        {
            openIndicator.SetActive(value && !IsOpening);
        }
        else
        {
            dangerIndicator.SetActive(value && !IsOpening);
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

    public void OpenBox()
    {
        if (canOpen)
        {
            IsOpening = true;
            openAnimator.SetTrigger("Play");
            _camera.gameObject.SetActive(true);
            openIndicator.SetActive(false);
            dangerIndicator.SetActive(false);

            if (unlockedIndicator != null)
                unlockedIndicator.SetActive(false);

            if (lockedIndicator != null)
                lockedIndicator.SetActive(false);

            DialogManager.Instance.Play(DialogAfterOpen);

            StartCoroutine(EndingRoutine());
        }
        else
        {
            if (TryOpenDialog != null && DialogManager.CurrentMessage != TryOpenDialog)
                DialogManager.Instance.Play(TryOpenDialog);

        }
    }

    //private void OnMouseEnter ()
    //{
    //	if (IsOpening)
    //		return;

    //	Hovered = true;
    //}

    //private void OnMouseExit ()
    //{
    //	if (IsOpening)
    //		return;

    //	Hovered = false;
    //}

    private void Update ()
	{
		//Hovered = View.isActiveAndEnabled && Hovered;
	}

	IEnumerator EndingRoutine()
	{
		foreach(var time in new TimedLoop(0.75f))
		{
			counterFader.alpha = 1.0f - time;
			yield return null;
		}
	}
}
