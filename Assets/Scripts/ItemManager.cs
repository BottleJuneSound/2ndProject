using TMPro;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public PlayerController player;
    public SoundManager soundManager;
    public TMP_Text lightCount;
    public TMP_Text matcheCount;
    public TMP_Text potionCount;
    
    public int lightCounter;
    public int matcheCounter;
    public int potionCounter;
    public int beforLightCounter;
    public int beforMatcheCounter;
    public float beforPotionCounter;

    public TMP_Text importAlarm;
    public string currentImportAlarm;

    static ItemManager instance;

    static public ItemManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }

    void Start()
    {
        if (player == null)
        {
            player = GetComponent<PlayerController>();
        }

        lightCount.text = "2";
        matcheCount.text = "2";
        potionCount.text = "2";
        
        beforLightCounter = 0;
        beforMatcheCounter = 0;
        beforPotionCounter = 0;

        lightCounter = 2;
        matcheCounter = 2;
        potionCounter = 2;
        //Debug.Log(lightCounter);
    }

    void Update()
    {

    }

    public void GetNewItem()
    {
        //npc의 새로운 아이템 생성 확인.
        //생성된 아이템에 맞추어 아래 메서드 실행
    }

    public void GetLightItem()
    {
        beforLightCounter = lightCounter;

        int currentLightCount = int.Parse(lightCount.text);
        currentLightCount++;
        lightCounter = currentLightCount;
        lightCount.text = currentLightCount.ToString();

        currentImportAlarm = "등불 오일 충전 횟수를 습득하였습니다.";
        importAlarm.text = currentImportAlarm;

        player.GetItemPopup();

    }
    public void GetMatcheItem()
    {
        beforMatcheCounter = matcheCounter;

        int currentMatcheCount = int.Parse(matcheCount.text);
        currentMatcheCount++;
        matcheCounter = currentMatcheCount;
        matcheCount.text = currentMatcheCount.ToString();

        currentImportAlarm = "성냥 아이템을 습득하였습니다.";
        importAlarm.text = currentImportAlarm;

        player.GetItemPopup();

    }

    public void GetPotionItem()
    {
        beforPotionCounter = potionCounter;

        int currentPotionCount = int.Parse(potionCount.text);
        currentPotionCount++;
        potionCounter = currentPotionCount;
        potionCount.text = currentPotionCount.ToString();

        currentImportAlarm = "체력 아이템을 습득하였습니다.";
        importAlarm.text = currentImportAlarm;

        player.GetItemPopup();

    }


    public void OnSpendLight()  // 버튼 입력에 할당할 라이트사용 활성화 함수
    {
        int currentLightCount = int.Parse(lightCount.text);
        currentLightCount--;

        lightCounter = currentLightCount;
        lightCount.text = currentLightCount.ToString();

        if (lightCounter < 0) 
        {
            Debug.Log("라이트 사용횟수가 바닥났다!"); 
            return;
        }
        soundManager.SpendOilSFX();
    }

    public void OnSpendMatche() // 버튼 입력에 할당 할 성냥사용 활성화 함수 
    {

        int currentMatcheCount = int.Parse(matcheCount.text);

        currentMatcheCount--;
        currentMatcheCount = Mathf.Clamp(currentMatcheCount, 0, 50);
        matcheCounter = Mathf.Clamp(matcheCounter, 0, 50);

        matcheCounter = currentMatcheCount;
        matcheCount.text = currentMatcheCount.ToString();

        if (matcheCounter <= 0)
        {
            Debug.Log("성냥 사용횟수가 바닥났다!");
            Debug.Log("currentMatcheCount " + currentMatcheCount);
            Debug.Log("matcheCounter " + matcheCounter);

            return;
        }
        soundManager.SpendMatcheSFX();

    }

    public void OnSpendPotion()
    {
        int currentPotionCount = int.Parse(potionCount.text); 
        currentPotionCount--;

        potionCounter = currentPotionCount;
        potionCount.text= currentPotionCount.ToString();

        if (potionCounter < 0)
        {
            Debug.Log("포션 사용횟수가 바닥났다!");
            return;
        }
        soundManager.SpendPotionSFX();

    }

}
