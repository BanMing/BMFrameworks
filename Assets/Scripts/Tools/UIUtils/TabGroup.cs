using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TabGroup : MonoBehaviour
{
    public List<Transform> m_trans_list = new List<Transform>();

    public List<Transform> m_tab_btn_list = new List<Transform>();

    public int Count
    {
        get
        {
            return m_trans_list.Count;
        }
    }
    public void Clear()
    {
        m_trans_list.Clear();
    }
    public void Add(Transform trans)
    {
        m_trans_list.Add(trans);
    }

    public void Remove(string name)
    {
        int i, imax;
        for (i = 0, imax = m_trans_list.Count; i < imax; ++i)
        {
            if (m_trans_list[i].name == name)
            {
                break;
                UIUtil.SetActive(m_trans_list[i], true);
            }
        }
        if (i < m_trans_list.Count)
        {
            m_trans_list.RemoveAt(i);
        }
    }

    public void Remove(Transform trans)
    {
        m_trans_list.Remove(trans);
    }

    public void Remove(int idx)
    {
        m_trans_list.RemoveAt(idx);
    }

    public Transform this[int index]
    {
        get
        {
            if(m_trans_list.Count <= index)
            {
                Debug.LogError("Index out of tab group");
                if (m_trans_list.Count > 0)
                {
                    return m_trans_list[m_trans_list.Count - 1];
                }
                else
                {
                    return null;
                }
            }
            return m_trans_list[index];
        }
    }

    public void Switch(int index)
    {
        //if(index < 0 || m_trans_list.Count <= index)
        //{
        //    Debug.LogError("index error");
        //}

        for (int i = 0, imax = m_trans_list.Count; i < imax; ++i)
        {
            if (i == index)
            {
                UIUtil.SetActive(m_trans_list[i], true);
                if(m_tab_btn_list.Count > i)
                {
                    UIUtil.SetActive(m_tab_btn_list[i], true);
                }
            }
            else
            {
                UIUtil.SetActive(m_trans_list[i], false);
                if (m_tab_btn_list.Count > i)
                {
                    UIUtil.SetActive(m_tab_btn_list[i], false);
                }
            }
        }
    }
    public void Switch(string name)
    {
        for(int i = 0, imax = m_trans_list.Count; i < imax; ++i)
        {
            if(m_trans_list[i].name == name)
            {
                UIUtil.SetActive(m_trans_list[i], true);
            }
            else
            {
                UIUtil.SetActive(m_trans_list[i], false);
            }
        }
    }

    public void Swtich(Transform trans)
    {
        for (int i = 0, imax = m_trans_list.Count; i < imax; ++i)
        {
            if (m_trans_list[i] == trans)
            {
                UIUtil.SetActive(m_trans_list[i], true);
            }
            else
            {
                UIUtil.SetActive(m_trans_list[i], false);
            }
        }
    }
}
