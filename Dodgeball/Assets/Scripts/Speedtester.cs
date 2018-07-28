using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Speedtester : MonoBehaviour
{

    void Start()
    {
        gameObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Highscore: " + (int)(PlayerPrefs.GetFloat("HighScore") * 100);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Ball")
        {
            float ballSpeed = coll.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = ((int)(ballSpeed * 100)).ToString();

            if (ballSpeed > PlayerPrefs.GetFloat("HighScore"))
            {
                PlayerPrefs.SetFloat("HighScore", ballSpeed);

                gameObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Highscore: " + (int)(PlayerPrefs.GetFloat("HighScore") * 100);
            }
        }
    }
}
