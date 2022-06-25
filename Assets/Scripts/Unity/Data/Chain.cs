using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public List<GameObject> chain; //List for chain elements
    public int minChainSize = 3; // Minimal chain size

    void Start()
    {
        chain = new List<GameObject>();
    }
}
