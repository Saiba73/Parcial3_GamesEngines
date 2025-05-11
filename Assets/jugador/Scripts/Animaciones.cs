using UnityEngine;

public class Animaciones : MonoBehaviour
{
    public Animator anim;
    public movimiento scriptMovimiento;
    void Start()
    {
        
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetTrigger("Ataque");
        }


        if(scriptMovimiento.alturaDeSalto > 2f && scriptMovimiento.saltoDeseado)
        {
            anim.SetTrigger("Super Salto");
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetBool("Puede Correr", true);
            anim.SetBool("Puede Caminar", false);
        }
        else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            anim.SetBool("Puede Caminar", true);
            anim.SetBool("Puede Correr", false);
        }
        else if(Input.GetAxis("Horizontal") == 0 || Input.GetAxis("Vertical") == 0)
        {
            anim.SetBool("Puede Caminar", false);
            anim.SetBool("Puede Caminar", false);
        }

        if (scriptMovimiento.saltoDeseado)
        {
            anim.SetTrigger("Saltar");
        }
        

    }
}
