#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
//using UnityMeshSimplifier;
using System.IO;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

namespace Infrastructure {

    public class MeshReducerForAssets : EditorWindow {
        /*private static string optimizedMeshesFolder = "Assets/OptimizedMeshes";

        // Настройки для оптимизации
        private static float reductionPercentage = 50f; // Процент уменьшения полигонов
        private static int polygonThreshold = 1000; // Порог для уменьшения полигонов

        // Словарь для хранения оптимизированных мешей
        private static Dictionary<Mesh, Mesh> meshesToOptimize = new Dictionary<Mesh, Mesh>();

        [MenuItem("Tools/Mesh Optimizer")]
        public static void ShowWindow() {
            GetWindow<MeshReducerForAssets>("Mesh Optimizer");
        }

        private void OnGUI() {
            GUILayout.Label("Mesh Optimization", EditorStyles.boldLabel);

            reductionPercentage = EditorGUILayout.FloatField("Polygon Reduction (%)", reductionPercentage);
            polygonThreshold = EditorGUILayout.IntField("Polygon Threshold", polygonThreshold);

            if (GUILayout.Button("Optimize All Meshes")) {
                OptimizeMeshesInProject();
            }
        }

        // Главный метод для оптимизации всех мешей в проекте
        private static void OptimizeMeshesInProject() {
            // Создаем папку для хранения оптимизированных мешей
            if (!Directory.Exists(optimizedMeshesFolder)) {
                Directory.CreateDirectory(optimizedMeshesFolder);
            }

            // Получаем все файлы в проекте, включая .fbx и .obj
            string[] assetPaths = AssetDatabase.GetAllAssetPaths();

            foreach (var assetPath in assetPaths) {
                if (assetPath.EndsWith(".fbx") || assetPath.EndsWith(".obj")) {
                    // Загружаем объект из файла
                    var importedObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                    if (importedObject == null) continue;

                    // Находим все MeshFilter компоненты в объекте
                    var meshFilters = importedObject.GetComponentsInChildren<MeshFilter>();
                    foreach (var meshFilter in meshFilters) {
                        var originalMesh = meshFilter.sharedMesh;
                        if (originalMesh != null && originalMesh.vertexCount > polygonThreshold) // Игнорируем маленькие меши
                        {
                            if (!meshesToOptimize.ContainsKey(originalMesh)) {
                                // Оптимизируем меш, если его еще не оптимизировали
                                Mesh optimizedMesh = SimplifyMesh(originalMesh);
                                meshesToOptimize[originalMesh] = optimizedMesh;

                                // Сохраняем оптимизированный меш в папке
                                string optimizedAssetPath = $"{optimizedMeshesFolder}/{originalMesh.name}_Optimized.asset";
                                AssetDatabase.CreateAsset(optimizedMesh, optimizedAssetPath);
                                AssetDatabase.SaveAssets();
                                Debug.Log($"Optimized mesh saved: {optimizedAssetPath}");
                            }

                            // Заменяем ссылку на оптимизированный меш в MeshFilter
                            meshFilter.sharedMesh = meshesToOptimize[originalMesh];
                        }
                    }

                    // Также заменяем ссылки на оптимизированные меши в других компонентах, например, SkinnedMeshRenderer
                    var skinnedMeshRenderers = importedObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                    foreach (var skinnedMeshRenderer in skinnedMeshRenderers) {
                        var originalMesh = skinnedMeshRenderer.sharedMesh;
                        if (originalMesh != null && originalMesh.vertexCount > polygonThreshold) {
                            if (!meshesToOptimize.ContainsKey(originalMesh)) {
                                // Оптимизируем меш, если его еще не оптимизировали
                                Mesh optimizedMesh = SimplifyMesh(originalMesh);
                                meshesToOptimize[originalMesh] = optimizedMesh;

                                // Сохраняем оптимизированный меш в папке
                                string optimizedAssetPath = $"{optimizedMeshesFolder}/{originalMesh.name}_Optimized.asset";
                                AssetDatabase.CreateAsset(optimizedMesh, optimizedAssetPath);
                                AssetDatabase.SaveAssets();
                                Debug.Log($"Optimized mesh saved: {optimizedAssetPath}");
                            }

                            // Заменяем ссылку на оптимизированный меш в SkinnedMeshRenderer
                            skinnedMeshRenderer.sharedMesh = meshesToOptimize[originalMesh];
                        }
                    }
                }
            }

            // Обновляем проект, чтобы изменения отобразились
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Применяем изменения к префабам
            UpdatePrefabLinks();

            // Обновляем сцены
            UpdateSceneLinks();

            Debug.Log("Mesh optimization complete!");
        }

        // Метод для упрощения меша с использованием UnityMeshSimplifier
        private static Mesh SimplifyMesh(Mesh originalMesh) {
            // Создаем объект MeshSimplifier
            var simplifier = new MeshSimplifier();

            // Загружаем оригинальный меш в MeshSimplifier
            simplifier.Initialize(originalMesh);

            // Устанавливаем процент уменьшения полигонов
            float reductionFactor = 1 - (reductionPercentage / 100f);

            // Выполняем упрощение меша
            simplifier.SimplifyMesh(reductionFactor);

            // Получаем новый упрощенный меш
            Mesh simplifiedMesh = simplifier.ToMesh();

            // Сохраняем имя оригинального меша с суффиксом "_Optimized"
            simplifiedMesh.name = originalMesh.name + "_Optimized";

            return simplifiedMesh;
        }

        // Обновление ссылок на меши в префабах
        private static void UpdatePrefabLinks() {
            string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab");

            foreach (var prefabPath in prefabPaths) {
                string assetPath = AssetDatabase.GUIDToAssetPath(prefabPath);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (prefab != null) {
                    // Получаем тип префаба
                    PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(prefab);

                    // Если это не инстанс префаба, то можем его редактировать
                    if (prefabType != PrefabAssetType.NotAPrefab) {
                        // Находим все MeshFilter компоненты в префабе
                        var meshFilters = prefab.GetComponentsInChildren<MeshFilter>(true);
                        foreach (var meshFilter in meshFilters) {
                            var originalMesh = meshFilter.sharedMesh;
                            if (originalMesh != null && meshesToOptimize.ContainsKey(originalMesh)) {
                                meshFilter.sharedMesh = meshesToOptimize[originalMesh];
                            }
                        }

                        // Также заменяем ссылки на оптимизированные меши в SkinnedMeshRenderer
                        var skinnedMeshRenderers = prefab.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                        foreach (var skinnedMeshRenderer in skinnedMeshRenderers) {
                            var originalMesh = skinnedMeshRenderer.sharedMesh;
                            if (originalMesh != null && meshesToOptimize.ContainsKey(originalMesh)) {
                                skinnedMeshRenderer.sharedMesh = meshesToOptimize[originalMesh];
                            }
                        }

                        // Применяем изменения в префаб
                        try {
                            PrefabUtility.SavePrefabAsset(prefab);
                        }
                        catch { }
                    } else {
                        // Если это инстанс префаба, применяем изменения к нему и затем обновляем префаб
                        PrefabUtility.ApplyPrefabInstance(prefab, InteractionMode.AutomatedAction);
                    }
                }
            }
        }

        // Обновление ссылок на меши в сценах
        private static void UpdateSceneLinks() {
            // Проходим по всем загруженным сценам
            for (int i = 0; i < EditorSceneManager.sceneCount; i++) {
                var scene = EditorSceneManager.GetSceneAt(i);

                // Проходим по корневым объектам сцены
                var rootObjects = scene.GetRootGameObjects();

                foreach (var rootObject in rootObjects) {
                    // Обновляем MeshFilter компоненты
                    var meshFilters = rootObject.GetComponentsInChildren<MeshFilter>(true);
                    foreach (var meshFilter in meshFilters) {
                        var originalMesh = meshFilter.sharedMesh;
                        if (originalMesh != null && meshesToOptimize.ContainsKey(originalMesh)) {
                            // Заменяем ссылку на оптимизированный меш
                            meshFilter.sharedMesh = meshesToOptimize[originalMesh];
                        }
                    }

                    // Обновляем SkinnedMeshRenderer компоненты
                    var skinnedMeshRenderers = rootObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                    foreach (var skinnedMeshRenderer in skinnedMeshRenderers) {
                        var originalMesh = skinnedMeshRenderer.sharedMesh;
                        if (originalMesh != null && meshesToOptimize.ContainsKey(originalMesh)) {
                            // Заменяем ссылку на оптимизированный меш
                            skinnedMeshRenderer.sharedMesh = meshesToOptimize[originalMesh];
                        }
                    }
                }
            }

            // Сохраняем сцены
            for (int i = 0; i < EditorSceneManager.sceneCount; i++) {
                var scene = EditorSceneManager.GetSceneAt(i);
                if (scene.isDirty) {
                    EditorSceneManager.SaveScene(scene);
                }
            }
        }*/
    }
}
#endif