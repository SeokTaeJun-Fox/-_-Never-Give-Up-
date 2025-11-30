using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using System;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    //캐릭터가 키보드로 움직이는지 조이스틱으로 움직이는지에대한 변수  
    [SerializeField] private MoveInput moveInput;

    //캐릭터가 점프키를 눌렀는지(pc버전) 혹은 ui버튼을 눌렀는지(안드로이드버전) 확인하는 변수 
    [SerializeField] private JumpInput jumpInput;

    //이동속도
    [SerializeField] private float moveSpeed;

    //점프력
    [SerializeField] private float jumpPower;

    //회전 속도
    [SerializeField] private float rotateSpeed;

    //바닥체크 스피어위치 트랜스폼 (캐릭터 컨트롤러.isGround가 제대로 작동하고있으면 삭제예정)
    [SerializeField] private Transform groundCheck;

    //바닥체크 스피어 반지름 (캐릭터 컨트롤러.isGround가 제대로 작동하고있으면 삭제예정)
    [SerializeField] private float groundCheckRadius;

    //바닥 레이어마스크 (캐릭터 컨트롤러.isGround가 제대로 작동하고있으면 삭제예정)
    [SerializeField] private LayerMask groundMask;
    
    //캐릭터가 움직이는 방향
    private Vector3 moveDir;

    //캐릭터 컨트롤러
    private CharacterController cc;

    //중력으로인한 벨로시티값
    private Vector3 velocity;

    //AddForce 대체용 벡터 (현재 받고있는 외부적인 힘)
    private Vector3 externalForce;

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

    private float prevRotateY;

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

    public Camera SetCamera
    { 
        set => realCamera = value;
    }

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
        cc = GetComponent<CharacterController>();
        //realCamera = Camera.main;
        prevRotateY = transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        if (cc)
        {
            GroundCheck();

            //착지 이벤트 실행구현
            if (!isPrevGrounded && isGrounded)
                OnLand?.Invoke();

            isPrevGrounded = isGrounded;

            if (moveInput != null)
            {
                //1. 이동
                if (canMove)
                {
                    moveDir = realCamera.transform.forward * moveInput.GetMoveDir().z +
                              realCamera.transform.right * moveInput.GetMoveDir().x;   //키보드 혹은 조이스틱의 입력값으로 받은 방향벡터
                    moveDir = new Vector3(moveDir.x, 0, moveDir.z).normalized;

                    cc.Move(moveDir * moveSpeed * Time.deltaTime);
                }
                else
                {
                    moveDir = Vector3.zero;
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

                //4. 중력관련
                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -2f;
                }

                velocity.y += Physics.gravity.y * Time.deltaTime;

                //5. 점프 구현
                if (jumpInput != null && canJump)
                {
                    if (jumpInput.IsJumpButtonPressed() && isGrounded)
                    {
                        Jump();
                    }
                }

                cc.Move(velocity * Time.deltaTime);

                //6. 외부 힘 적용
                if (externalForce.magnitude > 0.1f)
                {
                    cc.Move(externalForce * Time.deltaTime);
                    // 점점 줄이기 (마찰/저항)
                    externalForce = Vector3.Lerp(externalForce, Vector3.zero, 5f * Time.deltaTime);
                }

                //7. 회전

                if (moveDir.magnitude != 0)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), rotateSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (transform.rotation.eulerAngles.y != prevRotateY)
        {
            OnRotate?.Invoke(moveDir);
        }
        prevRotateY = transform.rotation.eulerAngles.y;
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

    // 외부에서 충격(힘) 가하기
    public void ApplyForce(Vector3 force)
    {
        externalForce += force;
    }

    private void Jump()
    {
        if (cc != null)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * Physics.gravity.y);
        }
        OnJump?.Invoke();
    }

    private void GroundCheck()
    {
        if (cc != null)
            isGrounded = cc.isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }

    private void OnDisable()
    {
        cc.enabled = false;
    }
}
