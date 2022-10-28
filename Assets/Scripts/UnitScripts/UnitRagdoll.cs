using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    // refer to the ragdoll root bone
    [SerializeField] Transform ragDollRootBone;


    // this script transfer the location of the original root bone transform into the ragdoll
    public void RagDollSetUp(Transform originalRoot_)
    {
        // match all the bones in ragdoll with the original unit model
        MatchAllBones(originalRoot_, ragDollRootBone);

        // ragdoll explosion
        ApplyExplosionToRagdoll(ragDollRootBone, transform.position, 300.0f, 10.0f);
    }


    // transfrom used to match the old and new bones
    private void MatchAllBones(Transform root_, Transform clone_)
    {
        foreach (Transform child in root_)
        {
            // find the child with same name inside the clone
            Transform cloneChild = clone_.Find(child.name);
            // if find one, copy the transform
            if (cloneChild != null)
            {
                cloneChild.transform.position = child.transform.position;
                cloneChild.transform.rotation = child.transform.rotation;

                // copy the next level
                MatchAllBones(child, cloneChild);
            }
        }

    }

    // make the bodypart explode upon death
    private void ApplyExplosionToRagdoll(Transform root_, Vector3 explosionPos_, float explosionForce, float explosionRange)
    {
        foreach(Transform child in root_)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPos_, explosionRange);
            }

            // apply to the next level
            ApplyExplosionToRagdoll(child, explosionPos_, explosionForce, explosionRange);
        }
    }
    
}
