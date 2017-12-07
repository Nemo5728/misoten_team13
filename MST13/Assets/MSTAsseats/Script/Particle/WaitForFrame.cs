using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaitForFrame : CustomYieldInstruction
{
    int endFrame = 0;

    public WaitForFrame(int waitFrame)
    {
        endFrame = waitFrame + Time.frameCount;
    }

    public override bool keepWaiting
    {
        get
        {
            if (endFrame <= Time.frameCount)
                return false;
            else
                return true;
        }
    }
}