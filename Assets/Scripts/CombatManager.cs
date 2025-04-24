using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }
    public event Action OnCombatStart;
    public event Action<bool> OnTurnChanged; // true = bot, false = player
    public event Action OnVictory;           // Event khi thắng (combat hoặc MovingScene)
    public event Action OnGameOver;          // Event khi thua
    public event Action OnGameOut;

    private bool isBotTurn = false;
    public bool IsBotTurn => isBotTurn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneLoader.Instance.LoadScene("MainMenu");
        isBotTurn = false;
        OnTurnChanged?.Invoke(isBotTurn);
    }

    public void StartCombat()
    {
        SceneLoader.Instance.LoadScene("CombatScene");
        OnCombatStart?.Invoke();
        isBotTurn = false;
        OnTurnChanged?.Invoke(isBotTurn);
    }

    public void EndPlayerTurn()
    {
        isBotTurn = true;
        Debug.Log("Player turn ended. Bot turn started.");
        OnTurnChanged?.Invoke(isBotTurn);
    }

    public void EndBotTurn()
    {
        isBotTurn = false;
        Debug.Log("Bot turn ended. Player turn started.");
        OnTurnChanged?.Invoke(isBotTurn);
    }

    // Gọi khi Player đánh bại Enemy trong CombatScene
    public void Victory()
    {
        Debug.Log("Victory in combat! Reloading MovingScene.");
        SceneLoader.Instance.LoadScene("MovingScene");
        OnVictory?.Invoke();
    }

    // Gọi khi thua trong PuzzleScene
    public void GameOver()
    {
        Debug.Log("Game Over!");
        SceneLoader.Instance.LoadScene("GameOverScene");
        OnGameOver?.Invoke();
    }

    // Gọi khi nhặt đủ vật phẩm trong MovingScene
    public void OnAllItemsCollected()
    {
        Debug.Log("All items collected! Ready for finish point.");
        // Không chuyển Scene ở đây, chờ Player đến đích
    }

    // Gọi khi Player đến đích với đủ vật phẩm
    public void CompleteLevel()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Victory>().HasAllItems())
        {
            Debug.Log("Level completed! Moving to VictoryScene.");
            SceneLoader.Instance.LoadScene("VictoryScene");
            OnVictory?.Invoke();
        }
        else
        {
            Debug.Log("You need to collect all items before finishing!");
        }
    }
}