using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] Button playB;
    [SerializeField] Button returnB;
    [SerializeField] Button settingB;
    [SerializeField] Button reviewB;
    [SerializeField] Button creditB;
    [SerializeField] Button onlineB;
    [SerializeField] Button botB;
    [SerializeField] [Range(1, 3)] int botDifficulty = 2; // 1: Fácil, 2: Medio, 3: Imposible (Minimax)
    [SerializeField] Button pvpB;
    [SerializeField] Button pvpB2;
    [SerializeField] Button easyBotB;
    [SerializeField] Button mediumBotB;
    [SerializeField] Button hardBotB;
    [SerializeField] Button retry;

    void Start()
    {
        if (playB != null)
        {
            playB.onClick.AddListener(() => MenuManager.instance.LoadSelectGame());
        }

        if (returnB != null)
        {
            returnB.onClick.AddListener(() => MenuManager.instance.LoadPreviousScene());
        }

        if (settingB != null)
        {
            settingB.onClick.AddListener(() => MenuManager.instance.LoadSetting());
        }

        if (reviewB != null)
        {
            reviewB.onClick.AddListener(() => MenuManager.instance.OpenReview());
        }

        if (creditB != null)
        {
            creditB.onClick.AddListener(() => MenuManager.instance.LoadCredit());
        }

        if (onlineB != null)
        {
            onlineB.onClick.AddListener(() => MenuManager.instance.LoadSelectGame());
        }

        if (botB != null)
        {
            botB.onClick.AddListener(() => MenuManager.instance.OpenGamePanelBot(botDifficulty));
        }

        if (easyBotB != null)
        {
            easyBotB.onClick.AddListener(() => MenuManager.instance.OpenGamePanelBot(1));
        }

        if (mediumBotB != null)
        {
            mediumBotB.onClick.AddListener(() => MenuManager.instance.OpenGamePanelBot(2));
        }

        if (hardBotB != null)
        {
            hardBotB.onClick.AddListener(() => MenuManager.instance.OpenGamePanelBot(3));
        }

        if (pvpB != null)
        {
            pvpB.onClick.AddListener(() => MenuManager.instance.OpenGamePanelPvP());
        }

        if (pvpB2 != null)
        {
            pvpB2.onClick.AddListener(() => MenuManager.instance.OpenGamePanelDynamicPvP());
        }

        if (retry != null)
        {
            retry.onClick.AddListener(() => GameManager.instance.Retry());
        }
    }
}
