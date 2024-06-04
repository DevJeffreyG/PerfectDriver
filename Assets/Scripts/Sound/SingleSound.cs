using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSound : SoundManager
{

    public override void Play()
    {
        if (this.source.loop) throw new System.Exception("Source es un Loop");
    
        this.source.volume = this.maxVol;
        if (!this.source.isPlaying)
            this.source.Play();

    }

    public override void Stop()
    {
        if(this.source.isPlaying)
            this.source.Stop();
    }
}
