using Assets._Scripts.GenericScritps;
using Assets._Scripts.Interfaces;
using UnityEngine;

public class PuertaBehavior : MonoBehaviour, IInteractable
{
    [Header("Mecanismo")]
    public bool abrir = false;
    public Animator doorMechanism;
    public DoorStatus doorStatus;

    [Header("Temporizador")]
    public bool cierreAutomatico = false;
    private GameTimer gameTimer;

    [Header("Sistema de Bloqueo")]
    public bool requiereObjeto = false;
    [Tooltip("El nombre exacto del objeto que necesita el jugador (ej: 'TarjetaRoja')")]
    public string nombreObjetoRequerido = "";

    public void Start()
    {
        if (cierreAutomatico)
        {
            gameTimer = GetComponent<GameTimer>();
            gameTimer.OneShot = true;
            gameTimer.onTimeout.AddListener(() => Interact());

            doorStatus.Opened.AddListener(() => DoorStatusIsOpened());
            doorStatus.Closed.AddListener(() => DoorStatusIsClosed());
        }
    }

    public void Interact(object parametro = null)
    {
        // 1. COMPROBAR CERRADURA: Si la puerta tiene el bloqueo activado
        if (requiereObjeto && abrir == false) // Solo comprobamos si intentamos abrirla
        {
            // Transformamos el parámetro genérico en el script del jugador
            Assets._Scripts.CanInteract inventarioJugador = parametro as Assets._Scripts.CanInteract;

            // Si el jugador no existe o su lista NO contiene el objeto requerido...
            if (inventarioJugador == null || !inventarioJugador.objetosRecogidos.Contains(nombreObjetoRequerido))
            {
                Debug.LogWarning("Acceso denegado. Necesitas: " + nombreObjetoRequerido);
                // Aquí podrías lanzar un sonido de error
                return; // Cortamos la función aquí. La puerta no se abre.
            }
        }

        // 2. ABRIR/CERRAR LA PUERTA: Si llegamos aquí, tenemos permiso
        if (doorMechanism.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) return;
        abrir = !abrir;
        doorMechanism.SetBool("Abrir", abrir);
    }

    private void DoorStatusIsOpened()
    {
        Debug.Log("DoorStatusIsOpened event");
        if (cierreAutomatico) { gameTimer.StartTimer(); }
    }

    private void DoorStatusIsClosed()
    {
        Debug.Log("DoorStatusIsClosed event");
        if (cierreAutomatico) { gameTimer.Stop(); }
    }
}