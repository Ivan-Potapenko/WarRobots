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

        // ��������� ��� �����������
        private static float reductionPercentage = 50f; // ������� ���������� ���������
        private static int polygonThreshold = 1000; // ����� ��� ���������� ���������

        // ������� ��� �������� ���������������� �����
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

        // ������� ����� ��� ����������� ���� ����� � �������
        private static void OptimizeMeshesInProject() {
            // ������� ����� ��� �������� ���������������� �����
            if (!Directory.Exists(optimizedMeshesFolder)) {
                Directory.CreateDirectory(optimizedMeshesFolder);
            }

            // �������� ��� ����� � �������, ������� .fbx � .obj
            string[] assetPaths = AssetDatabase.GetAllAssetPaths();

            foreach (var assetPath in assetPaths) {
                if (assetPath.EndsWith(".fbx") || assetPath.EndsWith(".obj")) {
                    // ��������� ������ �� �����
                    var importedObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                    if (importedObject == null) continue;

                    // ������� ��� MeshFilter ���������� � �������
                    var meshFilters = importedObject.GetComponentsInChildren<MeshFilter>();
                    foreach (var meshFilter in meshFilters) {
                        var originalMesh = meshFilter.sharedMesh;
                        if (originalMesh != null && originalMesh.vertexCount > polygonThreshold) // ���������� ��������� ����
                        {
                            if (!meshesToOptimize.ContainsKey(originalMesh)) {
                                // ������������ ���, ���� ��� ��� �� ��������������
                                Mesh optimizedMesh = SimplifyMesh(originalMesh);
                                meshesToOptimize[originalMesh] = optimizedMesh;

                                // ��������� ���������������� ��� � �����
                                string optimizedAssetPath = $"{optimizedMeshesFolder}/{originalMesh.name}_Optimized.asset";
                                AssetDatabase.CreateAsset(optimizedMesh, optimizedAssetPath);
                                AssetDatabase.SaveAssets();
                                Debug.Log($"Optimized mesh saved: {optimizedAssetPath}");
                            }

                            // �������� ������ �� ���������������� ��� � MeshFilter
                            meshFilter.sharedMesh = meshesToOptimize[originalMesh];
                        }
                    }

                    // ����� �������� ������ �� ���������������� ���� � ������ �����������, ��������, SkinnedMeshRenderer
                    var skinnedMeshRenderers = importedObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                    foreach (var skinnedMeshRenderer in skinnedMeshRenderers) {
                        var originalMesh = skinnedMeshRenderer.sharedMesh;
                        if (originalMesh != null && originalMesh.vertexCount > polygonThreshold) {
                            if (!meshesToOptimize.ContainsKey(originalMesh)) {
                                // ������������ ���, ���� ��� ��� �� ��������������
                                Mesh optimizedMesh = SimplifyMesh(originalMesh);
                                meshesToOptimize[originalMesh] = optimizedMesh;

                                // ��������� ���������������� ��� � �����
                                string optimizedAssetPath = $"{optimizedMeshesFolder}/{originalMesh.name}_Optimized.asset";
                                AssetDatabase.CreateAsset(optimizedMesh, optimizedAssetPath);
                                AssetDatabase.SaveAssets();
                                Debug.Log($"Optimized mesh saved: {optimizedAssetPath}");
                            }

                            // �������� ������ �� ���������������� ��� � SkinnedMeshRenderer
                            skinnedMeshRenderer.sharedMesh = meshesToOptimize[originalMesh];
                        }
                    }
                }
            }

            // ��������� ������, ����� ��������� ������������
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // ��������� ��������� � ��������
            UpdatePrefabLinks();

            // ��������� �����
            UpdateSceneLinks();

            Debug.Log("Mesh optimization complete!");
        }

        // ����� ��� ��������� ���� � �������������� UnityMeshSimplifier
        private static Mesh SimplifyMesh(Mesh originalMesh) {
            // ������� ������ MeshSimplifier
            var simplifier = new MeshSimplifier();

            // ��������� ������������ ��� � MeshSimplifier
            simplifier.Initialize(originalMesh);

            // ������������� ������� ���������� ���������
            float reductionFactor = 1 - (reductionPercentage / 100f);

            // ��������� ��������� ����
            simplifier.SimplifyMesh(reductionFactor);

            // �������� ����� ���������� ���
            Mesh simplifiedMesh = simplifier.ToMesh();

            // ��������� ��� ������������� ���� � ��������� "_Optimized"
            simplifiedMesh.name = originalMesh.name + "_Optimized";

            return simplifiedMesh;
        }

        // ���������� ������ �� ���� � ��������
        private static void UpdatePrefabLinks() {
            string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab");

            foreach (var prefabPath in prefabPaths) {
                string assetPath = AssetDatabase.GUIDToAssetPath(prefabPath);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (prefab != null) {
                    // �������� ��� �������
                    PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(prefab);

                    // ���� ��� �� ������� �������, �� ����� ��� �������������
                    if (prefabType != PrefabAssetType.NotAPrefab) {
                        // ������� ��� MeshFilter ���������� � �������
                        var meshFilters = prefab.GetComponentsInChildren<MeshFilter>(true);
                        foreach (var meshFilter in meshFilters) {
                            var originalMesh = meshFilter.sharedMesh;
                            if (originalMesh != null && meshesToOptimize.ContainsKey(originalMesh)) {
                                meshFilter.sharedMesh = meshesToOptimize[originalMesh];
                            }
                        }

                        // ����� �������� ������ �� ���������������� ���� � SkinnedMeshRenderer
                        var skinnedMeshRenderers = prefab.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                        foreach (var skinnedMeshRenderer in skinnedMeshRenderers) {
                            var originalMesh = skinnedMeshRenderer.sharedMesh;
                            if (originalMesh != null && meshesToOptimize.ContainsKey(originalMesh)) {
                                skinnedMeshRenderer.sharedMesh = meshesToOptimize[originalMesh];
                            }
                        }

                        // ��������� ��������� � ������
                        try {
                            PrefabUtility.SavePrefabAsset(prefab);
                        }
                        catch { }
                    } else {
                        // ���� ��� ������� �������, ��������� ��������� � ���� � ����� ��������� ������
                        PrefabUtility.ApplyPrefabInstance(prefab, InteractionMode.AutomatedAction);
                    }
                }
            }
        }

        // ���������� ������ �� ���� � ������
        private static void UpdateSceneLinks() {
            // �������� �� ���� ����������� ������
            for (int i = 0; i < EditorSceneManager.sceneCount; i++) {
                var scene = EditorSceneManager.GetSceneAt(i);

                // �������� �� �������� �������� �����
                var rootObjects = scene.GetRootGameObjects();

                foreach (var rootObject in rootObjects) {
                    // ��������� MeshFilter ����������
                    var meshFilters = rootObject.GetComponentsInChildren<MeshFilter>(true);
                    foreach (var meshFilter in meshFilters) {
                        var originalMesh = meshFilter.sharedMesh;
                        if (originalMesh != null && meshesToOptimize.ContainsKey(originalMesh)) {
                            // �������� ������ �� ���������������� ���
                            meshFilter.sharedMesh = meshesToOptimize[originalMesh];
                        }
                    }

                    // ��������� SkinnedMeshRenderer ����������
                    var skinnedMeshRenderers = rootObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                    foreach (var skinnedMeshRenderer in skinnedMeshRenderers) {
                        var originalMesh = skinnedMeshRenderer.sharedMesh;
                        if (originalMesh != null && meshesToOptimize.ContainsKey(originalMesh)) {
                            // �������� ������ �� ���������������� ���
                            skinnedMeshRenderer.sharedMesh = meshesToOptimize[originalMesh];
                        }
                    }
                }
            }

            // ��������� �����
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