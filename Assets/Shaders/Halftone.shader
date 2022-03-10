Shader "Surface /Halftone"
{
    //show values to edit in inspector
    Properties
    {
        _Color ("Tint", Color) = (0, 0, 0, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _HalftonePattern("Halftone Pattern", 2D) = "white" {}

        _RemapInputMin ("Remap input min value", Range(0, 1)) = 0
        _RemapInputMax ("Remap input max value", Range(0, 1)) = 1
        _RemapOutputMin ("Remap output min value", Range(0, 1)) = 0
        _RemapOutputMax ("Remap output max value", Range(0, 1)) = 1

        // _Occlusion ("Occlusion", Range(0, 1)) = 1
        //[HDR] _Emission ("Emission", Color) = (0, 0, 0, 1)
        //_Ramp ("Toon Ramp", 2D) = "white" {}
    }

    SubShader
    {
        //the material is completely non-transparent and is rendered at the same time as the other opaque geometry
        Tags{ "RenderType"="Opaque" "Queue"="Geometry" }

        CGPROGRAM

        //include useful shader functions
        #include "UnityCG.cginc"
        #pragma target 3.0

        //define vertex and fragment shader functions
        #pragma surface surf Halftone fullforwardshadows


        //properties passed from the inspector
        //NOTE: inpsector writes tiling and offset to additional var _MainTex_ST
        sampler2D _MainTex;
        fixed4 _Color;
        sampler2D _HalftonePattern;
        float4 _HalftonePattern_ST;
        float _RemapInputMin;
        float _RemapInputMax;
        float _RemapOutputMin;
        float _RemapOutputMax;

        //sampler2D _Ramp;
        // half _Occlusion;
        //half3 _Emission;

        // This function remaps values from a input to a output range
        float map(float input, float inMin, float inMax, float outMin, float outMax)
        {
            //inverse lerp with input range
            float relativeValue = (input - inMin) / (inMax - inMin);
            //lerp with output range
            return lerp(outMin, outMax, relativeValue);
        }

        struct HalftoneSurfaceOutput
        {
            fixed3 Albedo;
            float2 ScreenPos;
            half3 Emission;
            fixed Alpha;
            fixed3 Normal;
        };

        //the input/output that gets passed between shaders
        //NOTE: HLSL says 'fuck you'
        //struct Input -> surface shaders
        //struct v2f -> fragment shaders
        //struct appdata -> vertex shaders
        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        // the fragment shader function
        void surf(Input i, inout HalftoneSurfaceOutput o)
        {
            // get the texture color at the uv coordinate
            fixed4 col = tex2D(_MainTex, i.uv_MainTex);
            // multiply the texture color and tint color
            col *= _Color;

            // get the screenspace texture coordinates of the halftone pattern
            float aspect = _ScreenParams.x / _ScreenParams.y;
            o.ScreenPos = i.screenPos.xy / i.screenPos.w;
            o.ScreenPos = TRANSFORM_TEX(o.ScreenPos, _HalftonePattern);
            o.ScreenPos.x = o.ScreenPos.x * aspect;

            o.Albedo = col.rgb;
            //o.Occlusion = _Occlusion;
            //o.Emission = _Emission;
        }

        //custom lighting function. Must be named Lighting<method>.
        float4 LightingHalftone(HalftoneSurfaceOutput s, float3 lightDir, float atten)
        {
            //how much does the normal point towards the light?
            float towardsLight = dot(s.Normal, lightDir);
            //[-1, 1] -> [0, 1]
            towardsLight = towardsLight * 0.5 + 0.5;

            //combine shadow and light and clamp the result between 0 and 1 to get light intensity
            float lightIntensity = saturate(atten * towardsLight);

            //get halftone comparison value
            float halftoneValue = tex2D(_HalftonePattern, s.ScreenPos).r;
            halftoneValue = map(halftoneValue, _RemapInputMin, _RemapInputMax, _RemapOutputMin, _RemapOutputMax);

            //change light intensity between lit and shadow based on halftone pattern.
            //AA using fwidth, which approximates d(halftoneValue)
            float halftoneChange = fwidth(halftoneValue) * 0.5;
            lightIntensity = smoothstep(halftoneValue - halftoneChange, halftoneValue + halftoneChange, lightIntensity);

            //combine the color
            float4 col;
            //intensity we calculated previously, diffuse color, light falloff and shadowcasting, color of the light
            col.rgb = lightIntensity * s.Albedo * _LightColor0.rgb;
            //in case we want to make the shader transparent in the future - irrelevant right now
            col.a = s.Alpha;

            return col;
        }

        ENDCG
    }
    Fallback "Standard"
}