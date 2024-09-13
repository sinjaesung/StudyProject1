using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드를 가져오기

// 적 AI를 구현한다
public class Enemy : LivingEntity {
    public LayerMask whatIsTarget; // 추적 대상 레이어

    private LivingEntity targetEntity; // 추적할 대상
    private NavMeshAgent pathFinder; // 경로계산 AI 에이전트

    public ParticleSystem hitEffect; // 피격시 재생할 파티클 효과
    public AudioClip deathSound; // 사망시 재생할 소리
    public AudioClip hitSound; // 피격시 재생할 소리

    private Animator enemyAnimator; // 애니메이터 컴포넌트
    private AudioSource enemyAudioPlayer; // 오디오 소스 컴포넌트
    private Renderer enemyRenderer; // 렌더러 컴포넌트

    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점

    public Color OriginalColor;
    EnemySpawner enemyspawner;
    // 추적할 대상이 존재하는지 알려주는 프로퍼티

    public bool IsPaused = false;//얼려졌을때 또는 스턴기
    public List<string> StateList = new List<string>();

    private bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead && !IsPaused)
            {
                return true;
            }//일시정지된 경우에도 움직임을 멈추기위해(Run->Idle)

            // 그렇지 않다면 false
            return false;
        }
    }

    private void Awake() {
        enemyspawner = FindObjectOfType<EnemySpawner>();
        // 초기화 NavMeshAgent,enemyAnimator,enemyAudioPlayer
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudioPlayer = GetComponent<AudioSource>();

        enemyRenderer = GetComponentInChildren<Renderer>();

        StartCoroutine(UpdateStateCoroutine());
    }

    private void Start()
    {
        //플레이어를 따라다니는 네비게이션 기능 시작.
        StartCoroutine(UpdatePath());
    }
    public void ReStartUpdatePathCoroutine()
    {
        StopCoroutine(UpdatePath());
        StartCoroutine(UpdatePath());
    }
    private void OnDisable()
    {
        Debug.Log("Enemy오브젝트 제거시 모든 코루틴 종료");
        StopAllCoroutines();
    }


    // 적 AI의 초기 스펙을 결정하는 셋업 메서드
    public void Setup(float newHealth, float newDamage, float newSpeed, Color skinColor) {
        startingHealth = newHealth;
        health = newHealth;
        damage = newDamage;
        pathFinder.speed = newSpeed;
        enemyRenderer.material.color = skinColor;
        OriginalColor = skinColor;
        Debug.Log("Enemy Setup>>" + skinColor);
    }


    //상태관련
    public void AddStateList(string item)
    {
        StateList.Add(item);
    }
    public void PrintStateList()
    {
        for (int e = 0; e < StateList.Count; e++)
        {
            Debug.Log(e + "| PrintStateList>>" + StateList[e]);
        }
    }
    public void DeleteStateListItem(string item)
    {
        for(int n=0; n<StateList.Count; n++)
        {
            Debug.Log("Enemy타깃 "+ n + $"| {StateList[n]}");
        }
        Debug.Log("Enemy타깃 오브젝트 Freeze속성 있는거 모두 제거!"+transform.name);
        StateList.RemoveAll(e => e == "Freeze");
    }

    private IEnumerator UpdateStateCoroutine()
    {
        while (true)
        {
            if (StateList.Contains("Freeze"))
            {
                Debug.Log("해당 오브젝트에서 얼려진 상태가 발견된경우, 색상컬러 파랗게하는거랑,움직임 멈추게끔");
                enemyRenderer.material.color = Color.blue;
            }
            else
            {
                Debug.Log("해당 오브젝트에서 얼려진 상태가 해제");
                enemyRenderer.material.color = OriginalColor;
            }

            yield return null;
        }
    }


    private void Update() {
        //에너미가 움직이는 조건 정의
        //Target이 있을 때만 움직인다.
        //=> hasTarget(프로퍼티) 값에 따라 애니메이션 재생
        enemyAnimator.SetBool("HasTarget", hasTarget);

        //PrintStateList();
    }

    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    private IEnumerator UpdatePath() {
        //실시간으로 좀비가 가야 할 경로를 탐색. (=> 플레이어를 향해)
        //게임이 끝날 때까지 반복. 
        while(dead == false && IsPaused==false)
        {
            if (hasTarget)
            {
                //네비게이션 : 목적지까지의 경로를 자동으로 계산
                pathFinder.isStopped = false;

                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                //targetEntity에 새로운 값을 넣는 과정
                //네비게이션 이동 중지

                //주변 탐색 => 본인으로부터 반경 ~m 이내의 콜라이더 지대를 검색
                //반경 내의 모든 콜라이더 정보를 불러온다.
                //OverlapSphere 반경
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                for(int j=0; j<colliders.Length; j++)
                {
                    LivingEntity livingEntity = colliders[j].GetComponent<LivingEntity>();

                    if(livingEntity != null && livingEntity.dead == false)
                    {
                        targetEntity = livingEntity;
                        break;//타깃은 1명이니 이후 함수를 종료한다.
                    }
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
        Debug.Log("Enemy UpdatePath코루틴 종료 빠져나옴 코루틴");
        StopCoroutine(UpdatePath());
        //대상이 있을 때, 해당 대상이 있는 방향으로 경로를 갱신
        //대상이 없을 때, 새로운 대상을 추적.
    }

    // 데미지를 입었을때 실행할 처리
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        //부모 클래스의 OnDamage 메커니즘 그대로 실행.
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) {
        if(dead == false)
        {
            //죽지 않았을 떄에만 피격 효과 발동 => 효과음,피가 튀는 이펙트 효과
            enemyAudioPlayer.PlayOneShot(hitSound);

            //이펙트의 위치 : 맞은 위치
            //이펙트가 튀는 방향: 맞은 방향
            hitEffect.transform.position = hitPoint;
            //바라보는 방향을 일치시킨다.
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            //위치 선정 완료 후 재생
            hitEffect.Play();
        }

        base.OnDamage(damage, hitPoint, hitNormal);
        //부모 클래스의 OnDamage 메커니즘 그대로 실행.
    }

    // 사망 처리
    public override void Die() {
        //메커니즘은 부모와 동일
        base.Die();

        //다른 AI들의 경로를 방해하지 않기 위해
        Collider[] colliders = GetComponents<Collider>();
        //죽은 본인은 본인이 갖고 있는 모든 콜라이더를 비활성화한다.
        foreach(Collider collider in colliders)
        {
            collider.enabled = false;
        }

        //AI 추격 중지
        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        //이펙트 실행
        enemyAnimator.SetTrigger("Die");
        enemyAudioPlayer.PlayOneShot(deathSound);

        enemyspawner.UpdateUI();
    }

    private void OnTriggerStay(Collider other) {
        //에너미 공격 범위 내에 플레이어가 들어갔을 경우 데미지를 주는 기능
        //OnTriggerEnter : is Trigger가 true인 콜라이더 내부로 다른 객체가 들어갔을 경우
        //진입하는 타이밍에 1회 이벤트를 실행
     
        //OnTriggerStay : "" 객체가 트리거 범위 내에 있는동안 지속해서 이벤트를 실행
        //=>플레이어가 좀비의 공격 범위 내에 들어있는 내내 공격을 시도한다.

        if(dead == false && lastAttackTime + timeBetAttack < Time.time)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            if(attackTarget != null && attackTarget == targetEntity)
            {
                //공격 가능
                //최근 공격 시간 갱신
                lastAttackTime = Time.time; //마지막으로 상대를 떄린 시간을 현재 시간으로 갱신

                //공격 실행 => 내가 노리던 대상에게 데미지를 입힌다.
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                //공격 방향 : 플레이어와 좀비 사이의 거리~방향값 구하기.
                Vector3 hitNormal = transform.position - other.transform.position;

                attackTarget.OnDamage(damage,hitPoint,hitNormal);
            }
        }
        else
        {
            Debug.Log("Enemy공격 쿨타임>>");
        }
        //에너미 본인이 죽지 않은 시점에서
        //닿은 객체가 공격이 가능한 상태인지 > Living Entity를 상속받고 있는지
    }
}