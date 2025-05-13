using UnityEngine;

public class colision : MonoBehaviour
{

    public Score scoreScript;
    public Vida vidaScript;
    public Sonido scriptSonido;

    void Start()
    {
        scoreScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<Score>();
        vidaScript = GetComponent<Vida>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "enemigo")
        {
            //Destroy(collision.gameObject);
            vidaScript.QuitarVida();

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Item")
        {
            Destroy(other.gameObject);
            vidaScript.AgregarVida();

        }

        if (other.gameObject.tag == "Score")
        {
            scriptSonido.audioSecundario.clip = scriptSonido.Recojer;
            scriptSonido.audioSecundario.Play();
            Destroy(other.gameObject);
            scoreScript.AgregarPuntos();
        }

    }
}
