﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetProgAvENSC1A.Services
{
    interface IDatabase
    {
        public abstract bool AddEntry(object entry);
    }
}