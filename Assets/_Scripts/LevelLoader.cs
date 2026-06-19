using UnityEngine;
using UnityEngine.SceneManagement;




public class LevelLoader : MonoBehaviour
{

    private void Start()
    {
        SceneManager.LoadScene("LevelAmbient", LoadSceneMode.Additive);
        SceneManager.LoadScene("Mapa", LoadSceneMode.Additive);
        SceneManager.LoadScene("Victoria", LoadSceneMode.Additive);
    }

    



}
