using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public class Pose
    {
        BodyPartOrientation rightArmOrientation, leftArmOrientation, rightLegOrientation, leftLegOrientation, torsoOrientation, headOrientation;

        public Pose(BodyPartOrientation rightArmOrientation, BodyPartOrientation leftArmOrientation, BodyPartOrientation rightLegOrientation, BodyPartOrientation leftLegOrientation,
            BodyPartOrientation torsoOrientation, BodyPartOrientation headOrientation)
        {
            if (rightArmOrientation == null)
            {
                this.rightArmOrientation = LimbOrientation.GetNeutralOrientation(SharedContent.LimbID.RightArm);
            }
            if (leftArmOrientation == null)
            {
                this.leftArmOrientation = LimbOrientation.GetNeutralOrientation(SharedContent.LimbID.LeftArm);
            }
            if (rightLegOrientation == null)
            {
                this.rightLegOrientation = LimbOrientation.GetNeutralOrientation(SharedContent.LimbID.RightLeg);
            }
            if (leftLegOrientation == null)
            {
                this.leftLegOrientation = LimbOrientation.GetNeutralOrientation(SharedContent.LimbID.LeftLeg);
            }
            if (torsoOrientation == null)
            {
                this.torsoOrientation = TorsoOrientation.GetNeutralOrientation();
            }
            if (headOrientation == null)
            {
                this.headOrientation = HeadOrientation.GetNeutralOrientation();
            }
        }

        public bool IsInPose(SkeletonData poseData)
        {

        }
    }
}
