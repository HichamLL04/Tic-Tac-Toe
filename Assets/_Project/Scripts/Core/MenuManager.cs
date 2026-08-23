using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    private Stack<string> sceneHistory = new Stack<string>();
    private bool isGoingBack = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSelectGame()
    {
        LoadScene("2_SelectMenu", 1);
    }

    public void LoadMainMenu()
    {
        sceneHistory.Clear();
        LoadScene("1_Menu", 1);
    }

    public void LoadSetting()
    {
        LoadScene("3_Setting", 1);
    }

    public void LoadCredit()
    {
        LoadScene("4_Credit", 1);
    }

    public void OpenReview()
    {
        #if UNITY_ANDROID
        string packageName = "cat.Hlaaouni.TicTacToe";
        Application.OpenURL("market://details?id=" + packageName);
        #endif
    }

    public void OpenGamePanel()
    {
        LoadScene("5_GamePanel", 1);
    }

    public void LoadPreviousScene()
    {
        if (sceneHistory.Count > 0)
        {
            string previousScene = sceneHistory.Pop();
            isGoingBack = true;
            SceneManager.LoadScene(previousScene, LoadSceneMode.Single);
        }
        else
        {
            LoadMainMenu();
        }
    }

    private void LoadScene(string sceneName, int id)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (!isGoingBack && !string.IsNullOrEmpty(currentScene) && currentScene != sceneName)
        {
            sceneHistory.Push(currentScene);
        }
        isGoingBack = false;

        if (id == 0)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        else if (id == 1)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}