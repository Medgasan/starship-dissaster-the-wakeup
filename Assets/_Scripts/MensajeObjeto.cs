using UnityEngine;

public class MensajeObjeto : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject mensajeUI;
    public float distanciaMaxima = 3.5f;

    private Camera camaraPrincipal;
    private bool estaInteractuando = false;

    void Start()
    {
        camaraPrincipal = Camera.main;
        if (mensajeUI != null) mensajeUI.SetActive(false);
    }

    void Update()
    {
        // 1. Si el jugador ya ha pulsado la E y está en el minijuego/abriendo, forzamos el apagado
        if (estaInteractuando)
        {
            if (mensajeUI != null) mensajeUI.SetActive(false);
            return;
        }

        // 2. Trazamos el rayo para saber si el jugador nos está mirando
        Ray rayo = new Ray(camaraPrincipal.transform.position, camaraPrincipal.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(rayo, out hit, distanciaMaxima))
        {
            if (hit.collider.gameObject == gameObject)
            {
                // ¡Nos están mirando! Encendemos el texto
                if (mensajeUI != null) mensajeUI.SetActive(true);

                // 3. DETECTAR INTERACCIÓN: Si nos miran y pulsan la 'E'
                if (Input.GetKeyDown(KeyCode.E))
                {
                    estaInteractuando = true; // Bloqueamos el script
                    if (mensajeUI != null) mensajeUI.SetActive(false); // Apagamos el cartel de inmediato
                }

                return;
            }
        }

        // Si dejamos de mirarlo, nos aseguramos de limpiar el estado
        if (mensajeUI != null) mensajeUI.SetActive(false);
    }

    // Esta función sirve para volver a "activar" el cartel si el jugador sale del minijuego sin resolverlo (opcional)
    public void ResetearInteraccion()
    {
        estaInteractuando = false;
    }

    private void OnDestroy()
    {
        if (mensajeUI != null) mensajeUI.SetActive(false);
    }
}