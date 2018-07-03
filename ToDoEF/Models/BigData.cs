using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoEF.Models
{
    public class BigData
    {
        ModelContext db; 
        public BigData(ModelContext context)
        {
            db = context;            
           var  query = from l in db.TablelistSet
                        join lg in db.TablelistTablegroup
                        on l.Id equals lg.TablelistId
                        join g in db.TablegroupSet
                        on lg.TablegroupId equals g.Id
                        select new
                        {
                            Nameoftask = l.TaskName,
                            DateStart = l.DateStart,
                            DateEnd = l.DateEnd,
                            Groups = g.Name
                        };
        }
    }
}
