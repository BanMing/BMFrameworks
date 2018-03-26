/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/11/08
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using System.Collections;

public class MeshTool
{
    static public void UpdateMahjongTexture(GameObject target, string assetBundleName, string spriteName)
    {
        MeshFilter meshFilter = target.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            return;
        }

        System.Action<Texture> loadAction = delegate (Texture tex)
        {
            Rect rect = UIAtlasTool.Instance.GetTextureRect(assetBundleName, spriteName);

            float uMin = rect.xMin /(float)tex.width;
            float uMax = rect.xMax / (float)tex.width;

            float vMin = rect.yMin / (float)tex.height;
            float vMax = rect.yMax / (float)tex.height;

            var texCoords = new Vector2[4];
            texCoords[0] = new Vector2(uMin, vMin);
            texCoords[1] = new Vector2(uMax, vMin);
            texCoords[2] = new Vector2(uMax, vMax);
            texCoords[3] = new Vector2(uMin, vMax);

            meshFilter.mesh.uv = texCoords;
        };

        UIAtlasTool.Instance.GetTexture(assetBundleName, loadAction);
    }
}
