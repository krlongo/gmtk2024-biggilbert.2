using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

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
        HUD.OnContinue += NextLevel;
    }

    private void OnDisable ()
    {
        HUD.OnReset -= ResetGame;
        HUD.OnContinue -= NextLevel;
    }

    public void ResetGame()
    {
        // TODO: Change with proper scene names
        SceneManager.LoadScene("ClimbingScene");
    }

    public void NextLevel()
    {
        // TODO: Change with proper scene names
        switch (SceneManager.GetActiveScene().name)
        {
            case "LevelOne":
                SceneManager.LoadScene("LevelTwo");
                break;
            case "LevelTwo":
                SceneManager.LoadScene("LevelThree");
                break;
            case "LevelThree":
                SceneManager.LoadScene("GameWon");
                break;
        }
    }
}
