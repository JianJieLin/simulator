/**
 * Copyright (c) 2019 LG Electronics, Inc.
 *
 * This software contains code licensed as described in LICENSE.
 *
 */

namespace Simulator.Editor.PointCloud
{
    using UnityEditor;
    using UnityEngine;
    using Simulator.PointCloud;
    
    [CustomEditor(typeof(PointCloudRenderer))]
    public class PointCloudRendererEditor : Editor
    {
        private static class Styles
        {
            // public static readonly GUIContent PartialPointLightingContent = new GUIContent("Partial Point Lighting",
            //     "When enabled, point rendering will use deferred lighting. Result is not entirely correct, due to lack of normals data.");
            
            public static readonly GUIContent CascadeShowPreviewContent =
                new GUIContent("Show Preview", "Visible only in play mode.");

            public static readonly GUIContent CascadeOffsetContent = new GUIContent("Offset");
            public static readonly GUIContent CascadeSizeContent = new GUIContent("Size");

            public static readonly GUIContent FovReprojectionContent =
                new GUIContent("FOV Reprojection", "Render broader FOV to hide artifacts near screen edges.\nVisible only in play mode.");

            // public static readonly GUIContent TemporalSmoothingContent = new GUIContent("Temporal Smoothing",
            //     "Use data from previous frames to reduce flickering.");
            
            public static readonly GUIContent TemporalSmoothingContent = new GUIContent("Temporal Smoothing",
                "Temporarily disabled (not compatible with HDRP).");
            
            public static readonly GUIContent InterpolatedFramesContent = new GUIContent("Interpolated Frames",
                "Describes how long data from frame will be used for temporal smoothing.");

            public static readonly GUIContent DebugUseLinearDepthContent = new GUIContent(
                "Linear depth", "If true, solid rendering will internally use linear depth for better interpolation.");

            public static readonly GUIContent DebugBlendSkyContent = new GUIContent(
                "Blend sky", "If true, sky will be sampled for background color.");
            
            public static readonly GUIContent DebugForceFillContent = new GUIContent("Force fill",
                "Always fill holes below the horizon line.");

            public static readonly GUIContent DebugFillThresholdContent = new GUIContent(
                "Fill threshold", "Describes how high the horizon for auto-fill is.");
            
            public static readonly GUIContent DebugSolidBlitLevelContent = new GUIContent("Blit Level",
                "Final blit will display downsampled image on specified mip level.");

            public static readonly GUIContent DebugSolidRemoveHiddenContent =
                new GUIContent("Remove Hidden", "Run hidden point removal kernel.");

            public static readonly GUIContent DebugSolidPullPushContent =
                new GUIContent("Pull-Push", "Run pull-push kernels.");

            public static readonly GUIContent DebugSolidFixedLevelContent = new GUIContent("Remove Hidden Level",
                "Value other than 0 will overwrite cascades in hidden point removal kernel and always use specified mip level.");
            
            public static readonly GUIContent DebugSolidPullParamContent = new GUIContent("Pull Exponent",
                "Filter exponent used in pull kernel.");

            public static readonly GUIContent CalculateNormalsContent = new GUIContent("Calculate Normals",
                "When enabled, normal vectors will be calculated. Required for lighting.");
            
            public static readonly GUIContent SmoothNormalsContent = new GUIContent("Smooth Normals",
                "When enabled, normal vectors for point cloud will be smoothed out.");
            
            public static readonly GUIContent ShadowSizeContent = new GUIContent("Shadow Point Size",
                "Multiplier of point size during shadow caster pass.");
            
            public static readonly GUIContent ShadowBiasContent = new GUIContent("Shadow Bias",
                "Scaled depth offset of each point during shadow caster pass.");
        }
        
