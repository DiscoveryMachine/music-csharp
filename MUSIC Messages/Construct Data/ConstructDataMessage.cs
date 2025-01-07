/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.Construct_Data
{
    public class ConstructDataMessage : MUSICMessage
    {
        public string Callsign { get; set; }
        public string Name { get; set; }

        public EntityIDRecord GhostedID { get; set; }
        public EntityIDRecord PrimaryControllerID { get; set; }
        public EntityIDRecord CurrentControllerID { get; set; }

        public ConstructRender ConstructRender { get; set; }
        public ConstructType ConstructType { get; set; }

        public InteractionRecord Interactions { get; set; }
        public PhysicalRecord Physical { get; set; }

        public ConstructDataMessage() : base()
        {

        }

        public ConstructDataMessage(JObject json)
            : base(json.GetValue(MUSICJsonKeys.HEADER).ToObject<MUSICHeader>(),
                  json.GetValue(MUSICJsonKeys.ORIGIN_ID).ToObject<EntityIDRecord>())
        {
            GhostedID = json[MUSICJsonKeys.GHOSTED_ID]?.ToObject<EntityIDRecord>();

            PrimaryControllerID = json[MUSICJsonKeys.PRIMARY_CONTROLLER_ID]?.ToObject<EntityIDRecord>();

            CurrentControllerID = json[MUSICJsonKeys.CURRENT_CONTROLLER_ID]?.ToObject<EntityIDRecord>();

            ConstructRender = (ConstructRender)(int)json[MUSICJsonKeys.CONSTRUCT_INFO_RECORD][MUSICJsonKeys.CONSTRUCT_RENDER];

            ConstructType = (ConstructType)(int)json[MUSICJsonKeys.CONSTRUCT_INFO_RECORD][MUSICJsonKeys.CONSTRUCT_TYPE];

            Physical = HasPhysicalData(json) ? new PhysicalRecord(json) : null;

            Callsign = (string)json[MUSICJsonKeys.CALLSIGN];

            Name = (string)json[MUSICJsonKeys.CONSTRUCT_NAME];

            Interactions = new InteractionRecord();

            Interactions.AddInteractionsFromCsv((string)json[MUSICJsonKeys.INTERACTION_RECORD]);
        }

        private bool HasPhysicalData(JObject json)
        {
            return
                json.ContainsKey(MUSICJsonKeys.LOCATION) ||
                json.ContainsKey(MUSICJsonKeys.ORIENTATION) ||
                json.ContainsKey(MUSICJsonKeys.VELOCITY) ||
                json.ContainsKey(MUSICJsonKeys.ENTITY_TYPE) ||
                json.ContainsKey(MUSICJsonKeys.FORCE) ||
                json.ContainsKey(MUSICJsonKeys.DEAD_RECK) ||
                json.ContainsKey(MUSICJsonKeys.DAMAGE_RECORD);
        }

        public void Update(ConstructDataMessage message)
        {
            Callsign = message.Callsign is null ? Callsign : message.Callsign;

            Name = message.Name is null ? Name : message.Name;

            GhostedID = message.GhostedID is null ? GhostedID : message.GhostedID;

            PrimaryControllerID = message.PrimaryControllerID is null ? PrimaryControllerID : message.PrimaryControllerID;

            CurrentControllerID = message.CurrentControllerID is null ? CurrentControllerID : message.CurrentControllerID;

            ConstructRender = message.ConstructRender;

            ConstructType = message.ConstructType;

            Interactions = message.Interactions is null ? Interactions : message.Interactions;

            if (Physical == null && message.Physical != null)
                Physical = new PhysicalRecord();

            if (Physical != null)
                Physical.Update(message.Physical);
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitConstructData(this);
        }

        public override object Clone()
        {
            return new ConstructDataMessage()
            {
                MUSICHeader = new MUSICHeader(MUSICHeader.ExerciseID),
                OriginID = (EntityIDRecord)OriginID.Clone(),
                Callsign = string.Copy(Callsign),
                Name = string.Copy(Name),
                GhostedID = (EntityIDRecord)GhostedID?.Clone(),
                PrimaryControllerID = (EntityIDRecord)PrimaryControllerID?.Clone(),
                CurrentControllerID = (EntityIDRecord)CurrentControllerID?.Clone(),
                ConstructRender = ConstructRender,
                ConstructType = ConstructType,
                Interactions = (InteractionRecord)Interactions?.Clone(),
                Physical = (PhysicalRecord)Physical?.Clone()
            };
        }

        public override JObject ToJsonObject()
        {
            JObject obj = new JObject();
            obj.Add(MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject());
            obj.Add(MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject());

            if (GhostedID != null)
                obj.Add(MUSICJsonKeys.GHOSTED_ID, GhostedID.ToJsonObject());

            if (PrimaryControllerID != null)
                obj.Add(MUSICJsonKeys.PRIMARY_CONTROLLER_ID, PrimaryControllerID.ToJsonObject());
            else
                obj.Add(MUSICJsonKeys.PRIMARY_CONTROLLER_ID, OriginID.ToJsonObject());

            if (CurrentControllerID != null)
                obj.Add(MUSICJsonKeys.CURRENT_CONTROLLER_ID, CurrentControllerID.ToJsonObject());
            else
                obj.Add(MUSICJsonKeys.CURRENT_CONTROLLER_ID, OriginID.ToJsonObject() );

            JObject constructInformationRecord = new JObject();
            constructInformationRecord.Add(MUSICJsonKeys.CONSTRUCT_RENDER, (uint)ConstructRender);
            constructInformationRecord.Add(MUSICJsonKeys.CONSTRUCT_TYPE, (uint)ConstructType);
            obj.Add(MUSICJsonKeys.CONSTRUCT_INFO_RECORD, constructInformationRecord);

            if (Physical != null)
                obj.Add(Physical.ToJsonObject().Children());

            obj.Add(MUSICJsonKeys.CALLSIGN, Callsign);
            obj.Add(MUSICJsonKeys.CONSTRUCT_NAME, Name);

            if (Interactions != null)
                obj.Add(MUSICJsonKeys.INTERACTION_RECORD, Interactions.ToCsvString());
            else
                obj.Add(MUSICJsonKeys.INTERACTION_RECORD, "");

            return obj;
        }

        public override bool Equals(object obj)
        {
            var message = obj as ConstructDataMessage;
            return message != null &&
                   base.Equals(obj) &&
                   Callsign == message.Callsign &&
                   Name == message.Name &&
                   EqualityComparer<EntityIDRecord>.Default.Equals(GhostedID, message.GhostedID) &&
                   EqualityComparer<EntityIDRecord>.Default.Equals(PrimaryControllerID, message.PrimaryControllerID) &&
                   EqualityComparer<EntityIDRecord>.Default.Equals(CurrentControllerID, message.CurrentControllerID) &&
                   ConstructRender == message.ConstructRender &&
                   ConstructType == message.ConstructType &&
                   EqualityComparer<InteractionRecord>.Default.Equals(Interactions, message.Interactions) &&
                   EqualityComparer<PhysicalRecord>.Default.Equals(Physical, message.Physical);
        }

        public override int GetHashCode()
        {
            var hashCode = 1009759711;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Callsign);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(GhostedID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(PrimaryControllerID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(CurrentControllerID);
            hashCode = hashCode * -1521134295 + ConstructRender.GetHashCode();
            hashCode = hashCode * -1521134295 + ConstructType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<InteractionRecord>.Default.GetHashCode(Interactions);
            hashCode = hashCode * -1521134295 + EqualityComparer<PhysicalRecord>.Default.GetHashCode(Physical);
            return hashCode;
        }

        public static bool operator ==(ConstructDataMessage message1, ConstructDataMessage message2)
        {
            return EqualityComparer<ConstructDataMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(ConstructDataMessage message1, ConstructDataMessage message2)
        {
            return !(message1 == message2);
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
