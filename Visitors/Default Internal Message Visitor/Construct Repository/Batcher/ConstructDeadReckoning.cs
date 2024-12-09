//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms of
// that agreement.
//
// Copyright 2022-23 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages;

namespace MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository.Batcher
{
    public class ConstructDeadReckoning : IBatchOperation
    {
        //private const double FPS = 30;

        public void ApplyTo(Dictionary<EntityIDRecord, IConstruct> batch)
        {
            //foreach(IConstruct construct in batch.Values)
            //{
            //    //if construct is local
            //    if (!construct.IsRemote())
            //    {
            //        throw new InvalidOperationException();
            //    }

            //    ConstructDataMessage ConstructDataMsg = construct.GetConstructData();
            //    if (ConstructDataMsg.Physical != null && ConstructDataMsg.Physical.DeadReckoningParameters != null)
            //    {
            //        IDeadReckoningAlgorithm algo = ConstructDataMsg.Physical.DeadReckoningParameters.GetNewAlgorithm();
            //        if (algo != null)
            //            algo.ApplyDeadReckoning(ConstructDataMsg.Physical, (float)(1.0 / FPS));
            //    }
            //}
        }
    }
}
