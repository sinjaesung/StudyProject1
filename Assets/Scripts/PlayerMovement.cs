using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도


    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    public Camera mainCamera;

    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate() {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행

        Move();
        // Rotate();
        RotateCharacterTowardsMouse();

        //움직이는 애니메이션
        //애니메이터의 move 파라미터값을 갱신한다. (-1~1)
        //파라미터를 가져오는 위치 : PlayerInput에서 가져온다.
        playerAnimator.SetFloat("Move", playerInput.move);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move() {
        //리지드바디: MovePosition 사용
        //어디로 이동하느냐 : 현재 위치에서 속도에 따라 얼마나 앞으로 갈지 그때그때 결정
        Vector3 moveDistance = transform.forward * playerInput.move * moveSpeed * Time.deltaTime;

        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // 키보드입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate() {
        //리지드바디의 회전값을 변경 : 원래의 회전값에다 실시간으로 회전값을 더한다.
        //회전방향 x,z 축은 고정,y축만 회전 속도만큼 움직인다.

        //회전속도 : 회전 입력값으로 방향을 결정, 속도: rotateSpeed
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0,turn,0);
    }

    void RotateCharacterTowardsMouse()
    {
        // Step1: Get Mouse Position in Screen Coordinates
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Step2 : Convert Mouse Position to World Coordinates
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        if(groundPlane.Raycast(ray,out rayDistance))
        {
            //Get the point where the ray intersects the plane
            Vector3 pointInWorld = ray.GetPoint(rayDistance);
            Debug.Log("RotateChracterTowardsMouse pointInWorld>>" + pointInWorld);
            //Step3 : Calculate Direction and Rotate Character
            Vector3 direction = (pointInWorld - transform.position).normalized;
            direction.y = 0;//Ensure the charactder stays horizontal,no vertical rotation

            if(direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }

        }
    }
}