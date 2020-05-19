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

        /// <summary>
        /// Return this student's current promotion
        /// </summary>
        public Promotion CurrentPromotion => (Promotion)Promotions.FirstOrDefault(entry =>
        {
            Promotion prom = (Promotion) entry;
            return prom.Students.Contains(this) && prom.isCurrentPromotion;
        });

        /// <summary>
        /// Returns promotions in which this student was involved
        /// </summary>
        public List<EntryType> Promotions => App.DB[DBTable.Promotion].Entries.Where(entry =>
        {
            Promotion p = (Promotion)entry;
            return p.Students.Contains(this);
        }).ToList();

        /// <summary>
        /// Return current year's projects for this student
        /// </summary>
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
