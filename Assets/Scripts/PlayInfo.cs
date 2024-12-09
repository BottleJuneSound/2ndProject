using UnityEngine;

public class PlayInfo : MonoBehaviour
{
    public GameObject gamePlayInfo;
    public GameObject exitButton;
    public GameObject info00;
    public GameObject info01;
    public GameObject info02;
    public SoundManager soundManager;
    public bool infoButton = false;
    void Start()
    {
        gamePlayInfo.SetActive(false);
        info00.SetActive(false);
        info01.SetActive(false);
        info02.SetActive(false);
        infoButton = false;
        OnInfoButton();
    }

    void Update()
    {

    }

    public void OnInfoButton()
    {
        if (!infoButton)
        {
            if (gamePlayInfo.activeSelf) return;
            soundManager.ClickButtonSFX();
            infoButton = true;
            gamePlayInfo.SetActive(true);
            info00.SetActive(true);
            Time.timeScale = 0;

        }
        else if (infoButton)
        {
            if (!gamePlayInfo.activeSelf) return;
            soundManager.ClickButtonSFX();
            infoButton = false;
            gamePlayInfo.SetActive(false);
            info00.SetActive(false);
            info01.SetActive(false);
            info02.SetActive(false);
            Time.timeScale = 1f;
        }
        //if (gameObject.activeSelf) return;
        //soundManager.ClickButtonSFX();
        //gamePlayInfo.SetActive(true);
        //info00.SetActive(true);
    }

    public void OnNextButton()
    {
        soundManager.ClickButtonSFX();
        info00.SetActive(false);
        info01.SetActive(true);
    }
    public void OnNext02Button()
    {
        soundManager.ClickButtonSFX();
        info01.SetActive(false);
        info02.SetActive(true);
    }

    public void OnBeforButton()
    {
        soundManager.ClickButtonSFX();
        info01.SetActive(false);
        info00.SetActive(true);
    }
    public void OnBefor02Button()
    {
        soundManager.ClickButtonSFX();
        info02.SetActive(false);
        info01.SetActive(true);
    }


    public void OnExitWindow()
    {
        soundManager.ClickButtonSFX();
        gamePlayInfo.SetActive(false);
        info00.SetActive(false);
        info01.SetActive(false);
        info02.SetActive(false);
        infoButton = false;
        Time.timeScale = 1f;


    }
}
