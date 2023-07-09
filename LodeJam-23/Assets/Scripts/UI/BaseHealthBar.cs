using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealthBar : MonoBehaviour
{
    public virtual void OnPlayerHealthChange() {}
    public virtual void OnPlayerCharacterSwitch() {}
}
