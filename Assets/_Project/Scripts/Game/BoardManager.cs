using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardManager : MonoBehaviour
{
    [SerializeField] public Button[] botones;
    [SerializeField] Sprite[] circulos;
    [SerializeField] Sprite[] circulos_win;
    [SerializeField] Sprite[] circulos_loss;
    [SerializeField] Sprite[] cruzes;
    [SerializeField] Sprite[] cruzes_win;
    [SerializeField] Sprite[] cruzes_loss;
    [SerializeField] Sprite alpha;
    int[,] board = new int[3, 3];
    int[,] boardIndex = new int[3, 3];
    public static int turno; // 0 vacio, 1 cruz, 2 circulo
    public static BoardManager instance;

    private bool isProcessingTurn = false;

    // Modo Dinámico (3 piezas máximo por jugador, 20 movimientos máximo, 3 minutos límite)
    private System.Collections.Generic.Queue<(int row, int col)> p1Moves = new System.Collections.Generic.Queue<(int row, int col)>();
    private System.Collections.Generic.Queue<(int row, int col)> p2Moves = new System.Collections.Generic.Queue<(int row, int col)>();
    private int totalMovesCount = 0;
    private const int MAX_MOVES_DYNAMIC = 20;
    private float remainingTime = 180f;
    private bool isTimerRunning = false;

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
        InitTurn();
    }

    void Update()
    {
        if (MenuManager.isDynamicMode && isTimerRunning && GameManager.instance != null && !GameManager.instance.winner)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                isTimerRunning = false;
                DisableAllButtons();
                GameManager.instance.Result(0);
            }
        }
    }

    private void InitTurn()
    {
        turno = Random.Range(1, 3);
        isProcessingTurn = false;

        p1Moves.Clear();
        p2Moves.Clear();
        totalMovesCount = 0;
        remainingTime = 180f;
        isTimerRunning = MenuManager.isDynamicMode;

        if (MenuManager.isVsBot && turno == 2)
        {
            StartCoroutine(MakeBotMoveCoroutine());
        }
        else
        {
            EnableAllButtons();
        }
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
        if (isProcessingTurn) return;
        if (MenuManager.isVsBot && turno == 2) return;

        int row = index / 3;
        int col = index % 3;

        if (board[row, col] != 0) return;

        StartCoroutine(HumanTurnCoroutine(row, col));
    }

    private IEnumerator HumanTurnCoroutine(int row, int col)
    {
        isProcessingTurn = true;
        DisableAllButtons();

        bool gameEnded = PlacePiece(row, col);

        if (gameEnded)
        {
            yield break;
        }

        if (MenuManager.isVsBot)
        {
            yield return StartCoroutine(MakeBotMoveCoroutine());
        }
        else
        {
            // Pausa entre turnos en modo 2 jugadores (PvP)
            yield return new WaitForSeconds(0.4f);
            EnableAllButtons();
            isProcessingTurn = false;
        }
    }

    private IEnumerator MakeBotMoveCoroutine()
    {
        isProcessingTurn = true;
        DisableAllButtons();

        // Pausa antes de que el Bot juegue (tiempo de pensamiento)
        yield return new WaitForSeconds(0.5f);

        if (GameManager.instance != null && GameManager.instance.winner) yield break;

        var (r, c) = BotAI.GetMove(board, 2, MenuManager.botDifficulty);
        if (r >= 0 && c >= 0)
        {
            bool gameEnded = PlacePiece(r, c);
            if (!gameEnded)
            {
                // Pausa después de que el Bot juegue antes de habilitar el turno del jugador
                yield return new WaitForSeconds(0.4f);
                EnableAllButtons();
                isProcessingTurn = false;
            }
        }
    }

    /// <summary>
    /// Coloca la pieza y devuelve true si la partida ha terminado.
    /// </summary>
    private bool PlacePiece(int row, int col)
    {
        if (GameManager.instance != null && GameManager.instance.winner) return true;
        if (board[row, col] != 0) return false;

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

        if (MenuManager.isDynamicMode)
        {
            totalMovesCount++;

            var currentQueue = (currentTurno == 1) ? p1Moves : p2Moves;
            currentQueue.Enqueue((row, col));

            // Si ya hay más de 3 piezas del jugador, se elimina la primera pieza colocada
            if (currentQueue.Count > 3)
            {
                var (oldRow, oldCol) = currentQueue.Dequeue();
                board[oldRow, oldCol] = 0;
                boardIndex[oldRow, oldCol] = 0;

                int oldButtonIndex = oldRow * 3 + oldCol;
                Image oldImg = botones[oldButtonIndex].transform.parent.GetComponent<Image>();
                if (oldImg != null)
                {
                    oldImg.sprite = alpha;
                    oldImg.color = Color.white;
                }
            }

            // Si el jugador tiene 3 piezas activas, indicamos visualmente cuál es la más antigua (semi-transparente)
            if (currentQueue.Count == 3)
            {
                var (nextToVanishRow, nextToVanishCol) = currentQueue.Peek();
                int nextIndex = nextToVanishRow * 3 + nextToVanishCol;
                Image nextImg = botones[nextIndex].transform.parent.GetComponent<Image>();
                if (nextImg != null)
                {
                    nextImg.color = new Color(1f, 1f, 1f, 0.4f);
                }
            }
        }

        if (CheckWin(board[row, col]))
        {
            isTimerRunning = false;
            DisableAllButtons();
            ShowWinSprites(board[row, col]);
            GameManager.instance.Result(currentTurno);
            return true;
        }
        else if (MenuManager.isDynamicMode ? totalMovesCount >= MAX_MOVES_DYNAMIC : CheckDraw())
        {
            isTimerRunning = false;
            DisableAllButtons();
            ShowDrawSprites();
            GameManager.instance.Result(0);
            return true;
        }

        return false;
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

    void ShowDrawSprites()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                int player = board[row, col];
                if (player != 0)
                {
                    ChangeToLossSprite(row, col, player);
                }
            }
        }
    }

    void ChangeToLossSprite(int row, int col, int player)
    {
        int index = row * 3 + col;
        int spriteIndex = boardIndex[row, col];

        if (botones[index] == null) return;

        Image img = botones[index].transform.parent.GetComponent<Image>();
        if (img == null) return;

        img.color = Color.white;

        if (player == 1)
        {
            if (spriteIndex >= 0 && spriteIndex < circulos_loss.Length)
                img.sprite = circulos_loss[spriteIndex];
        }
        else
        {
            if (spriteIndex >= 0 && spriteIndex < cruzes_loss.Length)
                img.sprite = cruzes_loss[spriteIndex];
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

    public void Retry()
    {
        boardIndex = new int[3, 3];
        board = new int[3, 3];
        p1Moves.Clear();
        p2Moves.Clear();
        totalMovesCount = 0;
        remainingTime = 180f;
        SetAlpha();
        GameManager.instance.HideResult();
        InitTurn();
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