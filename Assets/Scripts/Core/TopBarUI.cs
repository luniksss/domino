using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TopBarUI : MonoBehaviour
{
    [Header("Кнопки")]
    public Button backButton;

    private void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackClicked);
        }
    }

    private void OnBackClicked()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameController controller = FindObjectOfType<GameController>();
            if (controller != null)
            {
                controller.SaveCurrentState();
            }

            GameLauncher.LoadMenuScene();
        }
        else if (MenuManager.Instance != null)
        {
            MenuManager.Instance.ShowMain();
        }
    }
}