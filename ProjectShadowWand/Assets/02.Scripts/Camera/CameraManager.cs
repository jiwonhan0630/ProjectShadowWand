using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ECameraState
{
    DEFAULT = 0,
    ZOOMIN = 1,
    ZOOMOUT,
    STAY
}
public class CameraManager : Manager<CameraManager>
{
    [Tooltip("사용할 카메라. 넣지 않을 경우 자동으로 메인 카메라를 넣습니다. ")]
    public Camera currentCamera;

    [HideInInspector]
    public Transform currentTransform;
    [Tooltip("팔로우, 줌인, 줌아웃 대상")]
    public Transform target;

    [Tooltip("size에 따라서 스케일을 변화시킬 오브젝트...")]
    public GameObject BGObject;

    //위 오브젝트의 스케일 x,y값.
    private float scObjX = 0f;
    private float scObjY = 0f;

    private Vector3 scObjVel = Vector3.zero;

    [Header("기본 설정")]

    [Tooltip("줌인/줌아웃을 사용하는가?")]
    public bool useZoom;

    [Tooltip("카메라의 기본 z값 입니다. 줌과는 관련 없습니다.")]
    public float cameraDefaultPositionZ = -10f;

    [Tooltip("카메라의 원래...그...원래 사이즈. 원래 줌?")]
    public float cameraDefaultSize = 5f;

    [Tooltip("카메라 줌 인의 최대치")]
    public float cameraZoomInSize = 3f;

    [Tooltip("카메라 줌 아웃의 최대치")]
    public float cameraZoomOutSize = 10f;

    [Tooltip("팔로우 속도")]
    public float followSpeed;

    [Tooltip("줌인/줌아웃 속도. 시간모드일 경우 속도가 아닌 '초'로 작용함.")]
    public float zoomSpeed;

    [Tooltip("시간모드인가? : 줌인/줌아웃 시, 속도는 신경쓰지 않고 무조건 zoomSpeed초 안에 줌인/줌아웃을 끝냄.")]
    public bool isTimeMode;


    private float velocity = 0.0f;//줌 가속도

    public float zoomTimer = 0f;
    public float currentZoomSpeed;

    [SerializeField]
    private ECameraState cameraState;

    [Header("카메라 제한영역 설정")]
    [Tooltip("제한 영역 설정을 할 것인가?")]
    public bool isConfine;

    [Tooltip("박스 콜라이더의 크기 만큼 제한 영역 설정을 합니다.")]
    public BoxCollider2D confineCollider;

    [Space(20)]

    private Vector3 currentConfinePos;
    private Vector3 currentChangeSize;
    private float height;
    private float width;

    private IEnumerator currentCoroutine;

    private float limitCalSize;

    private Vector3 originBGObjectScale;


    public GameObject whiteScreen;
    private Vector2 offset;
    private Vector2 size;
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public void SetTarget(Transform _t)
    {
        target = _t;
    }
    public void Init()
    {

        offset = confineCollider.offset;
        size = confineCollider.size;
        height = currentCamera.orthographicSize;
        width = height * Screen.width / Screen.height;

        #region if <=0

        if (currentCamera == null)
        {
            Debug.Log("currentCamera가 null");
            currentCamera = Camera.main;

        }
        currentTransform = currentCamera.transform;
        if (zoomSpeed <= 0f)
        {
            zoomSpeed = 1f;

        }

        if (followSpeed <= 0f)
        {
            followSpeed = 5f;
        }

        if (BGObject == null)
        {
            BGObject = new GameObject();
            BGObject.transform.parent = gameObject.transform;
            //그냥 오류 방지를 위해 빈깡 하나 넣기.
        }
        #endregion

        limitCalSize = 0.03f;
        currentZoomSpeed = 1f / zoomSpeed;
        cameraState = ECameraState.DEFAULT;
        isTimeMode = true;

        scObjX = BGObject.transform.localScale.x;
        scObjY = BGObject.transform.localScale.y;
        //cameraDefaultPositionZ = -10f;

        originBGObjectScale = BGObject.transform.localScale;
    }



    public void StartZoomMode()
    {
        if (useZoom == true)
        {
            StartCoroutine(CameraZoom());
            StartCoroutine(CameraControl());

        }

    }

    //private void FixedUpdate()
    //{
    //    FollowTarget(); // 타겟 팔로잉
    //}

