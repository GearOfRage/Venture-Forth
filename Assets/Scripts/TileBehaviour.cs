using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    GameLogic gl;

    public int indexX;
    public int indexY;

    bool mouseOver = false;

    public Color defaultColor;

    void Awake()
    {
        defaultColor = gameObject.GetComponent<SpriteRenderer>().material.color;
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
    }

    void OnMouseDown()
    {
        //Debug.Log("MouseDown " + gameObject.name);
        if (!gl.chain.Contains(gameObject))
        {
            Select(gameObject);
        }
        gl.isDragStarted = true;
    }

    void OnMouseUp()
    {
        //Debug.Log("MouseUp");
        gl.isDragStarted = false;
        gl.ChainDone();
    }

    void OnMouseEnter()
    {
        //Debug.Log("MouseEnter");
        mouseOver = true;
    }

    void OnMouseExit()
    {
        //Debug.Log("MouseExit");
        mouseOver = false;
    }

    void OnMouseOver()
    {
        //Пофіксить лінію
        if (gl.isDragStarted && !gl.chain.Contains(gameObject) && Vector2.Distance(gl.chain[gl.chain.Count - 1].transform.position, gameObject.transform.position) < 1.5f)
        {
            Select(gameObject);
        }
        if (gl.chain.Count > 1)
        {
            if (gl.isDragStarted && gameObject == gl.chain[gl.chain.Count - 2])
            {
                gl.chain.RemoveAt(gl.chain.Count - 1);
            }
        }
    }

    void Select(GameObject selectedObjet)
    {

        gl.chain.Add(selectedObjet);
        selectedObjet.GetComponent<SpriteRenderer>().material.color = Color.blue;

        Transform[] chainPoints = new Transform[gl.chain.Count];
        for (int i = 0; i < gl.chain.Count; i++)
        {
            chainPoints[i] = gl.chain[i].transform;
            //Debug.Log(chainPoints[i].name);
        }
        gl.chainRenderer.SetUpLine(chainPoints);
    }
}
