using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Characters.Entity.Characters
{
    
    class CharacterManager
    {
        internal static List<Character> ListCharacter { get; set; }
        
        public static int NumberCharacters;
        public static Dictionary<SkillsEnum, int> sumSkill;
        public static Dictionary<SkillsEnum, int> maxSkill;
        public static Dictionary<SkillsEnum, int> minSkill;

        internal static void Init()
        {
            ListCharacter = new List<Character>();
            InitDict(ref sumSkill, 0);
            InitDict(ref maxSkill, 0);
            InitDict(ref minSkill, 1000);
            LoadCharacter();
        }

        private static void InitDict(ref Dictionary<SkillsEnum, int> dict, int value)
        {
            dict = new Dictionary<SkillsEnum, int>();
            dict.Add(SkillsEnum.Intelligence, value);
            dict.Add(SkillsEnum.Resistance, value);
            dict.Add(SkillsEnum.Strenght, value);
            dict.Add(SkillsEnum.Agility, value);
            dict.Add(SkillsEnum.Charm, value);
            dict.Add(SkillsEnum.Appeal, value);
        }

        private static void LoadCharacter()
        {
            for (int i = 0; i < 1; i++)
            {
                AddCharacter(GenderEnum.Male);
                AddCharacter(GenderEnum.Female);
            }

            for (int i = 0; i < Skills.NUMBER_OF_SKILLS; i++)
            {
                SkillsEnum skill = Skills.GetTypeSkill(i);
                minSkill[skill] = Skills.RecalculateMin(skill);
                maxSkill[skill] = Skills.RecalculateMax(skill);
            }

        }

        public static Character CreateCharacter(GenderEnum gender)
        {
            return AddCharacter(gender);
        }

        private static Character AddCharacter(GenderEnum gender)
        {
            Character c = new Character(gender);
            AddCharacter(c);
            return c;
        }

        private static void AddCharacter(Character character)
        {
            ListCharacter.Add(character);
            NumberCharacters++;
        }


        internal static string MaxSkillToString()
        {
            string ris = "";
            foreach (SkillsEnum skill in SkillsEnum.GetValues(typeof(SkillsEnum)))
            {
                ris += skill.ToString().Substring(0, 3) + ":" + maxSkill[skill] + " ";
            }
            return ris;
        }

        internal static string MinSkillToString()
        {
            string ris = "";
            foreach (SkillsEnum skill in SkillsEnum.GetValues(typeof(SkillsEnum)))
            {
                ris += skill.ToString().Substring(0, 3) + ":" + minSkill[skill] + " ";
            }
            return ris;
        }
    }
}
