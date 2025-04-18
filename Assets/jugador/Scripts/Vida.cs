using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vida : MonoBehaviour
{
    public int hp;
    public TMP_Text txtHP;

    void Update()
    {
        txtHP.text = hp.ToString();
    }

    public void AgregarVida()
    {
        hp += 1;
    }

    public void QuitarVida()
    {
        hp -= 1;
        if (hp <= 0)
        {
            SceneManager.LoadScene("MenuDerrota", LoadSceneMode.Single);
        }

    }
}
