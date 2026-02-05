using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    
    void Start()
    {
        Cursor.visible = false;
    }
    /*
    public void TestTK()// 조준선 느낌으로 사용이 가능할듯
    {
        Vector3 mousePos = Input.mousePosition; // 마우스 위치를 가져옴
        mousePos.z = 10f; // 카메라 거리 조절용

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos); // 화면좌표를 월드좌표로 변환
        transform.position = worldPos; //오브젝트위치를 마우스위치로
    }
    */
    public void TestTK()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;

        Vector3 viewPos = Camera.main.ScreenToViewportPoint(mousePos);

        viewPos.x = Mathf.Clamp(viewPos.x, 0f, 1f); // 나중에 조준선이랑 맞추면 될듯
        viewPos.y = Mathf.Clamp(viewPos.y, 0.02f, 0.98f);

        Vector3 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(viewPos.x, viewPos.y, 10f)); ;
        worldPos.z = 0f;
    
        transform.position = worldPos;
    }
    
    void LateUpdate()
    {
        TestTK();
    }
}
