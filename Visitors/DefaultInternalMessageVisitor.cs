using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUSICLibrary.MUSIC_Messages;

namespace MUSICLibrary.Visitors
{
    class DefaultInternalMessageVisitor : IInternalMessageVisitor
    {
        public void Initialize(MUSICLibrary lib)
        {
            
        }

        public void VisitMUSICEvent(MUSICEventMessage msg)
        {
            throw new NotImplementedException();
        }

        public void VisitStateFieldMessage(StateFieldMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
