﻿/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace MUSICLibrary.MUSIC_Messages.Construct_Data.Dead_Reckoning
{
    public class DeadReckoningOther : IDeadReckoningAlgorithm
    {
        public void ApplyDeadReckoning(PhysicalRecord record, double secondsToProject)
        {
        }

        public byte GetAlgorithmIndex()
        {
            return 0;
        }
    }
}
