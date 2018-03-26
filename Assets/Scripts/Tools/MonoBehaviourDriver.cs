/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/11/16
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using System;
using System.Collections;

public class MonoBehaviourDriver : MonoBehaviour
{
    void Init()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void UnInit()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnloaded;
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    public Action m_Awake = null;
    void Awake()
    {
        if (m_Awake != null)
        {
            m_Awake();
        }        
    }

    public Action m_Start = null;
    void Start ()
    {
        Init();

        if (m_Start != null)
        {
            m_Start();
        }
    }

    public Action m_FixedUpdate = null;
    void FixedUpdate()
    {
        if (m_FixedUpdate != null)
        {
            m_FixedUpdate();
        }
    }

    public Action m_Update = null;
    void Update ()
    {
        if (m_Update != null)
        {
            m_Update();
        }
    }

    public Action m_LateUpdate = null;
    void LateUpdate()
    {
        if (m_LateUpdate != null)
        {
            m_LateUpdate();
        }
    }

    public Action m_OnEnable = null;
    void OnEnable()
    {
        if (m_OnEnable != null)
        {
            m_OnEnable();
        }
    }

    public Action m_OnDisable = null;
    void OnDisable()
    {
        if (m_OnDisable != null)
        {
            m_OnDisable();
        }
    }

    public Action m_OnDestroy = null;
    void OnDestroy()
    {
        if (m_OnDestroy != null)
        {
            m_OnDestroy();
        }

        UnInit();
    }

    #region 场景消息

