using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class WandShooter1 : MonoBehaviour
{
    public Wand wand; //사용할 완드(바주카)
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점

    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트

    public float UpperBodyIKWeight = 1;

    public bool isMoving;
    public float Timer = 0f;

    private void Start()
    {
        //playerinput,playeranimator 참조 받아오기
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        //슈터와 총이 항상 함께 있도록
        //슈터가 활성화되면,총도 활성화되도록 한다.
        wand.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        //슈터가 사라지면 총도 비활성화되도록 한다
        wand.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerAnimator.GetFloat("movementValue") > 0.001f)
        {
            isMoving = true;
        }
        else if (playerAnimator.GetFloat("mvovementValue") < 0.0999999f)
        {
            isMoving = false;
        }
        // 입력을 감지하고 총 발사하거나 재장전

        //총을 발사한다는 입력을 감지했을 때
        //총 발사 스크립트를 실행. (gun 스크립트의 Fire)
        if (playerInput.fire)
        {
            //총을 발사할 수 있는지 체크하는 함수 실행 ( gun 스크립트의 Fire)
            wand.Fire();
            playerAnimator.SetBool("BazookaActive", true);
            playerAnimator.SetBool("BazookaShooting", true);
            Timer = 0;
        }
        else if (!playerInput.fireDown)
        {
            playerAnimator.SetBool("BazookaShooting", false);
            Timer += Time.deltaTime;
        }

        if (Timer > 5f)
        {
            Debug.Log("Bazooka mode off,마우스를 뗀 이후로 5초이상 지난시점에 대전모드off");
            playerAnimator.SetBool("BazookaActive", false);
        }
    }

    // 탄약 UI 갱신
    private void UpdateUI()
    {
        /*if (UIManager.instance != null && gun != null)
        {
            //참조해야 할 클래스가 할당되어 있는지 체크
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }*/
    }

    // 애니메이터의 IK 갱신
    private void OnAnimatorIK(int layerIndex)
    {
        //FK:부모 조인트(관절) 의 위치에 따라서 자식 조인트(관절)의 위치를 연산해
        //자동으로 위치를 설정하는 방법.
        //IK: 자식 조인트의 위치에 따라서 부모 조인트으 ㅣ위치를 연산,자동으로 위치를 설정한다.

        //목적 : 총을 잡는 위치에 플레이어의 손을 고정한다.
        //손의 위치에 따라 팔꿈치의 위치는 자동으로 연산되도록 설정한다.

        //총의 위치가 플레이어의 오른쪽 팔꿈치를 따라다니도록 이벤트 함수 주기로 갱신한다.
        //GetIKHintPosition : IK(관절) 중 선언하는 키워드의 관절 위치 정보를 갖고온다.
        Debug.Log($"OnAnimatorIK {layerIndex}");
        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, UpperBodyIKWeight);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, UpperBodyIKWeight);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);


        //오른손
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, UpperBodyIKWeight);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, UpperBodyIKWeight);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}