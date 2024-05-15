using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPuller : MonoBehaviour
{
    public float gravityStrength = 80f;
    public float pullRadius = 10f;
    public float maxForcemagnitude = 100f;
    public LayerMask pullableMask;

    private void FixedUpdate()
    {
        var colls = Physics.OverlapSphere(transform.position, pullRadius, pullableMask);

        foreach (var item in colls)
        {
            var rb = item.GetComponent<Rigidbody>();
            if (rb != null && rb != GetComponent<Rigidbody>())
            {
                Vector3 forceDirection = transform.position - rb.position;
                float distance = forceDirection.magnitude;

                if (distance > 0)
                {
                    float gravityStrengthAtDistance = gravityStrength / distance;
                    Vector3 force = forceDirection.normalized * gravityStrengthAtDistance;

                    force = Vector3.ClampMagnitude(force, maxForcemagnitude);

                    rb.AddForce(force, ForceMode.Force);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.enabled = false;
        }
    }
}
