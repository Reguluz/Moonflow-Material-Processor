using UnityEditor;
using UnityEngine;

namespace Moonflow.MFAssetTools.MFMatProcessor
{
    public class MFMatTextureCon : MFMatBoolCon
    {
        public bool any;
        public string name;
        public Texture2D texture;
        public override string condName => "Texture";

        private string textureGUID;
        public override bool Check(Material mat)
        {
            //get all texture properties on the material
            int count = ShaderUtil.GetPropertyCount(mat.shader);
            for (int i = 0; i < count; i++)
            {
                if (ShaderUtil.GetPropertyType(mat.shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    string propertyName = ShaderUtil.GetPropertyName(mat.shader, i);
                    if (any)
                    {
                        if (mat.GetTexture(propertyName) == texture)
                        {
                            return equal;
                        }
                    }
                    else
                    {
                        if (propertyName == name)
                        {
                            return mat.GetTexture(propertyName) == texture;
                        }
                    }
                }
            }
            return false;
        }
        public bool Check(string[] texfromMatGuid)
        {
            if (any)
            {
                foreach (string s in texfromMatGuid)
                {
                    if (s == textureGUID)
                    {
                        return equal;
                    }
                }
            }
            else
            {
                foreach (string s in texfromMatGuid)
                {
                    if (s == textureGUID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void DrawLeft(float width)
        {
            any = EditorGUILayout.ToggleLeft("Any", any, GUILayout.Width(any ? width : 40));
            if (!any)
            {
                name = EditorGUILayout.TextField(name, GUILayout.Width(width - 43));
            }
        }
        
        public override void DrawRight(float width)
        {
            texture = (Texture2D) EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(width));
        }

        public override void UpdateRef()
        {
            base.UpdateRef();
            if(texture!=null)
                //get the GUID of the texture
                textureGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(texture));
        }
    }
}