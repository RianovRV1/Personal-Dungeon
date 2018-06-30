using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum UIState { Roster, Guild, Map }
    public UIState state;

    [SerializeField] private GameObject map;
    [SerializeField] private GameObject guild;
    [SerializeField] private GameObject RosterUI;

    void Start()
    {
        map.SetActive(false);
        guild.SetActive(false);
        RosterUI.SetActive(false);

        NextState();
    }

    IEnumerator GuildState()
    {
        Debug.Log("Guild: Enter");
        guild.SetActive(true);

        while (state == UIState.Guild)
        {
            yield return null;
        }

        guild.SetActive(false);
        Debug.Log("Guild: Exit");
        NextState();
    }

    IEnumerator MapState()
    {
        Debug.Log("Map: Enter");
        map.SetActive(true);
        this.GetComponent<MapControl>().enabled = true;
        
        while (state == UIState.Map)
        {
            yield return null;
        }

        this.GetComponent<MapControl>().enabled = false;
        map.SetActive(false);
        Debug.Log("Map: Exit");

        NextState();
    }

    IEnumerator RosterState()
    {
        Debug.Log("Roster: Enter");
        guild.SetActive(true);
        RosterUI.SetActive(true);

        while (state == UIState.Roster)
        {
            yield return null;
        }
        Debug.Log("Roster: Exit");
        guild.SetActive(false);
        RosterUI.SetActive(false);
        NextState();
    }

    void NextState()
    {
        string methodName = state.ToString() + "State";
        System.Reflection.MethodInfo info = GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }
}
