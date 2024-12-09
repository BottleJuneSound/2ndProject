using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public GameObject soundSystemPopup;
    public GameObject soundManager;
    public BossZone bossZone;
    public AudioMixer audioMixer;
    private float minVolume = -20f;
    private float maxVolume = 0;
    public Slider ambiSlider;
    public Slider sfxSlider;
    public Slider bgmSlider;
    public bool uiPop;
    public bool hasPlaySound = false;
    public bool changeAmbiFild = false;
    public bool changeAmbiBossZone = false;

    public AudioResource playerFootsteps;
    public AudioResource playerSlowSteps;
    public AudioResource getItem;
    public AudioResource spendMatch;
    public AudioResource spendOil;
    public AudioResource spendPotion;
    public AudioResource lightAttRattle;
    public AudioResource lightAtt;
    public AudioResource bossHit;
    public AudioResource bossDie;
    public AudioResource buttonClick;
    public AudioResource npcWalla;
    public AudioResource fildAmbi;
    public AudioResource bossZoneAmbi;

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
            || audioMixer.GetFloat("BGM", out currentVolume)) //시작시 페이더와 믹서의 초기값 설정
        {
            ambiSlider.value = MapVolumeToSliderValue(currentVolume);
            ambiSlider.value = 0.75f;

            sfxSlider.value = MapVolumeToSliderValue(currentVolume);
            sfxSlider.value = 0.75f;

            bgmSlider.value = MapVolumeToSliderValue(currentVolume);
            bgmSlider.value = 0.75f;
        }
    }

    public void Update()
    {
        if (soundManager.activeSelf && bossZone.playerCam.activeSelf)
        {
            if (hasPlaySound) return;

            if (!changeAmbiFild && !changeAmbiBossZone)
            {
                audioSource[6].volume -= Time.deltaTime;
                audioSource[6].volume = Mathf.Clamp(audioSource[6].volume, 0, 0.75f);
                changeAmbiFild = true;

                if(audioSource[6].volume == 0)
                {
                    changeAmbiBossZone = true;
                }

            }
            audioSource[6].volume += Time.deltaTime;
            audioSource[6].volume = Mathf.Clamp(audioSource[6].volume, 0, 0.75f);
            FildAmbi();

            if(audioSource[6].volume <= 0.75f)
            {
                hasPlaySound = true;
            }
        }
        
        if(soundManager.activeSelf && bossZone.bossFightCam.activeSelf)
        {
            if (hasPlaySound) return;

            if (changeAmbiFild && changeAmbiBossZone)
            {
                audioSource[6].volume -= Time.deltaTime;
                audioSource[6].volume = Mathf.Clamp(audioSource[6].volume, 0, 0.75f);
                changeAmbiFild = false;
                //changeAmbiBossZone = true;

                if (audioSource[6].volume == 0)
                {
                    changeAmbiBossZone = false;
                }
            }

            audioSource[6].volume += Time.deltaTime;
            audioSource[6].volume = Mathf.Clamp(audioSource[6].volume, 0, 0.75f);
            BossZoneAmbi();

            if (audioSource[6].volume <= 0.75f)
            {
                hasPlaySound = true;
            }
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


    public void FildAmbi()
    {
        audioSource[6].resource = fildAmbi;
        audioSource[6].Play();
    }

    public void BossZoneAmbi()
    {
        audioSource[6].resource = bossZoneAmbi;
        audioSource[6].Play();
    }

    public void PlayerFootstepsSFX()
    {
        audioSource[0].resource = playerFootsteps;
        audioSource[0].Play();
    }

    public void PlayerSlowStepSFX()
    {
        audioSource[0].resource = playerSlowSteps;
        audioSource[0].Play();
    }

    public void GetItemSFX()
    {
        audioSource[1].resource = getItem;
        audioSource[1].Play();
    }

    public void SpendMatcheSFX()
    {
        audioSource[1].resource = spendMatch;
        audioSource[1].Play();
    }

    public void SpendOilSFX()
    {
        audioSource[1].resource = spendOil;
        audioSource[1].Play();
    }

    public void SpendPotionSFX()
    {
        audioSource[1].resource = spendPotion;
        audioSource[1].Play();
    }

    public void LightAttRattleSFX()
    {
        audioSource[1].resource = lightAttRattle;
        audioSource[1].Play();
    }

    public void BossDieSFX()
    {
        audioSource[1].resource = bossDie;
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

    public void ClickButtonSFX()
    {
        audioSource[4].resource = buttonClick;
        audioSource[4].Play();
    }

    public void NPCWallaSFX()
    {
        audioSource[5].resource = npcWalla;
        audioSource[5].Play();
    }

    public void OnSoundSettingPopup()   //사운드 세팅 팝업 버튼 클릭
    {
        if (!uiPop)
        {
            if(soundSystemPopup.activeSelf) return;
            //Cursor.visible = true;
            ClickButtonSFX();
            uiPop = true;
            soundSystemPopup.SetActive(true);
            Time.timeScale = 0;

        }
        else if (uiPop)
        {
            if (!soundSystemPopup.activeSelf) return;
            ClickButtonSFX();
            uiPop = false;
            soundSystemPopup.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void AmbiFader(float sliderValue)
    {
        ClickButtonSFX();
        float volume = Mathf.Lerp(minVolume, maxVolume, sliderValue);
        audioMixer.SetFloat("Ambi", volume);
    }


    public void SFXFader(float sliderValue)
    {
        ClickButtonSFX();
        float volume = Mathf.Lerp (minVolume, maxVolume, sliderValue);
        audioMixer.SetFloat("SFX", volume);
    }

    public void BGMFader(float sliderValue)
    {
        ClickButtonSFX();
        float volume = Mathf.Lerp(minVolume, maxVolume, sliderValue);
        audioMixer.SetFloat("BGM", volume);
    }
}
