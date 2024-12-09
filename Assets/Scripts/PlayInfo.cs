using UnityEngine;

public class PlayInfo : MonoBehaviour
{
    public GameObject gamePlayInfo;
    public GameObject exitButton;
    public GameObject info00;
    public GameObject info01;
    public GameObject info02;
    void Start()
    {
        gamePlayInfo.SetActive(false);
        info00.SetActive(false);
        info01.SetActive(false);
        info02.SetActive(false);
        OnInfoButton();
    }

    void Update()
    {

    }

    public void OnInfoButton()
    {
        if (gameObject.activeSelf) return;

        gamePlayInfo.SetActive(true);
        info00.SetActive(true);
    }

    public void OnNextButton()
    {
        info00.SetActive(false);
        info01.SetActive(true);
    }
    public void OnNext02Button()
    {
        info01.SetActive(false);
        info02.SetActive(true);
    }

    public void OnBeforButton()
    {
        info01.SetActive(false);
        info00.SetActive(true);
    }
    public void OnBefor02Button()
    {
        info02.SetActive(false);
        info01.SetActive(true);
    }


    public void OnExitWindow()
    {
        gamePlayInfo.SetActive(false);
        info00.SetActive(false);
        info01.SetActive(false);
        info02.SetActive(false);
    }
}
