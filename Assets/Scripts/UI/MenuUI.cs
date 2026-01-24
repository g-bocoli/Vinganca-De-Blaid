using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuUI : MonoBehaviour
{
    [Header("Menu UI Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitGameButton;


    private void OnEnable()
    {
        playButton.onClick.AddListener(GoToGameplayScene);
        exitGameButton.onClick.AddListener(ExitGame);
    }

    private void GoToGameplayScene()
    {
        SceneManager.LoadScene("Game_LV01");
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit()
#endif
    }
}
