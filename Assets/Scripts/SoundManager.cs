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
    public Slider sfxSlider;
    public Slider bgmSlider;
    public bool uiPop;

    public AudioResource playerFootsteps;
    //public AudioResource playerEquip;
    public AudioResource getItem;
    public AudioResource lightAtt;
    public AudioResource bossHit;

    public AudioSource[] audioSource;


    
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
        if (audioMixer.GetFloat("Ambi", out currentVolume) 
            || audioMixer.GetFloat("SFX", out currentVolume) 
            || audioMixer.GetFloat("BGM", out currentVolume)) //���۽� ���̴��� �ͼ��� �ʱⰪ ����
        {
            ambiSlider.value = MapVolumeToSliderValue(currentVolume);
            ambiSlider.value = 0.75f;

            sfxSlider.value = MapVolumeToSliderValue(currentVolume);
            sfxSlider.value = 0.75f;

            bgmSlider.value = MapVolumeToSliderValue(currentVolume);
            bgmSlider.value = 0.75f;
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


    public void PlayerFootstepsSFX()
    {
        audioSource[0].resource = playerFootsteps;
        audioSource[0].Play();
    }

    public void GetItemSFX()
    {
        audioSource[1].resource = getItem;
        audioSource[1].Play();
    }

    public void LightAttackSFX()
    {
        audioSource[2].resource = lightAtt;
        audioSource[2].Play();
    }

    public void BossHitSFX()
    {
        audioSource[3].resource = bossHit;
        audioSource[3].Play();
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


    public void SFXFader(float sliderValue)
    {
        float volume = Mathf.Lerp (minVolume, maxVolume, sliderValue);
        audioMixer.SetFloat("SFX", volume);
    }

    public void BGMFader(float sliderValue)
    {
        float volume = Mathf.Lerp(minVolume, maxVolume, sliderValue);
        audioMixer.SetFloat("BGM", volume);
    }
}
