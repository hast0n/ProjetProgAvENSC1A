using System;
using System.Collections.Generic;
using System.Text;

// TODO: Add JSON serializable fields to each class extending IDatabase

namespace ProjetProgAvENSC1A.Services
{
    interface IDataTable
    {
        public abstract bool AddEntry(object entry);
    }
}
