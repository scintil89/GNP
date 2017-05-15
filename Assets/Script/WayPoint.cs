using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WayPoint : MonoBehaviour
{
    public class Node
    {
        public int nodeID;
        public string nodeName;
        public Vector3 pos = new Vector3();
        public List<Node> adjnodeList = new List<Node>(); //인접노드를 저장하는 자료구조
    }

    public List<Node> nodeList = new List<Node>();

    // Use this for initialization
    void Start ()
    {
        InitNodeList();
        InitAdjNodeList();
    }

    void InitNodeList()
    {
        //Node 이름이 있는 GameObject를 검색하여 nodeList에 넣음
        int num = 0;

        while ( GameObject.Find("Node" + num) == true)
        {
            Node tempNode = new Node();
            tempNode.nodeID = num;
            tempNode.nodeName = "Node" + num;
            tempNode.pos = GameObject.Find("Node" + num).transform.position; //고칠수 있을까?
            nodeList.Add(tempNode);

            num++;
        }

        //Debug.Log("init nodeList.. NodeNum : " + nodeList.Count); // ok
    }

    void InitAdjNodeList()
    {
        foreach(var node in nodeList)
        {
            //Debug.Log("test" + node.nodeName);




            //RaycastHit[] hitObj = Physics.SphereCastAll(node.pos, 3.0f, transform.forward);

            //foreach (var i in hitObj)
            //{
            //    //if(i.transform.gameObject.layer == 12)
            //    //    Debug.Log("test- " + node.nodeName + " hit : " + i.transform.name); // 
            //}
                

            //if (Physics.SphereCast(node.pos, 30.0f, transform.forward, out hitObj) == true)
            //{
            //    Debug.LogError("test" + hitObj.transform.name); // 
            //
            //    var tempNode = nodeList.Find(x => x.nodeName == hitObj.transform.gameObject.name);
            //
            //    if(tempNode)
            //        node.adjnodeList.Add(tempNode); //인접 노드로 추가

                //Debug.LogError("init adjnodeList.. NodeName : " + node.nodeID + ", adj " + tempNode.nodeName); // 
            //}
        }

        
    }

    void Release()
    {
        nodeList.Clear();
    }


}
