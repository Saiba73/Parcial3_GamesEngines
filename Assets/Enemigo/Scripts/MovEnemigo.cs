using UnityEngine;
using UnityEngine.AI;

public class MovEnemigo : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject destino;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destino = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        agent.SetDestination(destino.transform.position);

    }
}
