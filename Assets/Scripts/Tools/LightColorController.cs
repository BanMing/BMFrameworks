using UnityEngine;
using System.Collections;
//using MahJong;
public class LightColorController : MonoBehaviour {
    public Vector4 m_source_color = new Color(0.2f, 0.2f, 0.2f, 0);
    public Vector4 m_target_color = new Color(0.2f, 0.2f, 0.2f, 0);
    public Vector4 m_cur_color;
    
    public bool m_change_direction = true;
    public float m_interval_time = 1f;
    public bool m_is_flashing = false;
	// Update is called once per frame
	void Update ()
    {
        if(!m_is_flashing)
        {
            return;
        }
        if(m_cur_color.w > m_target_color.w && m_cur_color.w > m_source_color.w)
        {
            m_change_direction = false;
        }
        else if (m_cur_color.w < m_target_color.w && m_cur_color.w < m_source_color.w)
        {
            m_change_direction = true;
        }
        Vector4 change_value;
        if (m_change_direction)
        {
            change_value = m_target_color - m_source_color;
        }
        else
        {
            change_value = m_source_color - m_target_color;
        }
        change_value *= Time.deltaTime / m_interval_time;
        m_cur_color += change_value;
        Renderer renderer = transform.GetComponent<Renderer>();
        Color result_color = new Color(m_cur_color.x, m_cur_color.y, m_cur_color.z, m_cur_color.w);
        renderer.material.SetColor("_SelfColor", result_color);
    }

    public void SetColor(Color color, bool is_flashing)
    {
        m_is_flashing = is_flashing;
        m_target_color = color;
        m_cur_color = m_source_color;
        Renderer renderer = transform.GetComponent<Renderer>();
        Color result_color = new Color(m_cur_color.x, m_cur_color.y, m_cur_color.z, m_cur_color.w);
        renderer.material.SetColor("_SelfColor", result_color);
    }
}
