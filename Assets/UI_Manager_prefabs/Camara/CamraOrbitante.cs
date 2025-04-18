using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent (typeof(Camera))]
public class CamraOrbitante : MonoBehaviour
{
    [SerializeField]
    Transform enfoque = default;

    [SerializeField, Range(1f, 20f)]
    float distancia = 5f;

    [SerializeField, Min(0f)]
    float radioDeEnfoque = 1f;

    [SerializeField, Range(0f, 1f)]
    float centroEnfocando = 0.5f;

    [SerializeField, Range(1f, 360f)]
    float velocidadDeRotacion = 90f;

    [SerializeField, Range(-89f, 89f)]
    float anguloVerticalMin = -30f, anguloVerticalMax = 60f;

    [SerializeField, Min(0f)]
    float retrasoDeAlineacion = 5F;

    float ultimaRotacionManualTiempo;

    Vector2 angulosDeOrbita = new Vector2(45f, 0f);

    Vector3 puntoDeEnfoque, puntoDeEnfoquePrevio;

    private void OnValidate()
    {
        if(anguloVerticalMax < anguloVerticalMin)
        {
            anguloVerticalMax = anguloVerticalMin;
        }
    }

    void Awake()
    {
        puntoDeEnfoque = enfoque.position;
        transform.localRotation = Quaternion.Euler(angulosDeOrbita);
    }

    void LateUpdate()
    {
        ActualizarPuntoDeEnfoque();
        Quaternion rotacionVista;
        if (RotacionManual() || RotacionAutomatica())
        {
            RestringirAngulos();
            rotacionVista = Quaternion.Euler(angulosDeOrbita);
        }
        else
        {
            rotacionVista = transform.localRotation;
        }
        //Quaternion rotacionVista = Quaternion.Euler(angulosDeOrbita);
        Vector3 direccionDeCamara = rotacionVista * Vector3.forward;
        Vector3 posicionDeVista = puntoDeEnfoque - direccionDeCamara * distancia;
        transform.SetPositionAndRotation(posicionDeVista, rotacionVista);
    }

    void ActualizarPuntoDeEnfoque()
    {
        puntoDeEnfoquePrevio = puntoDeEnfoque;
        Vector3 objetivo = enfoque.position;
        if(radioDeEnfoque > 0f)
        {
            float distanciaDeCamaraYObjetivo = Vector3.Distance(objetivo, puntoDeEnfoque);
            float t = 1f;
            if(distanciaDeCamaraYObjetivo > 0.01f && centroEnfocando > 0f)
            {
                t = Mathf.Pow(1f - centroEnfocando, Time.unscaledDeltaTime); 
            }
            if(distanciaDeCamaraYObjetivo > radioDeEnfoque)
            {
                //puntoDeEnfoque = Vector3.Lerp(objetivo, puntoDeEnfoque, radioDeEnfoque / distanciaDeCamaraYObjetivo);
                t = Mathf.Min(t, radioDeEnfoque / distanciaDeCamaraYObjetivo);
            }
            puntoDeEnfoque = Vector3.Lerp(objetivo, puntoDeEnfoque, t);
        }
        else
        {
            puntoDeEnfoque = objetivo;
        }
    }

    bool RotacionManual()
    {
        Vector2 camaraInput = new Vector2(Input.GetAxis("Vertical Camera"), Input.GetAxis("Horizontal Camera"));
        const float e = 0.001f;
        if(camaraInput.x < -e || camaraInput.x > e || camaraInput.y < -e || camaraInput.y > e)
        {
            angulosDeOrbita += velocidadDeRotacion * Time.unscaledDeltaTime * camaraInput;
            ultimaRotacionManualTiempo = Time.unscaledTime;
            return true;
        }
        return false;
    }

    bool RotacionAutomatica ()
    {
        if(Time.unscaledTime - ultimaRotacionManualTiempo < retrasoDeAlineacion)
        {
            return false;
        }
        Vector2 movimiento = new Vector2(puntoDeEnfoque.x - puntoDeEnfoquePrevio.x, puntoDeEnfoque.z - puntoDeEnfoquePrevio.z);
        float movimientoDeltaSqr = movimiento.sqrMagnitude;
        if (movimientoDeltaSqr < 0.0001f)
        {
            return false;
        }

        float anguloAlQueSeDirige = ObtenerAngulo(movimiento / Mathf.Sqrt(movimientoDeltaSqr));
        angulosDeOrbita.y = anguloAlQueSeDirige;
        return true;
    }

    static float ObtenerAngulo (Vector2 direccion)
    {
        float angulo = Mathf.Acos(direccion.y) * Mathf.Rad2Deg;
        return direccion.x < 0f ? 360f - angulo: angulo ;
    }

    void RestringirAngulos()
    {
        angulosDeOrbita.x = Mathf.Clamp(angulosDeOrbita.x, anguloVerticalMin, anguloVerticalMax);

        if(angulosDeOrbita.y < 0f)
        {
            angulosDeOrbita.y += 360f;
        }
        else if(angulosDeOrbita.y >= 360f)
        {
            angulosDeOrbita.y -= 360;
        }
    }
}
