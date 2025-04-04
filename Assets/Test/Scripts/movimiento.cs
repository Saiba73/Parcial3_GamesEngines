using System;
using Unity.VisualScripting;
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

    [SerializeField, Range(0f, 90f)]
    float anguloMaximoDePiso = 25f;

    [SerializeField, Range(0f, 100f)]
    float velocidadMaximaDeApego = 100f;

    [SerializeField, Min(0f)]
    float distanciaDeRayo = 1f;

    int faseDeSalto;

    bool saltoDeseado;

    int contactoPisoContador;

    int pasosDesdeQueTocoPiso, pasosDesdeUltimoSalto;

    bool TocaPiso => contactoPisoContador > 0;

    Vector3 velocidad, velocidadDeseada, normalDeContacto;

    Rigidbody cuerpoRigido;



    /*private enum estadosJugador
    {
        idle,
        moviendo,
        salto
    }*/

    float productoScalarPisoMin;

    void OnValidate()
    {
        productoScalarPisoMin = Mathf.Cos(anguloMaximoDePiso * Mathf.Deg2Rad);
    }

    void Awake()
    {
        cuerpoRigido = GetComponent<Rigidbody>();
        OnValidate();
    }


    void Update()
    {
        saltoDeseado |= Input.GetButtonDown("Jump");
        Vector2 jugadorInput;
        jugadorInput.x = Input.GetAxis("Horizontal");
        jugadorInput.y = Input.GetAxis("Vertical");
        jugadorInput = Vector2.ClampMagnitude(jugadorInput, 1f);

        velocidadDeseada = new Vector3(jugadorInput.x, 0f, jugadorInput.y) * velocidadMaxima;

        GetComponent<Renderer>().material.SetColor(
            "_BaseColor", TocaPiso ? Color.black : Color.white
        );

    }

    //En FixedUpdate se llama al comiezo de cada paso de una simulacion de fisicas
    void FixedUpdate()
    {
        actualizarEstado();
        ajustarVelocidad();
        //float aceleracion = tocaPiso ? aceleracionMaxima : aceleracionMaximaAerea;
        //float cambioMaximoDeVelocidad = aceleracion * Time.deltaTime;

        //velocidad.x = Mathf.MoveTowards(velocidad.x, velocidadDeseada.x, cambioMaximoDeVelocidad);
        //velocidad.z = Mathf.MoveTowards(velocidad.z, velocidadDeseada.z, cambioMaximoDeVelocidad);

        if(saltoDeseado)
        {
            saltoDeseado = false;
            saltar();
        }
        
        cuerpoRigido.linearVelocity = velocidad;
        reinciarEstadoPiso();
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
            //tocaPiso |= normal.y > productoScalarPisoMin;
            if(normal.y >= productoScalarPisoMin)
            {
                contactoPisoContador += 1;
                normalDeContacto += normal;
            }
        }
    }

    void saltar()
    {
        if(TocaPiso || faseDeSalto < cantidadDeSaltosAereos)
        {
            pasosDesdeUltimoSalto++;
            faseDeSalto++;
            float velocidadDeSalto = Mathf.Sqrt(-2f * Physics.gravity.y * alturaDeSalto);
            float velocidadAliniadaAInclinacion = Vector3.Dot(velocidad, normalDeContacto);
            if(velocidadAliniadaAInclinacion > 0)
            {
                velocidadDeSalto = Mathf.Max(velocidadDeSalto - velocidadAliniadaAInclinacion, 0f);
            }
            velocidad += normalDeContacto * velocidadDeSalto;
        }
    }

    void actualizarEstado ()
    {
        pasosDesdeQueTocoPiso += 1;
        pasosDesdeUltimoSalto += 1;
        velocidad = cuerpoRigido.linearVelocity;
        if(TocaPiso || apegarseAlPiso())
        {
            pasosDesdeQueTocoPiso = 0;
            faseDeSalto = 0;
            if (contactoPisoContador > 1)
            {
                normalDeContacto.Normalize();
            }
        }
        else
        {
            normalDeContacto = Vector3.up;
        }
    }

    bool apegarseAlPiso ()
    {
        if (pasosDesdeQueTocoPiso > 1 || pasosDesdeUltimoSalto <= 2)
        {
            return false;
        }
        float rapidez = velocidad.magnitude;
        if(rapidez > velocidadMaximaDeApego)
        {
            return false;
        }
        if (!Physics.Raycast(cuerpoRigido.position, Vector3.down, out RaycastHit hit, distanciaDeRayo))
        {
            return false;
        }
        if (hit.normal.y < productoScalarPisoMin)
        {
            return false;
        }

        contactoPisoContador = 1;
        normalDeContacto = hit.normal;
        float escalar = Vector3.Dot(velocidad, hit.normal);
        if(escalar > 0f)
        {
            velocidad = (velocidad - hit.normal * escalar).normalized * rapidez;
        }
        return true;
    }

    void ajustarVelocidad()
    {
        Vector3 ejeX = projeccionEnPlanoDeContacto(Vector3.right).normalized;
        Vector3 ejeZ = projeccionEnPlanoDeContacto(Vector3.forward).normalized;

        float xActual = Vector3.Dot(velocidad, ejeX);
        float zActual = Vector3.Dot(velocidad,ejeZ);

        float aceleracion = TocaPiso ? aceleracionMaxima : aceleracionMaximaAerea;
        float cambioMaximoDeVelocidad = aceleracion * Time.deltaTime;

        float nuevaX = Mathf.MoveTowards(xActual, velocidadDeseada.x, cambioMaximoDeVelocidad);
        float nuevaZ = Mathf.MoveTowards(zActual, velocidadDeseada.z, cambioMaximoDeVelocidad);

        velocidad += ejeX * (nuevaX - xActual) + ejeZ * (nuevaZ - zActual);
    }

    void reinciarEstadoPiso()
    {
        contactoPisoContador = 0;
        normalDeContacto = Vector3.zero;
    }

    Vector3 projeccionEnPlanoDeContacto (Vector3 vector)
    {
        return vector - normalDeContacto * Vector3.Dot(vector, normalDeContacto);
    }
}
