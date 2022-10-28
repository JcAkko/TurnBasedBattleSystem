using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    // ragdoll prefab
    [SerializeField] Transform unitRagdollPrefab;

    // the root bone for the original unit
    [SerializeField] Transform originalRootBone;


    // health system
    private HealthSystem healthSystem;


    private void Awake()
    {
        // sign the health system
        healthSystem = this.GetComponent<HealthSystem>();

        // subscribe to the unit death event
        healthSystem.onUnitDeath += healthSystem_OnUnitDeath;
    }


    // function called upon unit death
    private void healthSystem_OnUnitDeath(object sender, EventArgs empty)
    {
        // summon the ragdoll
        Transform ragdollTransform = Instantiate(unitRagdollPrefab, this.transform.position, this.transform.rotation);
        // transform the root location to the ragdoll
        ragdollTransform.GetComponent<UnitRagdoll>().RagDollSetUp(originalRootBone);
    }

    



}
