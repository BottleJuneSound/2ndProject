using TMPro;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public PlayerController player;
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

        lightCount.text = "5";
        matcheCount.text = "0";
        potionCount.text = "0";

        lightCounter = 5;
        
        beforLightCounter = 0;
        beforMatcheCounter = 0;
        beforPotionCounter = 0;
        
        //Debug.Log(lightCounter);
    }

    void Update()
    {
        //왜계속 0으로 초기화될까?
        //Debug.Log("beforLightCounter " + beforLightCounter);
        //Debug.Log("beforMatcheCounter " + beforMatcheCounter);
        //Debug.Log("beforPotionCounter " + beforPotionCounter);
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

        currentImportAlarm = "등불 아이템 사용 횟수를 습득하였습니다.";
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
    }

    public void OnSpendMatche() // 버튼 입력에 할당 할 성냥사용 활성화 함수 
    {

    }

    public void OnSpendPotion()
    {

    }

}
