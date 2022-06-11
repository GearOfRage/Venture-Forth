using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    Fader fader;
    ChainBehaviour cb;
    Chain chain;

    void Start()
    {
        fader = GameObject.Find("GameManager").GetComponent<Fader>();
        cb = GameObject.Find("GameManager").GetComponent<ChainBehaviour>();
        chain = GameObject.Find("GameManager").GetComponent<Chain>();

        gameObject.GetComponentInChildren<SpriteRenderer>().transform.Rotate(0, 0, Random.Range(-10f, 10f), Space.Self);
    }

    void OnMouseDown()
    {
        if (fader.isFaderOn)
        {
            return;
        }
        if (!chain.chain.Contains(gameObject))
        {
            Select(gameObject);
        }
        cb.isDragStarted = true;
    }

    void OnMouseUp()
    {
        if (fader.isFaderOn)
        {
            return;
        }
        cb.isDragStarted = false;
        cb.ChainDone();
    }

    void OnMouseOver()
    {
        if (fader.isFaderOn)
        {
            return;
        }
        if (cb.isDragStarted 
            && !chain.chain.Contains(gameObject) 
            && chain.chain[0].GetComponent<TileClass>().tileType == gameObject.GetComponent<TileClass>().tileType
            && Vector2.Distance(chain.chain[chain.chain.Count - 1].transform.position, gameObject.transform.position) < 1.5f)
        {
            Select(gameObject);
            return;
        }
        if (chain.chain.Count > 1)
        {
            if (cb.isDragStarted && gameObject == chain.chain[chain.chain.Count - 2])
            {
                chain.chain.RemoveAt(chain.chain.Count - 1);
                cb.chainRenderer.DrawChain();
            }
        }
    }

    void Select(GameObject selectedObjet)
    {
        chain.chain.Add(selectedObjet);
        cb.chainRenderer.DrawChain();
    }
}
