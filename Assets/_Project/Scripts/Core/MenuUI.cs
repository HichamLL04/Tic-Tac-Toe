using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] Button playB;
    [SerializeField] Button returnB;
    [SerializeField] Button settingB;
    [SerializeField] Button muteB;
    [SerializeField] Button creditB;
    [SerializeField] Button onlineB;
    [SerializeField] Button botB;
    [SerializeField] Button pvpB;

    void Start()
    {
        if (playB != null)
        {
            playB.onClick.AddListener(() => MenuManager.instance.LoadSelectGame());
        }

        if (returnB != null)
        {
            returnB.onClick.AddListener(() => MenuManager.instance.LoadMainMenu());
        }

        if (settingB != null)
        {
            settingB.onClick.AddListener(() => MenuManager.instance.LoadSelectGame());
        }

        if (muteB != null)
        {
            muteB.onClick.AddListener(() => MenuManager.instance.LoadSelectGame());
        }

        if (creditB != null)
        {
            creditB.onClick.AddListener(() => MenuManager.instance.LoadSelectGame());
        }

        if (onlineB != null)
        {
            onlineB.onClick.AddListener(() => MenuManager.instance.LoadSelectGame());
        }

        if (botB != null)
        {
            botB.onClick.AddListener(() => MenuManager.instance.LoadSelectGame());
        }

        if (pvpB != null)
        {
            pvpB.onClick.AddListener(() => MenuManager.instance.LoadSelectGame());
        }
    }
}
