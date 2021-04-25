using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일반 동작
/// 1. 조이스틱 (터치 시작이 조이스틱이면 작동)
/// 2. 카메라 영역 ( 드래그 시 작동)
/// 3. 버튼 (드래그 시 비작동: 카메라 작동 시 비작동)
/// 
/// 동시 작동할 때,
/// 1. 조이스틱 + 버튼
/// 2. 카메라 + 버튼
/// 3. 조이스틱 + 카메라
/// 4. 카메라 + 조이스틱
/// </summary>
public class TouchManager : Singleton<TouchManager>
{
    public struct InputUIInfo //UI 조작 정보 구조체
    {
        public bool move;
        public bool camera;
        public bool jump;
        public bool attack;
        public bool skill;
    }

    private InputUIInfo inputUI;
    public InputUIInfo InputUI { get { return inputUI; } }

    // Start is called before the first frame update
    Touch touch;
    void Start()
    {
        inputUI.move = false;
        inputUI.camera = false;
        inputUI.jump = false;
        inputUI.attack = false;
        inputUI.skill = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount < 1) return; //터치가 없을 경우


    }
    
    public void setMove(bool value)
    {
        inputUI.move = value;
    }
    public void setCamera(bool value)
    {
        inputUI.camera = value;
    }
    public void setJump(bool value)
    {
        inputUI.jump = value;
    }
    public void setAttack(bool value)
    {
        inputUI.attack = value;
    }
    public void setSkill(bool value)
    {
        inputUI.skill = value;
    }

}
