using MiGente_Front.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiGente_Front.Services
{
    public class EmailService
    {
        migenteEntities db = new migenteEntities();
        public Config_Correo Config_Correo()
        {
            return db.Config_Correo.FirstOrDefault();
        }

    }
}