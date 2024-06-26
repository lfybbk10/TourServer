Shader "AVProVideo/Internal/UI/Transparent Packed (stereo) - AndroidOES"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _ChromaTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		_VertScale("Vertical Scale", Range(-1, 1)) = 1.0

		[KeywordEnum(None, Top_Bottom, Left_Right)] AlphaPack("Alpha Pack", Float) = 0
		[KeywordEnum(None, Top_Bottom, Left_Right)] Stereo("Stereo Mode", Float) = 0
		[KeywordEnum(None, Left, Right)] ForceEye ("Force Eye Mode", Float) = 0
		[Toggle(APPLY_GAMMA)] _ApplyGamma("Apply Gamma", Float) = 0
		[Toggle(STEREO_DEBUG)] _StereoDebug("Stereo Debug Tinting", Float) = 0
		[Toggle(USE_YPCBCR)] _UseYpCbCr("Use YpCbCr", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
			GLSLPROGRAM
			#pragma only_renderers gles gles3

			// TODO: replace use multi_compile_local instead (Unity 2019.1 feature)
			#pragma multi_compile ALPHAPACK_NONE ALPHAPACK_TOP_BOTTOM ALPHAPACK_LEFT_RIGHT
			#pragma multi_compile MONOSCOPIC STEREO_TOP_BOTTOM STEREO_LEFT_RIGHT
			#pragma multi_compile FORCEEYE_NONE FORCEEYE_LEFT FORCEEYE_RIGHT
			#pragma multi_compile __ APPLY_GAMMA
			#pragma multi_compile __ STEREO_DEBUG
			#pragma multi_compile __ USE_YPCBCR
			#pragma multi_compile __ USING_DEFAULT_TEXTURE
			#pragma multi_compile __ USING_URP

			#extension GL_OES_EGL_image_external : require
			#extension GL_OES_EGL_image_external_essl3 : enable

#if defined(APPLY_GAMMA)
			//#pragma target 3.0
#endif

#ifdef VERTEX

			#include "UnityCG.glslinc"
		#if defined(STEREO_MULTIVIEW_ON)
			UNITY_SETUP_STEREO_RENDERING
		#endif
			// TODO: once we drop support for Unity 4.x then we can include this
			//#include "UnityUI.cginc"    
			#define SHADERLAB_GLSL
			#include "../AVProVideo.cginc"
			
			INLINE bool Android_IsStereoEyeLeft()
			{
				#if defined(STEREO_MULTIVIEW_ON)
					int eyeIndex = SetupStereoEyeIndex();
					return (eyeIndex == 0);
				#else
					return IsStereoEyeLeft();
				#endif
			}		
			
		#if defined(ALPHAPACK_TOP_BOTTOM) || defined(ALPHAPACK_LEFT_RIGHT)
			varying vec4 texVal;
		#else
			varying vec2 texVal;
		#endif
		#if defined(STEREO_DEBUG)
			varying vec4 tint;
		#endif

			uniform vec4 _MainTex_ST;
			uniform vec4 _MainTex_TexelSize;
			uniform mat4 _MainTex_Xfrm;

			/// @fix: explicit TRANSFORM_TEX(); Unity's preprocessor chokes when attempting to use the TRANSFORM_TEX() macro in UnityCG.glslinc
/// 		(as of Unity 4.5.0f6; issue dates back to 2011 or earlier: http://forum.unity3d.com/threads/glsl-transform_tex-and-tiling.93756/)
			vec2 transformTex(vec4 texCoord, vec4 texST)
			{
				return (texCoord.xy * texST.xy + texST.zw);
			}

			void main()
			{
				#if defined(STEREO_MULTIVIEW_ON)
					int eyeIndex = SetupStereoEyeIndex();
					mat4 vpMatrix = GetStereoMatrixVP(eyeIndex);
					gl_Position = vpMatrix * unity_ObjectToWorld * gl_Vertex;
				#else
					gl_Position = XFormObjectToClip(gl_Vertex);
				#endif

				texVal.xy = transformTex(gl_MultiTexCoord0, _MainTex_ST);

				// Apply texture transformation matrix - adjusts for offset/cropping (when the decoder decodes in blocks that overrun the video frame size, it pads)
				texVal.xy = (_MainTex_Xfrm * vec4(texVal.x, texVal.y, 0.0, 1.0)).xy;

				#if defined(STEREO_TOP_BOTTOM) || defined(STEREO_LEFT_RIGHT)
					vec4 scaleOffset = GetStereoScaleOffset( Android_IsStereoEyeLeft(), _MainTex_ST.y < 0.0 );
					texVal.xy *= scaleOffset.xy;
					texVal.xy += scaleOffset.zw;
				#endif

				#if defined(ALPHAPACK_TOP_BOTTOM) || defined(ALPHAPACK_LEFT_RIGHT)
					texVal = OffsetAlphaPackingUV(_MainTex_TexelSize.xy, texVal.xy, _MainTex_ST.y < 0.0);
					#if defined(ALPHAPACK_TOP_BOTTOM)
						texVal.yw = texVal.wy;
					#endif
				#endif

				#if defined(STEREO_DEBUG)
					tint = GetStereoDebugTint( Android_IsStereoEyeLeft() );
				#endif
			}
#endif

#ifdef FRAGMENT
		#if defined(ALPHAPACK_TOP_BOTTOM) || defined(ALPHAPACK_LEFT_RIGHT)
			varying vec4 texVal;
		#else
			varying vec2 texVal;
		#endif

		#if defined(USING_DEFAULT_TEXTURE)
			uniform sampler2D _MainTex;
		#else
			uniform samplerExternalOES _MainTex;
		#endif

#if defined(STEREO_DEBUG)
			varying vec4 tint;
#endif

		#if defined(APPLY_GAMMA)
			vec3 GammaToLinear(vec3 col)
			{
				return pow(col, vec3(2.2, 2.2, 2.2));
			}
		#endif

			void main()
			{
				#if defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)
					#if __VERSION__ < 300
						vec4 col = texture2D(_MainTex, texVal.xy);
					#else
						vec4 col = texture(_MainTex, texVal.xy);
					#endif
				#else
					vec4 col = vec4(1.0, 1.0, 0.0, 1.0);
				#endif

				#if defined(APPLY_GAMMA)
					col.rgb = GammaToLinear(col.rgb);
				#endif

				#if defined(ALPHAPACK_TOP_BOTTOM) || defined(ALPHAPACK_LEFT_RIGHT)
					#if defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)
						#if __VERSION__ < 300
							vec3 rgb = texture2D(_MainTex, texVal.zw).rgb;
						#else
							vec3 rgb = texture(_MainTex, texVal.zw).rgb;
						#endif

						col.a = (rgb.r + rgb.g + rgb.b) / 3.0;
					#else
						col.a = 1.0;
					#endif
				#endif

#if defined(STEREO_DEBUG)
				col *= tint;
#endif

				gl_FragColor = col;
			}
#endif

			ENDGLSL
		}
	}
	Fallback "AVProVideo/Internal/UI/Transparent Packed (stereo)"
}
