using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollection : MonoBehaviour
{
	public Image ProgressBar;
	public Text Counter;

	public DampenInterpolator Dampener;

	public Lockbox Reciever;

	private ItemDisplay[] items;
	private int completed;
	
    private void Awake()
    {
		items = GetComponentsInChildren<ItemDisplay> ();
		ProgressBar.fillAmount = 0.0f;
		Counter.text = "0";
		Dampener.Value = 0.0f;
		Dampener.TargetValue = 0.0f;
	}

	private void Update ()
	{
		Dampener.Update (Time.deltaTime);
		ProgressBar.fillAmount = Dampener.Value;
	}

	public void Complete(Item item)
	{
		if (Reciever.RequiredItem == item)
		{
			Reciever.CanOpen = true;
		}
		foreach(var display in items)
		{
			if (display.Identifier == item)
			{
				display.Play ();

				completed++;
				Counter.text = completed.ToString();

				Dampener.TargetValue = ((float)completed) / ((float)items.Length);
				break;
			}
		}
	}
}
