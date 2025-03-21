using UnityEngine;

/* 
 - Nom du fichier : MusicNoteFactory
 - Contexte : Factory pattern pour la cr�ation de chaque note
 - Auteurs : Matis Gaetjens et Gabriel Tremblay 
*/

public class MusicNoteFactory
{
    public void CreateMusicNote(GameObject notePrefab, Vector3 position, Color color, KeyCode keyToPress)
    {
        GameObject noteObject = GameObject.Instantiate(notePrefab, GameObject.Find("NoteHolder").transform);
        noteObject.transform.position = position;
        ObjectNote objectNote = noteObject.GetComponent<ObjectNote>();
        objectNote.SetColor(color);
        objectNote.keyToPress = keyToPress;
    }
}
