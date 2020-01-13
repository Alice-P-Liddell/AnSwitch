using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnSwitch : MonoBehaviour
{
    //기능
    public bool isOn;                   //스위치 온오프

    //스위치 색상
    public Color handleColor = Color.white;
    public Color offBackgroundColor = new Color(213 / 255f, 213 / 255f, 213 / 255f, 1f);
    public Color onBackgroundColor = new Color(91 / 255f, 218 / 255f, 77 / 255f, 1f);

    //핸들 이동
    [Range(0, 3)]                       //moveDuration의 최소 최댓값을 개발자가 슬라이더로 설정 가능하게 만듦
    public float moveDuration = 0.2f;     //핸들 이동 애니메이션 속도
    const float totalHandleMoveLength = 76f;
    const float halfMoveLength = totalHandleMoveLength / 2;

    //참조
    Image handleImage;                  //핸들 이미지
    Image backgroundImage;              //배경 이미지
    RectTransform handleRectTransform;  //핸들 위치

    //코루틴
    Coroutine moveHandleCoroutine;              //핸들 이동 애니메이션 코루틴
    Coroutine changeBackgroundColorCoroutine;   //배경색 변경 코루틴

    void Start()
    {
        //핸들 위치 가져오기
        GameObject handleObject = transform.Find("Handle").gameObject;
        handleRectTransform = handleObject.GetComponent<RectTransform>();

        //핸들 이미지 가져오기
        handleImage = handleObject.GetComponent<Image>();
        handleImage.color = handleColor;

        //배경 이미지 가져오기
        backgroundImage = GetComponent<Image>();
        backgroundImage.color = offBackgroundColor;

        if (isOn)
        { handleRectTransform.anchoredPosition = new Vector2(halfMoveLength, 0); }
        else
        { handleRectTransform.anchoredPosition = new Vector2(-halfMoveLength, 0); }
    }

    public void OnClickSwitch()
    {
        isOn = !isOn;                   //IsOn의 값을 반대로 바꿈

        Vector2 fromPosition = handleRectTransform.anchoredPosition;
        Vector2 toPosition = (isOn) ? new Vector2(halfMoveLength, 0) : new Vector2(-halfMoveLength, 0);     //?
        Vector2 distance = toPosition - fromPosition;
        float ratio = Mathf.Abs(distance.x) / totalHandleMoveLength;        //Mathf.Abs() : 괄호 안의 값을 절댓값으로 바꿈
        float duration = moveDuration * ratio;


        //핸들 움직이는 코루틴
        if (moveHandleCoroutine != null)
        {
            StopCoroutine(moveHandleCoroutine);
            moveHandleCoroutine = null;
        }
        moveHandleCoroutine = StartCoroutine(moveHandle(fromPosition, toPosition, duration));

        Color fromColor = backgroundImage.color;
        Color toColor = (isOn) ? onBackgroundColor : offBackgroundColor;

        //배경 색 변경 코루틴
        if (changeBackgroundColorCoroutine != null)
        {
            StopCoroutine(changeBackgroundColorCoroutine);
            changeBackgroundColorCoroutine = null;
        }
        changeBackgroundColorCoroutine = StartCoroutine(changeBackgroundColor(fromColor, toColor, duration));
    }

    /// <summary>
    /// 클릭 시 핸들의 위치를 바꿔주는 함수
    /// </summary>
    /// <param name="fromPosition">핸들의 시작 위치</param>
    /// <param name="toPosition">핸들의 목적지 위치</param>
    /// <param name="duration">핸들이 이동하는 시간</param>
    /// <returns>없음</returns>
    //1. 클릭 시 핸들의 위치를 바꿔주는 함수, 프레임마다 업데이트 되어야하므로 코루틴함수로 만들자.
    IEnumerator moveHandle(Vector2 fromPosition, Vector2 toPosition, float duration)
    {
        //1-1. Duration 시간 동안 fromPosition에서 toPositiondmfh 핸들 이동시키기.
        float currentTime = 0f;         //얼마의 시간동안 이동했는가를 담을 변수
        while (currentTime < duration)
        {
            float t = currentTime / duration;
            Vector2 newPosition = Vector2.Lerp(fromPosition, toPosition, t);
            handleRectTransform.anchoredPosition = newPosition;

            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// 클릭 시 스위치 배경의 색상을 바꿔주는 함수
    /// </summary>
    /// <param name="fromColor">배경의 초기 색상</param>
    /// <param name="toColor">배경이 변경될 색상</param>
    /// <param name="duration">색상이 변경되는 시간</param>
    /// <returns>없음</returns>
    //2. 클릭 시 스위치 배경 색상 변경하는 함수
    IEnumerator changeBackgroundColor(Color fromColor, Color toColor, float duration)
    {
        //TODO: 1. 정해진 시간동안 색상 변경시키기
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float t = currentTime / duration;
            Color newColor = Color.Lerp(fromColor, toColor, t);
            backgroundImage.color = newColor;

            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
