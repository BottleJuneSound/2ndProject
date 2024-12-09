using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class BossZone : MonoBehaviour
{
    public GameObject bossZone;
    public GameObject bossFightCam;
    public CinemachineCamera sideCam;
    public GameObject playerCam;
    public CinemachineCamera thirdCam;
    public GameObject player;
    public GameObject boss;
    public GameObject globalVol;
    public HPManager hpManager;
    public BossHPManager bossHPManager;
    public CinemachineRotationComposer riseComposer;
    public CinemachineInputAxisController cineInput;
    public SoundManager soundManager;

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
            soundManager.hasPlaySound = false;
            playerCam.SetActive(false);
            bossFightCam.SetActive(true);
            boss.GetComponent<NavMeshAgent>().enabled = true;
            boss.SetActive(true);
        }


    }

    public void OnTriggerStay(Collider bossBox)     //����, �÷��̾� �¸����ǿ� ���� ī�޶� ��������
    {

        if (bossBox.gameObject.tag == "Player" && hpManager.playerDeath)    //�÷��̾ �������
        {
            bossFightCam.SetActive(false);
            playerCam.SetActive(true);
            thirdCam.Target.TrackingTarget = boss.transform;
            thirdCam.Target.LookAtTarget = boss.transform;
            riseComposer.Composition.ScreenPosition.y = 1.5f;
            cineInput.enabled = false;  //�ó׸ӽ� ��ǲ�� ������ ���� ���� ��
            
        }

        if (bossBox.gameObject.tag == "Player" && bossHPManager.currentActiveIndex == 0)    //�÷��̾ �̰��� ���
        {
            bossFightCam.SetActive(false);
            playerCam.SetActive(true);
            thirdCam.Target.TrackingTarget = player.transform;
            thirdCam.Target.LookAtTarget = player.transform;
        }


    }


    public void OnTriggerExit(Collider bossBox)
    {
        if (bossBox.gameObject.tag == "Player")
        {
            soundManager.hasPlaySound = false;
            playerCam.SetActive(true);
            bossFightCam.SetActive(false);
            boss.GetComponent<NavMeshAgent>().enabled = false;
            boss.SetActive(false);
        }
    }
}
