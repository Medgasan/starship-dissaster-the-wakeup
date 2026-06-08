using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Assets._Scripts;

public class DialogoCientifico : MonoBehaviour
{
    [Header("Componentes")]
    public TextMeshPro textoFlotante;
    public GameObject cartelAyudaUI;
    public AudioSource altavozVoz;
    public AudioClip sonidoVoz;

    [Header("Estado del NPC")]
    public bool estaInfectado = true;
    public string fraseAlCurarse = "¡Puedo respirar...! Toma mi tarjeta. Suerte ahí fuera...";

    [Header("Sistema de Inventario (Intercambio)")]
    public string objetoNecesario = "Antidoto";
    public string objetoRecompensa = "TarjetaRoja";

    [Header("Configuración de Diálogo (Las Dos Listas)")]
    [TextArea(2, 5)]
    public string[] frasesNormales;
    [TextArea(2, 5)]
    public string[] frasesConAntidoto;

    public KeyCode teclaInteraccion = KeyCode.E;
    public KeyCode teclaCurar = KeyCode.Q;
    public float velocidadTexto = 0.05f;
    public int frasesRepetiblesAlFinal = 1;

    [Header("Eventos del Final")]
    public UnityEvent alCurarActualizarFinal;

    private int fraseActual = 0;
    private bool jugadorCerca = false;
    private Coroutine rutinaEscritura;
    private CanInteract inventarioDelJugador;

    private bool leyendoListaAntidoto = false;

    // NUEVO: Variables para controlar cuándo mostrar la UI
    private bool haTerminadoDialogoAntidoto = false;
    private Camera camaraPrincipal;

    void Start()
    {
        textoFlotante.gameObject.SetActive(false);
        if (cartelAyudaUI != null) cartelAyudaUI.SetActive(false);
        camaraPrincipal = Camera.main; // Buscamos la cámara del jugador
    }

    void Update()
    {
        if (!jugadorCerca) return;

        bool tieneAntidoto = inventarioDelJugador != null && inventarioDelJugador.objetosRecogidos.Contains(objetoNecesario);

        // NUEVO: Comprobamos si el jugador está mirando directamente al científico
        bool loEstaMirando = false;
        if (camaraPrincipal != null)
        {
            RaycastHit hit;
            // Lanzamos un rayo desde la cámara hacia adelante (distancia de 10 metros max)
            if (Physics.Raycast(camaraPrincipal.transform.position, camaraPrincipal.transform.forward, out hit, 10f))
            {
                // Si el rayo choca contra el científico o alguna de sus partes
                if (hit.transform == transform || hit.transform.IsChildOf(transform))
                {
                    loEstaMirando = true;
                }
            }
        }

        // CONTROL DE LA UI: Solo se enciende si está infectado, tienes antídoto, ha dicho todo el diálogo y le estás mirando
        if (estaInfectado && tieneAntidoto && haTerminadoDialogoAntidoto && loEstaMirando)
        {
            if (cartelAyudaUI != null) cartelAyudaUI.SetActive(true);
        }
        else
        {
            if (cartelAyudaUI != null) cartelAyudaUI.SetActive(false);
        }

        // Avanzar el diálogo con E
        if (Input.GetKeyDown(teclaInteraccion))
        {
            if (!textoFlotante.gameObject.activeSelf)
            {
                textoFlotante.gameObject.SetActive(true);
            }
            AvanzarDialogo(tieneAntidoto);
        }

        // CURAR: Ahora también requiere que hayas terminado el diálogo y le estés mirando
        if (estaInfectado && tieneAntidoto && haTerminadoDialogoAntidoto && loEstaMirando && Input.GetKeyDown(teclaCurar))
        {
            CurarCientifico();
            if (cartelAyudaUI != null) cartelAyudaUI.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            inventarioDelJugador = other.GetComponentInChildren<CanInteract>();

            bool tieneAntidoto = inventarioDelJugador != null && inventarioDelJugador.objetosRecogidos.Contains(objetoNecesario);
            leyendoListaAntidoto = tieneAntidoto;

            if (fraseActual == 0 && estaInfectado)
            {
                textoFlotante.gameObject.SetActive(true);

                string[] listaActual = leyendoListaAntidoto ? frasesConAntidoto : frasesNormales;

                if (listaActual.Length > 0)
                {
                    MostrarFrase(listaActual[fraseActual]);
                    ComprobarSiTerminoDialogo(0, listaActual.Length); // Comprobamos si la lista tiene solo 1 frase
                    fraseActual++;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            inventarioDelJugador = null;
            textoFlotante.gameObject.SetActive(false);
            if (cartelAyudaUI != null) cartelAyudaUI.SetActive(false);

            if (rutinaEscritura != null) StopCoroutine(rutinaEscritura);
        }
    }

    void AvanzarDialogo(bool tieneAntidoto)
    {
        if (!estaInfectado)
        {
            MostrarFrase(fraseAlCurarse);
            return;
        }

        if (tieneAntidoto != leyendoListaAntidoto)
        {
            leyendoListaAntidoto = tieneAntidoto;
            fraseActual = 0;
            haTerminadoDialogoAntidoto = false; // Reiniciamos el estado
        }

        string[] listaActual = leyendoListaAntidoto ? frasesConAntidoto : frasesNormales;

        if (fraseActual < listaActual.Length)
        {
            MostrarFrase(listaActual[fraseActual]);
            ComprobarSiTerminoDialogo(fraseActual, listaActual.Length);
            fraseActual++;
        }
        else
        {
            fraseActual = Mathf.Max(0, listaActual.Length - frasesRepetiblesAlFinal);
            MostrarFrase(listaActual[fraseActual]);
            fraseActual++;
        }
    }

    // NUEVO: Función para encender el permiso de la UI
    void ComprobarSiTerminoDialogo(int indiceFraseActual, int totalFrases)
    {
        if (leyendoListaAntidoto && indiceFraseActual >= totalFrases - 1)
        {
            haTerminadoDialogoAntidoto = true;
        }
    }

    void CurarCientifico()
    {
        inventarioDelJugador.objetosRecogidos.Remove(objetoNecesario);

        if (!string.IsNullOrEmpty(objetoRecompensa))
        {
            if (!inventarioDelJugador.objetosRecogidos.Contains(objetoRecompensa))
            {
                inventarioDelJugador.objetosRecogidos.Add(objetoRecompensa);
            }
        }

        estaInfectado = false;
        alCurarActualizarFinal?.Invoke();
        MostrarFrase(fraseAlCurarse);
    }

    void MostrarFrase(string frase)
    {
        if (rutinaEscritura != null) StopCoroutine(rutinaEscritura);
        rutinaEscritura = StartCoroutine(EscribirTextoLetraALetra(frase));
    }

    IEnumerator EscribirTextoLetraALetra(string frase)
    {
        textoFlotante.text = "";
        float tonoOriginal = altavozVoz.pitch;
        int contadorLetras = 0;

        foreach (char letra in frase.ToCharArray())
        {
            textoFlotante.text += letra;

            if (letra != ' ')
            {
                if (contadorLetras % 2 == 0 && altavozVoz != null && sonidoVoz != null)
                {
                    altavozVoz.pitch = Random.Range(0.8f, 1.2f);
                    altavozVoz.PlayOneShot(sonidoVoz);
                }
                contadorLetras++;
            }
            yield return new WaitForSeconds(velocidadTexto);
        }

        if (altavozVoz != null) altavozVoz.pitch = tonoOriginal;
    }
}