using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class movimiento : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    float velocidadMaxima = 10f, velocidadDeCorrer = 2f;

    [SerializeField, Range(0f, 100f)]
    float aceleracionMaxima = 10f, aceleracionDeCorrer = 2f, aceleracionMaximaAerea = 1f;

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

    [SerializeField]
    Transform espacioDeInputDeJugador = default;

    int faseDeSalto;

    bool saltoDeseado;

    int contactoPisoContador, contactoEnpinacionContador;

    int pasosDesdeQueTocoPiso, pasosDesdeUltimoSalto;

    bool TocaPiso => contactoPisoContador > 0;

    bool TocaEnpinacion => contactoEnpinacionContador > 0;

    float velocidadMaximaOriginal;
    float aceleracionMaximaOriginal;
    float alturaDeSaltoOriginal;
    //float distanciaDeRayoOriginal;
    int cantidadDeSaltosOriginal;

    Vector3 velocidad, velocidadDeseada, normalDeContacto, normalDeEnpinacion;

    Rigidbody cuerpoRigido;

    float productoScalarPisoMin;


    public bool noPuedeAtaquar;
    void OnValidate()
    {
        productoScalarPisoMin = Mathf.Cos(anguloMaximoDePiso * Mathf.Deg2Rad);
    }

    void Awake()
    {
        cuerpoRigido = GetComponent<Rigidbody>();
        OnValidate();

        // Velocidades originales se guardan para poder crear variaciones en el movimiento.
        velocidadMaximaOriginal = velocidadMaxima;
        aceleracionMaximaOriginal = aceleracionMaxima;
        alturaDeSaltoOriginal = alturaDeSalto;
        //distanciaDeRayoOriginal = distanciaDeRayo;
        cantidadDeSaltosOriginal = cantidadDeSaltosAereos;
    }


    void Update()
    {
        

        if (Input.GetKey(KeyCode.E) && TocaPiso)
        {
            //transform.localScale = new Vector3(1f, 0.5f, 1f);
            noPuedeAtaquar = true;
            velocidadMaxima = 0f;
            aceleracionMaxima = 5f;
            if (alturaDeSalto != 50f)
            {
                alturaDeSalto += 0.01f;
            }
            //distanciaDeRayo = 0.5f;
            //cantidadDeSaltosAereos = 0;
        }
        else if (Input.GetKeyUp(KeyCode.E) || !TocaPiso)
        {
            //transform.localScale = new Vector3(1f, 1f, 1f);
            noPuedeAtaquar = false;
            velocidadMaxima = velocidadMaximaOriginal;
            aceleracionMaxima = aceleracionMaximaOriginal;
            alturaDeSalto = alturaDeSaltoOriginal;
            //distanciaDeRayo = distanciaDeRayoOriginal;
            cantidadDeSaltosAereos = cantidadDeSaltosOriginal;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && velocidadMaxima != velocidadMaxima + velocidadDeCorrer && TocaPiso)
        {
            velocidadMaxima += velocidadDeCorrer;
            aceleracionMaxima += aceleracionDeCorrer;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && velocidadMaxima != velocidadMaxima - velocidadDeCorrer)
        {
            velocidadMaxima -= velocidadDeCorrer;
            aceleracionMaxima -= aceleracionDeCorrer;
        }
        saltoDeseado |= Input.GetButtonDown("Jump");
        Vector2 jugadorInput;
        jugadorInput.x = Input.GetAxis("Horizontal");
        jugadorInput.y = Input.GetAxis("Vertical");
        jugadorInput = Vector2.ClampMagnitude(jugadorInput, 1f);

        if (espacioDeInputDeJugador)
        {
            Vector3 frente = espacioDeInputDeJugador.forward;
            frente.y = 0f;
            frente.Normalize();
            Vector3 derecha = espacioDeInputDeJugador.right;
            derecha.y = 0f;
            derecha.Normalize();
            velocidadDeseada = (frente * jugadorInput.y + derecha * jugadorInput.x) * velocidadMaxima;
        }
        else
        {
            velocidadDeseada = new Vector3(jugadorInput.x, 0f, jugadorInput.y) * velocidadMaxima;
        }
        /*GetComponent<Renderer>().material.SetColor(
            "_BaseColor", TocaPiso ? Color.black : Color.white
        );*/
    }

    //En FixedUpdate se llama al comiezo de cada paso de una simulacion de fisicas
    void FixedUpdate()
    {
        actualizarEstado();
        ajustarVelocidad();
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
            else if(normal.y > -0.01f)
            {
                contactoEnpinacionContador += 1;
                normalDeEnpinacion += normal;
            }
        }
    }

    void saltar()
    {
        Vector3 direccionDeSalto;
        if(TocaPiso)
        {
            direccionDeSalto = normalDeContacto;
        }
        else if(TocaEnpinacion)
        {
            direccionDeSalto = normalDeEnpinacion;
            faseDeSalto = 0;
        }
        else if(cantidadDeSaltosAereos > 0 && faseDeSalto <= cantidadDeSaltosAereos)
        {
            if(faseDeSalto == 0)
            {
                faseDeSalto = 1;
            }
            direccionDeSalto = normalDeContacto;
        }
        else
        {
            return;
        }
        
            pasosDesdeUltimoSalto++;
            faseDeSalto++;
            float velocidadDeSalto = Mathf.Sqrt(-2f * Physics.gravity.y * alturaDeSalto);
            direccionDeSalto = (direccionDeSalto + Vector3.up).normalized;
            float velocidadAliniadaAInclinacion = Vector3.Dot(velocidad, direccionDeSalto);
            if(velocidadAliniadaAInclinacion > 0)
            {
                velocidadDeSalto = Mathf.Max(velocidadDeSalto - velocidadAliniadaAInclinacion, 0f);
            }
            velocidad += direccionDeSalto * velocidadDeSalto;
        
    }

    void actualizarEstado ()
    {
        pasosDesdeQueTocoPiso += 1;
        pasosDesdeUltimoSalto += 1;
        velocidad = cuerpoRigido.linearVelocity;
        if(TocaPiso || apegarseAlPiso() || revisaContactosEnpinados())
        {
            pasosDesdeQueTocoPiso = 0;
            if(pasosDesdeUltimoSalto > 1)
            {
                faseDeSalto = 0;
            }
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
        contactoPisoContador = contactoEnpinacionContador = 0;
        normalDeContacto = normalDeEnpinacion = Vector3.zero;
    }

    bool revisaContactosEnpinados()
    {
        if(contactoEnpinacionContador > 1)
        {
            normalDeEnpinacion.Normalize();
            if(normalDeEnpinacion.y >= productoScalarPisoMin)
            {
                contactoEnpinacionContador = 1;
                normalDeContacto = normalDeEnpinacion;
                return true;
            }
        }
        return false;
    }

    Vector3 projeccionEnPlanoDeContacto (Vector3 vector)
    {
        return vector - normalDeContacto * Vector3.Dot(vector, normalDeContacto);
    }
}
