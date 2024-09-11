using UnityEngine;
using UnityEngine.AI; // 내비메쉬 관련 코드

// 주기적으로 아이템을 플레이어 근처에 생성하는 스크립트
public class ItemSpawner : MonoBehaviour {
    public GameObject[] items; // 생성할 아이템들
    public Transform playerTransform; // 플레이어의 트랜스폼

    public float maxDistance = 5f; // 플레이어 위치로부터 아이템이 배치될 최대 반경

    public float timeBetSpawnMax = 7f; // 최대 시간 간격
    public float timeBetSpawnMin = 2f; // 최소 시간 간격
    
    private float timeBetSpawn; // 생성 간격
    private float lastSpawnTime; // 마지막 생성 시점

    //아이템 스포너 원리
    //1. 맵에 랜덤으로 아이템을 1개씩 스폰
    //=> 아래 아이템 종류 중 1개를 랜덤으로 스폰
    //2. 아이템 스폰 타이밍도 랜덤. (아이템이 빨리 나올수도,느리게 나올수도)
    //3. 스폰 장소 : 맵의 랜덤 위치 => 플레이어의 이동 반경 내의 랜덤 위치에 생성.
    //4. 아이템은 일정 시간이 흐르면 획득하지 않아도 사라진다.
    private void Start() {
        //변수 초기화
        //첫번째 아이템이 스폰될 때까지의 시간 => 생성 간격
        //=>생성 간격은 정해진 범위 내에서 랜덤값 도출.
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

        //마지막 아이템 스폰 시간은 0으로 초기화
        lastSpawnTime = 0;
    }

    // 주기적으로 아이템 생성 처리 실행
    private void Update() {
        
        //아이템을 생성할 주기가 돌아오면
        //아이템을 생성한다.
        if(lastSpawnTime + timeBetSpawn < Time.time)
        {
            //마지막 아이템 생성 시간 갱신
            lastSpawnTime = Time.time;
            //생성 주기도 다시 바꿔준다.
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            Spawn();

        }
    }

    // 실제 아이템 생성 처리
    private void Spawn() {
        Debug.Log("아이템 스폰, 이번 아이템 생성에 걸린 시간 : " + timeBetSpawn);

        //어디에 스폰할지 설정
        //캐릭터의 근처 일정 반경내의 랜덤 위치에 아이템 스폰
        //navimeshAgent => 정해진 반경 내에서 랜덤 지점을 선택

        Vector3 spawnPosition = GetRandomPointOnNavMesh(playerTransform.position,maxDistance);
        //어떤 아이템을 스폰할지 선택

        //지면으로부터 약간 떨어진 위치로 높이만 변경
        spawnPosition += new Vector3(0, 0.5f, 0);

        //구한 좌표에 랜덤 아이템 1개를 스폰한다.
        int randomindex = Random.Range(0, items.Length);
        GameObject selectitem = items[randomindex];

        GameObject spawn = Instantiate(selectitem, spawnPosition, Quaternion.identity);

        Destroy(spawn, 30f);

    }

    // 내비메시 위의 랜덤한 위치를 반환하는 메서드
    // center를 중심으로 distance 반경 안에서 랜덤한 위치를 찾는다
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance) {

        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        return hit.position;
    }
}