using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCharacterMove : MonoBehaviour
{
    //ĳ���� ���� �̵� �ӵ� (�ȱ�)
    public float walkMoveSpd = 5.0f;

    //ĳ���� ȸ�� �̵� �ӵ� 
    public float rotateMoveSpd = 100.0f;

    //ĳ���� ȸ�� �������� ���� ������ �ӵ�
    public float rotateBodySpd = 5.0f;

    //ĳ���� �̵� �ӵ� ���� ��
    public float moveChageSpd = 0.5f;

    //���� ĳ���� �̵� ���� �� 
    private Vector3 vecNowVelocity = Vector3.zero;

    //���� ĳ���� �̵� ���� ���� 
    private Vector3 vecMoveDirection = Vector3.zero;

    //CharacterController ĳ�� �غ�
    private CharacterController controllerCharacter = null;

    //ĳ���� CollisionFlags �ʱⰪ ����
    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;


    [Header("�ִϸ��̼� �Ӽ�")]
    public AnimationClip Idle = null;
    public AnimationClip Walk = null;

    //������Ʈ�� �ʿ��մϴ� 
    private Animation animationPlayer = null;

    //ĳ���� ����  ĳ���� ���¿� ���� animation�� ǥ��
    public enum PlayerState { None, Idle, Walk }

    [Header("ĳ���ͻ���")]
    public PlayerState playerState = PlayerState.None;

    // Start is called before the first frame update
    void Start()
    {
        //CharacterController ĳ��
        controllerCharacter = GetComponent<CharacterController>();

        //Animation component ĳ��
        animationPlayer = GetComponent<Animation>();
        //Animation Component �ڵ� ��� ����
        animationPlayer.playAutomatically = false;
        //Ȥ�ó� ������� Animation �ִٸ�? ���߱�
        animationPlayer.Stop();

        //�ʱ� �ִϸ��̼��� ���� Enum
        playerState = PlayerState.Idle;

        //animation WrapMode : ��� ��� ���� 
        animationPlayer[Idle.name].wrapMode = WrapMode.Loop;
        animationPlayer[Walk.name].wrapMode = WrapMode.Loop;

    }

    // Update is called once per frame
    void Update()
    {
        //ĳ���� �̵� 
        Move();
        // Debug.Log(getNowVelocityVal());
        //ĳ���� ���� ���� 
        vecDirectionChangeBody();

        //���� ���¿� ���߾ �ִϸ��̼��� ��������ݴϴ�
        AnimationClipCtrl();

        //�÷��̾� ���� ���ǿ� ���߾� �ִϸ��̼� ��� 
        ckAnimationState();

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

        if (playerState == PlayerState.Walk)
        {
            spd = walkMoveSpd;
        }


        // ������ �̵� ��
        Vector3 moveAmount = (vecMoveDirection * spd * Time.deltaTime);

        collisionFlagsCharacter = controllerCharacter.Move(moveAmount);


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


    /// <summary>
    ///  �ִϸ��̼� ��������ִ� �Լ�
    /// </summary>
    /// <param name="clip">�ִϸ��̼�Ŭ��</param>
    void playAnimationByClip(AnimationClip clip)
    {
        //ĳ�� animation Component�� clip �Ҵ�
        //        animationPlayer.clip = clip;
        animationPlayer.GetClip(clip.name);
        //����
        animationPlayer.CrossFade(clip.name);
    }

    /// <summary>
    /// ���� ���¿� ���߾� �ִϸ��̼��� ���
    /// </summary>
    void AnimationClipCtrl()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                playAnimationByClip(Idle);
                break;
            case PlayerState.Walk:
                playAnimationByClip(Walk);
                break;
        }
    }

    /// <summary>
    ///  ���� ���¸� üũ���ִ� �Լ�
    /// </summary>
    void ckAnimationState()
    {
        //���� �ӵ� ��
        float nowSpd = getNowVelocityVal();

        if (nowSpd < 0.01f)
        {
            playerState = PlayerState.Idle;
        }

        else
        {
            playerState = PlayerState.Walk;
        }
    }

    /// <summary>
    /// �ִϸ��̼� Ŭ�� ����� ���� ���� �ִϸ��̼� �̺�Ʈ �Լ��� ȣ��
    /// </summary>
    /// <param name="clip">AnimationClip</param>
    /// <param name="FuncName">event function</param>
    void SetAnimationEvent(AnimationClip animationclip, string funcName)
    {
        //���ο� �̺�Ʈ ����
        AnimationEvent newAnimationEvent = new AnimationEvent();

        //�ش� �̺�Ʈ�� ȣ�� �Լ��� funcName
        newAnimationEvent.functionName = funcName;

        newAnimationEvent.time = animationclip.length - 0.5f;

        animationclip.AddEvent(newAnimationEvent);
    }
}
