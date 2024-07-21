using System.ComponentModel.DataAnnotations;

namespace RL.Data.DataModels
{
    public class UserAssignPlanProcedure
    {
        [Key]
        public int UserProcedureId { get; set; }
        public int ProcedureId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }       
       
    }
}
