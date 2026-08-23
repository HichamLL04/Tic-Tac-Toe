using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardManager : MonoBehaviour
{
    [SerializeField] public Button[] botones;
    [SerializeField] Sprite[] circulos;
    [SerializeField] Sprite[] circulos_win;
    [SerializeField] Sprite[] cruzes;
    [SerializeField] Sprite[] cruzes_win;
    [SerializeField] Sprite alpha;
    int[,] board = new int[3, 3];
    int[,] boardIndex = new int[3, 3];
    public static int turno; // 0 vacio, 1 cruz, 2 circulo
    public static BoardManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    void Start()
    {
        SetAlpha();

        turno = Random.Range(1, 3);
    }

    void SetAlpha()
    {
        foreach (Button boton in botones)
        {
            Image img = boton.transform.parent.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = alpha;
                img.color = Color.white;
            }
        }
    }

    public void OnButtonClick(int index)
    {
        int row = index / 3;
        int col = index % 3;
        PlacePiece(row, col);
        //StartCoroutine(PauseAllButtons());
    }

    void PlacePiece(int row, int col)
    {
        if (GameManager.instance.winner) return;

        if (board[row, col] != 0) return;

        int currentTurno = turno;
        var (sprite, spriteIndex) = GetRandomSprite();
        boardIndex[row, col] = spriteIndex;

        int buttonIndex = row * 3 + col;
        Image img = botones[buttonIndex].transform.parent.GetComponent<Image>();
        if (img != null)
        {
            img.sprite = sprite;
            img.color = Color.white;
        }

        board[row, col] = currentTurno;

        if (CheckWin(board[row, col]))
        {
            ShowWinSprites(board[row, col]);
            GameManager.instance.Result(currentTurno);
        }
        else if (CheckDraw())
        {
            GameManager.instance.Result(0);
        }
        else
        {
            EnableAllButtons();
        }
    }

    bool CheckWin(int player)
    {
        for (int row = 0; row < 3; row++)
        {
            if (board[row, 0] == player && board[row, 1] == player && board[row, 2] == player)
                return true;
        }
        for (int col = 0; col < 3; col++)
        {
            if (board[0, col] == player && board[1, col] == player && board[2, col] == player)
                return true;
        }
        if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
            return true;
        if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
            return true;

        return false;
    }

    bool CheckDraw()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == 0) return false;
            }
        }
        return true;
    }

    void ShowWinSprites(int player)
    {
        for (int row = 0; row < 3; row++)
        {
            if (board[row, 0] == player && board[row, 1] == player && board[row, 2] == player)
            {
                ChangeToWinSprite(row, 0, player);
                ChangeToWinSprite(row, 1, player);
                ChangeToWinSprite(row, 2, player);
                return;
            }
        }

        for (int col = 0; col < 3; col++)
        {
            if (board[0, col] == player && board[1, col] == player && board[2, col] == player)
            {
                ChangeToWinSprite(0, col, player);
                ChangeToWinSprite(1, col, player);
                ChangeToWinSprite(2, col, player);
                return;
            }
        }

        if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
        {
            ChangeToWinSprite(0, 0, player);
            ChangeToWinSprite(1, 1, player);
            ChangeToWinSprite(2, 2, player);
            return;
        }

        if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
        {
            ChangeToWinSprite(0, 2, player);
            ChangeToWinSprite(1, 1, player);
            ChangeToWinSprite(2, 0, player);
            return;
        }
    }

    void ChangeToWinSprite(int row, int col, int player)
    {
        int index = row * 3 + col;
        int spriteIndex = boardIndex[row, col];

        if (botones[index] == null) return;

        Image img = botones[index].transform.parent.GetComponent<Image>();
        if (img == null) return;

        img.color = Color.white;

        if (player == 1)
        {
            if (spriteIndex >= 0 && spriteIndex < circulos_win.Length)
                img.sprite = circulos_win[spriteIndex];
        }
        else
        {
            if (spriteIndex >= 0 && spriteIndex < cruzes_win.Length)
                img.sprite = cruzes_win[spriteIndex];
        }
    }

    void DisableAllButtons()
    {
        foreach (Button boton in botones)
        {
            boton.interactable = false;
        }
    }

    void EnableAllButtons()
    {
        foreach (Button boton in botones)
        {
            boton.interactable = true;
        }
    }

    IEnumerator PauseAllButtons()
    {
        DisableAllButtons();
        yield return new WaitForSeconds(0.5f);
        EnableAllButtons();
    }

    public void Retry()
    {
        boardIndex = new int[3, 3];
        board = new int[3, 3];
        SetAlpha();
        turno = Random.Range(1, 3);
        GameManager.instance.HideResult();
        EnableAllButtons();
    }

    (Sprite sprite, int index) GetRandomSprite()
    {
        Sprite sprite = alpha;
        int index = 0;

        if (turno == 1)
        {
            index = Random.Range(0, circulos.Length);
            sprite = circulos[index];
            turno = 2;
        }
        else if (turno == 2)
        {
            index = Random.Range(0, cruzes.Length);
            sprite = cruzes[index];
            turno = 1;
        }
        return (sprite, index);
    }
}