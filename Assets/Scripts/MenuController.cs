using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuController : MonoBehaviour
{
    public void PlayGame(){
     SceneManager.LoadScene("Game");
    }

    public void QuitGame(){
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif


        Application.Quit();
    }
}
