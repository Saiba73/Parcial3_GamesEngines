using UnityEngine;

public class RespawnearJugador : MonoBehaviour
{
    public GameObject SpawnPoint;
    Vector3 posicionDeSpawn;

    void Start()
    {
        posicionDeSpawn = SpawnPoint.transform.localPosition;
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            transform.localPosition = posicionDeSpawn;
        }
    }
}
