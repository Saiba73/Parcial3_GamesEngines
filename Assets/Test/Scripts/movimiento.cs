using UnityEngine;
using UnityEngine.InputSystem;

public class movimiento : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    float velocidadMaxima = 10f;

    [SerializeField, Range(0f, 100f)]
    float aceleracionMaxima = 10f, aceleracionMaximaAerea = 1f;

    [SerializeField, Range(0f, 10f)]
    float alturaDeSalto = 2f;

    [SerializeField, Range(0, 5)]
    int cantidadDeSaltosAereos;

    int faseDeSalto;

    bool saltoDeseado;

    bool tocaPiso;

    Vector3 velocidad, velocidadDeseada;

    Rigidbody cuerpoRigido;

    

    /*private enum estadosJugador
    {
        idle,
        moviendo,
        salto
    }*/

    void Awake()
    {
        cuerpoRigido = GetComponent<Rigidbody>();
    }


    void Update()
    {
        saltoDeseado |= Input.GetButtonDown("Jump");
        Vector2 jugadorInput;
        jugadorInput.x = Input.GetAxis("Horizontal");
        jugadorInput.y = Input.GetAxis("Vertical");
        jugadorInput = Vector2.ClampMagnitude(jugadorInput, 1f);

        velocidadDeseada = new Vector3(jugadorInput.x, 0f, jugadorInput.y) * velocidadMaxima;
        
    }

    //En FixedUpdate se llama al comiezo de cada paso de una simulacion de fisicas
    void FixedUpdate()
    {
        actualizarEstado();
        float aceleracion = tocaPiso ? aceleracionMaxima : aceleracionMaximaAerea;
        float cambioMaximoDeVelocidad = aceleracion * Time.deltaTime;

        velocidad.x = Mathf.MoveTowards(velocidad.x, velocidadDeseada.x, cambioMaximoDeVelocidad);
        velocidad.z = Mathf.MoveTowards(velocidad.z, velocidadDeseada.z, cambioMaximoDeVelocidad);

        if(saltoDeseado)
        {
            saltoDeseado = false;
            saltar();
        }
        
        cuerpoRigido.linearVelocity = velocidad;
        tocaPiso = false;
    }

    void OnCollisionEnter(Collision colision)
    {
        EvaluarColision(colision);
    }

    private void OnCollisionStay(Collision colision)
    {
        EvaluarColision(colision);
    }

    void EvaluarColision(Collision colision)
    {
        for(int i = 0; i < colision.contactCount; i++)
        {
            Vector3 normal = colision.GetContact(i).normal;
            tocaPiso |= normal.y > 0.9f;
        }
    }

    void saltar()
    {
        if(tocaPiso || faseDeSalto < cantidadDeSaltosAereos)
        {
            faseDeSalto++;
            float velocidadDeSalto = Mathf.Sqrt(-2f * Physics.gravity.y * alturaDeSalto);
            if(velocidad.y > 0)
            {
                velocidadDeSalto = Mathf.Max(velocidadDeSalto - velocidad.y, 0f);
            }
            velocidad.y += velocidadDeSalto;
        }
    }

    void actualizarEstado ()
    {
        velocidad = cuerpoRigido.linearVelocity;
        if(tocaPiso)
        {
            faseDeSalto = 0;
        }
    }
}
