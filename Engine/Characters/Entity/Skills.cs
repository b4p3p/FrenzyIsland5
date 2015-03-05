using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Engine.Characters.Entity.Characters
{

    public class Skills
    {
        public const int NUMBER_OF_SKILLS = 6;

        private Dictionary<SkillsEnum, int> valueSkill;

        public Skills()
        {
            int valueDefault = 10;
            
            valueSkill = new Dictionary<SkillsEnum, int>();
            SetSkill(SkillsEnum.Intelligence, valueDefault);
            SetSkill(SkillsEnum.Resistance, valueDefault);
            SetSkill(SkillsEnum.Strenght, valueDefault);
            SetSkill(SkillsEnum.Agility, valueDefault);
            SetSkill(SkillsEnum.Charm, valueDefault);
            SetSkill(SkillsEnum.Appeal, valueDefault);
        }

        public Skills(int Int, int Res, int Str, int Agl, int Cha, int App)
        {
            valueSkill = new Dictionary<SkillsEnum, int>();
            SetSkill(SkillsEnum.Intelligence, Int);
            SetSkill(SkillsEnum.Resistance, Res);
            SetSkill(SkillsEnum.Strenght, Str);
            SetSkill(SkillsEnum.Agility, Agl);
            SetSkill(SkillsEnum.Charm, Cha);
            SetSkill(SkillsEnum.Appeal, App);
        }

        public void SetSkill(SkillsEnum skill, int value)
        {
            int prec = value;
            
            if ( valueSkill.ContainsKey(skill) == false)
            {
                valueSkill.Add(skill, value);
                CharacterManager.sumSkill[skill] += value;
            }
            else
            {
                prec = valueSkill[skill];
                CharacterManager.sumSkill[skill] -= prec;

                valueSkill[skill] = value;
                CharacterManager.sumSkill[skill] += value;

            }

            if (prec == CharacterManager.minSkill[skill] || value == CharacterManager.minSkill[skill])
            {
                CharacterManager.minSkill[skill] = RecalculateMin(skill);
            }
            if (prec == CharacterManager.maxSkill[skill] || value == CharacterManager.minSkill[skill])
            {
                CharacterManager.maxSkill[skill] = RecalculateMax(skill);
            }

        }

        internal static int RecalculateMax(SkillsEnum skill)
        {
            int ris = 0;
            foreach (Character c in CharacterManager.ListCharacter)
            {
                int value = c.Skills.GetValue(skill); 
                if (value > ris)
                    ris = value;
            }
            return ris;
        }

        internal static int RecalculateMin(SkillsEnum skill)
        {
            int ris = 1000;
            foreach (Character c in CharacterManager.ListCharacter)
            {
                int value = c.Skills.GetValue(skill);
                if (value < ris)
                    ris = value;
            }
            return ris;
        }

        internal float ToPercentage(SkillsEnum skill)
        {
            float delta = CharacterManager.maxSkill[skill] - CharacterManager.minSkill[skill];
            float delta_value = CharacterManager.maxSkill[skill] - valueSkill[skill];

            if (delta == 0 ) return 0.5f;

            return 1 - delta_value / delta;
        }

        internal float GetAVGSkills()
        {
            float sum = ToPercentage(SkillsEnum.Intelligence) +
                        ToPercentage(SkillsEnum.Agility) +
                        ToPercentage(SkillsEnum.Appeal) +
                        ToPercentage(SkillsEnum.Charm) +
                        ToPercentage(SkillsEnum.Resistance) +
                        ToPercentage(SkillsEnum.Strenght);
            return sum / Skills.NUMBER_OF_SKILLS;
        }

        internal int GetValue(SkillsEnum skill)
        {
            return valueSkill[skill];
        }

        public static SkillsEnum GetTypeSkill(int index)
        {
            switch (index)
            {
                case 0: return SkillsEnum.Intelligence;
                case 1: return SkillsEnum.Resistance;
                case 2: return SkillsEnum.Strenght;
                case 3: return SkillsEnum.Agility;
                case 4: return SkillsEnum.Charm;
                case 5: return SkillsEnum.Appeal;
            }
            throw new IndexOutOfRangeException("GetTypeSkill");
        }

        public override string ToString()
        {
            string ris = "";
            foreach (SkillsEnum skill in SkillsEnum.GetValues(typeof(SkillsEnum)))
            {
                ris += skill.ToString().Substring(0, 3) + ": " + GetValue(skill) + " ";
            }
            return ris;
        }

    }
}
