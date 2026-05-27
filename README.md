# Starship Disaster: The Wakeup

> Juego de exploración y supervivencia en 3D construido con Unity 6. Despiertas solo a bordo de una nave espacial dañada — descubre qué ocurrió.

![Versión](https://img.shields.io/badge/versión-0.1.0-blue)
![Unity](https://img.shields.io/badge/Unity-6000.3.14f1-black?logo=unity)
![Estado](https://img.shields.io/badge/estado-en%20desarrollo-yellow)

---

## Descripción

**Starship Disaster: The Wakeup** es el primer capítulo de un juego sci-fi en primera/tercera persona. El jugador despierta a bordo de una nave espacial destruida y debe recorrerla interactuando con terminales y puertas para descubrir el desastre ocurrido.

---

## Stack Técnico

| Herramienta | Versión |
|---|---|
| Unity | 6000.3.14f1 (Unity 6) |
| Pipeline de renderizado | URP 17.3.0 |
| Sistema de Input | 1.19.0 |
| Navegación con IA | 2.0.12 |
| Cinemachine | 3.1.6 |
| VFX Graph | 17.3.0 |
| Post Processing | 3.5.4 |
| ProBuilder | 6.0.9 |
| Timeline | 1.8.12 |
| Visual Scripting | 1.9.11 |
| Inferencia con IA | 2.6.1 |

---

## Estructura del Proyecto

```
Assets/
├── _Scenes/
│   ├── SampleScene.unity   # Bootstrap / menú principal
│   ├── Mapa.unity          # Nivel principal de juego
│   └── Victoria.unity      # Secuencia de victoria / final
Packages/
└── manifest.json
ProjectSettings/
```

### Escenas

| Escena | Propósito |
|---|---|
| `SampleScene` | Punto de entrada |
| `Mapa` | Mapa principal de juego |
| `Victoria` | Secuencia de victoria / final |

### Tags y Capas personalizadas

| Tipo | Valores |
|---|---|
| Tags | `Terminal`, `Puerta` |
| Capas | `Interactables`, `Walls` |

---

## Primeros Pasos

### Requisitos previos

- [Unity Hub](https://unity.com/download)
- Unity Editor **6000.3.14f1**

### Configuración

```bash
git clone https://github.com/<tu-usuario>/starship-disaster-the-wakeup.git
```

1. Abre **Unity Hub** → **Abrir proyecto** → selecciona la carpeta clonada.
2. Unity resolverá los paquetes automáticamente mediante el Package Manager.
3. Abre `Assets/_Scenes/SampleScene.unity` y pulsa **Play**.

> ⚠️ Abrir con una versión distinta de Unity puede provocar advertencias de serialización.

---

## Compilación

`File → Build Settings` → selecciona la plataforma objetivo → **Build**.

Resolución por defecto: **1024 × 768**.

---

## Contribuir

1. Haz un fork del repositorio y crea una rama: `git checkout -b feature/tu-funcionalidad`
2. Confirma los cambios: `git commit -m "feat: descripción"`
3. Sube los cambios y abre una Pull Request.

---

## Licencia

