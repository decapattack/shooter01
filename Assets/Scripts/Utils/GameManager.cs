using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public bool isGameOver = false;
    public int currentScore = 0;

    void Awake()
    {
        // Implementação Singleton Clássica
        if (Instance == null)
        {
            Instance = this;
            // Opcional: Se quiser que a musica continue tocando entre fases
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        currentScore += amount;

        // Atualiza a View
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScoreUI(currentScore);
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        // Chama a View de Game Over
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver();
        }
    }

    public void RestartLevel()
    {
        // Reseta flags para nova partida
        isGameOver = false;
        currentScore = 0;

        // Recarrega a cena ativa
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
