using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class TargetScanner
{
    public float heightOffset = 0.0f;
    public float detectionRadius = 10;
    [Range(0.0f, 360.0f)]
    public float detectionAngle = 270;
    public float maxHeightDifference = 1.0f;
    [SerializeField] LayerMask detectLayerMask;
    public LayerMask viewBlockerLayerMask;
    [SerializeField] Collider[] buffer;

    /// <summary>
    /// Check if the player is visible according to that Scanner parameter.
    /// </summary>
    /// <param name="detector">The transform from which run the detection</param>
    /// /// <param name="useHeightDifference">If the computation should comapre the height difference to the maxHeightDifference value or ignore</param>
    /// <returns>The player PlayerController if visible, null otherwise</returns>
    public PlayerController DetectPlayer(Transform detector, bool useHeightDifference = true)
    { 
        var detected = Physics.OverlapSphereNonAlloc(detector.position, detectionRadius, buffer, detectLayerMask);
        if (detected == 0) return null;
        PlayerController closest = buffer[0].GetComponent<PlayerController>();
    
        Vector3 eyePos = detector.position + Vector3.up * heightOffset;
        Vector3 toPlayer = closest.foot.position - eyePos;
        Vector3 toPlayerTop = closest.transform.position + Vector3.up * 1.5f - eyePos;

        if (useHeightDifference && Mathf.Abs(toPlayer.y + heightOffset) > maxHeightDifference)
        { //if the target is too high or too low no need to try to reach it, just abandon pursuit
            return null;
        }

        Vector3 toPlayerFlat = toPlayer;
        toPlayerFlat.y = 0;

        if (toPlayerFlat.sqrMagnitude <= detectionRadius * detectionRadius)
        {
            if (Vector3.Dot(toPlayerFlat.normalized, detector.forward) >
                Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {

                bool canSee = false; 
           // Perform the first raycast
bool hitToPlayer = Physics.Raycast(eyePos, toPlayer.normalized, out RaycastHit hit1, detectionRadius, viewBlockerLayerMask, QueryTriggerInteraction.Ignore);
if (hitToPlayer)
{
    Debug.DrawRay(eyePos, toPlayer.normalized * hit1.distance, Color.red);
}
else
{
    Debug.DrawRay(eyePos, toPlayer, Color.blue);
    canSee = true;
}

// Perform the second raycast
bool hitToPlayerTop = Physics.Raycast(eyePos, toPlayerTop.normalized, out RaycastHit hit2, toPlayerTop.magnitude, viewBlockerLayerMask, QueryTriggerInteraction.Ignore);
if (hitToPlayerTop)
{
    Debug.DrawRay(eyePos, toPlayerTop.normalized * hit2.distance, Color.red);
}
else
{
    Debug.DrawRay(eyePos, toPlayerTop, Color.blue);
    canSee = true;
}
                if (canSee)
                    return closest;
            }
        }

        return null;
    }

    T GetClosestEnemy<T>(Transform tr, T[] enemies) where T : MonoBehaviour
    {
        T bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = tr.position;
        foreach (T potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }


#if UNITY_EDITOR
    public void EditorGizmo(Transform transform)
    {
        Color c = new Color(0, 0, 0.7f, 0.4f);

        UnityEditor.Handles.color = c;
        Vector3 rotatedForward = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, detectionAngle, detectionRadius);

        Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * heightOffset, 0.2f);
    }

#endif
}