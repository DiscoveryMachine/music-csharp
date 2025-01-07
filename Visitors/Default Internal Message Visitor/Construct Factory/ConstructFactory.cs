/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Construct_Data;

namespace MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Factory
{
    public class ConstructFactory : IConstructFactory
    {
        protected Dictionary<string, Type> RegisteredLocalConstructs { get; set; }
        protected Dictionary<string, Type> RegisteredRemoteConstructs { get; set; }
        public IMUSICTransmitter Transmitter { get; private set; }
        public IConstructRepository Repository { get; private set; }

        protected const string CREATE_ERROR_MSG_FORMAT = 
            "Cannot create construct of type {{{0}}} because it is not registered. " +
            "The most common cause of this exception is registering a local construct and" +
            " attempting to create a remote construct with a ConstructDataMessage or vice versa.";

        protected const string INCOMPLETE_MESSAGE_FORMAT = 
            "ConstructFactory.Create({0}) could not create a construct" +
            " because the message was incomplete. Missing required field: {1}";

        protected const string INVALID_ENTITY_ID_MESSAGE_FORMAT = 
            "Construct {{{0}}} from Site:{{{1}}}, App:{{{2}}} has an invalid entity ID of 0. " +
            "Entity IDs of 0 are reserved to reference the simulation at the site and app ID. Construct entity IDs must be greater than 0.";

        protected const string CONSTRUCT_DATA_PARAM = "ConstructDataMessage constructData";

        protected const string CREATE_ERROR_MESSAGE_FORMAT = 
            "[Warning]: Attempted to create a construct of type {0}, but a MissingMethodException was thrown. " +
            "ConstructFactory.Create has returned a null construct.\n" +
            "Typical causes of this exception are:\n{1}" +
            "\n";

        protected const string CREATE_ERROR_MESSAGE_CAUSE1 =
            "A local construct was registered as a remote construct, then when the factory tries to instantiate " +
            "the local construct as a remote construct, there is no constructor present in the local construct that matches the remote construct by design.\n";

        protected const string CREATE_ERROR_MESSAGE_CAUSE2 = "The given remote construct does not contain a constructor which matches the signature of: " +
            "MyRemoteConstruct(ConstructDataMessage constructData)\n";

        protected uint nextEntityID;

        public ConstructFactory(IMUSICTransmitter transmitter, IConstructRepository repository)
        {
            RegisteredLocalConstructs = new Dictionary<string, Type>();
            RegisteredRemoteConstructs = new Dictionary<string, Type>();
            Repository = repository;
            Transmitter = transmitter;
            nextEntityID = 1;
        }

        /// <summary>
        /// Creates a new remote construct.
        /// Throws:
        ///   ArgumentNullException when the construct's fully qualified name or origin ID are null.
        ///   InvalidOperationException when a construct's fully qualified name is not registered with its type.
        /// </summary>
        /// <param name="constructData">The construct data message received from an endpoint.</param>
        /// <returns>A new construct of the type associated with the ConstructDataMessage's fully qualified name.</returns>
        public IConstruct Create(ConstructDataMessage constructData)
        {
            if (constructData.Name == null)
                throw new ArgumentNullException(string.Format(INCOMPLETE_MESSAGE_FORMAT, CONSTRUCT_DATA_PARAM, "constructData.name"));

            if (constructData.OriginID == null)
                throw new ArgumentNullException(string.Format(INCOMPLETE_MESSAGE_FORMAT, CONSTRUCT_DATA_PARAM, "constructData.OriginID"));

            if (constructData.OriginID.EntityID == 0)
                throw new ArgumentException(string.Format(INVALID_ENTITY_ID_MESSAGE_FORMAT, constructData.Name, constructData.OriginID.SiteID, constructData.OriginID.AppID));

            if (!RegisteredRemoteConstructs.ContainsKey(constructData.Name))
                throw new InvalidOperationException(string.Format(CREATE_ERROR_MSG_FORMAT, constructData.Name));

            try
            {
                return (IConstruct)Activator.CreateInstance(RegisteredRemoteConstructs[constructData.Name], constructData, Transmitter, Repository);
            }
            catch (MissingMethodException e)
            {
                Console.Error.WriteLine(e.StackTrace);
                Console.Error.WriteLine(string.Format(CREATE_ERROR_MESSAGE_FORMAT, constructData.Name,
                    CREATE_ERROR_MESSAGE_CAUSE1 + CREATE_ERROR_MESSAGE_CAUSE2)); //TODO: Include the correct method signature in the msg.
                throw e;
            }
        }

        /// <summary>
        /// Creates a new local construct.
        /// </summary>
        /// <param name="createInfo">The minimum amount of information needed to create a new construct.</param>
        /// <returns>A new construct of the type associated with the ConstructCreateInfo's fully qualified name.</returns>
        virtual public IConstruct Create(IConstructCreateInfo createInfo)
        {
            if (!RegisteredLocalConstructs.ContainsKey(createInfo.QualifiedName))
                throw new InvalidOperationException(string.Format(CREATE_ERROR_MSG_FORMAT, createInfo.QualifiedName));
            createInfo.Transmitter = Transmitter;
            createInfo.ConstructID = new EntityIDRecord(createInfo.Transmitter.GetSiteAndAppID(), GetNextAndAdvanceEntityID());
            createInfo.Repository = Repository;
            try
            {
                return (IConstruct)Activator.CreateInstance(RegisteredLocalConstructs[createInfo.QualifiedName], createInfo);
            }
            catch (MissingMethodException)
            {
                Console.Out.WriteLine(string.Format(CREATE_ERROR_MESSAGE_FORMAT, createInfo.QualifiedName,
                    CREATE_ERROR_MESSAGE_CAUSE1 + CREATE_ERROR_MESSAGE_CAUSE2));
                return null;
            }
        }

        virtual public void RegisterLocalConstruct(IConstructCreateInfo createInfo)
        {
            if (!RegisteredLocalConstructs.ContainsKey(createInfo.QualifiedName))
                RegisteredLocalConstructs.Add(createInfo.QualifiedName, createInfo.ConstructType);
        }

        virtual public void RegisterRemoteConstruct(IConstructCreateInfo createInfo)
        {
            RegisteredRemoteConstructs.Add(createInfo.QualifiedName, createInfo.ConstructType);
        }

        public bool IsConstructRegistered(string fullyQualifiedName)
        {
            return RegisteredLocalConstructs.ContainsKey(fullyQualifiedName) || RegisteredRemoteConstructs.ContainsKey(fullyQualifiedName);
        }

        public uint GetNextAndAdvanceEntityID()
        {
            uint current = nextEntityID;
            nextEntityID++;
            return current;
        }
    }
}
