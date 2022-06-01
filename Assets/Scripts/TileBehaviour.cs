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

    void Start()
    {
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();

        gameObject.GetComponent<SpriteRenderer>().transform.Rotate(0, 0, Random.Range(-10f, 10f), Space.Self);
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
        if (gl.isDragStarted && !gl.chain.Contains(gameObject) && Vector2.Distance(gl.chain[gl.chain.Count - 1].transform.position, gameObject.transform.position) < 1.5f)
        {
            Select(gameObject);
        }
        if (gl.chain.Count > 1)
        {
            if (gl.isDragStarted && gameObject == gl.chain[gl.chain.Count - 2])
            {
                gl.chain.RemoveAt(gl.chain.Count - 1);
                gl.chainRenderer.DrawChain();
            }
        }
    }

    void Select(GameObject selectedObjet)
    {

        gl.chain.Add(selectedObjet);
        //selectedObjet.GetComponent<SpriteRenderer>().material.color = Color.blue;
        gl.chainRenderer.DrawChain();
    }

    
}