    public Action<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode> m_OnSceneLoaded = null;
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode node)
    {
        if (m_OnSceneLoaded != null)
        {
            m_OnSceneLoaded(scene, node);
        } 
    }

    public Action<UnityEngine.SceneManagement.Scene> m_OnSceneUnloaded = null;
    void OnSceneUnloaded(UnityEngine.SceneManagement.Scene scene)
    {
        if (m_OnSceneUnloaded != null)
        {
            m_OnSceneUnloaded(scene);
        }
    }

    public Action<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.Scene> m_OnActiveSceneChanged = null;
    public void OnActiveSceneChanged(UnityEngine.SceneManagement.Scene scene1, UnityEngine.SceneManagement.Scene scene2)
    {
        if (m_OnActiveSceneChanged != null)
        {
            m_OnActiveSceneChanged(scene1, scene2);
        }
    }

    #endregion

    #region 应用程序

    public Action<bool> m_OnApplicationFocus = null;
    void OnApplicationFocus(bool focusStatus)
    {
        if (m_OnApplicationFocus != null)
        {
            m_OnApplicationFocus(focusStatus);
        }
    }

    public Action<bool> m_OnApplicationPause = null;
    void OnApplicationPause(bool pauseStatus)
    {
        if (m_OnApplicationPause != null)
        {
            m_OnApplicationPause(pauseStatus);
        }
    }

    public Action m_OnApplicationQuit = null;
    void OnApplicationQuit()
    {
        if (m_OnApplicationQuit != null)
        {
            m_OnApplicationQuit();
        }
    }

    #endregion

    #region 碰撞

    public Action<Collision> m_OnCollisionEnter = null;
    void OnCollisionEnter(Collision collision)
    {
        if (m_OnCollisionEnter != null)
        {
            m_OnCollisionEnter(collision);
        }
    }

    public Action<Collision> m_OnCollisionExit = null;
    void OnCollisionExit(Collision collision)
    {
        if (m_OnCollisionExit != null)
        {
            m_OnCollisionExit(collision);
        }
    }

    public Action<Collision> m_OnCollisionStay = null;
    void OnCollisionStay(Collision collision)
    {
        if (m_OnCollisionStay != null)
        {
            m_OnCollisionStay(collision);
        }
    }

    public Action<Collision2D> m_OnCollisionEnter2D = null;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_OnCollisionEnter2D != null)
        {
            m_OnCollisionEnter2D(collision);
        }
    }

    public Action<Collision2D> m_OnCollisionExit2D = null;
    void OnCollisionExit2D(Collision2D collision)
    {
        if (m_OnCollisionExit2D != null)
        {
            m_OnCollisionExit2D(collision);
        }
    }

    public Action<Collision2D> m_OnCollisionStay2D = null;
    void OnCollisionStay2D(Collision2D collision)
    {
        if (m_OnCollisionStay2D != null)
        {
            m_OnCollisionStay2D(collision);
        }
    }

    public Action<GameObject> m_OnParticleCollision = null;
    void OnParticleCollision(GameObject other)
    {
        if (m_OnParticleCollision != null)
        {
            m_OnParticleCollision(other);
        }
    }

    public Action m_OnParticleTrigger = null;
    void OnParticleTrigger()
    {
        if (m_OnParticleTrigger != null)
        {
            m_OnParticleTrigger();
        }
    }

    #endregion

    #region 触发器

    public Action<Collider> m_OnTriggerEnter = null;
    void OnTriggerEnter(Collider other)
    {
        if (m_OnTriggerEnter != null)
        {
            m_OnTriggerEnter(other);
        }
    }

    public Action<Collider> m_OnTriggerExit = null;
    void OnTriggerExit(Collider other)
    {
        if (m_OnTriggerExit != null)
        {
            m_OnTriggerExit(other);
        }
    }

    public Action<Collider> m_OnTriggerStay = null;
    void OnTriggerStay(Collider other)
    {
        if (m_OnTriggerStay != null)
        {
            m_OnTriggerStay(other);
        }
    }

    public Action<Collider2D> m_OnTriggerEnter2D = null;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_OnTriggerEnter2D != null)
        {
            m_OnTriggerEnter2D(other);
        }
    }

    public Action<Collider2D> m_OnTriggerExit2D = null;
    void OnTriggerExit2D(Collider2D other)
    {
        if (m_OnTriggerExit2D != null)
        {
            m_OnTriggerExit2D(other);
        }
    }

    public Action<Collider2D> m_OnTriggerStay2D = null;
    void OnTriggerStay2D(Collider2D other)
    {
        if (m_OnTriggerStay2D != null)
        {
            m_OnTriggerStay2D(other);
        }
    }

    #endregion

    #region 渲染

    public Action m_OnBecameInvisible = null;
    void OnBecameInvisible()
    {
        if (m_OnBecameInvisible != null)
        {
            m_OnBecameInvisible();
        }
    }

    public Action m_OnBecameVisible = null;
    void OnBecameVisible()
    {
        if (m_OnBecameVisible != null)
        {
            m_OnBecameVisible();
        }
    }

    public Action m_OnWillRenderObject = null;
    void OnWillRenderObject()
    {
        if (m_OnWillRenderObject != null)
        {
            m_OnWillRenderObject();
        }
    }

    public Action m_OnPreCull = null;
    void OnPreCull()
    {
        if (m_OnPreCull != null)
        {
            m_OnPreCull();
        }
    }

    public Action m_OnPreRender = null;
    void OnPreRender()
    {
        if (m_OnPreRender != null)
        {
            m_OnPreRender();
        }
    }

    public Action m_OnPostRender = null;
    public void OnPostRender()
    {
        if (m_OnPostRender != null)
        {
            m_OnPostRender();
        }
    }

    public Action<RenderTexture, RenderTexture> m_OnRenderImage = null;
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (m_OnRenderImage != null)
        {
            m_OnRenderImage(src, dest);
        }
    }

    public Action m_OnRenderObject = null;
    void OnRenderObject()
    {
        if (m_OnRenderObject != null)
        {
            m_OnRenderObject();
        }
    }

    #endregion

    static public MonoBehaviourDriver Get(GameObject go)
    {
        MonoBehaviourDriver driver = go.GetComponent<MonoBehaviourDriver>();
        if (driver == null) driver = go.AddComponent<MonoBehaviourDriver>();
        return driver;
    }

    static public MonoBehaviourDriver NewInstance()
    {
        GameObject go = new GameObject();
        MonoBehaviourDriver driver = go.AddComponent<MonoBehaviourDriver>();
        return driver;
    }
}
