using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public GameObject soundSystemPopup;
    public AudioMixer audioMixer;
    private float minVolume = -20f;
    private float maxVolume = 0;
    public Slider ambiSlider;
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
        float currentVolume;
        if (audioMixer.GetFloat("Ambi", out currentVolume)) //시작시 페이더와 믹서의 초기값 설정
        {
            ambiSlider.value = MapVolumeToSliderValue(currentVolume);
            ambiSlider.value = 0.75f;
        }
    }

    private float MapVolumeToSliderValue(float volume)  //db를 슬라이더 값으로 변환
    {
        return Mathf.InverseLerp(minVolume, maxVolume, volume);
    }

    private float MapSliderValueToVolume(float sliderValue) //슬라이더 값을 db로 변환
    {
        return Mathf.Lerp(minVolume, maxVolume, sliderValue);
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

    public void AmbiFader(float sliderValue)
    {
        float volume = Mathf.Lerp(minVolume, maxVolume, sliderValue);
        audioMixer.SetFloat("Ambi", volume);
    }


    public void SFXFader()
    {

    }

    public void BGMFader()
    {

    }
}
