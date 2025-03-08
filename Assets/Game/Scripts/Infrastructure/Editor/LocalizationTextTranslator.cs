using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace Infrastructure {

    public static class LocalizationTextTranslator {

        [MenuItem("Tools/Translate All LocalizationText")]
        public static void TranslateAllLocalizationText() {
            // Перебираем все сцены в проекте
            /*foreach (var scenePath in AssetDatabase.FindAssets("t:Scene").Select(AssetDatabase.GUIDToAssetPath)) {
                EditorSceneManager.OpenScene(scenePath);
                TranslateLocalizationTextInScene();
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            }*/

            // Перебираем все ассеты в проекте
            foreach (var prefabPath in AssetDatabase.FindAssets("t:Prefab").Select(AssetDatabase.GUIDToAssetPath)) {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab != null && TranslateLocalizationTextInObject(prefab)) {
                    EditorUtility.SetDirty(prefab);
                }
            }

            // Перебираем все ScriptableObject в проекте
            foreach (var scriptableObjectPath in AssetDatabase.FindAssets("t:ScriptableObject").Select(AssetDatabase.GUIDToAssetPath)) {
                var scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(scriptableObjectPath);
                if (scriptableObject != null && TranslateLocalizationTextInScriptableObject(scriptableObject)) {
                    EditorUtility.SetDirty(scriptableObject);
                }
            }

            // Сохраняем изменения
            AssetDatabase.SaveAssets();
            Debug.Log("Перевод всех LocalizationText завершён.");
        }

        private static void TranslateLocalizationTextInScene() {
            var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var rootObject in rootObjects) {
                TranslateLocalizationTextInObject(rootObject);
            }
        }

        private static bool TranslateLocalizationTextInObject(GameObject obj) {
            bool hasChanges = false;
            var components = obj.GetComponentsInChildren<MonoBehaviour>(true);
            var visitedObjects = new HashSet<object>();

            foreach (var component in components) {
                hasChanges |= TranslateLocalizationTextInObjectFields(component, visitedObjects);
            }

            return hasChanges;
        }

        private static bool TranslateLocalizationTextInScriptableObject(ScriptableObject scriptableObject) {
            bool hasChanges = false;
            var visitedObjects = new HashSet<object>();
            hasChanges |= TranslateLocalizationTextInObjectFields(scriptableObject, visitedObjects);
            return hasChanges;
        }

        private static bool TranslateLocalizationTextInObjectFields(object obj, HashSet<object> visitedObjects) {
            if (obj == null || visitedObjects.Contains(obj)) return false;

            visitedObjects.Add(obj);
            bool hasChanges = false;

            var fields = obj.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields) {
                if (field.FieldType == typeof(LocalizationText)) {
                    var localizationText = field.GetValue(obj) as LocalizationText;
                    if (localizationText != null) {
                        localizationText.Translate();
                        hasChanges = true;
                    }
                } else if (field.FieldType.IsArray && ContainsLocalizationText(field.FieldType.GetElementType())) {
                    var array = field.GetValue(obj) as IEnumerable;
                    if (array != null) {
                        foreach (var item in array) {
                            hasChanges |= TranslateLocalizationTextInObjectFields(item, visitedObjects);
                        }
                    }
                } else if (ContainsLocalizationText(field.FieldType)) {
                    var nestedObject = field.GetValue(obj);
                    if (nestedObject != null) {
                        hasChanges |= TranslateLocalizationTextInObjectFields(nestedObject, visitedObjects);
                    }
                }
            }

            return hasChanges;
        }

        private static bool ContainsLocalizationText(System.Type type) {
            if (type == null || type == typeof(object)) return false;

            var visitedTypes = new HashSet<System.Type>();
            return ContainsLocalizationTextRecursive(type, visitedTypes);
        }

        private static bool ContainsLocalizationTextRecursive(System.Type type, HashSet<System.Type> visitedTypes) {
            if (type == null || type == typeof(object) || visitedTypes.Contains(type)) return false;

            visitedTypes.Add(type);

            if (type == typeof(LocalizationText)) return true;

            return type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                       .Any(field => ContainsLocalizationTextRecursive(field.FieldType, visitedTypes));
        }
    }
}