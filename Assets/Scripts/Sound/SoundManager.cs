using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoundManager : MonoBehaviour
{
    protected AudioSource source;
    protected bool startedfadeIn, startedFadeOut, fadedIn, fadedOut;
    protected float maxVol, minVol;

    private Coroutine C_fadeIn, C_fadeOut;

    public virtual void Start()
    {
        this.source = GetComponent<AudioSource>();
        this.maxVol = 1;
        this.minVol = 0;
    }

    public AudioSource getSource()
    {
        return source;
    }

    public void setMaxVol(float max)
    {
        this.maxVol = max;
    }

    public void setMinVol(float min)
    {
        this.minVol = min;
    }

    public abstract void Play();
    public abstract void Stop();

    public void ResetAudio()
    {
        this.source.time = 0;
    }

    public void Play(float fadeIn)
    {
        if (!this.startedfadeIn)
        {
            if (this.source.volume == maxVol) this.source.volume = minVol;
            this.Play();

            this.startedfadeIn = true;
            this.fadedIn = false;

            if(C_fadeOut != null) StopCoroutine(C_fadeOut);
            C_fadeIn = StartCoroutine(this.fadeIn(fadeIn));
        }
    }

    public void Play(float fadeIn, float fadeOut)
    {
        if (this.source.loop) throw new System.Exception("Source es un Loop, no debe usarse fadeOut en Play()");
        this.Play(fadeIn);

        if(this.fadedIn)
        {
            this.Stop(fadeOut, false);
        }
    }

    /*
     * instant 
     * - true: no espera a que se está acabando el sonido
     * - false: espera a que esté llegando al final para aplicar el efecto de fadeOut
     */
    public void Stop(float fadeOut, bool instant)
    {
        if (!startedFadeOut)
        {
            if(this.source.volume == minVol) this.source.volume = maxVol;

            this.startedFadeOut = true;
            this.fadedOut = false;
            
            if(C_fadeIn != null) StopCoroutine(C_fadeIn);
            StartCoroutine(this.fadeOut(fadeOut, instant));
        }

        if (this.fadedOut) this.Stop();
    }


    /**
     * Los IEnumerator permiten hacer acciones por más de 1 frame
     * En este caso, están siendo usados para incrementar o decrecer poco a poco el volumen de un audio (source)
     * yield en este caso hace que retorne (null) dentro de los whiles. Pero estos vuelvan a hacer otra iteración en el próximo frame.
     * Cuando la condición del while se cumple, se ejecuta el resto del código como cualquier función más, hasta que se vuelva a llamar.
     * Las Coroutines de Unity son las que permiten que estos IEnumerator se ejecuten 1 cada frame.
     */

    protected IEnumerator fadeIn(float delay)
    {
        float timeElapsed = 0f;
        float initalVol = this.source.volume;
        this.startedFadeOut = false;

        while (timeElapsed < delay)
        {
            this.source.volume = Mathf.Lerp(initalVol, maxVol, timeElapsed / delay);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        if(this.source.volume != maxVol) this.source.volume = maxVol;

        this.fadedIn = true;
    }

    protected IEnumerator fadeOut(float delay, bool instant)
    {
        float timeElapsed = 0f;
        float initalVol = this.source.volume;
        this.startedfadeIn = false;

        while (timeElapsed < delay)
        {
            // La diferencia entre la longitud del sonido, y lo que lleva reproducido
            float difference = this.source.clip.length - this.source.time;

            // Si la diferecia ya es menor al delay del fadeOut, empezar a hacerlo
            if (instant || difference < delay)
            {
                this.source.volume = Mathf.Lerp(initalVol, minVol, timeElapsed / delay);
                timeElapsed += Time.deltaTime;
            }

            yield return null;
        }

        if (this.source.volume != minVol) this.source.volume = minVol;

        this.fadedOut = true;
    }
}
