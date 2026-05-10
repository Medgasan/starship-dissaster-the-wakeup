using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ControladorRetro : MonoBehaviour
{
    [Header("--- INTERRUPTOR MAESTRO ---")]
    public bool efectosRetroActivados = true;

    [Header("--- REFERENCIAS ---")]
    public Camera camaraJugador;
    public RawImage pantallaUI;
    public RenderTexture texturaPantalla;
    public Material materialRetro;

    [Header("--- RENDIMIENTO (FPS) ---")]
    [Range(10, 60)] public int limiteFPS = 30;

    [Header("--- PANTALLA (PÍXELES) ---")]
    [Range(64, 640)] public int anchoResolucion = 320;
    [Tooltip("Calcula el alto automáticamente para aspecto clásico")]
    public bool forzarFormato4tercios = true;
    [Range(64, 480)] public int altoResolucion = 240;

    [Header("--- ESTÉTICA DEL SHADER ---")]
    [Range(10, 500)] public float temblorVertices = 100f;
    [Range(2, 64)] public float nivelesColor = 16f;
    [Range(0f, 1f)] public float deformacionTexturas = 1f;

    [Header("--- NIEBLA SILENT HILL ---")]
    public bool usarNiebla = true;
    public Color colorNiebla = new Color(0.2f, 0.2f, 0.2f);
    [Range(0, 50)] public float inicioNiebla = 2f;
    [Range(0, 100)] public float finNiebla = 20f;

    void Update()
    {
        if (camaraJugador == null || pantallaUI == null || texturaPantalla == null || materialRetro == null) return;

        // 1. LÓGICA DEL INTERRUPTOR GENERAL
        if (!efectosRetroActivados)
        {
            camaraJugador.targetTexture = null;
            pantallaUI.enabled = false;
            RenderSettings.fog = false;
            if (Application.isPlaying) Application.targetFrameRate = -1;

            // Reseteamos el shader para que se vea "moderno"
            materialRetro.SetFloat("_Resolucion_Vertices", 10000f);
            materialRetro.SetFloat("_Steps", 256f);
            materialRetro.SetFloat("_Deformacion_Textura", 0f);
            return;
        }

        // 2. ACTIVAR SISTEMA DE RENDERIZADO
        camaraJugador.targetTexture = texturaPantalla;
        pantallaUI.enabled = true;

        // 3. CONTROL DE FPS (Solo en Play)
        if (Application.isPlaying && Application.targetFrameRate != limiteFPS)
        {
            Application.targetFrameRate = limiteFPS;
        }

        // 4. CONTROL DE PÍXELES (Render Texture)
        int targetHeight = forzarFormato4tercios ? (anchoResolucion * 3) / 4 : altoResolucion;

        if (texturaPantalla.width != anchoResolucion || texturaPantalla.height != targetHeight)
        {
            texturaPantalla.Release();
            texturaPantalla.width = anchoResolucion;
            texturaPantalla.height = targetHeight;
            texturaPantalla.depth = 24;
            texturaPantalla.Create();
        }

        // 5. NIEBLA ATMOSFÉRICA
        RenderSettings.fog = usarNiebla;
        RenderSettings.fogColor = colorNiebla;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = inicioNiebla;
        RenderSettings.fogEndDistance = finNiebla;
        camaraJugador.backgroundColor = colorNiebla;

        // 6. ENVIAR TODO AL SHADER GRAPH
        materialRetro.SetFloat("_Resolucion_Vertices", temblorVertices);
        materialRetro.SetFloat("_Steps", nivelesColor);
        materialRetro.SetFloat("_Deformacion_Textura", deformacionTexturas);

        materialRetro.SetColor("_ColorNiebla", colorNiebla);
        materialRetro.SetFloat("_InicioNiebla", inicioNiebla);
        materialRetro.SetFloat("_FinNiebla", finNiebla);
    }
}