using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal_Botones : MonoBehaviour
{

    public void iniciarJuego()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
