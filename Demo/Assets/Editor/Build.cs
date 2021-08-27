// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using UnityEditor;
using System.Linq;
using UnityEditor.Build.Reporting;
using System.IO;
using UnityEngine;
using System;

namespace Microsoft.Azure.SpatialAnchors.Unity
{
    public static class Build
    {
        static readonly string ProjectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        private static readonly string iosExportPath =
        Path.GetFullPath(Path.Combine(ProjectPath, "../RNView/ios/UnityExport"));

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
    }
}