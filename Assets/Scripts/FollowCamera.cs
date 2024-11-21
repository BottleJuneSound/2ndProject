//using UnityEngine;
//using UnityEngine.InputSystem;

//public class FollowCamera : MonoBehaviour
//{
//    public float mouseSens = 0.1f;
//    public float mouseScroll = 0.3f;
//    public Transform cameraTranceform;
//    public Transform cameraObject;
//    //public Transform playerObject;
//    //public Transform playerAngle;


//    float horizontalAngle;
//    float verticalAngle;
//    float cameraDeep = 60;

//    InputAction lookAction;
//    InputAction cameraAction;

//    public Transform target; // 따라다닐 캐릭터 (Player)
//    public Vector3 followPosition = new Vector3(0, 2, -2.5f); // 카메라와 캐릭터 사이의 거리
//    public Vector3 followRotaition = new Vector3(12, 0, 0);
//    public float followSpeed = 5f; // 따라가는 속도

//    void LateUpdate()
//    {
//        if (target != null)
//        {
//            // 목표 위치 계산
//            Vector3 targetPosition = target.position + followPosition;

//            // 부드럽게 따라가기
//            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

//            // 카메라가 항상 캐릭터를 바라보도록 설정
//            transform.LookAt(target);
//        }
//    }
//    private void Awake()
//    {


//    }

//    void Start()
//    {
//        transform.position = followPosition;
//        transform.Rotate(followRotaition);

//        Cursor.lockState = CursorLockMode.Locked;
//        // 나중에 커서이미지로 바꿔보자. 시점 변환을 위한 마우스이기 때문에 없어도?
//        Cursor.visible = true;

//        InputActionAsset inputActions = GetComponent<PlayerInput>().actions;
//        lookAction = inputActions.FindAction("Look");
//        cameraAction = inputActions.FindAction("Scroll");



//        horizontalAngle = transform.localEulerAngles.y;
//        verticalAngle = 0;
//        cameraDeep = cameraObject.localPosition.z;

//    }

//    void Update()
//    {

//        //마우스 좌우 움직임
//        Vector2 look = lookAction.ReadValue<Vector2>();

//        float turnPlayer = look.x * mouseSens;
//        horizontalAngle += turnPlayer;
//        horizontalAngle = Mathf.Clamp(horizontalAngle, -70f, 70f);

//        //if (horizontalAngle >= 360) horizontalAngle -= 360;
//        //if (horizontalAngle < 0) horizontalAngle += 360;

//        Vector3 currentAngle = cameraObject.localEulerAngles;
//        currentAngle.y = horizontalAngle;
//        cameraObject.localEulerAngles = currentAngle;

//        //마우스 상하 움직임
//        float turnCam = look.y * mouseSens;
//        verticalAngle -= turnCam;
//        verticalAngle = Mathf.Clamp(verticalAngle, -80f, 80f);

//        currentAngle = cameraObject.localEulerAngles;
//        currentAngle.x = verticalAngle;
//        cameraObject.localEulerAngles = currentAngle;


//        //마우스 휠 움직임
//        Vector2 lookDeep = cameraAction.ReadValue<Vector2>();
//        float zoom = lookDeep.y * mouseScroll;
//        cameraDeep += zoom;
//        cameraDeep = Mathf.Clamp(cameraDeep, -40f, -0.7f);

//        Vector3 currentDeep = cameraObject.localPosition;
//        currentDeep.z = cameraDeep;
//        cameraObject.localPosition = currentDeep;

//    }

//}