using System;
using System.Collections.Generic;
using System.Text;

namespace KinAid_Attempt1
{
    public class VariableConstraint
    {
        LimbOrientation endingOrientation;
        int order;
        double timeout;

        int verify(LimbOrientation prevOrientation, LimbOrientation currOrientation);
    }
}
