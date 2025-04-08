using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ataque_Giratorio : MonoBehaviour
{
    public GameObject[] HitBoxAtaque;
    [SerializeField]
    int TiempoDeAtaque = 1;
    void Start()
    {
        for (int i = 0;  i < HitBoxAtaque.Length; i++)
        {
            HitBoxAtaque[i].SetActive(false);
        }
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Ataque();
        }
    }

    void Ataque()
    {
        for (int i = 0; i < HitBoxAtaque.Length; i++)
        {
            HitBoxAtaque[i].SetActive(true);
        }
        Invoke("DesactivarAtaque", TiempoDeAtaque);
    }

    void DesactivarAtaque()
    {
        for (int i = 0; i < HitBoxAtaque.Length; i++)
        {
            HitBoxAtaque[i].SetActive(false);
        }
    }
}
