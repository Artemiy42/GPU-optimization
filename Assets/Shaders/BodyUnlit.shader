Shader "Custom/Unlit/Body"
{
    Properties
    {
        _MainColor("Color", Color) = (0,0,0,1) 
        _Specular("Specular", Float) = 1
        _Smoothness("Smoothness", Float) = 1
        [KeywordEnum(Off, On)] _Light("Light", Float) = 0
        [KeywordEnum(Off, On)] _Additional_Light("Additional Light", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}
         
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _LIGHT_ON _LIGHT_OFF
            #pragma multi_compile _ADDITIONAL_LIGHT_ON _ADDITIONAL_LIGHT_OFF
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #if _ADDITIONAL_LIGHT_ON
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #endif
            // #pragma multi_compile_prepassfinal
			#pragma multi_compile_instancing
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            { 
                float2 uv : TEXCOORD0;
                float3 normalWS : NORMAL;
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD1;
                
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            UNITY_INSTANCING_BUFFER_START(BodyBuffer)
                UNITY_DEFINE_INSTANCED_PROP(float4, _MainColor)
                UNITY_DEFINE_INSTANCED_PROP(float, _Specular)
                UNITY_DEFINE_INSTANCED_PROP(float, _Smoothness)
            UNITY_INSTANCING_BUFFER_END(BodyBuffer)
            
            v2f vert(appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
	            UNITY_TRANSFER_INSTANCE_ID(v, o);

                o.positionWS = TransformObjectToWorld(v.vertex);
                o.positionCS = TransformWorldToHClip(o.positionWS);
                o.normalWS = TransformObjectToWorldNormal(v.normal);
                o.uv = v.uv;
	            
                return o;
            }

            half4 frag(v2f input) : SV_Target 
            {
                UNITY_SETUP_INSTANCE_ID(input);

                float4 color;

                #if _LIGHT_ON
	            InputData lightingInput = (InputData)0;
                lightingInput.normalWS = input.normalWS;
                lightingInput.positionWS = input.positionWS;
                lightingInput.viewDirectionWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                lightingInput.shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                                
	            SurfaceData surfaceInput = (SurfaceData)0;
                surfaceInput.albedo = UNITY_ACCESS_INSTANCED_PROP(BodyBuffer, _MainColor);
                surfaceInput.alpha = 1;
                surfaceInput.specular = UNITY_ACCESS_INSTANCED_PROP(BodyBuffer, _Specular);
                surfaceInput.smoothness = UNITY_ACCESS_INSTANCED_PROP(BodyBuffer, _Smoothness);

	            color = UniversalFragmentBlinnPhong(lightingInput, surfaceInput);
                #elif _LIGHT_OFF
                color = UNITY_ACCESS_INSTANCED_PROP(BodyBuffer, _MainColor);
                #endif

                return color;
            }
            ENDHLSL
        }
    }
}
