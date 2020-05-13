using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetProgAvENSC1A.Services;

namespace ProjetProgAvENSC1A.Models
{
    class Student : Person 
    {
        public string Student_ID { get; set; }

        public Promotion CurrentPromotion => (Promotion) Promotions.FirstOrDefault(entry =>
        {
            Promotion prom = (Promotion) entry;
            return prom.isCurrentPromotion;
        });

        public List<EntryType> Promotions => App.DB[DBTable.Promotion].Entries.Where(entry =>
        {
            Promotion p = (Promotion)entry;
            return p.Students.Contains(this);
        }).ToList();

        public List<EntryType> CurrentPromotionProjects => App.DB[DBTable.Project].Entries.Where(entry =>
        {
            Project p = (Project)entry;

            if (p.Contributors.ContainsValue(this))
            {
                return p.Promotions.Any(prom => prom.isCurrentPromotion);
            }

            return false;
        }).ToList();
    }
}
