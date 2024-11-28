using UnityEngine;
using UnityEngine.AI;

public class BossZone : MonoBehaviour
{
    public GameObject bossZone;
    public GameObject bossFightCam;
    public GameObject playerCam;
    public GameObject boss;

    void Start()
    {
     boss.SetActive(false);


    }

    void Update()
    {

    }

    public void OnTriggerEnter(Collider bossBox)
    {
        if(bossBox.gameObject.tag == "Player")
        {
            playerCam.SetActive(false);
            bossFightCam.SetActive(true);
            boss.GetComponent<NavMeshAgent>().enabled = true;
            boss.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider bossBox)
    {
        if (bossBox.gameObject.tag == "Player")
        {
            playerCam.SetActive(true);
            bossFightCam.SetActive(false);
            boss.GetComponent<NavMeshAgent>().enabled = false;
            boss.SetActive(false);
        }
    }
}
