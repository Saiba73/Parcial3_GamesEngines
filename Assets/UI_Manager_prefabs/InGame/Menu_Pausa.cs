using UnityEngine;

public class Menu_Pausa : MonoBehaviour
{
    
    public GameObject canvasDePausa;

    private void Start()
    {
        canvasDePausa.SetActive(false);
    }

    bool pausado = false;

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape) && !pausado)
        {
            pausar();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && pausado)
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
        Time.timeScale = 1;
    }
}
