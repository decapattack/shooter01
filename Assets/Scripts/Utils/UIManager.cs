using UnityEngine;
using TMPro; // Namespace necessário para usar TextMeshPro
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;

    void Awake()
    {
        Instance = this;
    }

    // Atualiza o texto do placar (String.Format estilo Java)
    public void UpdateScoreUI(int score)
    {
        scoreText.text = $"SCORE: {score:0000}";
    }

    // Ativa a tela de fim de jogo
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // Método que será chamado pelo Botão da UI
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}