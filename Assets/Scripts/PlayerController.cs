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
    CharacterController characterController;    //이거 유니티 기능임. 내가만든 스크립트아님 ㅠ

    private void Awake()
    {
        stateMachine = new StateMachine(this);  //없으면 null에러 발생

    }

    void Start()
    {
        lightAttackButton = false;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = true;
        // 나중에 커서이미지로 바꿔보자. 시점 변환을 위한 마우스이기 때문에 없어도?

        Cursor.lockState = CursorLockMode.Confined;
        //lightAttackCollider.SetActive(false);

        lightAttackCollider.GetComponent<CapsuleCollider>().radius = 0;
        lightAttackCollider.GetComponent<CapsuleCollider>().height = 0;

        //actions는 InputActionAsset타입 이다.
        //인풋시스템의 동작을 관리하는 변수명임
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
            Debug.Log("이제 시네머신 null아님!");
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

        //3D에서 y는 Z에 반영하는 것이 자연스러움. xyz 위치 바꿔주는 내용
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);

        // 카메라의 방향을 가져옴 (회전을 위한 기준)
        Transform camTransform = Camera.main.transform;
        Vector3 camForward = camTransform.forward; // 카메라의 앞 방향
        Vector3 camRight = camTransform.right;    // 카메라의 오른쪽 방향. x가 음수가 되면 자연스럽게 왼쪽이 된다.

        // y축 값을 0으로 설정하여 카메라가 수평 방향으로만 이동하도록 설정
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // 입력된 방향을 카메라의 회전 방향에 맞춰 조정
        Vector3 moveDir = (camRight * moveVector.x + camForward * moveVector.y).normalized;

        // 이동할 방향이 있을 경우 (이동 중일 경우) 캐릭터의 회전
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

        //상호작용 Text 내용변경하는 업데이트
        if (npcText.gameObject.activeSelf && !skillActive)
        {
            if (onSkillM)
            {
                skillActive = true;
                currentText = "약물치료를 시행하였습니다.";
                npcText.text = currentText;

                //GetItemPopup();
                Invoke("ResetAllSkill", 3f);
            }
            else if (onSkillN)
            {
                skillActive = true;
                currentText = "기도를 시행하였습니다.";
                npcText.text = currentText;

                //GetItemPopup();
                Invoke("ResetAllSkill", 3f);
            }
            else if (onSkillB)
            {
                skillActive = true;
                currentText = "방혈치료를 시행하였습니다.";
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
            //Debug.Log("반환중");
            //if (lightAttackButton) return;
            //Debug.Log(itemManager.lightCounter);
            isAttack = true;
            lightAttackButton = true;
            //Debug.Log("작동중");
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


        ////마우스 휠 움직임
        //Vector2 lookDeep = cameraAction.ReadValue<Vector2>();
        //float zoom = lookDeep.y * mouseScroll;
        //cameraDeep += zoom;
        //cameraDeep = Mathf.Clamp(cameraDeep, -40f, -0.7f);

        //Vector3 currentDeep = cameraObject.localPosition;
        //currentDeep.z = cameraDeep;
        //cameraObject.localPosition = currentDeep;

    }

    public void GetItemPopup() 
        //플레이어 컨트롤러에서 적용하면 프로그램이 꼬인다.
        //아이템매니저에서 카운터가 변경된 직후 반영하여 오류해결
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

    public void ResetAllSkill() //스킬 상태 초기화
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

    public void LightAttack()   //지속적인 키 입력은 bool타입으로 처리함 
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

    public void OnBloodWithdrawal() // 상호작용 스킬 사용시, 상호작용 시작부분으로 돌아옴.
    {
        loadB.gameObject.SetActive(true);
        //Debug.Log("방혈치료 시행!");
        OnInteractive();
        //activeInteract = false;

    }

    public void OnMedicine()
    {
        loadM.gameObject.SetActive(true);
        //Debug.Log("약물치료 시행!");
        OnInteractive();
        //activeInteract = false;
    }

    public void OnPray()
    {
        loadN.gameObject.SetActive(true);
        //Debug.Log("기도치료 시행!");
        OnInteractive();
        //activeInteract = false;

    }

    public void ClosePopup()    // 창을 비활성화 하고 인터렉티브 해제 메서드 실행
    {
        if (onClose == true) return;
        popupPanel.SetActive(false);
        OffInteractive();

    }

    public void OnInteractive()
    {
        if (activeInteract == true) return;
        if (gameObject.tag == "ClearNPC") return;

        //Debug.Log("상호작용을 시작합니다.");
        lightAttackAction.Disable();
        activeInteract = true;
        cineCam.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void OffInteractive()
    {
        if (activeInteract == false) return;

        //Debug.Log("상호작용이 종료되었습니다.");
        lightAttackAction.Enable();
        if (interactiveAction.IsPressed()) activeInteract = false;
        cineCam.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        onClose = false;

    }

    public void ReTryGetItemPopup() //환자 상호작용에서 정답 맞춘 후 띄우는 팝업 text변경
    {
        skillMAction.Disable();
        skillNAction.Disable();
        skillBAction.Disable();

        currentText = "이 환자는 이미 진료했던 환자다. 다른 환자를 찾아보자..";
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

        if (playerTrigger.gameObject.tag == "ClearNPC")   //  상호작용을 마친 npc는 태그를 교체하고 해당 부분을 실행시킨다.
        {
            //Debug.Log("변경된 태그 작동되는지 확인중");
            npcText.gameObject.SetActive(true);
            ReTryGetItemPopup();
        }

        if (playerTrigger.gameObject.tag == "BossZoneInfo" && bossHP.currentActiveIndex <= 4)
        {
            currentText = "당신은 아직 보스를 상대하기에 충분히 준비되지 않았습니다. \n마을의 병든 시민을 치료하여 보스의 공격에 대비하세요.";
            npcText.text = currentText;
            npcText.gameObject.SetActive(true);
        }

        if (playerTrigger.gameObject.tag == "BossZoneInfo" && bossHP.currentActiveIndex > 4)
        {
            currentText = "보스가 환자를 치료한 당신을 지켜보고 있습니다. \n보스를 처치하여 마을을 구하세요";
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
