using UnityEngine;

public class colision_ataque : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemigo")
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Activador")
        {
            SpawnEnemigos spawner = other.GetComponent<SpawnEnemigos>();
            spawner.ActivarOleada();

            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<Collider>().enabled = false;

        }
    }
}
