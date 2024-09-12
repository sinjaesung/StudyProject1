using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMeleeAttack : MonoBehaviour
{
    public float Timer = 0f;
    public int SingleMeleeVal;
    public Animator anim;

    public Transform attackArea;
    public float giveDamage = 10f;
    public float attackRadius;
    //public LayerMask knightLayer;

    public AudioSource SwordAudioPlayer; //  소리 재생기
    [SerializeField] public AudioClip SwordClip; // 소리(var)
    [SerializeField] public AudioClip SwordClip2; // 소리(var)

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            Timer += Time.deltaTime;
        }
        else
        {
            Debug.Log("SingleMeleeAttack Mode On:마우스왼쪽클릭down시마다 Timer=0되며 대전모드On");
            anim.SetBool("SingleHandAttackActive", true);
            Timer = 0f;
        }

        if (Timer > 5f)
        {
            Debug.Log("FistSingleMeleeAttack Mode Off, 마우스를 뗀 이후로 5초이상지난 시점에 대전모드Off");
            anim.SetBool("SingleHandAttackActive", false);
        }

        SingleMeleeModes();
    }

    void SingleMeleeModes()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SingleMeleeVal = Random.Range(1, 7);

            if (SingleMeleeVal == 1)
            {
                Attack();
                //Animation
                StartCoroutine(SingleAttack1());
            }

            if (SingleMeleeVal == 2)
            {
                Attack();
                //Animation
                StartCoroutine(SingleAttack2());
            }

            if (SingleMeleeVal == 3)
            {
                Attack();
                //Animation
                StartCoroutine(SingleAttack3());
            }

            if (SingleMeleeVal == 4)
            {
                Attack();
                //Animation
                StartCoroutine(SingleAttack4());
            }

            if (SingleMeleeVal == 5)
            {
                Attack();
                //Animation
                StartCoroutine(SingleAttack5());
            }
        }
    }

    void Attack()
    {
        Collider[] hitKnight = Physics.OverlapSphere(attackArea.position, attackRadius/*, knightLayer*/);

        foreach (Collider knight in hitKnight)
        {
            Debug.Log("SingleMeleeAttack [[Hitinfo]]:" + knight.transform.name);

            /* KnightAI knightAI = knight.GetComponent<KnightAI>();
             KnightAI2 knightAI2 = knight.GetComponent<KnightAI2>();

             if (knightAI != null)
             {
                 knightAI.TakeDamage(giveDamage);
             }
             if (knightAI2 != null)
             {
                 knightAI2.TakeDamage(giveDamage);
             }*/
            IDamageable target = knight.GetComponent<IDamageable>();

            //hit의 물체가 IDamagable을 갖고 있으면 어떤 정보가 저장될 것이고,
            //갖고 있지 않다면 target의 정보는 null이 될 것이다.

            if (target != null)
            {
                //부딪힌 물체가 데미지를 입을 수 있는 대상일 때
                //IDamagable을 갖고 있는 물체는 무조건 OnDamage 함수를 갖고 있다.
                Debug.Log("SingleMeleeAttack target is exists");
                //맞은 데미지: 총의 대미지
                SwordAudioPlayer.PlayOneShot(SwordClip);
                target.OnDamage(giveDamage, knight.ClosestPoint(attackArea.position),(knight.transform.position - attackArea.position));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackArea == null)
            return;

        Gizmos.DrawWireSphere(attackArea.position, attackRadius);
    }

    IEnumerator SingleAttack1()
    {
        anim.SetBool("SingleAttack1", true);
        //anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack1", false);

        //anim.SetFloat("movementValue", 0f);
    }

    IEnumerator SingleAttack2()
    {
        anim.SetBool("SingleAttack2", true);
        //anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack2", false);
        //anim.SetFloat("movementValue", 0f);
    }

    IEnumerator SingleAttack3()
    {
        anim.SetBool("SingleAttack3", true);
        //anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack3", false);
        //anim.SetFloat("movementValue", 0f);
    }

    IEnumerator SingleAttack4()
    {
        anim.SetBool("SingleAttack4", true);
        //anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack4", false);
        //anim.SetFloat("movementValue", 0f);
    }

    IEnumerator SingleAttack5()
    {
        anim.SetBool("SingleAttack5", true);
        SwordAudioPlayer.PlayOneShot(SwordClip2);
        //anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack5", false);
        SwordAudioPlayer.PlayOneShot(SwordClip2);
        //anim.SetFloat("movementValue", 0f);
    }
}
