using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Engine.Characters.Entity;
using Assets.Script;
using Engine.ComputerGraphics.Component;

namespace Engine.Characters.Entity.Characters
{
    public class PeopleController : MonoBehaviour
    {
        
        public IslandBehaviour Island
        {
            get
            {
                if (_island != null) return _island;
                _island = GameObject.Find(Tags.Terrain).GetComponent<IslandBehaviour>();
                return _island;
            }
        }
        private IslandBehaviour _island = null;

        public GameObject CharacterContainer
        {
            get
            {
                if (_characterContainer != null) return _characterContainer;
                _characterContainer = GameObject.FindGameObjectWithTag(Tags.CharacterContainer);
                return _characterContainer;
            }
        }
        private GameObject _characterContainer = null;

        void Start()
        {
            CharacterManager.Init();

            List<Character> ListCharacter = CharacterManager.ListCharacter;
            int cont = 0;
            foreach (Character item in ListCharacter)
            {
                AddModelCharacter(item, cont);
                cont++;
            }

        }

        private void AddModelCharacter(Character item, int numberCharacter)
        {
            GenderEnum sex = item.Gender;
            GameObject model;
            string name;
            if (sex == GenderEnum.Male)
            {
                model = Resources.Load("Character/vincent") as GameObject;
                name = "M";
            }
            else
            {
                model = Resources.Load("Character/mia") as GameObject;
                name = "F";
            }

            GameObject c_gameObject = Instantiate(model, GetAvaiblePosition(new Vector3(250, 0, 250)), Quaternion.identity) as GameObject;
            c_gameObject.name = name + numberCharacter;
            c_gameObject.transform.parent = CharacterContainer.transform;
        }

        void Update()
        {

        }
        
        private Vector3 GetAvaiblePosition(Vector3 center)
        {
            return GetAvaiblePosition(center, 0);
        }

        private Vector3 GetAvaiblePosition(Vector3 center, int radius)
        {

            if (radius == 0)
            {
                center.y = Island.GetRealHeight(center.z, center.x);
                if (!TestRay(center)) return center;
                return GetAvaiblePosition(center, 1);
            }

            Vector3 pos;
            float z, x;

            // x x x
            // - - -
            // - - -
            for (x = (int)center.x - radius; x <= (int)center.x + radius; x++)
            {
                z = center.z + radius;
                pos = new Vector3(x, Island.GetRealHeight(z, x), z);
                if (!TestRay(pos)) return pos;
            }
            // - - -
            // - - x
            // - - -
            for (z = (int)center.z - radius + 1; z <= (int)center.z + radius - 1; z++)
            {
                x = center.x + radius;
                pos = new Vector3(x, Island.GetRealHeight(z, x), z);
                if (!TestRay(pos)) return pos;
            }
            // - - -
            // - - -
            // x x x 
            for (x = (int)center.x + radius; x >= (int)center.x - radius; x--)
            {
                z = center.z - radius;
                pos = new Vector3(x, Island.GetRealHeight(z, x), z);
                if (!TestRay(pos)) return pos;
            }
            // - - -
            // x - -
            // - - -
            for (z = (int)center.z - radius + 1; z <= (int)center.z + radius - 1; z++)
            {
                x = center.x - radius;
                pos = new Vector3(x, Island.GetRealHeight(z, x), z);
                if (!TestRay(pos)) return pos;
            }

            return GetAvaiblePosition(center, radius + 1);
        }

        private bool TestRay(Vector3 pos)
        {
            //Debug.DrawRay( pos + Vector3.up * 3, Vector3.down * 2, Color.yellow , 100);
            Ray ray = new Ray(pos + Vector3.up * 3f, Vector3.down);
            return Physics.Raycast(ray, 2.5f);
        }

        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.red;

            //Vector3 center = new Vector3(250, 0, 250);
            //float height = Island.GetRealHeight(center.z, center.x) + 1;
            //center.y = height;

            //Vector3 start0 = center;
            //Vector3 end0 = center + Vector3.left;

            //Vector3 start1 = center + Vector3.forward;
            //Vector3 end1 = center + Vector3.back;

            //start0.y = height;
            //end0.y = height;
            //start1.y = height;
            //end1.y = height;

            //Gizmos.DrawLine(start0, end0);
            //Gizmos.DrawLine(start1, end1);

        }

    }
}
