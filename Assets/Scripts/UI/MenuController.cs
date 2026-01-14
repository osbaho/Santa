using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private const string GameplaySceneName = "Gameplay";
    private const string MainMenuSceneName = "Menu Principal";

    /// <summary>
    /// Carga la escena del juego.
    /// </summary>
    public void IniciarJuego()
    {
        SceneManager.LoadScene(GameplaySceneName);
    }

    /// <summary>
    /// Carga la escena del menú principal.
    /// </summary>
    public void IrAMenuPrincipal()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
