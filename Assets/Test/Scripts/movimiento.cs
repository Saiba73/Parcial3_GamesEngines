using UnityEngine;

public class movimiento : MonoBehaviour
{
    [SerializeField, Range(-100f, 100f)]
    float velocidadMaxima = 10f;

    Vector3 velocidad;
    void Start()
    {
        
    }


    void Update()
    {
        Vector2 jugadorInput;
        jugadorInput.x = Input.GetAxis("Horizontal");
        jugadorInput.y = Input.GetAxis("Vertical");
        jugadorInput = Vector2.ClampMagnitude(jugadorInput, 1f);
        Vector3 acceleracion = new Vector3(jugadorInput.x, 0f, jugadorInput.y) * velocidadMaxima;
        velocidad += acceleracion * Time.deltaTime;
        Vector3 desplazamiento = velocidad * Time.deltaTime;
        transform.localPosition += desplazamiento;
    }
}
