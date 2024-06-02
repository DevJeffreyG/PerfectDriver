using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Focusable
{
    abstract void onFocus();
    abstract void onLostFocus();
}
