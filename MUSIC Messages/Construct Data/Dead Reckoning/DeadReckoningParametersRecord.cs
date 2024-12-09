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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.Construct_Data.Dead_Reckoning
{
    public class DeadReckoningParametersRecord : IToJSON, IPrototype
    {
        private const string ALGORITHM = "deadReckAlgorithm";
        private const string ANGULAR_VELOCITY = "angularVelocity";
        private const string LINEAR_ACCELERATION = "linearAcceleration";

        public IDeadReckoningAlgorithm Algorithm { get; set; }
        public MUSICVector3 AngularVelocity { get; set; }
        public MUSICVector3 LinearAcceleration { get; set; }

        private Type[] algorithmLookup = new Type[]
        {
            typeof(DeadReckoningOther),
            typeof(DeadReckoningStatic),
            typeof(DeadReckoningFPW),
            typeof(DeadReckoningRPW),
            typeof(DeadReckoningRVW),
            typeof(DeadReckoningFVW),
            typeof(DeadReckoningFPB),
            typeof(DeadReckoningRPB),
            typeof(DeadReckoningRVB),
            typeof(DeadReckoningFVB)
        };

        [JsonConstructor]
        public DeadReckoningParametersRecord(byte algorithmIndex, MUSICVector3 angularVelocity, MUSICVector3 linearAcceleration)
        {
            Algorithm = (IDeadReckoningAlgorithm)Activator.CreateInstance(algorithmLookup[algorithmIndex]);
            AngularVelocity = angularVelocity;
            LinearAcceleration = linearAcceleration;
        }

        public DeadReckoningParametersRecord(IDeadReckoningAlgorithm algorithm, MUSICVector3 angularVelocity, MUSICVector3 linearAcceleration)
        {
            Algorithm = algorithm ?? new DeadReckoningOther();
            AngularVelocity = angularVelocity;
            LinearAcceleration = linearAcceleration;
        }

        public DeadReckoningParametersRecord(JObject json)
        {
            if (json is null)
                return;

            Algorithm = (IDeadReckoningAlgorithm)Activator.CreateInstance(algorithmLookup[(int)json[ALGORITHM]]);
            AngularVelocity = new MUSICVector3(json[ANGULAR_VELOCITY] as JObject);
            LinearAcceleration = new MUSICVector3(json[LINEAR_ACCELERATION] as JObject);
        }

        public DeadReckoningParametersRecord()
        {
        }

        public void Update(DeadReckoningParametersRecord deadReckoningUpdate)
        {
            Algorithm = deadReckoningUpdate.Algorithm == null ? Algorithm : deadReckoningUpdate.Algorithm;
            AngularVelocity = deadReckoningUpdate.AngularVelocity == null ? AngularVelocity : deadReckoningUpdate.AngularVelocity;
            LinearAcceleration = deadReckoningUpdate.LinearAcceleration == null ? LinearAcceleration : deadReckoningUpdate.LinearAcceleration;
        }

        public JObject ToJsonObject()
        {
            JObject json = new JObject();

            json[ALGORITHM] = Algorithm?.GetAlgorithmIndex() ?? 0;
            json[ANGULAR_VELOCITY] = AngularVelocity.ToJsonObject();
            json[LINEAR_ACCELERATION] = LinearAcceleration.ToJsonObject();

            return json;
        }

        public override bool Equals(object obj)
        {
            var record = obj as DeadReckoningParametersRecord;

            return record != null &&
                   Algorithm?.GetType() == record.Algorithm?.GetType() &&
                   EqualityComparer<MUSICVector3>.Default.Equals(AngularVelocity, record.AngularVelocity) &&
                   EqualityComparer<MUSICVector3>.Default.Equals(LinearAcceleration, record.LinearAcceleration);
        }

        public override int GetHashCode()
        {
            var hashCode = -23087156;
            hashCode = hashCode * -1521134295 + Algorithm.GetType().GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<MUSICVector3>.Default.GetHashCode(AngularVelocity);
            hashCode = hashCode * -1521134295 + EqualityComparer<MUSICVector3>.Default.GetHashCode(LinearAcceleration);
            return hashCode;
        }

        public object Clone()
        {
            return new DeadReckoningParametersRecord(Algorithm, (MUSICVector3)AngularVelocity.Clone(), (MUSICVector3)LinearAcceleration.Clone());
        }

        public static bool operator ==(DeadReckoningParametersRecord record1, DeadReckoningParametersRecord record2)
        {
            return EqualityComparer<DeadReckoningParametersRecord>.Default.Equals(record1, record2);
        }

        public static bool operator !=(DeadReckoningParametersRecord record1, DeadReckoningParametersRecord record2)
        {
            return !(record1 == record2);
        }
    }
}
