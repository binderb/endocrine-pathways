using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDictionary : MonoBehaviour {

	public List<string> cardTitles;
	public List<string> cardTexts;
	public List<string> cardTargets;
	public List<string> cardTypes;
	public List<string> cardHints;
	public List<Sprite> cardSprites;

	// Use this for initialization
	void Start () {
		buildCardDictionary();
	}

	// Update is called once per frame
	void Update () {

	}

	void buildCardDictionary () {

		// -------------------------
		// Titles
		// -------------------------

		// Hypothalamic Hormones
		cardTitles.Add("GnRH");
		cardTitles.Add("TRH");
		cardTitles.Add("GHRH");
		cardTitles.Add("CRH");

		// Pituitary Hormones
		cardTitles.Add("LH");
		cardTitles.Add("FSH");
		cardTitles.Add("TSH");
		cardTitles.Add("GH");
		cardTitles.Add("ACTH");

		// Thyroid Hormones
		cardTitles.Add("TH");
		cardTitles.Add("Calcitonin");

		// Parathyroid Hormones
		cardTitles.Add("PTH");

		// Adrenal Hormones
		cardTitles.Add("Cortisol");
		cardTitles.Add("Aldosterone");

		// Pancreatic Hormones
		cardTitles.Add("Glucagon");
		cardTitles.Add("Insulin");

		// Hepatic Hormones
		cardTitles.Add("IGF1");

		// Sex Hormones
		cardTitles.Add("Estradiol");
		cardTitles.Add("Testosterone");

		// Homeostatic Effects
		cardTitles.Add("Elevated Blood Glucose");
		cardTitles.Add("Decreased Blood Glucose");
		cardTitles.Add("Ovulation");
		cardTitles.Add("Spermatogenesis");
		cardTitles.Add("Elevated Body Temperature");
		cardTitles.Add("Release of Free Fatty Acids");
		cardTitles.Add("Increased Water Retention");
		cardTitles.Add("Elevated Blood Calcium");
		cardTitles.Add("Decreased Blood Calcium");


		// Posterior Pituitary Hormones
		cardTitles.Add("OT");
		cardTitles.Add("ADH");

		// More Effects
		cardTitles.Add("Uterine Contractions");

		// Neuroendocrine Stimulation
		cardTitles.Add("Neuroendocrine Stimulation");



		// -------------------------
		// Texts
		// -------------------------

		// Hypothalamic Hormones
		cardTexts.Add("Gonadotropin-releasing hormone.");
		cardTexts.Add("Thyrotropin-releasing hormone.");
		cardTexts.Add("Growth hormone-releasing hormone.");
		cardTexts.Add("Corticotropin-releasing hormone.");

		// Pituitary Hormones
		cardTexts.Add("Luteinizing hormone.");
		cardTexts.Add("Follicle-stimulating hormone.");
		cardTexts.Add("Thyroid-stimulating hormone.");
		cardTexts.Add("Growth hormone.");
		cardTexts.Add("Adrenocorticotropic hormone.");

		// Thyroid Hormones
		cardTexts.Add("Thyroid hormone is a mixture of 2 compounds; this card depicts T3. TH acts on almost every cell in the body—use it on any organ.");
		cardTexts.Add("A hormone important in calcium homeostasis.");

		// Parathyroid Hormones
		cardTexts.Add("A hormone important in calcium homeostasis.");

		// Adrenal Hormones
		cardTexts.Add("A glucocorticoid hormone. Although it exerts effects on multiple systems, only the effects on metabolism are considered here.");
		cardTexts.Add("A mineralcorticoid hormone.");

		// Pancreatic Hormones
		cardTexts.Add("A peptide hormone.");
		cardTexts.Add("A peptide hormone.");

		// Hepatic Hormones
		cardTexts.Add("Insulin-like growth factor 1.");

		// Sex Hormones
		cardTexts.Add("A sex steroid. This hormone doesn't have an organ target in this game presently, but you can use it to satisfy a goal.");
		cardTexts.Add("A sex steroid. This hormone has many targets, but only the reproductive effect is represented here. You can also use it to satisfy a goal.");

		// Homeostatic Effects
		cardTexts.Add("Glucose stored in the target tissue as glycogen is lysed apart and released into the bloodstream.");
		cardTexts.Add("Glucose has been absorbed into target tissues, lowering blood glucose levels.");
		cardTexts.Add("Luteinizing hormone has stimulated the release of an oocyte from an ovarian follicle.");
		cardTexts.Add("Follicle-stimulating hormone has stimulated the production of sperm in the testes.");
		cardTexts.Add("Increased cellular metabolism has generated additional heat, contributing to an increase in body temperature.");
		cardTexts.Add("Adipocytes have released free fatty acids into the bloodstream, for use as fuel by other cells.");
		cardTexts.Add("Kidney function has been modulated to retain more water during filtration, helping to increase blood volume and pressure.");
		cardTexts.Add("Kidney function has been modulated to retain more calcium during filtration. Osteoclast activity and/or digestive calcium uptake has also likely increased.");
		cardTexts.Add("Osteoblast activity has increased while osteoclast activity has decreased, leading to a drop in blood calcium levels as more bone matrix forms.");

		// Posterior Pituitary Hormones
		cardTexts.Add("Oxytocin.");
		cardTexts.Add("Anti-diuretic hormone, aka vasopressin.");

		// More Effects
		cardTexts.Add("Oxytocin has stimulated the smooth muscle of the uterus, preparing the reproductive tract for childbirth.");

		// Neuroendocrine Stimulation
		cardTexts.Add("Play this card on the hypothalamus to receive a hormone of your choice from the neuroendocrine cells there.");




		// -------------------------
		// Targets
		// -------------------------

		// Hypothalamic Hormones
		cardTargets.Add("pituitary");
		cardTargets.Add("pituitary");
		cardTargets.Add("pituitary");
		cardTargets.Add("pituitary");

		// Pituitary Hormones
		cardTargets.Add("ovaries,testes");
		cardTargets.Add("ovaries,testes");
		cardTargets.Add("thyroid");
		cardTargets.Add("liver,adipose,muscle,bone");
		cardTargets.Add("adrenals");

		// Thyroid Hormones
		cardTargets.Add("hypothalamus,pituitary,thyroid,parathyroid,thymus,adrenals,pancreas,liver,muscle,adipose,ovaries,testes,kidneys,pineal,bone,uterus");
		cardTargets.Add("bone");

		// Parathyroid Hormones
		cardTargets.Add("kidneys,bone");

		// Adrenal Hormones
		cardTargets.Add("liver,adipose,muscle");
		cardTargets.Add("kidneys");

		// Pancreatic Hormones
		cardTargets.Add("liver,adipose,muscle");
		cardTargets.Add("liver,adipose,muscle");

		// Hepatic Hormones
		cardTargets.Add("liver,adipose,muscle");

		// Sex Hormones
		cardTargets.Add("");
		cardTargets.Add("testes");

		// Homeostatic Effects
		cardTargets.Add("pancreas");
		cardTargets.Add("pancreas");
		cardTargets.Add("");
		cardTargets.Add("");
		cardTargets.Add("");
		cardTargets.Add("");
		cardTargets.Add("");
		cardTargets.Add("thyroid");
		cardTargets.Add("parathyroid");


		// Posterior Pituitary Hormones
		cardTargets.Add("uterus");
		cardTargets.Add("kidneys");

		// More Effects
		cardTargets.Add("");

		// Neuroendocrine Stimulation
		cardTargets.Add("hypothalamus");


		// -------------------------
		// Types
		// -------------------------

		// Hypothalamic Hormones
		cardTypes.Add("hormone_hypothalamic");
		cardTypes.Add("hormone_hypothalamic");
		cardTypes.Add("hormone_hypothalamic");
		cardTypes.Add("hormone_hypothalamic");

		// Pituitary Hormones
		cardTypes.Add("hormone_pituitary");
		cardTypes.Add("hormone_pituitary");
		cardTypes.Add("hormone_pituitary");
		cardTypes.Add("hormone_pituitary");
		cardTypes.Add("hormone_pituitary");

		// Thyroid Hormones
		cardTypes.Add("hormone_thyroid");
		cardTypes.Add("hormone_thyroid");

		// Parathyroid Hormones
		cardTypes.Add("hormone_parathyroid");

		// Adrenal Hormones
		cardTypes.Add("hormone_adrenal");
		cardTypes.Add("hormone_adrenal");

		// Pancreatic Hormones
		cardTypes.Add("hormone_pancreas");
		cardTypes.Add("hormone_pancreas");

		// Hepatic Hormones
		cardTypes.Add("hormone_hepatic");

		// Sex Hormones
		cardTypes.Add("hormone_gonadal");
		cardTypes.Add("hormone_gonadal");

		// Homeostatic Effects
		cardTypes.Add("effect");
		cardTypes.Add("effect");
		cardTypes.Add("effect");
		cardTypes.Add("effect");
		cardTypes.Add("effect");
		cardTypes.Add("effect");
		cardTypes.Add("effect");
		cardTypes.Add("effect");
		cardTypes.Add("effect");

		// Posterior Pituitary Hormones
		cardTypes.Add("hormone_pituitary");
		cardTypes.Add("hormone_pituitary");

		// More Effects
		cardTypes.Add("effect");


		// Neuroendocrine Stimulation
		cardTypes.Add("effect");


		// -------------------------
		// Hints
		// -------------------------

		// Hypothalamic Hormones
		cardHints.Add("GnRH is classified as a Releasing Hormone, which means it has a very specific (and nearby) target.");
		cardHints.Add("TRH is classified as a Releasing Hormone, which means it has a very specific (and nearby) target.");
		cardHints.Add("GHRH is classified as a Releasing Hormone, which means it has a very specific (and nearby) target.");
		cardHints.Add("CRH is classified as a Releasing Hormone, which means it has a very specific (and nearby) target.");

		// Pituitary Hormones
		cardHints.Add("LH (Luteinizing Hormone) has different effects depending on the gender of the individual, but in both cases it acts primarily as a tropic hormone, targeting the reproductive system.");
		cardHints.Add("FSH (Follicle Stimulating Hormone) has different effects depending on the gender of the individual, but in both cases it acts primarily as a tropic hormone, targeting the reproductive system.");
		cardHints.Add("TSH (Thyroid Stimulating Hormone) has its primary target in the name—keep in mind that as a tropic hormone, it is part of a longer chain of events that will eventually lead to a physiological effect.");
		cardHints.Add("GH (Growth Hormone) is the only anterior pituitary hormone covered here that is not a tropic hormone—rather than modulating another endocrine gland, it primarily acts in a direct fashion on target tissues.");
		cardHints.Add("ACTH (Adrenocorticotropic Hormone) has its primary target in the name—keep in mind that as a tropic hormone, it is part of a longer chain of events that will eventually lead to a physiological effect.");

		// Thyroid Hormones
		cardHints.Add("TH (Thyroid Hormone) acts on all tissue types, so you shouldn't be getting this error—sorry! Let your instructor know if you see this!");
		cardHints.Add("Calcitonin modulates calcium homeostasis, dampening down the levels of calcium in the blood. Where is there a pool of mineralized calcium in the body, that could work to sequester the excess in the bloodstream?");

		// Parathyroid Hormones
		cardHints.Add("PTH modulates calcium homeostasis, causing a dramatic increase in blood calcium levels. It does this via a number of mechanisms, including preventing elimination and drawing from a major available pool of mineralized calcium in the body.");

		// Adrenal Hormones
		cardHints.Add("Cortisol has a number of effects on the sympathetic nervous system that are not covered here. If the body is readying a fight-or-flight response, though, what are some things that it will need?");
		cardHints.Add("Aldosterone is important in maintaining blood pressure and volume. If you wanted to modulate how much water was in the bloodstream, what system would you target?");

		// Pancreatic Hormones
		cardHints.Add("Glucagon modulates blood glucose levels. What kinds of tissues are notable for storing large amounts of glycogen (the storage form of glucose)?");
		cardHints.Add("Insulin modulates blood glucose levels. What kinds of tissues are notable for storing large amounts of glycogen (the storage form of glucose)?");

		// Hepatic Hormones
		cardHints.Add("IGF-1 (Insulin-Like Growth Factor 1) is similar in function, in all practical ways covered here, to Growth Hormone (GH). It is secreted by the liver to prolong GH's effects, and will behave just like GH here.");

		// Sex Hormones
		cardHints.Add("Estradiol is important for reproductive development and maintenance, especially in females—but those roles are not covered here. For this exercise, estradiol is exclusively used to satisfy a particular goal.");
		cardHints.Add("Testosterone is important for reproductive development and maintenance, especially in males—but many of those functions are not covered here. It does have one role represented here—it is a synergist for Follicle Stimulating Hormone (FSH) in males.");

		// Homeostatic Effects
		cardHints.Add("Raising blood glucose levels can have indirect effects on the metabolic rates of many cells, but one organ is particularly specialized for detecting changes in blood glucose. Which is it?");
		cardHints.Add("Lowering blood glucose levels can have indirect effects on the metabolic rates of many cells, but one organ is particularly specialized for detecting changes in blood glucose. Which is it?");
		cardHints.Add("This is an effect card used to satisfy a goal—downstream effects as a result of ovulation are not represented here.");
		cardHints.Add("This is an effect card used to satisfy a goal—downstream effects as a result of spermatogenesis are not represented here.");
		cardHints.Add("This is an effect card used to satisfy a goal—downstream effects as a result of increased body temperature are not represented here.");
		cardHints.Add("This is an effect card used to satisfy a goal—downstream effects as a result of fatty acids being released into the bloodstream are not represented here.");
		cardHints.Add("This is an effect card used to satisfy a goal—downstream effects as a result of increased water retention are not represented here.");
		cardHints.Add("Raising blood calcium levels can have indirect effects especially on the behavior of excitable cells, but one organ is particularly specialized for detecting positive changes in blood calcium. Which is it?");
		cardHints.Add("Lowering blood calcium levels can have indirect effects especially on the behavior of excitable cells, but one organ is particularly specialized for detecting negative changes in blood calcium. Which is it?");


		// Posterior Pituitary Hormones
		cardHints.Add("OT (Oxytocin) plays a number of roles related to the body's various sexual responses—but it is also important in preparing the female reproductive tract for childbirth.");
		cardHints.Add("ADH (Anti-Diuretic Hormone) is important in maintaining blood pressure and volume. If you wanted to modulate how much water was in the bloodstream, what system would you target?");

		// More Effects
		cardHints.Add("This is an effect card used to satisfy a goal—downstream effects as a result of uterine contractions are not represented here.");

		// Neuroendocrine Stimulation
		cardHints.Add("While it's true that there are several other tissues (e.g. the thyroid, adrenal medulla) that use neuroendocrine cells to bridge the nervous and endocrine systems, this card is meant for the hypothalamus specifically.");

	}


}
