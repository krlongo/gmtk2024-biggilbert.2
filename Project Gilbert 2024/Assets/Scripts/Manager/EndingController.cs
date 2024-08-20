using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingController : MonoBehaviour
{

    public Sprite[] comicPanels;
    public int currentPage;
    public Image image;
    public GameObject quitButton;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            TurnPage(-1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TurnPage(1);
        }
    }

    public void TurnPage(int index)
    {
        if(index < 0 && currentPage != 0)
        {
            currentPage--;
            image.sprite = comicPanels[currentPage];
        }
        else if(index > 0 && currentPage != 7)
        {
            currentPage++;
            image.sprite = comicPanels[currentPage];
            if(currentPage == comicPanels.Length - 1)
            {
                quitButton.SetActive(true);
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

}
