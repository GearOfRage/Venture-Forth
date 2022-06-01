using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainRenderer : MonoBehaviour
{
    [HideInInspector]
    public LineRenderer lr;
    GameObject head;
    GameObject tail;

    public GameObject node;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        head = new GameObject();
        tail = new GameObject();
    }

    public void SetUpLine(Transform[] points)
    {
        GameObject.Destroy(head);
        GameObject.Destroy(tail);
        head = Instantiate(node, new Vector3(points[0].position.x, points[0].position.y, -1), Quaternion.identity);
        tail = Instantiate(node, new Vector3(points[points.Length - 1].position.x, points[points.Length - 1].position.y, -1), Quaternion.identity);

        lr.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].position);
        }
    }

    public void DestroyNodes()
    {
        GameObject.Destroy(head);
        GameObject.Destroy(tail);
    }
}
