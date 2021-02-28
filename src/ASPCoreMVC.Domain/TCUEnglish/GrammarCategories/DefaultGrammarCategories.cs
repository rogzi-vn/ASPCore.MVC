using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPCoreMVC.TCUEnglish.GrammarCategories
{
    public static class DefaultGrammarCategories
    {
        public static List<GrammarCategory> GrammaraCategories = new List<GrammarCategory>
        {
            new GrammarCategory
            {
                Name = "Uncategorized"
            },
             new GrammarCategory
            {
                Name = "Aspect"
            },
              new GrammarCategory
            {
                Name = "Case"
            },
               new GrammarCategory
            {
                Name = "Definiteness"
            },
                new GrammarCategory
            {
                Name = "Mood And Modality"
            },
                 new GrammarCategory
            {
                Name = "Noun Class"
            },
                  new GrammarCategory
            {
                Name = "Number"
            },
                   new GrammarCategory
            {
                Name = "Polarity"
            },
                    new GrammarCategory
            {
                Name = "Tense"
            },
                     new GrammarCategory
            {
                Name = "Transitivity"
            },
                      new GrammarCategory
            {
                Name = "Voice"
            }
        };
    }
}
