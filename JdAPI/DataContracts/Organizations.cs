﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JdAPI.DataContracts
{
    [DataContract]
    public class Organizations
    {
        public  List<Organization> organizations;
    }
}
