using Assets._Scripts; // Añadimos esto para poder leer el inventario del jugador
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
    public Vector2 coordenadaInicio = new Vector2(1, 1);

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
        // 1. COMPROBAR SEGURIDAD: Si la consola tiene el bloqueo activado
        if (requiereObjeto)
        {
            // Transformamos el parámetro genérico en el script del jugador para leer su mochila
            CanInteract inventarioJugador = parametro as CanInteract;

            // Si el jugador no existe o su lista NO contiene el objeto requerido...
            if (inventarioJugador == null || !inventarioJugador.objetosRecogidos.Contains(nombreObjetoRequerido))
            {
                Debug.LogWarning("Terminal bloqueada. Necesitas: " + nombreObjetoRequerido);
                // TODO: Reproducir sonido de error de ordenador (pip-pip)
                return; // Cortamos la función aquí. La pantalla de hackeo no se enciende.
            }
        }

        // 2. Si no requiere objeto, o si el jugador sí lo tiene en el inventario, empezamos
        EmpezarHackeo();
    }

    private void EmpezarHackeo()
    {
        MinijuegoHackeoTerminal terminal = canvasMinijuego.GetComponent<MinijuegoHackeoTerminal>();
        if (terminal != null)
        {
            terminal.CargarNuevoNivel(diseñoMapa, coordenadaInicio, this);
        }

        canvasMinijuego.SetActive(true);
        if (scriptMovimientoJugador != null) scriptMovimientoJugador.enabled = false;
        if (scriptCamaraJugador != null) scriptCamaraJugador.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HackeoCompletado()
    {
        canvasMinijuego.SetActive(false);
        if (scriptMovimientoJugador != null) scriptMovimientoJugador.enabled = true;
        if (scriptCamaraJugador != null) scriptCamaraJugador.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (puertaConectada != null) puertaConectada.Interact();
    }
}