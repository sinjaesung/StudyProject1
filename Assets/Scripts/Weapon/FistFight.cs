using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class FistFight : MonoBehaviour
{
    public float Timer = 0f;
    public int FistFightVal;
    public Animator anim;

    public Transform attackArea;
    public float giveDamage = 10f;
    public float attackRadius;
    //public LayerMask knightLayer;
    public InventoryNew inventory;

    [SerializeField] Transform LeftHandPunch;
    [SerializeField] Transform RightHandPunch;
    [SerializeField] Transform LeftLegKick;

    //Hit Effect
    [SerializeField] GameObject HitEffect1_singlefist;
    [SerializeField] GameObject HitEffect2_doublefist;
    [SerializeField] GameObject HitEffect3_handkick;
    [SerializeField] GameObject HitEffect4_kickcombo;
    [SerializeField] GameObject HitEffect5_leftkick;

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            //�ƹ� ���콺 �ȴ����� ������
            Timer += Time.deltaTime;
        }
        else
        {
            Debug.Log("Fist Fight Mode On:���콺����Ŭ��down�ø��� Timer=0�Ǹ� �������On");
            anim.SetBool("FistFightActive", true);
            Timer = 0f;
        }

        if (Timer > 5f)
        {
            Debug.Log("Fist Fight Mode Off, ���콺�� �� ���ķ� 5���̻����� ������ �������Off");
            anim.SetBool("FistFightActive", false);
            inventory.fistFightMode = false;
            Timer = 0f;
            this.gameObject.GetComponent<FistFight>().enabled = false;
        }

        FistFightModes();
    }

    void FistFightModes()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("FistFightModes�� ���콺����Ŭ���� ��쿡�� �������εǸ�,���� �ִϸ��̼�1~5 ��������");
            FistFightVal = Random.Range(1, 7);

            if (FistFightVal == 1)
            {
                //Attack
                attackArea = LeftHandPunch;
                attackRadius = 0.5f;
                Attack();
                //Animation
                StartCoroutine(SingleFist());
            }

            if (FistFightVal == 2)
            {
                //Attack
                attackArea = RightHandPunch;
                attackRadius = 0.6f;
                Attack();
                //Animation
                StartCoroutine(DoubleFist());
            }

            if (FistFightVal == 3)
            {
                //Attack
                attackArea = RightHandPunch;
                attackArea = LeftLegKick;
                attackRadius = 0.7f;
                Attack();
                //Animation
                StartCoroutine(FirstFistKick());
            }

            if (FistFightVal == 4)
            {
                //Attack
                attackArea = LeftLegKick;
                attackRadius = 0.9f;
                Attack();
                //Animation
                StartCoroutine(KickCombo());
            }

            if (FistFightVal == 5)
            {
                //Attack
                attackArea = LeftLegKick;
                attackRadius = 0.9f;
                Attack();
                //Animation
                StartCoroutine(LeftKick());
            }
        }
    }

    void Attack()
    {
        Collider[] hitKnight = Physics.OverlapSphere(attackArea.position, attackRadius/*, knightLayer*/);

        foreach (Collider knight in hitKnight)
        {
            Debug.Log("FistFight [[Hitinfo]]:" + knight.transform.name);

            //�Ǽ� ���� 
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

            //hit�� ��ü�� IDamagable�� ���� ������ � ������ ����� ���̰�,
            //���� ���� �ʴٸ� target�� ������ null�� �� ���̴�.

            if (target != null)
            {
                //�ε��� ��ü�� �������� ���� �� �ִ� ����� ��
                //IDamagable�� ���� �ִ� ��ü�� ������ OnDamage �Լ��� ���� �ִ�.

                //���� ������: ���� �����
                target.OnDamage(giveDamage);
                if (FistFightVal == 1)
                {
                    Instantiate(HitEffect1_singlefist, attackArea.transform.position, Quaternion.identity);
                }
                else if (FistFightVal == 2)
                {
                    Instantiate(HitEffect2_doublefist, attackArea.transform.position, Quaternion.identity);
                }
                else if (FistFightVal == 3)
                {
                    Instantiate(HitEffect3_handkick, attackArea.transform.position, Quaternion.identity);
                }
                else if (FistFightVal == 4)
                {
                    Instantiate(HitEffect4_kickcombo, attackArea.transform.position, Quaternion.identity);
                }
                else if (FistFightVal == 5)
                {
                    Instantiate(HitEffect5_leftkick, attackArea.transform.position, Quaternion.identity);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackArea == null)
            return;

        Gizmos.DrawWireSphere(attackArea.position, attackRadius);
    }

    IEnumerator SingleFist()
    {
        anim.SetBool("SingleFist", true);
        //anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.7f);
        anim.SetBool("SingleFist", false);
        //anim.SetFloat("movementValue", 0f);
    }

    IEnumerator DoubleFist()
    {
        anim.SetBool("DoubleFist", true);
        //anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("DoubleFist", false);
        //anim.SetFloat("movementValue", 0f);
    }

    IEnumerator FirstFistKick()
    {
        anim.SetBool("FirstFistKick", true);
        //anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("FirstFistKick", false);
        //anim.SetFloat("movementValue", 0f);
    }

    IEnumerator KickCombo()
    {
        anim.SetBool("KickCombo", true);
        //anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("KickCombo", false);
        //anim.SetFloat("movementValue", 0f);
    }

    IEnumerator LeftKick()
    {
        anim.SetBool("LeftKick", true);
        //anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("LeftKick", false);
        //anim.SetFloat("movementValue", 0f);
    }
}
