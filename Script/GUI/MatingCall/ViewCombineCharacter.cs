using UnityEngine;
using System.Collections;
using Engine.Characters.Entity;
using System.Collections.Generic;
using Assets.Script;
using Script.GUI.MatingCall;
using Assets.Engine.AI.GeneticAlgorithm;
using Engine.Characters.Entity.Characters;
using System.Linq;


public class ViewCombineCharacter : MonoBehaviour
{
    #region CONSTANT
		
    private const string PATH_CHARACTER = "GUI/MatingCall/Character";
    private const string NAME_CHARACTER_ = "character_";
    private const string NAME_TEXTURE = "Texture";
    private const string NAME_GRD_MALE =   "BackgroundCharactersMale/ScrollViewCharactersMale/GridCharactersMale";
    private const string NAME_GRD_FEMALE = "BackgroundCharactersFemale/ScrollViewCharactersFemale/GridCharactersFemale";
    private const string NAME_TEXTURE_MALE =    "GUI/icons/icon-character";
    private const string NAME_TEXTURE_FEMALE =  "GUI/icons/icon-characterfemale";

    #endregion

    #region INSPECTOR
    
    
    public GameObject SurfaceMale;
    public GameObject SurfaceFemale;
    public GameObject SurfaceSelectedItem_Male;
    public GameObject SurfaceSelectedItem_Female;
    public GameObject SliderUpper;
    public GameObject SliderLower;
    public GameObject LabelUpper;
    public GameObject LabelLower;
    public GameObject SliderMixingRatio;
    public GameObject SliderMutationPercentage;
    public GameObject LabelPercentageMutation;
    public GameObject ButtonQuit;
    public GameObject ButtonMating;
    public GameObject TopMenu;

    #endregion

    #region FIELD

    private static UIButton CmdButtonMating;
    private static ViewCombineCharacter controller;

    private static string nameSurfaceMale;
    private static string nameSurfaceFemale;
    private static string nameSurfaceSelectedItem_Male;
    private static string nameSurfaceSelectedItem_Female;

    private static GameObject selectedItem_A;
    private static GameObject selectedItem_B;
    private static GameObject thisMenu;

    private int contCharacter = 0;
    private int NumberMale = 0;
    private int NumberFemale = 0;

    private GeneticVariables geneticVariable = new GeneticVariables();
        
    #endregion FIELD

    #region PROPERTY

    public UIGrid GridCharacterMale 
    {
        get
        {
            if (_gridCharacterMale != null) return _gridCharacterMale;
            Transform tmp = transform.Find(NAME_GRD_MALE);
            _gridCharacterMale = tmp.GetComponent<UIGrid>();
            return _gridCharacterMale;
        }
    }
    private UIGrid _gridCharacterMale = null;

    public UIGrid GridCharacterFemale
    {
        get
        {
            if (_gridCharacterFemale != null) return _gridCharacterFemale;
            _gridCharacterFemale = transform.Find(NAME_GRD_FEMALE).GetComponent<UIGrid>();
            return _gridCharacterFemale;
        }
    }
    private UIGrid _gridCharacterFemale = null;

    public GameObject CharacterPrefab
    { 
        get
        {
            if ( _characterPrefab != null ) return _characterPrefab;
            _characterPrefab = Resources.Load<GameObject>(ViewCombineCharacter.PATH_CHARACTER);
            return _characterPrefab;
        }
    }
    private GameObject _characterPrefab;

    public Texture TextureMale 
    { 
        get
        {
            if (_textureMale != null) return _textureMale;
            _textureMale = Resources.Load(NAME_TEXTURE_MALE) as Texture;
            return _textureMale;
        }
    }
    private Texture _textureMale;

    public Texture TextureFemale
    {
        get
        {
            if (_textureFemale != null) return _textureFemale;
            _textureFemale = Resources.Load(NAME_TEXTURE_FEMALE) as Texture;
            return _textureFemale;
        }
    }
    private Texture _textureFemale;

    private LinkedList<GameObject> MemList 
    { 
        get { return _memList; }
    }
    private LinkedList<GameObject> _memList = new LinkedList<GameObject>();

