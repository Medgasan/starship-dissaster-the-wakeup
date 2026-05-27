using UnityEngine;
using TMPro;

public class MinijuegoHackeoTerminal : MonoBehaviour
{
    [Header("Conexiones")]
    public TextMeshProUGUI pantallaTexto;

    [Header("Estética de la Terminal (Colores)")]
    [Tooltip("Color del Avatar (El 0)")]
    public Color colorJugador = Color.green;
    [Tooltip("Color del espacio vacío (Los puntos)")]
    public Color colorVacio = Color.gray;
    [Tooltip("Color del cortafuegos (Las almohadillas)")]
    public Color colorMuro = Color.red;
    [Tooltip("Color de la meta (La X)")]
    public Color colorMeta = Color.yellow;

    private ConsolaHackeo consolaActual;
    private Vector2 posicionActual;
    private int[,] mapa;
    private int anchoMapa;
    private int altoMapa;

    public void CargarNuevoNivel(string[] nuevoDiseño, Vector2 nuevoInicio, ConsolaHackeo consola)
    {
        consolaActual = consola;
        posicionActual = nuevoInicio;

        altoMapa = nuevoDiseño.Length;
        anchoMapa = nuevoDiseño[0].Length;

        mapa = new int[altoMapa, anchoMapa];

        for (int y = 0; y < altoMapa; y++)
        {
            for (int x = 0; x < anchoMapa; x++)
            {
                mapa[y, x] = int.Parse(nuevoDiseño[y][x].ToString());
            }
        }

        DibujarTerminal();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) Mover(0, -1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) Mover(0, 1);
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) Mover(-1, 0);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) Mover(1, 0);
    }

    void Mover(int dirX, int dirY)
    {
        int nuevaX = (int)posicionActual.x + dirX;
        int nuevaY = (int)posicionActual.y + dirY;

        if (nuevaX < 0 || nuevaX >= anchoMapa || nuevaY < 0 || nuevaY >= altoMapa) return;
        if (mapa[nuevaY, nuevaX] == 1) return;

        posicionActual = new Vector2(nuevaX, nuevaY);
        DibujarTerminal();

        if (mapa[nuevaY, nuevaX] == 2)
        {
            consolaActual.HackeoCompletado();
        }
    }

    void DibujarTerminal()
    {
        string textoGenerado = "";

        // 1. Traducimos los colores del Inspector a formato Hexadecimal (#RRGGBB)
        string hexJugador = "#" + ColorUtility.ToHtmlStringRGB(colorJugador);
        string hexVacio = "#" + ColorUtility.ToHtmlStringRGB(colorVacio);
        string hexMuro = "#" + ColorUtility.ToHtmlStringRGB(colorMuro);
        string hexMeta = "#" + ColorUtility.ToHtmlStringRGB(colorMeta);

        // 2. Construimos el mapa aplicando los colores traducidos
        for (int y = 0; y < altoMapa; y++)
        {
            for (int x = 0; x < anchoMapa; x++)
            {
                if (x == (int)posicionActual.x && y == (int)posicionActual.y)
                {
                    textoGenerado += $"<color={hexJugador}>0</color>";
                }
                else if (mapa[y, x] == 0)
                {
                    textoGenerado += $"<color={hexVacio}>·</color>";
                }
                else if (mapa[y, x] == 1)
                {
                    textoGenerado += $"<color={hexMuro}>#</color>";
                }
                else if (mapa[y, x] == 2)
                {
                    textoGenerado += $"<color={hexMeta}>X</color>";
                }
                textoGenerado += "  ";
            }
            textoGenerado += "\n";
        }

        pantallaTexto.text = textoGenerado;
    }
}