using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealthBar : MonoBehaviour
{
    public virtual void OnEntityHealthChange() {}
    public virtual void OnAttachedEntitySwitch(EntityStats entity) {}
}
