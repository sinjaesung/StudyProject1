using UnityEngine;

// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트들이 사용할 수 있도록 제공
// 캐릭터의 키보드 센서

public class PlayerInput : MonoBehaviour {
    public string moveAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string rotateAxisName = "Horizontal"; // 좌우 회전을 위한 입력축 이름
    public string fireButtonName = "Fire1"; // 발사를 위한 입력 버튼 이름
    public string reloadButtonName = "Reload"; // 재장전을 위한 입력 버튼 이름

    // 값 할당은 내부에서만 가능
    public float move { get; private set; } // 감지된 움직임 입력값
    public float rotate { get; private set; } // 감지된 회전 입력값
    public bool fire { get; private set; } // 감지된 발사 입력값
    public bool fireDown { get; private set; }
    public bool reload { get; private set; } // 감지된 재장전 입력값

    // 매프레임 사용자 입력을 감지
    private void Update() {
        //move,rotate,fire,reload 각각의 키보드 입력 결과값을 저장한다.
        //매 프레임 갱신한다

        //게임이 진행중일 때만 키보드 입력을 감지한다.
        //게임이 진행중이지 않을 때는 초기값으로 세팅한다.

        //tip : 다른 컴포넌트를 불러올 때는 해당 컴포넌트가 존재하는지 여부도 함께 체크해주는 편이 좋다.
        if (GameManager.instance!= null && GameManager.instance.isGameover)
        {
            //모든 변수값을 초기화한다.
            move = 0;
            rotate = 0;
            fire = false;
            reload = false;
            fireDown = false;

            //초기화 이후 함수 종료
            return;
        }

        //아랫줄이 실행되는 조건 : 게임오버가 아니다.
        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);

        fire = Input.GetButton(fireButtonName);

        fireDown = Input.GetMouseButtonDown(0);

        //재장전을 연속으로 눌러 사용하지 않기 때문에
        //누른 순간 1회만 호출되도록 한다.
        reload = Input.GetButtonDown(reloadButtonName);
    }
}