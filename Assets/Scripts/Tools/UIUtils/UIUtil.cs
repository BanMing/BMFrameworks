using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class UIUtil{

    public static void SetActive(Transform trans, bool visible, bool recursive = false)
    {
        trans.gameObject.SetActive(visible);
        if (recursive)
        {
            for (int i = 0, imax = trans.childCount; i < imax; ++i)
            {
                Transform child = trans.GetChild(i);
                SetActive(child, visible, recursive);
            }
        }
    }

    public static void SetAlpha(Transform trans, float alpha)
    {
        //CanvasRenderer[] renderer = trans.GetComponentsInChildren<CanvasRenderer>();
        //for(int i = 0; i < renderer.Length; ++i)
        //{
        //    renderer[i].SetAlpha(alpha);
        //}
        if(trans == null)
        {
            return;
        }
        Graphic[] component = trans.GetComponentsInChildren<Graphic>();
        for (int i = 0; i < component.Length; ++i)
        {
            component[i].DOFade(alpha, 0);
        }
    }

    public static void DoAlpha(Transform trans, float alpha, float duration)
    {
        if(trans == null)
        {
            return;
        }
        Graphic[] component = trans.GetComponentsInChildren<Graphic>();
        for (int i = 0; i < component.Length; ++i)
        {
            component[i].DOFade(alpha, duration);
        }
    }

    public static void SetColor(Transform trans, Color color, List<Transform> ignore_list)
    {
        if(trans == null)
        {
            return;
        }
        Graphic[] component = trans.GetComponentsInChildren<Graphic>();
        for (int i = 0; i < component.Length; ++i)
        {
            component[i].color = color;
        }
    }

    public static void SetColor(Transform trans, Color color)
    {
        SetColor(trans, color, null);
    }

    public static void DoColor(Transform trans, Color color, float duration, List<Transform> ignore_list)
    {
        if (trans == null)
        {
            return;
        }
        Graphic[] component = trans.GetComponentsInChildren<Graphic>();
        for (int i = 0; i < component.Length; ++i)
        {
            component[i].DOColor(color, duration);
        }
    }

    public static void DoColor(Transform trans, Color color, float duration)
    {
        DoColor(trans, color, duration, null);
    }

    public static Transform GetTrans(Transform trans, string name)
    {
        Transform result_trans = trans.Find(name);
        if (trans != null)
        {
            return result_trans;
        }
        else
        {
            for (int i = 0, imax = trans.childCount; i < imax; ++i)
            {
                Transform child_trans = trans.Find(name);
                if (child_trans != null)
                {
                    return child_trans;
                }
            }
        }
        return null;
    }

    public static Vector2 ScreenPointToLocalPointInRectangle(Vector3 point)
    {

        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Instance.UIRoot, point, UIManager.Instance.RootCanvas.worldCamera, out pos))
        {
            return pos;
        }

        return Vector2.zero;
    }
    //public static void SetIconById(int id, RawImage raw_image)
    //{
    //    IconData iconData = IconData.dataMap.Get(id);
    //    AssetCacheMgr.GetUIResource(iconData.path, (obj) =>
    //    {
    //        if (obj != null)
    //        {
    //            raw_image.texture = obj as Texture;
    //        }
    //    });
    //}

    //public static void SetIcon(string icon_path, RawImage raw_image)
    //{
    //    AssetCacheMgr.GetUIResource(icon_path, (obj) =>
    //    {
    //        if (obj != null)
    //        {
    //            raw_image.texture = obj as Texture;
    //        }
    //    });
    //}
}
