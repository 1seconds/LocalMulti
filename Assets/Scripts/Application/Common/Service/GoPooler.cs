using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GoPooler : SingletonGameObject<GoPooler> {
    Dictionary<string, Stack> storedObjects = new Dictionary<string, Stack>();
    Dictionary<string, GameObject> storedPrefabs = new Dictionary<string, GameObject>();

    public T Get<T>(string path, GameObject parent) where T : GoItem {
        if (storedObjects.ContainsKey(path)) {
            Stack stack = storedObjects[path];
            GameObject stored = PopGameObjectFromStack(stack);
            if (stored != null) {
                SetReady(stored, parent);
                return stored.GetComponent<T>();
            }
        }

        GameObject newly = MakeLocal(path);
        if (newly == null) {
            return null;
        }

        SetReady(newly, parent);
        return newly.GetComponent<T>();
    }

    public T Get<T>(GameObject prefab, GameObject parent) where T : GoItem {
        string path = prefab.GetHashCode().ToString();
        storedPrefabs[path] = prefab;
        return Get<T>(path, parent);
    }

    public T Get<T>(GoItem prefab, GameObject parent) where T : GoItem {
        return this.Get<T>(prefab.gameObject, parent);
    }

    public T Get<T>(GoItem prefab, Transform parent) where T : GoItem {
        return this.Get<T>(prefab.gameObject, parent.gameObject);
    }

    public void Return<T>(List<T> items, bool clearList) where T : GoItem {
        foreach (var item in items) {
            Return(item);
        }

        if (clearList) {
            items.Clear();
        }
    }

    public void Return(GoItem item) {
        if (item == null) {
            return;
        }

        string path = item.resourcePath;
        if (path == null || path.Length == 0) {
            Destroy(item.gameObject);
            return;
        }

        if (item.isInPool == true) {
            return;
        }

        Stack stack = GetStack(path);
        stack.Push(new WeakReference(item.gameObject));
        item.isInPool = true;
        item.OnGoingIntoPool();
        item.transform.localPosition = Vector3.up * 9999;
        item.gameObject.SetActive(false);
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void Return(GoItem item, float delay) {
        StartCoroutine(ReturnWithDelay(item, delay));
    }

    private IEnumerator ReturnWithDelay(GoItem item, float delay) {
        if (item == null) {
            yield break;
        }
        
        int useCount = item.useCount;
        yield return new WaitForSeconds(delay);
        if (item == null || useCount != item.useCount) {
            yield break;
        }

        Return(item);
    }

    private GameObject MakeLocal(string path) {
        GameObject itemObj = null;
        
        GameObject prefab;
        if (!storedPrefabs.TryGetValue(path, out prefab) || prefab == null) {
            prefab = Resources.Load<GameObject>(path);
            storedPrefabs[path] = prefab;
        }

        itemObj = Instantiate(prefab) as GameObject;
        if (itemObj == null) {
            Debug.LogError("Can not load GameObject:" + path);
            return null;
        }

        GoItem item = itemObj.GetComponent<GoItem>();
        if (item == null) {
            Debug.LogError("Doesn't have GoItem:" + path);
        }

        item.resourcePath = path;
        item.transform.localScale = Vector3.one;
        return itemObj;
    }

    private Stack GetStack(string path) {
        Stack stack = null; 
        if (storedObjects.ContainsKey(path)) {
            stack = storedObjects[path];
        }
        else {
            stack = new Stack();
            storedObjects[path] = stack;
        }

        return stack;
    }

    private void SetReady(GameObject itemObj, GameObject parent) {
        if (parent != null) {
            itemObj.transform.SetParent(parent.transform);
        }

        itemObj.transform.localPosition = Vector3.up * 9999;
        itemObj.transform.localScale = Vector3.one;
        itemObj.SetActive(true);

        GoItem item = itemObj.GetComponent<GoItem>();
        if (item != null) {
            item.useCount++;
            item.isInPool = false;
            item.OnGettingOutPool();
        }
    }

    private GameObject PopGameObjectFromStack(Stack stack) {
        while (stack.Count > 0) {
            WeakReference item = (WeakReference)stack.Pop();

            if (item.Target != null) {
                return item.Target as GameObject;
            }
        }

        return null;
    }
}
