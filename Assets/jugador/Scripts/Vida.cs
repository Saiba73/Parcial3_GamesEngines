using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vida : MonoBehaviour
{
    public int hp;
    public TMP_Text txtHP;

    void Update()
    {
        //txtHP.text = hp.ToString();
    }

    public void AgregarVida()
    {
        hp++;
    }

    public void QuitarVida()
    {
        hp--;
        if (hp <= 0)
        {
            SceneManager.LoadScene("MenuDerrota", LoadSceneMode.Single);
        }

    }
}
