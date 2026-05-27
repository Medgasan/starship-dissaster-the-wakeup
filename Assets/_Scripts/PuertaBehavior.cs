using Assets._Scripts.GenericScritps;
using Assets._Scripts.Interfaces; // Asegura que lee la interfaz IInteractable
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

    [Header("Configuración de Seguridad")]
    [Tooltip("Si está marcada, el jugador no podrá abrirla pulsando la 'E' de frente. Tendrá que usar la consola de hackeo.")]
    public bool bloqueadaPorConsola = false;

    [Tooltip("Si está marcada, la puerta manual pedirá un objeto para abrirse.")]
    public bool requiereObjeto = false;
    [Tooltip("El nombre exacto de la tarjeta/llave (ej: 'TarjetaRoja')")]
    public string nombreObjetoRequerido = "";

    public void Start()
    {
        if (cierreAutomatico)
        {
            gameTimer = GetComponent<GameTimer>();
            gameTimer.OneShot = true;
            gameTimer.onTimeout.AddListener(() => AlternarPuerta());

            doorStatus.Opened.AddListener(() => DoorStatusIsOpened());
            doorStatus.Closed.AddListener(() => DoorStatusIsClosed());
        }
    }

    // INTERACCIÓN DIRECTA (Pulsar la 'E' de frente a la puerta)
    public void Interact(object parametro = null)
    {
        // 1. Filtro de la Consola: Si está sellada por software, el jugador no puede hacer nada a mano
        if (bloqueadaPorConsola)
        {
            Debug.LogWarning("Esta puerta está sellada electrónicamente. Requiere hackeo.");
            return;
        }

        // 2. Filtro de la Tarjeta/Objeto: Comprobamos el inventario si la abrimos de forma manual
        if (requiereObjeto && abrir == false) // Solo comprobamos al intentar abrir
        {
            // Convertimos el parámetro genérico en el script de interacción del jugador
            Assets._Scripts.CanInteract inventarioJugador = parametro as Assets._Scripts.CanInteract;

            if (inventarioJugador == null || !inventarioJugador.objetosRecogidos.Contains(nombreObjetoRequerido))
            {
                Debug.LogWarning("Acceso denegado. Se requiere: " + nombreObjetoRequerido);
                return; // Corta la ejecución aquí, impidiendo que se abra
            }
        }

        // 3. Si pasa los controles anteriores, la puerta se acciona con normalidad
        AlternarPuerta();
    }

    // FUNCIÓN PÚBLICA (La que llama la consola de hackeo de forma segura, saltándose los candados)
    public void AlternarPuerta()
    {
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