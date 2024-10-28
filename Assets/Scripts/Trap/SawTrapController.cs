using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrapController : MonoBehaviour
{
    private Animator sawTrapAnimator;
    private SpriteRenderer sawTrapSR;

    [Header("Saw Trap")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float coolDown = 3;
    [SerializeField] private Transform[] wayPoint;

    //tạo ra mảng mới lưu trữ vị trí ban đầu của các waypoint để sửa lỗi khi object cha là trap di chuyển thì các waypoint là object con sẽ theo cha
    //vì thể chúng ta sẽ tạo mảng lưu vị trí ban đầu để trap đi theo các vị trí ban đầu đó
    [SerializeField] private Vector3[] wayPointPosition;
    private bool isActive = true;
    public int wayPointIndex = 1;
    private int moveDirection = 1;
    private void Awake()
    {
        sawTrapAnimator = GetComponent<Animator>();
        sawTrapSR = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        UpdateWayPointsPosition();
        //trap sẽ bắt đầu ở điểm đầu tiên tương ứng với phần tử đầu tiên trong mảng wayPointPosition
        transform.position = wayPointPosition[0];
    }

    private void Update()
    {

        //nếu như trap chưa kích hoạt thì trả về không làm gì cả
        if (!isActive) return;

        sawTrapAnimator.SetBool("isActive", isActive);

        //di chuyển trap đến vị trí trong mảng với tham số là điểm bắt đầu, đích đến,Khoảng cách di chuyển tối đa trong 1 lần gọi
        transform.position = Vector2.MoveTowards(transform.position, wayPointPosition[wayPointIndex], moveSpeed * Time.deltaTime);

        //khoảng cách từ trap cho đến vị trí tiếp theo
        float wayPointsDistance = Vector2.Distance(transform.position, wayPointPosition[wayPointIndex]);

        //nếu khoảng cách là 0 và trap đang ở vị trí cuối cùng hoặc vị trí đầu trong mảng các vị trí
        if (wayPointsDistance == 0)
        {
            if (wayPointIndex == wayPoint.Length - 1 || wayPointIndex == 0)
            {
                //chuyển moveDirection sang -1 sau sẽ giảm dần index trong mảng giúp đổi hướng di chuyển của trap
                moveDirection = moveDirection * -1;

                //bắt đầu đếm ngược thời gian dừng của trap cho đến khi bắt đầu lại
                StartCoroutine(StopMovement(coolDown));
            }
            //tăng giảm vị trí trong mảng các vị trí sẽ di chuyển của trap
            wayPointIndex += moveDirection;
        }
    }

    private void UpdateWayPointsPosition()
    {
        //SawTrapWayPoint là một class mà mỗi instance của nó đại diện cho một điểm waypoint mà trap sẽ di chuyển đến
        //dùng GetComponentsInChildren để bắt đầu tìm component kiểu SawTrapWayPoint bắt đầu từ object cha(nơi nó được gọi) rồi xuống object con
        //GetComponentsInChildren<SawTrapWayPoint>() sẽ trả về mảng các waypoint và chuyển nó sang list
        List<SawTrapWayPoint> wayPointList = new List<SawTrapWayPoint>(GetComponentsInChildren<SawTrapWayPoint>());

        //nếu số phần tử danh sách và mảng khác nhau
        if(wayPointList.Count != wayPoint.Length )
        {
            //mảng waypoint sẽ có số phần tử tương đương với số phần tử trong list 
            wayPoint = new Transform[wayPointList.Count];

            //gán tất cả phần tử trong list vào mảng wayPoint
            for(int i = 0; i < wayPointList.Count; i++)
            {
                wayPoint[i] = wayPointList[i].transform;
            }
        }

        //mảng wayPointPosition sẽ có số phần tử tương đương với số phần tử trong mảng wayPoint
        wayPointPosition = new Vector3[wayPoint.Length];
        //gán tất cả phần tử trong list vào mảng wayPointPosition
        for (int i = 0; i < wayPoint.Length; i++)
        {
            wayPointPosition[i] = wayPoint[i].position;
        }
    }

    private IEnumerator StopMovement(float delay)
    {

        isActive = false;

        yield return new WaitForSeconds(delay);

        isActive = true;

        sawTrapSR.flipX = !sawTrapSR.flipX;
    }
}