    //private float followProgress = 0f;
    //private float followTimer = 0f;
    //private Vector3 originalPosition = Vector3.zero;

    [Tooltip("카메라가 이동하지 않는 상태입니다.")]
    public bool isStop;
    [Tooltip("단순히 타겟을 팔로우 하는 상태가 아닐 때 사용합니다.")]
    private bool isExtraMove;
    private Vector2 prevPosition;

    private float distance;
    public void FollowTarget() // 타겟 팔로잉
    {

        distance = Vector2.Distance(currentTransform.position, target.position);
        //currentTransform.position = Vector3.Lerp(currentTransform.position, target.position, Time.deltaTime * followSpeed);
        if (distance > 0.01f)
        {
            currentTransform.position = Vector3.Lerp(currentTransform.position, target.position, Time.deltaTime * followSpeed);

        }

        if (isConfine) //제한 설정이 되어있으면 
        {
            currentConfinePos = GetConfinePosition();
            currentTransform.position = new Vector3(currentConfinePos.x, currentConfinePos.y, cameraDefaultPositionZ);
        }
        else
        {
            currentTransform.position = new Vector3(currentTransform.position.x, currentTransform.position.y, cameraDefaultPositionZ);
        }

        if (Mathf.Abs(prevPosition.x - currentTransform.position.x) < 0.01f)
        {
            isStop = true;
        }
        else
        {
            isStop = false;
        }
        prevPosition = currentTransform.position;
    }
    private IEnumerator CameraZoom()
    {
        while (true)
        {
            currentZoomSpeed = 1f / zoomSpeed;

            if (InputManager.Instance.buttonScroll.ReadValue().y > 0)
            {

                cameraState = ECameraState.ZOOMIN;
            }

            if (InputManager.Instance.buttonScroll.ReadValue().y < 0)
            {

                cameraState = ECameraState.ZOOMOUT;
            }
            yield return null;
        }

    }
    private IEnumerator CameraControl()
    {
        while (true)
        {
            switch (cameraState)
            {
                case ECameraState.DEFAULT:
                    if (currentCamera.orthographicSize > cameraDefaultSize)
                    {
                        currentCoroutine = CameraZoomIn(cameraDefaultSize);
                    }
                    else if (currentCamera.orthographicSize < cameraDefaultSize)
                    {
                        currentCoroutine = CameraZoomOut(cameraDefaultSize);
                    }
                    else
                    {
                        currentCoroutine = null;
                    }
                    break;

                case ECameraState.ZOOMIN:
                    currentCoroutine = CameraZoomIn(cameraZoomInSize);
                    break;

                case ECameraState.ZOOMOUT:
                    currentCoroutine = CameraZoomOut(cameraZoomOutSize);
                    break;

                case ECameraState.STAY:
                    currentCoroutine = null;
                    break;

                default:
                    currentCoroutine = null;
                    break;
            }

            if (currentCoroutine != null)
            {
                yield return StartCoroutine(currentCoroutine);
            }
            else
            {

                yield return null;
            }
        }
    }
    private IEnumerator CameraZoomOut(float _size)
    {
        if (NowCameraState() == ECameraState.ZOOMIN)
        {
            _size = cameraDefaultSize;
        }

        if (isTimeMode) //시간 모드일 경우
        {
            zoomTimer = 0f;
            float oldOrthographicSize = currentCamera.orthographicSize; // 원래 사이즈를 저장

            while (Mathf.Abs(currentCamera.orthographicSize - _size) > limitCalSize)
            {
                zoomTimer += Time.smoothDeltaTime * currentZoomSpeed;
                currentCamera.orthographicSize = Mathf.Lerp(oldOrthographicSize, _size, zoomTimer);
                //스케일 변경 하고싶은 오브젝트의 스케일을...뭐 그 퍼센트만큼으로 변경시킨다. 되려나...

                // BG 크기 체인지
                ChangeScaleBGObject();

                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }
        else //아닐 경우
        {

            while (Mathf.Abs(currentCamera.orthographicSize - _size) > limitCalSize)
            {
                //zoomTimer += Time.deltaTime;
                currentCamera.orthographicSize = Mathf.SmoothDamp(currentCamera.orthographicSize, _size, ref velocity, zoomSpeed);
                //ChangeScaleThisObject();

                // BG 크기 체인지
                ChangeScaleBGObject();
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }
        currentCamera.orthographicSize = _size;
        cameraState = ECameraState.STAY;



    }
    private IEnumerator CameraZoomIn(float _size)
    {
        if (NowCameraState() == ECameraState.ZOOMOUT)
        {
            _size = cameraDefaultSize;
        }

        if (isTimeMode) //시간 모드일 경우
        {
            zoomTimer = 0f;
            float oldOrthographicSize = currentCamera.orthographicSize; // 원래 사이즈를 저장

            while (Mathf.Abs(currentCamera.orthographicSize - _size) > limitCalSize)
            {
                zoomTimer += Time.smoothDeltaTime * currentZoomSpeed;
                currentCamera.orthographicSize = Mathf.Lerp(oldOrthographicSize, _size, zoomTimer);

                // BG 크기 체인지
                ChangeScaleBGObject();

                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }
        else //아닐 경우
        {
            while (Mathf.Abs(currentCamera.orthographicSize - _size) > limitCalSize)
            {
                // zoomTimer += Time.deltaTime;
                currentCamera.orthographicSize = Mathf.SmoothDamp(currentCamera.orthographicSize, _size, ref velocity, zoomSpeed);

                // BG 크기 체인지
                ChangeScaleBGObject();

                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }

        currentCamera.orthographicSize = _size;
        cameraState = ECameraState.STAY;

        // BG 크기 체인지
        ChangeScaleBGObject();
    }


    private Vector3 limitTest;

    private Vector3 GetConfinePosition()
    {
        height = currentCamera.orthographicSize;
        width = height * Screen.width / Screen.height;

        float localX = size.x * 0.5f - width;
        float localY = size.y * 0.5f - height;

        float posX = offset.x;
        float posY = offset.y;

        //float posX = confinePos.x;
        //float posY = confinePos.y;

        //float localX = confineSize.x * 0.5f - width;

        //float localY = confineSize.y * 0.5f - height;

        float clampX = Mathf.Clamp(currentTransform.position.x, -localX + posX, localX + posX);
        float clampY = Mathf.Clamp(currentTransform.position.y, -localY + posY, localY + posY);

        Vector3 centerBottom = currentCamera.ViewportToWorldPoint(new Vector2(0.5f, 0f));
        //가운데 아래의 좌표를 얻어야함....

        //float clampX = Mathf.Clamp(centerBottom.x, -localX + confinePos.x, localX + confinePos.x);
        //float clampY = Mathf.Clamp(centerBottom.y, -localY + confinePos.y, localY + confinePos.y);


        Vector3 confinePosition = new Vector3(clampX, clampY, cameraDefaultPositionZ);
        limitTest = new Vector3(clampX, clampY, cameraDefaultPositionZ);
        return confinePosition;
    }


    private ECameraState NowCameraState()
    {
        if (currentCamera.orthographicSize == cameraZoomOutSize)
        {
            return ECameraState.ZOOMOUT;
        }
        else if (currentCamera.orthographicSize == cameraZoomInSize)
        {
            return ECameraState.ZOOMIN;
        }
        else if (currentCamera.orthographicSize == cameraDefaultSize)
        {
            return ECameraState.DEFAULT;
        }
        else
        {
            return ECameraState.STAY;
        }


    }

    private void ChangeScaleBGObject()
    {
        float BgScaleRatio = (currentCamera.orthographicSize / cameraDefaultSize);
        Vector3 scaleValue = originBGObjectScale * BgScaleRatio;
        BGObject.transform.localScale = new Vector3(scaleValue.x, scaleValue.y, originBGObjectScale.z);
    }

    private void OnDrawGizmos()
    {
        //if (isConfine)
        //{

        if (confineCollider != null)
        {
            

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(confineCollider.offset, 0.5f);
            Gizmos.DrawWireCube(confineCollider.offset, confineCollider.size);
        }
        //}

    }

    /// <summary>
    ///해당 오브젝트가 화면 범위 내에 있는지 검사합니다.
    /// </summary>
    /// <param name="_object"></param>
    /// <returns>화면 안에 있으면 true, 아니면 false를 리턴합니다.</returns>
    public bool CheckThisObjectInScreen(GameObject _object)
    {

        Vector3 screenPoint = currentCamera.WorldToViewportPoint(_object.transform.position);
        bool inScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        return inScreen;
    }

    public void SetWhiteScreen(bool _b)
    {
        whiteScreen.SetActive(_b);
    }
    private void OnDestroy()
    {
        instance = null;
    }
}
