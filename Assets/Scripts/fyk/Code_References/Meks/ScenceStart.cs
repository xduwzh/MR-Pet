using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// ���س��� ���� �ű�����ǰ����
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

