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
    ChainBehaviour cb;
    Chain chain;

    public GameObject node;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
        cb = GameObject.Find("GameManager").GetComponent<ChainBehaviour>();
        chain = GameObject.Find("GameManager").GetComponent<Chain>();
    }

    public void SetUpLine(Transform[] points)
    {
        Destroy(head);
        Destroy(tail);
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
        Transform[] chainPoints = new Transform[chain.chain.Count];
        for (int i = 0; i < chain.chain.Count; i++)
        {
            chainPoints[i] = chain.chain[i].transform;
        }
        cb.chainRenderer.SetUpLine(chainPoints);
    }

    public void DestroyNodes()
    {
        GameObject.Destroy(head);
        GameObject.Destroy(tail);
        lr.positionCount = 0;
    }

}
