using TMPro;
using Unity.Burst.CompilerServices;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject pressPanel;
    public GameObject popupPanel;
    public GameObject loadM;
    public GameObject loadN;
    public GameObject loadB;
    public GameObject buttonM;
    public GameObject buttonN;
    public GameObject buttonB;
    public GameObject importAlarm;
    public Enemy boss;
    public CapsuleCollider lightAttackCollider;
    public ItemManager itemManager;
    public LightManager lightManager;
    public HPManager hpManager;
    public BossHPManager bossHP;

    public TMP_Text npcText;
    public TMP_Text interText;

    public Transform playerAngle;
    public StateMachine stateMachine;

    public float walkingSpeed = 2;
    public float rotationSpeed = 1;
    public float terminalSpeed = 50;
    public float verticalSpeed = 0;

    [SerializeField]
    CinemachineInputAxisController cineCam;
    InputAction moveAction;
    InputAction runAction;
    public InputAction skillMAction;
    public InputAction skillNAction;
    public InputAction skillBAction;
    InputAction closePopupAction;
    public InputAction interactiveAction;
    public InputAction lightAttackAction;
    public InputAction AddLightOilAction;
    public InputAction lightOnOffAction;
    public InputAction AddPotionSpendAction;

    public string oriText;
    public string currentText;
    public string interOriText;
    public string interCurrentText;

    public bool activeInteract = false;
    public bool onSkillM = false;
    public bool onSkillN = false;
    public bool onSkillB = false;
    public bool onClose = false;

    public bool skillActive = false;
    public bool lightAttackButton = false;
    public bool isAttack = false;
    CharacterController characterController;    //�̰� ����Ƽ �����. �������� ��ũ��Ʈ�ƴ� ��

    private void Awake()
    {
        stateMachine = new StateMachine(this);  //������ null���� �߻�

    }

    void Start()
    {
        lightAttackButton = false;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = true;
        // ���߿� Ŀ���̹����� �ٲ㺸��. ���� ��ȯ�� ���� ���콺�̱� ������ ���?

        Cursor.lockState = CursorLockMode.Confined;
        //lightAttackCollider.SetActive(false);

        lightAttackCollider.GetComponent<CapsuleCollider>().radius = 0;
        lightAttackCollider.GetComponent<CapsuleCollider>().height = 0;

        //actions�� InputActionAssetŸ�� �̴�.
        //��ǲ�ý����� ������ �����ϴ� ��������
        InputActionAsset inputActions = GetComponent<PlayerInput>().actions;

        moveAction = inputActions.FindAction("Move");
        runAction = inputActions.FindAction("Sprint");
        skillMAction = inputActions.FindAction("SkillM");
        skillNAction = inputActions.FindAction("SkillN");
        skillBAction = inputActions.FindAction("SkillB");
        interactiveAction = inputActions.FindAction("Interact");
        closePopupAction = inputActions.FindAction("Exit");
        lightAttackAction = inputActions.FindAction("Attack");
        AddLightOilAction = inputActions.FindAction("ReloadLight");
        lightOnOffAction = inputActions.FindAction("SpendMatchLightOnOff");
        AddPotionSpendAction = inputActions.FindAction("SpendPotion");

        characterController = GetComponent<CharacterController>();
        GetComponent<Animator>().SetTrigger("PlayerIdle");

        stateMachine.Initialize(stateMachine.idleState);
        oriText = npcText.text;
        interOriText = interText.text;
        importAlarm.SetActive(false);

        //if (itemManager == null)
        //{
        //    itemManager = FindObjectOfType<ItemManager>();
        //}

        if (cineCam == null)
        {
            cineCam = GetComponent<CinemachineInputAxisController>();
            cineCam.enabled = true;
            Debug.Log("���� �ó׸ӽ� null�ƴ�!");
        }

    }

    void Update()
    {
        if (hpManager.playerDeath)
        {
            lightAttackCollider.GetComponent<CapsuleCollider>().radius = 0;
            lightAttackCollider.GetComponent<CapsuleCollider>().height = 0;
            //lightAttackCollider.SetActive(true);
            moveAction.Disable();
            runAction.Disable();
            skillMAction.Disable();
            skillNAction.Disable();
            skillBAction.Disable();
            interactiveAction.Disable();
            closePopupAction.Disable();

            return;
        }

        //3D���� y�� Z�� �ݿ��ϴ� ���� �ڿ�������. xyz ��ġ �ٲ��ִ� ����
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);

        // ī�޶��� ������ ������ (ȸ���� ���� ����)
        Transform camTransform = Camera.main.transform;
        Vector3 camForward = camTransform.forward; // ī�޶��� �� ����
        Vector3 camRight = camTransform.right;    // ī�޶��� ������ ����. x�� ������ �Ǹ� �ڿ������� ������ �ȴ�.

        // y�� ���� 0���� �����Ͽ� ī�޶� ���� �������θ� �̵��ϵ��� ����
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // �Էµ� ������ ī�޶��� ȸ�� ���⿡ ���� ����
        Vector3 moveDir = (camRight * moveVector.x + camForward * moveVector.y).normalized;

        // �̵��� ������ ���� ��� (�̵� ���� ���) ĳ������ ȸ��
        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        }

        if (moveVector.magnitude > 1)
        {
            move.Normalize();
        }

        float currentSpeed = walkingSpeed;
        if (runAction.IsPressed())
        {
            currentSpeed = walkingSpeed * 3;
        }

        move = moveDir * currentSpeed * Time.deltaTime;

        if (!moveAction.IsPressed() && !isAttack)
        {
            OnIdle();
        }
        else if (moveAction.IsPressed() && !runAction.IsPressed())
        {
            OnWalk();
        }
        else if (moveAction.IsPressed() && runAction.IsPressed())
        {
            OnRun();
        }


        stateMachine.Execute();
        characterController.Move(move);



        if (interactiveAction.IsPressed() && pressPanel.activeSelf)
        {
            OnInteractive();
        }

        if (activeInteract)
        {
            if (skillMAction.IsPressed() && pressPanel.activeSelf)
            {
                if (skillActive) return;
                onSkillM = true;
                OnMedicine();

            }

            else if (skillNAction.IsPressed() && pressPanel.activeSelf)
            {
                if (skillActive) return;
                onSkillN = true;
                OnPray();
            }

            else if (skillBAction.IsPressed() && pressPanel.activeSelf)
            {
                if (skillActive) return;
                onSkillB = true;
                OnBloodWithdrawal();
            }

            else if (closePopupAction.IsPressed() && pressPanel.activeSelf)
            {
                ClosePopup();
                onClose = true;
            }

        }

        //��ȣ�ۿ� Text ���뺯���ϴ� ������Ʈ
        if (npcText.gameObject.activeSelf && !skillActive)
        {
            if (onSkillM)
            {
                skillActive = true;
                currentText = "�๰ġ�Ḧ �����Ͽ����ϴ�.";
                npcText.text = currentText;

                //GetItemPopup();
                Invoke("ResetAllSkill", 3f);
            }
            else if (onSkillN)
            {
                skillActive = true;
                currentText = "�⵵�� �����Ͽ����ϴ�.";
                npcText.text = currentText;

                //GetItemPopup();
                Invoke("ResetAllSkill", 3f);
            }
            else if (onSkillB)
            {
                skillActive = true;
                currentText = "����ġ�Ḧ �����Ͽ����ϴ�.";
                npcText.text = currentText;

                //GetItemPopup();
                Invoke("ResetAllSkill", 3f);
            }

            //else
            //{
            //    npcText.text = oriText;
            //}
        }

        if (lightAttackAction.WasPressedThisFrame() && !isAttack && itemManager.matcheCounter >= 0)
        {
            //Debug.Log("��ȯ��");
            //if (lightAttackButton) return;
            //Debug.Log(itemManager.lightCounter);
            isAttack = true;
            lightAttackButton = true;
            //Debug.Log("�۵���");
            LightAttack();
        }
        //else if(lightAttackAction.WasPressedThisFrame() && !isAttack && itemManager.matcheCounter >= 0 && lightManager.lightOff)
        //{
        //    isAttack = true;
        //    lightAttackButton = true;
        //}
        else if (lightAttackAction.WasReleasedThisFrame() || itemManager.matcheCounter < 0)
        {
            isAttack = false;
            lightAttackButton = false;
            boss.playerLight.intensity = 50;
            EndLightAttack();
        }


        ////���콺 �� ������
        //Vector2 lookDeep = cameraAction.ReadValue<Vector2>();
        //float zoom = lookDeep.y * mouseScroll;
        //cameraDeep += zoom;
        //cameraDeep = Mathf.Clamp(cameraDeep, -40f, -0.7f);

        //Vector3 currentDeep = cameraObject.localPosition;
        //currentDeep.z = cameraDeep;
        //cameraObject.localPosition = currentDeep;

    }

    public void GetItemPopup() 
        //�÷��̾� ��Ʈ�ѷ����� �����ϸ� ���α׷��� ���δ�.
        //�����۸Ŵ������� ī���Ͱ� ����� ���� �ݿ��Ͽ� �����ذ�
    {
        if (itemManager.lightCounter == itemManager.beforLightCounter + 1)
        {
            importAlarm.SetActive(true);
        }

        if (itemManager.matcheCounter == itemManager.beforMatcheCounter + 1)
        {
            importAlarm.SetActive(true);
        }

        if (itemManager.potionCounter == itemManager.beforPotionCounter + 1)
        {
            importAlarm.SetActive(true);
        }
    }

    public void ResetAllSkill() //��ų ���� �ʱ�ȭ
    {
        if (skillActive)
        {
            onSkillM = false;
            onSkillN = false;
            onSkillB = false;
            loadM.gameObject.SetActive(false);
            loadN.gameObject.SetActive(false);
            loadB.gameObject.SetActive(false);
            importAlarm.SetActive(false);

            skillActive = false;
            npcText.text = oriText;
        }


    }

    public void LightAttack()   //�������� Ű �Է��� boolŸ������ ó���� 
    {
        if (lightAttackButton)
        {
            if (lightManager.lightOff)
            {
                lightAttackCollider.GetComponent<CapsuleCollider>().radius = 0;
                lightAttackCollider.GetComponent<CapsuleCollider>().height = 0;
                //lightAttackCollider.SetActive(true);
                moveAction.Disable();
                runAction.Disable();
                skillMAction.Disable();
                skillNAction.Disable();
                skillBAction.Disable();
                interactiveAction.Disable();
                closePopupAction.Disable();
                OnLightAttack();
            }
            
            if (!lightManager.lightOff)
            {
                lightAttackCollider.GetComponent<CapsuleCollider>().radius = 0.3f;
                lightAttackCollider.GetComponent<CapsuleCollider>().height = 2.5f;
                //lightAttackCollider.SetActive(true);
                moveAction.Disable();
                runAction.Disable();
                skillMAction.Disable();
                skillNAction.Disable();
                skillBAction.Disable();
                interactiveAction.Disable();
                closePopupAction.Disable();
                OnLightAttack();

                //Invoke("EndLightAttack", 3f);
            }


        }
    }

    public void EndLightAttack()
    {
        lightAttackCollider.GetComponent<CapsuleCollider>().radius = 0;
        lightAttackCollider.GetComponent<CapsuleCollider>().height = 0;
        //lightAttackCollider.SetActive(false);
        moveAction.Enable();
        runAction.Enable();
        skillMAction.Enable();
        skillNAction.Enable();
        skillBAction.Enable();
        interactiveAction.Enable();
        closePopupAction.Enable();
        OnIdle();
    }

    public void OnPlayerDeath()
    {
        stateMachine.TransitionTo(stateMachine.playerDeathState);
    }
    public void OnLightAttackEnd()
    {
        stateMachine.TransitionTo(stateMachine.lightAttackEnd);
    }
    public void OnLightAttack()
    {
        stateMachine.TransitionTo(stateMachine.lightFindState);
    }
    public void OnIdle()
    {
        stateMachine.TransitionTo(stateMachine.idleState);
    }
    public void OnWalk()
    {
        stateMachine.TransitionTo(stateMachine.walkState);

    }
    public void OnRun()
    {
        stateMachine.TransitionTo(stateMachine.runState);
    }

    public void OnBloodWithdrawal() // ��ȣ�ۿ� ��ų ����, ��ȣ�ۿ� ���ۺκ����� ���ƿ�.
    {
        loadB.gameObject.SetActive(true);
        //Debug.Log("����ġ�� ����!");
        OnInteractive();
        //activeInteract = false;

    }

    public void OnMedicine()
    {
        loadM.gameObject.SetActive(true);
        //Debug.Log("�๰ġ�� ����!");
        OnInteractive();
        //activeInteract = false;
    }

    public void OnPray()
    {
        loadN.gameObject.SetActive(true);
        //Debug.Log("�⵵ġ�� ����!");
        OnInteractive();
        //activeInteract = false;

    }

    public void ClosePopup()    // â�� ��Ȱ��ȭ �ϰ� ���ͷ�Ƽ�� ���� �޼��� ����
    {
        if (onClose == true) return;
        popupPanel.SetActive(false);
        OffInteractive();

    }

    public void OnInteractive()
    {
        if (activeInteract == true) return;
        if (gameObject.tag == "ClearNPC") return;

        //Debug.Log("��ȣ�ۿ��� �����մϴ�.");
        lightAttackAction.Disable();
        activeInteract = true;
        cineCam.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void OffInteractive()
    {
        if (activeInteract == false) return;

        //Debug.Log("��ȣ�ۿ��� ����Ǿ����ϴ�.");
        lightAttackAction.Enable();
        if (interactiveAction.IsPressed()) activeInteract = false;
        cineCam.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        onClose = false;

    }

    public void ReTryGetItemPopup() //ȯ�� ��ȣ�ۿ뿡�� ���� ���� �� ���� �˾� text����
    {
        skillMAction.Disable();
        skillNAction.Disable();
        skillBAction.Disable();

        currentText = "�� ȯ�ڴ� �̹� �����ߴ� ȯ�ڴ�. �ٸ� ȯ�ڸ� ã�ƺ���..";
        npcText.text = currentText;
    }

    private void OnTriggerEnter(Collider playerTrigger)
    {
        if (playerTrigger.gameObject.tag == "NPC")
        {
            interText.text = interOriText;
            npcText.text = oriText;
            npcText.gameObject.SetActive(true);
        }

        if (playerTrigger.gameObject.tag == "ClearNPC")   //  ��ȣ�ۿ��� ��ģ npc�� �±׸� ��ü�ϰ� �ش� �κ��� �����Ų��.
        {
            //Debug.Log("����� �±� �۵��Ǵ��� Ȯ����");
            npcText.gameObject.SetActive(true);
            ReTryGetItemPopup();
        }

        if (playerTrigger.gameObject.tag == "BossZoneInfo" && bossHP.currentActiveIndex <= 4)
        {
            currentText = "����� ���� ������ ����ϱ⿡ ����� �غ���� �ʾҽ��ϴ�. \n������ ���� �ù��� ġ���Ͽ� ������ ���ݿ� ����ϼ���.";
            npcText.text = currentText;
            npcText.gameObject.SetActive(true);
        }

        if (playerTrigger.gameObject.tag == "BossZoneInfo" && bossHP.currentActiveIndex > 4)
        {
            currentText = "������ ȯ�ڸ� ġ���� ����� ���Ѻ��� �ֽ��ϴ�. \n������ óġ�Ͽ� ������ ���ϼ���";
            npcText.text = currentText;
            npcText.gameObject.SetActive(true);
        }

    }

    private void OnTriggerStay(Collider playerTrigger)
    {
        if(playerTrigger.gameObject.tag == "Boss")
        {
            hpManager.LossHealth();
        }

    }

    private void OnTriggerExit(Collider playerTrigger)
    {
        if (playerTrigger.gameObject.tag == "NPC")
        {
            npcText.gameObject.SetActive(false);
        }

        if (playerTrigger.gameObject.tag == "ClearNPC") 
        {
            npcText.gameObject.SetActive(false);
            
            skillMAction.Enable();
            skillNAction.Enable();
            skillBAction.Enable();
        }
        if (playerTrigger.gameObject.tag == "Boss")
        {
            hpManager.hpAlarm.enabled = false;
        }

        if (playerTrigger.gameObject.tag == "BossZoneInfo")
        {
            npcText.gameObject.SetActive(false);
        }
    }




}
