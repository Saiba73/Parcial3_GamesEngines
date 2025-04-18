using UnityEngine;
using System.Collections.Generic;

public class SpawnEnemigos : MonoBehaviour
{
    public GameObject[] spawner;
    public GameObject enemigo;
    public GameObject orbe;
    public float tiempo;
    float contador;
    int oleadaEnemigos = 5;

    private List<GameObject> enemigosSpawneados = new List<GameObject>();
    private bool orbeActivo = false;
    private bool oleadaActiva = false;

    public void ActivarOleada()
    {
        if (!oleadaActiva && !orbeActivo)
        {
            oleadaActiva = true;
            contador = 0;
            Debug.Log("¡Oleada activada!");
        }
    }

    void Update()
    {
        if (!oleadaActiva) return;

        contador += Time.deltaTime;

        if (contador > tiempo && oleadaEnemigos > 0)
        {
            AparecerEnemigo();
            contador = 0;
        }

        if (!orbeActivo && oleadaEnemigos <= 0)
        {
            VerificarEnemigosVivos();
        }
    }

    public void AparecerEnemigo()
    {
        if (oleadaEnemigos > 0)
        {
            int i = Random.Range(0, spawner.Length);
            GameObject nuevoEnemigo = Instantiate(enemigo, spawner[i].transform.position, spawner[i].transform.rotation);
            enemigosSpawneados.Add(nuevoEnemigo);
            oleadaEnemigos--;
        }
    }

    void VerificarEnemigosVivos()
    {
        // Elimina las referencias nulas (enemigos destruidos)
        enemigosSpawneados.RemoveAll(item => item == null);

        if (enemigosSpawneados.Count == 0)
        {
            OleadaDerrotada();
        }
    }

    public void OleadaDerrotada()
    {
        if (!orbeActivo)
        {
            Instantiate(orbe, spawner[2].transform.position, spawner[2].transform.rotation);
            orbeActivo = true;
            Debug.Log("Oleada completada");
        }
    }
}