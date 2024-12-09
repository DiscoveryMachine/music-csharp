//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms  of
// that agreement.
//
// Copyright 2022-23 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using MUSICLibrary.Interfaces;
using MUSICLibrary.MessageFilters;
using MUSICLibrary.Visitors;
using MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Factory;
using MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository;

namespace MUSICLibrary
{
    public class DefaultMUSICLibraryBuilder : IMUSICLibraryBuilder
    {
        public IMUSICLibrary Build(LibraryConfiguration config)
        {
            //TODO: When testing the batching system using this builder, uncomment the below code as tests are written.
            //config.RepositoryConfig.LocalBatcher = new DefaultConstructBatcher(config.RepositoryConfig.LocalBatchInterval);
            //config.RepositoryConfig.RemoteBatcher = new DefaultConstructBatcher(config.RepositoryConfig.RemoteBatchInterval);

            config.MessageFilter = new DefaultMessageFilter(config);
            MUSIC lib = new MUSIC(config);

            ConstructRepository repository = new ConstructRepository(lib, config.RepositoryConfig);
            ConstructFactory factory = new ConstructFactory(lib, repository);

            DefaultInternalMessageVisitor visitor = new DefaultInternalMessageVisitor(lib, config);
            visitor.Initialize(factory, repository);

            config.InternalMessageVisitor = visitor;

            return lib;
        }
    }
}
