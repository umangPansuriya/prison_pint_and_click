using System;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] protected float _abilityTime;
    [SerializeField] protected float _abilityRegenrationTime;
    protected float _tempTime;

    protected event Action AbilityChange_Action;
}
