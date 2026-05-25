using UnityEngine;

public class PuertaAcceso : MonoBehaviour
{
    [Header("--- ESTADO DE LA PUERTA ---")]
    public bool requiereTarjeta = true;
    public bool estaAbierta = false;

    [Header("--- ANIMACIÓN (POR CÓDIGO) ---")]
    public float anguloApertura = 90f;
    public float velocidadApertura = 3f;

    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    void Start()
    {
        // Guardamos las rotaciones inicial y final
        rotacionCerrada = transform.rotation;
        rotacionAbierta = Quaternion.Euler(transform.eulerAngles + new Vector3(0, anguloApertura, 0));
    }

    void Update()
    {
        // Si la puerta debe estar abierta, la rotamos suavemente con Slerp
        if (estaAbierta)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionAbierta, Time.deltaTime * velocidadApertura);
        }
    }

    // El jugador llamará a esta función al pulsar la 'E'
    public void IntentarAbrir(bool jugadorTieneTarjeta)
    {
        if (estaAbierta) return; // Si ya está abierta, no hacemos nada

        if (!requiereTarjeta || jugadorTieneTarjeta)
        {
            Debug.Log("Acceso concedido. Abriendo esclusa...");
            estaAbierta = true;
            // TODO: Reproducir sonido mecánico de éxito
        }
        else
        {
            Debug.Log("Acceso denegado. Se requiere Tarjeta de Acceso.");
            // TODO: Reproducir sonido de error (pip-pip)
        }
    }
}