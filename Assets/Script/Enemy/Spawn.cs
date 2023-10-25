using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public float spwanTime =3f;
    public float curTime;
    public GameObject Enemy;
    public Transform[] spawnPoints;
    public bool[] isSpawn;

    // Start is called before the first frame update
    void Start()
    {
        isSpawn = new bool[spawnPoints.Length];
        for (int i = 0; i < isSpawn.Length; i++)
        {
            isSpawn[i] = false;
        }

    }

    // Update is called once per frame
    private void Update()
    {
        if(curTime >= spwanTime)
        {
            int x = Random.Range(0, spawnPoints.Length);
            SpwanEnemy(x);
        }
        curTime += Time.deltaTime;
    }
    public void SpwanEnemy(int ranNum)
    {
        curTime = 0;
        Instantiate(Enemy, spawnPoints[ranNum]);
        isSpawn[ranNum] = true;
    }
}
