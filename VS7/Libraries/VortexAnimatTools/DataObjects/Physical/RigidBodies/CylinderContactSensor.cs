using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Crownwood.Magic.Controls;
using AnimatTools.Framework;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using VortexAnimatTools.DataObjects.Physical.PropertyHelpers;

namespace VortexAnimatTools.DataObjects.Physical.RigidBodies
{
	/// <summary>
	/// Summary description for MuscleAttachment.
	/// </summary>
	public class CylinderContactSensor : VortexAnimatTools.DataObjects.Physical.RigidBodies.Cylinder
	{
		#region Attributes
		#endregion

		#region Properties

		public override String ImageName {get{return "VortexAnimatTools.Graphics.CylinderContact_Treeview.gif";}}
		public override String ButtonImageName {get{return "VortexAnimatTools.Graphics.CylinderContact_Button.gif";}}
		public override String Type {get{return "CylinderContactSensor";}}
		public override String BodyPartName {get{return "Cylinder Contact Sensor";}}
		public override System.Type PartType {get{return typeof(VortexAnimatTools.DataObjects.Physical.RigidBodies.CylinderContactSensor);}}
		public override bool CanBeRootBody {get{return false;}}
		public override bool UsesAJoint {get{return false;}}
		public override bool AllowCollisions {get{return false;}}
		public override bool HasDynamics {get{return false;}}

		#endregion

		#region Methods

		public CylinderContactSensor(AnimatTools.Framework.DataObject doParent) : base(doParent)
		{
			m_thDataTypes.DataTypes.Clear();
			m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("ContactCount", "Contact Count", "", "", 0, 1));
			m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("BodyPositionX", "Position X Axis", "Meters", "m", -10, 10));
			m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("BodyPositionY", "Position Y Axis", "Meters", "m", -10, 10));
			m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("BodyPositionZ", "Position Z Axis", "Meters", "m", -10, 10));
			m_thDataTypes.ID = "ContactCount";
		}

		protected override void DrawReceptiveFields(AnimatTools.Forms.BodyPlan.Command.enumCommandMode cmCommand)
		{
			this.DrawRigidBody(cmCommand);
		}

		public override void AddChildBody(AnimatTools.DataObjects.Physical.RigidBody rbChildBody)
		{
			//If you try and add a body to a muscle attachment then you really need to add it to the attachment parent.
			if(this.Parent != null)
			{
				AnimatTools.DataObjects.Physical.RigidBody rbParent = (AnimatTools.DataObjects.Physical.RigidBody) this.Parent;
				
				rbChildBody.Parent = rbParent;
				rbParent.AddChildBody(rbChildBody);
			}
		}

		protected override void BuildProperties()
		{
			base.BuildProperties();

			if(m_Properties.Properties.Contains("Cd") == true)
        m_Properties.Properties.Remove("Cd");

			if(m_Properties.Properties.Contains("Cdr") == true)
				m_Properties.Properties.Remove("Cdr");

			if(m_Properties.Properties.Contains("Ca") == true)
				m_Properties.Properties.Remove("Ca");

			if(m_Properties.Properties.Contains("Car") == true)
				m_Properties.Properties.Remove("Car");

			if(m_Properties.Properties.Contains("Contact Sensor") == true)
				m_Properties.Properties.Remove("Contact Sensor");

			if(m_Properties.Properties.Contains("Density") == true)
				m_Properties.Properties.Remove("Density");

			if(m_Properties.Properties.Contains("Odor Sources") == true)
				m_Properties.Properties.Remove("Odor Sources");

			if(m_Properties.Properties.Contains("FoodSource") == true)
				m_Properties.Properties.Remove("FoodSource");
		}

		public override AnimatTools.DataObjects.Physical.BodyPart CreateNewBodyPart(AnimatTools.Framework.DataObject doParent)
		{return new CylinderContactSensor(doParent);}		

		public override AnimatTools.Framework.DataObject Clone(AnimatTools.Framework.DataObject doParent, bool bCutData, AnimatTools.Framework.DataObject doRoot)
		{
			CylinderContactSensor bnPart = new CylinderContactSensor(doParent);
			bnPart.CloneInternal(this, bCutData, doRoot);
			if(doRoot != null && doRoot == this) bnPart.AfterClone(this, bCutData, doRoot, bnPart);
			return bnPart;
		}
		
		public override AnimatTools.Collections.BodyParts SwapBodyPartList()
		{
			AnimatTools.Collections.BodyParts aryList = new AnimatTools.Collections.BodyParts(null);
			aryList.Add(new VortexAnimatTools.DataObjects.Physical.RigidBodies.BoxContactSensor(null));
			return aryList;
		}

		#endregion

	}
}
