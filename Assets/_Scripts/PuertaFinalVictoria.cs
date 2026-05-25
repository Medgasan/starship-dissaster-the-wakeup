using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class PuertaFinalVictoria : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("El nombre exacto de la escena de victoria en tu carpeta de Assets")]
    public string nombreEscenaVictoria = "Victoria";

    private DoorStatus doorStatus;

    void Start()
    {
        // Buscamos automáticamente el componente DoorStatus que está en esta misma puerta
        doorStatus = GetComponent<DoorStatus>();

        if (doorStatus != null)
        {
            // Nos conectamos al evento "Opened".
            // En cuanto la puerta termine de abrirse, ejecutará automáticamente nuestra función GanarJuego
            doorStatus.Opened.AddListener(GanarJuego);
        }
        else
        {
            Debug.LogError("¡Error! Este script necesita estar en el mismo objeto que el componente DoorStatus.");
        }
    }

    void GanarJuego()
    {
        Debug.Log("¡Puerta final abierta por completo! Cargando pantalla de victoria...");
        SceneManager.LoadScene(nombreEscenaVictoria);
    }
}