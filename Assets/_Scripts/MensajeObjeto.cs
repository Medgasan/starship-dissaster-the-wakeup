using UnityEngine;

public class MensajeObjeto : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject mensajeUI;
    public float distanciaMaxima = 3.5f;

    private Camera camaraPrincipal;

    void Start()
    {
        // El script busca automáticamente la cámara del jugador al empezar
        camaraPrincipal = Camera.main;

        if (mensajeUI != null) mensajeUI.SetActive(false);
    }

    void Update()
    {
        // 1. Trazamos una línea matemática desde la cámara hacia el frente
        Ray rayo = new Ray(camaraPrincipal.transform.position, camaraPrincipal.transform.forward);
        RaycastHit hit;

        // 2. Comprobamos si esa línea choca con algo físico
        if (Physics.Raycast(rayo, out hit, distanciaMaxima))
        {
            // 3. ¿El objeto contra el que ha chocado soy YO (este mismo objeto)?
            if (hit.collider.gameObject == gameObject)
            {
                // ¡Me están mirando! Enciendo el texto
                if (mensajeUI != null) mensajeUI.SetActive(true);
                return; // Cortamos aquí para que no llegue al código de apagar
            }
        }

        // Si la línea no choca con nada, o choca con la pared/otro objeto, me apago
        if (mensajeUI != null) mensajeUI.SetActive(false);
    }

    // Mantenemos esto para evitar el bug fantasma al destruir llaves
    private void OnDestroy()
    {
        if (mensajeUI != null) mensajeUI.SetActive(false);
    }
}