    #endregion

    #region EVENT
    
    // GUI

    private void OnLowerChange()
    {
        float std = -2;
        float diff = SliderLower.GetComponent<UISlider>().value - 0.50f;
        float value = diff / 0.25f + std;
        LabelLower.GetComponent<UILabel>().text = value.ToString();
        geneticVariable.MinMutation = (int)value;
    }

    private void OnUpperChange()
    {
        float std = 4;
        float diff = SliderUpper.GetComponent<UISlider>().value - 0.50f;
        float value = diff / 0.25f + std;
        LabelUpper.GetComponent<UILabel>().text = "+" + value;
        geneticVariable.MaxMutation = (int)value;
    }

    private void MixingRatio_Change()
    {
        geneticVariable.MixingRatio = SliderMixingRatio.GetComponent<UISlider>().value;
    }

    private void PercentageMutation_Change()
    {
        float start = 10;
        float unit = SliderMutationPercentage.GetComponent<UISlider>().value / 0.25f;
        float value = unit * 10 + start;
        LabelPercentageMutation.GetComponent<UILabel>().text = value + "%";
        geneticVariable.PercentageMutation = value / 100;
    }
 
    private void ButtonMating_OnClick()
    {
        GameObject grid_male = SurfaceSelectedItem_Male.transform.GetChild(0).gameObject;
        GameObject grid_female = SurfaceSelectedItem_Female.transform.GetChild(0).gameObject;

        GameObject selection_male = grid_male.transform.GetChild(0).gameObject;
        GameObject selection_female = grid_female.transform.GetChild(0).gameObject;

        Character ch_male = selection_male.GetComponent<ViewCharacter>().Character;
        Character ch_female = selection_female.GetComponent<ViewCharacter>().Character;

        Character newCharacter = Genetic.Couple(ch_male, ch_female, geneticVariable);

        //PeopleController peopleController = PeopleController.GetComponent<PeopleController>();
        //peopleController.AddCharacter(newCharacter);

        Debug.Log("New max: " + CharacterManager.MaxSkillToString() );
        Debug.Log("New min: " + CharacterManager.MinSkillToString());

        AddRectCharacters(newCharacter, true);
    }

    private void ButtonQuit_OnClick()
    {
        GuiController.CloseActiveWindow();
    }

    // TOP MENU

    private void ButtonAVG_Male_OnClick()
    {
        var list = from p in GridCharacterMale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>().Character.Skills.GetAVGSkills() descending
                   select p;
        Reorder(list, GridCharacterMale.transform);
    }

    private void Button_Int_Male_OnClick()
    {
        var list = from p in GridCharacterMale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>().Character.Skills.GetValue(SkillsEnum.Intelligence) descending
                   select p;
        Reorder(list, GridCharacterMale.transform);
    }

    private void Button_Res_Male_OnClick()
    {
        var list = from p in GridCharacterMale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>()
                            .Character.Skills.GetValue(SkillsEnum.Resistance) descending
                   select p;
        Reorder(list, GridCharacterMale.transform);
    }

    private void Button_Str_Male_OnClick()
    {
        var list = from p in GridCharacterMale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>()
                            .Character.Skills.GetValue(SkillsEnum.Strenght) descending
                   select p;
        Reorder(list, GridCharacterMale.transform);
    }

    private void Button_Agi_Male_OnClick()
    {
        var list = from p in GridCharacterMale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>()
                            .Character.Skills.GetValue(SkillsEnum.Agility) descending
                   select p;
        Reorder(list, GridCharacterMale.transform);
    }

    private void Button_Cha_Male_OnClick()
    {
        var list = from p in GridCharacterMale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>()
                            .Character.Skills.GetValue(SkillsEnum.Charm) descending
                   select p;
        Reorder(list, GridCharacterMale.transform);
    }

    private void Button_App_Male_OnClick()
    {
        var list = from p in GridCharacterMale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>()
                            .Character.Skills.GetValue(SkillsEnum.Appeal) descending
                   select p;
        Reorder(list, GridCharacterMale.transform);
    }

