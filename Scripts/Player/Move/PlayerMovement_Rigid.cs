using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Rigid : MonoBehaviour
{
    //캐릭터가 키보드로 움직이는지 조이스틱으로 움직이는지에대한 변수  
    [SerializeField] private MoveInput moveInput;

    //캐릭터가 점프키를 눌렀는지(pc버전) 혹은 ui버튼을 눌렀는지(안드로이드버전) 확인하는 변수 
    [SerializeField] private JumpInput jumpInput;

    //이동속도
    [SerializeField] private float moveSpeed;

    //점프력
    [SerializeField] private float jumpPower;

    //바닥체크 스피어위치 트랜스폼
    [SerializeField] private Transform groundCheck;

    //바닥체크 스피어 반지름
    [SerializeField] private float groundCheckRadus;

    //바닥 레이어마스크
    [SerializeField] private LayerMask groundMask;

    //장애물 레이어 (플레이어는 이 장애물 앞에 있으면 장애물 쪽으로 이동하지 못한다)
    [SerializeField] private LayerMask ObstacleLayer;

    //장애물 판정 y위칫값
    [SerializeField] private float obstacleCheckYPos;

    //장애물 판정 최대거리 (플레이어는 장애물까지 최대거리 안에있으면 장애물 쪽으로 이동하지 못한다)
    [SerializeField] private float obstacleCheckDistance;


    //캐릭터가 움직이는 방향
    private Vector3 moveDir;

    //캐릭터 리지드바디
    private Rigidbody myRigid;

    //현재 움직이고있는지 확인한다.
    private bool isMove;

    //직전 fixedUpdate에 움직였는지 확인한다.
    private bool prebIsMove;

    //이동할수있는지 확인한다.
    private bool canMove;

    //점프할수있는지 확인한다.
    private bool canJump;

    //바닥에 닿았는지 확인한다.
    private bool isGrounded;

    //직전 update에 땅에 닿았는지 확인한다.
    private bool isPrevGrounded;

    private Camera realCamera;

    #region Event변수
    //회전 했을 때 실행하는 이벤트
    public event Action<Vector3> OnRotate;
    //움직이기 시작했을 때 실행하는 이벤트
    public event Action<Vector3> OnMoveEnter;
    //움직일 때 마다 실행하는 이벤트
    public event Action<Vector3> OnMove;
    //멈출 때 실행하는 이벤트
    public event Action OnStop;
    //점프했을 때 실행하는 이벤트
    public event Action OnJump;
    //착지했을 때 실행하는 이벤트
    public event Action OnLand;
    #endregion

    public MoveInput setMoveInput
    {
        set => moveInput = value;
    }

    public JumpInput setJumpInput
    {
        set => jumpInput = value;
    }

    public float JumpPower
    {
        get => jumpPower;
        set => jumpPower = value;
    }

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    private void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
        realCamera = Camera.main;
    }

    void Update()
    {
        if (myRigid != null)
        {
            //바닥에 닿았는지 확인한다.
            GroundCheck();

            //착지 이벤트 실행구현
            if (!isPrevGrounded && isGrounded)
                OnLand?.Invoke();

            isPrevGrounded = isGrounded;

            //점프 구현
            if (jumpInput != null && canJump)
            {
                if (jumpInput.IsJumpButtonPressed() && isGrounded)
                {
                    Jump();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (myRigid != null)
        {
            if (moveInput != null)
            {
                //1. 이동
                if (canMove)
                {
                    moveDir = realCamera.transform.forward * moveInput.GetMoveDir().z +
                              realCamera.transform.right * moveInput.GetMoveDir().x;   //키보드 혹은 조이스틱의 입력값으로 받은 방향벡터
                    moveDir = new Vector3(moveDir.x, 0, moveDir.z).normalized;
                }
                else
                {
                    moveDir = Vector3.zero;
                }

                //2. 플레이어 앞 장애물 판정
                //플레이어 앞에 장애물 있으면 움직이는 행동을 하지만 앞을 나아가지 않는다.
                RaycastHit hit;
                bool isObstacle = false;
                if (!Physics.Raycast(transform.position + transform.up * obstacleCheckYPos, moveDir, out hit, obstacleCheckDistance, ObstacleLayer.value))
                {
                    myRigid.MovePosition(myRigid.position + moveDir * moveSpeed * Time.fixedDeltaTime);
                    isObstacle = true;
                }

                //3. 움직임 부울린 변수저장
                //sqrMagnitude는 루트 연산을 하지 않으므로 처리가 가볍다.
                if (!prebIsMove && moveDir.sqrMagnitude > 0)    //움직임이 시작했을 때
                {
                    isMove = true;
                    OnMoveEnter?.Invoke(moveDir);
                }
                else if (moveDir.sqrMagnitude > 0)  //현재 움직이고있을 때
                {
                    isMove = true;
                    OnMove?.Invoke(moveDir);
                }
                else if (prebIsMove && moveDir.sqrMagnitude == 0)   //멈출 때
                {
                    isMove = false;
                    OnStop?.Invoke();
                }

                prebIsMove = isMove;

                //5. 회전

                if (moveDir.magnitude != 0)
                {
                    transform.rotation = Quaternion.LookRotation(moveDir);
                    OnRotate?.Invoke(moveDir);
                }

            }
        }
    }

    /// <summary>
    /// 이동,점프를 할 수 있는/할 수 없는 상태가 된다
    /// </summary>
    /// <param name="inIsMoveAndJump"></param>
    public void SetCanMoveAndJump(bool inIsMoveAndJump)
    {
        canMove = inIsMoveAndJump;
        canJump = inIsMoveAndJump;
    }

    private void Jump()
    {
        if (myRigid != null)
        {
            myRigid.velocity = new Vector3(myRigid.velocity.x, 0f, myRigid.velocity.z); //수직 속도 초기화
            myRigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        OnJump?.Invoke();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadus, groundMask.value);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadus);
    }
}
