/******************************************************************
** 文件名:	
** 版  权:	(C)  
** 创建人:  Liange
** 日  期:  2015.4.30
** 描  述: 	

    只在Windows和编辑器中显示
    http://wiki.unity3d.com/index.php?title=FramesPerSecond
**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowFPS : MonoBehaviour 
{
    public float mUpdateInterval = 0.5f;
    public bool m_show = true;
    
    private float mLeftTime = 0;

    private int mFrames = 0;    // Frames drawn over the interval
    private float mAccum = 0;   // FPS accumulated over the interval

    private string mTextData = null;

    private GUIStyle mGUIStyle = new GUIStyle();

    private Rect m_PosSize;

    private bool m_already_quality_tip = false;
    private int m_low_fps_count = 0;

    void Start()
    {   
#if UNITY_EDITOR||UNITY_STANDALONE_WIN
#else
        Destroy(this);
        return;
#endif
        DontDestroyOnLoad(gameObject);
        mLeftTime = mUpdateInterval;
        
        mGUIStyle.normal.textColor = Color.red;
        mGUIStyle.fontSize = 12;

        m_PosSize = new Rect(Screen.width - 130, 1, 70, 50);
    }

    void Update()
    {
        ++mFrames;

		mLeftTime -= Time.deltaTime;
		mAccum += Time.timeScale / Time.deltaTime;

        //mLeftTime -= Time.unscaledDeltaTime;
        //mAccum += 1 / Time.unscaledDeltaTime;

        // Interval ended - update GUI text and start new interval
        if (mLeftTime <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = mAccum / mFrames;
			
            //int particleNum = CountParticleNum();
            //mTextData = System.String.Format("FPS:{0:F2}, 粒子数:{1:d}", fps, particleNum);
			mTextData = System.String.Format("FPS:{0:F2}", fps);
			
            if (fps < 20)
            {
                mGUIStyle.normal.textColor = Color.red;
            }
            else if (fps < 25)
            {
                mGUIStyle.normal.textColor = Color.yellow;
            }
            else
            {
                mGUIStyle.normal.textColor = Color.green;
            }
            
            mLeftTime = mUpdateInterval;
            mAccum = 0.0F;
            mFrames = 0;
        }
    }

    void OnGUI()
    {
        if (m_show)
        {
            GUI.Label(m_PosSize, mTextData, mGUIStyle);
        }
    }

    //计算当前场景粒子数量
    int CountParticleNum()
    {
        int totalNum = 0;
        ParticleSystem[] psList = GameObject.FindObjectsOfType<ParticleSystem>();

        foreach (ParticleSystem ps in psList)
        {
            if (ps.isPlaying)
            {
                //同屏粒子数(当前被渲染数量)
                if (ps.GetComponent<Renderer>() != null && ps.GetComponent<Renderer>().isVisible)
                {
                    totalNum += ps.particleCount;
                }   
            }
        }

        return totalNum;
    }
}
