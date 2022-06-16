using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    public void OnGUI()
    {
        var style = new GUIStyle(GUI.skin.button);
        style.fontSize = 65;
        GUI.backgroundColor = Color.yellow;
        if (GUI.Button(new Rect(710, 650, 500, 200), "START", style))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
