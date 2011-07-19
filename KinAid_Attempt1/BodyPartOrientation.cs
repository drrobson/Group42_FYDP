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
        public abstract int IsInBetweenOrientations(BodyPartOrientation initialOrientation, BodyPartOrientation finalOrientation);

        /// <summary>
        /// Checks whether: 1) the vector to check is approximately in the plane created by the other vectors, where it is approximately in the plane if
        /// the angle between it and its projection in the plane is less than or equal to the allowable deviation in degrees, and 2) the vector to check's
        /// projection in the plane yields a vector that segments the interior angle of the angle between the other vectors. Returns the percentage of the angular
        /// distance between the first plane-defining vector and the second plane-defining vector that the vector to check has traveled
        /// </summary>
        /// <param name="vectorToCheck"></param>
        /// <param name="firstVector"></param>
        /// <param name="secondVector"></param>
        /// <returns>-1 if vector does satisfy requirements. -2 if there is a negligable (less than allowable deviation) angle between the first plane vector
        /// and the second plane vector</returns>
        public static int IsVectorOnPathInPlane(Vector3D vectorToCheck, Vector3D firstPlaneVector, Vector3D secondPlaneVector)
        {
            double totalDisplacementToTravel = Vector3D.AngleBetween(firstPlaneVector, secondPlaneVector);
            if (totalDisplacementToTravel <= SharedContent.AllowableDeviationInDegrees) return -2;

            Vector3D normalToPlane = Vector3D.CrossProduct(firstPlaneVector, secondPlaneVector);
            normalToPlane.Normalize();
            Vector3D projectionOfInclIntoNormal = Vector3D.DotProduct(vectorToCheck, normalToPlane) * normalToPlane;
            Vector3D projectionOfInclIntoPlane = vectorToCheck - projectionOfInclIntoNormal;

            //Verifies that the deviation from the plane defined by the initial and final orientations is less than the allowable deviation
            if (Vector3D.AngleBetween(projectionOfInclIntoPlane, vectorToCheck) > SharedContent.AllowableDeviationInDegrees)
            {
                return -1;
            }

            //Verifies that this orientation instance falls between the initial and final orientations in the interior angle give or take the allowable deviation
            if ((Vector3D.AngleBetween(projectionOfInclIntoPlane, secondPlaneVector) > Vector3D.AngleBetween(secondPlaneVector, firstPlaneVector) && 
                Vector3D.AngleBetween(projectionOfInclIntoPlane,firstPlaneVector) > SharedContent.AllowableDeviationInDegrees) ||
                (Vector3D.AngleBetween(projectionOfInclIntoPlane, firstPlaneVector) > Vector3D.AngleBetween(secondPlaneVector, firstPlaneVector) &&
                Vector3D.AngleBetween(projectionOfInclIntoPlane, secondPlaneVector) > SharedContent.AllowableDeviationInDegrees))
            {
                return -1;
            }

            double currentDisplacementTraveled = Vector3D.AngleBetween(projectionOfInclIntoPlane, firstPlaneVector);

            return (int)(100 * (currentDisplacementTraveled / totalDisplacementToTravel));
        }
    }
}
