using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using scene;

public class PuzzleDoor : Door
{
    protected override bool TryUnlock()
    {
/*        if (!PuzzleSolved)
        {
            return false;
        }*/
        return base.TryUnlock();
    }
}
