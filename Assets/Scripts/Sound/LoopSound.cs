using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopSound : SoundManager
{
    private bool firstInit;

    public override void Start()
    {
        base.Start();
        this.firstInit = false;
    }

    public override void Play()
    {
        if (!this.source.loop) throw new System.Exception("Source no es un Loop");
        if(!firstInit)
        {
            this.firstInit = true;
            this.source.Play();
        } else if (!this.source.isPlaying)
        {
            if (this.fadedOut) this.source.volume = this.maxVol;
            this.source.UnPause();
        }
    }

    public override void Stop()
    {
        if(firstInit && this.source.isPlaying)
        {
            this.source.Pause();
        }
    }
}
