﻿//=====================================================================
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
using System.Collections.Generic;
using System.Linq;

namespace MUSICLibrary.MUSIC_Messages
{
    public class InteractionRecord : IPrototype
    {
        public HashSet<string> AvailableInteractions { get; set; }
       
        public InteractionRecord()
        {
            AvailableInteractions = new HashSet<string>();
        }

        public InteractionRecord(string csvInteractions)
        {
            AvailableInteractions = new HashSet<string>();
            AddInteractionsFromCsv(csvInteractions);
        }

        public InteractionRecord(HashSet<string> interactions)
        {
            AvailableInteractions = interactions;
        }

        public string ToCsvString()
        {
            return string.Join(",", AvailableInteractions.ToList());
        }

        public void AddInteractionsFromCsv(string csvString)
        {
            if (!string.IsNullOrEmpty(csvString))
                AvailableInteractions.UnionWith(csvString.Replace(" ", "").Split(','));
        }

        public object Clone()
        {
            return new InteractionRecord(new HashSet<string>(AvailableInteractions));
        }

        public override bool Equals(object obj)
        {
            if (obj is InteractionRecord other)
                return AvailableInteractions.SequenceEqual(other.AvailableInteractions);

            return false;
        }

        public override int GetHashCode()
        {
            return 1713996305 + EqualityComparer<HashSet<string>>.Default.GetHashCode(AvailableInteractions);
        }

        public static bool operator ==(InteractionRecord record1, InteractionRecord record2)
        {
            return EqualityComparer<InteractionRecord>.Default.Equals(record1, record2);
        }

        public static bool operator !=(InteractionRecord record1, InteractionRecord record2)
        {
            return !(record1 == record2);
        }
    }
}