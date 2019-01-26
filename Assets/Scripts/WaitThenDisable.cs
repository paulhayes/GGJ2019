using UnityEngine;

public class WaitThenDisable : MonoBehaviour
{
	public float Delay = 1.0f;

    private void Start()
    {
		Invoke ("DisableObject", Delay);
    }

	private void DisableObject()
	{
		gameObject.SetActive (false);
	}
}
