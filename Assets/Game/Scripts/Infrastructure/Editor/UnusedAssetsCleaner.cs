#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;


namespace Infrastructure {
    public class UnusedAssetsCleaner : EditorWindow {

        [MenuItem("Tools/Clean Unused Assets")]
        public static void ShowWindow() {
            GetWindow<UnusedAssetsCleaner>("Unused Assets Cleaner");
        }

        private void OnGUI() {
            GUILayout.Label("�������� �������������� �������", EditorStyles.boldLabel);

            if (GUILayout.Button("����� � ������� �������������� ������")) {
                CleanUnusedAssets();
            }
        }

        private static void CleanUnusedAssets() {
            // �������� ���� ���� ����, ����������� � Build Settings
            string[] scenePaths = GetScenesInBuild();
            if (scenePaths.Length == 0) {
                Debug.LogWarning("��� ���� � Build Settings. �������� ���� �� ���� ����� ����� ��������.");
                return;
            }

            // ������ ���� ������� � �������
            string[] allAssets = AssetDatabase.GetAllAssetPaths();
            HashSet<string> usedAssets = new HashSet<string>();

            // ������� ��� ������, ������������ �������
            foreach (string scene in scenePaths) {
                string[] dependencies = AssetDatabase.GetDependencies(scene, true);
                foreach (string dependency in dependencies) {
                    usedAssets.Add(dependency);
                }
            }

            // ���������� ��� ������ � ���������������
            List<string> unusedAssets = new List<string>();
            foreach (string asset in allAssets) {
                if (!usedAssets.Contains(asset) &&
                    asset.StartsWith("Assets/") && // ��������� ������ ������ � ����� �������
                    !asset.EndsWith(".cs") &&     // ���������� �������
                    !Directory.Exists(asset))    // ���������� �����
                {
                    unusedAssets.Add(asset);
                }
            }

            // ������� �������������� ������
            foreach (string unusedAsset in unusedAssets) {
                Debug.Log($"��������: {unusedAsset}");
                AssetDatabase.DeleteAsset(unusedAsset);
            }

            // ��������� ������
            AssetDatabase.Refresh();

            Debug.Log($"�������� ���������. ������� {unusedAssets.Count} �������.");
        }

        private static string[] GetScenesInBuild() {
            List<string> scenePaths = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
                if (scene.enabled) {
                    scenePaths.Add(scene.path);
                }
            }
            return scenePaths.ToArray();
        }
    }
}
#endif