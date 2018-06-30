using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public GameObject characterPrefab;
    public static GameObject prefab;
    
    private static string dataPath;
    private static string characterDataPath;
    private static string characterFileName;

    private void Awake()
    {
        prefab = characterPrefab;
        //get path and file name then combine
        characterFileName = "Characters.json";
        dataPath = Application.persistentDataPath;
        characterDataPath = Path.Combine(dataPath, characterFileName);
    }

    public static Character CreateCharacter(CharacterData data, GameObject prefab, Transform trans)
    {
        GameObject newCharacter;
        Character character;

        //make a new instance of character
        newCharacter = Instantiate(prefab, trans);
        //get character script and if none exists, make one
        character = newCharacter.GetComponent<Character>() ?? newCharacter.AddComponent<Character>();

        //set character data
        character.characterData = data;

        return character;
    }

    public void Load()
    {
        CharacterSave.Load(characterDataPath);
    }

    public void Save()
    {
        CharacterSave.Save(characterDataPath, CharacterSave.characterContainer);
    }
}
