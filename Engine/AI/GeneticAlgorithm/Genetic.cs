using Assets.Engine.Other;
using Engine.Characters.Entity;
using Engine.Characters.Entity.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Engine.AI.GeneticAlgorithm
{
    class Genetic
    {
        public static Character Couple( Character A , Character B, GeneticVariables variable)
        {
            Character newCharacter = CreateGenderCharacter(); 
            
            CrossOver(A, B, newCharacter, variable);
            Mutation(newCharacter, variable);

            return newCharacter;
        }

        private static Character CreateGenderCharacter()
        {
            Character c;
            bool coin = Randomize.Coin();
            //Debug.Log("Coin: " + coin);
            if (coin)
                c = CharacterManager.CreateCharacter( GenderEnum.Male );
            else
                c = CharacterManager.CreateCharacter( GenderEnum.Female );
            return c;
        }

        private static void CrossOver(Character A, Character B, Character character , GeneticVariables variable)
        {
            // mixingRatio = 0    A -> character
            // mixingRatio = 1    B -> character

            float mixingRatio = variable.MixingRatio;
            float dice;
            int valueSkill;
            
            string debug = "Mixingratio: " + mixingRatio + " Dice crossover: ";
            for (int i = 0; i < Skills.NUMBER_OF_SKILLS; i++)
            {
                SkillsEnum mixingSkill = Skills.GetTypeSkill(i);
                dice = Randomize.NextFloat(0, 1);

                debug += dice + " ";

                if (dice > mixingRatio)
                    valueSkill = A.Skills.GetValue(mixingSkill);    //mother
                else
                    valueSkill = B.Skills.GetValue(mixingSkill);    //father
                
                character.Skills.SetSkill(mixingSkill, valueSkill);
            }

            //Debug.Log(debug);
        }

        private static void Mutation(Character newCharacter, GeneticVariables variable)
        {
            float dice;
            float percentage = variable.PercentageMutation;
            
            string debug_dice = "Percentage: " + percentage + " ";
            string debug_mutation = "ValueMutation[" + variable.MinMutation + " " + variable.MaxMutation + "]: ";
            string debug_value = "ValueSkill: ";

            for (int i = 0; i < Skills.NUMBER_OF_SKILLS; i++)
            {
                SkillsEnum skill = Skills.GetTypeSkill(i);
                dice = Randomize.Next();
                
                debug_dice += dice + " ";

                if ( dice < percentage )
                {
                    int value = newCharacter.Skills.GetValue(skill);
                    int mutation = Randomize.NextInt(variable.MinMutation, variable.MaxMutation);

                    debug_mutation += mutation + " # ";
                    debug_value += value;

                    if ( mutation != 0)
                        newCharacter.Skills.SetSkill(skill, mutation + value);
                }
            }

            //Debug.Log(debug_dice);
            Debug.Log(debug_mutation);
        }

        
    }
}
