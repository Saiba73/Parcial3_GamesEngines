using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ataque_Giratorio : MonoBehaviour
{
    public GameObject[] HitBoxAtaque;

    [SerializeField]
    int TiempoDeAtaque = 1;

    bool prevenirRepeticionDeAtaque = false;
    
    public movimiento scriptMovimiento;
    void Start()
    {
        for (int i = 0;  i < HitBoxAtaque.Length; i++)
        {
            HitBoxAtaque[i].SetActive(false);
        }
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && prevenirRepeticionDeAtaque == false && scriptMovimiento.noPuedeAtaquar == false)
        {
            Ataque();
            prevenirRepeticionDeAtaque = true;
        }
        else if (Input.GetKeyUp(KeyCode.Q) && prevenirRepeticionDeAtaque == true)
        {
            prevenirRepeticionDeAtaque = false;
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
