using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour // Observer pattern based on : https://unity.com/how-to/create-modular-and-maintainable-code-observer-pattern
{
    public ObjectNote subjectToObserve;
    public Transform prefabEffect;

    private void OnNoteMissed()
    {
        Transform effectInstance = prefabEffect;
        ParticleSystem particleSystem = effectInstance.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;

            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.red);
        }
        Instantiate(effectInstance, subjectToObserve.transform.position, Quaternion.identity);

    }

    private void OnNoteHit()
    {
        Transform effectInstance = prefabEffect;
        ParticleSystem particleSystem = effectInstance.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;

            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.green);
        }
        Instantiate(effectInstance, subjectToObserve.transform.position, Quaternion.identity);
        
    }

    private void Start()
    {
        if (subjectToObserve != null)
        {
            subjectToObserve.NoteMissed += OnNoteMissed;

            subjectToObserve.NoteHit += OnNoteHit;
        }
    }

    private void OnDestroy()
    {
        if (subjectToObserve != null)
        {
            subjectToObserve.NoteMissed -= OnNoteMissed;

            subjectToObserve.NoteHit -= OnNoteHit;
        }
    }
}
