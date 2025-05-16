using UnityEngine;

public class Respawn_Agua : MonoBehaviour
{

    public GameObject puntoDeSpawn;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.position = puntoDeSpawn.transform.position;
        }
    }
}
