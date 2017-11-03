using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public List<Node> _AdjNodes; //인접 노드

    public Node prevNode { get; set; }

    public float distance = float.MaxValue;

    //private void Start()
    //{
    //    gameObject.tag = "Node";
    //}

    void OnDrawGizmos()
    {
        if (_AdjNodes == null)
            return;

        Gizmos.color = Color.red;

        foreach (var node in _AdjNodes)
        {
            if (node != null)
                Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
