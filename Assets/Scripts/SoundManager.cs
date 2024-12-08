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
        if (audioMixer.GetFloat("Ambi", out currentVolume)) //���۽� ���̴��� �ͼ��� �ʱⰪ ����
        {
            ambiSlider.value = MapVolumeToSliderValue(currentVolume);
            ambiSlider.value = 0.75f;
        }
    }

    private float MapVolumeToSliderValue(float volume)  //db�� �����̴� ������ ��ȯ
    {
        return Mathf.InverseLerp(minVolume, maxVolume, volume);
    }

    private float MapSliderValueToVolume(float sliderValue) //�����̴� ���� db�� ��ȯ
    {
        return Mathf.Lerp(minVolume, maxVolume, sliderValue);
    }

    public void OnSoundSettingPopup()   //���� ���� �˾� ��ư Ŭ��
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
