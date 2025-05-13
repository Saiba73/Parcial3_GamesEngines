using UnityEngine;

public class Animaciones : MonoBehaviour
{
    public Animator anim;
    public movimiento scriptMovimiento;
    public Sonido scriptSonido;
    int cantidadDeSaltos = 0;
    void Start()
    {
        
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetTrigger("Ataque");
        }


        if(scriptMovimiento.alturaDeSalto > 2f && Input.GetKeyDown(KeyCode.Space))
        {
            
            scriptSonido.personajeAudio.clip = scriptSonido.Salto;
            scriptSonido.personajeAudio.Play();
            

            anim.SetTrigger("Super Salto");
            cantidadDeSaltos++;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && cantidadDeSaltos < 2)
        {
            
            scriptSonido.personajeAudio.clip = scriptSonido.Salto;
            scriptSonido.personajeAudio.Play();
            

            anim.SetTrigger("Saltar");
            cantidadDeSaltos++;
            Debug.Log(cantidadDeSaltos);
        }

        if(scriptMovimiento.TocaPiso)
        {
           cantidadDeSaltos = 0;
        }

        


        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.E) && scriptMovimiento.TocaPiso)
        {
            //Debug.Log("Entro");
            anim.SetBool("Puede Caminar", false);
            anim.SetBool("Puede Correr", true);
        }
        else if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !Input.GetKey(KeyCode.E) && scriptMovimiento.TocaPiso )
        {
            anim.SetBool("Puede Caminar", true);
            anim.SetBool("Puede Correr", false);
        }
        
        else if(Input.GetAxis("Horizontal") == 0 || Input.GetAxis("Vertical") == 0)
        {
            anim.SetBool("Puede Caminar", false);
            anim.SetBool("Puede Caminar", false);
        }

        
        

    }
}
