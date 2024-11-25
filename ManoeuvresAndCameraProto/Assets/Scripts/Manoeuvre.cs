using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manoeuvre
{
    public enum ManoeuvreID
    {
        catchClimb,
        crawl
    }

    public ManoeuvreID manoeuvreID;
    public abstract void BeginManoeuvre(PlayerController playerController);
    public abstract void UpdateManoeuvre();
    public abstract void EndManoeuvre();
}
