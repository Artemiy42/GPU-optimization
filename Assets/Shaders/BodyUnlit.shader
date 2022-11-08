Shader "Custom/Unlit/Body"
{
    Properties
    {
        _MainColor("Color", Color) = (0,0,0,0) 
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
                float4 _MainColorInstance = UNITY_ACCESS_INSTANCED_PROP(BodyBuffer, _MainColor);
                Light mainLight = GetMainLight();
                float dotLight = dot(i.normal, mainLight.direction);
                return _MainColorInstance * dotLight;
            }
            ENDHLSL
        }
    }
}
