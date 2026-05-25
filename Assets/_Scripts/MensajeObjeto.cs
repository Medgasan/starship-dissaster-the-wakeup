using UnityEngine;

public class MensajeObjeto : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject mensajeUI;      // Arrastra aquí el texto "Pulse E..." del Canvas
    public Transform jugador;         // Arrastra aquí a tu personaje para medir la distancia
    public float distanciaMaxima = 3.5f; // Distancia para que aparezca el mensaje

    // Esta función la llama Unity automáticamente MIENTRAS estés mirando el objeto
    private void OnMouseOver()
    {
        // Calculamos a qué distancia está el jugador de la puerta
        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= distanciaMaxima)
        {
            // Si lo miras y estás cerca, enciende el texto
            if (mensajeUI != null) mensajeUI.SetActive(true);
        }
        else
        {
            // Si lo miras pero estás muy lejos, lo apaga
            if (mensajeUI != null) mensajeUI.SetActive(false);
        }
    }

    // Esta función la llama Unity en el milisegundo en que DEJAS de mirar el objeto
    private void OnMouseExit()
    {
        if (mensajeUI != null) mensajeUI.SetActive(false);
    }
}