using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // GameObject References
    #region

    // All used to reference Text Objects
    public GameObject MainMenu;
    public GameObject Pause;
    public GameObject Confirmation;
    public GameObject Controls;
    public GameObject Credits;
    public GameObject TimeText;
    public GameObject Interact;
    public GameObject Win;
    public GameObject Lose;
    public GameObject Sprint;

    // To store the previous menu of the player
    private GameObject Previous;

    // To register when the game is paused and to restart the game
    bool IsPaused = false;
    public static bool RestartGame = false;

    // References
    public CS_GameManager GameManage;
    private PlayerControls controls;

    // For Starting Dialogue
    public GameObject Dialogue;
    public TextMeshProUGUI DialogueText;

    public float DialogueDuration = 6f;

    #endregion

    // Awake for Controls
    #region

    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Pause.performed += ctx => TogglePause();
    }

    #endregion

    // Start
    #region

    // Start is called before the first frame update
    // Activates the Main Menu on start and resets the game if RestartGame is true
    void Start()
    {
        if (RestartGame)
        {
            RestartGame = false;
            PlayGame();
            return;
        }

        MainMenu.SetActive(true);
        Pause.SetActive(false);
        Confirmation.SetActive(false);
        Controls.SetActive(false);
        Credits.SetActive(false);
        TimeText.SetActive(false);
        Interact.SetActive(false);
        Sprint.SetActive(false);
        Win.SetActive(false);
        Lose.SetActive(false);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    #endregion

    // PlayGame Function
    #region

    // Activates when Play Button is pressed
    public void PlayGame()
    {
        MainMenu.SetActive(false);
        TimeText.SetActive(true);
        Interact.SetActive(true);
        Sprint.SetActive(true);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ShowDialogue("OH SHIT !!! I forgot my phone on the train! I have 30 SECONDS to get it!!! \n I don't have time to get a ticket! I need to jump over and go!!! Forget about the guards!");
    }

    #endregion

    // PauseGame Function
    #region

    // Activates when pause key is pressed
    public void PauseGame()
    {
        Pause.SetActive(true);
        MainMenu.SetActive(false);
        TimeText.SetActive(false);
        Interact.SetActive(false);
        Sprint.SetActive(false);
        Confirmation.SetActive(false);
        Controls.SetActive(false);
        Credits.SetActive(false);
        Win.SetActive(false);
        Lose.SetActive(false);

        Time.timeScale = 0f;
        IsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    #endregion

    // ResumeGame Function
    #region

    // Activates when Resume button is pressed
    public void ResumeGame()
    {
        Pause.SetActive(false);
        TimeText.SetActive(true);
        Interact.SetActive(true);
        Sprint.SetActive(true);

        Time.timeScale = 1f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    #endregion

    // OpenControls Function
    #region

    // Activates when Controls Button is pressed
    public void OpenControls(GameObject FromMenu)
    {
        Previous = FromMenu;

        MainMenu.SetActive(false);
        Pause.SetActive(false);

        Controls.SetActive(true);
    }

    #endregion

    // OpenCredits Function
    #region

    // Activates when Credits Button is pressed
    public void OpenCredits(GameObject FromMenu)
    {
        Previous = FromMenu;

        MainMenu.SetActive(false);
        Credits.SetActive(true);
    }

    #endregion

    // CloseSubMenu Function
    #region

    // Activates when the Close Button is pressed on the Controls and Credits menu
    public void CloseSubMenu()
    {
        Debug.Log("Close button pressed");

        Controls.SetActive(false);
        Credits.SetActive(false);

        if (Previous != null)
        {
            Previous.SetActive(true);
        }
        else
        {
            MainMenu.SetActive(true);
        }
    }

    #endregion

    // AskMainMenu Function
    #region

    // Activates when Main menu Button is pressed
    public void AskMainMenu(GameObject FromMenu)
    {
        Previous = FromMenu;

        Confirmation.SetActive(true);
        Pause.SetActive(false);
        Win.SetActive(false);
        Lose.SetActive(false);
    }

    #endregion

    // ConfirmMainMenu Function
    #region

    // Activates when the Yes Button is pressed in the Main Menu Confirmation Menu
    public void ConfirmMainMenu()
    { 
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

    // CancelMainMenu Function
    #region

    // Activates when the No Button is pressed in the Main Menu Confirmation Menu
    public void CancelMainMenu()
    {
        Confirmation.SetActive(false);

        if (Previous != null)
        {
            Previous.SetActive(true);
        }
    }

    #endregion

    // ExitGame Function
    #region

    // Activates when the Exit Button is pressed
    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion

    // ShowDialogue Function
    #region

    public void ShowDialogue(string Message)
    {
        DialogueText.text = Message;
        Dialogue.SetActive(true);

        StartCoroutine(HideDialogueAfterTime());
    }

    IEnumerator HideDialogueAfterTime()
    {
        yield return new WaitForSeconds(DialogueDuration);

        Dialogue.SetActive(false);
    }

    #endregion

    // To make sure the Controls work
    #region

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    #endregion

    // TogglePause Function
    #region

    void TogglePause()
    {
        if (!IsPaused)
            PauseGame();
        else
            ResumeGame();
    }

    #endregion
}