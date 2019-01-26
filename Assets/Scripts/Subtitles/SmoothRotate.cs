using UnityEngine;
using System.Collections;

public class SmoothRotate : MonoBehaviour
{
	public float speed = 1.0f;
	public float magnitude = 0.1f;

	public void Start ()
	{
		StartCoroutine (Shake ());
	}

	IEnumerator Shake ()
	{
		float elapsed = 0.0f;

		Vector3 originalCamRot = transform.eulerAngles;

		while (true)
		{
			float percentComplete = Time.unscaledTime + 16;

			float alpha = speed * percentComplete;

			float x = Mathf.PerlinNoise (alpha, 0) * 2.0f - 1.0f;
			float y = Mathf.PerlinNoise (0, alpha) * 2.0f - 1.0f;

			x *= magnitude;
			y *= magnitude;

			transform.eulerAngles = new Vector3 (originalCamRot.x + x, originalCamRot.y + y, originalCamRot.z);

			yield return null;
		}

		transform.eulerAngles = originalCamRot;
	}
}
