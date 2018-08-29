using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {

	public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
