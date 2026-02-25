using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CS_GameManager : MonoBehaviour
{
    [System.Serializable]
    public class GameState
    {
        public bool isGameloss = false;
        public bool isGameWon = false;
    }
    private GameState gameState = new GameState();

    // For Win and Lose Screens
    [SerializeField] private GameObject WinText;
    [SerializeField] private GameObject LoseText;

    public void GameLoss()
    {
        if (gameState.isGameWon)
        {
            
        }
        else
        {
            gameState.isGameloss = true;
            Debug.Log("Game Lost");
            LoseText.SetActive(true);
            FreezeGame();
        }
    }

    public void GameWon()
    {
        if (gameState.isGameloss)
        {
            
        }
        else
        {
            gameState.isGameWon = true;
            Debug.Log("Game Won");
            WinText.SetActive(true);
            FreezeGame();
        }
    }

    public void TriggeredEvent()
    {
        if (gameState.isGameloss || gameState.isGameWon)
        {
            
        }
        else
        {
            Debug.Log("Triggered Event");
        }
    }

    // For stopping the game when you win or lose, unlock cursor.
    private void FreezeGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Unfreezes and restarts game when button is pressed.
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}