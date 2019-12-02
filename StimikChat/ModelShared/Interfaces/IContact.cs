using System;

namespace ModelShared.Interfaces
{
    public interface IContact
    {
        int UserId { get; set; }

        DateTime Created { get; set; }
    }
}
