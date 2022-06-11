using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectParticle
{
    CoinParticles = 1,
    EquipParticles = 2,
    ExpParticles = 3
}

public class CollectVisuals : MonoBehaviour
{

    [SerializeField] GameObject CoinParticles;
    [SerializeField] GameObject EquipParticles;
    [SerializeField] GameObject ExpParticles;
    
    GameLogic gl;
    GameObject particlesPrefab;

    public float lifeTime = 1f;

    private void Start()
    {
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
    }
    public void RunParticles(CollectParticle particle)
    {
        Vector3 pos = new Vector3();
        switch (particle)
        {
            case CollectParticle.CoinParticles:
                particlesPrefab = CoinParticles;
                pos = gl.player.coinProgressBar.transform.position;
                break;

            case CollectParticle.EquipParticles:
                particlesPrefab = EquipParticles;
                pos = gl.player.equipmentProgressBar.transform.position;
                break;

            case CollectParticle.ExpParticles:
                particlesPrefab = ExpParticles;
                pos = gl.player.experienceProgressBar.transform.position;
                break;

            default:
                break;
        }
        Instantiate(particlesPrefab, pos, Quaternion.identity);
    }
}
