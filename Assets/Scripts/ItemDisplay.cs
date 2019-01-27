using System.Collections;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
	public Item Identifier;
	public AnimationCurve InCurve;
	public float InDuration = 1.0f;
	public GameObject Rendering;

	private void Start ()
	{
		Rendering.SetActive (false);
	}

	public void Play ()
	{
		Rendering.SetActive (true);
		StartCoroutine (PlayCoroutine ());
	}

	private IEnumerator PlayCoroutine()
	{
		foreach(var time in new TimedLoop(InDuration))
		{
			Rendering.transform.localScale = Vector3.one * InCurve.Evaluate (time);
			yield return null;
		}
	}
}
