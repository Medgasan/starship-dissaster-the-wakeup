Starship Disaster: The Wakeup

Juego de exploración y supervivencia en 3D construido con Unity 6. Despiertas solo a bordo de una nave espacial dañada — descubre qué ocurrió.

Mostrar imagen
Mostrar imagen
Mostrar imagen

Descripción
Starship Disaster: The Wakeup es el primer capítulo de un juego sci-fi en primera/tercera persona. El jugador despierta a bordo de una nave espacial destruida y debe recorrerla interactuando con terminales y puertas para descubrir el desastre ocurrido.

Stack Técnico
HerramientaVersiónUnity6000.3.14f1 (Unity 6)Pipeline de renderizadoURP 17.3.0Sistema de Input1.19.0Navegación con IA2.0.12Cinemachine3.1.6VFX Graph17.3.0Post Processing3.5.4ProBuilder6.0.9Timeline1.8.12Visual Scripting1.9.11Inferencia con IA2.6.1

Estructura del Proyecto
Assets/
├── _Scenes/
│   ├── SampleScene.unity   # Bootstrap / menú principal
│   ├── Mapa.unity          # Nivel principal de juego
│   └── Victoria.unity      # Secuencia de victoria / final
Packages/
└── manifest.json
ProjectSettings/
Escenas
EscenaPropósitoSampleScenePunto de entradaMapaMapa principal de juegoVictoriaSecuencia de victoria / final
Tags y Capas personalizadas
TipoValoresTagsTerminal, PuertaCapasInteractables, Walls

Primeros Pasos
Requisitos previos

Unity Hub
Unity Editor 6000.3.14f1

Configuración
bashgit clone https://github.com/<tu-usuario>/starship-disaster-the-wakeup.git

Abre Unity Hub → Abrir proyecto → selecciona la carpeta clonada.
Unity resolverá los paquetes automáticamente mediante el Package Manager.
Abre Assets/_Scenes/SampleScene.unity y pulsa Play.


⚠️ Abrir con una versión distinta de Unity puede provocar advertencias de serialización.


Compilación
File → Build Settings → selecciona la plataforma objetivo → Build.
Resolución por defecto: 1024 × 768.

Contribuir

Haz un fork del repositorio y crea una rama: git checkout -b feature/tu-funcionalidad
Confirma los cambios: git commit -m "feat: descripción"
Sube los cambios y abre una Pull Request.
