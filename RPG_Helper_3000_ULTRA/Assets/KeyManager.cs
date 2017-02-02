using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class KeyManager : MonoBehaviour {

    public enum KeyReceivingImportance {
        high = 10,
        standard = 0,
        low = -10
    }

    private static KeyManager instance;

    private Dictionary<KeyCode, Dictionary<KeyReceivingImportance, List<IKeyCodeReceiver>>> listeners = new Dictionary<KeyCode, Dictionary<KeyReceivingImportance, List<IKeyCodeReceiver>>>();

    public void registerListener(IKeyCodeReceiver listener, KeyCode key, KeyReceivingImportance prio) {
        if (listeners.ContainsKey(key) == false) {
            listeners.Add(key, new Dictionary<KeyReceivingImportance, List<IKeyCodeReceiver>>());
        }
        if (listeners[key].ContainsKey(prio) == false) {
            listeners[key].Add(prio, new List<IKeyCodeReceiver>());
        }

        if (listeners[key][prio].Contains(listener))
        {
            return;
        }
        listeners[key][prio].Add(listener);
    }

    public void unregisterListener(IKeyCodeReceiver listener, KeyCode key, KeyReceivingImportance prio) {
        listeners[key][prio].Remove(listener);
    }

    public KeyManager() {
        if (instance != null) throw new System.SystemException();
        instance = this;
    }

    public static KeyManager GetInstance() {
        return instance;
    }

	// Update is called once per frame
	void Update () {

        foreach (KeyCode kc in listeners.Keys) {
            if (Input.GetKeyDown(kc)) {
                OnKeyDown(kc);
            }
        }
    }

    private void OnKeyDown(KeyCode kc)
    {
        KeyReceivingImportance[] priorities = ((KeyReceivingImportance[])System.Enum.GetValues(typeof(KeyReceivingImportance)))
                                          .OrderByDescending(x => x).ToArray();

        for (int i = 0; i < priorities.Length; ++i) {
            KeyReceivingImportance prio = priorities[i];
            if (listeners[kc].Keys.Contains(prio))
            {
                List<IKeyCodeReceiver> receiver = listeners[kc][prio];
                int amountListeners = receiver.Count;
                for (int k = 0; k < amountListeners; ++k)
                {
                    if (receiver[k].HandlesKeyDown(kc) == true)
                    {
                        return; //someone handled the event
                    }
                }
            }
        }
    }
}
