﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;

namespace Coffee.UIExtensions
{
	[ExecuteInEditMode]
	public class UIMirrorReflection : BaseMeshEffect
	{
		//################################
		// Serialize Members.
		//################################
		[SerializeField][Range(1, 500)] float m_Height = 50f;
		[SerializeField] float m_Spacing = 50f;
		[SerializeField] Color m_StartColor = new Color(1, 1, 1, 0.75f);
		[SerializeField] Color m_EndColor = new Color(1, 1, 1, 0);


		//################################
		// Public Members.
		//################################

		/// <summary>
		/// Target graphic for the effect.
		/// </summary>
		public Graphic targetGraphic { get { return base.graphic; } }

		/// <summary>
		/// Height for mirror reflected image.
		/// </summary>
		public float height
		{
			get { return m_Height; }
			set
			{ 
				if (!Mathf.Approximately(m_Height, value))
				{
					m_Height = value;
					SetDirty();
				}
			}
		}

		/// <summary>
		/// Spacing for mirror reflected image.
		/// </summary>
		public float spacing
		{
			get { return m_Spacing; }
			set
			{ 
				if (!Mathf.Approximately(m_Spacing, value))
				{
					m_Spacing = value;
					SetDirty();
				}
			}
		}

		/// <summary>
		/// Mirror reflection color (start).
		/// </summary>
		public Color startColor
		{
			get { return m_StartColor; }
			set
			{ 
				if (m_StartColor != value)
				{
					m_StartColor = value;
					SetDirty();
				}
			}
		}

		/// <summary>
		/// Mirror reflection color (end).
		/// </summary>
		public Color etartColor
		{
			get { return m_EndColor; }
			set
			{ 
				if (m_EndColor != value)
				{
					m_EndColor = value;
					SetDirty();
				}
			}
		}

		/// <summary>
		/// Modifies the mesh.
		/// </summary>
		public override void ModifyMesh(VertexHelper vh)
		{
			// Invalid.
			if (!isActiveAndEnabled || vh.currentVertCount == 0 || (vh.currentVertCount % 4 != 0 && vh.currentVertCount % 6 != 0))
			{
				return;
			}

			_rect = graphic.rectTransform.rect;

			var quad = UIVertexUtil.s_QuadVerts;
			var inputVerts = UIVertexUtil.s_InputVerts;
			var outputVerts = UIVertexUtil.s_OutputVerts;

			inputVerts.Clear();
			outputVerts.Clear();

			vh.GetUIVertexStream(inputVerts);

			for (int i = 0; i < inputVerts.Count; i += 6)
			{
				if (graphic is Text)
				{
					quad[0] = inputVerts[i + 4];	// bottom-left
					quad[1] = inputVerts[i + 0];	// top-left
					quad[2] = inputVerts[i + 1];	// top-right
					quad[3] = inputVerts[i + 2];	// bottom-right
				}
				else
				{
					quad[0] = inputVerts[i + 0];	// bottom-left
					quad[1] = inputVerts[i + 1];	// top-left
					quad[2] = inputVerts[i + 2];	// top-right
					quad[3] = inputVerts[i + 4];	// bottom-right
				}
				UIVertexUtil.AddQuadToStream(quad, outputVerts);	// origin quad
				AddMirrorReflectedQuad(quad, outputVerts);	// reflected quad
			}

			vh.Clear();
			vh.AddUIVertexTriangleStream(outputVerts);

			inputVerts.Clear();
			outputVerts.Clear();
		}


		//################################
		// Private Members.
		//################################
		Rect _rect;

		void Update()
		{
			graphic.SetVerticesDirty ();
		}


		void AddMirrorReflectedQuad(UIVertex[] quad, List<UIVertex> result)
		{
			// Read the existing quad vertices
			UIVertex v0 = quad[0];	// bottom-left
			UIVertex v1 = quad[1];	// top-left
			UIVertex v2 = quad[2];	// top-right
			UIVertex v3 = quad[3];	// bottom-right

			var t = graphic.rectTransform;
//			var matrix =Matrix4x4.TRS (Vector3.zero, Quaternion.identity, Vector3.one);
//			var matrix = Matrix4x4.TRS (new Vector3(0, -_rect.height-m_Spacing), Quaternion.identity, new Vector3(1,-1,1));

			var lm = Matrix4x4.TRS (t.localPosition, t.localRotation, t.localScale);

			var matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3(1,-1,1));

			matrix = Matrix4x4.TRS (new Vector3(0, -_rect.height-m_Spacing), Quaternion.identity, new Vector3(1,-1,1));

			var rot = Quaternion.Euler (-t.parent.rotation.eulerAngles);
			var rot2 = Quaternion.Euler (-t.localRotation.eulerAngles);
			var rot3 = Quaternion.Euler (t.localRotation.eulerAngles);

