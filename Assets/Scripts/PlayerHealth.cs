using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity {
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더

    public AudioClip deathClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리
    public AudioClip itemPickupClip; // 아이템 습득 소리

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트

    private void Awake() {
        // 사용할 컴포넌트를 가져오기
        playerAudioPlayer = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable() {
        //LivingEntity: 생명체의 설정 초기화
        //=>플레이어도 생명체이기 때문에 초기화 함수 같이 사용
        base.OnEnable();

        //플레이어에게서만 해야할 초기화를 추가로 진행.
        //1.체력 슬라이드바 초기화, 체력 동기화
        //슬라이더활성화,슬라이더 최대값 체력의 최대값에 맞춘다, 슬라이더 현재값 체력에 현재값에 맞춘다.
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;

        //2.플레이어 조작과 관련된 스크립트 활성화
        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }
    private void Update()
    {
        if(healthSlider.value != health)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, health, Time.deltaTime);
        }
    }
    // 체력 회복
    public override void RestoreHealth(float newHealth) {
        base.RestoreHealth(newHealth);

        //healthSlider.value = health;
    }

    // 데미지 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection) {
        //데미지를 입으면 효과음을 재생 + 사망하지 않았을 때만
        if(dead == false)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }

        //데미지 메커니즘은 LivingEntity와 동일
        base.OnDamage(damage, hitPoint, hitDirection);

        //체력UI갱신
        healthSlider.value = health;
    }

    // 사망 처리
    public override void Die() {
        base.Die();

        healthSlider.gameObject.SetActive(false);

        playerAudioPlayer.PlayOneShot(deathClip);

        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        //아이템에 부딪히면 해당 아이템을 사용한다.

        //플레이어가 아이템을 먹을 수 있는 상태인지 체크 => 안 죽었는가?

        //죽지 않았을 때
        //부딪힌 물체(other)가 item인지를 체크 => Item을 참조하고 있는 물체인지?
        //IItem을 물체가 참조하고 있다면 사용 가능.
        //아이템 사용, 아이템 획득 효과음 1회 재생.
        if(dead == false )
        {
            //죽지 않았을 떄 
            //부딪힌 물체(other)가 item인지를 체크 =
            IItem item = other.GetComponent<IItem>();

            //IItem을 물체가 참조하고 있다면 사용 가능.
            if(item != null)
            {
                Debug.Log("아이템 사용>>");
                //아이템 사용
                item.Use(this.gameObject);
                //아이템 획득 효과음 1회 재생
                playerAudioPlayer.PlayOneShot(itemPickupClip);
            }
        }
    }
}