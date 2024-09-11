using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Bazooka : Gun
{
    public GameObject explodePrefab; //Assign the ParticleSystem prefab in the inspector

    protected override void Shot()
    {
        Debug.Log("Gun_Bazooka 탕");

        //총을 쏜다 -> 무언가 맞는다 -> 누가 맞았는지 체크 -> 에너미일 경우 대미지

        //총이 무엇을 쏘았는지 계산하는 방법 -> 레이케스트(직선 광선)

        RaycastHit hit;

        Vector3 hitPosition = Vector3.zero;

        //레이케스트 공식
        //Physics.Raycast(광선의 시작 지점,광선이 어디로 뻗어나갈지 방향,충돌 정보(hit로 고정),최대광선길이)
        //시작지점:총구, 방향:총(총구)이 바라보는 방향
        //out~ : out 뒤에 적은 변수에 어떤 값을 저장한다.

        //레이케스트 함수의 결과는 true 혹은 false로 도출된다.
        //true : 사정거리 내에서 뭔가에 부딪혔을 때 반환.
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            //광선이 무언가에 부딪혔을 때 사용

            //총이 공격 가능 대상을 가격했는지 체크
            //공격 가능 대상이란? : IDamagable을 참조하는 객체. => 해당 객체로부터 IDamagable 속성을 가져올 수 있다.
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            //hit의 물체가 IDamagable을 갖고 있으면 어떤 정보가 저장될 것이고,
            //갖고 있지 않다면 target의 정보는 null이 될 것이다.

            if (target != null)
            {
                //부딪힌 물체가 데미지를 입을 수 있는 대상일 때
                //IDamagable을 갖고 있는 물체는 무조건 OnDamage 함수를 갖고 있다.

                //맞은 데미지: 총의 대미지
                //맞은 지점: 총알이 다은 지점
                //맞은 회전값: 부딪힌 장소의 회전값 - hit.normal
                target.OnDamage(damage, hit.point, hit.normal);
            }

            //공격 가능한 대상에게 총알이 닿았든, 공격 가능하지 않은 대상에게 닿았든(벽,울타리)
            //무조건 닿은 위치값을 저장한다.
            hitPosition = hit.point;
        }
        else
        {
            //레이케스트 상으로 사정거리 내에서 아무것도 부딪히지 않았을 떄 실행할 코드

            //충돌 위치: 총을 발사한 위치로부터 최대 사정거리 이후의 위치값
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        //물체에 맞았든,맞지 않았든 총을 발사하면 실행되는 행동을 아래에 기입

        //총 발사 이펙트
        StartCoroutine(ShotEffect(hitPosition));
        SpawnExplodesEffect(hitPosition);
        HitPos = hitPosition;

        //탄창 소모
        //현재 탄창 갯수 - 1
        magAmmo--;
        //만약 탄창이 0개면 -> 총의 상태를 '탄창 빔'으로 만든다.
        if (magAmmo <= 0)
        {
            state = State.Empty;
        }

        UIManager.instance.UpdateAmmoText(magAmmo, ammoRemain);
    }
    public void SpawnExplodesEffect(Vector3 hitPosition)
    {
        // Step 1: Instantiate the particle system at the specified position
        GameObject particleInstance = Instantiate(explodePrefab, hitPosition, Quaternion.identity);
        // Step 2: Start the coroutine to destroy the particle system after playback
        StartCoroutine(DestroyParticleWhenFinished(particleInstance));
    }
    // Coroutine to check if the particle system is finished and destroy it
    private System.Collections.IEnumerator DestroyParticleWhenFinished(GameObject particleObject)
    {
        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();

        // Wait until the particle system has completely stopped
        while (particleSystem != null && particleSystem.isPlaying)
        {
            yield return null; // Wait for the next frame
        }

        // Destroy the particle system object after playback ends
        Destroy(particleObject);
    }
}
