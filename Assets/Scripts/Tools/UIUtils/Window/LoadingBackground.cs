using UnityEngine;
using System.Collections;

public class LoadingBackground : SingletonMonoBehaviour<LoadingBackground>
{

    new static public string ResPath = "Prefab/Loading";
    public GameObject m_LoadingObject = null;

    static public void Close()
    {
        if (m_Instance != null)
        {
            Destroy(Instance.gameObject);
        }
    }

    public void SetVisible(bool visible)
    {
        this.gameObject.SetActive(visible);
    }
}