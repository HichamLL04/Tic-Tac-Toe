using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    [SerializeField] Button[] botones;
    [SerializeField] Sprite[] circulos;
    [SerializeField] Sprite[] cruzes;
    [SerializeField] Sprite alpha;
    int[,] board = new int[3, 3];
    static int turno; // 0 vacio, 1 cruz, 1 circulo


    void Start()
    {
        SetAlpha();
        for (int i = 0; i < botones.Length; i++)
        {
            int index = i;
            botones[i].onClick.AddListener(() => OnButtonClick(index));
        }
        turno = Random.Range(1, 3);
    }

    void SetAlpha()
    {
        foreach (Button boton in botones)
        {
            Image img = boton.GetComponent<Image>();
            img.sprite = alpha;
        }
    }

    void OnButtonClick(int index)
    {
        int row = index / 3;
        int col = index % 3;
        PlacePiece(row, col);
    }

    void PlacePiece(int row, int col)
    {
        if (board[row, col] != 0) return;
        botones[row * 3 + col].GetComponent<Image>().sprite = GetRandomSprite();
        board[row,col] = turno;
    }

    Sprite GetRandomSprite()
    {
        Sprite sprite = alpha;
        if (turno == 1)
        {
            sprite = circulos[Random.Range(0, circulos.Length)];
            turno = 2;

        }
        else if (turno == 2)
        {
            sprite = cruzes[Random.Range(0, cruzes.Length)];
            turno = 1;

        }
        return sprite;
    }

}
