using UnityEngine;
using UnityEditor;
using System.Text;
using System;

namespace XP.TableModel
{
    public class UnitEditorWindow : EditorWindow {
  
        private bool _hide = false; 
 
        public static void Open() { 
            UnitEditorWindow _window =  CreateInstance(typeof(UnitEditorWindow)) as UnitEditorWindow;
          
            _window.ShowModalUtility(); 
        }

     
        private void OnEnable()
        {
            titleContent = new GUIContent(DateTime.Now.Ticks.ToString());
            this.position = new Rect(500,400,600,300);
        }
        private void OnDisable()
        {
            if (_hide)
            {
                PlayerPrefs.SetInt(_key,2);
            }
        }
       
        private void OnGUI()
        { 
            GUILayout.BeginHorizontal();
            GUILayout.Label(_ToString(_key), new GUIStyle { 
                 fontSize=50,
                alignment = TextAnchor.MiddleCenter,
                normal = new GUIStyleState()
                {
                    textColor = new Color(0,1,1,1)
                }
            });
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            GUI.contentColor = new Color(1, 0.92f, 0.016f, 1);
            GUILayout.TextArea(_ToString(_value), new GUIStyle
            {
                fontSize = 20,
                alignment = TextAnchor.MiddleCenter,
                normal=new GUIStyleState() { 
                 textColor=Color.yellow
                }
            });
            GUI.contentColor = new Color(1,1,1,0.3f);
            _hide = GUILayout.Toggle(_hide,_ToString(_value2));
            
        }

        public const string _key
       = "VW5pdHnooajmoLznu4Tku7Y=";


        private const string _value
            = @"5qih5Z2X6LSf6LSj5Lq677ya5aSP6bmPCuWmguaenOaCqOWcqOS9v+eUqOi/h+eoi+S4reWPkeeOsOmXrumimArlj6/ku6XnlKjku6XkuIvmlrnlvI/ogZTns7vmiJHvvJoKUVHvvJo2NDgzODA0MjEK6YKu566x77yaNjQ4MzgwNDIxQHFxLmNvbQrlvq7kv6HvvJpCRDQ3NTc=";
        
        
        private const string _value2
            = "5LiN5YaN5o+Q56S6";

        private string _ToString(string str)
        {
            var bytes = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(bytes);
        }

     
    }
}