    private void ButtonAVG_Female_OnClick()
    {
        var list = from p in GridCharacterFemale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>().Character.Skills.GetAVGSkills() descending
                   select p;
        Reorder(list, GridCharacterFemale.transform);
    }

    private void Button_Int_Female_OnClick()
    {
        var list = from p in GridCharacterFemale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>()
                            .Character.Skills.GetValue(SkillsEnum.Intelligence) descending
                   select p;
        Reorder(list, GridCharacterFemale.transform);
    }

    private void Button_Res_Female_OnClick()
    {
        var list = from p in GridCharacterFemale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>()
                            .Character.Skills.GetValue(SkillsEnum.Resistance) descending
                   select p;
        Reorder(list, GridCharacterFemale.transform);
    }

    private void Button_Str_Female_OnClick()
    {
        var list = from p in GridCharacterFemale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>()
                            .Character.Skills.GetValue(SkillsEnum.Strenght) descending
                   select p;
        Reorder(list, GridCharacterFemale.transform);
    }

    private void Button_Agi_Female_OnClick()
    {
        var list = from p in GridCharacterFemale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>()
                            .Character.Skills.GetValue(SkillsEnum.Agility) descending
                   select p;
        Reorder(list, GridCharacterFemale.transform);
    }

    private void Button_Cha_Female_OnClick()
    {
        var list = from p in GridCharacterFemale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>()
                            .Character.Skills.GetValue(SkillsEnum.Charm) descending
                   select p;
        Reorder(list, GridCharacterFemale.transform);
    }

    private void Button_App_Female_OnClick()
    {
        var list = from p in GridCharacterFemale.GetChildList()
                   orderby p.GetComponent<ViewCharacter>()
                            .Character.Skills.GetValue(SkillsEnum.Appeal) descending
                   select p;
        Reorder(list, GridCharacterFemale.transform);
    }

    #endregion

    internal ViewCombineCharacter()  { }
        
    void Start()
    {
        Init();
        AddEvent();
    }

    private void AddEvent()
    {
        EventDelegate.Add(SliderLower.GetComponent<UISlider>().onChange, OnLowerChange);
        EventDelegate.Add(SliderUpper.GetComponent<UISlider>().onChange, OnUpperChange);
        EventDelegate.Add(SliderMixingRatio.GetComponent<UISlider>().onChange, MixingRatio_Change);
        EventDelegate.Add(SliderMutationPercentage.GetComponent<UISlider>().onChange, PercentageMutation_Change);

        EventDelegate.Add(ButtonMating.GetComponent<UIButton>().onClick, ButtonMating_OnClick);
        EventDelegate.Add(ButtonQuit.GetComponent<UIButton>().onClick, ButtonQuit_OnClick);
        
        EventDelegate.Add( GetUIButtonByName("CmdOrderAVGMale").onClick, ButtonAVG_Male_OnClick);
        EventDelegate.Add(GetUIButtonByName("CmdOrderAVGFemale").onClick, ButtonAVG_Female_OnClick);

        foreach (GenderEnum gender in GenderEnum.GetValues(typeof(GenderEnum)))
        {
            foreach (SkillsEnum skill in SkillsEnum.GetValues(typeof(SkillsEnum)))
            {
                UIButton button = GetUIButton(gender, skill);
                if ( gender == GenderEnum.Male)
                {
                    switch (skill)
                    {
                        case SkillsEnum.Intelligence:
                            EventDelegate.Add(button.GetComponent<UIButton>().onClick, Button_Int_Male_OnClick); continue;
                        case SkillsEnum.Resistance:
                            EventDelegate.Add(button.GetComponent<UIButton>().onClick, Button_Res_Male_OnClick); continue;
                        case SkillsEnum.Strenght:
                            EventDelegate.Add(button.GetComponent<UIButton>().onClick, Button_Str_Male_OnClick); continue;
                        case SkillsEnum.Agility:
                            EventDelegate.Add(button.GetComponent<UIButton>().onClick, Button_Agi_Male_OnClick); continue;
                        case SkillsEnum.Charm:
                            EventDelegate.Add(button.GetComponent<UIButton>().onClick, Button_Cha_Male_OnClick); continue;
                        case SkillsEnum.Appeal:
                            EventDelegate.Add(button.GetComponent<UIButton>().onClick, Button_App_Male_OnClick); continue;
                    }
                }
                else
                {
                    switch (skill)
                    {
                        case SkillsEnum.Intelligence:
                            EventDelegate.Add(button.GetComponent<UIButton>().onClick, Button_Int_Female_OnClick); continue;
                        case SkillsEnum.Resistance:
                            EventDelegate.Add(button.GetComponent<UIButton>().onClick, Button_Res_Female_OnClick); continue;
                        case SkillsEnum.Strenght:
                            EventDelegate.Add(button.GetComponent<UIButton>().onClick, Button_Str_Female_OnClick); continue;
                        case SkillsEnum.Agility:
                            EventDelegate.Add(button.GetComponent<UIButton>().onClick, Button_Agi_Female_OnClick); continue;
                        case SkillsEnum.Charm:
                            EventDelegate.Add(button.GetComponent<UIButton>().onClick, Button_Cha_Female_OnClick); continue;
                        case SkillsEnum.Appeal:
                            EventDelegate.Add(button.GetComponent<UIButton>().onClick, Button_App_Female_OnClick); continue;
                    }
                }

            }
        }
    }

