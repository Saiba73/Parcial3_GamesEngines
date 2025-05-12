using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Pausa : MonoBehaviour
{

    public GameObject canvasDePausa;
    public GameObject controles;

    private void Start()
    {
        canvasDePausa.SetActive(false);
        controles.SetActive(false);

    }

    bool pausado = false;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !pausado)
        {
            pausar();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pausado)
        {
            continuar();
        }

    }

    void pausar()
    {
        canvasDePausa.SetActive(true);
        pausado = true;
        Time.timeScale = 0;
    }

    public void continuar()
    {
        canvasDePausa.SetActive(false);
        pausado = false;
        controles.SetActive(false);
        Time.timeScale = 1;
    }

    public void controlesMenu()
    {
        controles.SetActive(true);
    }

    public void controlesMenuCerrar()
    {
        controles.SetActive(false);
    }

    public void salirJuego()
    {
        SceneManager.LoadScene("MenuPrincipal", LoadSceneMode.Single);
    }
}