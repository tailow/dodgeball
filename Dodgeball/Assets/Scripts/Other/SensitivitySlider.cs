using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensitivitySlider : MonoBehaviour
{
    public TMP_Text sensitivityText;

    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = transform.parent.parent.parent.gameObject.GetComponent<PlayerMovement>();

        sensitivityText.text = GetComponent<Slider>().value.ToString();
    }

    public void ChangeSensitivity()
    {
        playerMovement.sensitivity = GetComponent<Slider>().value / 10f;

        sensitivityText.text = GetComponent<Slider>().value.ToString();
    }
}
