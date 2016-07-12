using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{


    public class WorkOrder

    {
        public int idWorkorder { set; get; }
        public DateTime Date { set; get; }
        public long pauseTime { set; get; }
        public string usedMaterials { set; get; }
        public string description { set; get; }
        public int idEmployeeAssignment { set; get; }
        public bool isVerified { set; get; }


        public WorkOrder(bool isverified, int idemployeeassignment, int idworkorder, DateTime date, long pausetime = 0, string usedmaterials = "", string Description = "")
        {
            isverified = isVerified;
            idemployeeassignment = idEmployeeAssignment;
            idworkorder = idWorkorder;
            date = Date;
            pausetime = pauseTime;
            usedmaterials = usedMaterials;
            description = description;
           
         

        }
    }
}
