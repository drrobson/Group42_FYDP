using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public abstract class BodyPartOrientation
    {
        /*
        public static void CalibrateNeutralBodyPartOrientations(SkeletonData neutralOrientationData)
        {
            //Set neutral head orientation
            HeadOrientation.ActualNeutralInclination = HeadOrientation.CalculateHeadInclination(neutralOrientationData);
            Console.WriteLine("Neutral head orientation\n\thead inclination = {0} degrees from (0,1,0)", Vector3D.AngleBetween(HeadOrientation.ActualNeutralInclination,
                HeadOrientation.IdealNeutralInclination));

            //Set neutral torso orientation
            TorsoOrientation.ActualNeutralTorsoRotation = TorsoOrientation.CalculateTorsoRotation(neutralOrientationData);
            TorsoOrientation.ActualNeutralTorsoInclination = TorsoOrientation.CalculateTorsoInclination(neutralOrientationData);
            Console.WriteLine("Neutral torso orientation\n\ttorso inclination = {0} degrees from (0,1,0)\n\ttorso rotation = {1} degrees", Vector3D.AngleBetween(TorsoOrientation.ActualNeutralTorsoInclination,
                TorsoOrientation.IdealNeutralTorsoInclination), TorsoOrientation.ActualNeutralTorsoRotation);

            //Set neutral limb orientations
            LimbOrientation.ActualNeutralUpperLimbOrientations[(int)SharedContent.BodyPartID.RightArm] = LimbOrientation.CalculateUpperLimbInclination(SharedContent.BodyPartID.RightArm, neutralOrientationData);
            LimbOrientation.ActualNeutralUpperLimbOrientations[(int)SharedContent.BodyPartID.LeftArm] = LimbOrientation.CalculateUpperLimbInclination(SharedContent.BodyPartID.LeftArm, neutralOrientationData);
            LimbOrientation.ActualNeutralUpperLimbOrientations[(int)SharedContent.BodyPartID.RightLeg] = LimbOrientation.CalculateUpperLimbInclination(SharedContent.BodyPartID.RightLeg, neutralOrientationData);
            LimbOrientation.ActualNeutralUpperLimbOrientations[(int)SharedContent.BodyPartID.LeftLeg] = LimbOrientation.CalculateUpperLimbInclination(SharedContent.BodyPartID.LeftLeg, neutralOrientationData);

            LimbOrientation.ActualNeutralBendInLimbs[(int)SharedContent.BodyPartID.RightArm] = LimbOrientation.CalculateBendInLimb(SharedContent.BodyPartID.RightArm, neutralOrientationData);
            LimbOrientation.ActualNeutralBendInLimbs[(int)SharedContent.BodyPartID.LeftArm] = LimbOrientation.CalculateBendInLimb(SharedContent.BodyPartID.LeftArm, neutralOrientationData);
            LimbOrientation.ActualNeutralBendInLimbs[(int)SharedContent.BodyPartID.RightLeg] = LimbOrientation.CalculateBendInLimb(SharedContent.BodyPartID.RightLeg, neutralOrientationData);
            LimbOrientation.ActualNeutralBendInLimbs[(int)SharedContent.BodyPartID.LeftLeg] = LimbOrientation.CalculateBendInLimb(SharedContent.BodyPartID.LeftLeg, neutralOrientationData);
            Console.Write("Neutral limb orientation\n");
            Console.Write("\tright arm: upper inclination = {0}, bend in arm = {1}\n", Vector3D.AngleBetween(LimbOrientation.ActualNeutralUpperLimbOrientations[(int)SharedContent.BodyPartID.RightArm], LimbOrientation.IdealNeutralUpperLimbOrientations[(int)SharedContent.BodyPartID.RightArm]),
                LimbOrientation.ActualNeutralBendInLimbs[(int)SharedContent.BodyPartID.RightArm]);
            Console.Write("\tleft arm: upper inclination = {0}, bend in arm = {1}\n", Vector3D.AngleBetween(LimbOrientation.ActualNeutralUpperLimbOrientations[(int)SharedContent.BodyPartID.LeftArm], LimbOrientation.IdealNeutralUpperLimbOrientations[(int)SharedContent.BodyPartID.LeftArm]),
                LimbOrientation.ActualNeutralBendInLimbs[(int)SharedContent.BodyPartID.LeftArm]);
            Console.Write("\tright leg: upper inclination = {0}, bend in leg = {1}\n", Vector3D.AngleBetween(LimbOrientation.ActualNeutralUpperLimbOrientations[(int)SharedContent.BodyPartID.RightLeg], LimbOrientation.IdealNeutralUpperLimbOrientations[(int)SharedContent.BodyPartID.RightLeg]),
                LimbOrientation.ActualNeutralBendInLimbs[(int)SharedContent.BodyPartID.RightLeg]);
            Console.Write("\tleft leg: upper inclination = {0}, bend in leg = {1}\n", Vector3D.AngleBetween(LimbOrientation.ActualNeutralUpperLimbOrientations[(int)SharedContent.BodyPartID.LeftLeg], LimbOrientation.IdealNeutralUpperLimbOrientations[(int)SharedContent.BodyPartID.LeftLeg]),
                LimbOrientation.ActualNeutralBendInLimbs[(int)SharedContent.BodyPartID.LeftLeg]);
        }
         * */
        /*
        public static BodyPartOrientation GetNeutralBodyPartOrientation(SharedContent.BodyPartID bodyPartID, bool getIdealOrientation)
        {
            BodyPartOrientation neutralBodyPartOrientation;
            switch (bodyPartID)
            {
                case SharedContent.BodyPartID.Head:
                    neutralBodyPartOrientation = getIdealOrientation ? new HeadOrientation(HeadOrientation.IdealNeutralInclination) :
                        new HeadOrientation(HeadOrientation.ActualNeutralInclination);
                    break;
                case SharedContent.BodyPartID.Torso:
                    neutralBodyPartOrientation = getIdealOrientation ? new TorsoOrientation(TorsoOrientation.IdealNeutralTorsoRotation, TorsoOrientation.IdealNeutralTorsoInclination) :
                        new TorsoOrientation(TorsoOrientation.ActualNeutralTorsoRotation, TorsoOrientation.ActualNeutralTorsoInclination);
                    break;
                default:
                    //We know that the body part is a limb, so we cast to limb ID
                    neutralBodyPartOrientation = getIdealOrientation ? new LimbOrientation(bodyPartID, LimbOrientation.IdealNeutralUpperLimbOrientations[(int)bodyPartID],
                        LimbOrientation.IdealNeutralBendInLimbs[(int)bodyPartID]) : new LimbOrientation(bodyPartID, LimbOrientation.ActualNeutralUpperLimbOrientations[(int)bodyPartID],
                        LimbOrientation.ActualNeutralBendInLimbs[(int)bodyPartID]);
                    break;
            }

            return neutralBodyPartOrientation;
        }
         * */

        public abstract void CalibrateOrientation(SkeletonData bodyPartData);
        public abstract bool IsBodyPartInOrientation(SkeletonData bodyPartData);
        public abstract bool IsInBetweenOrientations(BodyPartOrientation initialOrientation, BodyPartOrientation finalOrientation);
    }
}