        private SerializedProperty Colorize;
        private SerializedProperty Render;
        private SerializedProperty Mask;
        private SerializedProperty ConstantSize;
        private SerializedProperty PixelSize;
        private SerializedProperty AbsoluteSize;
        private SerializedProperty MinPixelSize;
        // private SerializedProperty PartialPointLighting;
        private SerializedProperty DebugUseLinearDepth;
        private SerializedProperty DebugForceFill;
        private SerializedProperty DebugBlendSky;
        private SerializedProperty DebugFillThreshold;
        private SerializedProperty DebugSolidBlitLevel;
        private SerializedProperty SolidRemoveHidden;
        private SerializedProperty DebugSolidPullPush;
        private SerializedProperty DebugSolidFixedLevel;
        private SerializedProperty DebugShowRemoveHiddenCascades;
        private SerializedProperty RemoveHiddenCascadeOffset;
        private SerializedProperty RemoveHiddenCascadeSize;
        private SerializedProperty DebugSolidPullParam;
        private SerializedProperty SolidFovReprojection;
        private SerializedProperty ReprojectionRatio;
        private SerializedProperty PreserveTexelSize;
        private SerializedProperty DebugShowSmoothNormalsCascades;
        private SerializedProperty CalculateNormals;
        private SerializedProperty SmoothNormals;
        private SerializedProperty SmoothNormalsCascadeOffset;
        private SerializedProperty SmoothNormalsCascadeSize;
        private SerializedProperty TemporalSmoothing;
        private SerializedProperty InterpolatedFrames;
        private SerializedProperty CastShadows;
        private SerializedProperty ShadowPointSize;
        private SerializedProperty ShadowBias;

        protected virtual void OnEnable()
        {
            FindSharedProperties();
        }

        protected void FindSharedProperties()
        {
            Colorize = serializedObject.FindProperty(nameof(PointCloudRenderer.Colorize));
            Render = serializedObject.FindProperty(nameof(PointCloudRenderer.RenderMode));
            Mask = serializedObject.FindProperty(nameof(PointCloudRenderer.Mask));
            ConstantSize = serializedObject.FindProperty(nameof(PointCloudRenderer.ConstantSize));
            PixelSize = serializedObject.FindProperty(nameof(PointCloudRenderer.PixelSize));
            AbsoluteSize = serializedObject.FindProperty(nameof(PointCloudRenderer.AbsoluteSize));
            MinPixelSize = serializedObject.FindProperty(nameof(PointCloudRenderer.MinPixelSize));
            // PartialPointLighting = serializedObject.FindProperty(nameof(PointCloudRenderer.PartialPointLighting));
            DebugSolidBlitLevel = serializedObject.FindProperty(nameof(PointCloudRenderer.DebugSolidBlitLevel));
            DebugFillThreshold = serializedObject.FindProperty(nameof(PointCloudRenderer.DebugFillThreshold));
            DebugUseLinearDepth = serializedObject.FindProperty(nameof(PointCloudRenderer.DebugUseLinearDepth));
            DebugForceFill = serializedObject.FindProperty(nameof(PointCloudRenderer.DebugForceFill));
            DebugBlendSky = serializedObject.FindProperty(nameof(PointCloudRenderer.DebugBlendSky));
            SolidRemoveHidden = serializedObject.FindProperty(nameof(PointCloudRenderer.SolidRemoveHidden));
            DebugSolidPullPush = serializedObject.FindProperty(nameof(PointCloudRenderer.DebugSolidPullPush));
            DebugSolidFixedLevel = serializedObject.FindProperty(nameof(PointCloudRenderer.DebugSolidFixedLevel));
            DebugShowRemoveHiddenCascades = serializedObject.FindProperty(nameof(PointCloudRenderer.DebugShowRemoveHiddenCascades));
            RemoveHiddenCascadeOffset = serializedObject.FindProperty(nameof(PointCloudRenderer.RemoveHiddenCascadeOffset));
            RemoveHiddenCascadeSize = serializedObject.FindProperty(nameof(PointCloudRenderer.RemoveHiddenCascadeSize));
            DebugSolidPullParam = serializedObject.FindProperty(nameof(PointCloudRenderer.DebugSolidPullParam));
            SolidFovReprojection = serializedObject.FindProperty(nameof(PointCloudRenderer.SolidFovReprojection));
            ReprojectionRatio = serializedObject.FindProperty(nameof(PointCloudRenderer.ReprojectionRatio));
            PreserveTexelSize = serializedObject.FindProperty(nameof(PointCloudRenderer.PreserveTexelSize));
            CalculateNormals = serializedObject.FindProperty(nameof(PointCloudRenderer.CalculateNormals));
            SmoothNormals = serializedObject.FindProperty(nameof(PointCloudRenderer.SmoothNormals));
            DebugShowSmoothNormalsCascades = serializedObject.FindProperty(nameof(PointCloudRenderer.DebugShowSmoothNormalsCascades));
            SmoothNormalsCascadeOffset = serializedObject.FindProperty(nameof(PointCloudRenderer.SmoothNormalsCascadeOffset));
            SmoothNormalsCascadeSize = serializedObject.FindProperty(nameof(PointCloudRenderer.SmoothNormalsCascadeSize));
            TemporalSmoothing = serializedObject.FindProperty(nameof(PointCloudRenderer.TemporalSmoothing));
            InterpolatedFrames = serializedObject.FindProperty(nameof(PointCloudRenderer.InterpolatedFrames));
            ShadowPointSize = serializedObject.FindProperty(nameof(PointCloudRenderer.ShadowPointSize));
            ShadowBias = serializedObject.FindProperty(nameof(PointCloudRenderer.ShadowBias));
        }

