using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public class HeadOrientation : BodyPartOrientation
    {
        public Vector3D inclination;
        public Vector3D calibratedInclination;

        public HeadOrientation(SkeletonData bodyPartData)
        {
            this.inclination = HeadOrientation.CalculateHeadInclination(bodyPartData);
            this.calibratedInclination = this.inclination;
        }

        public HeadOrientation(Vector3D inclination)
        {
            this.inclination = inclination;
            this.inclination.Normalize();
            this.calibratedInclination = this.inclination;
        }

        public static Vector3D CalculateHeadInclination(SkeletonData bodyPartData)
        {
            Vector centerShoulderPosition = bodyPartData.Joints[JointID.ShoulderCenter].Position;
            Vector headPosition = bodyPartData.Joints[JointID.Head].Position;

            return new Vector3D(headPosition.X - centerShoulderPosition.X, headPosition.Y - centerShoulderPosition.Y,
                headPosition.Z - centerShoulderPosition.Z);
        }

        public override void CalibrateOrientation(SkeletonData bodyPartData)
        {
            this.calibratedInclination = HeadOrientation.CalculateHeadInclination(bodyPartData);
            this.calibratedInclination.Normalize();
        }

        public override bool IsBodyPartInOrientation(SkeletonData bodyPartData)
        {
            Vector3D currentInclination = HeadOrientation.CalculateHeadInclination(bodyPartData);

            if (Vector3D.AngleBetween(currentInclination, this.calibratedInclination) > SharedContent.GetAllowableDeviationInDegrees())
            {
                return false;
            }
            return true;
        }

        public override UserPerformanceAnalysisInfo IsInBetweenOrientations(BodyPartOrientation initialOrientation, BodyPartOrientation finalOrientation)
        {
            HeadOrientation initialHeadOrientation = (HeadOrientation)initialOrientation;
            HeadOrientation finalHeadOrientation = (HeadOrientation)finalOrientation;

            UserPerformanceAnalysisInfo headInclinationInfo = BodyPartOrientation.IsVectorOnPathInPlane(this.calibratedInclination, initialHeadOrientation.calibratedInclination, finalHeadOrientation.calibratedInclination);

            if (headInclinationInfo.failed)
            {
                Console.WriteLine("Failed when checking head inclination");
                return headInclinationInfo;
            }
            else if (Vector3D.AngleBetween(initialHeadOrientation.inclination, finalHeadOrientation.inclination) <= SharedContent.GetAllowableDeviationInDegrees())
            {
                //The actual exercise has a negligable change in the head inclination
                return new UserPerformanceAnalysisInfo(true);
            }
            else
            {
                return headInclinationInfo;
            }
        }
    }
}
