using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Factory
{
    class ConstructCreateInfo : IConstructCreateInfo
    {
        public string QualifiedName { get; set; }
        public string Callsign { get; set; }
        public EntityIDRecord ConstructID { get; set; }
        public Type ConstructType { get; set; }
        public IConstructRepository Repository { get; set; }
        public IMUSICTransmitter Transmitter { get; set; }
        public MUSICVector3 ConstructLocation { get; set; }
        public MUSICVector3 ConstructOrientation { get; set; }
    }
}
