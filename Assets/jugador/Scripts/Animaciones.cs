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


        if(scriptMovimiento.alturaDeSalto > 2f && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Super Salto");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Saltar");
        }


        
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !(Input.GetKey(KeyCode.E)) || (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !(Input.GetKey(KeyCode.E) && Input.GetKeyUp(KeyCode.LeftShift)))
        {
            anim.SetBool("Puede Caminar", true);
            anim.SetBool("Puede Correr", false);
        }
        else if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && Input.GetKeyDown(KeyCode.LeftShift) && !(Input.GetKey(KeyCode.E)))
        {
            anim.SetBool("Puede Caminar", false);
            anim.SetBool("Puede Correr", true);
        }
        else if(Input.GetAxis("Horizontal") == 0 || Input.GetAxis("Vertical") == 0)
        {
            anim.SetBool("Puede Caminar", false);
            anim.SetBool("Puede Caminar", false);
        }

        
        

    }
}
