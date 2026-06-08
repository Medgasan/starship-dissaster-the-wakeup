using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Assets._Scripts;

public class DialogoCientificoEleccion : MonoBehaviour
{
    [Header("Componentes")]
    public TextMeshPro textoFlotante;
    [Tooltip("Panel de UI que dice: '1. Dar Antídoto | 2. Negarlo'")]
    public GameObject panelEleccionUI;
    public AudioSource altavozVoz;
    public AudioClip sonidoVoz;

    [Header("Estado del NPC")]
    public bool estaInfectado = true;

    [Header("Sistema de Inventario (Intercambio)")]
    public string objetoNecesario = "Antidoto";
    public string objetoRecompensa = "TarjetaRoja";

    [Header("1. Diálogos Iniciales")]
    [TextArea(2, 4)] public string[] frasesNormales;
    [TextArea(2, 4)] public string[] frasesConAntidoto;

    [Header("2. Diálogos Post-Decisión")]
    [TextArea(2, 4)] public string[] frasesSiLoCuras;
    [TextArea(2, 4)] public string[] frasesSiLoNiegas;

    [Header("Configuración General")]
    public KeyCode teclaInteraccion = KeyCode.E;
    public float velocidadTexto = 0.05f;
    public int frasesRepetiblesAlFinal = 1;

    [Header("Eventos")]
    public UnityEvent alBloquearControles;
    public UnityEvent alDesbloquearControles;
    public UnityEvent alCurarActualizarFinal;

    private int fraseActual = 0;
    private bool jugadorCerca = false;
    private Coroutine rutinaEscritura;
    private CanInteract inventarioDelJugador;

    private bool leyendoListaAntidoto = false;

    // Control de la decisión
    private bool esperandoEleccion = false;
    private bool decisionTomada = false;
    private bool seLlevoElAntidoto = false;

    // Escudo contra el Doble Trigger
    private bool yaEstamosHablando = false;

    void Start()
    {
        textoFlotante.gameObject.SetActive(false);
        if (panelEleccionUI != null) panelEleccionUI.SetActive(false);
    }

    void Update()
    {
        if (!jugadorCerca) return;

        if (esperandoEleccion)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ProcesarDecision(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ProcesarDecision(false);
            }
            return;
        }

        if (Input.GetKeyDown(teclaInteraccion))
        {
            if (!textoFlotante.gameObject.activeSelf)
            {
                textoFlotante.gameObject.SetActive(true);
            }

            bool tieneAntidoto = inventarioDelJugador != null && inventarioDelJugador.objetosRecogidos.Contains(objetoNecesario);
            AvanzarDialogo(tieneAntidoto);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ESCUDO ANTI-BUGS: Si ya entró un collider del jugador, ignoramos el resto
            if (yaEstamosHablando) return;
            yaEstamosHablando = true;

            jugadorCerca = true;
            inventarioDelJugador = other.GetComponentInChildren<CanInteract>();

            bool tieneAntidoto = inventarioDelJugador != null && inventarioDelJugador.objetosRecogidos.Contains(objetoNecesario);

            if (!decisionTomada && tieneAntidoto != leyendoListaAntidoto)
            {
                leyendoListaAntidoto = tieneAntidoto;
                fraseActual = 0;
            }

            if (fraseActual == 0)
            {
                textoFlotante.gameObject.SetActive(true);
                string[] listaActual = ObtenerListaCorrecta();

                if (listaActual.Length > 0)
                {
                    MostrarFrase(listaActual[fraseActual]);
                    fraseActual++;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            yaEstamosHablando = false; // Reiniciamos el escudo al salir
            jugadorCerca = false;
            inventarioDelJugador = null;
            textoFlotante.gameObject.SetActive(false);
            if (panelEleccionUI != null) panelEleccionUI.SetActive(false);

            if (rutinaEscritura != null) StopCoroutine(rutinaEscritura);
        }
    }

    void AvanzarDialogo(bool tieneAntidoto)
    {
        if (!decisionTomada && tieneAntidoto != leyendoListaAntidoto)
        {
            leyendoListaAntidoto = tieneAntidoto;
            fraseActual = 0;
        }

        string[] listaActual = ObtenerListaCorrecta();

        if (fraseActual >= listaActual.Length)
        {
            if (!decisionTomada && leyendoListaAntidoto)
            {
                IniciarEleccion();
                return;
            }

            fraseActual = Mathf.Max(0, listaActual.Length - frasesRepetiblesAlFinal);
            MostrarFrase(listaActual[fraseActual]);
            fraseActual++;
        }
        else
        {
            MostrarFrase(listaActual[fraseActual]);
            fraseActual++;
        }
    }

    string[] ObtenerListaCorrecta()
    {
        if (decisionTomada)
        {
            return seLlevoElAntidoto ? frasesSiLoCuras : frasesSiLoNiegas;
        }

        return leyendoListaAntidoto ? frasesConAntidoto : frasesNormales;
    }

    void IniciarEleccion()
    {
        esperandoEleccion = true;

        if (panelEleccionUI != null) panelEleccionUI.SetActive(true);
        alBloquearControles?.Invoke();
    }

    void ProcesarDecision(bool darAntidoto)
    {
        esperandoEleccion = false;
        decisionTomada = true;
        seLlevoElAntidoto = darAntidoto;
        fraseActual = 0;

        if (panelEleccionUI != null) panelEleccionUI.SetActive(false);
        alDesbloquearControles?.Invoke();

        if (darAntidoto)
        {
            inventarioDelJugador.objetosRecogidos.Remove(objetoNecesario);
            if (!string.IsNullOrEmpty(objetoRecompensa) && !inventarioDelJugador.objetosRecogidos.Contains(objetoRecompensa))
            {
                inventarioDelJugador.objetosRecogidos.Add(objetoRecompensa);
            }
            estaInfectado = false;
            alCurarActualizarFinal?.Invoke();
        }

        AvanzarDialogo(inventarioDelJugador.objetosRecogidos.Contains(objetoNecesario));
    }

    void MostrarFrase(string frase)
    {
        if (rutinaEscritura != null) StopCoroutine(rutinaEscritura);
        rutinaEscritura = StartCoroutine(EscribirTextoLetraALetra(frase));
    }

    IEnumerator EscribirTextoLetraALetra(string frase)
    {
        // 1. Ponemos la frase entera de golpe, pero la hacemos invisible
        textoFlotante.text = frase;
        textoFlotante.maxVisibleCharacters = 0; // REINICIO: Esto arregla el bug de los 9 caracteres

        // Forzamos a Unity a procesar la longitud real de la frase
        textoFlotante.ForceMeshUpdate();
        int totalLetrasVisibles = textoFlotante.textInfo.characterCount;

        float tonoOriginal = altavozVoz != null ? altavozVoz.pitch : 1f;

        // 2. Vamos revelando las letras una a una mágicamente
        for (int i = 0; i <= totalLetrasVisibles; i++)
        {
            textoFlotante.maxVisibleCharacters = i;

            // Sonido intermitente (en letras pares)
            if (i > 0 && i % 2 == 0 && altavozVoz != null && sonidoVoz != null)
            {
                altavozVoz.pitch = Random.Range(0.8f, 1.2f);
                altavozVoz.PlayOneShot(sonidoVoz);
            }

            yield return new WaitForSeconds(velocidadTexto);
        }

        // 3. Restauramos la voz al terminar
        if (altavozVoz != null) altavozVoz.pitch = tonoOriginal;

        // Seguro extra por si acaso: al terminar, que se vea todo
        textoFlotante.maxVisibleCharacters = 99999;
    }
}