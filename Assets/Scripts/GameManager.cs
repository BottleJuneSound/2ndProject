using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject BossZoneAlarm;

    static GameManager instance;

    public static GameManager Instance
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

    void Start()
    {
        BossZoneAlarm.SetActive(false);

    }

    void Update()
    {
        
    }

    public void AgainPressed()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("GameScene");
    }

    public void QuitPressed()
    {
        Application.Quit();
    }

    public void ExitPressed()
    {
        BossZoneAlarm.SetActive(false);
    }

}
