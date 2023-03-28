using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public abstract class ProjectileModifier
{
    protected Rigidbody projectileRigidbody;
    protected Transform projectileTransform;
    protected ModularProjectile projectileComponent;

    public void SetupModifier(Rigidbody projectileRigidbody, Transform projectileTransform, ModularProjectile projectileComponent)
    {
        this.projectileRigidbody = projectileRigidbody;
        this.projectileTransform = projectileTransform;
        this.projectileComponent = projectileComponent;

        OnModifierApplied();
    }

    public bool IsValid()
    {
        return projectileRigidbody != null && projectileTransform != null && projectileComponent != null;
    }

    public override string ToString()
    {
        string name = this.GetType().Name;

        name = name.Split('_')[1];

        name = Regex.Replace(name, "([a-z])([A-Z])", "$1 $2");

        return name;
    }

    public abstract void OnModifierApplied();

    public abstract void TickModifier(float deltaTime);
}