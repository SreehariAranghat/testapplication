using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUOffLoad
{
    public enum ImportType
    {
        Regular
        ,Consolidated
    }

    public class MarksCardCreator
    {
       static  MUPRJEntities entities = new MUPRJEntities();

        public static void CreateMarksCard(string registernumber,ImportType type)
        {
             
        }

        public void ImportData()
        {

        }
    }
}