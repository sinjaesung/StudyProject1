using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryNew : MonoBehaviour
{
    [Header("Weapons To use")]
    public GameObject Weapon1;//�⺻ ����
    public GameObject Weapon2;//���ǹ���2
    public GameObject Weapon3;//����ī ����3
    public GameObject Weapon4;//����� �ϵ�
    public GameObject Weapon5;//���̾����
    public GameObject Weapon6;//������(�˼�)

    public bool isWeapon1Active = false;//�⺻���� Ȱ������
    public bool isWeapon2Active = false;//���� Ȱ������
    public bool isWeapon3Active = false;//����ī Ȱ������
    public bool isWeapon4Active = false;//�����ϵ� Ȱ������
    public bool isWeapon5Active = false;//���̾���� Ȱ������
    public bool isWeapon6Active = false;//������(�˼�) Ȱ������

    public bool fistFightMode = false;

    [Header("Scripts")]
    public SingleMeleeAttack SMAS;//�˹���
    public FistFight fistFight;//�Ǽ� �ο�
    public PlayerShooter playershooterScript;//�⺻����
    public PlayerShooter2 playershooter2Script;//���ǹ���2
    public PlayerShooter3 playershooter3Script;//����ī����3
    public WandShooter1 wandshooter1Script;//�����ϵ�
    public WandShooter2 wandshooter2Script;//���̾����

    public GameManager GM;
    public Animator anim;


    [Header("Current Weapons UI")]
    public GameObject NoWeapon;//UI �ƹ��͵� �ȳ���������
    public GameObject CurrentWeapon1;
    public GameObject CurrentWeapon2;
    public GameObject CurrentWeapon3;
    public GameObject CurrentWeapon4;
    public GameObject CurrentWeapon5;
    public GameObject CurrentWeapon6;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        //�ѹ������ ��� �����߿��� ���⺯�� ����
        if (playershooterScript.gun.state == Gun.State.Reloading ||
        playershooter2Script.gun.state == Gun.State.Reloading ||
        playershooter3Script.gun.state == Gun.State.Reloading
        )
        {
            return;
        }

        if (isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active == false && isWeapon6Active == false && fistFightMode == false)
        {
            //�⺻ ��Ŭ ��� idle
            NoWeapon.SetActive(true);
            fistFightMode = true;
            isRifleActive();
        }

        if (Input.GetMouseButtonDown(0) && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active == false && isWeapon6Active == false &&  fistFightMode == false)
        {
            //�⺻ ��Ŭ ��� �������
            fistFightMode = true;
            isRifleActive();
        }

        if (Input.GetKeyDown("1") && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active == false && isWeapon6Active == false)
        {
            //�⺻ ���� ������
            isWeapon1Active = true;
            isRifleActive();
            CurrentWeapon1.SetActive(true);
            NoWeapon.SetActive(false);
            fistFightMode = false;
        }
        else if (Input.GetKeyDown("1") && isWeapon1Active == true)
        {
            //�⺻ ���� ����ֱ�
            isWeapon1Active = false;
            isRifleActive();
            CurrentWeapon1.SetActive(false);
        }

        if (Input.GetKeyDown("2") && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active==false && isWeapon6Active==false)
        {
            //���� ������
            isWeapon2Active = true;
            isRifleActive();
            CurrentWeapon2.SetActive(true);
            NoWeapon.SetActive(false); 
            fistFightMode = false;
        }
        else if (Input.GetKeyDown("2") && isWeapon2Active == true)
        {
            //���� ����ֱ�
            isWeapon2Active = false;
            isRifleActive();
            CurrentWeapon2.SetActive(false);
        }

        if (Input.GetKeyDown("3") && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active == false && isWeapon6Active == false)
        {
            //����ī ������
            isWeapon3Active = true;
            isRifleActive();
            CurrentWeapon3.SetActive(true);
            NoWeapon.SetActive(false);
            fistFightMode = false;
        }
        else if (Input.GetKeyDown("3") && isWeapon3Active == true)
        {
            //����ī ����ֱ�
            isWeapon3Active = false;
            isRifleActive();
            CurrentWeapon3.SetActive(false);
        }


        if (Input.GetKeyDown("4") && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active==false && isWeapon6Active == false)
        {
            //�������� ������
            isWeapon4Active = true;
            isRifleActive();
            CurrentWeapon4.SetActive(true);
            NoWeapon.SetActive(false);
            fistFightMode = false;
        }
        else if (Input.GetKeyDown("4") && isWeapon4Active == true)
        {
            //�������� ����ֱ�
            isWeapon4Active = false;
            isRifleActive();
            CurrentWeapon4.SetActive(false);
        }

        if (Input.GetKeyDown("5") && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active == false && isWeapon6Active == false)
        {
            //�ҹ��� ������
            isWeapon5Active = true;
            isRifleActive();
            CurrentWeapon5.SetActive(true);
            NoWeapon.SetActive(false);
            fistFightMode = false;
        }
        else if (Input.GetKeyDown("5") && isWeapon5Active == true)
        {
            //�ҹ��� ����ֱ�
            isWeapon5Active = false;
            isRifleActive();
            CurrentWeapon5.SetActive(false);
        }

        if (Input.GetKeyDown("6") && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active == false && isWeapon6Active == false)
        {
            //�˹��� ������
            isWeapon6Active = true;
            isRifleActive();
            CurrentWeapon6.SetActive(true);
            NoWeapon.SetActive(false);
            fistFightMode = false;
        }
        else if (Input.GetKeyDown("6") && isWeapon6Active == true)
        {
            //�˹��� ����ֱ�
            isWeapon6Active = false;
            isRifleActive();
            CurrentWeapon6.SetActive(false);
        }
    }

    void isRifleActive()
    {
        if (fistFightMode == true)
        {
            //�⺻ �Ǽ� ���
            fistFight.GetComponent<FistFight>().enabled = true;
        }

        if (isWeapon6Active == true)
        {
            StartCoroutine(Weapon6GO());//������� �ߵ�
            SMAS.GetComponent<SingleMeleeAttack>().enabled = true;
            anim.SetBool("SingleHandAttackActive", true);
            anim.SetBool("FistFightActive", false);
        }
        if (isWeapon6Active == false)
        {
            StartCoroutine(Weapon6GO());//������� ����
            SMAS.GetComponent<SingleMeleeAttack>().enabled = false;
            anim.SetBool("SingleHandAttackActive", false);
        }

        if (isWeapon1Active == true)
        {
            StartCoroutine(Weapon1GO());//�⺻���Ѹ��
            playershooterScript.enabled = true;
            anim.SetBool("RifleActive", true);
            anim.SetBool("FistFightActive", false);
        }
        if (isWeapon1Active == false)
        {
            StartCoroutine(Weapon1GO());//�⺻���Ѹ��
            playershooterScript.enabled = false;
            anim.SetBool("RifleActive", false);
        }
        if (isWeapon2Active == true)
        {
            StartCoroutine(Weapon2GO());//����
            playershooter2Script.enabled = true;
            anim.SetBool("RifleActive", true);
            anim.SetBool("FistFightActive", false);
        }
        if (isWeapon2Active == false)
        {
            StartCoroutine(Weapon2GO());//����
            playershooter2Script.enabled = false;
            anim.SetBool("RifleActive", false);
        }

        if (isWeapon3Active == true)
        {
            StartCoroutine(Weapon3GO());//����ī
            playershooter3Script.enabled = true;
            anim.SetBool("BazookaActive", true);
            anim.SetBool("FistFightActive", false);
        }
        if (isWeapon3Active == false)
        {
            StartCoroutine(Weapon3GO());//����ī
            playershooter3Script.enabled = false;
            anim.SetBool("BazookaActive", false);
        }

        //��������
        if (isWeapon4Active == true)
        {
            StartCoroutine(Weapon4GO());
            wandshooter1Script.enabled = true;
            anim.SetBool("BazookaActive", true);
            anim.SetBool("FistFightActive", false);
        }
        if (isWeapon4Active == false)
        {
            StartCoroutine(Weapon4GO());
            wandshooter1Script.enabled = false;
            anim.SetBool("BazookaActive", false);
        }

        //�ҹ���
        if (isWeapon5Active == true)
        {
            StartCoroutine(Weapon5GO());
            wandshooter2Script.enabled = true;
            anim.SetBool("BazookaActive", true);
            anim.SetBool("FistFightActive", false);
        }
        if (isWeapon5Active == false)
        {
            StartCoroutine(Weapon5GO());
            wandshooter2Script.enabled = false;
            anim.SetBool("BazookaActive", false);
        }
    }

    IEnumerator Weapon6GO()
    {
        if (!isWeapon6Active)
        {
            Weapon6.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        if (isWeapon6Active)
        {
            Weapon6.SetActive(true);
        }
    }
    IEnumerator Weapon1GO()
    {
        if (!isWeapon1Active)
        {
            Weapon1.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        if (isWeapon1Active)
        {
            Weapon1.SetActive(true);
        }
    }
    IEnumerator Weapon2GO()
    {
        if (!isWeapon2Active)
        {
            Weapon2.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        if (isWeapon2Active)
        {
            Weapon2.SetActive(true);
        }
    }
    IEnumerator Weapon3GO()
    {
        if (!isWeapon3Active)
        {
            Weapon3.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        if (isWeapon3Active)
        {
            Weapon3.SetActive(true);
        }
    }
    IEnumerator Weapon4GO()
    {
        if (!isWeapon4Active)
        {
            Weapon4.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        if (isWeapon4Active)
        {
            Weapon4.SetActive(true);
        }
    }
    IEnumerator Weapon5GO()
    {
        if (!isWeapon5Active)
        {
            Weapon5.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        if (isWeapon5Active)
        {
            Weapon5.SetActive(true);
        }
    }
}
