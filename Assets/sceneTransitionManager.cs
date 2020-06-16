using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneTransitionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnRestart()
    {
        SceneManager.LoadScene(sceneName: "Main");
    }

    
}
