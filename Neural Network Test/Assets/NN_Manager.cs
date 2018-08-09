using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NN_Manager : MonoBehaviour {

    public Transform spawnpoint;
    public int populationCount;
    public GameObject runnerPrefab;

    private void Start()
    {
        populate();
    }
    private void populate()
    {
        for (int p = 0; p < populationCount; p++)
        {
            Instantiate(runnerPrefab, spawnpoint.position, spawnpoint.rotation);
        }
    }
}
