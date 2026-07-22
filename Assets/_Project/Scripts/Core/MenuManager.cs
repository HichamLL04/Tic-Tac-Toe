using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

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

    private void LoadScene(string sceneName, int id)
    {
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