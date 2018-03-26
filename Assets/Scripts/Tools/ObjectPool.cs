using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool
{
    private int m_used_count;
    List<Transform> m_trans_list = new List<Transform>();
    Transform m_template;
    public ObjectPool(Transform trans)
    {
        m_template = trans;
        m_template.gameObject.SetActive(false);
        m_used_count = 0;
    }

    public Transform Get()
    {
        ++m_used_count;
        if(m_used_count - 1 < m_trans_list.Count)
        {
            if(m_trans_list[m_used_count - 1] != null)
            {
                Transform result = m_trans_list[m_used_count - 1];
                result.gameObject.SetActive(true);
                return result;
            }
            else
            {
                return Get();
            }
        }
        else
        {
            Transform trans = Transform.Instantiate(m_template) as Transform;
            m_trans_list.Add(trans);
            trans.gameObject.SetActive(true);
            return trans;
        }
    }
    public void Recycle(Transform trans_to_recycle)
    {
        int index = m_trans_list.FindIndex((Transform trans) =>
        {
            if (trans == trans_to_recycle)
            {
                return true;
            }
            else
            {
                return false;
            }
        });
        Recycle(index);
    }

    public void Recycle(int idx)
    {
        Transform trans = m_trans_list[idx];
        trans.gameObject.SetActive(false);
        --m_used_count;
        m_trans_list.RemoveAt(idx);
        m_trans_list.Add(trans);
    }

    public void RecycleAll()
    {
        for (int i = 0; i < m_trans_list.Count; ++i)
        {
            m_trans_list[i].gameObject.SetActive(false);
        }
        m_used_count = 0;
    }

    public void Clear()
    {
        m_used_count=0;
        m_trans_list.Clear();
    }
}
