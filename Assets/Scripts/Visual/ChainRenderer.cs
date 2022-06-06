using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainRenderer : MonoBehaviour
{
    [HideInInspector]
    public LineRenderer lr;
    public GameLogic gl;
    GameObject head;
    GameObject tail;

    public GameObject node;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
    }

    public void SetUpLine(Transform[] points)
    {
        GameObject.Destroy(head);
        GameObject.Destroy(tail);
        head = Instantiate(node, new Vector3(points[0].position.x, points[0].position.y, -1), Quaternion.identity, lr.transform);
        tail = Instantiate(node, new Vector3(points[points.Length - 1].position.x, points[points.Length - 1].position.y, -1), Quaternion.identity, lr.transform);
        head.name = "ChainHead";
        tail.name = "ChainTail";

        lr.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].position);
        }
    }

    public void DrawChain()
    {
        Transform[] chainPoints = new Transform[gl.chain.Count];
        for (int i = 0; i < gl.chain.Count; i++)
        {
            chainPoints[i] = gl.chain[i].transform;
        }
        gl.chainRenderer.SetUpLine(chainPoints);
    }

    public void DestroyNodes()
    {
        GameObject.Destroy(head);
        GameObject.Destroy(tail);
        lr.positionCount = 0;
    }

}
