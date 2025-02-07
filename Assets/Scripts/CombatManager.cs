using UnityEngine;
using System;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }
    public event Action OnCombatStart;
    //public event Action OnCombatEnd;

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

    /*public void EndCombat()
    {
            SceneLoader.Instance.LoadScene("MovingScene");
            OnCombatEnd?.Invoke();
    }*/
}