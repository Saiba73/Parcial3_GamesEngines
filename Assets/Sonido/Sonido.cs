using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sonido : MonoBehaviour
{
    
    bool audioActivo;

    public AudioSource personajeAudio;
    public AudioSource audioSecundario;
    public AudioClip Salto;
    public AudioClip Giro;
    public AudioClip Activador;
    public AudioClip Recojer;
    public AudioClip DestruyeEnemigo;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            personajeAudio.clip = Giro;
            personajeAudio.Play();
        }
    }
    public void AuidoOn()
    {
        
    }
}
