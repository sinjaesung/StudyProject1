using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

// 총을 구현한다
public class Wand : MonoBehaviour
{
    public Transform fireTransform; // 빔이 발사될 위치

    private LineRenderer BeamLineRenderer; // 빔 궤적을 그리기 위한 렌더러(var)

    //private AudioSource gunAudioPlayer; // 소리 재생기
    //[SerializeField] public AudioClip shotClip; // 발사 소리(var)

    [SerializeField] public float damage = 25; // 공격력(var)

    [SerializeField] public float timeBetFire = 0.12f; // 빔 발사 간격(var)
    public float lastFireTime; //마지막으로 발사한 시점

    public Vector3 HitPos;

    public Camera mainCamera;

    // Reference to the projectile prefab
    public GameObject projectilePrefab;
    // Array to hold nearby target points
    public Vector3[] nearbyPoints = new Vector3[5];
    // Height from which projectiles will spawn (sky)
    public float spawnHeight = 50f;
    // Speed of the projectile's movement
    public float projectileSpeed = 10f;

    private PlayerInput playerInput; // 플레이어의 입력

    private void Awake()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        BeamLineRenderer = GetComponent<LineRenderer>();
        //gunAudioPlayer = GetComponent<AudioSource>();

        //컴포넌트 설정 누락을 방지하기 위해
        //라인렌더러 설정을 이곳에서 진행.
        //라인렌더러 점 갯수 설정 이후
        BeamLineRenderer.positionCount = 2;

        //라인렌더를 잠시 비활성화
        BeamLineRenderer.enabled = false;

        playerInput = GetComponent<PlayerInput>();
    }
    private void OnEnable()
    {
        //쏜 시점으로 0으로 초기화.
        lastFireTime = 0;
    }

    // 발사 시도
    public void Fire()
    {
        //상태 체크: 현재 발사를 할 수 있는 상태인가?
        //총알 발사 간격만큼 이상의 시간이 흘렀느냐
        //둘다 만족할 경우에 발사 처리를 진행한다.

        //발사 시간 체크 : 마지막 발사 시간 + 발사 간격을 한 시간이 현재 시간보다 작다
        //시간이 더 흘렀다는 뜻이니 발사가 가능하도록.
        Debug.Log($"빔 쏘기 시도{Time.time} >= {lastFireTime} + {timeBetFire}({lastFireTime + timeBetFire}>>");

        if (Time.time >= lastFireTime + timeBetFire)
        {
            Debug.Log("빔 쏘기 가능>>");
            Shot();
            //빔을 쏘았으니 마지막 빔을 쏜 시간을 현재로 갱신한다.
            lastFireTime = Time.time;
        }
        else
        {
            Debug.Log("빔 쿨타임");
        }
    }

    // 실제 발사 처리
    protected virtual void Shot()
    {
        Debug.Log("지잉");
        // Step1: Get Mouse Position in Screen Coordinates
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Step2 : Convert Mouse Position to World Coordinates
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            //Get the point where the ray intersects the plane
            Vector3 pointInWorld = ray.GetPoint(rayDistance);
            Debug.Log("WandShot MouseCursor to world Plane raycast pointInWorld>>" + pointInWorld);

            StartCoroutine(ShotEffect(pointInWorld));

            SetNearbyPoints(pointInWorld);
        }
    }

    void SetNearbyPoints(Vector3 pointInWorld_)
    {
        float offsetDistance = 3f; // Distance to offset nearby points

        nearbyPoints[0] = pointInWorld_ + new Vector3(offsetDistance, 0, 0);
        nearbyPoints[1] = pointInWorld_ + new Vector3(-offsetDistance, 0, 0);
        nearbyPoints[2] = pointInWorld_ + new Vector3(0, 0, offsetDistance);
        nearbyPoints[3] = pointInWorld_ + new Vector3(0, 0, -offsetDistance);
        nearbyPoints[4] = pointInWorld_ + new Vector3(offsetDistance, 0, offsetDistance);

        LaunchProjectiles(pointInWorld_);
    }
    // Spawn projectiles in the sky and make them fly down diagonally
    void LaunchProjectiles(Vector3 pointInWorld_)
    {
        Vector3[] allTargetPoints = new Vector3[6];
        allTargetPoints[0] = pointInWorld_;
        // Include the nearby points in the array of all target points
        for (int i = 0; i < nearbyPoints.Length; i++)
        {
            allTargetPoints[i + 1] = nearbyPoints[i];
        }

        // Spawn a projectile at each point and make it move towards its target
        foreach (Vector3 target in allTargetPoints)
        {
            // Calculate spawn position in the sky above the target
            Vector3 spawnPosition = target + new Vector3(0, spawnHeight, spawnHeight);

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            // Start moving the projectile towards the target point
            StartCoroutine(MoveProjectileToTarget(projectile, target));
        }
    }

    // Coroutine to move the projectile towards the target
    private IEnumerator MoveProjectileToTarget(GameObject projectile, Vector3 target)
    {
        while (projectile != null)
        {
            // Move the projectile towards the target position
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, target, projectileSpeed * Time.deltaTime);

            // If the projectile reaches the target, destroy it
            if (Vector3.Distance(projectile.transform.position, target) < 0.1f)
            {
                Destroy(projectile);
                yield break;
            }

            yield return null;
        }
    }
    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    protected IEnumerator ShotEffect(Vector3 hitPosition)
    {

        //소리는 한 번만 재생한다.
       // gunAudioPlayer.PlayOneShot(shotClip);

        //총알 궤적 그려주기

        //선을 긋기 위해선 시작점,끝점을 알아야 한다.
        //시작점 : 총구
        //끝나는 점 : 부딪힌 위치
        //SetPosition(몇 번째 점인지, 값)

        BeamLineRenderer.SetPosition(0, fireTransform.position);
        BeamLineRenderer.SetPosition(1, hitPosition);

        //시작점,끝점을 지정한 이후에
        //선이 보이도록 활성화한다,.
        BeamLineRenderer.enabled = true;

        //0.03초만 선이 보이고, 이후 바로 선이 사라진다.
        yield return new WaitForSeconds(0.03f);

        BeamLineRenderer.enabled = false;
    }
}