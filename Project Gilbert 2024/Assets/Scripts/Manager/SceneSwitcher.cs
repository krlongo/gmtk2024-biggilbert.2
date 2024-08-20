using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        HUD.OnReset += ResetGame;
        BonfireBehavior.OnRest += LoadBonfireHUD;
        BonfireHUD.OnContinue += NextLevel;
    }

    private void OnDisable ()
    {
        HUD.OnReset -= ResetGame;
        BonfireBehavior.OnRest -= LoadBonfireHUD;
        BonfireHUD.OnContinue -= NextLevel;
    }

    public void ResetGame()
    {
        // TODO: Change with proper scene names
        SceneManager.LoadScene("LevelOne");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadBonfireHUD()
    {
        SceneManager.LoadScene("Bonfire");
    }

    public void NextLevel()
    {
        Debug.Log("NextLevel");
        // TODO: Change with proper scene names
        switch (playerData.currentLevel)
        {
            case 0:
                SceneManager.LoadScene("LevelOne");
                break;
            case 1:
                SceneManager.LoadScene("LevelTwo");
                break;
            case 2:
                SceneManager.LoadScene("LevelThree");
                break;
            case 3:
                SceneManager.LoadScene("Ending");
                break;
        }
        playerData.currentLevel++;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