    private UIButton GetUIButton(GenderEnum gender, SkillsEnum skill)
    {
        string name = "CmdOrder";
        switch (skill)
        {
            case SkillsEnum.Intelligence:
                name += "Int"; break;
            case SkillsEnum.Resistance:
                name += "Res"; break;
            case SkillsEnum.Strenght:
                name += "Str"; break;
            case SkillsEnum.Agility:
                name += "Agi"; break;
            case SkillsEnum.Charm:
                name += "Cha"; break;
            case SkillsEnum.Appeal:
                name += "App"; break;
        }
        
        if (gender == GenderEnum.Male) 
            name += "Male";
        else
            name += "Female";

        return GetUIButtonByName(name);
    }

    private UIButton GetUIButtonByName( string name )
    {
        return TopMenu.transform.FindChild(name).GetComponent<UIButton>();
    }

    private void Init()
    {
        nameSurfaceMale = SurfaceMale.name;
        nameSurfaceFemale = SurfaceFemale.name;
        nameSurfaceSelectedItem_Male = SurfaceSelectedItem_Male.name;
        nameSurfaceSelectedItem_Female = SurfaceSelectedItem_Female.name;

        CmdButtonMating = ButtonMating.GetComponent<UIButton>();
        CmdButtonMating.isEnabled = false;

        Destroy(selectedItem_A);
        Destroy(selectedItem_B);

        controller = new ViewCombineCharacter();
    }

    public void LoadCharacters()
    {
        Debug.Log("Max skill before init: " + CharacterManager.MaxSkillToString());

        foreach (GameObject item in MemList)
        {
            Destroy(item);
        }

        foreach (Character item in CharacterManager.ListCharacter)
        {
            AddRectCharacters(item, false);
        }

        Debug.Log("Max skill after init: " + CharacterManager.MaxSkillToString());

    }

    public void AddRectCharacters(Character c, bool append)
    {
        GameObject newGameObject = NewItem(contCharacter, c.Gender);
        newGameObject.AddComponent<ViewCharacter>().Character = c;

        SetTexure(newGameObject, c.Gender);
        SetSkills(newGameObject, c.Skills);
            
        contCharacter++;

        AddCharacterList(newGameObject, c.Gender);

        if ( append )
        {
                
            GridCharacterMale.repositionNow = true;
            GridCharacterFemale.repositionNow = true;

            foreach (GameObject item in MemList)
            {
                ViewCharacter view = item.GetComponent<ViewCharacter>();
                Character c_view = view.Character;
                
                SetSkills(item, c_view.Skills);

                Debug.Log(c_view.Skills + " sumskill: " + CharacterManager.sumSkill[SkillsEnum.Intelligence] +
                                          " minskill: " + CharacterManager.minSkill[SkillsEnum.Intelligence] +
                                          " maxskill: " + CharacterManager.maxSkill[SkillsEnum.Intelligence] +
                                          " avgskill: " + c_view.Skills.ToPercentage(SkillsEnum.Intelligence));
            }

            SetSkills(selectedItem_A);
            SetSkills(selectedItem_B);
        }
    }

