using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ItemList : MonoBehaviour
{
    public enum Direction
    {
        Vertical,
        Horiziontal
    }


    //basic items
    [Header("列表的基本设置")]
    public Transform m_content_root;
    public Transform m_template;
    [SerializeField]
    private string m_trans_name;

    public List<Transform> m_trans_list;
    private ObjectPool m_object_pool;

    //滑动列表设置
    [Header("以下为滑动列表设置，is_dynamic为true时生效")]
    [SerializeField]
    private bool m_is_dynamic;
    [SerializeField]
    private ScrollRect m_scrollview;
    [SerializeField]
    private Direction m_direction;
    [SerializeField]
    private Vector2 m_cell_size;
    [SerializeField]
    private int m_cell_count_eachline;

    private Vector2 m_page_size;
    private int m_page_count;
    private int m_total_count;
    private int m_cur_index;
    private Vector3 m_origion_pos;
    
    //动态滑动列表额外设置

    [SerializeField, Range(1, 5)]
    private int m_cache_count;

    private Action<Transform, int> m_instantiate_action;


    public int Count
    {
        get
        {
            if(m_trans_list != null)
            {
                return m_trans_list.Count;
            }
            else
            {
                return 0;
            }
        }
    }

	public int CellCountEachline
	{
		set{
			m_cell_count_eachline = value;
		}
	}
    #region INTERFACE
    //-------Interface----------

    /// <summary>
    /// 初始化，创建对象池并清空列表
    /// </summary>
    public void Init(Action<Transform, int> action = null)
    {
        CheckPool();
        m_trans_list.Clear();
        m_object_pool.RecycleAll();

        if (m_is_dynamic)
        {
            GridLayoutGroup grid = transform.GetComponentInChildren<GridLayoutGroup>();
            if(grid != null)
            {
                grid.enabled = false;
            }
            ContentSizeFitter filter = transform.GetComponentInChildren<ContentSizeFitter>();
            if(filter != null)
            {
                filter.enabled = false;
            }
            if(action != null)
            {
                m_instantiate_action = action;
            }

            RectTransform rectTrans = m_scrollview.GetComponent<RectTransform>();
            m_page_size = new Vector2(rectTrans.rect.width, rectTrans.rect.height);
            if (m_cell_size == null || m_cell_size == Vector2.zero)
            {
                m_cell_size = m_template.GetComponent<RectTransform>().sizeDelta;
            }
            m_page_count = Mathf.CeilToInt(m_page_size.y * 1.0f / m_cell_size.y) * m_cell_count_eachline;
        }
    }

    /// <summary>
    /// 反初始化
    /// </summary>
    public void UnInit()
    {
        CheckPool();
        Clear();
    }

    public void Clear()
    {
        m_trans_list.Clear();
        m_object_pool.RecycleAll();
    }

    public void DeepClear()
    {
        m_object_pool.Clear();
    }

    /// <summary>
    /// 重新生成指定个数的Transform
    /// </summary>
    /// <param name="count"></param>
    public void InstantiateItemList(int count)
    {
        CheckPool();
        Clear();

        for (int i = 0; i < count; ++i)
        {
            Transform trans = m_object_pool.Get();
            trans.name = m_trans_name + i;
            if (m_content_root != null)
            {
                trans.SetParent(m_content_root, false);
            }
            else
            {
                trans.SetParent(transform, false);
            }
            trans.SetSiblingIndex(i);
            m_trans_list.Add(trans);
        }
    }

    /// <summary>
    /// 检查对象池是否初始化，如果没有则初始化
    /// </summary>
    public void CheckPool()
    {
        if (m_object_pool == null)
        {
            if (m_template == null)
            {
                m_template = transform.GetChild(0).GetComponent<RectTransform>();
            }
            m_object_pool = new ObjectPool(m_template);
            if (m_trans_name == null)
            {
                m_trans_name = m_template.name;
            }
        }
    }

    /// <summary>
    /// 生成一个Transform
    /// </summary>
    /// <returns></returns>
    public Transform InstantiateItem()
    {
        CheckPool();
        int idx = m_trans_list.Count;
        Transform trans = m_object_pool.Get();
        trans.name = m_trans_name + idx;
      
        if (m_content_root != null)
        {
            trans.SetParent(m_content_root, false);
        }
        else
        {
            trans.SetParent(transform, false);
            Debug.LogWarning("m_conent_rectTrans is null!");
        }
        trans.SetSiblingIndex(idx);
        m_trans_list.Add(trans);
        return trans;
    }

    /// <summary>
    /// 根据下标回收item
    /// </summary>
    /// <param name="index"></param>
    public void RemoveItem(int index)
    {
        if(m_trans_list.Count > index)
        {
            m_object_pool.Recycle(index);
        }
    }

    /// <summary>
    /// 回收transform
    /// </summary>
    /// <param name="trans"></param>
    public void RemoveItem(Transform trans)
    {
        m_trans_list.Remove(trans);
        m_object_pool.Recycle(trans);
    }

    public void LookatByIdx(int idx)
    {
        
    }

    public void SetChildActive(bool flag) {
        for (int i = 0; i < m_trans_list.Count; i++)
        {
            MyUnityTool.SetActive(m_trans_list[i], flag);
        }
    }

    /// <summary>
    /// 让滑动列表移到最低端
    /// </summary>
    public void ResetContentToButtom()
    {
        if (gameObject.active)
        {
            StartCoroutine(mResetContentToButtom());
        }
    }
    private IEnumerator mResetContentToButtom() {
        yield return new WaitForEndOfFrame();
        var contentRectTransform = (RectTransform)m_content_root;
        contentRectTransform.anchorMin = Vector2.zero;
        contentRectTransform.anchorMax = new Vector2(1, 0);
        var moveY = contentRectTransform.rect.height;
        contentRectTransform.localPosition = new Vector3(contentRectTransform.localPosition.x, moveY, contentRectTransform.localPosition.z);
	}

	public void ResetContentToTop()
	{
		if (gameObject.active)
		{
			StartCoroutine(mResetContentToTop());
		}
	}

	private IEnumerator mResetContentToTop() {
		yield return new WaitForEndOfFrame();
		//m_content_root.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		var contentRectTransform = (RectTransform)m_content_root;
		Vector2 sizeDelta = contentRectTransform.sizeDelta;
		contentRectTransform.offsetMax = new Vector2 (contentRectTransform.offsetMax.x,0);
		contentRectTransform.offsetMin = contentRectTransform.offsetMax - new Vector2(sizeDelta.x,m_cell_size.y * Mathf.CeilToInt(m_total_count * 1.0f / m_cell_count_eachline));
	}


    /// <summary>
    /// 设置动态列表用的回调函数
    /// </summary>
    /// <param name="action"></param>
    public void SetInstantiateAction(Action<Transform, int> action)
    {
        m_instantiate_action = action;
    }
    
    /// <summary>
    /// 设置动态列表总数目
    /// </summary>
    /// <param name="count"></param>
    public void SetDynamicTotalCount(int count)
    {
        m_total_count = count;
        m_cur_index = -1;
        
        UpdateItems();
        if ( m_direction == Direction.Vertical)
        {
            m_content_root.GetComponent<RectTransform>().sizeDelta = new Vector2(m_cell_size.x * m_cell_count_eachline, m_cell_size.y * Mathf.CeilToInt(count * 1.0f / m_cell_count_eachline));
        }
    }

    void Update()
    {
        if(!m_is_dynamic)
        {
            return;
        }
        if (m_scrollview == null)
        {
            return;
        }
        UpdateItems();
    }

    #endregion INTERFACE

    #region PRIVATE

    private void UpdateItems()
    {
        int start_index;
        if (m_direction == Direction.Horiziontal)
        {
            start_index = GetStartIndexByPos(m_content_root.localPosition.x);
        }
        else
        {
            start_index = GetStartIndexByPos(m_content_root.localPosition.y);
        }

        if(start_index == m_cur_index)
        {
            return;
        }
        else
        {
            m_cur_index = start_index;
        }

        Clear();

		int cur_index = m_cur_index;
		int end_index = m_cur_index + m_page_count + m_cell_count_eachline * m_cache_count * 2 - 1;
        if(end_index > m_total_count - 1)
        {
            end_index = m_total_count - 1;
        }
        while(cur_index <= end_index)
        {
            Vector2 pos = GetContentPosByIndex(cur_index);
            Transform trans = InstantiateItem();
            trans.name = m_trans_name + cur_index;
            RectTransform rect_trans = trans.GetComponent<RectTransform>();
            rect_trans.anchoredPosition = pos;
            if(m_instantiate_action != null)
            {
                m_instantiate_action(trans, cur_index);
            }
            
            ++cur_index;
        }
    }

    private int GetStartIndexByPos(float pos)
    {
        float start_pos;
        int start_index = 0;
        if (m_direction == Direction.Vertical)
        {
            start_pos = m_content_root.GetComponent<RectTransform>().anchoredPosition.y
				- m_cell_size.y * m_cache_count;
            if(start_pos < 0)
            {
                start_pos = 0;
            }
			start_index = (int)(start_pos / m_cell_size.y) * m_cell_count_eachline;
        }
        return start_index;
    }

    private Vector2 GetContentPosByIndex(int index)
    {
        int rows;
        int columns;
        Vector2 pos = Vector2.zero;
        if (m_direction == Direction.Vertical)
        {
			rows = Mathf.CeilToInt((index + 1) * 1.0f / m_cell_count_eachline) - 1;
            columns = index % m_cell_count_eachline;
            pos = new Vector2(columns * m_cell_size.x, -rows * m_cell_size.y);
        }
        return pos;
    }
    #endregion
}
