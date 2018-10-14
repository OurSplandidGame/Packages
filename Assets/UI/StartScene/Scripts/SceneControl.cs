using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public VideoPlayer StartVideo;
    public GameObject GameName;
    public GameObject StartPanel;
    public GameObject SettingPanel;
    public Toggle soundToggle;
    Animator anim;
    private bool isFirstTime;
    private AudioSource BackgroundMusic;

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        isFirstTime = true;
        GameName.SetActive(false);
        StartPanel.SetActive(false);
        SettingPanel.SetActive(false);
        BackgroundMusic = StartVideo.GetComponent<AudioSource>();
    }

    private void Update()
    {
        //1056 is ths total startVideo.frame
        if (isFirstTime && ((ulong)StartVideo.frame == 1020) || Input.GetMouseButton(0))
        {
            anim.SetTrigger("PopIn");
            GameName.SetActive(true);
            ActiveStartPanel();
            isFirstTime = false;
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ActiveStartPanel()
    {
        StartPanel.SetActive(true);
        SettingPanel.SetActive(false);
    }

    public void ActiveSettingPanel()
    {
        StartPanel.SetActive(false);
        SettingPanel.SetActive(true);
    }

    public void SwitchSound()
    {
        if (soundToggle.isOn) BackgroundMusic.Play();
         else BackgroundMusic.Pause();
    }
}
