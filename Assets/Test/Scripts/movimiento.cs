using UnityEngine;
using UnityEngine.InputSystem;

public class movimiento : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    float velocidadMaxima = 10f;

    [SerializeField, Range(0f, 100f)]
    float aceleracionMaxima = 10f;

    //[SerializeField, Range(0f, 1f)]
    //float rebote = 0.5f;

    Vector3 velocidad, velocidadDeseada;

    Rigidbody cuerpoRigido;

    //[SerializeField]
    //Rect areaPermitida = new Rect(-5f, -5f, 10f, 10f);

    void Awake()
    {
        cuerpoRigido = GetComponent<Rigidbody>();
    }


    void Update()
    {
        Vector2 jugadorInput;
        jugadorInput.x = Input.GetAxis("Horizontal");
        jugadorInput.y = Input.GetAxis("Vertical");
        jugadorInput = Vector2.ClampMagnitude(jugadorInput, 1f);

        velocidadDeseada = new Vector3(jugadorInput.x, 0f, jugadorInput.y) * velocidadMaxima;
        
    }

    //En FixedUpdate se llama al comiezo de cada paso de una simulacion de fisicas
    void FixedUpdate()
    {
        velocidad = cuerpoRigido.linearVelocity;
        float cambioMaximoDeVelocidad = aceleracionMaxima * Time.deltaTime;

        velocidad.x = Mathf.MoveTowards(velocidad.x, velocidadDeseada.x, cambioMaximoDeVelocidad);
        velocidad.z = Mathf.MoveTowards(velocidad.z, velocidadDeseada.z, cambioMaximoDeVelocidad);

        
        cuerpoRigido.linearVelocity = velocidad;
    }
}
