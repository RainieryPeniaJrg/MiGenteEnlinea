using MiGente_Front.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiGente_Front.Services
{
    public class BotServices
    {
        public OpenAi_Config getOpenAI()
        {
            using (var db = new migenteEntities())
            {
                return  db.OpenAi_Config.FirstOrDefault();

            }
        }
    }
}