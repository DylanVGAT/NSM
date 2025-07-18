using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public string url = "https://github.com/DylanVGAT";

    public void StartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();
        }
        SceneManager.LoadScene("CharacterSelectionMenu", LoadSceneMode.Single);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void InputScene()
    {
        SceneManager.LoadScene("InputScene");
    }

    public void GitHub()
    {
        Application.OpenURL(url);
    }

    public void BackToMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();
        }

        var winner = GameObject.Find("PodiumWinner");
        if (winner != null)
        {
            Destroy(winner);
        }

        SceneManager.LoadScene("MainMenu");
    }

}
