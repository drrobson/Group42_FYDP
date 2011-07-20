using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public static class ExerciseFactory
    {
        static Exercise shoulderAbduction = null;
        static Exercise bicepCurl = null;

        public static Exercise[] GetExercises()
        {
            shoulderAbduction = shoulderAbduction ?? ExerciseFactory.CreateRightShoulderAbductionExercise();
            bicepCurl = bicepCurl ?? ExerciseFactory.CreateRightBicepCurlExercise();

            return new Exercise[] { shoulderAbduction, bicepCurl };
        }

        private static Exercise CreateRightBicepCurlExercise()
        {
            Pose armAtSide = new Pose(new Dictionary<SharedContent.BodyPartID, BodyPartOrientation>()
            {
                {SharedContent.BodyPartID.RightArm, new LimbOrientation(SharedContent.BodyPartID.RightArm, new Vector3D(0,-1,0), new Vector3D(0,-1,0))}
            });
            Pose armCurledUp = new Pose(new Dictionary<SharedContent.BodyPartID, BodyPartOrientation>()
            {
                {SharedContent.BodyPartID.RightArm, new LimbOrientation(SharedContent.BodyPartID.RightArm, new Vector3D(0,-1,0), new Vector3D(0,0,-1))}
            });

            ExerciseStep curlArm = new ExerciseStep(armAtSide, armCurledUp, TimeSpan.FromSeconds(2), ExerciseStepType.Movement, "Curling arm");
            ExerciseStep uncurlArm = new ExerciseStep(armCurledUp, armAtSide, TimeSpan.FromSeconds(2), ExerciseStepType.Movement, "Uncurling arm");

            return new Exercise("Right Arm Bicep Curl", "From a neutral pose with your right arm at your side, bend your right forearm up directly in front of you until it is parallel with the floor, then lower your forearm back into neutral position",
                new Pose[] { armAtSide, armCurledUp }, new ExerciseStep[] { curlArm, uncurlArm });
        }

        /// <summary>
        /// Right Shoulder abduction - from shoulder at neutral position (at side), lift outwards (in plane of body) to approximately 90 degrees
        /// </summary>
        /// <returns></returns>
        private static Exercise CreateRightShoulderAbductionExercise()
        {
            Pose armAtSide = new Pose(new Dictionary<SharedContent.BodyPartID, BodyPartOrientation>()
            {
                {SharedContent.BodyPartID.RightArm, new LimbOrientation(SharedContent.BodyPartID.RightArm, new Vector3D(0,-1,0), new Vector3D(0,-1,0))}
            });
            Pose armOutstretched = new Pose(new Dictionary<SharedContent.BodyPartID, BodyPartOrientation>()
            {
                {SharedContent.BodyPartID.RightArm, new LimbOrientation(SharedContent.BodyPartID.RightArm, new Vector3D(1,0,0), new Vector3D(1,0,0))}
            });

            ExerciseStep raiseArm = new ExerciseStep(armAtSide, armOutstretched, TimeSpan.FromSeconds(3), ExerciseStepType.Movement, "Raising arm");
            ExerciseStep holdArmOutstretched = new ExerciseStep(armOutstretched, armOutstretched, TimeSpan.FromSeconds(2), ExerciseStepType.Hold, "Holding position");
            ExerciseStep lowerArm = new ExerciseStep(armOutstretched, armAtSide, TimeSpan.FromSeconds(3), ExerciseStepType.Movement, "Lowering arm");

            return new Exercise("Right Shoulder Abduction", "From a neutral position with your right arm at your side, raise your right arm to shoulder height directly out from your body, then lower your arm back into neutral position",
                new Pose[] { armAtSide, armOutstretched }, new ExerciseStep[] { raiseArm, holdArmOutstretched, lowerArm });
        }

        private static Exercise CreateExercise()
        {
            /*
             * Dan
             */
            return null;
        }
    }
}
