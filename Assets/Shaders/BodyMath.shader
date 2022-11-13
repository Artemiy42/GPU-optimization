Shader "Custom/Unlit/BodyMath"
{
    Properties
    {
        _MainColor("Color", Color) = (0,0,0,0)
        _LoopAmount("LoopAmount", Float) = 100000 
        [KeywordEnum(Add, Sub, Mul, Div, Sin, Pow, Log, Dot)] _Operation("Operation", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#pragma multi_compile_instancing
            #pragma multi_compile _OPERATION_ADD _OPERATION_SUB _OPERATION_MUL _OPERATION_DIV _OPERATION_SIM _OPERATION_LOG _OPERATION_DOT
            
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
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
                
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            uniform float _LoopAmount;
            float _Test = 1;
            
            UNITY_INSTANCING_BUFFER_START(BodyBuffer)
                UNITY_DEFINE_INSTANCED_PROP(float4, _MainColor)
            UNITY_INSTANCING_BUFFER_END(BodyBuffer)
            
            v2f vert(appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
	            UNITY_TRANSFER_INSTANCE_ID(v, o);
                
                o.vertex = TransformObjectToHClip(v.vertex);
                o.normal = TransformObjectToWorldNormal(v.normal);
                o.uv = v.uv;
	            
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                for (int index = 0; index < _LoopAmount; index++)
                {
                    #if _OPERATION_ADD
                    _Test += 1;
                    #elif _OPERATION_SUB
                    _Test -= 1;
                    #elif _OPERATION_MUL
                    _Test *= 1;
                    #elif _OPERATION_DIV
                    _Test /= 1;
                    #elif _OPERATION_SIN
                    _Test = sin(1);
                    #elif _OPERATION_LOG
                    _Test = log2(1);
                    #elif _OPERATION_DOT
                    _Test = dot(i.normal, i.normal);
                    #else
                    ;    
                    #endif
                }

                float4 _MainColorInstance = UNITY_ACCESS_INSTANCED_PROP(BodyBuffer, _MainColor);
                Light mainLight = GetMainLight();
                float dotLight = dot(i.normal, mainLight.direction);
                return _MainColorInstance * dotLight + _Test;
            }
            ENDHLSL
        }
    }
}
