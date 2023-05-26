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

    private void OnNotePerfect()
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
    private void OnNoteGood()
    {
        Transform effectInstance = prefabEffect;
        ParticleSystem particleSystem = effectInstance.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;

            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.cyan);
        }
        Instantiate(effectInstance, subjectToObserve.transform.position, Quaternion.identity);

    }
    private void OnNoteBad()
    {
        Transform effectInstance = prefabEffect;
        ParticleSystem particleSystem = effectInstance.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;

            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.yellow);
        }
        Instantiate(effectInstance, subjectToObserve.transform.position, Quaternion.identity);

    }

    private void Start()
    {
        if (subjectToObserve != null)
        {
            subjectToObserve.NoteMissed += OnNoteMissed;
            subjectToObserve.NotePerfect += OnNotePerfect;
            subjectToObserve.NoteGood += OnNoteGood;
            subjectToObserve.NoteBad += OnNoteBad;
        }
    }

    private void OnDestroy()
    {
        if (subjectToObserve != null)
        {
            subjectToObserve.NoteMissed -= OnNoteMissed;
            subjectToObserve.NotePerfect -= OnNotePerfect;
            subjectToObserve.NoteGood -= OnNoteGood;
            subjectToObserve.NoteBad -= OnNoteBad;
        }
    }
}
