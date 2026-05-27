using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class PuertaFinalVictoria : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("El nombre exacto de la escena de victoria en tu carpeta de Assets")]
    public string nombreEscenaVictoria = "Victoria";

    public DoorStatus doorStatus;

    void Start()
    {
        doorStatus.Opened.AddListener(GanarJuego);
    }

    void GanarJuego()
    {
        Debug.Log("¡Puerta final abierta por completo! Cargando pantalla de victoria...");
        SceneManager.LoadScene(nombreEscenaVictoria);
    }
}