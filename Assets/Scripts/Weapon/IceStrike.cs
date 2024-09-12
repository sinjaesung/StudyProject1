using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStrike : MonoBehaviour
{
    [SerializeField] private float iceTime = 3;
    [SerializeField] private float DestroyTime = 8;
    [SerializeField] private float StartTIme;
    private void Awake()
    {
        StartTIme = Time.time;
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();

        //hit�� ��ü�� IDamagable�� ���� ������ � ������ ����� ���̰�,
        //���� ���� �ʴٸ� target�� ������ null�� �� ���̴�.

        if (target != null)
        {
            Debug.Log("IceStrike Freezing Target>>" + other.transform.name);
            //�ε��� ��ü�� �������� ���� �� �ִ� ����� ��
            //IDamagable�� ���� �ִ� ��ü�� ������ OnDamage �Լ��� ���� �ִ�.

            //���� ������: ���� �����
            //���� ����: �Ѿ��� ���� ����
            //���� ȸ����: �ε��� ����� ȸ���� - hit.normal
            if (other.GetComponent<Enemy>() != null)
            { 
                 StartCoroutine(FreezeObject(other.GetComponent<Enemy>()));        
            }
        }
    }

    //Coroutine to freeze the object for 3 seconds
    private IEnumerator FreezeObject(Enemy target)
    {
        target.AddStateList("Freeze");
        target.IsPaused = true;//���������� �ڷ�ƾ ����
        Debug.Log("Object frozen for 3 seconds."+target.transform.name);

        yield return new WaitForSeconds(iceTime);

        target.DeleteStateListItem("Freeze");
        target.IsPaused = false; //�簳�� �ڷ�ƾ�� ���ư����ְ���
        target.ReStartUpdatePathCoroutine();//�ڷ�ƾ�簳
        Debug.Log("Object thawed."+target.transform.name);
       
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Update()
    {
        if(Time.time - StartTIme >= DestroyTime)
        {
            Debug.Log("���� �������� 8���̻� ���� ����� ����ó��");
            Destroy(gameObject);
        }
    }
}
