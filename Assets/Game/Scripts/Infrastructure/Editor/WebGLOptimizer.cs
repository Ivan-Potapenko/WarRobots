using UnityEditor;
using UnityEngine;

public class WebGLOptimizer : EditorWindow
{
    [MenuItem("Tools/WebGL Optimizer")]
    public static void OptimizeForWebGL() {
        // Optimize Quality Settings
        for (int i = 0; i < QualitySettings.names.Length; i++) {
            QualitySettings.SetQualityLevel(i, false);
            QualitySettings.shadowDistance = 20f; // ��������� ��������� �����
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            QualitySettings.realtimeReflectionProbes = false;
            QualitySettings.lodBias = 1.5f; // ����������� LOD-�����
        }
        Debug.Log("Quality settings optimized.");

        // Optimize Graphics Settings
        PlayerSettings.colorSpace = ColorSpace.Gamma;
        PlayerSettings.graphicsJobs = true; // �������� Graphics Jobs
        PlayerSettings.MTRendering = true; // �������������� ���������

        Debug.Log("Graphics settings optimized.");

        // Optimize Build Settings for WebGL
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip; // �������� GZIP-������
        PlayerSettings.WebGL.memorySize = 256; // ����������� �������� ������ � MB
        PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm; // ���������� WebAssembly
        PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.None; // ��������� ����������

        Debug.Log("Build settings optimized for WebGL.");

        // Optimize textures and materials
        OptimizeTextures();

        Debug.Log("All optimizations applied successfully.");
    }

    private static void OptimizeTextures() {
        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        foreach (string assetPath in allAssets) {
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if(textureImporter != null && textureImporter.maxTextureSize == 2048 && !assetPath.Contains("Packages")) {
                Debug.Log(assetPath);
            }
            /*if (textureImporter != null) {
                textureImporter.textureCompression = TextureImporterCompression.Compressed;
                textureImporter.crunchedCompression = true;
                textureImporter.compressionQuality = 20; // ������ ����� ��������� � ��������
                textureImporter.mipmapEnabled = false; // ��������� MipMap
                AssetDatabase.ImportAsset(assetPath);
            }*/
        }
        Debug.Log("Textures optimized.");
    }

    [MenuItem("Tools/Optimize Audio Clips for Web")]
    public static void OptimizeAudioClips() {
        string[] guids = AssetDatabase.FindAssets("t:AudioClip");
        int totalClips = guids.Length;

        if (totalClips == 0) {
            Debug.Log("No AudioClips found in the project.");
            return;
        }

        for (int i = 0; i < totalClips; i++) {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            AudioImporter importer = AssetImporter.GetAtPath(path) as AudioImporter;

            if (importer == null)
                continue;

            // ��������� �������
            importer.forceToMono = true; // �������������� � ���� ��� �������� ������
            importer.loadInBackground = true; // ��������� ����� � ����

            AudioImporterSampleSettings settings = importer.defaultSampleSettings;
            settings.loadType = AudioClipLoadType.CompressedInMemory; // ������ � ������
            settings.compressionFormat = AudioCompressionFormat.Vorbis; // ������ Vorbis
            settings.quality = 0.1f; // �������� (����������� ��������)

            importer.defaultSampleSettings = settings;

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            Debug.Log($"Optimized: {path} ({i + 1}/{totalClips})");
        }

        Debug.Log("Audio optimization complete.");
    }

    [MenuItem("Tools/Mesh Optimizer/Optimize All Meshes")]
    public static void OptimizeAllMeshes() {
        string[] allMeshPaths = AssetDatabase.FindAssets("t:Model"); // ������� ��� ������
        int count = allMeshPaths.Length;

        for (int i = 0; i < count; i++) {
            string assetPath = AssetDatabase.GUIDToAssetPath(allMeshPaths[i]);
            ModelImporter modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;

            if (modelImporter != null) {
                // ��������� ���������������� ���������
                modelImporter.importNormals = ModelImporterNormals.Import; // ������ ��������
                modelImporter.importTangents = ModelImporterTangents.None; // ��������� �������� (���� �� ����� PBR)
                modelImporter.importBlendShapes = false; // ��������� Blend Shapes
                modelImporter.importCameras = false; // ��������� ������
                modelImporter.importLights = false; // ��������� ��������� �����
                modelImporter.optimizeMeshPolygons = true; // ����������� ���������
                modelImporter.optimizeMeshVertices = true; // ����������� ������
                modelImporter.weldVertices = true; // ����������� ������
                modelImporter.meshCompression = ModelImporterMeshCompression.Medium; // ���������� �����
                modelImporter.materialImportMode = ModelImporterMaterialImportMode.None; // ��������� ���������

                AssetDatabase.WriteImportSettingsIfDirty(assetPath);
            }

            EditorUtility.DisplayProgressBar("Optimizing Meshes", $"Processing {i + 1}/{count}", (float)(i + 1) / count);
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
        Debug.Log("Mesh optimization completed!");
    }
}
