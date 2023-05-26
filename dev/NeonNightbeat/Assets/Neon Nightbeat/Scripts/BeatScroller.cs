using UnityEngine;

/* 
 - Nom du fichier : BeatScroller
 - Contexte : D�filement des notes
 - Auteurs : Matis Gaetjens et Gabriel Tremblay 
*/

public class BeatScroller : MonoBehaviour // Le script qui va �tre attach� � un objet qui contient toutes les notes et qui les faire tomber
{
    public float beatTempo;
    public bool hasStarted;

    void Start()
    {
        beatTempo = beatTempo / 60f;
    }

    void Update()
    {
        
        if (hasStarted)
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
    }
}
