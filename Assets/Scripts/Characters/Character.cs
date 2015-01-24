using UnityEngine;

public abstract class Character
{
    protected const float baseAttributeValue = 50f;

    //Modifier Ranges
    protected const float randomMinModifier = 0.85f;
    protected const float randomMaxModifier = 1.15f;

    protected const sbyte childMinModifier = -25;
    protected const sbyte childMaxModifier = -5;
    protected const sbyte lateAgeMinModifier = -15;
    protected const sbyte lateAgeMaxModifier = -5;

    //Stats
    protected float dexterity;
    protected float endurance;
    protected float intelligence;
    protected float charisma;

    //Modifiers
    protected AgeGroup age;
    protected float training;

    protected enum AgeGroup
    {
        Child,
        Adult,
        LateAge
    }

    //Will be overriden in the derived classes
    public virtual void Initialise()
    {
        dexterity = baseAttributeValue * Random.Range(randomMinModifier, randomMaxModifier);
        endurance = baseAttributeValue * Random.Range(randomMinModifier, randomMaxModifier);
        intelligence = baseAttributeValue * Random.Range(randomMinModifier, randomMaxModifier);
        charisma = baseAttributeValue * Random.Range(randomMinModifier, randomMaxModifier);
    }

    public override string ToString()
    {
        return "Age Group = " + age + "  Dexterity = " + dexterity + "  Endurance = " + endurance + "  Intelligence = " + intelligence + "  Charisma = " + charisma;
    }

    public static Character CreateCharacter()
    {
        int random = Random.Range(0, 4); // Change to however many types of characters there are. Might be easier to pick the character type from an array.
        switch (random)
        {
            case 0:
                return new Brainiac();              
            case 1:
                //return new OtherTypes();
            case 2:
                //return new OtherTypes();
            case 3:
                //return new OtherTypes();
            default:
                return new Brainiac();
        }
    }

    public float Dexterity
    {
        get { return dexterity; }
    }

    public float Endurance
    {
        get { return endurance; }
    }

    public float Intelligence
    {
        get { return intelligence; }
    }

    public float Charisma
    {
        get { return charisma; }
    }
}