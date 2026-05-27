using UnityEngine;
using UnityEngine.UI;

public class MinijuegoHackeoGrid : MonoBehaviour
{
    [Header("Conexiones")]
    public ConsolaHackeo consolaPrincipal; // El script del panel 3D que hicimos antes
    public RectTransform cursorUI; // La imagen que representa al jugador en la UI

    [Header("Configuración del Tablero")]
    public float tamañoCasilla = 64f; // Los píxeles que se mueve el cursor por cada paso
    public Vector2 inicio = new Vector2(0, 0); // Coordenada donde empieza (X, Y)

    private Vector2 posicionActual;

    // EL MAPA (0 = Camino libre, 1 = Muro/Cortafuegos, 2 = Meta)
    // Este es un laberinto de ejemplo de 5x5. ¡Puedes hacerlo del tamaño que quieras!
    private int[,] mapa = new int[,]
    {
        { 0, 0, 1, 0, 2 },
        { 1, 0, 1, 0, 1 },
        { 0, 0, 0, 0, 0 },
        { 0, 1, 1, 1, 0 },
        { 0, 0, 0, 1, 0 }
    };

    private int anchoMapa;
    private int altoMapa;

    void Awake()
    {
        altoMapa = mapa.GetLength(0);
        anchoMapa = mapa.GetLength(1);
    }

    void OnEnable()
    {
        // Cada vez que se enciende la pantalla, el jugador vuelve a la salida
        posicionActual = inicio;
        ActualizarCursor();
    }

    void Update()
    {
        // Detectamos el movimiento con las teclas
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) Mover(0, -1); // Arriba
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) Mover(0, 1);  // Abajo
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) Mover(-1, 0); // Izquierda
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) Mover(1, 0); // Derecha
    }

    void Mover(int dirX, int dirY)
    {
        int nuevaX = (int)posicionActual.x + dirX;
        int nuevaY = (int)posicionActual.y + dirY;

        // 1. Comprobar que no nos salimos del tablero
        if (nuevaX < 0 || nuevaX >= anchoMapa || nuevaY < 0 || nuevaY >= altoMapa) return;

        // 2. Comprobar si nos chocamos con un muro (número 1)
        if (mapa[nuevaY, nuevaX] == 1)
        {
            Debug.Log("Bloqueado por cortafuegos");
            return;
        }

        // 3. Si el camino está libre, nos movemos
        posicionActual = new Vector2(nuevaX, nuevaY);
        ActualizarCursor();

        // 4. Comprobar si hemos pisado la meta (número 2)
        if (mapa[nuevaY, nuevaX] == 2)
        {
            Debug.Log("¡Laberinto completado!");
            consolaPrincipal.HackeoCompletado(); // Le avisamos a la consola 3D de que abra la puerta
        }
    }

    void ActualizarCursor()
    {
        // Movemos la imagen visualmente multiplicando su posición por el tamaño de la casilla
        cursorUI.anchoredPosition = new Vector2(posicionActual.x * tamañoCasilla, -posicionActual.y * tamañoCasilla);
    }
}