[ComandosComunesGit.md](https://github.com/user-attachments/files/27961919/ComandosComunesGit.md)
# 🚀 Chuleta completa de Git

## ⚙️ Configuración

| Comando | Descripción |
|---------|-------------|
| `git config --global user.name "Tu Nombre"` | Establece nombre de usuario para los commits. |
| `git config --global user.email "tu@email.com"` | Establece correo electrónico. |
| `git config --global core.editor "code --wait"` | Cambia editor por defecto (ej: VS Code). |
| `git config --list` | Muestra toda la configuración actual. |
| `git config --global alias.st status` | Crea un alias (ej: `git st` para `git status`). |

## 🆕 Crear un repositorio

| Comando | Descripción |
|---------|-------------|
| `git init` | Inicializa un repositorio vacío en la carpeta actual. |
| `git clone <url>` | Clona un repositorio remoto (HTTPS o SSH). |
| `git clone <url> nombre` | Clona en una carpeta con nombre diferente. |

## 📁 Flujo básico (trabajo local)

| Comando | Descripción |
|---------|-------------|
| `git status` | Muestra el estado de los archivos. |
| `git add <archivo>` | Añade un archivo al área de staging. |
| `git add .` | Añade todos los archivos modificados y nuevos. |
| `git add -A` | Añade todo, incluyendo eliminaciones. |
| `git reset <archivo>` | Saca el archivo del staging. |
| `git rm <archivo>` | Elimina el archivo del repositorio y del disco. |
| `git rm --cached <archivo>` | Elimina del control de versiones pero lo conserva en disco. |
| `git commit -m "mensaje"` | Crea un commit con los cambios del staging. |
| `git commit -a -m "mensaje"` | Añade todos los cambios (sin nuevos archivos) y hace commit. |
| `git commit --amend -m "nuevo mensaje"` | Corrige el último commit. |

## 🔄 Historial y diferencias

| Comando | Descripción |
|---------|-------------|
| `git log` | Muestra el historial de commits (`q` para salir). |
| `git log --oneline --graph` | Historial resumido y gráfico. |
| `git log --author="nombre"` | Filtra commits por autor. |
| `git diff` | Muestra diferencias sin stage. |
| `git diff --staged` | Muestra diferencias en el staging. |
| `git diff <commit1> <commit2>` | Compara dos commits. |
| `git show <commit>` | Muestra los cambios de un commit específico. |

## 🌿 Ramas (branching)

| Comando | Descripción |
|---------|-------------|
| `git branch` | Lista ramas locales. |
| `git branch -r` | Lista ramas remotas. |
| `git branch -a` | Lista todas las ramas. |
| `git branch <nombre>` | Crea una rama. |
| `git checkout <nombre>` | Cambia a otra rama. |
| `git switch <nombre>` | Alternativa moderna a `checkout`. |
| `git checkout -b <nombre>` | Crea y cambia a la nueva rama. |
| `git switch -c <nombre>` | Crear y cambiar (moderno). |
| `git branch -d <nombre>` | Borra una rama (solo si fusionada). |
| `git branch -D <nombre>` | Borra una rama forzosamente. |
| `git branch -m <viejo> <nuevo>` | Renombra una rama. |
| `git merge <rama>` | Fusiona la rama especificada en la actual. |
| `git merge --abort` | Cancela una fusión con conflictos. |

## ⚠️ Conflictos de fusión

1. `git merge <rama>` → aparece conflicto.
2. Editar archivos conflictivos (marcas `<<<<<<<`, `=======`, `>>>>>>>`).
3. `git add <archivo>` para marcar como resuelto.
4. `git commit` para terminar la fusión.

## 💾 Trabajo con repositorios remotos

| Comando | Descripción |
|---------|-------------|
| `git remote add origin <url>` | Agrega un remoto llamado `origin`. |
| `git remote -v` | Muestra los remotos configurados. |
| `git remote remove <nombre>` | Elimina un remoto. |
| `git push -u origin main` | Envía `main` a `origin` y lo establece como upstream. |
| `git push` | Envía los commits al remoto upstream. |
| `git push origin <rama>` | Envía una rama específica. |
| `git push --tags` | Envía todas las etiquetas. |
| `git pull` | Trae cambios del remoto y los fusiona. |
| `git fetch` | Descarga cambios del remoto sin fusionar. |

## 🔄 Sincronización avanzada

| Comando | Descripción |
|---------|-------------|
| `git pull --rebase` | Trae cambios y los rebasa localmente. |
| `git fetch origin` + `git merge origin/main` | Equivalente a `git pull` paso a paso. |
| `git push --force` | Fuerza el push (peligroso). |
| `git push --force-with-lease` | Más seguro que `--force`. |

## 🧹 Deshacer cambios

| Comando | Descripción |
|---------|-------------|
| `git checkout -- <archivo>` | Descarta cambios en un archivo. |
| `git restore <archivo>` | Alternativa moderna. |
| `git restore --staged <archivo>` | Saca archivo del staging. |
| `git reset --soft HEAD~1` | Deshace el último commit, mantiene cambios en staging. |
| `git reset HEAD~1` (mixed) | Deshace commit y saca cambios del staging. |
| `git reset --hard HEAD~1` | **Peligroso**: borra commit y cambios locales. |
| `git revert <commit-hash>` | Crea un commit que deshace los cambios del commit indicado. |

## 🏷️ Etiquetas (tags)

| Comando | Descripción |
|---------|-------------|
| `git tag` | Lista todas las etiquetas. |
| `git tag -a v1.0 -m "Mensaje"` | Crea una etiqueta anotada. |
| `git tag v1.0-light` | Crea una etiqueta ligera. |
| `git show v1.0` | Muestra detalles de la etiqueta. |
| `git push origin v1.0` | Envía una etiqueta al remoto. |
| `git push origin --tags` | Envía todas las etiquetas. |
| `git tag -d v1.0` | Borra etiqueta local. |
| `git push origin --delete v1.0` | Borra etiqueta en remoto. |

## 📦 Stash (guardar cambios temporalmente)

| Comando | Descripción |
|---------|-------------|
| `git stash` | Guarda cambios no confirmados. |
| `git stash push -m "mensaje"` | Guarda con mensaje. |
| `git stash list` | Lista los stashes. |
| `git stash apply` | Aplica el último stash pero lo mantiene. |
| `git stash pop` | Aplica el último stash y lo elimina. |
| `git stash drop stash@{0}` | Elimina un stash específico. |
| `git stash clear` | Borra todos los stashes. |

## 🧪 Rebase (reescribir historial)

| Comando | Descripción |
|---------|-------------|
| `git rebase <branch>` | Reaplica commits de la rama actual encima de otra. |
| `git rebase -i HEAD~3` | Rebase interactivo (squash, reordenar, editar). |
| `git rebase --continue` | Continúa tras resolver conflictos. |
| `git rebase --skip` | Omite un commit conflictivo. |
| `git rebase --abort` | Cancela el rebase. |

> ⚠️ **No hacer rebase en ramas públicas/compartidas** a menos que sepas bien lo que haces.

## 🔎 Inspección y depuración

| Comando | Descripción |
|---------|-------------|
| `git blame <archivo>` | Muestra quién modificó cada línea. |
| `git bisect start` | Inicia búsqueda binaria. |
| `git bisect bad` | Marca commit actual como malo. |
| `git bisect good <commit>` | Marca un commit como bueno. |
| `git shortlog -sn` | Número de commits por autor. |
| `git grep "texto"` | Busca texto en el repositorio. |
| `git reflog` | Registro de movimientos de HEAD. |

## 🔗 Submódulos

| Comando | Descripción |
|---------|-------------|
| `git submodule add <url>` | Añade un submódulo. |
| `git submodule update --init` | Clona y prepara submódulos. |
| `git submodule update --remote` | Actualiza submódulos a su última versión. |

## 🧹 Limpieza y mantenimiento

| Comando | Descripción |
|---------|-------------|
| `git clean -n` | Muestra archivos no trackeados que se borrarían. |
| `git clean -f` | Elimina archivos no trackeados. |
| `git clean -fd` | Elimina archivos y directorios no trackeados. |
| `git gc` | Optimiza el repositorio. |
| `git fsck` | Verifica la integridad. |

## 📌 Consejos útiles

- Usa **alias** para ahorrar tiempo (`git config --global alias.co checkout`).
- Configura **colores** con `git config --global color.ui auto`.
- Crea una **clave SSH** para GitHub/GitLab.
- Siempre usa `.gitignore` para archivos temporales o secretos.
- Explora la documentación con `git help <comando>`.
