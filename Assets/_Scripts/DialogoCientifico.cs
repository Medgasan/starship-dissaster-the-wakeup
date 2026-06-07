using System.Collections;
using UnityEngine;
using TMPro;

public class DialogoCientifico : MonoBehaviour
{
    [Header("Componentes")]
    public TextMeshPro textoFlotante;
    public AudioSource altavozVoz;
    public AudioClip sonidoVoz;

    [Header("Configuración de Diálogo")]
    [TextArea(2, 5)]
    public string[] frases;
    public KeyCode teclaInteraccion = KeyCode.E;

    [Tooltip("Tiempo en segundos entre cada letra")]
    public float velocidadTexto = 0.05f;

    [Tooltip("Cuántas frases del final queremos que repita en bucle")]
    public int frasesRepetiblesAlFinal = 1; // Aquí controlas el bucle

    private int fraseActual = 0;
    private bool jugadorCerca = false;
    private Coroutine rutinaEscritura;

    void Start()
    {
        textoFlotante.gameObject.SetActive(false);
    }

    void Update()
    {
        // Si estamos cerca, pulsamos E, y la primera frase ya ha pasado
        if (jugadorCerca && Input.GetKeyDown(teclaInteraccion) && fraseActual > 0)
        {
            // Si nos fuimos y el texto se apagó, lo volvemos a encender al interactuar
            if (!textoFlotante.gameObject.activeSelf)
            {
                textoFlotante.gameObject.SetActive(true);
            }

            AvanzarDialogo();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;

            // Solo salta solo si es la primera vez que nos acercamos
            if (fraseActual == 0 && frases.Length > 0)
            {
                textoFlotante.gameObject.SetActive(true);
                MostrarFrase(frases[fraseActual]);
                fraseActual++;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            textoFlotante.gameObject.SetActive(false);

            if (rutinaEscritura != null)
            {
                StopCoroutine(rutinaEscritura);
            }
        }
    }

    void AvanzarDialogo()
    {
        // Si aún nos quedan frases nuevas por leer
        if (fraseActual < frases.Length)
        {
            MostrarFrase(frases[fraseActual]);
            fraseActual++;
        }
        else
        {
            // 🔥 EL TRUCO DEL BUCLE 🔥
            // Retrocedemos el contador para repetir las últimas frases
            fraseActual = Mathf.Max(0, frases.Length - frasesRepetiblesAlFinal);
            MostrarFrase(frases[fraseActual]);
            fraseActual++; // Preparamos el contador para el siguiente click
        }
    }

    void MostrarFrase(string frase)
    {
        if (rutinaEscritura != null)
        {
            StopCoroutine(rutinaEscritura);
        }
        rutinaEscritura = StartCoroutine(EscribirTextoLetraALetra(frase));
    }

    // El motor de la máquina de escribir y el sonido optimizado
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

        if (altavozVoz != null)
        {
            altavozVoz.pitch = tonoOriginal;
        }
    }
}