using Assets._Scripts; // Permite acceder a la mochila del jugador (CanInteract)
using Assets._Scripts.Interfaces;
using UnityEngine;

public class ConsolaHackeo : MonoBehaviour, IInteractable
{
    [Header("Conexiones")]
    public GameObject canvasMinijuego;
    public PuertaBehavior puertaConectada;

    [Header("Control del Jugador")]
    public MonoBehaviour scriptMovimientoJugador;
    public MonoBehaviour scriptCamaraJugador;

    [Header("SISTEMA DE SEGURIDAD")]
    public bool requiereObjeto = false;
    [Tooltip("El nombre exacto del objeto necesario para encender esta terminal (ej: 'CableDatos')")]
    public string nombreObjetoRequerido = "";

    [Header("DISEÑO DEL LABERINTO")]
    [Tooltip("Coordenada X,Y dentro de la matriz donde aparecerá el jugador (el 0)")]
    public Vector2 coordenadaInicio = new Vector2(1, 1);

    [Tooltip("Dibuja aquí el laberinto: 0 (Libre), 1 (Muro), 2 (Meta X)")]
    public string[] diseñoMapa = new string[]
    {
        "1111111",
        "1001021",
        "1101011",
        "1000001",
        "1011101",
        "1000101",
        "1111111"
    };

    public void Interact(object parametro = null)
    {
        // 1. COMPROBAR SEGURIDAD: Si la consola pide un objeto clave para encenderse
        if (requiereObjeto)
        {
            CanInteract inventarioJugador = parametro as CanInteract;

            if (inventarioJugador == null || !inventarioJugador.objetosRecogidos.Contains(nombreObjetoRequerido))
            {
                Debug.LogWarning("Terminal inactiva. Se necesita el dispositivo: " + nombreObjetoRequerido);
                return; // Corta la función. El minijuego no se inicia.
            }
        }

        // 2. Si las condiciones se cumplen, arranca la terminal
        EmpezarHackeo();
    }

    private void EmpezarHackeo()
    {
        // Buscamos el componente del juego de texto en el Canvas y le cargamos los datos de ESTA consola
        MinijuegoHackeoTerminal terminal = canvasMinijuego.GetComponent<MinijuegoHackeoTerminal>();
        if (terminal != null)
        {
            terminal.CargarNuevoNivel(diseñoMapa, coordenadaInicio, this);
        }

        // Activamos la interfaz visual y pausamos al jugador
        canvasMinijuego.SetActive(true);
        if (scriptMovimientoJugador != null) scriptMovimientoJugador.enabled = false;
        if (scriptCamaraJugador != null) scriptCamaraJugador.enabled = false;

        // Liberamos el puntero del ratón para que el foco no se vuelva loco
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Esta función se ejecuta desde el script del Canvas en cuanto el jugador pisa la 'X'
    public void HackeoCompletado()
    {
        // Ocultamos el minijuego de la pantalla
        canvasMinijuego.SetActive(false);

        // Devolvemos el control y la cámara al jugador
        if (scriptMovimientoJugador != null) scriptMovimientoJugador.enabled = true;
        if (scriptCamaraJugador != null) scriptCamaraJugador.enabled = true;

        // Escondemos y bloqueamos el cursor del ratón en el centro de nuevo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ordenamos la apertura de la compuerta asociada saltándonos los bloqueos manuales
        if (puertaConectada != null)
        {
            puertaConectada.AlternarPuerta();
        }
    }
}