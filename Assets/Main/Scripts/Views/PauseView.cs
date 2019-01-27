using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseView : AbstractView
{
    [SerializeField] Slider mouseSensSlider;
    [SerializeField] TextMeshProUGUI mouseSensNum;

    [SerializeField] SettingFloat mouseSens;

    private bool isViewing;

    public override void Begin()
    {
        this.enabled = true;
        gameObject.SetActive(true);

        Time.timeScale = 0;

        mouseSensSlider.minValue = 0.1f;
        mouseSensSlider.maxValue = 3;

        mouseSensSlider.value = mouseSens.value;
        mouseSensNum.text = mouseSens.value.ToString("F1");
    }

    public override void End()
    {
        Time.timeScale = 1;

        gameObject.SetActive(false);
        this.enabled = false;
    }

    public void Toggle ()
    {
        if (isViewing)
        {
            End();
        } else
        {
            Begin();
        }

        isViewing = !isViewing;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseSensSlider.value != mouseSens.value)
        {
            mouseSens.value = mouseSensSlider.value;

            mouseSensNum.text = mouseSens.value.ToString("F1");
        }

    }
}
