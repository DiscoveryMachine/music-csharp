//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms  of
// that agreement.
//
// Copyright 2023 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using MUSICLibrary.Interfaces;

namespace MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository
{
    public class RepositoryConfiguration
    {
        public IConstructBatcher LocalBatcher { get; set; }
        public IConstructBatcher RemoteBatcher { get; set; }
        public uint LocalBatchCount { get; set; }
        public uint RemoteBatchCount { get; set; }
        public uint LocalBatchInterval { get; set; }
        public uint RemoteBatchInterval { get; set; }
    }
}
