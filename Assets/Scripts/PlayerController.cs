using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject pressPanel;
    public Transform playerAngle;
    public StateMachine stateMachine;

    public float walkingSpeed = 2;
    public float rotationSpeed = 1;
    public float terminalSpeed = 50;
    public float verticalSpeed = 0;

    InputAction moveAction;
    InputAction runAction;
    InputAction skillMAction;
    InputAction skillNAction;
    InputAction skillBAction;
    public InputAction interactiveAction;

    public bool activeInteract = false;
    public CharacterController characterController;    //이거 유니티 기능임. 내가만든 스크립트아님 ㅠ

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

        characterController = GetComponent<CharacterController>();
        stateMachine.Initialize(stateMachine.idleState);

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
                OnMedicine();
            }

            if (skillNAction.IsPressed() && pressPanel.activeSelf)
            {
                OnPray();
            }

            if (skillBAction.IsPressed() && pressPanel.activeSelf)
            {
                OnBloodWithdrawal();
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

    public void OnBloodWithdrawal()
    {
        Debug.Log("방혈치료 시행!");
        activeInteract = false;

    }

    public void OnMedicine()
    {
        Debug.Log("약물치료 시행!");
        activeInteract = false;
    }

    public void OnPray()
    {
        Debug.Log("기도치료 시행!");
        activeInteract = false;

    }

    public void OnInteractive()
    {
        if(activeInteract == true) return;

        Debug.Log("상호작용을 시작합니다.");
        activeInteract = true;

    }

    public void OffInteractive()
    {
        if(activeInteract == false) return;

        Debug.Log("상호작용이 종료되었습니다.");
        if (interactiveAction.IsPressed()) activeInteract = false;
    }


}
