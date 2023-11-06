using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using CryptoSysPKI;

namespace EPCIS_XMLs_Generation
{
   public class UUIDGeneration
   {
       public static string Get_UUID()
       {
           return Guid.NewGuid().ToString();
       }

    }
}
