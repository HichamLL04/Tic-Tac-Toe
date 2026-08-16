using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject resultado;
    [SerializeField] Sprite[] resultados;
    [SerializeField] GameObject tablero;
    int turnoActual;

    public void Result(int turno)
    {
        turnoActual = turno;
        Invoke(nameof(ShowResult), 1f);
    }

    void ShowResult()
    {
        if (turnoActual == 1)
        {
            tablero.SetActive(false);
            resultado.GetComponent<Image>().sprite = resultados[2];
            resultado.SetActive(true);
        }
        else if (turnoActual == 2)
        {
            tablero.SetActive(false);
            resultado.GetComponent<Image>().sprite = resultados[1];
            resultado.SetActive(true);
        }
        else
        {
            tablero.SetActive(false);
            resultado.GetComponent<Image>().sprite = resultados[0];
            resultado.SetActive(true);
        }
    }
}