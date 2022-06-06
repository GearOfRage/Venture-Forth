using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    GameLogic gl;

    void Start()
    {
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
        gameObject.GetComponentInChildren<SpriteRenderer>().transform.Rotate(0, 0, Random.Range(-10f, 10f), Space.Self);
    }

    void OnMouseDown()
    {
        if (!gl.chain.Contains(gameObject))
        {
            Select(gameObject);
        }
        gl.isDragStarted = true;
    }

    void OnMouseUp()
    {
        gl.isDragStarted = false;
        gl.ChainDone();
    }

    void OnMouseOver()
    {
        if (gl.isDragStarted 
            && !gl.chain.Contains(gameObject) 
            && gl.chain[0].GetComponent<TileClass>().tileType == gameObject.GetComponent<TileClass>().tileType
            && Vector2.Distance(gl.chain[gl.chain.Count - 1].transform.position, gameObject.transform.position) < 1.5f)
        {
            Select(gameObject);
            return;
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
        gl.chainRenderer.DrawChain();
    }
}
