using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public virtual string GetName()
    {
        return this.ToString();
    }

    public virtual void OnHighlighted() { }
    public virtual void OnUnHighlighted() { }
    public abstract void OnInteracted();
}
