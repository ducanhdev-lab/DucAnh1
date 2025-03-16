using System;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }
    public event Action OnCombatStart;

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
        SceneLoader.Instance.LoadScene("MovingScene");
    }

    public void StartCombat()
    {
        SceneLoader.Instance.LoadScene("CombatScene");
        OnCombatStart?.Invoke();
    }
}