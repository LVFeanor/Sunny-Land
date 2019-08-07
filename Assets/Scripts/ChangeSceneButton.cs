using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    public void ChangeScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
        Time.timeScale = 1f;
    }
}
