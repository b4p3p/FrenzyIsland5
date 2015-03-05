using UnityEngine;
using System.Collections;
using Assets.Engine.ComputerGraphics.Bezier;
using System.Collections.Generic;
using Assets.Engine.Other;

namespace Engine.Characters.Entity.Characters
{
    public class Character
    {

        #region FIELD

        

        #endregion FIELD

        #region PROPERTY

        public float Speed
        {
            get { return speed; }
            set { if (speed != value) speed = value; }
        }
        private float speed = 0;

        public GenderEnum Gender { get; set; }

        public Skills Skills { get; set; }

        #endregion PROPERTY

        #region CONSTRUCTOR

        // NEVER, NEVER and... NEVER use this function. Use CharacterManager.AddCharacter

        public Character(GenderEnum sex = GenderEnum.Male, Skills skills = null)
        {
            Gender = sex;
            if (skills == null)
                Skills = new Skills();
            else
                Skills = skills;
        }

        #endregion CONSTRUCTOR

    }
}

