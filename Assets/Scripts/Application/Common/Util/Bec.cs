using UnityEngine;
using System.Collections.Generic;

public class Bec {
    public static Bec instance = new Bec();
    
    const float defaultThreshold = 0.5f;
    Dictionary<string, float> map = new Dictionary<string, float>();

    public bool denyAll { get; set; }

    public bool Can() {
        return Can("default");
    }

    public bool Can(string name) {
        return CanEnter(name, defaultThreshold);
    }

    public bool CanEnter(string name, float threshold) {
        if (denyAll) {
            return false;
        }

        float now = Time.realtimeSinceStartup;
        if (map.ContainsKey(name)) {
            float diff = now - map[name];
            if (diff < threshold) {
                return false;
            }
        }

        map[name] = now;
        Service.sound.PlayEffect("click");
        return true;
    }

    public void Clear() {
        map.Clear();
        denyAll = false;
    }

    public void Clear(string name) {
        map.Remove(name);
    }
}