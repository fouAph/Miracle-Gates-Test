using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class AiController : MonoBehaviour
{
    NavMeshAgent agent;
    private Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
            StartCoroutine(GenerateAiPath());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 1)
        {
            StartCoroutine(GenerateAiPath());
        }
    }

    private IEnumerator GenerateAiPath()
    {
        yield return new WaitForSeconds(2);
        print("walk");
        targetPosition = transform.position + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
        agent.SetDestination(targetPosition);
    }
}
