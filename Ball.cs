using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour // 현재 공에 붙어 있는 스크립트 
{
    bool ClickOn = false;  
    Ray _rayToCatapult;
    float _maxLength = 3f; // 공을 당길 수 있는 최대 범위
    public Transform _zeroPoint; // 공이 처음 시작 할 위치


    public bool touchOn = false; 
    

    Rigidbody2D _rb2d;
    SpringJoint2D _spring;
    Vector2 _prev_velocity;


    LineRenderer _linkback, _lineforce;
    bool _isShowLine = true;
    AngryGameMananger Angry;
    bool CrashCheck = false;

    void Start()
    {
        _zeroPoint = GameObject.Find("Catapultposition").transform;
        _rayToCatapult = new Ray(_zeroPoint.position, Vector3.zero); // ray를 원점은 _zeroPoint.position 으로 , 방향은 Vector3.zero로 설정

        _rb2d = GetComponent<Rigidbody2D>();
        _spring = GetComponent<SpringJoint2D>();
        _lineforce = GameObject.Find("LineFore").GetComponent<LineRenderer>();
        _linkback = GameObject.Find("LineBack").GetComponent<LineRenderer>();
        Angry = GameObject.Find("AngryGamaManager").GetComponent<AngryGameMananger>();

    }


    void Update()
    {
        if (!touchOn) return;

        if(Angry.GameMode == 4) _rb2d.simulated = false;
        if (Angry.GameMode == 2 || Angry.GameMode == 3) _rb2d.simulated = false;
        if (Angry.GameMode == 1) _rb2d.simulated = true;


        if (ClickOn) // 마우스 클릭을 했다면
        {
            Vector3 mouseWorldPoint =
                Camera.main.ScreenToWorldPoint(Input.mousePosition); // 현재 마우스 포인터의 위치를 받는다

            mouseWorldPoint.z = 0f; // z축은 현재 상황에서 필요없음으로 초기화

            Vector2 _newVector = mouseWorldPoint - _zeroPoint.position; // 처음 공의 위치에서 마우스 포인터로 가는 방향을 계산한다


            // _newVector.sqrMagnitude 현재 Vector의 크기에 제곱을 한 값이다
            // ex) Vector3(2,3,4)인 벡터가 있다면   Vector3(2,3,4).sqrMagnitude = (2*2) + (3*3) + (4*4) = 29 이다

            if (_newVector.sqrMagnitude > _maxLength * _maxLength) // 만약 마우스 범위가 _maxLength를 넘어간다면
            {
                _rayToCatapult.direction = _newVector; // ray의 방향을 현재 마우스의 방향으로 재설정 한다

                mouseWorldPoint = _rayToCatapult.GetPoint(_maxLength);
                // 현재 _rayToCatapult의 원점은 "공이 처음 세팅된 위치" 이고 방향은 "현재 마우스의 위치 방향" 이다
                // 이를 GetPoint(_maxLength)를 통하여 "공의 처음 위치에서 -> 마우스가 있는 방향으로 _maxLength의 만큼의 위치를 얻겠다는 뜻"
            }


            transform.position = mouseWorldPoint;
        }


        if (_spring != null) // 공을 발사 한다면 
        {
            // _prev_velocity.sqrMagnitude 현재 Rigidbody2D의 제곱의 크기이다

            if (_prev_velocity.sqrMagnitude > _rb2d.velocity.sqrMagnitude) // 만약 현재 값보다 저장된 값이 더 크다면라는 조건이 통과가 된다면
            {
                // 현재 _prev_velocity의 값은 날라가는 힘의 최대치
                // _rb2d.velocity는 힘의 최대치 바로 밑 단계

                Destroy(_spring); 
                _rb2d.velocity = _prev_velocity; // 따라서 힘의 최대치로 변경해준다
                deleteLine();
            }

            if (ClickOn == false) _prev_velocity = _rb2d.velocity; // 저장된 값이 현재 값보다 작다면 _prev_velocity의 값을 계속 새로 변경한다
        }

        updateLine();

    }

    void updateLine()
    {
        if (!_isShowLine) return;

        _linkback.SetPosition(1, transform.position);
        _lineforce.SetPosition(1, transform.position);
    }

    void deleteLine()
    {
        _isShowLine = false;
        _lineforce.gameObject.SetActive(false);
        _linkback.gameObject.SetActive(false);
    }


    void OnMouseDown()
    {
        if (!touchOn) return;

        ClickOn = true;
    }

    void OnMouseUp()
    {
        if (!touchOn) return;

        ClickOn = false;
        _rb2d.bodyType = RigidbodyType2D.Dynamic;
    }


    

    void OnCollisionEnter2D(Collision2D col)
    {
        if (CrashCheck) return;

        CrashCheck = true;
        Invoke("Crash", 5f);
    }

   
    void Crash()
    {
        if (Angry.BirdCheck != 0)
        {
            Angry.Life -= 1;

            if(Angry.Life > -1) Destroy(Angry.Lifes[Angry.Life]);
            if(Angry.Life == 0) _rb2d.simulated = false;
        }

        _lineforce.gameObject.SetActive(true);
        _linkback.gameObject.SetActive(true);

        Angry.GameSet();
    }   

    
}
