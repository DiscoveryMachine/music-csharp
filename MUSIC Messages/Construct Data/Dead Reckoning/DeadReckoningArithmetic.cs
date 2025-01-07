/*
 * Copyright (c) 2006-2011, Naval Postgraduate School, MOVES Institute All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * 
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * 
 * Neither the name of the Naval Postgraduate School, MOVES Institute, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY NPS AND CONTRIBUTORS ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS AND CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using MathNet.Numerics.LinearAlgebra;

namespace MUSICLibrary.MUSIC_Messages.Construct_Data.Dead_Reckoning
{
    // This is where the implementation below originated from, but altered for readability.
    // https://github.com/open-dis/open-dis-java/tree/4345d53b151369eb5ced4d2167c641413e7775c0/src/main/java/edu/nps/moves/deadreckoning
    public static class DeadReckoningArithmetic
    {
        public static MUSICVector3 CalculateLinearMotion(MUSICVector3 v0, MUSICVector3 v1, double deltaTime)
        {
           return new MUSICVector3(
                v0.X + v1.X * deltaTime,
                v0.Y + v1.Y * deltaTime,
                v0.Z + v1.Z * deltaTime
            );
        }

        public static MUSICVector3 CalculateUpdatedOrientation(PhysicalRecord physical, double deltaTime)
        {
            MUSICVector3 initialOrientation = (MUSICVector3)physical.Orientation.Clone();

            Matrix<double> deadReckoningMatrix = new DeadReckoningMatrixBuilder(physical, deltaTime).BuildRotationMatrix();

            Rotation updatedOrientation = new Rotation(deadReckoningMatrix.ToArray(), 1e-15).ApplyTo(new Rotation(initialOrientation));

            return updatedOrientation.GetEulerAngles();
        }

        public static MUSICVector3 CalculateLinearMotionWithAcceleration(PhysicalRecord physical, double deltaTime)
        {
            var result = CalculateLinearMotion(physical.Location, physical.LinearVelocity, deltaTime);

            var linearAcceleration = physical.DeadReckoningParameters.LinearAcceleration;
            result.X += 0.5 * linearAcceleration.X * deltaTime * deltaTime;
            result.Y += 0.5 * linearAcceleration.Y * deltaTime * deltaTime;
            result.Z += 0.5 * linearAcceleration.Z * deltaTime * deltaTime;

            return result;
        }
    }
}
