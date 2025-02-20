using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject panel;
    public GameObject menu;
    public Button playButton;
    public Button[] buttons;

    private void Awake()
    {
        //unlock
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(MenuPanel);
    }

    public void MenuPanel()
    {
        panel.SetActive(false);
        menu.SetActive(true);
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
    }
}
