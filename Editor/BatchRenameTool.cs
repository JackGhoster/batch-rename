using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


namespace Tools.BatchRename {
    public class BatchRenameTool : EditorWindow  {
        public Object[] ObjectsToRename { get; private set; }
        public string Name { get; private set; } = string.Empty;      

        [MenuItem("HuanguolTools/BatchRename")]
        public static void ShowWindow() {
            var window = GetWindow(typeof(BatchRenameTool));
            window.minSize = new Vector2(300, 100);
            window.maxSize = new Vector2(300, 100);
        }

        private void OnGUI() {
            GUILayout.Label("Rename multiple selected objects", EditorStyles.boldLabel);
            Name = EditorGUILayout.TextField("Enter New Names", Name);           

            if (GUILayout.Button("Rename")) {
                RenameObjects();
            }

        }

        private void RenameObjects() {
            ObjectsToRename = Selection.objects;
            List<GameObject> sceneObjects = new();
            List<Object> assetObjects = new();

            if (ObjectsToRename.Length == 0) return;

            //populate the separate lists for scene gameobjects and assets 
            for (int i = 0; i < ObjectsToRename.Length; i++) {
                GameObject sceneObject = ObjectsToRename[i] as GameObject;
                if(sceneObject != null && sceneObject.activeInHierarchy) sceneObjects.Add(sceneObject);         
                else assetObjects.Add(ObjectsToRename[i]);
            }

            //rename scene gameobjects
            for (int i = 0; i < sceneObjects.Count; i++) {
                GameObject objectToRename = sceneObjects[i];
                string newName = sceneObjects.Count < 2 ? Name : Name + i;
                objectToRename.name = newName;
            }

            //rename assets
            for (int i = 0; i < assetObjects.Count; i++) {
                Object objectToRename = assetObjects[i];
                string newName = assetObjects.Count < 2 ? Name : Name + i;
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(objectToRename), newName);
            }

        }
    }
}