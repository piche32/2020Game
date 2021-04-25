using UnityEngine;
using UnityEngine.EventSystems; //키보드, 마우스, 터치를 이벤트로 오브젝트에 보낼 수 있는 기능 지원


public class JoyStickScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    PlayerScript playerSc;

    [SerializeField]
    private RectTransform mLever;
    private RectTransform mRectTransform;

    [SerializeField, Range(10f, 100f)]
    private float mLeverRange;

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private RectTransform target;


    private Vector2 mInputDir;
    public Vector2 MInputDir { get { return mInputDir; } set { mInputDir = value; } }

    private void Awake()
    {
        playerSc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        mRectTransform = GetComponent<RectTransform>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        playerSc.IsMoving = true;
        ControlJoystickLever(eventData);
    }

    //오브젝트를 클릭해서 드래그 하는 도중에 들어오는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        playerSc.IsMoving = false;
        mLever.anchoredPosition = Vector2.zero;
    }

    private void ControlJoystickLever(PointerEventData eventData)
    {
        TouchManager.Instance.setMove(true);
        Vector3 leverPos = new Vector3(eventData.position.x, eventData.position.y, mLever.position.z);
        mLever.position = leverPos;
        mLever.anchoredPosition = mLever.anchoredPosition.magnitude < mLeverRange ?
            mLever.anchoredPosition : mLever.anchoredPosition.normalized * mLeverRange;
        mInputDir = mLever.anchoredPosition.normalized;
    }
}