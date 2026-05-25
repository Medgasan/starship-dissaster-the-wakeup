using UnityEngine;

public class ControlLinterna : MonoBehaviour
{
    [Header("Configuración de la Luz")]
    [Tooltip("Arrastra aquí el objeto de la luz (Spotlight)")]
    public Light luzLinterna;

    [Tooltip("Tecla para encender/apagar la linterna")]
    public KeyCode teclaLinterna = KeyCode.F;

    [Header("Efectos de Sonido (Opcional)")]
    public AudioSource sonidoClick;

    void Start()
    {
        // Si se nos olvida arrastrar la luz en el Inspector, el script intenta buscarla automáticamente
        if (luzLinterna == null)
        {
            luzLinterna = GetComponent<Light>();
        }
    }

    void Update()
    {
        // Comprobamos si el jugador ha pulsado la tecla asignada
        if (Input.GetKeyDown(teclaLinterna))
        {
            if (luzLinterna != null)
            {
                // Invertimos el estado de la luz: si está encendida (true) pasa a apagada (false), y viceversa.
                luzLinterna.enabled = !luzLinterna.enabled;

                // Si hemos añadido un sonido de 'clic', lo reproducimos
                if (sonidoClick != null)
                {
                    sonidoClick.Play();
                }
            }
        }
    }
}