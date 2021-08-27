// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using UnityEditor;
using System.Linq;
using UnityEditor.Build.Reporting;
using System.IO;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

namespace Microsoft.Azure.SpatialAnchors.Unity
{
    public static class Build
    {
        static readonly string ProjectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        private static readonly string androidExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../RNView/android/UnityExport"));
        private static readonly string iosExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../RNView/ios/UnityExport"));

        [MenuItem("ReactNative/Export Android %&n", false, 1)]
        public static void DoBuildAndroidLibrary()
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.Generic;
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
            EditorUserBuildSettings.androidETC2Fallback = AndroidETC2Fallback.Quality32Bit;

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
            {
                locationPathName = androidExportPath,
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
                options = BuildOptions.None,
                scenes = GetEnabledScenes(),
            };

            // checks for exceptions and returns proper error code
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            if (summary.result != BuildResult.Succeeded)
            {
                throw new Exception("Build failed");
            }
            else {
                Debug.Log("Build Succeed");
            }

        }

        [MenuItem("ReactNative/Export IOS %&i", false, 3)]
        public static void DoBuildIOS()
        {
            if (Directory.Exists(iosExportPath))
            {
                Directory.Delete(iosExportPath, true);
            }

            EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
            EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;

            var options = BuildOptions.None;
            var report = BuildPipeline.BuildPlayer(
                GetEnabledScenes(),
                iosExportPath,
                BuildTarget.iOS,
                options
            );

            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new Exception("Build failed");
            }
        }

        static string[] GetEnabledScenes()
        {
            var scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            return scenes;
        }

        static void Copy(string source, string destinationPath)
        {
            if (Directory.Exists(destinationPath))
                Directory.Delete(destinationPath, true);

            Directory.CreateDirectory(destinationPath);

            foreach (string dirPath in Directory.GetDirectories(source, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(source, destinationPath));

            foreach (string newPath in Directory.GetFiles(source, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(source, destinationPath), true);
        }
    }
}