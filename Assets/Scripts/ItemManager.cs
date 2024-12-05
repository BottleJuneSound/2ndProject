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
        //npc�� ���ο� ������ ���� Ȯ��.
        //������ �����ۿ� ���߾� �Ʒ� �޼��� ����
    }

    public void GetLightItem()
    {
        beforLightCounter = lightCounter;

        int currentLightCount = int.Parse(lightCount.text);
        currentLightCount++;
        lightCounter = currentLightCount;
        lightCount.text = currentLightCount.ToString();

        currentImportAlarm = "��� ���� ���� Ƚ���� �����Ͽ����ϴ�.";
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

        currentImportAlarm = "���� �������� �����Ͽ����ϴ�.";
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

        currentImportAlarm = "ü�� �������� �����Ͽ����ϴ�.";
        importAlarm.text = currentImportAlarm;

        player.GetItemPopup();

    }


    public void OnSpendLight()  // ��ư �Է¿� �Ҵ��� ����Ʈ��� Ȱ��ȭ �Լ�
    {
        int currentLightCount = int.Parse(lightCount.text);
        currentLightCount--;

        lightCounter = currentLightCount;
        lightCount.text = currentLightCount.ToString();

        if (lightCounter < 0) 
        {
            Debug.Log("����Ʈ ���Ƚ���� �ٴڳ���!"); 
            return;
        }
    }

    public void OnSpendMatche() // ��ư �Է¿� �Ҵ� �� ���ɻ�� Ȱ��ȭ �Լ� 
    {

        int currentMatcheCount = int.Parse(matcheCount.text);

        currentMatcheCount--;
        currentMatcheCount = Mathf.Clamp(currentMatcheCount, 0, 50);
        matcheCounter = Mathf.Clamp(matcheCounter, 0, 50);

        matcheCounter = currentMatcheCount;
        matcheCount.text = currentMatcheCount.ToString();

        if (matcheCounter <= 0)
        {
            Debug.Log("���� ���Ƚ���� �ٴڳ���!");
            Debug.Log("currentMatcheCount " + currentMatcheCount);
            Debug.Log("matcheCounter " + matcheCounter);

            return;
        }
    }

    public void OnSpendPotion()
    {
        int currentPotionCount = int.Parse(potionCount.text); 
        currentPotionCount--;

        potionCounter = currentPotionCount;
        potionCount.text= currentPotionCount.ToString();

        if (potionCounter < 0)
        {
            Debug.Log("���� ���Ƚ���� �ٴڳ���!");
            return;
        }
    }

}
