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

using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages
{
    public class TransferConstructIDMessage : TargetedMUSICMessage, IMUSICCommandMessage
    {
        public EntityIDRecord OldID { get; set; }
        public EntityIDRecord NewID { get; set; }
        public uint CommandIdentifier => 454000002;

        public TransferConstructIDMessage(uint exerciseID, EntityIDRecord originID, EntityIDRecord recieverID,
            EntityIDRecord oldID, EntityIDRecord newID) : base(exerciseID, originID, recieverID)
        {
            OldID = oldID;
            NewID = newID;
        }

        public TransferConstructIDMessage(JObject obj) : base(obj)
        {
            OldID = obj[MUSICJsonKeys.OLD_ID].ToObject<EntityIDRecord>();
            NewID = obj[MUSICJsonKeys.NEW_ID].ToObject<EntityIDRecord>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitTransferConstructID(this);
        }

        public override object Clone()
        {
            return new TransferConstructIDMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    (EntityIDRecord)OldID.Clone(),
                    (EntityIDRecord)NewID.Clone()
                );
        }

        public override JObject ToJsonObject()
        {
            return new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.COMMAND_IDENTIFIER, CommandIdentifier },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.RECEIVER_ID, ReceiverID.ToJsonObject() },
                { MUSICJsonKeys.OLD_ID, OldID.ToJsonObject() },
                { MUSICJsonKeys.NEW_ID, NewID.ToJsonObject() }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is TransferConstructIDMessage other)
                if (base.Equals(other) && OldID == other.OldID && NewID == other.NewID)
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 122224770;

            hashCode = hashCode * -1127174295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1127174295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1127174295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1127174295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OldID);
            hashCode = hashCode * -1127174295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(NewID);

            return hashCode;
        }

        public static bool operator ==(TransferConstructIDMessage message1, TransferConstructIDMessage message2)
        {
            return EqualityComparer<TransferConstructIDMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(TransferConstructIDMessage message1, TransferConstructIDMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