			var matrix2 =Matrix4x4.TRS (Vector3.zero, t.rotation, Vector3.one);


//			quad[0].position = matrix2.MultiplyPoint3x4(rot * matrix.MultiplyPoint3x4((quad[0].position)));
//			quad[1].position = matrix2.MultiplyPoint3x4(rot * matrix.MultiplyPoint3x4((quad[1].position)));
//			quad[2].position = matrix2.MultiplyPoint3x4(rot * matrix.MultiplyPoint3x4((quad[2].position)));
//			quad[3].position = matrix2.MultiplyPoint3x4(rot * matrix.MultiplyPoint3x4((quad[3].position)));

//			quad[0].position = (matrix.MultiplyPoint3x4(rot2*rot * (quad[0].position)));
//			quad[1].position = (matrix.MultiplyPoint3x4(rot2*rot * (quad[1].position)));
//			quad[2].position = (matrix.MultiplyPoint3x4(rot2*rot * (quad[2].position)));
//			quad[3].position = (matrix.MultiplyPoint3x4(rot2*rot * (quad[3].position)));

//			var matrix = t.localToWorldMatrix.inverse
//				* Matrix4x4.TRS (t.position, Quaternion.identity, t.lossyScale);

//			var matrix = Matrix4x4.TRS (t.localPosition, t.localRotation, t.localScale).inverse
//				* Matrix4x4.TRS (t.localPosition, Quaternion.identity, t.localScale);

//			var matrix = Matrix4x4.TRS (new Vector3(0, -_rect.height-m_Spacing), Quaternion.identity, new Vector3(1,-1,1));


			// Reverse quad index.
//			quad[0].position = rot2*matrix.MultiplyPoint3x4(lm.MultiplyPoint3x4(rot3*lm.inverse.MultiplyPoint3x4(quad[0].position)));
//			quad[1].position = rot2*matrix.MultiplyPoint3x4(lm.MultiplyPoint3x4(rot3*lm.inverse.MultiplyPoint3x4(quad[1].position)));
//			quad[2].position = rot2*matrix.MultiplyPoint3x4(lm.MultiplyPoint3x4(rot3*lm.inverse.MultiplyPoint3x4(quad[2].position)));
//			quad[3].position = rot2*matrix.MultiplyPoint3x4(lm.MultiplyPoint3x4(rot3*lm.inverse.MultiplyPoint3x4(quad[3].position)));

			matrix =Matrix4x4.TRS (new Vector3(0, -_rect.height-m_Spacing), Quaternion.identity, new Vector3(1,-1,1));

			var vecs = new Vector3[4];
			t.GetWorldCorners (vecs);
			var worldY = t.position.y - t.rect.height * t.lossyScale.y * t.pivot.y * 2;


			var scale = t.lossyScale;
			scale.y *= -1;
			var eul = -t.rotation.eulerAngles;
			eul.y *= -1;

			matrix = Matrix4x4.identity
				*
				Matrix4x4.TRS (t.position, t.rotation, t.lossyScale).inverse
				*
				Matrix4x4.TRS (new Vector3(t.position.x, worldY, t.position.z), Quaternion.Euler (eul), scale)
				;
			


			var lwm = t.localToWorldMatrix;
			var wlm = t.worldToLocalMatrix;

//			// Add reflected quad.
//			UIVertexUtil.AddQuadToStream(quad, result);
//			return;

			var minY = Mathf.Min (
				Mathf.Min (quad[0].position.y, quad[1].position.y),
				Mathf.Min (quad[2].position.y, quad[3].position.y)
			);

			// Reflection is unnecessary.
			if (m_Height < (v0.position.y - _rect.yMin) && m_Height < (v3.position.y - _rect.yMin))
			{
				return;
			}

			// Trim quad.
			if (m_Height < (v1.position.y - _rect.yMin) || m_Height < (v2.position.y - _rect.yMin))
			{
				v1 = UIVertexUtil.Lerp(v0, v1, GetLerpFactor(v0.position.y, v1.position.y));
				v2 = UIVertexUtil.Lerp(v3, v2, GetLerpFactor(v3.position.y, v2.position.y));
			}

			// Calculate reflected position and color.
//			MirrorReflectVertex(ref v0);
//			MirrorReflectVertex(ref v1);
//			MirrorReflectVertex(ref v2);
//			MirrorReflectVertex(ref v3);

			// Reverse quad index.
			quad[0] = v1;
			quad[1] = v0;
			quad[2] = v3;
			quad[3] = v2;


			quad[0].position = (matrix.MultiplyPoint3x4((quad[0].position)));
			quad[1].position = (matrix.MultiplyPoint3x4((quad[1].position)));
			quad[2].position = (matrix.MultiplyPoint3x4((quad[2].position)));
			quad[3].position = (matrix.MultiplyPoint3x4((quad[3].position)));

			// Add reflected quad.
			UIVertexUtil.AddQuadToStream(quad, result);
		}

		float GetLerpFactor(float bottom, float top)
		{
			return (m_Height + _rect.yMin - bottom) / (top - bottom);
		}

		void MirrorReflectVertex(ref UIVertex vt)
		{
			var col = vt.color;
			var pos = vt.position;

			// Reflected color
			var factor = Mathf.Clamp01((pos.y - _rect.yMin) / m_Height);
			col *= Color.Lerp(m_StartColor, m_EndColor, factor);
			vt.color = col;

			// Reflected position.
//			pos.y = _rect.yMin * 2 - m_Spacing - pos.y;
//			vt.position = pos;
		}


		/// <summary>
		/// Mark the target graphic as dirty.
		/// </summary>
		void SetDirty()
		{
			if (graphic)
				graphic.SetVerticesDirty();
		}
	}
}