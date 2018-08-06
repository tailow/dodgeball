using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public void PlayMultiplayer()
    {
        SceneManager.LoadScene("scene_launcher");
    }

    public void PlayTraining()
    {
        SceneManager.LoadScene("scene_training");
    }
}
