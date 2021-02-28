using ASPCoreMVC.TCUEnglish.ExamSkillCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPCoreMVC.TCUEnglish.WordClasses
{
    public static class DefaultWordClasses
    {
        public static List<WordClass> WordClasses = new List<WordClass>
        {
            new WordClass
            {
                Name = "Verbs",
                About = "Verbs are action or state words like: run, work, study, be, seem",
            },
            new WordClass
            {
                Name = "Nouns",
                About = "Nouns are words for people, places or things like: mother, town, Rome, car, dog",
            },
            new WordClass
            {
                Name = "Adjectives",
                About = "Adjectives are words that describe nouns, like: kind, clever, expensive",
            },
            new WordClass
            {
                Name = "Adverbs",
                About = "Adverbs are words that modify verbs, adjectives or other adverbs, like: quickly, back, ever, badly, away generally, completely",
            },
            new WordClass
            {
                Name = "Prepositions",
                About = "Prepositions are words usually in front of a noun or pronoun and expressing a relation to another word or element, like: after, down, near, of, plus, round, to",
            },
            new WordClass
            {
                Name = "Pronouns",
                About = "Pronouns are words that take the place of nouns, like: me, you, his, it, this, that, mine, yours, who, what",
            },
            new WordClass
            {
                Name = "Interjections",
                About = "Interjections have no grammatical value - words like: ah, hey, oh, ouch, um, well",
            }
        };
    }
}
