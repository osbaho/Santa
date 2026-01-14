Shader "Santa/WorldSpaceDissolve"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        [MainColor] _BaseColor("Base Color", Color) = (1,1,1,1)
        
        [Header(Dissolve Settings)]
        [HDR] _DissolveColor("Dissolve Edge Color", Color) = (1, 0.5, 0, 1)
        _DissolveWidth("Dissolve Edge Width", Range(0.0, 2.0)) = 0.5
        
<<<<<<< HEAD
        [Header(Surface Options)]
        _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        [NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Normal Scale", Float) = 1.0
        [NoScaleOffset] _OcclusionMap("Occlusion", 2D) = "white" {}
        _OcclusionStrength("Occlusion Strength", Range(0.0, 1.0)) = 1.0
        
=======
>>>>>>> origin/odio_github
        [Toggle(_INVERT_DISSOLVE)] _InvertDissolve("Invert (For Liberated Objects)", Float) = 0
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalPipeline" 
            "Queue"="Geometry" 
        }
        
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            
            // URP Keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _INVERT_DISSOLVE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
<<<<<<< HEAD
                float4 tangentOS    : TANGENT;
=======
>>>>>>> origin/odio_github
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 positionWS   : TEXCOORD1;
                float3 normalWS     : NORMAL;
<<<<<<< HEAD
                float4 tangentWS    : TANGENT;
=======
>>>>>>> origin/odio_github
                float2 uv           : TEXCOORD0;
            };

            // CBUFFER for Unity Per Material properties
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half4 _DissolveColor;
                half _DissolveWidth;
<<<<<<< HEAD
                half _Metallic;
                half _Smoothness;
                half _BumpScale;
                half _OcclusionStrength;
=======
>>>>>>> origin/odio_github
            CBUFFER_END

            // Global variables driven by script
            float3 _GlobalDissolveCenter;
            float _GlobalDissolveRadius;

<<<<<<< HEAD
            TEXTURE2D(_BaseMap);            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_BumpMap);            SAMPLER(sampler_BumpMap);
            TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
            TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
=======
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
>>>>>>> origin/odio_github

            Varyings Vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
<<<<<<< HEAD
                
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                output.normalWS = normalInput.normalWS;
                output.tangentWS = float4(normalInput.tangentWS, input.tangentOS.w); // Real Tangent W needed for bitangent
                
=======
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
>>>>>>> origin/odio_github
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                return output;
            }

<<<<<<< HEAD
            // Helper to initialize InputData for PBR
            void InitializeInputData(Varyings input, float3 normalTS, out InputData inputData)
            {
                inputData = (InputData)0;
                inputData.positionWS = input.positionWS;
                
                // Normal handling
                half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                inputData.normalWS = TransformTangentToWorld(normalTS, half3x3(input.tangentWS.xyz, cross(input.normalWS, input.tangentWS.xyz) * input.tangentWS.w, input.normalWS));
                inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
                
                inputData.viewDirectionWS = viewDirWS;
                
                // Shadows and GI
                inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
                inputData.fogCoord = ComputeFogFactor(input.positionCS.z);
                inputData.vertexLighting = half3(0,0,0);
                inputData.bakedGI = SampleSH(inputData.normalWS); // Simplified GI
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
                inputData.shadowMask = half4(1, 1, 1, 1);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                // 1. Dissolve Logic
                float dist = distance(input.positionWS, _GlobalDissolveCenter);
                float transition = dist - _GlobalDissolveRadius;
                
                float edgeMask = 1.0 - smoothstep(0.0, _DissolveWidth, abs(transition));
                
                #if defined(_INVERT_DISSOLVE)
                    clip(-transition); 
                #else
                    clip(transition);
                #endif

                // 2. Texture Sampling
                half4 albedoAlpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                
                // Normal Map
                half3 normalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, input.uv), _BumpScale);

                // Metallic/Smoothness
                // Assuming standard Unity packing (Metal R, Occlusion G, Detail B, Smoothness A) or simple Metal texture
                // Here simplifying to Properties or packed map if needed. keeping it simple for now based on properties provided.
                half metallic = _Metallic;
                half smoothness = _Smoothness;
                
                half occlusion = 1.0;
                // occlusion = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, input.uv).g;
                // occlusion = LerpWhiteTo(occlusion, _OcclusionStrength);

                // 3. Initialize PBR Data
                InputData inputData;
                InitializeInputData(input, normalTS, inputData);

                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = albedoAlpha.rgb;
                surfaceData.metallic = metallic;
                surfaceData.smoothness = smoothness;
                surfaceData.normalTS = normalTS;
                surfaceData.emission = _DissolveColor.rgb * edgeMask; // Emissive Edge here!
                surfaceData.occlusion = occlusion;
                surfaceData.alpha = albedoAlpha.a;

                // 4. Calculate PBR Lighting
                half4 color = UniversalFragmentPBR(inputData, surfaceData);
                
                return color;
=======
            half4 Frag(Varyings input) : SV_Target
            {
                // 1. Calculate Dissolve factor based on World Distance
                float dist = distance(input.positionWS, _GlobalDissolveCenter);
                float transition = dist - _GlobalDissolveRadius;
                
                // 2. Logic:
                // If transition < 0: Inside the sphere
                // If transition > 0: Outside the sphere
                
                // Edge mask (1 at the edge, 0 elsewhere)
                float edgeMask = 1.0 - smoothstep(0.0, _DissolveWidth, abs(transition));
                
                // Clip logic
                // Standard (Gentrified): Visible OUTSIDE (transition > 0), Invisible INSIDE
                // Inverted (Liberated): Visible INSIDE (transition < 0), Invisible OUTSIDE
                
                #if defined(_INVERT_DISSOLVE)
                    // LIBERATED: Must be INSIDE the radius (transition < 0)
                    // We clip if transition > 0
                    clip(-transition); 
                #else
                    // GENTRIFIED: Must be OUTSIDE the radius (transition > 0)
                    // We clip if transition < 0
                     clip(transition);
                #endif

                // 3. Base Color
                half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                
                // 4. Apply Lighting
                Light mainLight = GetMainLight();
                half3 ambient = SampleSH(input.normalWS);
                half3 lightColor = mainLight.color * mainLight.distanceAttenuation * mainLight.shadowAttenuation;
                half NdotL = saturate(dot(input.normalWS, mainLight.direction));
                
                half3 finalColor = albedo.rgb * (ambient + lightColor * NdotL);

                // 5. Add Emissive Edge
                finalColor += _DissolveColor.rgb * edgeMask;

                return half4(finalColor, 1.0);
>>>>>>> origin/odio_github
            }
            ENDHLSL
        }
        
        // ShadowCaster Pass (Important for shadows to disappear correctly)
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }

            ColorMask 0

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_fragment _ _INVERT_DISSOLVE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 positionWS   : TEXCOORD1;
            };

            // Global variables driven by script
            float3 _GlobalDissolveCenter;
            float _GlobalDissolveRadius;

            Varyings Vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float dist = distance(input.positionWS, _GlobalDissolveCenter);
                float transition = dist - _GlobalDissolveRadius;

                #if defined(_INVERT_DISSOLVE)
                    clip(-transition); 
                #else
                    clip(transition);
                #endif

                return 0;
            }
            ENDHLSL
        }
    }
}
