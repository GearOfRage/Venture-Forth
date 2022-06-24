using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectVisuals : MonoBehaviour
{
    [SerializeField] ProgressTypeE myName;
    ParticleSystem[] particleSystems;

    private void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem item in particleSystems)
        {
            item.Stop();
        }

        //Add subscription OnCollect
        TurnLogic.OnCollect += CollectGoldVisual;
        TurnLogic.OnCollect += CollectEquipmentVisual;
        TurnLogic.OnCollect += CollectExperienceVisual;
    }

    void CollectExperienceVisual(ProgressTypeE progressType)
    {
        if (myName != progressType)
            return;

        foreach (ParticleSystem item in particleSystems)
        {
            item.Play();
        }
        //StartCoroutine(Follow(gameObject,start,end));
    }

    void CollectEquipmentVisual(ProgressTypeE progressType)
    {
        if (myName != progressType)
            return;

        foreach (ParticleSystem item in particleSystems)
        {
            item.Play();
        }
    }

    void CollectGoldVisual(ProgressTypeE progressType)
    {
        if (myName != progressType)
            return;

        foreach (ParticleSystem item in particleSystems)
        {
            item.Play();
        }
    }

    IEnumerator Follow(GameObject follower, Vector3 start, Vector3 target)
    {
        float normalizedTime = 0f;
        float duration = 1f;

        //follower.transform.position.Set(start.x,start.y,start.z);
        follower.transform.position = start;

        while (normalizedTime <= duration)
        {
            normalizedTime += Time.deltaTime / duration;

            follower.transform.position = Vector3.Lerp(follower.transform.position, target, normalizedTime);
            yield return null;
        }
    }
}
