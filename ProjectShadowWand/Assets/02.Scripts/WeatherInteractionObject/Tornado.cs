using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Tornado : WeatherInteractionObject
{

    private void Update()
    {
        Exectue();   
    }
    public override void Exectue()
    {
        base.Exectue();
    }

    public override void ChangeState()
    {
        base.ChangeState();
    }

    public override void ProcessRainy()
    {

    }
    public override void ProcessSunny()
    {

    }
}
