using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ToDoEF.Models
{
    public static class DbInitializer
    {
        public static void Initialize(ModelContext context)
        {
            context.Database.EnsureCreated();

           
            if (context.TablelistSet.Any())
            {
                return;   // DB has been seeded
            }
            var TaskNames = new Tablelist[]
            {
                new Tablelist{TaskName="Задача №1", DateStart=DateTime.Parse("2015-09-01"), DateEnd=DateTime.Parse("2018-09-01")},
                new Tablelist{TaskName="Задача №2", DateStart=DateTime.Parse("2015-09-01"), DateEnd=DateTime.Parse("2018-09-01")},
                new Tablelist{TaskName="Задача №3", DateStart=DateTime.Parse("2015-09-01"), DateEnd=DateTime.Parse("2018-09-01")}
            };
            foreach (Tablelist s in TaskNames)
            {
                context.TablelistSet.Add(s);
            }
            context.SaveChanges();


            for (int i = 0; i < 10; i++)
                context.TablegroupSet.Add(new Tablegroup { Name = $"Группа {i}" });

            context.SaveChanges();
            List<int> listidgroup = new List<int>();
            foreach (var model in context.TablegroupSet.ToList())
            {
              listidgroup.Add(model.Id);
            }
            List<int> listidtask = new List<int>();
            foreach (var model in context.TablelistSet.ToList())
            {
                listidtask.Add(model.Id);
            }
            int id(int max)
            {
                Random number = new Random();        
                int i= number.Next(0, max);               
                return i;
            }
            var connect = new Tablelistgroup[]
            {
                new Tablelistgroup{TablegroupId=listidgroup[id(listidgroup.Count)],TablelistId=listidtask[id(listidtask.Count)]},
                new Tablelistgroup{TablegroupId=listidgroup[id(listidgroup.Count)],TablelistId=listidtask[id(listidtask.Count)]},
                new Tablelistgroup{TablegroupId=listidgroup[id(listidgroup.Count)],TablelistId=listidtask[id(listidtask.Count)]},
                new Tablelistgroup{TablegroupId=listidgroup[id(listidgroup.Count)],TablelistId=listidtask[id(listidtask.Count)]},
                new Tablelistgroup{TablegroupId=listidgroup[id(listidgroup.Count)],TablelistId=listidtask[id(listidtask.Count)]},
                new Tablelistgroup{TablegroupId=listidgroup[id(listidgroup.Count)],TablelistId=listidtask[id(listidtask.Count)]},
                new Tablelistgroup{TablegroupId=listidgroup[id(listidgroup.Count)],TablelistId=listidtask[id(listidtask.Count)]}

            };
            foreach (Tablelistgroup s in connect)
            {
                context.TablelistTablegroup.Add(s);
            }
            context.SaveChanges();

           
        }
    }
}
       
       
   

