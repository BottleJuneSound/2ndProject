using TMPro;
using Unity.Burst.CompilerServices;
using Unity.Cinemachine;
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

    public TMP_Text npcText;

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

    public string oriText;
    public string currentText;

    public bool activeInteract = false;
    public bool onSkillM = false;
    public bool onSkillN = false;
    public bool onSkillB = false;
    //public bool onSkillFail = false;

    public bool skillActive = false;
    CharacterController characterController;    //이거 유니티 기능임. 내가만든 스크립트아님 ㅠ

    private void Awake()
    {
        stateMachine = new StateMachine(this);  //없으면 null에러 발생

    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // 나중에 커서이미지로 바꿔보자. 시점 변환을 위한 마우스이기 때문에 없어도?
        Cursor.visible = true;

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

        characterController = GetComponent<CharacterController>();
        stateMachine.Initialize(stateMachine.idleState);
        oriText = npcText.text;

        if (cineCam == null)
        {
            cineCam = GetComponent<CinemachineInputAxisController>();
            cineCam.enabled = true;
            Debug.Log("이제 시네머신 null아님!");
        }

    }

    void Update()
    {

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

        if (!moveAction.IsPressed())
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
                if(skillActive) return;
                onSkillM = true;
                OnMedicine();

            }

            if (skillNAction.IsPressed() && pressPanel.activeSelf)
            {
                if (skillActive) return;
                onSkillN = true;
                OnPray();
            }

            if (skillBAction.IsPressed() && pressPanel.activeSelf)
            {
                if (skillActive) return;
                onSkillB = true;
                OnBloodWithdrawal();
            }

            if(closePopupAction.IsPressed() && pressPanel.activeSelf)
            {
                ClosePopup();
            }

        }

        //상호작용 Text 내용변경하는 업데이트
        if (npcText.gameObject.activeSelf && !skillActive)
        {
            if (onSkillM)
            {
                //buttonM.GetComponent<Image>().color = Color.clear / Time.deltaTime;       // 점진전으로 a값 증가 시킬 수 있는 방법 찾기
                skillActive = true;
                currentText = "약물치료를 시행하였습니다.";
                npcText.text = currentText;
                Invoke("ResetAllSkill", 3f);
            }
            else if (onSkillN)
            {
                skillActive = true;
                currentText = "기도를 시행하였습니다.";
                npcText.text = currentText;
                Invoke("ResetAllSkill", 3f);
            }
            else if (onSkillB)
            {
                skillActive = true;
                currentText = "방혈치료를 시행하였습니다.";
                npcText.text = currentText;
                Invoke("ResetAllSkill", 3f);
            }
            //else if (onSkillFail)
            //{
            //    skillActive = true;
            //    currentText = "환자가 혼란스러워합니다.";
            //    npcText.text = currentText;
            //    Invoke("ResetAllSkill", 3f);
            //}
            else
            {
                npcText.text = oriText;
            }
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

    public void ResetAllSkill() //스킬 상태 초기화
    {
        if (skillActive)
        {
            onSkillM = false;
            onSkillN = false;
            onSkillB = false;
            //onSkillFail = false;
            loadM.gameObject.SetActive(false);
            loadN.gameObject.SetActive(false);
            loadB.gameObject.SetActive(false);




            skillActive = false;
            npcText.text = oriText;
        }


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
        Debug.Log("방혈치료 시행!");
        OnInteractive();
        //activeInteract = false;

    }

    public void OnMedicine()
    {
        loadM.gameObject.SetActive(true);
        Debug.Log("약물치료 시행!");
        OnInteractive();
        //activeInteract = false;
    }

    public void OnPray()
    {
        loadN.gameObject.SetActive(true);
        Debug.Log("기도치료 시행!");
        OnInteractive();
        //activeInteract = false;

    }

    public void ClosePopup()    // 창을 비활성화 하고 인터렉티브 해제 메서드 실행
    {
        popupPanel.SetActive(false);
        OffInteractive();

    }

    public void OnInteractive()
    {
        if(activeInteract == true) return;

        Debug.Log("상호작용을 시작합니다.");
        activeInteract = true;
        cineCam.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void OffInteractive()
    {
        if(activeInteract == false) return;

        Debug.Log("상호작용이 종료되었습니다.");
        if (interactiveAction.IsPressed()) activeInteract = false;
        cineCam.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }


    private void OnTriggerEnter(Collider playerTrigger)
    {
        if (playerTrigger.gameObject.tag == "NPC")
        {
            npcText.gameObject.SetActive(true);

        }
    }
    private void OnTriggerExit(Collider playerTrigger)
    {
        if (playerTrigger.gameObject.tag == "NPC")
        {
            npcText.gameObject.SetActive(false);

        }
    }


}