    private void AddCharacterList(GameObject c, GenderEnum gender)
    {
        MemList.AddLast(c);
        if (gender == GenderEnum.Male)   NumberMale++;
        if (gender == GenderEnum.Female) NumberFemale++;
    }

    void Update()
    {
        //float contItems = GridCharacters.GetChildList().Count;
        //GridCharacters.maxPerLine = (int)(contItems / 2) + 1;
    }
        
    public static bool Elaborate(GameObject surface, GameObject item, bool cloneOnDrag)
    {
        bool error = controller.CheckErrorInstance(surface, item, cloneOnDrag);
        TryEnableButton();
        return error;
    }

    private bool CheckErrorInstance(GameObject surface, GameObject item , bool cloneOnDrag)
    {
        TypeContainer typeContainer = GetContainer(surface);

        bool error = CheckDragDropOperation(typeContainer, surface , item, cloneOnDrag);

        if (error) return true;

        return false;

    }

    private TypeContainer GetContainer(GameObject surface)
    {
        if ( surface.name == nameSurfaceSelectedItem_Male )
            return TypeContainer.SelectedMale;
        if (surface.name == nameSurfaceSelectedItem_Female )
            return TypeContainer.SelectedFemale;
        if (surface.name == nameSurfaceMale )
            return TypeContainer.MainMale;
        if (surface.name == nameSurfaceFemale)
            return TypeContainer.MainFemale;

        return TypeContainer.Other;
    }

    #region CHECK CONTAINER

    private bool CheckDragDropOperation(TypeContainer typeContainer, GameObject surface,
                                        GameObject item, bool cloneOnDrag)
    {
        switch (typeContainer)
        {
            case TypeContainer.Other:
                return CheckOtherContainer(item, surface, cloneOnDrag);

            case TypeContainer.MainMale:
            case TypeContainer.MainFemale:
                return CheckMainContainer(item, cloneOnDrag);

            default:
                return CheckSelectionContainer(typeContainer, item, cloneOnDrag);
        }
    }

    private bool CheckOtherContainer(GameObject item,
                                        GameObject surface, bool cloneOnDrag)
    {

        if (!cloneOnDrag)
        {
            if (surface.name.StartsWith("character") == false) return false;

            Result resSurface = GetItem(surface);

            if (resSurface == null)
            {
                Debug.Log("find is null");
                return false;
            }

            Result resItem = GetItem(item);

            if (resItem == null)
            {
                return true;
            }
            else
            {
                DeleteItem(item, cloneOnDrag);
                return true;
            }
        }
        else
        {
            Result mem = GetItem(item);
            if ( mem != null)
            {
                DeleteItem(mem.item, cloneOnDrag);
                return true;
            }
            else
            {
                return true;
            }
        }
    }

    private bool CheckSelectionContainer(TypeContainer typeContainer, GameObject item, bool cloneOnDrag)
    {
        GameObject memItem = GetItem(typeContainer);

        if (!cloneOnDrag)
        {
            if (memItem == null)
            {
                DeleteItem(item, cloneOnDrag);   //if is in another container
                SetItem(item, typeContainer);
                return false;
            }else
            {
                return true;
            }
        }
        else
        {
            if ( memItem == null )
            {
                Result resMem = GetItem(item);
                    
                //destroy other object
                if ( resMem != null )
                {
                    DeleteItem(resMem.item, cloneOnDrag);
                }
                    
                //set new object
                SetItem(item, typeContainer);

                return false;
            }
            else
            {
                DeleteItem(memItem, cloneOnDrag);
                return true;
            }
        }
    }

        

