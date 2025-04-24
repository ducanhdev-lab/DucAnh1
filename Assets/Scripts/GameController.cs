using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayAgain();
        }
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }
    public void PlayAgain()
    {
            SceneLoader.Instance.LoadScene("MovingScene");
    }
    public void StartGame() { 
          SceneLoader.Instance.LoadScene("MovingScene");    
    }
    public void Quit()
    {
        Application.Quit();
    }
}