using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMove : MonoBehaviour
{
    //ĳ���� ���� �̵� �ӵ� (�ȱ�)
    public float walkMoveSpd = 2.0f;

    //ĳ���� ���� �̵� �ӵ� (�޸���)
    public float runMoveSpd = 3.5f;

    //ĳ���� ȸ�� �̵� �ӵ� 
    public float rotateMoveSpd = 100.0f;

    //ĳ���� ȸ�� �������� ���� ������ �ӵ�
    public float rotateBodySpd = 2.0f;

    //ĳ���� �̵� �ӵ� ���� ��
    public float moveChageSpd = 0.1f;

    //���� ĳ���� �̵� ���� �� 
    private Vector3 vecNowVelocity = Vector3.zero;

    //���� ĳ���� �̵� ���� ���� 
    private Vector3 vecMoveDirection = Vector3.zero;

    //CharacterController ĳ�� �غ�
    private CharacterController controllerCharacter = null;

    //ĳ���� CollisionFlags �ʱⰪ ����
    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

    // Start is called before the first frame update
    void Start()
    {
        //CharacterController ĳ��
        controllerCharacter = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        //ĳ���� �̵� 
        Move();
        // Debug.Log(getNowVelocityVal());
        //ĳ���� ���� ���� 
        vecDirectionChangeBody();
    }

    /// <summary>
    /// �̵��Լ� �Դϴ� ĳ����
    /// </summary>
    void Move()
    {
        Transform CameraTransform = Camera.main.transform;
        //���� ī�޶� �ٶ󺸴� ������ ����� � �����ΰ�.
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        //forward.z, forward.x
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        //Ű�Է� 
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //�ɸ��Ͱ� �̵��ϰ��� �ϴ� ���� 
        Vector3 targetDirection = horizontal * right + vertical * forward;

        //���� �̵��ϴ� ���⿡�� ���ϴ� �������� ȸ�� 

        vecMoveDirection = Vector3.RotateTowards(vecMoveDirection, targetDirection, rotateMoveSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        vecMoveDirection = vecMoveDirection.normalized;
        //ĳ���� �̵� �ӵ�
        float spd = walkMoveSpd;
    }


    /// <summary>
    /// ���� �� �ɸ��� �̵� �ӵ� �������� ��  
    /// </summary>
    /// <returns>float</returns>
    float getNowVelocityVal()
    {
        //���� ĳ���Ͱ� ���� �ִٸ� 
        if (controllerCharacter.velocity == Vector3.zero)
        {
            //��ȯ �ӵ� ���� 0
            vecNowVelocity = Vector3.zero;
        }
        else
        {

            //��ȯ �ӵ� ���� ���� /
            Vector3 retVelocity = controllerCharacter.velocity;
            retVelocity.y = 0.0f;

            vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, moveChageSpd * Time.fixedDeltaTime);

        }
        //�Ÿ� ũ��
        return vecNowVelocity.magnitude;
    }

    /// <summary>
	/// GUI SKin
	/// </summary>
    private void OnGUI()
    {
        if (controllerCharacter != null && controllerCharacter.velocity != Vector3.zero)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 50;
            labelStyle.normal.textColor = Color.white;
            //ĳ���� ���� �ӵ�
            float _getVelocitySpd = getNowVelocityVal();
            GUILayout.Label("����ӵ� : " + _getVelocitySpd.ToString(), labelStyle);

            //���� ĳ���� ���� + ũ��
            GUILayout.Label("���纤�� : " + controllerCharacter.velocity.ToString(), labelStyle);

            //����  ����� ũ�� �ӵ�
            GUILayout.Label("������� ũ�� �ӵ� : " + vecNowVelocity.magnitude.ToString(), labelStyle);

        }
    }
    /// <summary>
    /// ĳ���� ���� ���� ���� �Լ�
    /// </summary>
    void vecDirectionChangeBody()
    {
        //ĳ���� �̵� ��
        if (getNowVelocityVal() > 0.0f)
        {
            //�� ����  �ٶ���� �ϴ� ���� ���?
            Vector3 newForward = controllerCharacter.velocity;
            newForward.y = 0.0f;

            //�� ĳ���� ���� ���� 
            transform.forward = Vector3.Lerp(transform.forward, newForward, rotateBodySpd * Time.deltaTime);

        }
    }

}