    private bool CheckMainContainer(GameObject item, bool cloneOnDrag)
    {
        if (cloneOnDrag)
        {
            Result mem = GetItem(item);
            if ( mem != null)
            {
                DeleteItem(mem.item, cloneOnDrag);
            }
            return true;
        }
        else
        {
            DeleteItem(item, cloneOnDrag);
            return false;
        }
    }

    #endregion CHECK CONTAINER

    #region SET/GET ITEM

    private void DeleteItem(GameObject item, bool cloneOnDrag)
    {
        if (selectedItem_A != null && selectedItem_A == item)
        {
            selectedItem_A = null;
            if (cloneOnDrag)
            {
                Destroy(selectedItem_A);
                Destroy(item);
            }
        }
        if (selectedItem_B != null && selectedItem_B == item)
        {
            if (cloneOnDrag)
            {
                Destroy(selectedItem_B);
                Destroy(item);
            }
            selectedItem_B = null;
        }
    }

    private void SetItem(GameObject item, TypeContainer typeContainer)
    {
        switch (typeContainer)
        {
            case TypeContainer.SelectedMale:
                selectedItem_A = item;
                break;
            case TypeContainer.SelectedFemale:
                selectedItem_B = item;
                break;
            default:
                break;
        }
    }

    private GameObject GetItem(TypeContainer typeContainer)
    {
        switch(typeContainer)
        {
            case TypeContainer.SelectedMale: return selectedItem_A;
            case TypeContainer.SelectedFemale: return selectedItem_B;
            default: return null;
        }

    }

    private Result GetItem(GameObject item)
    {
        if (selectedItem_A != null && selectedItem_A.name == item.name)
            return new Result (selectedItem_A , TypeContainer.SelectedMale  );
        if (selectedItem_B != null && selectedItem_B.name == item.name)
            return new Result(selectedItem_B, TypeContainer.SelectedFemale);
        return null;
    }

    #endregion SET/GET ITEM

    private static void TryEnableButton()
    {
        if ( selectedItem_A != null && selectedItem_B != null)
        {
            CmdButtonMating.isEnabled = true;
        }
        else
        {
            CmdButtonMating.isEnabled = false;
        }
    }

    private void SetSkills(GameObject gameobject_container)
    {
        Character c = gameobject_container.GetComponent<ViewCharacter>().Character;
        SetSkills(gameobject_container, c.Skills);
    }

    private void SetSkills(GameObject gameobject, Skills skills)
    {
        UISlider slider;
        string pathGrid;

        SkillsEnum skill;

        pathGrid = "GridSkills/SkillInt/Progress";
        skill = SkillsEnum.Intelligence;
        slider = gameobject.transform.Find(pathGrid).GetComponent<UISlider>();
        slider.value = skills.ToPercentage(skill);
        gameobject.transform.Find(pathGrid + "/SkillValue").GetComponent<UILabel>().text = skills.GetValue(skill).ToString();

        pathGrid = "GridSkills/SkillRes/Progress";
        skill = SkillsEnum.Resistance;
        slider = gameobject.transform.Find(pathGrid).GetComponent<UISlider>();
        slider.value = skills.ToPercentage(skill);
        gameobject.transform.Find(pathGrid + "/SkillValue").GetComponent<UILabel>().text = skills.GetValue(skill).ToString();

        pathGrid = "GridSkills/SkillStr/Progress";
        skill = SkillsEnum.Strenght;
        slider = gameobject.transform.Find(pathGrid).GetComponent<UISlider>();
        slider.value = skills.ToPercentage(skill);
        gameobject.transform.Find(pathGrid + "/SkillValue").GetComponent<UILabel>().text = skills.GetValue(skill).ToString();

        pathGrid = "GridSkills/SkillAgl/Progress";
        skill = SkillsEnum.Agility;
        slider = gameobject.transform.Find(pathGrid).GetComponent<UISlider>();
        slider.value = skills.ToPercentage(skill);
        gameobject.transform.Find(pathGrid + "/SkillValue").GetComponent<UILabel>().text = skills.GetValue(skill).ToString();
            
        pathGrid = "GridSkills/SkillCha/Progress";
        skill = SkillsEnum.Charm;
        slider = gameobject.transform.Find(pathGrid).GetComponent<UISlider>();
        slider.value = skills.ToPercentage(skill);
        gameobject.transform.Find(pathGrid + "/SkillValue").GetComponent<UILabel>().text = skills.GetValue(skill).ToString();

        pathGrid = "GridSkills/SkillApp/Progress";
        skill = SkillsEnum.Appeal;
        slider = gameobject.transform.Find(pathGrid).GetComponent<UISlider>();
        slider.value = skills.ToPercentage(skill);
        gameobject.transform.Find(pathGrid + "/SkillValue").GetComponent<UILabel>().text = skills.GetValue(skill).ToString();

        //AVG
        pathGrid = "SkilssProgress";
        GameObject game = gameobject.transform.Find(pathGrid).gameObject;
        slider = gameobject.transform.Find(pathGrid).GetComponent<UISlider>();
        slider.value = skills.GetAVGSkills();
    }

