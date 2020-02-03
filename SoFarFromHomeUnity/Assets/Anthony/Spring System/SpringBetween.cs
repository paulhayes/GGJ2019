using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Spring
{
	public float Power;
	public float Damper;

	public float Target;

	public float Value;
	public float Velocity;
	
	public void Update(float deltaTime)
	{
		float direction = Target - Value;

		Velocity += direction * Power * deltaTime;

		Velocity -= Velocity * Damper * deltaTime;

		Value += Velocity * deltaTime;
	}
}

public class SpringBetween : MonoBehaviour
{
	public Spring spring;
	public float bounceForce = 10;
	public float Scaler = 1.25f;

	public CanvasGroup Fader;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.X))
		{
			spring.Velocity += bounceForce;
		}

		spring.Update(Time.deltaTime);

		Fader.alpha = spring.Value;
		Fader.transform.localScale = Vector3.one + (Vector3.one * Scaler * (1.0f - Mathf.Clamp01 (spring.Value)));
	}
}
