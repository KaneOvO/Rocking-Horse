Shader "Roystan/Grass"
{
    Properties
    {
        [Header(Shading)]
        _TopColor("Top Color", Color) = (1,1,1,1)
        _BottomColor("Bottom Color", Color) = (1,1,1,1)
        _TranslucentGain("Translucent Gain", Range(0,1)) = 0.5
        [Space]
        _TessellationUniform ("Tessellation Uniform", Range(1, 64)) = 1
        [Header(Blades)]
        _BladeWidth("Blade Width", Float) = 0.05
        _BladeWidthRandom("Blade Width Random", Float) = 0.02
        _BladeHeight("Blade Height", Float) = 0.5
        _BladeHeightRandom("Blade Height Random", Float) = 0.3
        _BladeForward("Blade Forward Amount", Float) = 0.38
        _BladeCurve("Blade Curvature Amount", Range(1, 4)) = 2
        _BendRotationRandom("Bend Rotation Random", Range(0, 1)) = 0.2
        [Header(Wind)]
        _WindDistortionMap("Wind Distortion Map", 2D) = "white" {}
        _WindStrength("Wind Strength", Float) = 1
        _WindFrequency("Wind Frequency", Vector) = (0.05, 0.05, 0, 0)
    }

    CGINCLUDE
    #include "UnityCG.cginc"
    #include "Autolight.cginc"


    struct vertexInput
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float4 tangent : TANGENT;
    };

    struct vertexOutput
    {
        float4 vertex : SV_POSITION;
        float3 normal : NORMAL;
        float4 tangent : TANGENT;
    };

    struct TessellationFactors
    {
        float edge[3] : SV_TessFactor;
        float inside : SV_InsideTessFactor;
    };

    vertexInput vert(vertexInput v)
    {
        return v;
    }

    vertexOutput tessVert(vertexInput v)
    {
        vertexOutput o;
        // Note that the vertex is NOT transformed to clip
        // space here; this is done in the grass geometry shader.
        o.vertex = v.vertex;
        o.normal = v.normal;
        o.tangent = v.tangent;
        return o;
    }

    float _TessellationUniform;

    TessellationFactors patchConstantFunction(InputPatch<vertexInput, 3> patch)
    {
        TessellationFactors f;
        f.edge[0] = _TessellationUniform;
        f.edge[1] = _TessellationUniform;
        f.edge[2] = _TessellationUniform;
        f.inside = _TessellationUniform;
        return f;
    }

    [UNITY_domain("tri")]
    [UNITY_outputcontrolpoints(3)]
    [UNITY_outputtopology("triangle_cw")]
    [UNITY_partitioning("integer")]
    [UNITY_patchconstantfunc("patchConstantFunction")]
    vertexInput hull(InputPatch<vertexInput, 3> patch, uint id : SV_OutputControlPointID)
    {
        return patch[id];
    }

    [UNITY_domain("tri")]
    vertexOutput domain(TessellationFactors factors, OutputPatch<vertexInput, 3> patch,
                        float3 barycentricCoordinates : SV_DomainLocation)
    {
        vertexInput v;

        #define MY_DOMAIN_PROGRAM_INTERPOLATE(fieldName) v.fieldName = \
		patch[0].fieldName * barycentricCoordinates.x + \
		patch[1].fieldName * barycentricCoordinates.y + \
		patch[2].fieldName * barycentricCoordinates.z;

        MY_DOMAIN_PROGRAM_INTERPOLATE(vertex)
        MY_DOMAIN_PROGRAM_INTERPOLATE(normal)
        MY_DOMAIN_PROGRAM_INTERPOLATE(tangent)

        return tessVert(v);
    }

    struct geometryOutput
    {
        float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
        float3 posWS: TEXCOORD1;
        float3 posOS: TEXCOORD2;
    };

    float rand(float3 co)
    {
        return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453);
    }

    // Construct a rotation matrix that rotates around the provided axis, sourced from:
    // https://gist.github.com/keijiro/ee439d5e7388f3aafc5296005c8c3f33
    float3x3 AngleAxis3x3(float angle, float3 axis)
    {
        float c, s;
        sincos(angle, s, c);

        float t = 1 - c;
        float x = axis.x;
        float y = axis.y;
        float z = axis.z;

        return float3x3(
            t * x * x + c, t * x * y - s * z, t * x * z + s * y,
            t * x * y + s * z, t * y * y + c, t * y * z - s * x,
            t * x * z - s * y, t * y * z + s * x, t * z * z + c
        );
    }

    geometryOutput VertexOutput(float3 pos, float3 normal, float2 uv)
    {
        geometryOutput o;

        o.pos = UnityObjectToClipPos(pos);
        o.posWS = mul(unity_ObjectToWorld,pos);
        o.posOS = pos;
		o.uv = uv;

        return o;
    }

    geometryOutput GenerateGrassVertex(float3 vertexPosition, float width, float height, float forward, float2 uv,
                                       float3x3 transformMatrix)
    {
        float3 tangentPoint = float3(width, forward, height);

        float3 tangentNormal = normalize(float3(0, -1, forward));

        float3 localPosition = vertexPosition + mul(transformMatrix, tangentPoint);
        float3 localNormal = mul(transformMatrix, tangentNormal);
        return VertexOutput(localPosition, localNormal, uv);
    }

    float _BladeHeight;
    float _BladeHeightRandom;

    float _BladeWidthRandom;
    float _BladeWidth;

    float _BladeForward;
    float _BladeCurve;

    float _BendRotationRandom;

    sampler2D _WindDistortionMap;
    float4 _WindDistortionMap_ST;

    float _WindStrength;
    float2 _WindFrequency;

    #define BLADE_SEGMENTS 3

    // Geometry program that takes in a single triangle and outputs a blade
    // of grass at that triangle first vertex position, aligned to the vertex's normal.
    [maxvertexcount(BLADE_SEGMENTS * 2 + 1)]
    void geo(triangle vertexOutput IN[3], inout TriangleStream<geometryOutput> triStream)
    {
        float3 pos = IN[0].vertex.xyz;

        // Each blade of grass is constructed in tangent space with respect
        // to the emitting vertex's normal and tangent vectors, where the width
        // lies along the X axis and the height along Z.

        // Construct random rotations to point the blade in a direction.
        float3x3 facingRotationMatrix = AngleAxis3x3(rand(pos) * UNITY_TWO_PI, float3(0, 0, 1));
        // Matrix to bend the blade in the direction it's facing.
        float3x3 bendRotationMatrix = AngleAxis3x3(rand(pos.zzx) * _BendRotationRandom * UNITY_PI * 0.5,
                                                   float3(-1, 0, 0));

        // Sample the wind distortion map, and construct a normalized vector of its direction.
        float2 uv = pos.xz * _WindDistortionMap_ST.xy + _WindDistortionMap_ST.zw + _WindFrequency * _Time.y;
        float2 windSample = (tex2Dlod(_WindDistortionMap, float4(uv, 0, 0)).xy * 2 - 1) * _WindStrength;
        float3 wind = normalize(float3(windSample.x, windSample.y, 0));

        float3x3 windRotation = AngleAxis3x3(UNITY_PI * windSample, wind);

        // Construct a matrix to transform our blade from tangent space
        // to local space; this is the same process used when sampling normal maps.
        float3 vNormal = IN[0].normal;
        float4 vTangent = IN[0].tangent;
        float3 vBinormal = cross(vNormal, vTangent) * vTangent.w;

        float3x3 tangentToLocal = float3x3(
            vTangent.x, vBinormal.x, vNormal.x,
            vTangent.y, vBinormal.y, vNormal.y,
            vTangent.z, vBinormal.z, vNormal.z
        );

        // Construct full tangent to local matrix, including our rotations.
        // Construct a second matrix with only the facing rotation; this will be used 
        // for the root of the blade, to ensure it always faces the correct direction.
        float3x3 transformationMatrix = mul(mul(mul(tangentToLocal, windRotation), facingRotationMatrix),
                                            bendRotationMatrix);
        float3x3 transformationMatrixFacing = mul(tangentToLocal, facingRotationMatrix);

        float height = (rand(pos.zyx) * 2 - 1) * _BladeHeightRandom + _BladeHeight;
        float width = (rand(pos.xzy) * 2 - 1) * _BladeWidthRandom + _BladeWidth;
        float forward = rand(pos.yyz) * _BladeForward;

        for (int i = 0; i < BLADE_SEGMENTS; i++)
        {
            float t = i / (float)BLADE_SEGMENTS;

            float segmentHeight = height * t;
            float segmentWidth = width * (1 - t);
            float segmentForward = pow(t, _BladeCurve) * forward;

            // Select the facing-only transformation matrix for the root of the blade.
            float3x3 transformMatrix = i == 0 ? transformationMatrixFacing : transformationMatrix;

            triStream.Append(GenerateGrassVertex(pos, segmentWidth, segmentHeight, segmentForward, float2(0, t),
                                                 transformMatrix));
            triStream.Append(GenerateGrassVertex(pos, -segmentWidth, segmentHeight, segmentForward, float2(1, t),
                                                 transformMatrix));
        }

        // Add the final vertex as the tip.
        triStream.Append(GenerateGrassVertex(pos, 0, height, forward, float2(0.5, 1), transformationMatrix));
    }
    ENDCG

    SubShader
    {
        Cull Off

        Pass
        {
            Tags
            {
                "RenderType" = "Opaque"
                "LightMode" = "ForwardBase"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geo
            #pragma fragment frag
            #pragma hull hull
            #pragma domain domain
            #pragma target 4.6

            #include "Lighting.cginc"
            #include "Noise.cginc"
            
            float4 _TopColor;
            float4 _BottomColor;
            float _TranslucentGain;

            float4 frag(geometryOutput i, fixed facing : VFACE) : SV_Target
            {
                float4 col = lerp(_BottomColor, _TopColor, i.uv.y);
                float scale = 0.75;
                float v1 = voronoi1(i.posOS.xz*scale+float2(0.1,0),_Time.y*0.4,0,1,10);
                float v2 = voronoi1(i.posOS.xz*scale,_Time.y*0.4,0,1,10);
                float v3 = voronoi1(i.posOS.xz*scale+float2(-0.1,0.1),_Time.y*0.4,0,1,10);
                float3 c2 = float3(v1,v2,v3);
                c2 = pow(c2,2);
                return col * 120*c2.xyzz + 0.3;
            }
            ENDCG
        }

    }
}