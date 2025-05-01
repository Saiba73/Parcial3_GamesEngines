using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public int puntos;
    public TMP_Text txtScore;

    void Update()
    {
        txtScore.text = puntos.ToString();
    }

    public void AgregarPuntos()
    {
        puntos = puntos + 1;
        if (puntos == 4)
        {
            SceneManager.LoadScene("MenuVictoria", LoadSceneMode.Single);
        }
    }
}
