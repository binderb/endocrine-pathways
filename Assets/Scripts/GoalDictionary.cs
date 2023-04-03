using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDictionary : MonoBehaviour {

	public List<string> goalTitles;
	public List<string> goalTexts;
	public List<int> goalSpriteIndices;
	public List<string> goalSolutions;
	public List<Sprite> goalSprites;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void buildGoalDictionary () {

		// -------------------------
		// Titles
		// -------------------------

		goalTitles.Add("Raise Body Temperature");
		goalTitles.Add("Raise Blood Glucose");
		goalTitles.Add("Lower Blood Glucose");
		goalTitles.Add("Secrete Estradiol");
		goalTitles.Add("Secrete Testosterone");
		goalTitles.Add("Stimulate Ovulation");
		goalTitles.Add("Stimulate Spermatogenesis");
		goalTitles.Add("Release Free Fatty Acids");
		goalTitles.Add("Increase Water Retention");
		goalTitles.Add("Raise Blood Calcium");
		goalTitles.Add("Lower Blood Calcium");
		goalTitles.Add("Stimulate Uterine Contractions");




		// -------------------------
		// Texts
		// -------------------------

		goalTexts.Add("Find a way to obtain this effect, and place the effect card here to complete this goal.");
		goalTexts.Add("Find a way to obtain this effect, and place the effect card here to complete this goal.");
		goalTexts.Add("Find a way to obtain this effect, and place the effect card here to complete this goal.");
		goalTexts.Add("Find a way to obtain this hormone, and place the hormone card here to complete this goal.");
		goalTexts.Add("Find a way to obtain this hormone, and place the hormone card here to complete this goal.");
		goalTexts.Add("Find a way to obtain this effect, and place the effect card here to complete this goal.");
		goalTexts.Add("Find a way to obtain this effect, and place the effect card here to complete this goal.");
		goalTexts.Add("Find a way to obtain this effect, and place the effect card here to complete this goal.");
		goalTexts.Add("Find a way to obtain this effect, and place the effect card here to complete this goal.");
		goalTexts.Add("Find a way to obtain this effect, and place the effect card here to complete this goal.");
		goalTexts.Add("Find a way to obtain this effect, and place the effect card here to complete this goal.");
		goalTexts.Add("Find a way to obtain this effect, and place the effect card here to complete this goal.");


		// -------------------------
		// Sprite Indices
		// -------------------------

		goalSpriteIndices.Add(0);
		goalSpriteIndices.Add(3);
		goalSpriteIndices.Add(3);
		goalSpriteIndices.Add(4);
		goalSpriteIndices.Add(5);
		goalSpriteIndices.Add(1);
		goalSpriteIndices.Add(2);
		goalSpriteIndices.Add(6);
		goalSpriteIndices.Add(7);
		goalSpriteIndices.Add(8);
		goalSpriteIndices.Add(8);
		goalSpriteIndices.Add(9);

		// -------------------------
		// Solutions
		// -------------------------

		goalSolutions.Add("Elevated Body Temperature");
		goalSolutions.Add("Elevated Blood Glucose");
		goalSolutions.Add("Decreased Blood Glucose");
		goalSolutions.Add("Estradiol");
		goalSolutions.Add("Testosterone");
		goalSolutions.Add("Ovulation");
		goalSolutions.Add("Spermatogenesis");
		goalSolutions.Add("Release of Free Fatty Acids");
		goalSolutions.Add("Increased Water Retention");
		goalSolutions.Add("Elevated Blood Calcium");
		goalSolutions.Add("Decreased Blood Calcium");
		goalSolutions.Add("Uterine Contractions");

	}

}
