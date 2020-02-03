using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCanvas : MonoBehaviour
{
	public static ObjectCanvas Instance;

	public Transform Holder;

	public GameObject LastTarget;
	private GameObject destroy;
	
	public CurveInterpolator EntryBounce;
	
	private void Awake ()
	{
		Instance = this;
	}

	private void Update ()
	{
		EntryBounce.Update (Time.deltaTime);

		Holder.localScale = Vector3.one * EntryBounce.Value;
	}

	public void Render(GameObject target)
	{
		if(destroy != null)
			Destroy (destroy);

		LastTarget = target;

		var clone = Instantiate (target);

		clone.transform.SetParent (Holder);
		clone.transform.localPosition = Vector3.zero;
		clone.transform.localScale = Vector3.one;
		clone.transform.localRotation = Quaternion.identity;

		destroy = clone;
		EntryBounce.TargetValue = 1.0f;
	}

	public void Clear()
	{
		EntryBounce.TargetValue = 0.0f;
		// Destroy (destroy);
	}
}