        public sealed override void OnInspectorGUI()
        {
            var obj = target as PointCloudRenderer;

            serializedObject.Update();

            DrawInspector(obj);

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawInspector(PointCloudRenderer obj)
        {
            DrawProtectedProperties(obj);
        }

        protected void DrawProtectedProperties(PointCloudRenderer obj)
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Only preview is currently displayed. Enter play mode to see actual output.",
                    MessageType.Info);
            }

            EditorGUILayout.PropertyField(Colorize);
            EditorGUILayout.PropertyField(Render);
            EditorGUILayout.PropertyField(Mask);

            // Use existing SerializedProperty property to remember foldout state
            Render.isExpanded = EditorGUILayout.Foldout(Render.isExpanded, "Rendering Settings");
            if (Render.isExpanded)
            {
                EditorGUI.indentLevel++;
                DrawShadowsContent((obj.Mask & PointCloudRenderer.RenderMask.Shadows) != 0);
                
                switch (obj.RenderMode)
                {
                    case PointCloudRenderer.RenderType.Points:
                    case PointCloudRenderer.RenderType.Cones:
                    {
                        EditorGUILayout.PropertyField(ConstantSize);
                        if (obj.ConstantSize)
                        {
                            EditorGUILayout.PropertyField(PixelSize);
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(AbsoluteSize);
                            EditorGUILayout.PropertyField(MinPixelSize);
                        }
                        // EditorGUILayout.PropertyField(PartialPointLighting, Styles.PartialPointLightingContent);
                        break;
                    }
                    case PointCloudRenderer.RenderType.Solid:
                    {
                        DrawRemoveHiddenCascadesContent();
                        DrawNormalsContent(obj);
                        DrawFovReprojectionContent(obj.SolidFovReprojection);
                        DrawTemporalSmoothingContent(obj.TemporalSmoothing);

                        // Use existing SerializedProperty property to remember foldout state
                        DebugSolidBlitLevel.isExpanded = EditorGUILayout.Foldout(DebugSolidBlitLevel.isExpanded, "Debug");
                        if (DebugSolidBlitLevel.isExpanded)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(DebugUseLinearDepth, Styles.DebugUseLinearDepthContent);
                            EditorGUILayout.PropertyField(DebugForceFill, Styles.DebugForceFillContent);
                            EditorGUILayout.PropertyField(DebugFillThreshold, Styles.DebugFillThresholdContent);
                            EditorGUILayout.PropertyField(DebugBlendSky, Styles.DebugBlendSkyContent);
                            EditorGUILayout.PropertyField(DebugSolidBlitLevel, Styles.DebugSolidBlitLevelContent);
                            EditorGUILayout.PropertyField(SolidRemoveHidden, Styles.DebugSolidRemoveHiddenContent);
                            EditorGUILayout.PropertyField(DebugSolidPullPush, Styles.DebugSolidPullPushContent);
                            EditorGUILayout.PropertyField(DebugSolidFixedLevel, Styles.DebugSolidFixedLevelContent);
                            EditorGUILayout.PropertyField(DebugSolidPullParam, Styles.DebugSolidPullParamContent);
                            EditorGUI.indentLevel--;
                        }

                        break;
                    }
                }
                EditorGUI.indentLevel--;
            }
        }

        private void DrawRemoveHiddenCascadesContent()
        {
            var rect = CreateBox(4);
            
            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.LabelField(rect, "Cascades (Remove Hidden)", EditorStyles.boldLabel);
            
            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(rect, DebugShowRemoveHiddenCascades, Styles.CascadeShowPreviewContent);
            
            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(rect, RemoveHiddenCascadeOffset, Styles.CascadeOffsetContent);
            
            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(rect, RemoveHiddenCascadeSize, Styles.CascadeSizeContent);

            EditorGUI.indentLevel = indentLevel;
        }
        
        private void DrawNormalsContent(PointCloudRenderer editedObject)
        {
            var lineCount = 2;
            if (editedObject.CalculateNormals && editedObject.SmoothNormals)
                lineCount += 4;

            var rect = CreateBox(lineCount);
            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.PropertyField(rect, CalculateNormals, Styles.CalculateNormalsContent);

            if (editedObject.CalculateNormals)
            {
                rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(rect, SmoothNormals, Styles.SmoothNormalsContent);

                if (editedObject.SmoothNormals)
                {
                    rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.LabelField(rect, "Cascades (Smooth Normals)", EditorStyles.boldLabel);
                    
                    rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(rect, DebugShowSmoothNormalsCascades, Styles.CascadeShowPreviewContent);
            
                    rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(rect, SmoothNormalsCascadeOffset, Styles.CascadeOffsetContent);
            
                    rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(rect, SmoothNormalsCascadeSize, Styles.CascadeSizeContent);
                }
            }
            else
            {
                rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.LabelField(rect, "(required for lighting)");
            }

            EditorGUI.indentLevel = indentLevel;
        }

        private void DrawFovReprojectionContent(bool unfold)
        {
            var lineCount = unfold ? 3 : 1;
            var rect = CreateBox(lineCount);
            
            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.PropertyField(rect, SolidFovReprojection, Styles.FovReprojectionContent);
            
            if (unfold)
            {
                rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(rect, ReprojectionRatio);
                
                rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(rect, PreserveTexelSize);
            }

            EditorGUI.indentLevel = indentLevel;
        }
        
        private void DrawTemporalSmoothingContent(bool unfold)
        {
            // Temporarily disabled due to incompatibility with HDRP (camera relative rendering)
            EditorGUI.BeginDisabledGroup(true);
            {
                var lineCount = unfold ? 2 : 1;
                var rect = CreateBox(lineCount);

                var indentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                EditorGUI.PropertyField(rect, TemporalSmoothing, Styles.TemporalSmoothingContent);

                if (unfold)
                {
                    rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(rect, InterpolatedFrames, Styles.InterpolatedFramesContent);
                }

                EditorGUI.indentLevel = indentLevel;
            }
            EditorGUI.EndDisabledGroup();
        }
        
        private void DrawShadowsContent(bool unfold)
        {
            var lineCount = unfold ? 3 : 1;
            var rect = CreateBox(lineCount);
            
            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            if (unfold)
            {
                EditorGUI.LabelField(rect, "Shadow Settings", EditorStyles.boldLabel);
                
                rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(rect, ShadowPointSize, Styles.ShadowSizeContent);
                
                rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(rect, ShadowBias, Styles.ShadowBiasContent);
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                {
                    EditorGUI.LabelField(rect, "Shadows disabled by mask", EditorStyles.boldLabel);
                }
                EditorGUI.EndDisabledGroup();
            }

            EditorGUI.indentLevel = indentLevel;
        }

        private Rect CreateBox(int lineCount)
        {
            var rect = EditorGUILayout.GetControlRect(false,
                lineCount * EditorGUIUtility.singleLineHeight +
                (lineCount + 3) * EditorGUIUtility.standardVerticalSpacing);
            rect = EditorGUI.IndentedRect(rect);
            
            rect.height -= 2 * EditorGUIUtility.standardVerticalSpacing;
            rect.y += EditorGUIUtility.standardVerticalSpacing;
            GUI.Box(rect, GUIContent.none);
            rect.width -= 8;
            rect.x += 4;
            rect.height = EditorGUIUtility.singleLineHeight;
            
            rect.y += EditorGUIUtility.standardVerticalSpacing;

            return rect;
        }
    }
}
