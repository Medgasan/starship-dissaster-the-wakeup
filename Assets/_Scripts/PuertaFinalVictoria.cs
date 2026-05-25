using UnityEngine;
using UnityEngine.SceneManagement; // ¡Línea vital para poder cambiar de escena!

public class ZonaEscape : MonoBehaviour
{
    [Header("Configuración del Destino")]
    [Tooltip("El nombre exacto de tu escena de victoria tal y como está guardada (ej: 'EscenaVictoria')")]
    public string nombreEscenaVictoria = "Victoria";

    // Esta función la ejecuta Unity automáticamente cuando algo físico atraviesa el Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Comprobamos si lo que ha cruzado es el jugador mirando su etiqueta (Tag)
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Nave evacuada con éxito! Cargando victoria...");

            // Le decimos a Unity que cierre el mapa actual y abra la pantalla de victoria
            SceneManager.LoadScene(nombreEscenaVictoria);
        }
    }
}