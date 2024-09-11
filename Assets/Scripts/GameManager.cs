using UnityEngine;

// 점수와 게임 오버 여부를 관리하는 게임 매니저
public class GameManager : MonoBehaviour {
    // 싱글톤 접근용 프로퍼티
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    private int score = 0; // 현재 게임 점수
    public bool isGameover { get; private set; } // 게임 오버 상태

    private void Awake() {
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start() {
        //플레이어가 죽을 경우 EndGame()을 실행할 수 있도록
        //플레이어 사망 이벤트에 추가

        //PlayerHealth가 상속받은 OnDeath 이벤트에 함수 추가
        //플레이어를 씬 내에서 찾아 참조
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore) {
        //현재 게임 점수 갱신
        //UI 갱신 => UIManager와 연동
        if(isGameover == false)
        {
            //이 함수를 실행하면 newScore만큼 점수를 추가한다.
            score += newScore;
            //점수 UI 갱신
            UIManager.instance.UpdateScoreText(score);
        }
    }

    // 게임 오버 처리
    public void EndGame() {
        //현재의 게임 상태를 '게임오버 되었다' 라고 갱신 (bool 값 갱신)
        //게임 오버 UI 활성화
        Debug.Log("게임오버");
        isGameover = true;

        UIManager.instance.SetActiveGameoverUI(true);
    }
}