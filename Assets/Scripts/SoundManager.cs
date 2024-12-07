using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject soundSystemPopup;
    //public PlayerController player;

    public bool uiPop;
    
    static SoundManager instance;

    static public SoundManager Instance
    {
        get
        {
            return instance;
        }
    }

    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        uiPop = false;
        soundSystemPopup.SetActive(false);

    }

    public void OnSoundSettingPopup()   //사운드 세팅 팝업 버튼 클릭
    {
        if (!uiPop)
        {
            //Cursor.visible = true;
            uiPop = true;
            soundSystemPopup.SetActive(true);
            Time.timeScale = 0;

        }
        else if (uiPop)
        {
            uiPop = false;
            soundSystemPopup.SetActive(false);
            Time.timeScale = 1f;
        }

    }

    public void AmbiFader()
    {

    }

    public void SFXFader()
    {

    }

    public void BGMFader()
    {

    }
}
