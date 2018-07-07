using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoEF.Models
{
    public  class Tablelist
    {
            public int Id { get; set; }
            public string TaskName { get; set; }
            public System.DateTime? DateStart { get; set; }
            public System.DateTime? DateEnd { get; set; }        
      
            public  ICollection<Tablelistgroup> Tablelistgroups { get; set; }        
        }
        public  class Tablegroup
        {       
          
        public int Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public  ICollection<Tablelistgroup> Tablelistgroups { get; set; }

        
    }
    
    public  class Tablelistgroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TablelistId { get; set; }
        public Tablelist Tablelist { get; set; }
        public int TablegroupId { get; set; }
        public Tablegroup Tablegroup { get; set; }        

    }
    public class ViewModel
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Groups { get; set; }
        public int?  TablelistId { get; set; }
        public int? TablelgroupId { get; set; }        
        //   public Tablegroup Tablegroup { get; set;}
        //   public ICollection<Tablelistgroup> Tablelistgroups { get; set; }
        //   public Tablelist Tablelist { get; set; }
        //   public ICollection<Tablelist> Tablelists { get; set; }

        public ViewModel(int _id, string _nameOfTask, DateTime? _dateStart, DateTime? _dateEnd, string _groups,int? _TablelistId,int? _TablelgroupId)
        {
            TaskName = _nameOfTask;
            DateStart = _dateStart;
            DateEnd = _dateEnd;
            Groups = _groups;
            Id = _id;
            TablelistId= _TablelistId;
            TablelgroupId= _TablelgroupId;            
        }
        
}

    public enum Gender
    {
        Male,
        Female
    }
}
