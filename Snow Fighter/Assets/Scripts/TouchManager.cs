using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        public float move;
        public float camera;
        public float jump;
        public float attack;
        public float skill;
    }

    private InputUIInfo inputUI;
    public InputUIInfo InputUI { get { return inputUI; } }

    PlayerSightScript camera = null;

    // Start is called before the first frame update
    Touch touch;
    void Start()
    {
        inputUI.move = -1;
        inputUI.camera = -1;
        inputUI.jump = -1;
        inputUI.attack = -1;
        inputUI.skill = -1;

        camera = Camera.main.GetComponent<PlayerSightScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount < 1) return; //터치가 없을 경우
        GetTouchInput();
    }

    private void GetTouchInput()
    {
        //터치 입력 갯수
        for(int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);

            switch (t.phase)
            {
                case TouchPhase.Began:
                    if(t.position.x > Screen.width / 2)
                    {
                        inputUI.camera = t.fingerId;
                        camera.IsCameraRotating = true;
                    }
                    break;

                case TouchPhase.Moved:
                    if (!EventSystem.current.IsPointerOverGameObject(i))
                    {
                        if(t.fingerId == this.inputUI.camera)
                        {
                            camera.SetCameraRot(t);
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    if(t.fingerId == this.inputUI.camera)
                    {
                        camera.IsCameraRotating = false;
                        inputUI.camera = -1;
                    }
                    break;
            }
        }
    }
    
    public void setMove(float value)
    {
        inputUI.move = value;
    }
    public void setCamera(float value)
    {
        inputUI.camera = value;
    }
    public void setJump(float value)
    {
        inputUI.jump = value;
    }
    public void setAttack(float value)
    {
        inputUI.attack = value;
    }
    public void setSkill(float value)
    {
        inputUI.skill = value;
    }

}
