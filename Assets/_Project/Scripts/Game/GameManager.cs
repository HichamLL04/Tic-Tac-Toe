using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject resultado;
    [SerializeField] Sprite[] resultados;
    [SerializeField] GameObject tablero;
    int turnoActual;
    public static GameManager instance;
    public bool winner = false;


    void Awake()
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

    public void Result(int turno)
    {
        winner = true;
        turnoActual = turno;
        Invoke(nameof(ShowResult), 1.5f);
    }

    void ShowResult()
    {
        if (turnoActual == 1)
        {
            tablero.SetActive(false);
            resultado.GetComponentInChildren<Image>().sprite = resultados[2];
            resultado.SetActive(true);
        }
        else if (turnoActual == 2)
        {
            tablero.SetActive(false);
            resultado.GetComponentInChildren<Image>().sprite = resultados[1];
            resultado.SetActive(true);
        }
        else
        {
            tablero.SetActive(false);
            resultado.GetComponentInChildren<Image>().sprite = resultados[0];
            resultado.SetActive(true);
        }
    }

    public void HideResult()
    {
        resultado.SetActive(false);
        tablero.SetActive(true);
    }

    public void Retry()
    {
        turnoActual = BoardManager.turno;
        winner = false;
        BoardManager.instance.Retry();
    }
}