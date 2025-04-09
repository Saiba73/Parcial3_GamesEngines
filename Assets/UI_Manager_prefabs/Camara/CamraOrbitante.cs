using UnityEngine;

[RequireComponent (typeof(Camera))]
public class CamraOrbitante : MonoBehaviour
{
    [SerializeField]
    Transform enfoque = default;

    [SerializeField, Range(1f, 20f)]
    float distancia = 5f;

    [SerializeField, Min(0f)]
    float radioDeEnfoque = 1f;

    Vector3 puntoDeEnfoque;

    void Awake()
    {
        puntoDeEnfoque = enfoque.position;
    }

    void LateUpdate()
    {
        ActualizarPuntoDeEnfoque();
        Vector3 direccionDeCamara = transform.forward;
        transform.localPosition = puntoDeEnfoque - direccionDeCamara * distancia;
    }

    void ActualizarPuntoDeEnfoque()
    {
        Vector3 objetivo = enfoque.position;
        if(radioDeEnfoque > 0f)
        {
            float distanciaDeCamaraYObjetivo = Vector3.Distance(objetivo, puntoDeEnfoque);
            if(distanciaDeCamaraYObjetivo > radioDeEnfoque)
            {
                puntoDeEnfoque = Vector3.Lerp(objetivo, puntoDeEnfoque, radioDeEnfoque / distanciaDeCamaraYObjetivo);
            }
        }
        else
        {
            puntoDeEnfoque = objetivo;
        }
    }
}
