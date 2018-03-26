using UnityEngine;  
using System.Collections;  
using UnityEngine.UI;  
  
public class LocalizationUI : MonoBehaviour  
{  
  
    public int ID;  
    private Text thisText;
  
    void Start()  
    {
        thisText = GetComponent<Text>();  
        thisText.text = LanguageConfig.GetText(ID);
    }
}