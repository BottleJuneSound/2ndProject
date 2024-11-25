using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 2;
    public float mouseSens = 0.1f;
    public float mouseScroll = 0.3f;
    public Transform cameraTranceform;
    public StateMachine stateMachine;
    public Transform cameraObject;
    public Transform playerObject;
    public Transform playerAngle;
    public GameObject pressPanel;

    float horizontalAngle;
    float verticalAngle;
    float cameraDeep = 60;

    InputAction moveAction;
    InputAction lookAction;
    InputAction cameraAction;
    InputAction runAction;
    InputAction skillMAction;
    InputAction skillNAction;
    InputAction skillBAction;


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
        lookAction = inputActions.FindAction("Look");
        cameraAction = inputActions.FindAction("Scroll");
        runAction = inputActions.FindAction("Sprint");
        skillMAction = inputActions.FindAction("SkillM");
        skillNAction = inputActions.FindAction("SkillN");
        skillBAction = inputActions.FindAction("SkillB");




        characterController = GetComponent<CharacterController>();
        stateMachine.Initialize(stateMachine.idleState);

        horizontalAngle = transform.localEulerAngles.y;
        verticalAngle = 0;
        cameraDeep = cameraObject.localPosition.z;

    }

    void Update()
    {
        

        Vector2 moveVector = moveAction.ReadValue<Vector2>();

        //3D에서 y는 Z에 반영하는 것이 자연스러움. xyz 위치 바꿔주는 내용
        Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);


        if (moveVector.magnitude > 1)
        {
            move.Normalize();
        }

        float currentSpeed = walkingSpeed;
        if (runAction.IsPressed())
        {
            currentSpeed = walkingSpeed * 3;
        }

        move = move * currentSpeed * Time.deltaTime;
        move = transform.TransformDirection(move);  //벡터의 방향을 바라보는 방향으로 초기화


        // A와 D 키로 회전 처리 (왼쪽: -90, 오른쪽: +90)
        if (moveVector.x < 0)  // A 키를 눌렀을 때
        {
            playerAngle.Rotate(0, -90 * 2 * Time.deltaTime, 0);  // -90 회전 (왼쪽)
        }
        else if (moveVector.x > 0)  // D 키를 눌렀을 때
        {
            playerAngle.Rotate(0, 90 * 2 * Time.deltaTime, 0);  // +90 회전 (오른쪽)
        }


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



        //마우스 좌우 움직임
        Vector2 look = lookAction.ReadValue<Vector2>();

        float turnPlayer = look.x * mouseSens;
        horizontalAngle += turnPlayer;
        //horizontalAngle = Mathf.Clamp(horizontalAngle, -70f, 70f);

        if (horizontalAngle >= 360) horizontalAngle -= 360;
        if (horizontalAngle < 0) horizontalAngle += 360;

        Vector3 currentAngle = playerObject.localEulerAngles;
        currentAngle.y = horizontalAngle;
        playerObject.localEulerAngles = currentAngle;

        //마우스 상하 움직임
        float turnCam = look.y * mouseSens;
        verticalAngle -= turnCam;
        verticalAngle = Mathf.Clamp(verticalAngle, -80f, 80f);

        currentAngle = cameraObject.localEulerAngles;
        currentAngle.x = verticalAngle;
        cameraObject.localEulerAngles = currentAngle;


        //마우스 휠 움직임
        Vector2 lookDeep = cameraAction.ReadValue<Vector2>();
        float zoom = lookDeep.y * mouseScroll;
        cameraDeep += zoom;
        cameraDeep = Mathf.Clamp(cameraDeep, -40f, -0.7f);

        Vector3 currentDeep = cameraObject.localPosition;
        currentDeep.z = cameraDeep;
        cameraObject.localPosition = currentDeep;

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

    }

    public void OnMedicine()
    {
        Debug.Log("약물치료 시행!");
    }

    public void OnPray()
    {
        Debug.Log("기도치료 시행!");

    }


}
