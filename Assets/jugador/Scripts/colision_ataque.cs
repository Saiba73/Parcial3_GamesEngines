using UnityEngine;

public class colision_ataque : MonoBehaviour
{
    public Sonido scriptSonido;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemigo")
        {
            //scriptSonido.audioSecundario.clip = scriptSonido.DestruyeEnemigo;
            //scriptSonido.audioSecundario.Play();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Activador")
        {
            scriptSonido.audioSecundario.clip = scriptSonido.Activador;
            scriptSonido.audioSecundario.Play();


            SpawnEnemigos spawner = other.GetComponent<SpawnEnemigos>();
            spawner.ActivarOleada();

            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<Collider>().enabled = false;
            other.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;

        }
    }
}
