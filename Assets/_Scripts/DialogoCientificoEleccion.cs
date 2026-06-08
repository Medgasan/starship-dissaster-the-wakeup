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

    [Header("2. Diálogos Post-Decisión (Este NPC)")]
    [TextArea(2, 4)] public string[] frasesSiLoCuras;
    [TextArea(2, 4)] public string[] frasesSiLoNiegas;

    [Header("3. Consecuencias (El otro NPC)")]
    [Tooltip("Frases si llegas aquí pero ya le diste el antídoto al OTRO científico")]
    [TextArea(2, 4)] public string[] frasesAntidotoGastado;

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

    // 0 = Normal, 1 = Tiene Antídoto, 2 = Ya lo gastó
    private int estadoInventarioAnterior = 0;

    private bool esperandoEleccion = false;
    private bool decisionTomada = false;
    private bool seLlevoElAntidoto = false;
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
            if (Input.GetKeyDown(KeyCode.Alpha1)) ProcesarDecision(true);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) ProcesarDecision(false);
            return;
        }

        if (Input.GetKeyDown(teclaInteraccion))
        {
            if (!textoFlotante.gameObject.activeSelf) textoFlotante.gameObject.SetActive(true);
            AvanzarDialogo();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (yaEstamosHablando) return;
            yaEstamosHablando = true;

            jugadorCerca = true;
            inventarioDelJugador = other.GetComponentInChildren<CanInteract>();

            int estadoActual = ComprobarEstadoInventario();

            // Si el estado del inventario ha cambiado mientras estábamos fuera, reiniciamos la charla
            if (!decisionTomada && estadoActual != estadoInventarioAnterior)
            {
                estadoInventarioAnterior = estadoActual;
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
            yaEstamosHablando = false;
            jugadorCerca = false;
            inventarioDelJugador = null;
            textoFlotante.gameObject.SetActive(false);
            if (panelEleccionUI != null) panelEleccionUI.SetActive(false);

            if (rutinaEscritura != null) StopCoroutine(rutinaEscritura);
        }
    }

    // MAGIA: Comprobamos si tiene el objeto, o si tiene la marca de haberlo gastado
    int ComprobarEstadoInventario()
    {
        if (inventarioDelJugador == null) return 0;
        if (inventarioDelJugador.objetosRecogidos.Contains(objetoNecesario)) return 1;
        if (inventarioDelJugador.objetosRecogidos.Contains(objetoNecesario + "_Usado")) return 2;
        return 0;
    }

    void AvanzarDialogo()
    {
        int estadoActual = ComprobarEstadoInventario();

        if (!decisionTomada && estadoActual != estadoInventarioAnterior)
        {
            estadoInventarioAnterior = estadoActual;
            fraseActual = 0;
        }

        string[] listaActual = ObtenerListaCorrecta();

        if (fraseActual >= listaActual.Length)
        {
            // Solo lanzamos la elección si tiene el antídoto activo (estado 1)
            if (!decisionTomada && estadoActual == 1)
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
        // Si ya tomaste la decisión con este científico en particular
        if (decisionTomada)
        {
            return seLlevoElAntidoto ? frasesSiLoCuras : frasesSiLoNiegas;
        }

        // Si no has decidido nada aquí, comprobamos qué traes en los bolsillos
        int estadoActual = ComprobarEstadoInventario();
        if (estadoActual == 1) return frasesConAntidoto;
        if (estadoActual == 2) return frasesAntidotoGastado;

        return frasesNormales;
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
            // Le quitamos el antídoto normal y le damos la marca secreta
            inventarioDelJugador.objetosRecogidos.Remove(objetoNecesario);
            inventarioDelJugador.objetosRecogidos.Add(objetoNecesario + "_Usado");

            if (!string.IsNullOrEmpty(objetoRecompensa) && !inventarioDelJugador.objetosRecogidos.Contains(objetoRecompensa))
            {
                inventarioDelJugador.objetosRecogidos.Add(objetoRecompensa);
            }
            estaInfectado = false;
            alCurarActualizarFinal?.Invoke();
        }

        AvanzarDialogo();
    }

    void MostrarFrase(string frase)
    {
        if (rutinaEscritura != null) StopCoroutine(rutinaEscritura);
        rutinaEscritura = StartCoroutine(EscribirTextoLetraALetra(frase));
    }

    IEnumerator EscribirTextoLetraALetra(string frase)
    {
        textoFlotante.text = frase;
        textoFlotante.maxVisibleCharacters = 0;
        textoFlotante.ForceMeshUpdate();
        int totalLetrasVisibles = textoFlotante.textInfo.characterCount;

        float tonoOriginal = altavozVoz != null ? altavozVoz.pitch : 1f;

        for (int i = 0; i <= totalLetrasVisibles; i++)
        {
            textoFlotante.maxVisibleCharacters = i;

            if (i > 0 && i % 2 == 0 && altavozVoz != null && sonidoVoz != null)
            {
                altavozVoz.pitch = Random.Range(0.8f, 1.2f);
                altavozVoz.PlayOneShot(sonidoVoz);
            }

            yield return new WaitForSeconds(velocidadTexto);
        }

        if (altavozVoz != null) altavozVoz.pitch = tonoOriginal;
        textoFlotante.maxVisibleCharacters = 99999;
    }
}