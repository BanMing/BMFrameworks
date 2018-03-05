using UnityEngine;
public class ResourcesManager {
    public static UnityEngine.GameObject GetInstanceGameOject (string path) {
        // var obj= Resources.Load<GameObject>(path);
        // Debug.Log(obj.name);
        // GameObject gameObject= GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(path));
        
        return GameObject.Instantiate<GameObject> (Resources.Load<GameObject> (path));
    }
}