using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour // ���� ���� �پ� �ִ� ��ũ��Ʈ 
{
    bool ClickOn = false;  
    Ray _rayToCatapult;
    float _maxLength = 3f; // ���� ��� �� �ִ� �ִ� ����
    public Transform _zeroPoint; // ���� ó�� ���� �� ��ġ


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
        _rayToCatapult = new Ray(_zeroPoint.position, Vector3.zero); // ray�� ������ _zeroPoint.position ���� , ������ Vector3.zero�� ����

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


        if (ClickOn) // ���콺 Ŭ���� �ߴٸ�
        {
            Vector3 mouseWorldPoint =
                Camera.main.ScreenToWorldPoint(Input.mousePosition); // ���� ���콺 �������� ��ġ�� �޴´�

            mouseWorldPoint.z = 0f; // z���� ���� ��Ȳ���� �ʿ�������� �ʱ�ȭ

            Vector2 _newVector = mouseWorldPoint - _zeroPoint.position; // ó�� ���� ��ġ���� ���콺 �����ͷ� ���� ������ ����Ѵ�


            // _newVector.sqrMagnitude ���� Vector�� ũ�⿡ ������ �� ���̴�
            // ex) Vector3(2,3,4)�� ���Ͱ� �ִٸ�   Vector3(2,3,4).sqrMagnitude = (2*2) + (3*3) + (4*4) = 29 �̴�

            if (_newVector.sqrMagnitude > _maxLength * _maxLength) // ���� ���콺 ������ _maxLength�� �Ѿ�ٸ�
            {
                _rayToCatapult.direction = _newVector; // ray�� ������ ���� ���콺�� �������� �缳�� �Ѵ�

                mouseWorldPoint = _rayToCatapult.GetPoint(_maxLength);
                // ���� _rayToCatapult�� ������ "���� ó�� ���õ� ��ġ" �̰� ������ "���� ���콺�� ��ġ ����" �̴�
                // �̸� GetPoint(_maxLength)�� ���Ͽ� "���� ó�� ��ġ���� -> ���콺�� �ִ� �������� _maxLength�� ��ŭ�� ��ġ�� ��ڴٴ� ��"
            }


            transform.position = mouseWorldPoint;
        }


        if (_spring != null) // ���� �߻� �Ѵٸ� 
        {
            // _prev_velocity.sqrMagnitude ���� Rigidbody2D�� ������ ũ���̴�

            if (_prev_velocity.sqrMagnitude > _rb2d.velocity.sqrMagnitude) // ���� ���� ������ ����� ���� �� ũ�ٸ��� ������ ����� �ȴٸ�
            {
                // ���� _prev_velocity�� ���� ���󰡴� ���� �ִ�ġ
                // _rb2d.velocity�� ���� �ִ�ġ �ٷ� �� �ܰ�

                Destroy(_spring); 
                _rb2d.velocity = _prev_velocity; // ���� ���� �ִ�ġ�� �������ش�
                deleteLine();
            }

            if (ClickOn == false) _prev_velocity = _rb2d.velocity; // ����� ���� ���� ������ �۴ٸ� _prev_velocity�� ���� ��� ���� �����Ѵ�
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
