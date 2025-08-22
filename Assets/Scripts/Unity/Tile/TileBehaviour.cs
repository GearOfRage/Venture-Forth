using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    Fader fader;
    ChainBehaviour cb;
    Chain chain;
    TurnLogic tl;
    StatsProjection statsProjection;

    void Start()
    {
        fader = GameObject.Find("GameManager").GetComponent<Fader>();
        cb = GameObject.Find("GameManager").GetComponent<ChainBehaviour>();
        chain = GameObject.Find("GameManager").GetComponent<Chain>();
        tl = GameObject.Find("GameManager").GetComponent<TurnLogic>();
        statsProjection = GameObject.Find("GameManager").GetComponent<StatsProjection>();

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
            PlaySound();
            Select(gameObject);
            if (statsProjection != null)
                statsProjection.ShowProjection();
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
        if (statsProjection != null)
            statsProjection.HideProjection();
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
            PlaySound();
            Select(gameObject);
            tl.CalculatePotentialDamageToEnemies();
            if (statsProjection != null)
                statsProjection.ShowProjection();
            return;
        }
        if (chain.chain.Count > 1)
        {
            if (cb.isDragStarted && gameObject == chain.chain[chain.chain.Count - 2])
            {
                chain.chain.RemoveAt(chain.chain.Count - 1);
                cb.chainRenderer.DrawChain();
                tl.CalculatePotentialDamageToEnemies();
                if (statsProjection != null)
                    statsProjection.ShowProjection();
            }
        }
    }

    void PlaySound()
    {
        AudioManager am = FindObjectOfType<AudioManager>();
        switch (gameObject.GetComponent<TileClass>().tileName)
        {
            case TileNameE.RegularEnemy:
                am.Play("SkullPick");
                break;
            case TileNameE.ExperiencePotion:
                break;
            case TileNameE.Poison:
                break;
            case TileNameE.HealthPotion:
                am.Play("PotionPick");
                break;
            case TileNameE.Shield:
                am.Play("ShieldPick");
                break;
            case TileNameE.Coin:
                am.Play("CoinPick");
                break;
            case TileNameE.Sword:
                am.Play("SwordPick");
                break;
            case TileNameE.Crown:
                break;
            case TileNameE.MagicSword:
                break;
            case TileNameE.ManaPotion:
                break;
            case TileNameE.BrokenSword:
                break;
            case TileNameE.BrokenShield:
                break;
            case TileNameE.EliteEnemy:
                break;
            default:
                break;
        }
    }

    void Select(GameObject selectedObjet)
    {
        chain.chain.Add(selectedObjet);
        cb.chainRenderer.DrawChain();
    }
}
