using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WayPoint : MonoBehaviour
{
    public Stack<Vector3> way;

    public GameObject target;

    //public GameObject tempStart;
    //public GameObject tempEnd;

    Node CalcNearNode(Vector3 pos)
    {
        //GameObject 전체 탐색?
        //Ray 여러개?
        //Sphere 충돌 판정?

        GameObject near = null;

        float closestDist = Mathf.Infinity;

        foreach (var node in GameObject.FindGameObjectsWithTag("Node"))
        {
            var dist = (node.transform.position - pos).magnitude;

            if (dist < closestDist)
            {
                near = node;
                closestDist = dist;
            }
        }

        if (near != null)
        {
            return near.GetComponent<Node>();
        }

        return null;
    }

    public void CalcWay()
    {
        //var starttime = Time.realtimeSinceStartup;
        //Debug.Log("start time " + starttime);
        target = gameObject.GetComponent<UnitScript>().target;

        //way
        way = new Stack<Vector3>();

        //방문하기 전 노드
        SortedList<float, Node> opened = new SortedList<float, Node>();
        //방문한 Node
        List<Node> closed = new List<Node>();

        //시작점 근처의 노드 탐색
        Node nowNode = CalcNearNode(transform.position);
        //Node nowNode = tempStart.GetComponent<Node>();

        //도착점 근처의 노드 탐색
        Node endNode = CalcNearNode(target.transform.position);
        //Node endNode = tempEnd.GetComponent<Node>();

        //예외처리
        if (nowNode == null || endNode == null || nowNode == endNode)
        {
            Debug.Log("CalcWay : Init Node Err");
            return;
        }

        //루프 돌기 위해 초기화
        opened.Add(0, nowNode);

        nowNode.prevNode = null;
        nowNode.distance = 0.0f;

        //way를 계산하기 위한 루프
        while (opened.Count > 0)
        {
            nowNode = opened.Values[0];
            opened.RemoveAt(0);

            float dist = nowNode.distance;
            //Debug.Log(dist);

            //
            closed.Add(nowNode);

            //종료조건
            if(nowNode == endNode)
            {
                break;
            }

            //
            foreach(var adjnode in nowNode._AdjNodes)
            {
                var newdist = dist + (adjnode.transform.position - nowNode.transform.position).magnitude;

                //방문한 노드이거나, 중복이 있을 경우
                if (closed.Contains(adjnode) || opened.ContainsValue(adjnode))
                {
                    //기존 거리가 새 거리보다 짧으면 패스
                    if(adjnode.distance < newdist)
                    {
                        continue;
                    }
                }

                //다음 노드의 이전 노드를 지금 노드로
                adjnode.prevNode = nowNode;

                //현재 거리 + 다음 노드 거리
                adjnode.distance = newdist;
                 
                var distanceToTarget = (adjnode.transform.position - endNode.transform.position).magnitude;

                //opened list에 인접노드를 추가.
                opened.Add(adjnode.distance + distanceToTarget, adjnode);
            }//end foreach
        }//end While

        //끝에서부터 시작점까지 스택에 넣는다.
        if (nowNode == endNode)
        {
            way.Push(target.transform.position);

            while (nowNode.prevNode != null)
            {
                way.Push(nowNode.transform.position);
                nowNode = nowNode.prevNode;
            }
        }//end if


        //var endtime = Time.realtimeSinceStartup;
        //Debug.Log("end time " + endtime);
        //
        //Debug.Log(endtime - starttime); 
    }

    public void StopMove()
    {
        way = null;
    }

    //void Awake()
    //{
    //    //for test
    //    CalcWay();
    //
    //    while(way.Count > 0)
    //    {
    //        Debug.Log("Way : " + way.Pop().name);
    //    }
    //}
}
