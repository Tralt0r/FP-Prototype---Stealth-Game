using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CS_GameManager : MonoBehaviour
{
    [System.Serializable]
    public class GameState
    {
        public bool isGameloss = false;
        public bool isGameWon = false;
    }
    private GameState gameState = new GameState();
    
    public void GameLoss()
    {
        if (gameState.isGameWon)
        {
            
        }
        else
        {
            gameState.isGameloss = true;
            Debug.Log("Game Lost");
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
        }
    }
}
