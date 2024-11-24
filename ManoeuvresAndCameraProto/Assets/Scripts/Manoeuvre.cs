using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manoeuvre
{
    public abstract void BeginManoeuvre();
    public abstract void UpdateManoeuvre();
    public abstract void EndManoeuvre();
}
