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

    [SerializeField] PlayerClass player;

    GameObject particlesPrefab;
    GameObject particles;

    public float lifeTime = 1f;

    public void RunParticles(CollectParticle particle)
    {
        Vector3 pos = new Vector3();
        switch (particle)
        {
            case CollectParticle.CoinParticles:
                particlesPrefab = CoinParticles;
                pos = player.coinProgressBar.transform.position;
                break;

            case CollectParticle.EquipParticles:
                particlesPrefab = EquipParticles;
                pos = player.equipmentProgressBar.transform.position;
                break;

            case CollectParticle.ExpParticles:
                particlesPrefab = ExpParticles;
                pos = player.experienceProgressBar.transform.position;
                break;

            default:
                break;
        }
        particles = Instantiate(particlesPrefab, pos, Quaternion.identity);
    }
}
