using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBlob : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    public int hp;

    public int level;
    private Rigidbody rb;

    private bool combined = true;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.SetDestination(new Vector3());
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateUpAxis = true;
        rb = GetComponent<Rigidbody>();
        var scale = (int) Math.Pow(2, level -1 );

        EnemySpawningSystem.Instance.blobsSpawned.Add(this);
        transform.localScale = Vector3.one * scale;
        hp *= scale;
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(navMeshAgent.nextPosition);
        if (hp < 0)
        {
            Debug.Log("ded");
            Destroy(gameObject);
            if (level > 1)
            {
                SpawnBlob(transform.position, level - 1).combined = false;
                SpawnBlob(transform.position, level - 1).combined = false;

            }
    
        }
    }

    private void OnDestroy()
    {
        if (EnemySpawningSystem.Instance)
        {
            EnemySpawningSystem.Instance.blobsSpawned.Remove(this);
        }
    }

    private static EnemyBlob SpawnBlob(Vector3 transformPosition, int level)
    {
        var blob = Instantiate(EnemySpawningSystem.Instance.enemyPrefab, transformPosition, Quaternion.identity, parent: EnemySpawningSystem.Instance.transform);
        var enemyBlob = blob.GetComponent<EnemyBlob>();
        enemyBlob.level = level;
        enemyBlob.combined = false;

        return enemyBlob;
    }


    public void OnTriggerEnter(Collider other)
    {
        var enemyBlob = other.gameObject.GetComponentInParent<EnemyBlob>();

        if (enemyBlob && !combined)
        {
            if (enemyBlob.level == level)
            {
                enemyBlob.combined = true;
                combined = true;
                Destroy(gameObject);
                Destroy(enemyBlob.gameObject);
                SpawnBlob((enemyBlob.transform.position + transform.position) / 2, level + 1);

            }
               
        }
    }

    public void OnMyTriggerEnter(Collider other)
    {
        
    }
}
