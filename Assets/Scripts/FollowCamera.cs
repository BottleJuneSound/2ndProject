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

//    public Transform target; // ����ٴ� ĳ���� (Player)
//    public Vector3 followPosition = new Vector3(0, 2, -2.5f); // ī�޶�� ĳ���� ������ �Ÿ�
//    public Vector3 followRotaition = new Vector3(12, 0, 0);
//    public float followSpeed = 5f; // ���󰡴� �ӵ�

//    void LateUpdate()
//    {
//        if (target != null)
//        {
//            // ��ǥ ��ġ ���
//            Vector3 targetPosition = target.position + followPosition;

//            // �ε巴�� ���󰡱�
//            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

//            // ī�޶� �׻� ĳ���͸� �ٶ󺸵��� ����
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
//        // ���߿� Ŀ���̹����� �ٲ㺸��. ���� ��ȯ�� ���� ���콺�̱� ������ ���?
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

//        //���콺 �¿� ������
//        Vector2 look = lookAction.ReadValue<Vector2>();

//        float turnPlayer = look.x * mouseSens;
//        horizontalAngle += turnPlayer;
//        horizontalAngle = Mathf.Clamp(horizontalAngle, -70f, 70f);

//        //if (horizontalAngle >= 360) horizontalAngle -= 360;
//        //if (horizontalAngle < 0) horizontalAngle += 360;

//        Vector3 currentAngle = cameraObject.localEulerAngles;
//        currentAngle.y = horizontalAngle;
//        cameraObject.localEulerAngles = currentAngle;

//        //���콺 ���� ������
//        float turnCam = look.y * mouseSens;
//        verticalAngle -= turnCam;
//        verticalAngle = Mathf.Clamp(verticalAngle, -80f, 80f);

//        currentAngle = cameraObject.localEulerAngles;
//        currentAngle.x = verticalAngle;
//        cameraObject.localEulerAngles = currentAngle;


//        //���콺 �� ������
//        Vector2 lookDeep = cameraAction.ReadValue<Vector2>();
//        float zoom = lookDeep.y * mouseScroll;
//        cameraDeep += zoom;
//        cameraDeep = Mathf.Clamp(cameraDeep, -40f, -0.7f);

//        Vector3 currentDeep = cameraObject.localPosition;
//        currentDeep.z = cameraDeep;
//        cameraObject.localPosition = currentDeep;

//    }

//}