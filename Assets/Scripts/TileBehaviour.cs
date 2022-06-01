using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    GameLogic gl;

    public int indexX;
    public int indexY;


    [SerializeField]
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
            //gl.head = Instantiate(gl.node, gl.chain.Last().transform.localPosition, Quaternion.identity, gl.chainRenderer.transform);
            //gl.tail = Instantiate(gl.node, gl.chain.First().transform.localPosition, Quaternion.identity, gl.chainRenderer.transform);
            
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
        if (gl.isDragStarted && !gl.chain.Contains(gameObject) && Vector2.Distance(gl.chain.Last().transform.position, gameObject.transform.position) < 1.5f)
        {
            Select(gameObject);
            //Debug.Log("Added: " + gameObject.name);
        }

        //Debug.Log("Over: " + gameObject.name);
        //Debug.Log("MouseOver");
        //StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        float duration = 2f;

        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;

        }
        if (mouseOver)
        {
            ShowToolTip();
        }
        yield return null;
    }

    private void ShowToolTip()
    {
        Debug.Log("Tool tip is showed");
    }

    void Select(GameObject selectedObjet)
    {

        gl.chain.Add(selectedObjet);
        selectedObjet.GetComponent<SpriteRenderer>().material.color = Color.blue;

        gl.head.transform.Translate(gl.chain.Last().transform.localPosition);

        Transform[] chainPoints = new Transform[gl.chain.Count];
        for (int i = 0; i < gl.chain.Count; i++)
        {
            chainPoints[i] = gl.chain[i].transform;
            //Debug.Log(chainPoints[i].name);
        }
        gl.chainRenderer.SetUpLine(chainPoints);
    }
}
