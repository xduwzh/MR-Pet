using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// 加载场景 —— 脚本挂载前场景
/// </summary>
public class ScenceStart : MonoBehaviour
{
    public GameObject winPanel;

    public void btnStart()
    {
        SceneManager.LoadScene(1);
    }

    public void btnQuit()
    {
        Application.Quit();
    }

    public void closePanel()
    {
        winPanel.SetActive(false);
    }
}

