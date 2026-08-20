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

    void Start()
    {
        SetAlpha();

        turno = Random.Range(1, 3);
    }

    void SetAlpha()
    {
        foreach (Button boton in botones)
        {
            Image img = boton.GetComponentInParent<Image>();
            img.sprite = alpha;
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
        botones[row * 3 + col].transform.parent.GetComponent<Image>().sprite = GetRandomSprite();
        board[row, col] = turno;

        if (CheckWin(board[row, col]))
        {
            GameManager.instance.Result(turno);
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
        board = new int[3, 3];
        SetAlpha();
        turno = Random.Range(1, 3);
        GameManager.instance.HideResult();
        EnableAllButtons();
    }

    Sprite GetRandomSprite()
    {
        Sprite sprite = alpha;
        if (turno == 1)
        {
            sprite = circulos[Random.Range(0, circulos.Length / 2)];
            turno = 2;
        }
        else if (turno == 2)
        {
            sprite = cruzes[Random.Range(0, circulos.Length / 2)];
            turno = 1;
        }
        return sprite;
    }

    Sprite SetWinSprite()
    {
        return null;
    }
}