    private void SetTexure(GameObject gameObject, GenderEnum gender)
    {
        UITexture texture = gameObject.transform.Find(NAME_TEXTURE).GetComponent<UITexture>();
        switch (gender)
        {
            case GenderEnum.Male:
                texture.mainTexture = TextureMale;
                break;
            case GenderEnum.Female:
                texture.mainTexture = TextureFemale;
                break;
        }
    }

    private GameObject NewItem(int cont, GenderEnum gender)
    {
        Vector3 pos;
        if (gender == GenderEnum.Male)
            pos = GetPosition(NumberMale);
        else
            pos = GetPosition(NumberFemale);

        GameObject character = Instantiate(CharacterPrefab) as GameObject;
        character.name = NAME_CHARACTER_ + cont;
        character.transform.parent = GetGridGender(gender).transform;
        character.transform.localPosition = pos;
        character.transform.localScale = new Vector3(1, 1, 1);
        return character;
    }

    private Vector3 GetPosition( int cont )
    {
        float height = CharacterPrefab.GetComponent<UISprite>().height;
        float pos = 0;
        pos = (height + 1) * cont;
        return new Vector3(0, -pos, 0);
    }

    private UIGrid GetGridGender(GenderEnum gender)
    {
        switch (gender)
        {
            case GenderEnum.Male:
                return GridCharacterMale;
            case GenderEnum.Female:
                return GridCharacterFemale;
        }
        return null;
    }

    private void Reorder(IOrderedEnumerable<Transform> list, Transform parent)
    {
        int cont = 0;
        foreach (Transform item in list)
        {
            item.localPosition = GetPosition(cont);
            item.parent = null;
            cont++;
        }

        foreach (Transform item in list)
        {
            item.parent = parent;
        }
    }
}

internal class Result
{
    internal Result(GameObject item, TypeContainer typeContainer)
    {
        this.item = item;
        this.typeContainer = typeContainer;
    }

    internal GameObject item;
    internal TypeContainer typeContainer;
}

//switch (typeContainer)
//{
//    case TypeContainer.Other:

//        Debug.Log("container other");

//        if (item_A == item)
//        {
//            item_A = null;
//        }
//        if (item_B == item)
//        {
//            item_B = null;
//        }
//        return false;
//    case TypeContainer.A:

//        if (item_A == item)
//        {
//            Debug.Log("item A is item");
//            return false;   // A -> A = NOTHING
//        }

//        if (item_B == item)                 // B -> A = ERROR
//        {
//            Debug.Log("item B is item");
//            item_B = null;
//            return true;
//        }

//        if (item_A == null)                 // A empty = ok
//        {
//            Debug.Log("item A is null");
//            item_A = item;
//            return false;
//        }
//        else
//        {
//            Debug.Log("item A is not null");
//            return true;                    // A != null
//        }


//    case TypeContainer.B:

//        if (item_B == item) return false;

//        if (item_A == item)                 // A -> B = ERROR
//        {
//            item_A = null;
//            return true;
//        }

//        if (item_B == null)                 // B is EMPTY
//        {
//            item_B = item;
//            return false;
//        }
//        else
//        {
//            return true;                    // B != null
//        }
//}


