using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCanvasToken
{
	public ObjectCanvas Canvas;

	public ObjectCanvasToken(ObjectCanvas canvas)
	{
		Canvas = canvas;
	}
}

public class ObjectCanvas : MonoBehaviour
{
	public static ObjectCanvas Instance;

	public RectTransform Holder;

	private GameObject destroy;

	private void Awake ()
	{
		Instance = this;
	}

	public ObjectCanvasToken Render(GameObject target)
	{
		var clone = Instantiate (target);

		clone.transform.SetParent (Holder);
		clone.transform.localPosition = Vector3.zero;

		destroy = clone;

		return new ObjectCanvasToken (this);
	}

	public void Clear()
	{
		Destroy (destroy);
	}
}
