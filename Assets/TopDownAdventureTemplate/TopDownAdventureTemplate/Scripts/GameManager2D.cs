using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームオーバー処理とリスタート処理を管理します。
/// </summary>
public class GameManager2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth2D playerHealth;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Settings")]
    [SerializeField] private bool pauseOnGameOver = true;

    private bool gameOver;

    private void Awake()
    {
        if (playerHealth == null) playerHealth = FindFirstObjectByType<PlayerHealth2D>();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.PlayerDied += OnPlayerDied;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.PlayerDied -= OnPlayerDied;
        }
    }

    private void OnPlayerDied()
    {
        if (gameOver) return;
        gameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (pauseOnGameOver)
        {
            Time.timeScale = 0f;
        }
    }

    public void RestartCurrentScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
