using UnityEngine;

public class colision_ataque : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemigo")
        {
            Destroy(other.gameObject);
        }
    }
}
