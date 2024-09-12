using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryNew : MonoBehaviour
{
    [Header("Weapons To use")]
    public GameObject Weapon1;//기본 권총
    public GameObject Weapon2;//샷건무기2
    public GameObject Weapon3;//바주카 무기3
    public GameObject Weapon4;//스노우 완드
    public GameObject Weapon5;//파이어스워드
    public GameObject Weapon6;//스워드(검술)

    public bool isWeapon1Active = false;//기본권총 활성여부
    public bool isWeapon2Active = false;//샷건 활성여부
    public bool isWeapon3Active = false;//바주카 활성여부
    public bool isWeapon4Active = false;//스노우완드 활성여부
    public bool isWeapon5Active = false;//파이어스워드 활성여부
    public bool isWeapon6Active = false;//스워드(검술) 활성여부

    public bool fistFightMode = false;

    [Header("Scripts")]
    public SingleMeleeAttack SMAS;//검무기
    public FistFight fistFight;//맨손 싸움
    public PlayerShooter playershooterScript;//기본무기
    public PlayerShooter2 playershooter2Script;//샷건무기2
    public PlayerShooter3 playershooter3Script;//바주카무기3
    public WandShooter1 wandshooter1Script;//스노우완드
    public WandShooter2 wandshooter2Script;//파이어스워드

    public GameManager GM;
    public Animator anim;


    [Header("Current Weapons UI")]
    public GameObject NoWeapon;//UI 아무것도 안끼고있을때
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
        //총무기들의 경우 장전중에는 무기변경 제한
        if (playershooterScript.gun.state == Gun.State.Reloading ||
        playershooter2Script.gun.state == Gun.State.Reloading ||
        playershooter3Script.gun.state == Gun.State.Reloading
        )
        {
            return;
        }

        if (isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active == false && isWeapon6Active == false && fistFightMode == false)
        {
            //기본 너클 모드 idle
            NoWeapon.SetActive(true);
            fistFightMode = true;
            isRifleActive();
        }

        if (Input.GetMouseButtonDown(0) && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active == false && isWeapon6Active == false &&  fistFightMode == false)
        {
            //기본 너클 모드 전투모드
            fistFightMode = true;
            isRifleActive();
        }

        if (Input.GetKeyDown("1") && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active == false && isWeapon6Active == false)
        {
            //기본 권총 꺼내기
            isWeapon1Active = true;
            isRifleActive();
            CurrentWeapon1.SetActive(true);
            NoWeapon.SetActive(false);
            fistFightMode = false;
        }
        else if (Input.GetKeyDown("1") && isWeapon1Active == true)
        {
            //기본 권총 집어넣기
            isWeapon1Active = false;
            isRifleActive();
            CurrentWeapon1.SetActive(false);
        }

        if (Input.GetKeyDown("2") && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active==false && isWeapon6Active==false)
        {
            //샷건 꺼내기
            isWeapon2Active = true;
            isRifleActive();
            CurrentWeapon2.SetActive(true);
            NoWeapon.SetActive(false); 
            fistFightMode = false;
        }
        else if (Input.GetKeyDown("2") && isWeapon2Active == true)
        {
            //샷건 집어넣기
            isWeapon2Active = false;
            isRifleActive();
            CurrentWeapon2.SetActive(false);
        }

        if (Input.GetKeyDown("3") && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active == false && isWeapon6Active == false)
        {
            //바주카 꺼내기
            isWeapon3Active = true;
            isRifleActive();
            CurrentWeapon3.SetActive(true);
            NoWeapon.SetActive(false);
            fistFightMode = false;
        }
        else if (Input.GetKeyDown("3") && isWeapon3Active == true)
        {
            //바주카 집어넣기
            isWeapon3Active = false;
            isRifleActive();
            CurrentWeapon3.SetActive(false);
        }


        if (Input.GetKeyDown("4") && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active==false && isWeapon6Active == false)
        {
            //얼음무기 꺼내기
            isWeapon4Active = true;
            isRifleActive();
            CurrentWeapon4.SetActive(true);
            NoWeapon.SetActive(false);
            fistFightMode = false;
        }
        else if (Input.GetKeyDown("4") && isWeapon4Active == true)
        {
            //얼음무기 집어넣기
            isWeapon4Active = false;
            isRifleActive();
            CurrentWeapon4.SetActive(false);
        }

        if (Input.GetKeyDown("5") && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active == false && isWeapon6Active == false)
        {
            //불무기 꺼내기
            isWeapon5Active = true;
            isRifleActive();
            CurrentWeapon5.SetActive(true);
            NoWeapon.SetActive(false);
            fistFightMode = false;
        }
        else if (Input.GetKeyDown("5") && isWeapon5Active == true)
        {
            //불무기 집어넣기
            isWeapon5Active = false;
            isRifleActive();
            CurrentWeapon5.SetActive(false);
        }

        if (Input.GetKeyDown("6") && isWeapon1Active == false && isWeapon2Active == false && isWeapon3Active == false && isWeapon4Active == false && isWeapon5Active == false && isWeapon6Active == false)
        {
            //검무기 꺼내기
            isWeapon6Active = true;
            isRifleActive();
            CurrentWeapon6.SetActive(true);
            NoWeapon.SetActive(false);
            fistFightMode = false;
        }
        else if (Input.GetKeyDown("6") && isWeapon6Active == true)
        {
            //검무기 집어넣기
            isWeapon6Active = false;
            isRifleActive();
            CurrentWeapon6.SetActive(false);
        }
    }

    void isRifleActive()
    {
        if (fistFightMode == true)
        {
            //기본 맨손 모드
            fistFight.GetComponent<FistFight>().enabled = true;
        }

        if (isWeapon6Active == true)
        {
            StartCoroutine(Weapon6GO());//검투모드 발동
            SMAS.GetComponent<SingleMeleeAttack>().enabled = true;
            anim.SetBool("SingleHandAttackActive", true);
            anim.SetBool("FistFightActive", false);
        }
        if (isWeapon6Active == false)
        {
            StartCoroutine(Weapon6GO());//검투모드 해제
            SMAS.GetComponent<SingleMeleeAttack>().enabled = false;
            anim.SetBool("SingleHandAttackActive", false);
        }

        if (isWeapon1Active == true)
        {
            StartCoroutine(Weapon1GO());//기본권총모드
            playershooterScript.enabled = true;
            anim.SetBool("RifleActive", true);
            anim.SetBool("FistFightActive", false);
        }
        if (isWeapon1Active == false)
        {
            StartCoroutine(Weapon1GO());//기본권총모드
            playershooterScript.enabled = false;
            anim.SetBool("RifleActive", false);
        }
        if (isWeapon2Active == true)
        {
            StartCoroutine(Weapon2GO());//샷건
            playershooter2Script.enabled = true;
            anim.SetBool("RifleActive", true);
            anim.SetBool("FistFightActive", false);
        }
        if (isWeapon2Active == false)
        {
            StartCoroutine(Weapon2GO());//샷건
            playershooter2Script.enabled = false;
            anim.SetBool("RifleActive", false);
        }

        if (isWeapon3Active == true)
        {
            StartCoroutine(Weapon3GO());//바주카
            playershooter3Script.enabled = true;
            anim.SetBool("BazookaActive", true);
            anim.SetBool("FistFightActive", false);
        }
        if (isWeapon3Active == false)
        {
            StartCoroutine(Weapon3GO());//바주카
            playershooter3Script.enabled = false;
            anim.SetBool("BazookaActive", false);
        }

        //얼음무기
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

        //불무기
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
