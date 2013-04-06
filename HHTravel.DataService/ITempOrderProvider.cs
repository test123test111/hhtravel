using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.DataService
{
    public interface ITempOrderProvider
    {
        void AddOrUpdate(Guid sessionId, string content);

        string GetContent(Guid sessionId);
    }
}
