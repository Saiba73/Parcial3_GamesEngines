using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuVictoria_Botones : MonoBehaviour
{
    public void iniciarJuego()
    {
        SceneManager.LoadScene("JuegoPrincipal", LoadSceneMode.Single);
    }
    public void salirJuego()
    {
        SceneManager.LoadScene("MenuPrincipal", LoadSceneMode.Single);
    }
}
